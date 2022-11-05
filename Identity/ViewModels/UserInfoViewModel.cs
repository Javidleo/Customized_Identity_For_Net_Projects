namespace Identity.ViewModels
{
    public class UserInfoViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public bool PhoneNumberVerified { get; set; }
        public bool IsLockedOut { get; set; }
        public bool RequireTwoFactorAuthentication { get; set; }
    }
}