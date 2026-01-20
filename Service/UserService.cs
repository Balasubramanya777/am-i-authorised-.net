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

        public async Task<ApiResponse<SignInResponse>> Authenticate(SignInRequest signInReq)
        {
            UserDTO? userDto = await _userRepository.GetUserByUserName(signInReq.UserName);
            if (userDto == null)
                return new ApiResponse<SignInResponse>(null, false, "AuthSignInInvalid", HttpStatusCode.BadRequest);

            string? token = AuthenticateHelper(signInReq, userDto);
            if (string.IsNullOrEmpty(token))
                return new ApiResponse<SignInResponse>(null, false, "AuthSignInInvalid", HttpStatusCode.BadRequest);

            SignInResponse response = new()
            {
                AccessToken = token,
                UserName = userDto.UserName,
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName
            };

            return new ApiResponse<SignInResponse>(response, true, "AuthSignInSuccess", HttpStatusCode.OK);
        }

        private string? AuthenticateHelper(SignInRequest signInReq, UserDTO userDto)
        {
            if (!PasswordHasher.VerifyPassword(signInReq.Password, userDto.Password))
                return null;

            return _jwtToken.GenerateJWT(userDto);
        }

        public async Task<ApiResponse<bool>> CreateUser(SignUpRequest signUpRequest)
        {
            if (string.IsNullOrWhiteSpace(signUpRequest.UserName) || string.IsNullOrWhiteSpace(signUpRequest.FirstName) || string.IsNullOrWhiteSpace(signUpRequest.LastName)
                 || string.IsNullOrWhiteSpace(signUpRequest.Email) || string.IsNullOrWhiteSpace(signUpRequest.Password) || signUpRequest.RoleId <= default(int))
                return new ApiResponse<bool>(false, false, "AuthSignInInvalid", HttpStatusCode.BadRequest);

            UserDTO? isUserExist = await _userRepository.GetUserByUserName(signUpRequest.UserName);
            if (isUserExist != null)
                return new ApiResponse<bool>(false, false, "ErrorAlreadyExistsWith", HttpStatusCode.Conflict);

            Role? isRoleExist = await _userRepository.GetRoleById(signUpRequest.RoleId);
            if (isRoleExist == null)
                return new ApiResponse<bool>(false, false, "ErrorRoleNotFound", HttpStatusCode.BadRequest);


            string hashedPassword = PasswordHasher.HashPassword(signUpRequest.Password);

            User user = new()
            {
                UserName = signUpRequest.UserName,
                FirstName = signUpRequest.FirstName,
                LastName = signUpRequest.LastName,
                Email = signUpRequest.Email,
                Password = hashedPassword,
                Role = isRoleExist
            };

            _userRepository.Add(user);
            await _userRepository.SaveChangesAsync();
            return new ApiResponse<bool>(true, true, "ResponseSaveSuccess", HttpStatusCode.Created);
        }

        public async Task<ApiResponse<bool>> CreateRole(RoleUpsertDTO roleDTO)
        {
            if (string.IsNullOrWhiteSpace(roleDTO.RoleName) || roleDTO.PermissionIds == null || roleDTO.PermissionIds.Count <= default(int))
                return new ApiResponse<bool>(false, false, "RoleNameCannotBeEmpty", HttpStatusCode.BadRequest);

            Role? isRoleExist = await _userRepository.GetRoleByName(roleDTO.RoleName);
            if (isRoleExist != null)
                return new ApiResponse<bool>(false, false, "ErrorAlreadyExistsWith", HttpStatusCode.Conflict);

            Role role = new()
            {
                RoleName = roleDTO.RoleName
            };

            foreach (int permissionId in roleDTO.PermissionIds)
            {
                Permission? permission = await _userRepository.GetPermissionById(permissionId);
                if (permission == null)
                    return new ApiResponse<bool>(false, false, "ErrorPermissionNotFound", HttpStatusCode.BadRequest);

                RolePermission rolePermission = new()
                {
                    Role = role,
                    Permission = permission
                };

                _userRepository.Add(rolePermission);
            }

            _userRepository.Add(role);
            await _userRepository.SaveChangesAsync();
            return new ApiResponse<bool>(true, true, "ResponseSaveSuccess", HttpStatusCode.Created);
        }

        public async Task<ApiResponse<bool>> CreatePermission(PermissionDTO permissionDTO)
        {
            if (string.IsNullOrWhiteSpace(permissionDTO.Code))
                return new ApiResponse<bool>(false, false, "PermissionNameCannotBeEmpty", HttpStatusCode.BadRequest);

            Permission? isPermissionExist = await _userRepository.GetPermissionByCode(permissionDTO.Code);
            if (isPermissionExist != null)
                return new ApiResponse<bool>(false, false, "ErrorAlreadyExistsWith", HttpStatusCode.Conflict);

            Permission permission = new()
            {
                Code = permissionDTO.Code,
                Description = permissionDTO.Description!
            };

            _userRepository.Add(permission);
            await _userRepository.SaveChangesAsync();
            return new ApiResponse<bool>(true, true, "ResponseSaveSuccess", HttpStatusCode.Created);
        }

        public async Task<ApiResponse<List<UserDTO>>> GetUsers()
        {
            List<UserDTO> userDtos = [];
            List<User> users = await _userRepository.GetUsers();

            foreach (User user in users)
            {
                userDtos.Add(new()
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email
                });
            }
            return new ApiResponse<List<UserDTO>>(userDtos, true, "RetriveSuccess", HttpStatusCode.OK);
        }

        public async Task<ApiResponse<List<Role>>> GetRoles()
        {
            return new ApiResponse<List<Role>>(await _userRepository.GetRoles(), true, "RetriveSuccess", HttpStatusCode.OK);
        }

    }
}
