using Microsoft.EntityFrameworkCore;
using RoomBooking.Application.Interfaces;
using RoomBooking.Domain.Entities;

namespace RoomBooking.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<RoomEntity> Rooms => Set<RoomEntity>();
    public DbSet<RoomServiceEntity> RoomServices => Set<RoomServiceEntity>();
    public DbSet<BookingEntity> Bookings => Set<BookingEntity>();
    public DbSet<BookingSelectedServiceEntity> BookingSelectedServices => Set<BookingSelectedServiceEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<RoomEntity>()
            .Property(r => r.BasePricePerHour)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<RoomServiceEntity>()
            .Property(s => s.Price)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<BookingEntity>()
            .Property(b => b.TotalPrice)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<RoomServiceEntity>()
            .HasOne(s => s.Room)
            .WithMany(r => r.AvailableServices)
            .HasForeignKey(s => s.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BookingEntity>()
            .HasOne(b => b.Room)
            .WithMany()
            .HasForeignKey(b => b.RoomId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}