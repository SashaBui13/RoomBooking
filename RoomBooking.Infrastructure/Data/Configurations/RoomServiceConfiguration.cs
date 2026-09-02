using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoomBooking.Domain.Entities;

namespace RoomBooking.Infrastructure.Data.Configurations;

public class RoomServiceConfiguration : IEntityTypeConfiguration<RoomServiceEntity>
{
    public void Configure(EntityTypeBuilder<RoomServiceEntity> builder)
    {
        builder.ToTable("RoomServices");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(s => s.Price)
            .HasPrecision(18, 2)
            .IsRequired();
    }
}