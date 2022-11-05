using Identity.DataSource;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Identity.Services.IdentityServices.RoleManagement
{
    public class AppRoleManager : RoleManager<AppRole>, IAppRoleManager
    {
        //private readonly IRoleStore<AppRole> _roleStore;
        //private readonly IEnumerable<IRoleValidator<AppRole>> _rolevalidators;
        //private readonly ILookupNormalizer _keyNormalizer;
        //private readonly IdentityErrorDescriber _errorDescriber;
        //private readonly ILogger<RoleManager<AppRole>> _logger;
        private readonly DbSet<AppRole> _roles;
        private readonly ApplicationDbContext _context;
        public AppRoleManager(IRoleStore<AppRole> store, IEnumerable<IRoleValidator<AppRole>> roleValidators,
                                     ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors,
                                     ILogger<RoleManager<AppRole>> logger, ApplicationDbContext context)
                                     : base(store, roleValidators, keyNormalizer, errors, logger)
        {
            //_roleStore = store;
            //_rolevalidators = roleValidators;
            //_keyNormalizer = keyNormalizer;
            //_errorDescriber = errors;
            //_logger = logger;
            _context = context;
            _roles = _context.Set<AppRole>();
        }

        public async Task<bool> DoesExist(string roleName)
        => await _context.Roles.AnyAsync(i => i.Name == roleName);
    }
}
