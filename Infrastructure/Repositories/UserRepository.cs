using Microsoft.EntityFrameworkCore;
using MovieWebApp.Domain.Entities;
using MovieWebApp.Domain.Interfaces;
using MovieWebApp.Infrastructure.Data;

namespace MovieWebApp.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.users.FirstOrDefaultAsync(u => u.UserId == id);
        }
        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddAsync(User user)
        {
            _context.users.Add(user);
            await _context.SaveChangesAsync();
        }

        public Task<List<User>> GetallAsync(User user)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(User user)
        {
            _context.users.Update(user);
            await _context.SaveChangesAsync();


        }
    }
}
