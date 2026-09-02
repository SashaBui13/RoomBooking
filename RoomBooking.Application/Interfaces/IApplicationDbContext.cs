using Microsoft.EntityFrameworkCore;
using RoomBooking.Domain.Entities;
using System.Collections.Generic;

namespace RoomBooking.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<RoomEntity> Rooms { get; }
    DbSet<RoomServiceEntity> RoomServices { get; }
    DbSet<BookingEntity> Bookings { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}