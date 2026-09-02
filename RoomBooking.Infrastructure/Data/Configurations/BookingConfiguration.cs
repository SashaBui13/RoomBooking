using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoomBooking.Domain.Entities;

namespace RoomBooking.Infrastructure.Data.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<BookingEntity>
{
    public void Configure(EntityTypeBuilder<BookingEntity> builder)
    {
        builder.ToTable("Bookings");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.StartTime)
            .IsRequired();

        builder.Property(b => b.EndTime)
            .IsRequired();

        builder.Property(b => b.TotalPrice)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.HasMany(b => b.SelectedServices)
            .WithOne(s => s.Booking)
            .HasForeignKey(s => s.BookingId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}