using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Domain.Entities
{
    public class RoomService
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public int RoomId { get; set; }
        public Room Room { get; set; }
    }
}
