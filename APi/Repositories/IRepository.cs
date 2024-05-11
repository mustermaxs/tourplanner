namespace Tourplanner.Repositories;

public interface IRepository<T>
{
    public Task<T?> Get(int id);
    public Task<IEnumerable<T>?> GetAll();
    public Task Delete(T obj);
    public Task Create(T obj);
    public Task SaveAsync();
    public Task UpdateAsync(T obj);
}