namespace RoomBooking.Application.DTOs;

public class CreateBookingDto
{
    public int RoomId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int AttendeesCount { get; set; }
    public List<int> SelectedServiceIds { get; set; } = new();
}

public class BookingDto
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public string RoomName { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int AttendeesCount { get; set; }
    public decimal TotalPrice { get; set; }
    public List<string> SelectedServices { get; set; } = new();
}