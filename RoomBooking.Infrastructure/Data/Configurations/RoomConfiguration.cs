using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoomBooking.Domain.Entities;

namespace RoomBooking.Infrastructure.Data.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable("Rooms");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(r => r.Capacity)
            .IsRequired();

        builder.Property(r => r.BasePricePerHour)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.HasMany(r => r.AvailableServices)
            .WithOne(s => s.Room)
            .HasForeignKey(s => s.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(r => r.Bookings)
            .WithOne(b => b.Room)
            .HasForeignKey(b => b.RoomId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}