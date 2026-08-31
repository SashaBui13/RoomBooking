using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoomBooking.Domain.Entities;

namespace RoomBooking.Infrastructure.Data.Configurations;

public class BookingSelectedServiceConfiguration : IEntityTypeConfiguration<BookingSelectedService>
{
    public void Configure(EntityTypeBuilder<BookingSelectedService> builder)
    {
        builder.ToTable("BookingSelectedServices");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.ServiceName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(s => s.PriceSnapshot)
            .HasPrecision(18, 2)
            .IsRequired();
    }
}