 using Microsoft.AspNetCore.Mvc;

namespace SistemaRestaurante_Admin.Controllers.Cargos
{
    public class CargosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CrearCargos()
        {
            return View();
        }
    }
}
