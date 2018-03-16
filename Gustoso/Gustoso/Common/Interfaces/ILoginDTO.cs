using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gustoso.Common.Interfaces
{
    public interface ILoginDTO
    {
        string Login { get; set; }

        string Password { get; set; }
    }
}
