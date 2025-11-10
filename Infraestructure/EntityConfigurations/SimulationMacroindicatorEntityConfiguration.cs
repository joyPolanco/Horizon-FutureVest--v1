using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.EntityConfigurations
{
    public class SimulationMacroindicatorEntityConfiguration : IEntityTypeConfiguration<SimulationMacroIndicator>
    {
        public void Configure(EntityTypeBuilder<SimulationMacroIndicator> builder)
        {
            builder.HasKey(sm => sm.Id);

            builder.Property(sm => sm.WeightFactor).IsRequired();

            builder.HasOne(s => s.RealMacroIndicator)
             .WithOne(m => m.SimulationMacroIndicator)
             .HasForeignKey<SimulationMacroIndicator>(s => s.RealMacroIndicatorId)
             .OnDelete(DeleteBehavior.SetNull);


        }
    }
}
