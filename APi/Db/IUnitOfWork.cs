using Tourplanner.Entities;
using Tourplanner.Repositories;
using Tourplanner.Services;
using Api.Services.Logging;
using Microsoft.EntityFrameworkCore.Storage;
using LoggerFactory = Api.Services.Logging.LoggerFactory;

namespace Tourplanner.Db
{
    public interface IUnitOfWork : IDisposable
    {
        public ITourRepository TourRepository { get; }
        public IMapRepository MapRepository { get; }
        public ITileRepository TileRepository { get; }
        public ITourLogRepository TourLogRepository { get; }
        Task CommitAsync();
        Task RollbackAsync();
        Task RollbackAsync(Exception ex);
        public Task BeginTransactionAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly TourContext _tourContext;
        private readonly IImageService _imageService;
        private ITourRepository? _tourRepository;
        private IMapRepository? _mapRepository;
        private ITileRepository? _tileRepository;
        private ITourLogRepository? _tourLogRepository;
        protected readonly ILoggerWrapper Logger = LoggerFactory.GetLogger();
        private IDbContextTransaction? _transaction = null;

        public UnitOfWork(TourContext tourContext, IImageService imageService)
        {
            _tourContext = tourContext;
            _imageService = imageService;
        }
        public ITourRepository TourRepository => _tourRepository ??= new TourRepository(_tourContext);
        public IMapRepository MapRepository => _mapRepository ??= new MapRepository(_tourContext, _imageService);
        public ITileRepository TileRepository => _tileRepository ??= new TileRepository(_tourContext, _imageService);
        public ITourLogRepository TourLogRepository => _tourLogRepository??= new TourLogRepository(_tourContext);

        public async Task BeginTransactionAsync()
        {
            _transaction ??= await _tourContext.Database.BeginTransactionAsync();
        }
        public async Task CommitAsync()
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("Transaction has not been started.");
            }

            try
            {
                await _tourContext.SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            catch (Exception)
            {
                await RollbackAsync();
                throw;
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackAsync()
        {
            Logger.Error("Rolling back transaction");
            await _tourContext.DisposeAsync();
        }
        
        public async Task RollbackAsync(Exception ex)
        {
            Logger.Error($"Rolling back transaction. Exception {ex.Message}");
            await _tourContext.DisposeAsync();
        }

        public void Dispose()
        {
            _tourContext.Dispose();
        }
    }
}