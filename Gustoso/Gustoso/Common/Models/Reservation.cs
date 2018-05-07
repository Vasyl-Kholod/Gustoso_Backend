using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gustoso.Common.Models
{
    public class Reservation
    {
        public int id { get; set; }
        public string clientName { get; set; }
        public string clientPhone { get; set; }
        public string clientEmail { get; set; }
        public int tableNumber { get; set; }
        public DateTime dateOfReservation { get; set; }
        public Boolean isConfirmed { get; set; }

        public Reservation()
        {

        }

    }
}
