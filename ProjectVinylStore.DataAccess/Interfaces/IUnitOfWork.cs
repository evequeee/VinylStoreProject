using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectVinylStore.DataAccess.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAlbumRepository Albums { get; }
        IVinylRecordRepository VinylRecords { get; }
        IUserRepository Users { get; }
        IOrderRepository Orders { get; }

        Task<int> CommitAsync();
    }
}
