using System.Net;
using AmIAuthorised.DataAccessLayer.DTO;
using AmIAuthorised.DataAccessLayer.Entity;
using AmIAuthorised.Repository;
using AmIAuthorised.Utility;

namespace AmIAuthorised.Service
{
    public class UserService : AbstractService
    {
        private readonly UserRepository _userRepository;
        private readonly JwtToken _jwtToken;
        public UserService(UserRepository userRepository, JwtToken jwtToken)
        {
            _userRepository = userRepository;
            _jwtToken = jwtToken;
        }

        public async Task<ApiResponse<bool>> CreateUser(SignUpRequest userDto)
        {
            if (string.IsNullOrWhiteSpace(userDto.UserName) || string.IsNullOrWhiteSpace(userDto.FirstName) || string.IsNullOrWhiteSpace(userDto.LastName)
                 || string.IsNullOrWhiteSpace(userDto.Email) || string.IsNullOrWhiteSpace(userDto.Password))
                return new ApiResponse<bool>(false, false, "AuthSignInInvalid", HttpStatusCode.BadRequest);

            User? isExist = await _userRepository.GetUserByUserName(userDto.UserName);
            if (isExist != null)
                return new ApiResponse<bool>(false, false, "ErrorAlreadyExistsWith", HttpStatusCode.Conflict);

            string hashedPassword = PasswordHasher.HashPassword(userDto.Password);

            User user = new()
            {
                UserName = userDto.UserName,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Password = hashedPassword
            };

            await _userRepository.CreateUser(user);
            return new ApiResponse<bool>(true, true, "ResponseSaveSuccess", HttpStatusCode.Created);
        }

        public async Task<ApiResponse<SignInResponse>> Authenticate(SignInRequest signInReq)
        {
            User? user = await _userRepository.GetUserByUserName(signInReq.UserName);
            if (user == null)
                return new ApiResponse<SignInResponse>(null, false, "AuthSignInInvalid", HttpStatusCode.BadRequest);

            string? token = AuthenticateHelper(signInReq, user);
            if (string.IsNullOrEmpty(token))
                return new ApiResponse<SignInResponse>(null, false, "AuthSignInInvalid", HttpStatusCode.BadRequest);

            UserDTO userDto = new()
            {
                UserId = user.UserId,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email

            };

            return new ApiResponse<SignInResponse>(new SignInResponse { AccessToken = token, User = userDto }, true, "AuthSignInSuccess", HttpStatusCode.OK);
        }

        private string? AuthenticateHelper(SignInRequest signInReq, User user)
        {
            if (!PasswordHasher.VerifyPassword(signInReq.Password, user.Password))
                return null;

            return _jwtToken.GenerateJWT(user.UserId.ToString(), user.Email);
        }

        public async Task<ApiResponse<bool>> CreateRole(Role role)
        {
            if (string.IsNullOrWhiteSpace(role.RoleName))
                return new ApiResponse<bool>(false, false, "RoleNameCannotBeEmpty", HttpStatusCode.BadRequest);

            Role? isExist = await _userRepository.GetRoleByName(role.RoleName);
            if (isExist != null)
                return new ApiResponse<bool>(false, false, "ErrorAlreadyExistsWith", HttpStatusCode.Conflict);

            Role newRole = new()
            {
                RoleName = role.RoleName
            };
            await _userRepository.CreateRole(newRole);
            return new ApiResponse<bool>(true, true, "ResponseSaveSuccess", HttpStatusCode.Created);
        }

    }
}
