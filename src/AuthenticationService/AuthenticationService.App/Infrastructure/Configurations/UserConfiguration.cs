using AuthenticationService.App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthenticationService.App.Infrastructure.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.Surname).HasMaxLength(64);

        builder.Property(u => u.Name).HasMaxLength(64);

        builder.Property(u => u.Patronymic).HasMaxLength(64);

        builder.Property(u => u.Email).HasMaxLength(255);

        builder.Property(u => u.PasswordHash).HasMaxLength(1024);
    }
}
