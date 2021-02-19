using DeliveryBoy.BusinessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text;

namespace ContentCreatorSearch.Business.Configuration
{
    internal class TipDetailConfiguration : IEntityTypeConfiguration<TipDetail>
    {
        public void Configure(EntityTypeBuilder<TipDetail> builder)
        {
            builder.ToTable("tipdetails");
            builder.HasKey(x => x.Id);
        }
    }
}
