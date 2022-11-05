using Identity.DataSource;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Identity.Services.IdentityServices.UserManagement
{
    public class AppUserManager : UserManager<AppUser>, IAppUserManager
    {
        private readonly IUserStore<AppUser> _store;
        private readonly IOptions<IdentityOptions> _optionsAccessor;
        private readonly IPasswordHasher<AppUser> _passwordHasher;
        private readonly IEnumerable<IUserValidator<AppUser>> _userValidators;
        private readonly IEnumerable<IPasswordValidator<AppUser>> _passwordValidators;
        private readonly ILookupNormalizer _keyNormalizer;
        private readonly IdentityErrorDescriber _errors;
        private readonly IServiceProvider _service;
        private readonly ILogger<UserManager<AppUser>> _logger;
        private DbSet<AppUser> _users;
        private DbSet<AppRole> _roles;
        private readonly DbSet<AppUserRole> _userRoles;
        private readonly ApplicationDbContext _context;

        public AppUserManager(IUserStore<AppUser> store, IOptions<IdentityOptions> optionsAccessor,
                        IPasswordHasher<AppUser> passwordHasher, IEnumerable<IUserValidator<AppUser>> userValidators,
                        IEnumerable<IPasswordValidator<AppUser>> passwordValidators, ILookupNormalizer keyNormalizer,
                        IdentityErrorDescriber errors, IServiceProvider services,
                        ILogger<UserManager<AppUser>> logger, ApplicationDbContext context) : base(store, optionsAccessor,
                                                                  passwordHasher, userValidators, passwordValidators,
                                                                  keyNormalizer, errors, services, logger)
        {
            _store = store;
            _optionsAccessor = optionsAccessor;
            _passwordHasher = passwordHasher;
            _userValidators = userValidators;
            _passwordValidators = passwordValidators;
            _keyNormalizer = keyNormalizer;
            _errors = errors;
            _service = services;
            _logger = logger;
            _context = context;
            _users = _context.Set<AppUser>();
            _roles = _context.Set<AppRole>();
            _userRoles = _context.Set<AppUserRole>();
        }

        public async Task<AppUser> FindByPhoneNumberAsync(string phoneNumber)
        => await _users.FirstOrDefaultAsync(i => i.PhoneNumber == phoneNumber);

        public async Task<AppUser> FindByPhoneNumberAsync(string phoneNumber, bool verified)
        => await _users.FirstOrDefaultAsync(i => i.PhoneNumber == phoneNumber && i.PhoneNumberConfirmed == verified);


        public AppUser FindByPhoneNumber(string phoneNumber, bool verified)
        => _users.FirstOrDefault(i => i.PhoneNumber == phoneNumber && i.PhoneNumberConfirmed == verified);

        public async Task<IdentityResult> AddUserRoleAsync(int roleId, int userId, DateTime fromDate, DateTime? toDate)
        {
            var userRole = AppUserRole.Create(roleId, userId, fromDate, toDate);

            try
            {
                await _userRoles.AddAsync(userRole);
                await _context.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch (Exception)
            {
                return IdentityResult.Failed();
            }
        }
        public async Task<bool> PhoneNumberExistAsync(string phoneNumber, bool verified)
        => await _users.AnyAsync(i => i.PhoneNumber == phoneNumber && i.PhoneNumberConfirmed == verified);

        public async Task<bool> UserExistAsync(int userId)
        => await _users.AnyAsync(i => i.Id == userId);

        public DbSet<AppUser> UserDbSetGenerator()
        {
            return _users = _context.Set<AppUser>();
        }

        public async Task<AppUser> FindByIdAsync(int userId)
        => await _users.FirstOrDefaultAsync(i => i.Id == userId);


        public async Task<AppUser> FindByIdAsync(Guid userGuid)
        => await _users.FirstOrDefaultAsync(i => i.Guid == userGuid);
    }
}