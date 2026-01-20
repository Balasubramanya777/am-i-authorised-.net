using System.Net;
using AmIAuthorised.DataAccessLayer.Entity;
using AmIAuthorised.Repository;
using AmIAuthorised.Utility;

namespace AmIAuthorised.Service
{
    public class ApplicationService : AbstractService
    {
        private readonly ApplicationRepository _applicationRepository;
        private readonly CurrentUser _currentUser;

        public ApplicationService(ApplicationRepository applicationRepository, CurrentUser currentUser)
        {
            _applicationRepository = applicationRepository;
            _currentUser = currentUser;
        }

        public async Task<ApiResponse<bool>> CreateApplication(string applicationName)
        {
            if (string.IsNullOrWhiteSpace(applicationName))
                return new ApiResponse<bool>(false, false, "ApplicationNameis required", HttpStatusCode.BadRequest);

            Application application = new()
            {
                ApplicationNumber = KeyGenerator.GenerateKey(),
                ApplicationName = applicationName,
                ApplicationStatus = (int)ApplicationStatus.Draft,
                CreatedBy = long.Parse(_currentUser.UserId!)
            };
            await _applicationRepository.CreateApplication(application);
            return new ApiResponse<bool>(true, true, "Application created", HttpStatusCode.Created);
        }

        public async Task<ApiResponse<string>> UpdateApplicationName(long applicationId, string newName)
        {
            Application? application = await _applicationRepository.GetApplicationById(applicationId);
            if (application == null)
                return new ApiResponse<string>(string.Empty, false, "InvalidRequest", HttpStatusCode.BadRequest);

            application.ApplicationName = newName;
            await _applicationRepository.UpdateApplication(application);
            return new ApiResponse<string>(string.Empty, true, "Success", HttpStatusCode.OK);
        }

        public async Task<ApiResponse<Application>> GetApplicationById(long applicationId)
        {
            Application? application = await _applicationRepository.GetApplicationById(applicationId);
            if (application == null)
                return new ApiResponse<Application>(null, false, "InvalidRequest", HttpStatusCode.BadRequest);

            return new ApiResponse<Application>(application, true, "Success", HttpStatusCode.OK);
        }

        public async Task<ApiResponse<List<Application>>> GetApplications()
        {
            return new ApiResponse<List<Application>>(await _applicationRepository.GetApplications(), true, "Success", HttpStatusCode.OK);
        }

        public async Task<ApiResponse<string>> UpdateApplicationStatus(long applicationId, int status)
        {
            Application? application = await _applicationRepository.GetApplicationById(applicationId);
            if (application == null)
                return new ApiResponse<string>(string.Empty, false, "InvalidRequest", HttpStatusCode.BadRequest);

            application.ApplicationStatus = status;
            await _applicationRepository.UpdateApplication(application);
            return new ApiResponse<string>(string.Empty, true, "Success", HttpStatusCode.OK);
        }

        public async Task<ApiResponse<string>> ApplicationStatusUnderReview(long applicationId)
        {
            return await UpdateApplicationStatus(applicationId, (int)ApplicationStatus.UnderReview);
        }

        public async Task<ApiResponse<string>> ApplicationStatusAction(long applicationId, int status)
        {
            if (status != (int)ApplicationStatus.Approved && status != (int)ApplicationStatus.Rejected)
                return new ApiResponse<string>(string.Empty, false, "InvalidRequest", HttpStatusCode.BadRequest);

            return await UpdateApplicationStatus(applicationId, status);
        }

    }
}
