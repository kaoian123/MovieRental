using System.Security.Cryptography;
using System.Text;

namespace MovieRental.Services
{
    public class TokenService(string secretKey)
    {
        private readonly string _secretKey = secretKey;

        public string GenerateToken(string email)
        {
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var rawToken = $"{email}:{timestamp}";
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_secretKey));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawToken));
            var token = $"{rawToken}:{Convert.ToBase64String(hash)}";
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
        }

        public bool ValidateToken(string token, string email, TimeSpan expiration)
        {
            Console.WriteLine("這6");
            var decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(token));
            var parts = decodedToken.Split(':');
            if (parts.Length != 3)
            {
                Console.WriteLine("這");
                return false;
            }

            var tokenEmail = parts[0];
            var timestamp = parts[1];
            var tokenHash = parts[2];

            if (tokenEmail != email)
            {
                Console.WriteLine("這1");
                return false;
            }

            if (!DateTime.TryParseExact(timestamp, "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out var tokenTime))
            {
                Console.WriteLine("這2");
                return false;
            }

            if (DateTime.UtcNow - tokenTime > expiration)
            {
                Console.WriteLine("這3");
                return false;
            }

            var rawToken = $"{tokenEmail}:{timestamp}";
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_secretKey));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawToken));
            var computedHash = Convert.ToBase64String(hash);

            return computedHash == tokenHash;
        }
    }
}
