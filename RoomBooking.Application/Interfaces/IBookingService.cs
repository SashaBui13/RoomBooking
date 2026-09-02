using RoomBooking.Application.DTOs;
using RoomBooking.Domain.Entities;

namespace RoomBooking.Application.Interfaces;

public interface IBookingService
{
    Task<List<RoomEntity>> GetAvailableRoomsAsync(DateTime start, DateTime end, int capacity);
    Task<BookingDto> CreateBookingAsync(CreateBookingDto dto);
    Task<List<BookingDto>> GetAllBookingsAsync();
    Task<bool> CancelBookingAsync(int bookingId);
}