using Microsoft.AspNetCore.Identity;

namespace Identity.Common
{
    public class IdentityErrorHandler
    {
        public static string MergeErrorMessages(IEnumerable<IdentityError> errors)
        {
            string result = string.Empty;

            foreach (var error in errors)
            {
                result += error.Code + Environment.NewLine;
                result += error.Description + Environment.NewLine;
            }
            return result;
        }
    }
}
