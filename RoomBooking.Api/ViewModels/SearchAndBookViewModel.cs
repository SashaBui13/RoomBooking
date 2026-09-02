using RoomBooking.Domain.Entities;

namespace RoomBooking.Web.ViewModels;

public class SearchAndBookViewModel
{
    public DateTime StartTime { get; set; } = DateTime.Today.AddHours(10);
    public DateTime EndTime { get; set; } = DateTime.Today.AddHours(14);
    public int RequiredCapacity { get; set; } = 10;

    public int? SelectedRoomId { get; set; }
    public List<int> SelectedServiceIds { get; set; } = new();

    public List<RoomEntity> AvailableRooms { get; set; } = new();
}