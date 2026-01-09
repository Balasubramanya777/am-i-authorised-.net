using AmIAuthorised.Repository;

namespace AmIAuthorised.Service
{
    public class UserService : AbstractService
    {
        private readonly UserRepository _userRepository;
        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }
    }
}
