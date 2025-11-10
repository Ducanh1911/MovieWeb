namespace MovieWebApp.Domain.SeedWorks
{
    public interface IRepository<T, key> where T : class
    {
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
        Task<T> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync();

    }
}
