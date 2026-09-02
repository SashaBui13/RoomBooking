using Microsoft.EntityFrameworkCore;
using RoomBooking.Application.DTOs;
using RoomBooking.Application.Interfaces;
using RoomBooking.Domain.Entities;
using RoomBooking.Domain.Interfaces;

namespace RoomBooking.Application.Services;

public class BookingService : IBookingService
{
    private readonly IApplicationDbContext _context;
    private readonly IBookingPriceCalculator _priceCalculator;

    public BookingService(IApplicationDbContext context, IBookingPriceCalculator priceCalculator)
    {
        _context = context;
        _priceCalculator = priceCalculator;
    }

    public async Task<List<RoomEntity>> GetAvailableRoomsAsync(DateTime start, DateTime end, int capacity)
    {
        //ID залів, які зайняті
        var busyRoomIds = await _context.Bookings
            .Where(b => b.StartTime < end && b.EndTime > start)
            .Select(b => b.RoomId)
            .Distinct()
            .ToListAsync();

        //зали з потрібною місткістю, які не зайняті
        return await _context.Rooms
            .Include(r => r.AvailableServices)
            .Where(r => r.Capacity >= capacity && !busyRoomIds.Contains(r.Id))
            .ToListAsync();
    }

    public async Task<BookingDto> CreateBookingAsync(CreateBookingDto dto)
    {
        var room = await _context.Rooms
            .Include(r => r.AvailableServices)
            .FirstOrDefaultAsync(r => r.Id == dto.RoomId)
            ?? throw new InvalidOperationException("Зал не знайдено.");

        if (room.Capacity < dto.AttendeesCount)
            throw new InvalidOperationException("Кількість людей перевищує місткість залу.");

        bool isBusy = await _context.Bookings.AnyAsync(b =>
            b.RoomId == dto.RoomId && b.StartTime < dto.EndTime && b.EndTime > dto.StartTime);

        if (isBusy)
            throw new InvalidOperationException("Зал вже заброньований на цей час.");

        var selectedServices = room.AvailableServices
            .Where(s => dto.SelectedServiceIds.Contains(s.Id))
            .ToList();

        decimal totalPrice = _priceCalculator.CalculateTotalPrice(
            room, dto.StartTime, dto.EndTime, selectedServices);

        var booking = new BookingEntity
        {
            RoomId = dto.RoomId,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            AttendeesCount = dto.AttendeesCount,
            TotalPrice = totalPrice
        };

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        return new BookingDto
        {
            Id = booking.Id,
            RoomId = room.Id,
            RoomName = room.Name,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            AttendeesCount = booking.AttendeesCount,
            TotalPrice = booking.TotalPrice,
            SelectedServices = selectedServices.Select(s => s.Name).ToList()
        };
    }

    public async Task<List<BookingDto>> GetAllBookingsAsync()
    {
        var bookings = await _context.Bookings
            .Include(b => b.Room)
            .OrderByDescending(b => b.StartTime)
            .ToListAsync();

        return bookings.Select(b => new BookingDto
        {
            Id = b.Id,
            RoomId = b.RoomId,
            RoomName = b.Room?.Name ?? "Невідомо",
            StartTime = b.StartTime,
            EndTime = b.EndTime,
            AttendeesCount = b.AttendeesCount,
            TotalPrice = b.TotalPrice
        }).ToList();
    }

    public async Task<bool> CancelBookingAsync(int bookingId)
    {
        var booking = await _context.Bookings.FindAsync(bookingId);
        if (booking == null) return false;

        _context.Bookings.Remove(booking);
        await _context.SaveChangesAsync();
        return true;
    }
}