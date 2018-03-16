using System.ComponentModel.DataAnnotations;
using Gustoso.Common.Interfaces;

namespace Gustoso.Common.DTO.Request
{
    public class LoginDTO: ILoginDTO
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
