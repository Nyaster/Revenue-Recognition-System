using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Revenue_Recognition_System.Models.Configurations;

public class CompanyEfConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.Property(x => x.Adress).IsRequired();
        builder.Property(x => x.CompanyName).IsRequired();
        builder.Property(x => x.Email).IsRequired();
        builder.Property(x => x.PhoneNumber).IsRequired();
        builder.Property(x => x.Krs).IsRequired();
    }
}