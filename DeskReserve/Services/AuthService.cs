using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;
using DeskReserve.Utils;
using DeskReserve.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DeskReserve.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace DeskReserve.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly IConfiguration _configuration;
        private User _user = new User();

        public AuthService(IUserRepository userRepository,
            ITokenRepository tokenRepository,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            _configuration = configuration;
        }

        public async Task<string> CreateToken()
        {
            var signingCredentials = GetSigningCredentials();
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

            var role = await _userRepository.GetRoleById(_user.UserId);
            claims.Add(new Claim(ClaimTypes.Role, role.RoleName));

            return claims;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var singinKey = Environment.GetEnvironmentVariable("SIGNIN_KEY");
            var kid = Environment.GetEnvironmentVariable("KID");
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(singinKey));
            secret.KeyId = kid;
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256Signature);
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

            var validPassword = VerifyPassword(loginModel.Password, _user.PasswordHash, _user.PasswordSalt);
            return (!ReferenceEquals(_user, null) && validPassword);
        }

        public async Task<bool> ChangeUserPassword(ChangePasswordModel changePasswordModel)
        {
            return false;
        }

        public async Task<bool> CreateUser(User user)
        {
            user.UserId = Guid.NewGuid();

            return await _userRepository.Add(user);
        }

        public async Task<bool> UpdateUser(RegisterModel newUserInfo)
        {
            _user = await _userRepository.GetByEmail(newUserInfo.Email) ?? throw new EntityNotFoundException();

            return await _userRepository.Update(HashUserPassword(newUserInfo));
        }

        public User HashUserPassword(RegisterModel registerModel)
        {
            registerModel.MapProperties(_user);

            byte[] salt = Encoding.ASCII.GetBytes("mySalt");
            string passwordHash = ComputeHash(registerModel.Password, new SHA256CryptoServiceProvider(), salt);

            _user.PasswordHash = passwordHash;
            _user.PasswordSalt = Encoding.ASCII.GetString(salt);

            return _user;
        }

        public async Task<string> CreateRefreshToken()
        {
            var newRefreshToken = GenerateRandomToken();

            await _tokenRepository.RemoveRefreshToken(_user.UserId);
            await _tokenRepository.SaveRefreshToken(_user.UserId, newRefreshToken);

            return newRefreshToken;
        }

        private static string GenerateRandomToken()
        {
            return Guid.NewGuid().ToString("N");
        }

        public static string ComputeHash(string input, HashAlgorithm algorithm, Byte[] salt)
        {
            Byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            // Combine salt and input bytes
            Byte[] saltedInput = new Byte[salt.Length + inputBytes.Length];
            salt.CopyTo(saltedInput, 0);
            inputBytes.CopyTo(saltedInput, salt.Length);

            Byte[] hashedBytes = algorithm.ComputeHash(saltedInput);

            return BitConverter.ToString(hashedBytes);
        }


        public static bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            byte[] salt = Encoding.ASCII.GetBytes(storedSalt);
            var enteredPasswordHash = ComputeHash(enteredPassword, new SHA256CryptoServiceProvider(), salt);
            return string.Equals(storedHash, enteredPasswordHash);
        }

        public IEnumerable<Claim> GetAllClaimsFromToken(string token)
        {
            IEnumerable<Claim> claims;

            try
            {
                var jsonToken = ReadToken(token.Replace("Bearer ", string.Empty)) ?? throw new Exception();
                claims = jsonToken.Claims;
            }
            catch (Exception e)
            {
                claims = null;
            }

            return claims;
        }

        public static JwtSecurityToken ReadToken(string jwtToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jsonToken = tokenHandler.ReadToken(jwtToken) as JwtSecurityToken;

            return jsonToken;
        }
    }
}

