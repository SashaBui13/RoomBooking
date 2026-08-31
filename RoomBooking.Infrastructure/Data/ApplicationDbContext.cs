using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RoomBooking.Domain.Entities;

namespace RoomBooking.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<RoomService> RoomServices => Set<RoomService>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<BookingSelectedService> BookingSelectedServices => Set<BookingSelectedService>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}