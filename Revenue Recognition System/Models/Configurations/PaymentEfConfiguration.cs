using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Revenue_Recognition_System.Models.Configurations;

public class PaymentEfConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Amount).IsRequired();
        builder.HasOne(x => x.Contract).WithMany().HasForeignKey(x => x.ContractId);
        builder.Property(x => x.IsReturned).IsRequired();
        builder.Property(x => x.PaymentDate).IsRequired();
    }
}