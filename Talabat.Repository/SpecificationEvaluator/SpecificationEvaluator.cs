using Microsoft.EntityFrameworkCore;
using Talabat.Core.Specifications;

namespace Talabat.Repository.SpecificationEvaluator
{
    public static class SpecificationEvaluator<T> where T : class
    {
        public static IQueryable<T> GetQuery(IQueryable<T> EntryPoint, ISpecification<T> spec)
        {
            var Query = EntryPoint; // _TalabatDbContext.Set<T>
            if (spec.Criteria != null) // spec hold criteria (Condition) and list of includes
                Query = Query.Where(spec.Criteria); // _TalabatDbContext.Set<T>.where(condition)
            if (spec.OrderBy is not null)
                Query = Query.OrderBy(spec.OrderBy);
            if (spec.OrderByDescending is not null)
                Query = Query.OrderByDescending(spec.OrderByDescending);
            if (spec.IsPaginationEnabled)
                Query = Query.Skip(spec.Skip).Take(spec.Take);
            Query = spec.includes.Aggregate(Query, (CurrentQuery, NextQuery) => CurrentQuery.Include(NextQuery));
            return Query;
        }
    }
}
