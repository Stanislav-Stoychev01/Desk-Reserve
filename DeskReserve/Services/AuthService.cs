using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;
using DeskReserve.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DeskReserve.Exceptions;
using DeskReserve.Utils;

namespace DeskReserve.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRolesRepository _userRolesRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly IConfiguration _configuration;
        private User _user = new User();

        public AuthService(IUserRepository userRepository,
            IUserRolesRepository userRolesRepository,
            ITokenRepository tokenRepository,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _userRolesRepository = userRolesRepository;
            _tokenRepository = tokenRepository;
            _configuration = configuration;
        }

        public async Task<bool> CreateUser(RegisterModel regigerModel, string passwordHash, string salt)
        {
            regigerModel.MapProperties(_user);
            _user.UserId = Guid.NewGuid();
            _user.PasswordHash = passwordHash;
            _user.PasswordSalt = salt;

            Guid roleId = await _userRolesRepository.GetRoleIdByRoleName("Employee");
            UserRole userRole = new UserRole()
            {
                UserRolesId = Guid.NewGuid(),
                UserId = _user.UserId,
                RoleId = roleId
            };

            var success = false;
            try
            {
                success = await _userRolesRepository.AddUserRole(userRole);
                success = await _userRepository.Add(_user);
            }
            catch (Exception ex)
            {
                return false;
            }

            return success;
        }

        public async Task<string> CreateToken()
        {
            var signingCredentials = SecurityUtils.GetSigningCredentials();
            var claims = await CreateClaims();
            var token = GenerateTokenOptions(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var expiration = DateTime.Now.AddMinutes(60);

            var token = new JwtSecurityToken(
                issuer: jwtSettings.GetSection("Issuer").Value,
                claims: claims,
                expires: expiration,
                signingCredentials: signingCredentials
                );

            return token;
        }

        private async Task<List<Claim>> CreateClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, _user.Email),
                new Claim(ClaimTypes.Name, _user.UserName)
            };

            var role = await _userRolesRepository.GetRoleByUserId(_user.UserId) ?? throw new EntityNotFoundException();
            claims.Add(new Claim(ClaimTypes.Role, role.RoleName));

            return claims;
        }

        public async Task<bool> ValidateUser(LoginModel loginModel)
        {
            try
            {
                _user = await _userRepository.GetByEmail(loginModel.Email) ?? throw new EntityNotFoundException();
            } 
            catch (Exception ex)
            {
                return false;
            }

            var validPassword = SecurityUtils.VerifyPassword(loginModel.Password, _user.PasswordHash, _user.PasswordSalt);
            return (!ReferenceEquals(_user, null) && validPassword);
        }

        public async Task<bool> ChangeUserPassword(string userEmail, ChangePasswordModel changePasswordModel)
        {
            try
            {
                _user = await _userRepository.GetByEmail(userEmail) ?? throw new EntityNotFoundException();

            }
            catch (Exception ex)
            {
                return false;
            }

            var passwordHashAndSalt = SecurityUtils.HashUserPassword(changePasswordModel.NewPassword);
            _user.PasswordHash = passwordHashAndSalt.Item1;
            _user.PasswordSalt = passwordHashAndSalt.Item2;

            return await _userRepository.Update(_user);
        }

        public async Task<string> CreateRefreshToken()
        {
            var newRefreshToken = SecurityUtils.GenerateRandomToken();

            await _tokenRepository.RemoveRefreshToken(_user.UserId);
            await _tokenRepository.SaveRefreshToken(_user.UserId, newRefreshToken);

            return newRefreshToken;
        }

        public async Task<User> GetUser(Guid userId)
        {
            return await _userRepository.GetById(userId) ?? throw new EntityNotFoundException();
        }

        public async Task<Role> GetRole(Guid userId)
        {
            return await _userRolesRepository.GetRoleByUserId(userId) ?? throw new EntityNotFoundException();
        }

        public async Task<Guid> GetRoleId(string roleName)
        {
            Guid roleId;
            try
            {
                roleId = await _userRolesRepository.GetRoleIdByRoleName(roleName);
            }
            catch (Exception ex)
            {
                return Guid.Empty;
            }

            return roleId;
        }

        public async Task<UserRole> GetUserRole(Guid userId)
        {
            return await _userRolesRepository.GetUserRole(userId) ?? throw new EntityNotFoundException();
        }

        public async Task<bool> UpdateUserRole(UserRole newRole)
        {
            return await _userRolesRepository.Update(newRole);
        }
    }
}

