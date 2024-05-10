using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using Talabat.Core.Interfaces;
using Talabat.Core.Specifications;
using Talabat.Repository.Contexts;
using Talabat.Repository.SpecificationEvaluator;

namespace Talabat.Repository.Repositories
{
    public class GenericRepo<T> : IGenericRepo<T> where T : class
    {
        private readonly TalabatContext _dbContext;
        public GenericRepo(TalabatContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<T> GetByIdAsync(int id) => await _dbContext.Set<T>().FindAsync(id);
        public async Task<IReadOnlyList<T>> GetAllAsync() => await _dbContext.Set<T>().ToListAsync();
        public async Task<IReadOnlyList<T>> GetAllAsync(ISpecification<T> Spec) => await ApplySpecification(Spec).ToListAsync();
        public async Task<T> GetEntityWithSpecAsync(ISpecification<T> Spec) => await ApplySpecification(Spec).FirstOrDefaultAsync();
        public async Task<int> GetCountWithSpecAsync(ISpecification<T> Spec) => await ApplySpecification(Spec).CountAsync();
        private IQueryable<T> ApplySpecification(ISpecification<T> Spec) => SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), Spec);
        public async Task AddAsync(T item) => await _dbContext.Set<T>().AddAsync(item);
        public void Delete(T item) => _dbContext.Set<T>().Remove(item);
        public void UpdateAsync(T item) => _dbContext.Set<T>().Update(item);
    }
}
