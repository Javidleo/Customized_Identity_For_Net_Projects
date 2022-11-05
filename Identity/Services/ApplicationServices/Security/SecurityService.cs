using System.Security.Cryptography;
using System.Text;

namespace Identity.Services.ApplicationServices.Security
{
    public class SecurityService
    {
        public static string GetSha256Hash(string input)
        {
            using var algorithm = SHA256.Create();
            var byteValue = Encoding.UTF8.GetBytes(input);
            var hash = algorithm.ComputeHash(byteValue);
            return Convert.ToBase64String(hash);
        }

        public static Guid CreateSecureGuid()
        {
            var bytes = new byte[16];
            RandomNumberGenerator randomGenerator = RandomNumberGenerator.Create();
            randomGenerator.GetBytes(bytes);
            return new Guid(bytes);
        }
    }
}
