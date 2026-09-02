using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Domain.Entities
{
    public class BookingSelectedServiceEntity
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public BookingEntity Booking { get; set; }

        public string ServiceName { get; set; }
        public decimal PriceSnapshot { get; set; }
    }
}
