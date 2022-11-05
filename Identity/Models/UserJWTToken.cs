using System.ComponentModel.DataAnnotations;

namespace Identity.Models
{
    public class UserJWTToken
    {
        [Key]
        public int jwt_Id { get; set; }

        public string jwt_RefreshToken { get; set; }

        public DateTime jwt_RefreshTokenExpiresDateTime { get; set; }

        public Guid usr_guid { get; set; }

        public UserJWTToken()
        {

        }
        private UserJWTToken(Guid userGuid, string refreshToken, DateTime refreshTokenExpireationTime)
        {
            usr_guid = userGuid;
            jwt_RefreshToken = refreshToken;
            jwt_RefreshTokenExpiresDateTime = refreshTokenExpireationTime;
        }
        public static UserJWTToken Create(Guid userGuid, string refreshToken, DateTime refreshTokenExpireationTime)
        => new(userGuid, refreshToken, refreshTokenExpireationTime);
    }
}
