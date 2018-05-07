using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gustoso.Common.Models
{
    public class ReservationStatus
    {
        public int id { get; set; }
        public int id_reservation { get; set; }
        public string status { get; set; }
    }
}
