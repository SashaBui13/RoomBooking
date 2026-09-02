using Microsoft.AspNetCore.Mvc;
using RoomBooking.Application.DTOs;
using RoomBooking.Application.Interfaces;

namespace RoomBooking.Api.Controllers;

public class RoomsController : Controller
{
    private readonly IRoomService _roomService;

    public RoomsController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    public async Task<IActionResult> Index()
    {
        var rooms = await _roomService.GetAllRoomsAsync();
        return View(rooms);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateRoomDto dto)
    {
        if (ModelState.IsValid)
        {
            await _roomService.CreateRoomAsync(dto);
            return RedirectToAction(nameof(Index));
        }
        return View(dto);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var room = await _roomService.GetRoomByIdAsync(id);
        if (room == null) return NotFound();

        return View(room);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, decimal basePricePerHour, string name)
    {
        var result = await _roomService.UpdateRoomAsync(id, basePricePerHour, name);
        if (!result) return NotFound();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddService(int roomId, string serviceName, decimal servicePrice)
    {
        await _roomService.AddServiceToRoomAsync(roomId, serviceName, servicePrice);
        return RedirectToAction(nameof(Edit), new { id = roomId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _roomService.DeleteRoomAsync(id);
        return RedirectToAction(nameof(Index));
    }
}