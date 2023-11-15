using Microsoft.AspNetCore.Mvc;
using DeskReserve.Domain;
using Microsoft.AspNetCore.Authorization;
using DeskReserve.Interfaces;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;
using DeskReserve.Utils;
using DeskReserve.Exceptions;
using DeskReserve.Data.DBContext.Entity;

namespace DeskReserve.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;

        public AccountController(
            ILogger<AccountController> logger,
            IAuthService authService,
            IConfiguration configuration)
        {
            _logger = logger;
            _authService = authService;
            _configuration = configuration;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            _logger.LogInformation($"Registration attempt for {registerModel.Email}");

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Tuple<string, string> passwordHashAndSalt = SecurityUtils.HashUserPassword(registerModel.Password);

                var result = await _authService.CreateUser(registerModel, passwordHashAndSalt.Item1, passwordHashAndSalt.Item2);

                if(!result)
                {
                    return BadRequest($"User Registration failed");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Register)}");
                return Problem($"Something went wrong in the {nameof(Register)}", statusCode: 500);
            }

            return Ok("Registration successful.");
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            _logger.LogInformation($"Login Attempt for {loginModel.Email}");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (!await _authService.ValidateUser(loginModel))
                {
                    _logger.LogError("Invalid email and password combination.");
                    return Unauthorized();
                }

                return Accepted(new AuthenticatedResponse { Token = await _authService.CreateToken() });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(Login)}");
                return Problem($"Something Went Wrong in the {nameof(Login)}", statusCode: 500);
            }
        }

        [Authorize]
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Logout()
        {
            try
            {
                //TODO: clear refresh token from db
                return Ok("Logged out successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong during logout in the {nameof(Logout)}");
                return Problem($"Something went wrong during logout in the {nameof(Logout)}", statusCode: 500);
            }
        }

        [Authorize]
        [HttpPost("change-password")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel changePasswordModel)
        {
            if (changePasswordModel.NewPassword != changePasswordModel.ConfirmNewPassword)
            {
                return BadRequest("Passwords don't match");
            }

            Request.Headers.TryGetValue("Authorization", out StringValues jwtToken);
            List<Claim> claims = null;

            try
            {
                claims = SecurityUtils.GetAllClaimsFromToken(jwtToken).ToList() ?? throw new ClaimsNotSetException();
            }
            catch (Exception ex)
            { 
                _logger.LogError(ex, "Could not get all claims Token from passed token");
                return Problem($"Something Went Wrong in the {nameof(ChangePassword)}", statusCode: 500);
            }

            string userEmail = claims.Where(c => c.Type == ClaimTypes.Email).FirstOrDefault()?.Value;
            LoginModel loginModel = new LoginModel()
            {
                Email = userEmail,
                Password = changePasswordModel.CurrentPassword
            };

            if (!await _authService.ValidateUser(loginModel))
            {
                _logger.LogError("Current password does not match.");
                return Unauthorized();
            }

            try
            {
                await _authService.ChangeUserPassword(userEmail, changePasswordModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Problem($"Something Went Wrong in the {nameof(ChangePassword)}", statusCode: 500);
            }

            return Ok("Password changed successfully.");
        }

        [Authorize]
        [HttpPut("user/{id}/role")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChnagePermissions(Guid id, [FromBody] string newRoleName)
        {
            Role userRole = null;
            try
            {
                userRole = await _authService.GetRole(id);
                if (String.Equals(userRole.RoleName, newRoleName))
                {
                    return Ok();
                }

                userRole.RoleName = newRoleName;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Problem($"Something Went Wrong in the {nameof(ChnagePermissions)}", statusCode: 500);
            }

            var success = await _authService.UpdateRole(userRole);

            return success ? Ok($"User role updated to {newRoleName}") : BadRequest();
        }
    }
}
