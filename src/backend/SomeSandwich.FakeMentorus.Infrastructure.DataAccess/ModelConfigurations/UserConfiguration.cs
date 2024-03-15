using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SomeSandwich.FakeMentorus.Domain.Users;

namespace SomeSandwich.FakeMentorus.Infrastructure.DataAccess.ModelConfigurations;

/// <summary>
/// Contains database model configuration for <see cref="User"/>.
/// </summary>
internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(e => e.Email, "Email");
        builder.HasIndex(e => e.NormalizedEmail, "NormalizedEmail").IsUnique();
        builder.HasIndex(e => e.RemovedAt);
        builder.Property(e => e.CreatedAt)
            .HasColumnType("timestamp")
            .HasDefaultValueSql("now() at time zone 'UTC'");
        builder.Property(e => e.UpdatedAt)
            .HasColumnType("timestamp")
            .HasDefaultValueSql("now() at time zone 'UTC'");
        builder.Property(e => e.RemovedAt)
            .HasComment("For soft-deletes")
            .HasColumnType("timestamp");

        builder.HasQueryFilter(user => user.RemovedAt == null);
    }
}
