using Microsoft.AspNetCore.Mvc;

namespace SistemaRestaurante_Admin.Controllers.Account
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            //De momento solo redirige a la vista del menú
            return View("/Views/Home/MenuAdmin.cshtml");
        }
    }
}
