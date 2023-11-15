using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DeskReserve.Utils
{
    public static class SecurityUtils
    {
        public static SigningCredentials GetSigningCredentials()
        {
            var singinKey = Environment.GetEnvironmentVariable("SIGNIN_KEY");
            var kid = Environment.GetEnvironmentVariable("KID");
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(singinKey));
            secret.KeyId = kid;
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256Signature);
        }

        public static Tuple<string, string> HashUserPassword(string password)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            string randomString = new string(Enumerable.Repeat(chars, 10).Select(s => s[random.Next(s.Length)]).ToArray());
            byte[] salt = Encoding.ASCII.GetBytes(randomString);
            string passwordHash = ComputeHash(password, new SHA256CryptoServiceProvider(), salt);

            return new Tuple<string, string>(passwordHash, Encoding.ASCII.GetString(salt));
        }

        public static string GenerateRandomToken()
        {
            return Guid.NewGuid().ToString("N");
        }

        public static string ComputeHash(string input, HashAlgorithm algorithm, Byte[] salt)
        {
            Byte[] inputBytes = Encoding.UTF8.GetBytes(input);

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

        public static IEnumerable<Claim> GetAllClaimsFromToken(string token)
        {
            IEnumerable<Claim> claims;

            try
            {
                var jsonToken = SecurityUtils.ReadToken(token.Replace("Bearer ", string.Empty)) ?? throw new Exception();
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
