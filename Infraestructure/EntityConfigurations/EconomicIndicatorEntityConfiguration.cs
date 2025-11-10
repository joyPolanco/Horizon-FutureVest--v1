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
  
    public class EconomicIndicatorEntityConfiguration : IEntityTypeConfiguration<EconomicIndicator>
    {
        public void Configure(EntityTypeBuilder<EconomicIndicator> builder)
        {
            #region Basic 
            builder.HasKey(ei=> ei.Id);
            builder.Property(ei => ei.Year).IsRequired();
            builder.Property(ei => ei.Value).IsRequired();

            #endregion

            #region Relationships
           
            #endregion
        }
    }
}

