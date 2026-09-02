using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Domain.Entities
{
    public class RoomEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public decimal BasePricePerHour { get; set; }

        public ICollection<RoomServiceEntity> AvailableServices { get; set; } = new List<RoomServiceEntity>();
        public ICollection<BookingEntity> Bookings { get; set; } = new List<BookingEntity>();
}
}
