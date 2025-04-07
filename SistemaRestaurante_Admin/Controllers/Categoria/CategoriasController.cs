using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_de_Restaurante___Modulo_de_Administracion.Models;

namespace SistemaRestaurante_Admin.Controllers.Categoria
{
    public class CategoriasController : Controller
    {
        private readonly restauranteDbContext _context;

        public CategoriasController(restauranteDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult VerCategorias()
        {
            var categorias = (from c in _context.Categoria
                              where c.estado == 1
                              select c)
                              .ToList();
            return View("/Views/Categorias/Categoria.cshtml", categorias);
        }

        [HttpPost]
        public IActionResult InsertarCategoria(string nombre)
        {
            if (string.IsNullOrEmpty(nombre))
            {
                TempData["Error"] = "El nombre de la categoría es requerido.";
                return RedirectToAction("VerCategorias");
            }

            // Verificar si la categoría ya existe en la base de datos (activa o inactiva)
            var categoriaExistente = _context.Categoria.FirstOrDefault(c => c.nombre.ToLower() == nombre.ToLower());

            if (categoriaExistente != null)
            {
                if (categoriaExistente.estado == 1)
                {
                    TempData["Error"] = "La categoría ya existe y está activa.";
                }
                else
                {
                    TempData["Error"] = "La categoría ya existe pero está inactiva.";
                }
                return RedirectToAction("VerCategorias");
            }

            // Si no existe, insertar la nueva categoría
            var nuevaCategoria = new Models.Categoria
            {
                nombre = nombre,
                estado = 1  // Activar la categoría
            };

            _context.Categoria.Add(nuevaCategoria);
            _context.SaveChanges();

            TempData["Success"] = "Categoría creada exitosamente.";
            return RedirectToAction("VerCategorias");
        }

        // Acción para editar una categoría
        [HttpPost]
        public IActionResult EditarCategoria(int id, string nombre)
        {
			if (string.IsNullOrEmpty(nombre))
			{
				TempData["Error"] = "El nombre de la categoría es requerido.";
				return RedirectToAction("VerCategorias");
			}

			var categoria = _context.Categoria.FirstOrDefault(c => c.id == id);

            if (categoria == null)
            {
                TempData["Error"] = "La categoría no fue encontrada.";
                return RedirectToAction("VerCategorias");
            }

            categoria.nombre = nombre;

            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();

            TempData["Success"] = "Categoría editada correctamente.";

            return RedirectToAction("VerCategorias");
        }

        // Acción para eliminar una categoría
        [HttpPost]
        public IActionResult EliminarCategoria(int id)
        {
            var categoria = _context.Categoria.FirstOrDefault(c => c.id == id);
            if (categoria == null)
            {
                TempData["Error"] = "La categoría no fue encontrada.";
                return RedirectToAction("VerCategorias");
            }

            categoria.estado = 0;
            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();

            TempData["Success"] = "Categoría eliminada correctamente.";
            return RedirectToAction("VerCategorias");
        }
    }
}
