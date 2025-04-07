using Microsoft.AspNetCore.Mvc;
using Sistema_de_Restaurante___Modulo_de_Administracion.Models;
using SistemaRestaurante_Admin.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SistemaRestaurante_Admin.Controllers.Combos
{
    public class CombosController : Controller
    {
        private readonly restauranteDbContext _context;

        public CombosController(restauranteDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> VerCombos()
        {
            try
            {
                // Obtener las categorías activas
                var categorias = await _context.Categoria
                    .Where(c => c.estado == 1)
                    .ToListAsync();

                // Obtener los platos activos
                var platos = await _context.Platos
                    .Where(p => p.Estado == 1)
                    .ToListAsync();

                // Obtener todos los combos, sin filtrar por estado, para que se muestren tanto los activos como los inactivos
                var combos = await _context.Combos
                    .Include(c => c.Categoria)  // Incluir la categoría del combo
                    .ToListAsync();  // No filtrar por estado, para obtener todos los combos

                // Pasar los datos a la vista
                ViewBag.Categorias = categorias;
                ViewBag.Platos = platos;
                ViewBag.Combos = combos;

                return View("/Views/Combos/CrearCombos.cshtml");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener los combos: {ex.Message}");
            }
        }






        [HttpPost]
        public async Task<IActionResult> CrearCombo([FromBody] ComboDto comboDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(comboDto.Nombre) || comboDto.Precio <= 0 || string.IsNullOrWhiteSpace(comboDto.Descripcion)
                    || comboDto.CategoriaId <= 0 || comboDto.PlatosSeleccionados == null || !comboDto.PlatosSeleccionados.Any())
                {
                    return Json(new { success = false, message = "Todos los campos son obligatorios y debe seleccionar al menos un plato." });
                }

                var nuevoCombo = new Models.Combos
                {
                    Nombre = comboDto.Nombre,
                    Precio = comboDto.Precio,
                    Descripcion = comboDto.Descripcion,
                    CategoriaId = comboDto.CategoriaId,
                    Estado = 1
                };

                _context.Combos.Add(nuevoCombo);
                await _context.SaveChangesAsync();

                foreach (var platoId in comboDto.PlatosSeleccionados)
                {
                    var platoCombo = new Platos_Combos
                    {
                        ComboId = nuevoCombo.Id,
                        PlatoId = platoId,
                        Estado = 1
                    };
                    _context.Platos_Combo.Add(platoCombo);
                }

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Combo creado exitosamente." });
            }
            catch (Exception ex)
            {
                // Captura y muestra el mensaje de error completo para depuración
                return Json(new { success = false, message = $"Error al crear el combo: {ex.Message}" });
            }
        }




        [HttpGet]
        public async Task<IActionResult> ObtenerCombo(int id)
        {
            try
            {
                var combo = await _context.Combos
                    .Include(c => c.Categoria)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (combo == null)
                    return NotFound(new { message = "Combo no encontrado" });

                // Obtener los platos asociados al combo desde la tabla intermedia
                var platosCombo = await _context.Platos_Combo
                    .Where(pc => pc.ComboId == id)
                    .Join(_context.Platos, pc => pc.PlatoId, p => p.Id, (pc, p) => new
                    {
                        id = p.Id,
                        nombre = p.Nombre,
                        precio = p.Precio,
                        imagen = p.Imagen
                    })
                    .ToListAsync();

                return Json(new
                {
                    id = combo.Id,
                    nombre = combo.Nombre,
                    precio = combo.Precio,
                    descripcion = combo.Descripcion,
                    categoriaId = combo.CategoriaId,
                    platos = platosCombo
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al obtener el combo: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditarCombo([FromBody] ComboDto comboDto)
        {
            try
            {
                // Validación de campos obligatorios
                if (string.IsNullOrWhiteSpace(comboDto.Nombre) || comboDto.Precio <= 0 ||
                    string.IsNullOrWhiteSpace(comboDto.Descripcion) || comboDto.CategoriaId <= 0 ||
                    comboDto.PlatosSeleccionados == null || !comboDto.PlatosSeleccionados.Any())
                {
                    return Json(new { success = false, message = "Todos los campos son obligatorios y debe seleccionar al menos un plato." });
                }

                // Buscar el combo por ID
                var combo = await _context.Combos.FindAsync(comboDto.Id);
                if (combo == null)
                    return Json(new { success = false, message = "El combo no existe." });

                // Actualizar los datos del combo
                combo.Nombre = comboDto.Nombre;
                combo.Precio = comboDto.Precio;
                combo.Descripcion = comboDto.Descripcion;
                combo.CategoriaId = comboDto.CategoriaId;

                // Eliminar los platos antiguos del combo
                var platosExistentes = _context.Platos_Combo.Where(pc => pc.ComboId == comboDto.Id).ToList();
                if (platosExistentes.Any())
                {
                    _context.Platos_Combo.RemoveRange(platosExistentes);
                    await _context.SaveChangesAsync();  // Guardar después de eliminar
                }

                // Agregar los nuevos platos al combo
                foreach (var platoId in comboDto.PlatosSeleccionados)
                {
                    var platoCombo = new Platos_Combos
                    {
                        ComboId = comboDto.Id,
                        PlatoId = platoId,
                        Estado = 1
                    };
                    _context.Platos_Combo.Add(platoCombo);
                }

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Combo actualizado exitosamente." });
            }
            catch (Exception ex)
            {
                // Mostrar el error exacto que ocurrió
                return Json(new { success = false, message = $"Error al actualizar el combo: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EliminarCombo([FromBody] int id)
        {
            try
            {
                var combo = await _context.Combos.FindAsync(id);
                if (combo == null)
                    return NotFound(new { message = "Combo no encontrado" });

                // Cambiar el estado del combo a 0 (inactivo)
                combo.Estado = 0;  // Esto asume que la columna 'estado' está en la tabla 'combos'

                // Eliminar los platos asociados al combo en la tabla Platos_Combos
                var platosCombo = _context.Platos_Combo.Where(pc => pc.ComboId == id);
                if (platosCombo.Any())
                {
                    _context.Platos_Combo.RemoveRange(platosCombo);
                }

                // Guardar los cambios
                await _context.SaveChangesAsync();

                return Ok(new { message = "Combo desactivado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al desactivar el combo: {ex.Message}" });
            }
        }



    }
}
