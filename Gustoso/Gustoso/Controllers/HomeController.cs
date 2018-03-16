using Microsoft.AspNetCore.Mvc;

namespace Gustoso.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return Ok("Сервер успішно запустився!");
        }

        public IActionResult Error()
        {
            return Ok("Помилка!");
        }
    }
}
