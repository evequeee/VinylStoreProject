using ProjectVinylStore.DataAccess.Interfaces;
using ProjectVinylStore.DataAccess.Repositories;

namespace ProjectVinylStore.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IAlbumRepository Albums { get; private set; }
        public IVinylRecordRepository VinylRecords { get; private set; }
        public IUserRepository Users { get; private set; }
        public IOrderRepository Orders { get; private set; }

        private bool _disposed;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Albums = new AlbumRepository(_context);
            VinylRecords = new VinylRecordRepository(_context);
            Users = new UserRepository(_context);
            Orders = new OrderRepository(_context);
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}