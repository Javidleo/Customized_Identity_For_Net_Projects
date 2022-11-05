namespace Identity.ViewModels
{
    public class UserListViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorAuthenticationEnabled { get; set; }
    }
}