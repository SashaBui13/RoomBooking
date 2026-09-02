using RoomBooking.Domain.Entities;

namespace RoomBooking.Domain.Interfaces;

public interface IBookingPriceCalculator
{
    decimal CalculateRentalPrice(decimal basePricePerHour, DateTime startTime, DateTime endTime);
    decimal CalculateTotalPrice(RoomEntity room, DateTime startTime, DateTime endTime, IEnumerable<RoomServiceEntity> selectedServices);
}