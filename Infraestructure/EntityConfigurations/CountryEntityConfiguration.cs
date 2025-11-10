using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.EntityConfigurations
{
    public class CountryEntityConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            #region Basic 
            builder. HasKey(c=>c.Id);
            builder.HasIndex(c => c.IsoCode).IsUnique();
            builder.Property(c => c.IsoCode).IsRequired().HasMaxLength(3);
            builder.Property(c => c.Name).IsRequired().HasMaxLength(50);
            
            #endregion

            #region Relationships
            builder.HasMany(c => c.EconomicIndicators)
                .WithOne(c => c.Country)
                .HasForeignKey(ei=> ei.CountryId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion
        }
    }
}
