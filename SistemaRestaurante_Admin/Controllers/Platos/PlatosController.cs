using Microsoft.AspNetCore.Mvc;
using Sistema_de_Restaurante___Modulo_de_Administracion.Models;
using Microsoft.AspNetCore.Hosting;
using SistemaRestaurante_Admin.Models;

namespace SistemaRestaurante_Admin.Controllers.Platos
{
    public class PlatosController : Controller
    {
        private readonly restauranteDbContext _context;
        private readonly ImgBBService _imgBBService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public PlatosController(restauranteDbContext context, ImgBBService imgBBService, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _imgBBService = imgBBService;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult VerPlatos()
        {
            ViewBag.PlatillosExistentes = _context.Platos.ToList();
            ViewBag.Categorias = _context.Categoria.ToList();
            return View("/Views/Platos/CrearPlatos.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> CrearPlato(string nombre, string descripcion, decimal precio, int categoria, IFormFile imagen)
        {
            try
            {
                // Validar los datos de entrada
                if (string.IsNullOrEmpty(nombre))
                    ModelState.AddModelError("nombre", "El nombre es requerido");

                if (precio <= 0)
                    ModelState.AddModelError("precio", "El precio debe ser mayor que cero");

                if (imagen == null || imagen.Length == 0)
                    ModelState.AddModelError("imagen", "Debe seleccionar una imagen");

                if (!ModelState.IsValid)
                {
                    ViewBag.PlatillosExistentes = _context.Platos.ToList();
                    ViewBag.Categorias = _context.Categoria.ToList();
                    return View("/Views/Platos/CrearPlatos.cshtml");
                }

                // Subir la imagen a ImgBB
                string imageUrl;
                using (var stream = imagen.OpenReadStream())
                {
                    imageUrl = await _imgBBService.UploadImageAsync(stream, imagen.FileName);
                }

                

                // Crear el nuevo platillo
                var nuevoPlato = new Models.Platos
                {
                    Nombre = nombre,
                    Descripcion = descripcion,
                    Precio = precio,
                    Categoria_Id = categoria,
                    Imagen = imageUrl,
                    Estado = 1
                };

                // Guardar en la base de datos
                _context.Platos.Add(nuevoPlato);
                await _context.SaveChangesAsync();

                return RedirectToAction("VerPlatos");
            }
            catch (Exception ex)
            {
                // Loggear el error (puedes implementar un logger aquí)
                ModelState.AddModelError("", "Ocurrió un error al crear el platillo: " + ex.Message);
                ViewBag.PlatillosExistentes = _context.Platos.ToList();
                ViewBag.Categorias = _context.Categoria.ToList();
                return View("/Views/Platos/CrearPlatos.cshtml");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Modificar(int id, string nombre, string descripcion, decimal precio, int categoria, IFormFile imagen)
        {
            try
            {
                var platillo = await _context.Platos.FindAsync(id);
                if (platillo == null)
                {
                    return NotFound();
                }

                platillo.Nombre = nombre;
                platillo.Descripcion = descripcion;
                platillo.Precio = precio;
                platillo.Categoria_Id = categoria;

                if (imagen != null && imagen.Length > 0)
                {
                    // Subir nueva imagen a ImgBB
                    using var stream = imagen.OpenReadStream();
                    platillo.Imagen = await _imgBBService.UploadImageAsync(stream, imagen.FileName);
                }

                _context.Platos.Update(platillo);
                await _context.SaveChangesAsync();

                return RedirectToAction("VerPlatos");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al modificar el platillo: " + ex.Message);
                ViewBag.PlatillosExistentes = _context.Platos.ToList();
                ViewBag.Categorias = _context.Categoria.ToList();
                return View("CrearPlatos");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var platillo = await _context.Platos.FindAsync(id);
                if (platillo == null)
                {
                    return NotFound();
                }

                _context.Platos.Remove(platillo);
                await _context.SaveChangesAsync();

                return RedirectToAction("VerPlatos");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al eliminar el platillo: " + ex.Message);
                ViewBag.PlatillosExistentes = _context.Platos.ToList();
                ViewBag.Categorias = _context.Categoria.ToList();
                return View("CrearPlatos");
            }
        }
    }
}