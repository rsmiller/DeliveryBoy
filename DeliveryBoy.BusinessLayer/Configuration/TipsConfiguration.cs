using DeliveryBoy.BusinessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text;

namespace ContentCreatorSearch.Business.Configuration
{
    internal class TipsConfiguration : IEntityTypeConfiguration<Tip>
    {
        public void Configure(EntityTypeBuilder<Tip> builder)
        {
            builder.ToTable("tips");
            builder.HasKey(x => x.Id);
        }
    }
}
