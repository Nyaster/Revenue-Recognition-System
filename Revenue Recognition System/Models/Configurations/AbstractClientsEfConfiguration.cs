using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Revenue_Recognition_System.Models.Configurations;

public class AbstractClientsEfConfiguration : IEntityTypeConfiguration<AbstractClient>
{
    public void Configure(EntityTypeBuilder<AbstractClient> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.PhoneNumber).IsRequired();
        builder.Property(x => x.Adress).IsRequired();
        builder.Property(x => x.Email).IsRequired();
    }
}