using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomBooking.Application.Interfaces;

namespace RoomBooking.Web.Controllers;

public class AnalyticsController : Controller
{
    private readonly IApplicationDbContext _context;

    public AnalyticsController(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var bookings = await _context.Bookings
            .Include(b => b.Room)
            .AsNoTracking()
            .ToListAsync();

        ViewBag.TotalRevenue = bookings.Sum(b => b.TotalPrice);
        ViewBag.TotalBookings = bookings.Count;

        ViewBag.PopularRooms = bookings
            .GroupBy(b => b.Room != null ? b.Room.Name : "Невідомий зал")
            .Select(g => new
            {
                RoomName = g.Key,
                BookingCount = g.Count(),
                Revenue = g.Sum(b => b.TotalPrice)
            })
            .OrderByDescending(x => x.BookingCount)
            .ToList();

        return View();
    }
}