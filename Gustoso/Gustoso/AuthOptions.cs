using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Gustoso
{
    public class AuthOptions
    {
        public const string ISSUER = "Gustoso";
        public const string AUDIENCE = "http://localhost:54334/";
        const string KEY = "gustoso_super_system!ahaha";
        public const int LIFETIME = 60 * 24; // час життя токена -  в мінутах
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
