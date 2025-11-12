using MovieWebApp.Application.DTOs;
using MovieWebApp.Application.Interfaces;
using MovieWebApp.Domain.Entities;
using MovieWebApp.Domain.Interfaces;

namespace MovieWebApp.Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<CommentDto> CreateCommentAsync(CreateCommentDto createCommentDto, int userId)
        {
            var comment = new Comment
            {
                UserId = userId,
                MovieId = createCommentDto.MovieId,
                Content = createCommentDto.Content,
                CreatedAt = DateTime.UtcNow
            };

            var createdComment = await _commentRepository.AddAsync(comment);
            return MapToDto(createdComment);
        }

        public async Task<CommentDto> UpdateCommentAsync(int commentId, UpdateCommentDto updateCommentDto, int userId)
        {
            var comment = await _commentRepository.GetByIdAsync(commentId);
            if (comment == null)
                throw new ArgumentException("Không tìm thấy bình luận");

            if (comment.UserId != userId)
                throw new UnauthorizedAccessException("Bạn không có quyền cập nhật bình luận này");

            comment.Content = updateCommentDto.Content;
            comment.UpdatedAt = DateTime.UtcNow;

            var updatedComment = await _commentRepository.UpdateAsync(comment);
            return MapToDto(updatedComment);
        }

        public async Task<bool> DeleteCommentAsync(int commentId, int userId)
        {
            var comment = await _commentRepository.GetByIdAsync(commentId);
            if (comment == null)
                return false;

            if (comment.UserId != userId)
                throw new UnauthorizedAccessException("Bạn không có quyền xóa bình luận này");

            return await _commentRepository.DeleteAsync(commentId);
        }

        public async Task<CommentDto> GetCommentByIdAsync(int commentId)
        {
            var comment = await _commentRepository.GetByIdAsync(commentId);
            return comment != null ? MapToDto(comment) : null;
        }

        public async Task<List<CommentDto>> GetCommentsByMovieIdAsync(int movieId)
        {
            var comments = await _commentRepository.GetByMovieIdAsync(movieId);
            return comments.Select(MapToDto).ToList();
        }

        public async Task<List<CommentDto>> GetCommentsByUserIdAsync(int userId)
        {
            var comments = await _commentRepository.GetByUserIdAsync(userId);
            return comments.Select(MapToDto).ToList();
        }

        private CommentDto MapToDto(Comment comment)
        {
            return new CommentDto
            {
                CommentId = comment.CommentId,
                UserId = comment.UserId,
                MovieId = comment.MovieId,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                UpdatedAt = comment.UpdatedAt,
                UserName = comment.User?.UserName ?? "Unknown"
            };
        }
    }
}

















