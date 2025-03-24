using Microsoft.AspNetCore.Mvc;

namespace SistemaRestaurante_Admin.Controllers.Empleados
{
    public class EmpleadosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult VerEmpleados()
        {
            return View("/Views/Empleados/DarDeAltaEmpleados.cshtml");
        }
    }
}
