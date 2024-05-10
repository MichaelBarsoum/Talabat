using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;

namespace Talabat.Repository.Configurations
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(P => P.barnd).WithMany().HasForeignKey(P => P.ProductBrandId);
            builder.HasOne(P => P.Producttype).WithMany().HasForeignKey(P => P.ProductTypeId);
            builder.Property(P=>P.Name).IsRequired();
            builder.Property(P=>P.Description).IsRequired();
            builder.Property(P=>P.PictureUrl).IsRequired();
            builder.Property(P => P.Price).HasColumnType("decimal(18,2)");
        }
    }
}
