using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        await Task.CompletedTask;
    }
}
