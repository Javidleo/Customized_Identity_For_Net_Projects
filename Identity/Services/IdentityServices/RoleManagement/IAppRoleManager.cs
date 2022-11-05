using Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.Services.IdentityServices.RoleManagement
{
    public interface IAppRoleManager
    {
        #region Base Methods
        IQueryable<AppRole> Roles { get; }
        Task<AppRole> FindByIdAsync(string roleId);

        Task<AppRole> FindByNameAsync(string name);
        Task<IdentityResult> CreateAsync(AppRole appRole);
        Task<IdentityResult> UpdateAsync(AppRole role);
        #endregion


        Task<bool> DoesExist(string roleName);

    }
}
