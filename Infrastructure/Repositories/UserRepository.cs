using Microsoft.EntityFrameworkCore;
using MovieWebApp.Domain.Entities;
using MovieWebApp.Domain.Interfaces;
using MovieWebApp.Infrastructure.Data;
using MovieWebApp.Infrastructure.SeedWorks;

namespace MovieWebApp.Infrastructure.Repositories
{
    public class UserRepository : RepositoryBase<User, int >, IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

    
        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

    }
}
