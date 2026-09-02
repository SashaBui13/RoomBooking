using Microsoft.AspNetCore.Mvc;
using RoomBooking.Application.DTOs;
using RoomBooking.Application.Interfaces;
using RoomBooking.Web.ViewModels; 

namespace RoomBooking.Web.Controllers;

public class BookingsController : Controller
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    public async Task<IActionResult> Index()
    {
        var bookings = await _bookingService.GetAllBookingsAsync();
        return View(bookings);
    }

    [HttpGet]
    public async Task<IActionResult> Search(DateTime? startTime, DateTime? endTime, int? requiredCapacity)
    {
        var model = new SearchAndBookViewModel();

        if (startTime.HasValue && endTime.HasValue && requiredCapacity.HasValue)
        {
            model.StartTime = startTime.Value;
            model.EndTime = endTime.Value;
            model.RequiredCapacity = requiredCapacity.Value;

            model.AvailableRooms = await _bookingService.GetAvailableRoomsAsync(
                model.StartTime,
                model.EndTime,
                model.RequiredCapacity);
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Book(CreateBookingDto dto)
    {
        try
        {
            await _bookingService.CreateBookingAsync(dto);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return RedirectToAction(nameof(Search), new
            {
                startTime = dto.StartTime,
                endTime = dto.EndTime,
                requiredCapacity = dto.AttendeesCount
            });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id)
    {
        await _bookingService.CancelBookingAsync(id);
        return RedirectToAction(nameof(Index));
    }
}