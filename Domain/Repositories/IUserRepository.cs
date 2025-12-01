using MovieWebApp.Domain.Entities;
using MovieWebApp.Domain.SeedWorks;

namespace MovieWebApp.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User, int>
    {
      
        Task<User> GetByEmailAsync(string email);
      
    }
}
