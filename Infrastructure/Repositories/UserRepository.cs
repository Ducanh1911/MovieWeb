using Microsoft.EntityFrameworkCore;
using MovieWebApp.Domain.Entities;
using MovieWebApp.Domain.Interfaces;
using MovieWebApp.Infrastructure.Data;
using MovieWebApp.Infrastructure.SeedWorks;

namespace MovieWebApp.Infrastructure.Repositories
{
    public class UserRepository : RepositoryBase<User, int> ,IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context) : base(context)
        {
            {
                _context = context;
            }
        }
        //public async Task<User> GetByIdAsync(int id)
        //{
        //    return await _context.users.FirstOrDefaultAsync(u => u.UserId == id);
        //}
        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        //public async Task AddAsync(User user)
        //{
        //    _context.users.Add(user);
        //    await _context.SaveChangesAsync();
        //}

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.users.ToListAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.users.FindAsync(id);
            if (user == null) return false;

            _context.users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task UpdateAsync(User user)
        {
            _context.users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
