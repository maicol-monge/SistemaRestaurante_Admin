using Microsoft.AspNetCore.Mvc;

namespace SistemaRestaurante_Admin.Controllers.Categoria
{
    public class CategoriasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult VerCategorias()
        {
            return View("/Views/Categorias/Categoria.cshtml");
        }
    }
}
