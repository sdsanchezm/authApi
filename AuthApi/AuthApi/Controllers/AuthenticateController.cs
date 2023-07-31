using AuthApi.Domain.Interfaces;
using AuthApi.Helpers;
using AuthApi.Models.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;


namespace AuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IApplicationService _applicationService;

        public AuthenticateController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IApplicationService applicationService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _applicationService = applicationService;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        [ValidateApplication]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            await ChekUserRoles();
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        [ValidateApplication]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            await ChekUserRoles();

            var appGuid = ControllerContext.HttpContext.Request.Headers["appId"].ToString();

            //var userExists = await _userManager.FindByNameAsync(model.Username);
            //if (userExists != null)
            //    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            if (await _applicationService.ValidateUserApplication(appGuid, model.Username, model.Email))
            {
                return BadRequest(new Response { Status = "User", Message = "User already exist for this application" });
            }

            var app = await _applicationService.GetApplicationByGuid(appGuid, true);

            if (app == null)
            {
                return BadRequest(new Response { Status = "Application", Message = "Selected application doesn't exist" });
            }

            User user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                Approved = false,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            await _userManager.AddToRoleAsync(user, UserRoles.Basic);

            await _applicationService.AsociateUserApplication(user.Id, app.Id);

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [ValidateApplication]
        [AllowAnonymous]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterAdminModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (model.SuperSecretKey != _configuration.GetValue<string>("SuperSecretKey"))
            {
                return Unauthorized();
            }

            await ChekUserRoles();

            var app = await _applicationService.GetApplicationsByApplicationName("AdminApp", true);

            if (app.Count() == 0)
            {
                return BadRequest(new Response { Status = "Application", Message = "Selected application doesn't exist" });
            }

            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                var t = await _applicationService.ValidateUserApplication(app.FirstOrDefault().AppGuid, model.Username, model.Email);

                if (t)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });
                }

                await _applicationService.AsociateUserApplication(userExists.Id, app.FirstOrDefault().Id);
            }

            User user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                Approved = false,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            await _userManager.AddToRoleAsync(user, UserRoles.Admin);

            await _applicationService.AsociateUserApplication(user.Id, app.FirstOrDefault().Id);

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        private async Task ChekUserRoles()
        {
            Type type = typeof(UserRoles);

            var fields = type.GetFields();

            foreach (var roleField in fields)
            {
                var role = roleField.GetValue(fields).ToString();

                if (!string.IsNullOrWhiteSpace(role) && !await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                }
            }
        }
    }
}

