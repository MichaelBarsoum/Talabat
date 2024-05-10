using System.Text.Json;
using Talabat.Core.Models;
using Talabat.Core.Models.Order;
using Talabat.Repository.Contexts;

namespace Talabat.Repository.Helpers
{
    public static class DataSeeding
    {
        public static async Task SeedAsync(TalabatContext dbcontext)
        {
            if (!dbcontext.ProductBrands.Any())
            {
                var file = File.ReadAllText("../Talabat.Repository/Helpers/DataSeed/brands.json");
                var brand = JsonSerializer.Deserialize<List<ProductBrand>>(file);
                if (brand?.Count > 0)
                    foreach (var item in brand)
                        await dbcontext.Set<ProductBrand>().AddAsync(item);

            }
            if (!dbcontext.ProductTypes.Any())
            {
                var data = File.ReadAllText("../Talabat.Repository/Helpers/DataSeed/types.json");
                var typedata = JsonSerializer.Deserialize<List<ProductType>>(data);
                if (typedata?.Count > 0)
                    foreach (var item in typedata)
                        await dbcontext.Set<ProductType>().AddAsync(item);

            }
            if (!dbcontext.Products.Any())
            {
                string data = File.ReadAllText("../Talabat.Repository/Helpers/DataSeed/products.json");
                var productData = JsonSerializer.Deserialize<List<Product>>(data);
                if (productData?.Count > 0)
                    foreach (var item in productData)
                        await dbcontext.Set<Product>().AddAsync(item);
            }
            if (!dbcontext.DeliveryMethods.Any())
            {
                string data = File.ReadAllText("../Talabat.Repository/Helpers/DataSeed/delivery.json");
                var DeliveryData = JsonSerializer.Deserialize<List<DeliveryMethod>>(data);
                if (DeliveryData?.Count > 0)
                    foreach (var item in DeliveryData)
                        await dbcontext.Set<DeliveryMethod>().AddAsync(item);
            }

            await dbcontext.SaveChangesAsync();
        }
    }
}
