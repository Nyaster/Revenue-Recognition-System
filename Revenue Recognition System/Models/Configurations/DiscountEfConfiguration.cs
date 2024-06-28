using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Revenue_Recognition_System.Models.Configurations;

public class DiscountEfConfiguration : IEntityTypeConfiguration<Discount>
{
    public void Configure(EntityTypeBuilder<Discount> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.StartDate).IsRequired();
        builder.Property(x => x.EndDate).IsRequired();
        builder.Property(x => x.Percentage).IsRequired();
        builder.Property(x => x.AppliesToSubscription).IsRequired();
        //Some data for you
        List<Discount> TestData = new List<Discount>();
        TestData.Add(new Discount()
        {
            Id = 100,
            Name = "Black Friday",
            AppliesToSubscription = false,
            Percentage = 10,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddMonths(3),
        });
        TestData.Add(new Discount()
        {
            Id = 101,
            Name = "Black Friday1",
            AppliesToSubscription = false,
            Percentage = 20,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddMonths(3),
        });
        builder.HasData(TestData);
    }
}