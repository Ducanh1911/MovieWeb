using MovieWebApp.Application.DTOs;

namespace MovieWebApp.Application.Interfaces
{
    public interface IAdminService
    {
        Task<DashboardStatsDto> GetDashboardStatsAsync();
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(int userId);
        Task<bool> UpdateUserRoleAsync(int userId, string role);
        Task<bool> DeleteUserAsync(int userId);
        Task<List<RatingAdminDto>> GetAllRatingsAsync();
        Task<bool> DeleteRatingAsync(int ratingId);
        Task<List<CommentAdminDto>> GetAllCommentsAsync();
        Task<bool> DeleteCommentAsync(int commentId);
    }
}
