using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Domain.Entities
{
    public class BookingEntity
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public RoomEntity Room { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int AttendeesCount { get; set; }
        public decimal TotalPrice { get; set; }

        public ICollection<BookingSelectedServiceEntity> SelectedServices { get; set; } = new List<BookingSelectedServiceEntity>();
    }
}
