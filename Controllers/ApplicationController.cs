using AmIAuthorised.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AmIAuthorised.Utility;

namespace AmIAuthorised.Controllers
{
    [ApiController]
    [Route("api/application")]
    [Authorize]
    public class ApplicationController : ControllerBase
    {
        private readonly ApplicationService _applicationService;

        public ApplicationController(ApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [Authorize(Policy = "APPLICATION_VIEW")]
        [HttpGet("applications/{id}")]
        public async Task<IActionResult> GetApplicationById(long id)
        {
            var response = await _applicationService.GetApplicationById(id);
            return response.ToActionResult();
        }

        [Authorize(Policy = "APPLICATION_VIEW_ALL")]
        [HttpGet("applications")]
        public async Task<IActionResult> GetApplications()
        {
            var response = await _applicationService.GetApplications();
            return response.ToActionResult();
        }

        [Authorize(Policy = "APPLICATION_CREATE")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateApplication(string applicationName)
        {
            var response = await _applicationService.CreateApplication(applicationName);
            return response.ToActionResult();
        }

        [Authorize(Policy = "APPLICATION_EDIT")]
        [HttpPost("update")]
        public async Task<IActionResult> UpdateApplicationName(long applicationId, string name)
        {
            var response = await _applicationService.UpdateApplicationName(applicationId, name);
            return response.ToActionResult();
        }

        [Authorize(Policy = "APPLICATION_SUBMIT")]
        [HttpPost("submit")]
        public async Task<IActionResult> ApplicationSubmit(long applicationId)
        {
            var response = await _applicationService.ApplicationStatusUnderReview(applicationId);
            return response.ToActionResult();
        }

        [Authorize(Policy = "APPLICATION_ACTION")]
        [HttpPost("action")]
        public async Task<IActionResult> ApplicationAction(long applicationId, int status)
        {
            var response = await _applicationService.ApplicationStatusAction(applicationId, status);
            return response.ToActionResult();
        }
    }
}
