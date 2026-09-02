using Microsoft.Extensions.DependencyInjection;
using RoomBooking.Application.Interfaces;
using RoomBooking.Application.Services;
using RoomBooking.Domain.Interfaces;
using RoomBooking.Domain.Services;

namespace RoomBooking.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IBookingPriceCalculator, BookingPriceCalculator>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IRoomService, RoomService>();

        return services;
    }
}