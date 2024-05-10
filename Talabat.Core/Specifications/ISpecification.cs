using System.Linq.Expressions;

namespace Talabat.Core.Specifications
{
    public interface ISpecification<T> where T : class
    {
        Expression<Func<T , bool>> Criteria { get; set; }
        List<Expression<Func<T , object>>> includes { get; set; }
        Expression<Func<T , object>> OrderBy { get; set; }
        Expression<Func<T , object>> OrderByDescending { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginationEnabled { get; set; }

    }
}
