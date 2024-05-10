using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Specifications;

namespace Talabat.Core.Interfaces
{
    public interface IGenericRepo<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAllAsync(ISpecification<T> Spec);
        Task<T> GetEntityWithSpecAsync(ISpecification<T> Spec);
        Task<int> GetCountWithSpecAsync(ISpecification<T> Spec);
        Task AddAsync(T item);
        void Delete(T item);
        void UpdateAsync(T item);
    }
}
