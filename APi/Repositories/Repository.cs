using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Tourplanner.Models;
using Api.Services.Logging;
using LoggerFactory = Api.Services.Logging.LoggerFactory;

namespace Tourplanner.Repositories;

public abstract class Repository<TEntity> : IRepository<TEntity>, IDisposable
    where TEntity : class
{
    protected TourContext context;
    private bool disposed = false;
    protected DbSet<TEntity> dbSet;
    protected ILoggerWrapper Logger = LoggerFactory.GetLogger();

    public Repository(TourContext tourContext)
    {
        context = tourContext;
        dbSet = context.Set<TEntity>();
    }


    public virtual async Task<TEntity?> Get(int id)
    {
        Logger.Info($"[Repository] Get {typeof(TEntity)} with id {id}");
        return await dbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<TEntity>?> GetAll()
    {
        Logger.Info($"[Repository] GetAll {typeof(TEntity)}");
        return await dbSet.ToListAsync();
    }

    public virtual async Task Create(TEntity entity)
    {
        Logger.Info($"[Repository] Create {typeof(TEntity)}");
        await dbSet.AddAsync(entity);
    }

    public virtual async Task Delete(TEntity entity)
    {
        Logger.Info($"[Repository] Delete {typeof(TEntity)}");
        dbSet.Remove(entity);
    }

    public async Task SaveAsync()
    {
        Logger.Info($"[Repository] SaveAsync {typeof(TEntity)}");
        await context.SaveChangesAsync();
    }

    public virtual async Task DeleteById(int id)
    {
        var entity = dbSet.Find(id);
        if (entity == null)
        {
            return;
        }
        dbSet.Remove(entity);
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        Logger.Info($"[Repository] Update {typeof(TEntity)}");
        dbSet.Update(entity);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        try
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Logger.Info($"[Repository] Dispose {typeof(TEntity)}");
                    context.Dispose();
                }
            }

            this.disposed = true;
        }
        catch (Exception e)
        {
            Logger.Fatal($"[Repository] Dispose {typeof(TEntity)}");
            throw;
        }
    }
}