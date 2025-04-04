using Microsoft.AspNetCore.Mvc;

namespace SistemaRestaurante_Admin.Controllers.Combos
{
    public class CombosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult VerCombos()
        {
            return View("/Views/Combos/CrearCombos.cshtml");
        }
    }
}
