using Microsoft.EntityFrameworkCore;
using RoomBooking.Application.DTOs;
using RoomBooking.Application.Interfaces;
using RoomBooking.Domain.Entities;

namespace RoomBooking.Application.Services;

public class RoomService : IRoomService
{
    private readonly IApplicationDbContext _context;

    public RoomService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<RoomEntity>> GetAllRoomsAsync()
    {
        return await _context.Rooms
            .Include(r => r.AvailableServices)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<RoomEntity?> GetRoomByIdAsync(int id)
    {
        return await _context.Rooms
            .Include(r => r.AvailableServices)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<RoomEntity> CreateRoomAsync(CreateRoomDto dto)
    {
        var room = new RoomEntity
        {
            Name = dto.Name,
            Capacity = dto.Capacity,
            BasePricePerHour = dto.BasePricePerHour,
            AvailableServices = dto.Services.Select(s => new RoomServiceEntity
            {
                Name = s.Name,
                Price = s.Price
            }).ToList()
        };

        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();
        return room;
    }

    public async Task<bool> UpdateRoomAsync(int id, decimal newBasePricePerHour, string? newName = null)
    {
        var room = await _context.Rooms.FindAsync(id);
        if (room == null) return false;

        room.BasePricePerHour = newBasePricePerHour;
        if (!string.IsNullOrWhiteSpace(newName))
        {
            room.Name = newName;
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddServiceToRoomAsync(int roomId, string serviceName, decimal servicePrice)
    {
        var room = await _context.Rooms.FindAsync(roomId);
        if (room == null) return false;

        var service = new RoomServiceEntity
        {
            Name = serviceName,
            Price = servicePrice,
            RoomId = roomId
        };

        _context.RoomServices.Add(service);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteRoomAsync(int id)
    {
        var room = await _context.Rooms.FindAsync(id);
        if (room == null) return false;

        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync();
        return true;
    }
}