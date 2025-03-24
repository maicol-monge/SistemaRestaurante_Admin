using Microsoft.AspNetCore.Mvc;

namespace SistemaRestaurante_Admin.Controllers.Promociones
{
    public class PromocionesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult VerPromociones()
        {
            return View("/Views/Promociones/CrearPromociones.cshtml");
        }
    }
}
