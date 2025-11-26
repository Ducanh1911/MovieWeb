namespace MovieWebApp.Application.DTOs
{
    public class DashboardStatsDto
    {
        public int TotalMovies { get; set; }
        public int TotalUsers { get; set; }
        public int TotalRatings { get; set; }
        public int TotalComments { get; set; }
        public int TotalFavorites { get; set; }
        public double AverageRating { get; set; }
        public List<MovieSummaryDto> RecentMovies { get; set; } = new List<MovieSummaryDto>();
        public List<MovieSummaryDto> TopRatedMovies { get; set; } = new List<MovieSummaryDto>();
    }

    public class MovieSummaryDto
    {
        public int MovieId { get; set; }
        public string MovieName { get; set; } = string.Empty;
        public string Poster { get; set; } = string.Empty;
        public double Rating { get; set; }
        public int ViewCount { get; set; }
        public DateTime? CreatedAt { get; set; }
    }

    public class UserDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class UpdateUserRoleDto
    {
        public string Role { get; set; } = string.Empty;
    }

    public class RatingAdminDto
    {
        public int RatingId { get; set; }
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public int StarRating { get; set; }
        public string? Review { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UserName { get; set; }
        public string? MovieName { get; set; }
    }

    public class CommentAdminDto
    {
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UserName { get; set; }
        public string? MovieName { get; set; }
    }
}
