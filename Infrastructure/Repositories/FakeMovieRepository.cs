using MovieWebApp.Domain.Entities;
using MovieWebApp.Domain.Interfaces;

namespace MovieWebApp.Infrastructure.Repositories
{
    public class FakeMovieRepository : IMovieRepository
    {
        private readonly List<Movie> _movies = new();
        private int _nextId = 1;

        public Task<IEnumerable<Movie>> GetAsync()
        {
            return Task.FromResult(_movies.AsEnumerable());
        }

        public Task<Movie> GetByIdAsync(int id)
        {
            var movie = _movies.FirstOrDefault(m => m.MovieId == id);
            return Task.FromResult(movie);
        }

        public Task<(IEnumerable<Movie> Movies, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var total = _movies.Count;
            var items = _movies
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsEnumerable();

            return Task.FromResult((items, total));
        }

        public Task<IEnumerable<Movie>> SearchByNameAsync(string keyword)
        {
            var result = _movies
                .Where(m => m.MovieName.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                .AsEnumerable();

            return Task.FromResult(result);
        }

        public Task<Movie> AddAsync(Movie movie, List<int> genreIds)
        {
            movie.MovieId = _nextId++;
            _movies.Add(movie);
            return Task.FromResult(movie);
        }

        public Task<Movie> UpdateAsync(Movie movie, List<int> genreIds)
        {
            var existing = _movies.FirstOrDefault(m => m.MovieId == movie.MovieId);
            if (existing == null) return Task.FromResult<Movie>(null);

            existing.MovieName = movie.MovieName;
            existing.Description = movie.Description;
            existing.ReleaseYear = movie.ReleaseYear;
            existing.Country = movie.Country;
            existing.Language = movie.Language;
            existing.Poster = movie.Poster;
            existing.VideoUrl = movie.VideoUrl;
            existing.Genres = movie.Genres;

            return Task.FromResult(existing);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var movie = _movies.FirstOrDefault(m => m.MovieId == id);
            if (movie == null) return Task.FromResult(false);

            _movies.Remove(movie);
            return Task.FromResult(true);
        }
    }
}
