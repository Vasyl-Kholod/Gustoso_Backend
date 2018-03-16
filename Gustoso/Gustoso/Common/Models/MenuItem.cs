using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gustoso.Common.Models
{
    public class MenuItem
    {
        public int id { get; set; }
        public string dish { get; set; }
        public string ingredients { get; set; }
        public string weight { get; set; }
        public string price { get; set; }

        public MenuItem()
        {

        }
    }
}
