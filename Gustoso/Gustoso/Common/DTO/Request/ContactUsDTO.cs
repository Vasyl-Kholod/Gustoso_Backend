using Gustoso.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gustoso.Common.DTO.Request
{
    public class ContactUsDTO: IContactUsDTO
    {
        [Required]
        public string clientName { get; set; }

        [Required]
        public string clientEmail { get; set; }

        [Required]
        public string clientSubject { get; set; }

        [Required]
        public string clientMessage { get; set; }
    }
}
