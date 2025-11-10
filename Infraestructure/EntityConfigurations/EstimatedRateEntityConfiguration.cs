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
    public class EstimatedRateEntityConfiguration : IEntityTypeConfiguration<EstimatedRate>
    {
        public void Configure(EntityTypeBuilder<EstimatedRate> builder)
        {
            builder.HasKey(er => er.Id);
            builder.Property(er => er.MinRate).IsRequired();
            builder.Property(er => er.MaxRate).IsRequired();

            builder.HasData(new EstimatedRate
            {
                Id=1,
                MaxRate=15,
                MinRate=2
            });
        }
    }
}
