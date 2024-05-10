using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Interfaces.InterfaceUnitOfWork
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenericRepo<T> Repository<T>() where T : class;

        Task<int> CompleteAsync();

    }
}
