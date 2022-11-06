using Microsoft.AspNetCore.Identity;

namespace Identity.Models
{
    public class AppUser : IdentityUser<int>
    {
        public Guid Guid { get; set; }
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? NationalCode { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime? BirthDate { get; set; }

        public string? PersianBirthDate { get; set; }

        public bool? Gender { get; set; }


        // ///////////////   Help Desk Constractor
        public AppUser(string firstName, string lastName, string userName, string phoneNumber, string email, string nationalCode)
        {
            FirstName = firstName;
            LastName = lastName;
            UserName = userName;
            PhoneNumber = phoneNumber;
            Email = email;
            NationalCode = nationalCode;
        }

        public static AppUser Create(string firstName, string lastName, string userName, string phoneNumber, string email,
                                    string nationalCode)
        => new(firstName, lastName, userName, phoneNumber, email, nationalCode);


        public AppUser(string email, string firstName, string lastName, bool emailConfirmed)
        {
            Email = email;
            FirstName = FirstName;
            LastName = lastName;
            EmailConfirmed = emailConfirmed;
        }

        public static AppUser Create(string email, string firstName, string lastName, bool emailConfirmed = true)
        => new(email, firstName, lastName, emailConfirmed);


        private AppUser(string userName, string phoneNumber)
        {
            UserName = userName;
            PhoneNumber = phoneNumber;
        }

        public static AppUser Create(string userName, string phoneNumber)
        => new(userName, phoneNumber);


        public void SetInformation(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

    }
}
