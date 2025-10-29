    using MovieWebApp.Application.DTOs;
using MovieWebApp.Application.Interfaces;
using MovieWebApp.Domain.Entities;
using MovieWebApp.Domain.Interfaces;
using System.Reflection.Metadata.Ecma335;

namespace MovieWebApp.Application.Services
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;
        public GenreService(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<IEnumerable<Genre>> GetAllAsync()
        {
            return await _genreRepository.getAsync();
        }

        public async Task<Genre> GetByIdAsync(int id)
        {
            return await _genreRepository.getByIdAsync(id);
        }

        public async Task<Genre> CreateGenreAsync(GenreDto genredto)
        {
            var genre = new Genre()
            {
                Name = genredto.Name,
            };
            return await _genreRepository.CreateAsync(genre);
        }

       
        //public Task<Genre> UpdateGenreAsync(int id, GenreDto genre)
        //{
            
        //}
        public async Task<bool> DeleteGenreAsync(int id)
        {
            return await (_genreRepository.deleteAsync(id));
        }
    }
}
