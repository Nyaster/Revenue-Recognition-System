using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Revenue_Recognition_System.Models.Configurations;

public class IndividualEfConfiguration : IEntityTypeConfiguration<Individual>
{
    public void Configure(EntityTypeBuilder<Individual> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Adress).IsRequired();
        builder.Property(x => x.FirstName).IsRequired();
        builder.Property(x => x.SecondName).IsRequired();
        builder.Property(x => x.PhoneNumber).IsRequired();
        builder.Property(x => x.IsDeleted).IsRequired();
    }
}