using Microsoft.EntityFrameworkCore;
using MovieWebApp.Domain.SeedWorks;
using MovieWebApp.Infrastructure.Data;

namespace MovieWebApp.Infrastructure.SeedWorks
{
    public class RepositoryBase<T, Key> : IRepository<T, Key> where T : class
    {
        private readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public RepositoryBase(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        // Thêm entity
        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);          // thêm async
            await _context.SaveChangesAsync();      // lưu xuống DB
            return entity;                          // trả về entity vừa thêm
        }

        // Cập nhật entity
        public async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);                  // đánh dấu entity cần update
            await _context.SaveChangesAsync();      // lưu xuống DB
            return entity;                          // trả về entity vừa update
        }

        // Xoá entity theo id
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id); // tìm entity theo id
            if (entity == null)
                return false;

            _dbSet.Remove(entity);                   // xoá entity
            await _context.SaveChangesAsync();       // lưu thay đổi
            return true;
        }

        // Lấy entity theo id
        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);      // trả về entity hoặc null
        }

        // Lấy tất cả entity
        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();      // trả về danh sách entity
        }
    }
}
