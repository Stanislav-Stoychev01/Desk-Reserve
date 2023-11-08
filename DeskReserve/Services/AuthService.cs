using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;
using DeskReserve.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DeskReserve.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private User _user = new User();

        public AuthService(IUserRepository userRepository,
            ITokenRepository tokenRepository,
            IConfiguration configuration,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<string> CreateToken()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var token = GenerateTokenOptions(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var expiration = DateTime.Now.AddMinutes(Convert.ToDouble(
                jwtSettings.GetSection("lifetime").Value));

            var token = new JwtSecurityToken(
                issuer: jwtSettings.GetSection("Issuer").Value,
                claims: claims,
                expires: expiration,
                signingCredentials: signingCredentials
                );

            return token;
        }

        private async Task<List<Claim>> GetClaims()
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
            var key = Environment.GetEnvironmentVariable("KEY");
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        public async Task<bool> ValidateUser(LoginModel loginModel)
        {
            _user = await _userRepository.GetByEmail(loginModel.Email);
            var validPassword = ReferenceEquals(_user.PasswordHash, loginModel.Password);
            return (!ReferenceEquals(_user, null) && validPassword);
        }

        public async Task<string> CreateRefreshToken()
        {
            var newRefreshToken = GenerateRandomToken();

            await _tokenRepository.RemoveRefreshToken(_user.UserId);
            await _tokenRepository.SaveRefreshToken(_user.UserId, newRefreshToken);

            return newRefreshToken;
        }

        public async Task<TokenRequest> VerifyRefreshToken(TokenRequest request)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(request.Token);
            var email = tokenContent.Claims.ToList().FirstOrDefault(q => q.Type == ClaimTypes.Email)?.Value;
            _user = await _userRepository.GetByEmail(email);
            try
            {
                var isValid = await _tokenRepository.VerifyUserToken(_user.UserId, request.RefreshToken);
                if (isValid)
                {
                    return new TokenRequest { Token = await CreateToken(), RefreshToken = await CreateRefreshToken() };
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return null;
        }

        public User CreateUser(RegisterModel registerModel)
        {
            var user = _mapper.MapProperties(registerModel, _user);

            byte[] salt = Encoding.ASCII.GetBytes("mySalt");
            string passwordHash = ComputeHash(registerModel.Password, new SHA256CryptoServiceProvider(), salt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = salt.ToString();

            return user;
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
    }
}

