using MovieWebApp.Domain.Entities;
using MovieWebApp.Domain.SeedWorks;

namespace MovieWebApp.Domain.Repositories
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken, int>
    {
        Task<RefreshToken?> GetByTokenAsync(string token);
        Task<IEnumerable<RefreshToken>> GetActiveTokensByUserIdAsync(int userId);
        Task RevokeAllUserTokensAsync(int userId, string? revokedByIp = null);
    }
}
