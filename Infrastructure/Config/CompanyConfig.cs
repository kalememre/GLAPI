using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config
{
    public class CompanyConfig : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("COMPANY", t =>
            {
                t.HasCheckConstraint("CK_Company_Isin", "\"Isin\" ~ '^[A-Z]{2}[A-Z0-9]{9}[0-9]$'");
            });
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.StockTicker).IsRequired();
            builder.Property(x => x.Exchange).IsRequired();
            builder.Property(x => x.Isin).IsRequired().HasMaxLength(12);
            builder.HasIndex(x => x.Isin).IsUnique();
        }
    }
}