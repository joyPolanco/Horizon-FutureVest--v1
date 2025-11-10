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
    public class MacroEconomicIndicatorEntityConfiguration : IEntityTypeConfiguration<MacroeconomicIndicator>
    {
        public void Configure(EntityTypeBuilder<MacroeconomicIndicator> builder)
        {
            #region Basic 
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Name).IsRequired();
            builder.Property(m => m.WeightFactor).IsRequired().HasMaxLength(1);
            builder.Property(m => m.HigherIsBetter).IsRequired();
            #endregion

            #region Relationships
            builder.HasMany(mi => mi.EconomicIndicators)
                .WithOne(ei => ei.MacroeconomicIndicator)
                .HasForeignKey(mi => mi.MacroeconomicIndicatorId)
                .OnDelete(DeleteBehavior.Cascade);
               
            builder.HasOne(mi => mi.SimulationMacroIndicator)   
        .WithOne(sm => sm.RealMacroIndicator)        
        .HasForeignKey<SimulationMacroIndicator>(sm => sm.RealMacroIndicatorId)
        .OnDelete(DeleteBehavior.Cascade);

            #endregion
        }
    }
}

