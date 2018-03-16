using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gustoso.Common.Models
{
    public class ContactUs
    {
        public int id { get; set; }

        public string clientName { get; set; }

        public string clientEmail { get; set; }

        public string clientSubject { get; set; }

        public string clientMessage { get; set; }

        public ContactUs()
        {

        }
    }
}
