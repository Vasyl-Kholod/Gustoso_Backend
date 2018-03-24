using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gustoso.Common.Models
{
    public class Rating
    {
        public int id { get; set; }
        
        public string userName { get; set; }

        public string slideName { get; set; }

        public int ratingValue { get; set; }
    }
}
