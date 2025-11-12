using MovieWebApp.Domain.Entities;
using MovieWebApp.Domain.SeedWorks;

namespace MovieWebApp.Domain.Interfaces
{
    public interface ICommentRepository : IRepository<Comment, int>
    {
        //Task<Comment> CreateAsync(Comment comment);
        //Task<Comment> UpdateAsync(Comment comment);
        //Task<bool> DeleteAsync(int commentId);
        Task<Comment> GetByIdAsync(int commentId);
        Task<List<Comment>> GetByMovieIdAsync(int movieId);
        Task<List<Comment>> GetByUserIdAsync(int userId);
        Task<List<Comment>> GetAllAsync();
    }
}


