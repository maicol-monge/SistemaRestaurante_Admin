
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_de_Restaurante___Modulo_de_Administracion.Models;
using SistemaRestaurante_Admin.Models.Cargos;

namespace SistemaRestaurante_Admin.Controllers.Cargos
{
    public class CargosController : Controller
    {
        private readonly restauranteDbContext _context;

        public CargosController(restauranteDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult VerCrearCargos()
        {
            
            var cargos = (from c in _context.cargo
                          where c.estado == 1 // Solo los activos
                          select c)
                        .ToList();
            return View("/Views/Cargos/CrearCargos.cshtml", cargos);
        }

        [HttpPost]
        public IActionResult InsertarCargo(string nombre)
        {
            if (string.IsNullOrEmpty(nombre))
            {
                TempData["Error"] = "El nombre del cargo es requerido.";
                return RedirectToAction("VerCrearCargos");
            }

            // Verificar si el cargo ya existe en la base de datos (activo o inactivo)
            var cargoExistente = _context.cargo.FirstOrDefault(c => c.nombre == nombre);

            if (cargoExistente != null)
            {
                if (cargoExistente.estado == 1)
                {
                    TempData["Error"] = "El cargo ya existe y está activo.";
                }
                else
                {
                    TempData["Error"] = "El cargo ya existe pero está inactivo.";
                }
                return RedirectToAction("VerCrearCargos");
            }

            // Si no existe, insertar el nuevo cargo
            var nuevoCargo = new cargo
            {
                nombre = nombre,
                estado = 1  // Activar el cargo
            };

            _context.cargo.Add(nuevoCargo);
            _context.SaveChanges();

            TempData["Success"] = "Cargo creado exitosamente.";
            return RedirectToAction("VerCrearCargos");
        }



        // Acción para editar un cargo
        [HttpPost]
        public IActionResult EditarCargo(int id, string nombre) 
{
         var cargo = _context.cargo.FirstOrDefault(c => c.id == id);

            if (cargo == null)
             {

                TempData["Error"] = "El cargo no fue encontrado.";
                return RedirectToAction("VerCrearCargos");
             }
    
             cargo.nombre = nombre;

             _context.Entry(cargo).State = EntityState.Modified;
             _context.SaveChanges();

             TempData["Success"] = "Cargo editado correctamente.";

             return RedirectToAction("VerCrearCargos");
        }


        // Acción para eliminar un cargo
        [HttpPost]
        public IActionResult EliminarCargo(int id)
        {
            var cargo = _context.cargo.FirstOrDefault(c => c.id == id);
            if (cargo == null)
            {
                TempData["Error"] = "El cargo no fue encontrado.";
                return RedirectToAction("VerCrearCargos");
            }

            cargo.estado = 0; 
            _context.Entry(cargo).State = EntityState.Modified;
            _context.SaveChanges();

            TempData["Success"] = "Cargo eliminado correctamente.";
            return RedirectToAction("VerCrearCargos");
        }



    }
}
