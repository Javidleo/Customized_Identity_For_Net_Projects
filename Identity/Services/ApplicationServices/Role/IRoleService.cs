using Identity.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Identity.Services.ApplicationServices.Role
{
    public interface IRoleService
    {
        #region Role 
        Task CreateRole(string name, string description);
        Task ChangeRoleProperties(string roleName, string description);
        Task DeactivateRole(string roleName);
        Task<List<RoleListViewModel>> GetAllRoles();
        #endregion
        #region User Role
        Task<IdentityResult> AddUserRoleAsync(string roleName, int userId, DateTime fromDate, DateTime? toDate);

        Task<IdentityResult> AddUserRoleAsync(string roleName, int userId, DateTime fromDate);
        #endregion
    }
}
