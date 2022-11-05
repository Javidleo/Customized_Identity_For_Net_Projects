using System.Security.Claims;

namespace Identity.Models
{
    public class JWTTokenData
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public IEnumerable<Claim> Claims { get; set; }
    }
}
