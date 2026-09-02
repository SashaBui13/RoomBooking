using RoomBooking.Application.DTOs;
using RoomBooking.Domain.Entities;

namespace RoomBooking.Application.Interfaces;

public interface IRoomService
{
    Task<List<RoomEntity>> GetAllRoomsAsync();
    Task<RoomEntity?> GetRoomByIdAsync(int id);
    Task<RoomEntity> CreateRoomAsync(CreateRoomDto dto);
    Task<bool> UpdateRoomAsync(int id, decimal newBasePricePerHour, string? newName = null);
    Task<bool> AddServiceToRoomAsync(int roomId, string serviceName, decimal servicePrice);
    Task<bool> DeleteRoomAsync(int id);
}