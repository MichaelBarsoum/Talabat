using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Interfaces;
using Talabat.Core.Interfaces.InterfaceUnitOfWork;
using Talabat.Repository.Contexts;

namespace Talabat.Repository.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TalabatContext _dbContext;
        private Hashtable Repositories; // hashtable [Key => Object] and [Value => Object]
        public UnitOfWork(TalabatContext dbContext)
        {
            _dbContext = dbContext;
            Repositories = new Hashtable();
        }
        public async Task<int> CompleteAsync() => await _dbContext.SaveChangesAsync();
        public async ValueTask DisposeAsync() => await _dbContext.DisposeAsync();

        public IGenericRepo<T> Repository<T>() where T : class
        {
            var type = typeof(T).Name; // key => T
            if (!Repositories.ContainsKey(type))
            {
                var Repo = new GenericRepo<T>(_dbContext);
                Repositories.Add(type, Repo);
            }
            return Repositories[type] as IGenericRepo<T>;
        }
    }
}
