using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gustoso.Common.Interfaces
{
    public interface IRegistrationDTO
    {
        string FirstName { get; set; }

        string LastName { get; set; }

        string Email { get; set; }

        string Phone { get; set; }

        string Password { get; set; }

        string PasswordConfirm { get; set; }
    }
}
