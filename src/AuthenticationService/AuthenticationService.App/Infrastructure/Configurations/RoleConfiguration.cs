using AuthenticationService.App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthenticationService.App.Infrastructure.Configurations;

internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(r => r.Id);

        builder.HasIndex(r => r.Name).IsUnique();

        builder.Property(r => r.Name).HasMaxLength(64);

        builder.Property(r => r.Description).HasMaxLength(256);

        builder.HasData([
            new Role
            {
                Id = 1,
                Name = "Salesman",
                Description = "The man who sales staff"
            },
            new Role
            {
                Id = 2,
                Name = "Customer",
                Description = "The man who buys staff"
            },
            new Role
            {
                Id = 3,
                Name = "Admin",
                Description = "The main character"
            }
        ]);
    }
}
