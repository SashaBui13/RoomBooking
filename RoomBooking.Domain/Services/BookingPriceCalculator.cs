using RoomBooking.Domain.Entities;
using RoomBooking.Domain.Interfaces;

namespace RoomBooking.Domain.Services;

public class BookingPriceCalculator : IBookingPriceCalculator
{
    public decimal CalculateRentalPrice(decimal basePricePerHour, DateTime startTime, DateTime endTime)
    {
        if (endTime <= startTime)
        {
            throw new ArgumentException("Час завершення повинен бути пізнішим за час початку.");
        }

        if (basePricePerHour < 0)
        {
            throw new ArgumentException("Базова ставка не може бути від'ємною.");
        }

        decimal totalRentalPrice = 0m;
        var currentSlot = startTime;

        while (currentSlot < endTime)
        {
            var nextSlot = currentSlot.AddMinutes(1);
            if (nextSlot > endTime)
            {
                nextSlot = endTime;
            }

            var durationInHours = (decimal)(nextSlot - currentSlot).TotalHours;
            var hour = currentSlot.Hour;

            decimal multiplier = hour switch
            {
                >= 6 and < 9 => 0.90m,    //знижка 10%
                >= 12 and < 14 => 1.15m,  //націнка 15%
                >= 9 and < 18 => 1.00m,   //100%
                >= 18 and < 23 => 0.80m,  //знижка 20%
                _ => 1.00m                //нічні
            };

            totalRentalPrice += basePricePerHour * multiplier * durationInHours;
            currentSlot = nextSlot;
        }

        return Math.Round(totalRentalPrice, 2, MidpointRounding.AwayFromZero);
    }

    public decimal CalculateTotalPrice(RoomEntity room, DateTime startTime, DateTime endTime, IEnumerable<RoomServiceEntity> selectedServices)
    {
        ArgumentNullException.ThrowIfNull(room);

        var rentalPrice = CalculateRentalPrice(room.BasePricePerHour, startTime, endTime);
        var servicesPrice = selectedServices?.Sum(s => s.Price) ?? 0m;

        return Math.Round(rentalPrice + servicesPrice, 2, MidpointRounding.AwayFromZero);
    }
}