using AmIAuthorised.DataAccessLayer.DTO;
using AmIAuthorised.Service;
using AmIAuthorised.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> UpdateApplicationName(ApplicationUpdate request)
        {
            var response = await _applicationService.UpdateApplicationName(request.ApplicationId, request.ApplicationName);
            return response.ToActionResult();
        }

        [Authorize(Policy = "APPLICATION_SUBMIT")]
        [HttpPost("{id}/submit")]
        public async Task<IActionResult> ApplicationSubmit(long id)
        {
            var response = await _applicationService.ApplicationStatusUnderReview(id);
            return response.ToActionResult();
        }

        [Authorize(Policy = "APPLICATION_CREATE")]
        [Authorize(Policy = "APPLICATION_SUBMIT")]
        [HttpPost("/submit")]
        public async Task<IActionResult> ApplicationCreateAndSubmit(string applicationName)
        {
            var response = await _applicationService.ApplicationCreateAndSubmit(applicationName);
            return response.ToActionResult();
        }

        [Authorize(Policy = "APPLICATION_ACTION")]
        [HttpPost("action")]
        public async Task<IActionResult> ApplicationAction(ApplicationAction request)
        {
            var response = await _applicationService.ApplicationStatusAction(request.ApplicationId, request.ApplicationStatus);
            return response.ToActionResult();
        }
    }
}
