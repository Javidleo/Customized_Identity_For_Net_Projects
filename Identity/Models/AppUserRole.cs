using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Identity.Models
{
    public class AppUserRole : IdentityUserRole<int>
    {
        public AppUserRole()
        {

        }

        [DataType(DataType.Date)]
        public DateTime FromDate { get; set; } = DateTime.Now.Date;

        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }

        private AppUserRole(int roleId, int userId, DateTime fromDate, DateTime? toDate)
        {
            RoleId = roleId;
            UserId = UserId;
            FromDate = fromDate.Date;
            ToDate = toDate == null ? null : toDate.Value.Date;
        }

        public static AppUserRole Create(int roleId, int userId, DateTime fromDate, DateTime? toDate)
        => new(roleId, userId, fromDate, toDate);
    }
}
