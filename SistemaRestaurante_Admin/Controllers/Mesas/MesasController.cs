using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_de_Restaurante___Modulo_de_Administracion.Models;
using SistemaRestaurante_Admin.Models.Mesas;

namespace SistemaRestaurante_Admin.Controllers.Mesas
{
    public class MesasController : Controller
    {
        private readonly restauranteDbContext _context;

        public MesasController(restauranteDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult VerGestionarMesas()
        {
            var mesas = (from m in _context.mesas
                         where m.estado == 1 // Solo los activos
                          select m)  
                       .ToList();
           
            return View("/Views/Mesas/GestionarMesas.cshtml", mesas);
        }


        [HttpPost]
        public IActionResult InsertarMesa(int numero, int capacidad, string disponibilidad)
        {
            if (numero <= 0 || capacidad <= 0 || string.IsNullOrEmpty(disponibilidad))
            {
                TempData["Error"] = "Todos los campos son obligatorios.";
                return RedirectToAction("VerGestionarMesas");
            }

            var mesaExistente = _context.mesas.FirstOrDefault(m => m.numero == numero);
            if (mesaExistente != null)
            {
                TempData["Error"] = "El número de mesa ya existe.";
                return RedirectToAction("VerGestionarMesas");
            }

            var nuevaMesa = new mesas
            {
                numero = numero,
                capacidad = capacidad,
                disponibilidad = disponibilidad,
                estado = 1 // Activo
            };

            _context.mesas.Add(nuevaMesa);
            _context.SaveChanges();

            TempData["Success"] = "Mesa creada exitosamente.";
            return RedirectToAction("VerGestionarMesas");
        }

        [HttpPost]
        public IActionResult EditarMesa(int id, int numero, int capacidad, string disponibilidad)
        {
            var mesa = _context.mesas.FirstOrDefault(m => m.id == id);
            if (mesa == null)
            {
                return NotFound();
            }

            mesa.numero = numero;
            mesa.capacidad = capacidad;
            mesa.disponibilidad = disponibilidad;

            _context.Entry(mesa).State = EntityState.Modified;
            _context.SaveChanges();

            TempData["Success"] = "Mesa modificada exitosamente.";
            return RedirectToAction("VerGestionarMesas");
        }

        [HttpPost]
        public IActionResult EliminarMesa(int id)
        {
            var mesa = _context.mesas.FirstOrDefault(m => m.id == id);
            if (mesa == null)
            {
                return NotFound();
            }

            mesa.estado = 0; // Desactivar la mesa
            _context.Entry(mesa).State = EntityState.Modified;
            _context.SaveChanges();

            TempData["Success"] = "Mesa eliminada exitosamente.";
            return RedirectToAction("VerGestionarMesas");
        }
    }
}
