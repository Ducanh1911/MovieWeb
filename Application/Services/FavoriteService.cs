using MovieWebApp.Application.DTOs;
using MovieWebApp.Application.Interfaces;
using MovieWebApp.Domain.Entities;
using MovieWebApp.Domain.Interfaces;

namespace MovieWebApp.Application.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMovieRepository _movieRepository;

        public FavoriteService(
            IFavoriteRepository favoriteRepository,
            IUserRepository userRepository,
            IMovieRepository movieRepository)
        {
            _favoriteRepository = favoriteRepository;
            _userRepository = userRepository;
            _movieRepository = movieRepository;
        }

        public async Task<IEnumerable<Movie>> GetFavoritesByUserAsync(int userId)
        {
            var favorites = await _favoriteRepository.GetFavoritesByUserAsync(userId);
            if (favorites == null || !favorites.Any())
                return new List<Movie>();

            return favorites.Select(f => f.Movie).ToList();
        }

        public async Task<Favorite> AddFavoriteAsync(FavoriteDto dto)
        {
            var user = await _userRepository.GetByIdAsync(dto.UserId);
            if (user == null)
                throw new InvalidOperationException("Người dùng không tồn tại.");

            var movie = await _movieRepository.GetByIdAsync(dto.MovieId);
            if (movie == null)
                throw new InvalidOperationException("Phim không tồn tại.");

            var existing = await _favoriteRepository.GetFavoriteAsync(dto.UserId, dto.MovieId);
            if (existing != null)
                throw new InvalidOperationException("Phim đã có trong danh sách yêu thích.");

            var favorite = new Favorite
            {
                UserId = dto.UserId,
                MovieId = dto.MovieId
            };

            await _favoriteRepository.AddAsync(favorite);
            return favorite;
        }

        public async Task<bool> RemoveFavoriteAsync(int userId, int movieId)
        {
            var favorite = await _favoriteRepository.GetFavoriteAsync(userId, movieId);
            if (favorite == null) return false;

            await _favoriteRepository.RemoveAsync(favorite);
            return true;
        }
    }
}
