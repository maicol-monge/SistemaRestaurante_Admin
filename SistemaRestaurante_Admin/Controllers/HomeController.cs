using Microsoft.AspNetCore.Mvc;
using Sistema_de_Restaurante___Modulo_de_Administracion.Models;
using SistemaRestaurante_Admin.Models;
using SistemaRestaurante_Admin.Models.Servicios;
using System.Diagnostics;

namespace SistemaRestaurante_Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var usuarioId = HttpContext.Session.GetString("UsuarioId");
            if (usuarioId != null)
            {
                return RedirectToAction("VerMenuAdmin", "Home");
            }
            return RedirectToAction("Login", "Account");
        }

        [Authentication]
        public IActionResult VerMenuAdmin()
        {
            return View("/Views/Home/MenuAdmin.cshtml");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
