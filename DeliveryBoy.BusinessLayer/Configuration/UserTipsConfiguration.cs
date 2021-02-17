using DeliveryBoy.BusinessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text;

namespace ContentCreatorSearch.Business.Configuration
{
    internal class UserTipsConfiguration : IEntityTypeConfiguration<UserTip>
    {
        public void Configure(EntityTypeBuilder<UserTip> builder)
        {
            builder.ToTable("usertips");
            builder.HasKey(x => x.Id);
        }
    }
}
