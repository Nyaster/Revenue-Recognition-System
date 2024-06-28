using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Revenue_Recognition_System.Models.Configurations;

public class SoftwareEfConfiguration : IEntityTypeConfiguration<Software>
{
    public void Configure(EntityTypeBuilder<Software> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Version).IsRequired();
        builder.Property(x => x.Description).IsRequired();
        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Price);
        builder.Property(x => x.SubscriptionPrice).IsRequired();
        List<Software> softwares = new List<Software>();
        softwares.Add(new Software()
        {
            Name = "blender",
            Price = 1000,
            ReleaseDate = DateTime.Now,
            Category = "3d",
            SubscriptionPrice = 0,
            Version = "1.0.0",
            Description = "3d cool design tool"
        });
        softwares.Add(new Software()
        {
            Name = "koikatsu",
            Price = 1050,
            ReleaseDate = DateTime.Now,
            Category = "3d",
            SubscriptionPrice = 0,
            Version = "1.0.0",
            Description = "very cool program"
        });
        builder.HasData(softwares);
    }
}