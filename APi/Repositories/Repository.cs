using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Tourplanner.Models;

namespace Tourplanner.Repositories;

public abstract class Repository<TEntity> : IRepository<TEntity>, IDisposable
where TEntity : class
{
    protected TourContext context;
    private bool disposed = false;
    protected DbSet<TEntity> dbSet;

    public Repository(TourContext tourContext)
    {
        context = tourContext;
        dbSet = context.Set<TEntity>();
    }

    public virtual async Task<TEntity?> Get(int id)
    {
        return await dbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<TEntity>?> GetAll()
    {
        return await dbSet.ToListAsync();
    }

    public virtual async Task Create(TEntity entity)
    {
        await dbSet.AddAsync(entity);
        await SaveAsync();
    }

    public virtual async Task Delete(TEntity entity)
    {
        dbSet.Remove(entity);
        await SaveAsync();
    }
    
    public async Task SaveAsync()
    {
        await context.SaveChangesAsync();
    }

    public virtual async Task DeleteById(int id)
    {
        var entity = dbSet.Find(id);
        dbSet.Remove(entity);
        await SaveAsync();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                context.Dispose();
            }
        }

        this.disposed = true;
    }
}