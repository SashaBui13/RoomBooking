using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Domain.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public decimal BasePricePerHour { get; set; }

        public ICollection<RoomService> AvailableServices { get; set; } = new List<RoomService>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
}
