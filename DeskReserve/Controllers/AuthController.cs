using Microsoft.AspNetCore.Mvc;
using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;
using Microsoft.AspNetCore.Authorization;
using DeskReserve.Interfaces;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;

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
                User user = _authService.HashUserPassword(registerModel);

                var result = await _authService.CreateUser(user);

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
        public async Task<IActionResult> Logout()
        {
            try
            {
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
                claims = _authService.GetAllClaimsFromToken(jwtToken).ToList() ?? throw new NullReferenceException();
            }
            catch (Exception ex)
            { 
                _logger.LogError(ex, "Could not get all claims Token from passed token");
                return Problem($"Something Went Wrong in the {nameof(ChangePassword)}", statusCode: 500);
            }

            LoginModel loginModel = new LoginModel()
            {
                Email = claims.Where(c => c.Type == ClaimTypes.Email).FirstOrDefault()?.Value,
                Password = changePasswordModel.CurrentPassword
            };

            if (!await _authService.ValidateUser(loginModel))
            {
                _logger.LogError("Invalid password.");
                return Unauthorized();
            }

            RegisterModel newUser = new RegisterModel()
            {
                UserName = claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault()?.Value,
                Email = loginModel.Email,
                Password = loginModel.Password,
            };

            try
            {
                await _authService.UpdateUser(newUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Problem($"Something Went Wrong in the {nameof(ChangePassword)}", statusCode: 500);
            }

            return Ok("Password changed successfully.");
        }
    }
}
