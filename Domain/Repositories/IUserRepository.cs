using MovieWebApp.Domain.Entities;
using MovieWebApp.Domain.SeedWorks;

namespace MovieWebApp.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User, int>
    {
        //Task<User> GetByIdAsync(int id);
        Task<User> GetByEmailAsync(string email);
        //Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task<bool> DeleteAsync(int id);
        Task<List<User>> GetAllAsync();
    }
}
