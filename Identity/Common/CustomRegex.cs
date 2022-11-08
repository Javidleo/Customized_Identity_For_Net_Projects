namespace Identity.Common
{
    public class CustomRegex
    {
        public const string Guid = "^[{]?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}[}]?$";

        public const string PhoneNumber = @"(\+98|0)?9\d{9}";

        public const string UserName = "^[A-Za-z0-9]*$";

        public const string Password = "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$";

        public const string EnglishAndPersianLettersAndNumbers = @"^[\u0600-\u06FF\s\d a-zA-Z]+$";

        public const string EnglishLetters = @"^[A-Za-z\s]*$";

        public const string EnglishLettersAndNumbers = @"^[A-Za-z\d\s]+$";

    }
}
