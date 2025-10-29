using MovieWebApp.Application.DTOs;

namespace MovieWebApp.Application.Interfaces
{
    public interface ICommentService
    {
        Task<CommentDto> CreateCommentAsync(CreateCommentDto createCommentDto, int userId);
        Task<CommentDto> UpdateCommentAsync(int commentId, UpdateCommentDto updateCommentDto, int userId);
        Task<bool> DeleteCommentAsync(int commentId, int userId);
        Task<CommentDto> GetCommentByIdAsync(int commentId);
        Task<List<CommentDto>> GetCommentsByMovieIdAsync(int movieId);
        Task<List<CommentDto>> GetCommentsByUserIdAsync(int userId);
    }
}

















