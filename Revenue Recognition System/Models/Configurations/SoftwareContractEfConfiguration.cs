using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Revenue_Recognition_System.Models.Configurations;

public class SoftwareContractEfConfiguration : IEntityTypeConfiguration<SoftwareContract>
{
    public void Configure(EntityTypeBuilder<SoftwareContract> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Price).IsRequired();
        builder.Property(x => x.IsActive).IsRequired();
        builder.Property(x => x.IsPaid).IsRequired();
        builder.Property(x => x.EndDate).IsRequired();
        builder.Property(x => x.StartDate).IsRequired();
        builder.Property(x => x.SupportYears).IsRequired();
        builder.HasOne(x => x.Client).WithMany().HasForeignKey(x => x.ClientId);
        builder.HasOne(x => x.Software).WithMany().HasForeignKey(x => x.SoftwareId);
    }
}