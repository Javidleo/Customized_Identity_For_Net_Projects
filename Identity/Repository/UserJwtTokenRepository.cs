using Identity.DataSource;
using Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace Identity.Repository
{
    public class UserJwtTokenRepository
    {
        private readonly ApplicationDbContext _context;

        public UserJwtTokenRepository(ApplicationDbContext context) => _context = context;

        public void Add(UserJWTToken token)
        {
            _context.UserJWTToken.Add(token);
            _context.SaveChanges();
        }

        public async Task AddAsync(UserJWTToken token)
        {
            await _context.UserJWTToken.AddAsync(token);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveUserTokensAsync(Guid usr_guid)
        {
            await _context.UserJWTToken.Where(i => i.usr_guid == usr_guid)
                .ForEachAsync(i => _context.UserJWTToken.Remove(i));

            await _context.SaveChangesAsync();
        }

        public async Task<UserJWTToken> FindTokenByRefreshTokenAsync(string refreshToken)
        => await _context.UserJWTToken.FirstOrDefaultAsync(i => i.jwt_RefreshToken == refreshToken);
    }
}
