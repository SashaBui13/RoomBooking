using MockQueryable;
using MockQueryable.Moq;
using Moq;
using RoomBooking.Application.DTOs;
using RoomBooking.Application.Interfaces;
using RoomBooking.Application.Services;
using RoomBooking.Domain.Entities;
using RoomBooking.Domain.Interfaces;
using Xunit;

namespace RoomBooking.UnitTests;

public class BookingValidationTests
{
    private readonly Mock<IApplicationDbContext> _mockContext;
    private readonly Mock<IBookingPriceCalculator> _mockCalculator;
    private readonly BookingService _service;

    public BookingValidationTests()
    {
        _mockContext = new Mock<IApplicationDbContext>();
        _mockCalculator = new Mock<IBookingPriceCalculator>();
        _service = new BookingService(_mockContext.Object, _mockCalculator.Object);
    }

    [Fact]
    public async Task CreateBooking_ThrowsException_WhenCapacityExceeded()
    {
        // Зал місткістю на 30 людей
        var room = new RoomEntity
        {
            Id = 1,
            Name = "Зал C",
            Capacity = 30,
            BasePricePerHour = 1500m,
            AvailableServices = new List<RoomServiceEntity>()
        };

        var roomsList = new List<RoomEntity> { room }.BuildMockDbSet();
        var bookingsList = new List<BookingEntity>().BuildMockDbSet();

        _mockContext.Setup(c => c.Rooms).Returns(roomsList.Object);
        _mockContext.Setup(c => c.Bookings).Returns(bookingsList.Object);

        var dto = new CreateBookingDto
        {
            RoomId = 1,
            StartTime = DateTime.Now.AddHours(1),
            EndTime = DateTime.Now.AddHours(3),
            AttendeesCount = 35
        };

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.CreateBookingAsync(dto));

        Assert.Equal("Кількість людей перевищує місткість залу.", exception.Message);
    }

    [Fact]
    public async Task CreateBooking_AllowsBooking_WhenBackToBackTimes()
    {
        var room = new RoomEntity
        {
            Id = 1,
            Name = "Зал B",
            Capacity = 50,
            BasePricePerHour = 1000m,
            AvailableServices = new List<RoomServiceEntity>()
        };

        var existingBooking = new BookingEntity
        {
            Id = 1,
            RoomId = 1,
            StartTime = new DateTime(2026, 10, 1, 10, 0, 0),
            EndTime = new DateTime(2026, 10, 1, 12, 0, 0),
            AttendeesCount = 10
        };

        var roomsList = new List<RoomEntity> { room }.BuildMockDbSet();
        var bookingsList = new List<BookingEntity> { existingBooking }.BuildMockDbSet();

        _mockContext.Setup(c => c.Rooms).Returns(roomsList.Object);
        _mockContext.Setup(c => c.Bookings).Returns(bookingsList.Object);

        var backToBackDto = new CreateBookingDto
        {
            RoomId = 1,
            StartTime = new DateTime(2026, 10, 1, 12, 0, 0),
            EndTime = new DateTime(2026, 10, 1, 14, 0, 0),
            AttendeesCount = 10
        };

        var exception = await Record.ExceptionAsync(() => _service.CreateBookingAsync(backToBackDto));
        Assert.Null(exception);
    }
}