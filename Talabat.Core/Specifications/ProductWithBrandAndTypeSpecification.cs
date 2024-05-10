using Talabat.Core.Models;

namespace Talabat.Core.Specifications
{
    public class ProductWithBrandAndTypeSpecification : Specification<Product>
    {
        public ProductWithBrandAndTypeSpecification(SpecificationParams Params)
          : base
          (P =>
          (string.IsNullOrEmpty(Params.Search) || P.Name.ToLower().Contains(Params.Search))
          &&
            (!Params.BrandId.HasValue || P.ProductBrandId == Params.BrandId)
            &&
            (!Params.TypeId.HasValue || P.ProductTypeId == Params.TypeId)
          )
        {
            includes.Add(P => P.barnd);
            includes.Add(P => P.Producttype);
            if (!string.IsNullOrEmpty(Params.Sort))
            {
                switch (Params.Sort)
                {
                    case "PriceAsc":
                        AddOrderByAscending(P => P.Price); break;
                    case "PriceDesc":
                        AddOrderByDescending(P => P.Price); break;
                    default:
                        AddOrderByAscending(P => P.Name); break;
                }
            }
            ApplyPagination(Params.PageSize * (Params.PageIndex - 1), Params.PageSize);

        }
        public ProductWithBrandAndTypeSpecification(int id) : base(P => P.Id == id)
        {
            includes.Add(P => P.barnd);
            includes.Add(P => P.Producttype);
        }
    }
}
