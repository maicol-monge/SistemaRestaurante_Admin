using Microsoft.AspNetCore.Mvc;

namespace SistemaRestaurante_Admin.Controllers.Mesas
{
    public class MesasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult VerGestionarMesas()
        {
            return View("/Views/Mesas/GestionarMesas.cshtml");
        }
    }
}
