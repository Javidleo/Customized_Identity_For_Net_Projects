namespace Identity.Common.Settings
{
    public class BearerToken
    {
        public string Key { set; get; }
        public string EncryptKey { set; get; }
        public string Issuer { set; get; }
        public string Audience { set; get; }
        public int AccessTokenExpirationMinutes { set; get; }
        public int RefreshTokenExpirationMinutes { set; get; }
    }
}
