using FinalidadeEstudo.Domain.Entities;
using FinalidadeEstudo.Domain.ValueObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinalidadeEstudo.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.HasIndex(x => x.CpfCnpj)
        .IsUnique();

        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Email)
            .HasConversion(
                email => email.Value,
                value => Email.Create(value)
            )
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(u => u.Password)
            .IsRequired();

        builder.Property(u => u.CreatedAt)
            .IsRequired();

        builder.Property(u => u.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(u => u.CpfCnpj)
            .HasConversion(
                cpfCnpj => cpfCnpj.Value,
                value => CpfCnpj.Create(value)
            )
            .IsRequired()
            .HasMaxLength(14);

        builder.Property(u => u.DateBirth)
            .IsRequired();
    }
}
