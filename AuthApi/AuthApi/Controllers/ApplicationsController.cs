using AuthApi.Domain.Interfaces;
using AuthApi.Helpers;
using AuthApi.Models.Application;
using AuthApi.Models.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApplicationsController : ControllerBase
    {
        private readonly IApplicationService _applicationService;

        public ApplicationsController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpGet]
        [Route("getApplications")]
        [Roles(UserRoles.Admin, UserRoles.SuperAdmin)]
        [ProducesResponseType(typeof(List<Application>), 200)]
        public async Task<IActionResult> GetApplications()
        {
            return Ok(await _applicationService.GetApplicationsAsync());
        }

        [HttpGet]
        [Route("getApplications/{guid}")]
        [Roles(UserRoles.Admin, UserRoles.SuperAdmin)]
        [ValidateApplication]
        [ProducesResponseType(typeof(List<Application>), 200)]
        public async Task<IActionResult> GetApplicationByGuid(string guid)
        {
            return Ok(await _applicationService.GetApplicationByGuid(guid, true));
        }

        [HttpPost]
        [Route("CreateApplication")]
        [Roles(UserRoles.Admin, UserRoles.SuperAdmin)]
        [ProducesResponseType(typeof(List<Application>), 200)]
        public async Task<IActionResult> CreateApplication(string applicationName)
        {
            if (ModelState.IsValid)
            {
                return Ok(await _applicationService.CreateApplicationAsync(applicationName));
            }

            return BadRequest();
        }
    }
}
