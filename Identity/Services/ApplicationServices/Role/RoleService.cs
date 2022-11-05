using Identity.Common;
using Identity.Common.Exceptions;
using Identity.Models;
using Identity.Services.IdentityServices.RoleManagement;
using Identity.Services.IdentityServices.UserManagement;
using Identity.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.Services.ApplicationServices.Role
{
    public class RoleService : IRoleService
    {
        private readonly IAppRoleManager _roleManager;
        private readonly IAppUserManager _userManager;

        public RoleService(IAppRoleManager roleManager, IAppUserManager userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }


        public async Task CreateRole(string name, string Description)
        {
            if (await _roleManager.DoesExist(name))
                throw new ConflictException("This Role Already Exist");

            var role = AppRole.Create(name, Description);
            var result = await _roleManager.CreateAsync(role);

            if (result.Errors.Any())
            {
                var errorMessage = IdentityErrorHandler.MergeErrorMessages(result.Errors);
                throw new NotAcceptableException(errorMessage);
            }
        }

        public async Task<IdentityResult> AddUserRoleAsync(string roleName, int userId, DateTime fromDate)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role is null)
                throw new NotFoundException("role notfound");

            var result = await _userManager.AddUserRoleAsync(role.Id, userId, fromDate, null);
            if (result.Succeeded)
                return IdentityResult.Success;

            var errorMessage = IdentityErrorHandler.MergeErrorMessages(result.Errors);
            throw new NotAcceptableException(errorMessage);
        }

        public async Task<IdentityResult> AddUserRoleAsync(string roleName, int userId, DateTime fromDate, DateTime? toDate)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role is null)
                throw new NotFoundException("role notfound");

            if (await _userManager.UserExistAsync(userId) is false)
                throw new NotFoundException("user notfound");

            var result = await _userManager.AddUserRoleAsync(role.Id, userId, fromDate, toDate);
            if (result.Succeeded)
                return result;

            var errorMessage = IdentityErrorHandler.MergeErrorMessages(result.Errors);
            throw new NotAcceptableException(errorMessage);
        }

        public async Task ChangeRoleProperties(string rolename, string description)
        {
            var role = await _roleManager.FindByNameAsync(rolename);
            if (role is null)
                throw new NotFoundException("role notfound");

            if (role.IsActive is false)
                throw new NotAcceptableException("cannot update deactivated role");

            role.ChangeProperties(rolename, description);

            var result = await _roleManager.UpdateAsync(role);

            if (result.Errors.Any())
            {
                var errorMessage = IdentityErrorHandler.MergeErrorMessages(result.Errors);
                throw new NotAcceptableException(errorMessage);
            }
        }

        public async Task DeactivateRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role is null)
                throw new NotFoundException("role notfound");

            role.Deactivate();

            var result = await _roleManager.UpdateAsync(role);

            if (result.Errors.Any())
            {
                var errorMessage = IdentityErrorHandler.MergeErrorMessages(result.Errors);
                throw new NotAcceptableException(errorMessage);
            }
        }

        public async Task<List<RoleListViewModel>> GetAllRoles()
        {
            var roleList = await _roleManager.Roles.Select(i => new RoleListViewModel
            {
                Name = i.Name,
                Description = i.Description,
                IsActive = i.IsActive,
            }).AsNoTracking().ToListAsync();

            return roleList;
        }
    }
}
