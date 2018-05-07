using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gustoso.Common.DTO.Request
{
    public class ReservationDTO
    {
        public string clientName { get; set; }
        public string clientPhone { get; set; }
        public string clientEmail { get; set; }
        public int tableNumber { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }

        public ReservationDTO()
        {

        }
    }
}
