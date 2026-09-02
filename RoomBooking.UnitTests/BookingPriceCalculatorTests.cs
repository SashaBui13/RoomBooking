using RoomBooking.Domain.Entities;
using RoomBooking.Domain.Services;
using Xunit;

namespace RoomBooking.UnitTests;

public class BookingPriceCalculatorTests
{
    private readonly BookingPriceCalculator _calculator = new();

    [Fact]
    public void CalculateRentalPrice_StandardHours_CalculatesExactBasePrice()
    {
        var start = new DateTime(2026, 10, 1, 10, 0, 0);
        var end = new DateTime(2026, 10, 1, 12, 0, 0);
        decimal basePricePerHour = 1000m;

        var price = _calculator.CalculateRentalPrice(basePricePerHour, start, end);

        Assert.Equal(2000m, price);
    }

    [Fact]
    public void CalculateRentalPrice_PeakHours_Applies15PercentMarkup()
    {
        var start = new DateTime(2026, 10, 1, 12, 0, 0);
        var end = new DateTime(2026, 10, 1, 14, 0, 0);
        decimal basePricePerHour = 1000m;

        var price = _calculator.CalculateRentalPrice(basePricePerHour, start, end);

        Assert.Equal(2300m, price);
    }

    [Fact]
    public void CalculateRentalPrice_MorningHours_Applies10PercentDiscount()
    {
        var start = new DateTime(2026, 10, 1, 7, 0, 0);
        var end = new DateTime(2026, 10, 1, 9, 0, 0);
        decimal basePricePerHour = 1000m;

        var price = _calculator.CalculateRentalPrice(basePricePerHour, start, end);

        Assert.Equal(1800m, price);
    }

    [Fact]
    public void CalculateTotalPrice_IncludesRentalAndAdditionalServices()
    {
        var room = new RoomEntity
        {
            Id = 1,
            Name = "Зал A",
            Capacity = 50,
            BasePricePerHour = 2000m
        };

        var start = new DateTime(2026, 10, 1, 10, 0, 0);
        var end = new DateTime(2026, 10, 1, 11, 0, 0);

        var services = new List<RoomServiceEntity>
        {
            new() { Id = 1, Name = "Проєктор", Price = 500m },
            new() { Id = 2, Name = "Wi-Fi", Price = 300m }
        };

        var totalPrice = _calculator.CalculateTotalPrice(room, start, end, services);

        Assert.Equal(2800m, totalPrice);
    }

    [Fact]
    public void CalculateRentalPrice_ThrowsException_WhenEndTimeBeforeStartTime()
    {
        var start = new DateTime(2026, 10, 1, 12, 0, 0);
        var end = new DateTime(2026, 10, 1, 10, 0, 0);

        Assert.Throws<ArgumentException>(() =>
            _calculator.CalculateRentalPrice(1000m, start, end));
    }

    [Fact]
    public void CalculateRentalPrice_EveningHours_Applies20PercentDiscount()
    {
        var start = new DateTime(2026, 10, 1, 19, 0, 0);
        var end = new DateTime(2026, 10, 1, 21, 0, 0);
        decimal basePricePerHour = 1000m;

        var price = _calculator.CalculateRentalPrice(basePricePerHour, start, end);

        Assert.Equal(1600m, price);
    }

    [Fact]
    public void CalculateRentalPrice_MixedInterval_CalculatesEachHourCorrectly()
    {
        var start = new DateTime(2026, 10, 1, 11, 0, 0);
        var end = new DateTime(2026, 10, 1, 13, 0, 0);
        decimal basePricePerHour = 1000m;

        var price = _calculator.CalculateRentalPrice(basePricePerHour, start, end);

        Assert.Equal(2150m, price);
    }
}