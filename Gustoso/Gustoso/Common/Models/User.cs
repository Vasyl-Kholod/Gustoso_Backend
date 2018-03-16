using Microsoft.AspNetCore.Identity;

namespace Gustoso.Common.Models
{
    public class User: IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DateOfRegistration { get; set; }

        public string Phone { get; set; }
    }
}
