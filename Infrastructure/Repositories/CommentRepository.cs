using Microsoft.EntityFrameworkCore;
using MovieWebApp.Domain.Entities;
using MovieWebApp.Domain.Interfaces;
using MovieWebApp.Infrastructure.Data;

namespace MovieWebApp.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Comment> CreateAsync(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment> UpdateAsync(Comment comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<bool> DeleteAsync(int commentId)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null) return false;

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Comment> GetByIdAsync(int commentId)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Movie)
                .FirstOrDefaultAsync(c => c.CommentId == commentId);
        }

        public async Task<List<Comment>> GetByMovieIdAsync(int movieId)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Movie)
                .Where(c => c.MovieId == movieId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Comment>> GetByUserIdAsync(int userId)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Movie)
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Movie)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }
    }
}
