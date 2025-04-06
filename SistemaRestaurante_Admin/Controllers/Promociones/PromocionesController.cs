using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_de_Restaurante___Modulo_de_Administracion.Models;
using SistemaRestaurante_Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SistemaRestaurante_Admin.Controllers.Promociones
{
    public class PromocionesController : Controller
    {
        private readonly restauranteDbContext _context;

        public PromocionesController(restauranteDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> VerPromociones(int? id)
        {
            var viewModel = new PromocionViewModel
            {
                Promocion = new Promocion(),
                Categorias = await _context.Categoria.Where(c => c.estado == 1).ToListAsync(),
                CombosDisponibles = await ObtenerCombosDisponiblesAsync(),
                PromocionesActivas = await ObtenerPromocionesActivasAsync()
            };

            if (id.HasValue && id > 0)
            {
                var promocionEditar = await _context.Promociones
                    .Include(p => p.ComboPromociones)
                    .FirstOrDefaultAsync(p => p.id == id && p.estado == 1);

                if (promocionEditar != null)
                {
                    viewModel.Promocion = new Promocion
                    {
                        id = promocionEditar.id,
                        nombre = promocionEditar.nombre,
                        descripcion = promocionEditar.descripcion,
                        descuento = promocionEditar.descuento,
                        categoria_id = promocionEditar.categoria_id,
                        FechaInicioVista = promocionEditar.ComboPromociones.Min(cp => cp.fecha_inicio),
                        FechaFinVista = promocionEditar.ComboPromociones.Max(cp => cp.fecha_fin),
                        CombosSeleccionados = string.Join(",", promocionEditar.ComboPromociones.Select(cp => cp.combo_id))
                    };
                }
                else
                {
                    TempData["ErrorMessage"] = "La promoción solicitada no existe o ya fue eliminada";
                }
            }

            return View("/Views/Promociones/CrearPromociones.cshtml", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearPromocion([Bind("Promocion")] PromocionViewModel model)
        {
            try
            {
                Console.WriteLine($"Fecha Inicio Recibida: {model.Promocion.FechaInicioVista}");
                Console.WriteLine($"Fecha Fin Recibida: {model.Promocion.FechaFinVista}");

                var fechaInicio = model.Promocion.FechaInicioVista.Date;
                var fechaFin = model.Promocion.FechaFinVista.Date;
                var hoy = DateTime.Today;

                Console.WriteLine($"Fecha Inicio Normalizada: {fechaInicio}");
                Console.WriteLine($"Fecha Fin Normalizada: {fechaFin}");
                Console.WriteLine($"Hoy: {hoy}");

                bool tieneErrores = false;
                if (fechaInicio == fechaFin)
                {
                    ModelState.AddModelError("Promocion.FechaFinVista", "La fecha de fin no puede ser igual a la fecha de inicio");
                    tieneErrores = true;
                }

                // Validación fecha inicio (puede ser hoy)
                if (fechaInicio < hoy)
                {
                    ModelState.AddModelError("Promocion.FechaInicioVista", "La fecha de inicio no puede ser anterior a hoy");
                    tieneErrores = true;
                    Console.WriteLine("Error: Fecha inicio anterior a hoy");
                }

                // Validación fecha fin (no puede ser anterior a hoy)
                if (fechaFin < hoy)
                {
                    ModelState.AddModelError("Promocion.FechaFinVista", "La fecha de fin no puede ser anterior a hoy");
                    tieneErrores = true;
                    Console.WriteLine("Error: Fecha fin anterior a hoy");
                }

                // Validar que fecha fin no sea anterior a fecha inicio
                if (fechaFin < fechaInicio)
                {
                    ModelState.AddModelError("Promocion.FechaFinVista", "La fecha de fin no puede ser anterior a la de inicio");
                    tieneErrores = true;
                    Console.WriteLine("Error: Fecha fin anterior a fecha inicio");
                }

                if (string.IsNullOrEmpty(model.Promocion.CombosSeleccionados))
                {
                    ModelState.AddModelError("Promocion.CombosSeleccionados", "Debe seleccionar al menos un combo");
                    tieneErrores = true;
                    Console.WriteLine("Error: No hay combos seleccionados");
                }

                if (tieneErrores)
                {
                    return await RecargarViewModelYRetornarVista(model);
                }

                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    var promocion = new Promocion
                    {
                        nombre = model.Promocion.nombre,
                        descripcion = model.Promocion.descripcion,
                        descuento = model.Promocion.descuento,
                        categoria_id = model.Promocion.categoria_id,
                        estado = 1
                    };

                    _context.Promociones.Add(promocion);
                    await _context.SaveChangesAsync();

                    var comboIds = model.Promocion.CombosSeleccionados
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(int.Parse)
                        .Distinct()
                        .ToList();

                    var combosExistentes = await _context.Combos
                        .Where(c => comboIds.Contains(c.Id) && c.Estado == 1)
                        .Select(c => c.Id)
                        .ToListAsync();

                    if (combosExistentes.Count != comboIds.Count)
                    {
                        ModelState.AddModelError("Promocion.CombosSeleccionados", "Uno o más combos seleccionados no existen");
                        return await RecargarViewModelYRetornarVista(model);
                    }

                    var comboPromociones = comboIds.Select(comboId => new ComboPromocion
                    {
                        combo_id = comboId,
                        promocion_id = promocion.id,
                        fecha_inicio = fechaInicio,
                        fecha_fin = fechaFin,
                        estado = 1
                    }).ToList();

                    await _context.ComboPromocion.AddRangeAsync(comboPromociones);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    TempData["SuccessMessage"] = "Promoción creada exitosamente!";
                    return RedirectToAction("VerPromociones");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _context.ChangeTracker.Clear();
                    TempData["ErrorMessage"] = $"Error al guardar: {ex.Message}";
                    Console.WriteLine($"Error en transacción: {ex.Message}");
                    return await RecargarViewModelYRetornarVista(model);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error inesperado: {ex.Message}";
                Console.WriteLine($"Error inesperado: {ex.Message}");
                return await RecargarViewModelYRetornarVista(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ModificarPromocion(PromocionViewModel model)
        {
            try
            {
                Console.WriteLine($"Fecha Inicio Recibida (Modif): {model.Promocion.FechaInicioVista}");
                Console.WriteLine($"Fecha Fin Recibida (Modif): {model.Promocion.FechaFinVista}");

                var fechaInicio = model.Promocion.FechaInicioVista.Date;
                var fechaFin = model.Promocion.FechaFinVista.Date;
                var hoy = DateTime.Today;

                bool tieneErrores = false;
                if (fechaInicio == fechaFin)
                {
                    ModelState.AddModelError("Promocion.FechaFinVista", "La fecha de fin no puede ser igual a la fecha de inicio");
                    tieneErrores = true;
                }

               
                if (fechaFin < hoy)
                {
                    ModelState.AddModelError("Promocion.FechaFinVista", "La fecha de fin no puede ser anterior a hoy");
                    tieneErrores = true;
                    Console.WriteLine("Error (Modif): Fecha fin anterior a hoy");
                }

                if (fechaFin < fechaInicio)
                {
                    ModelState.AddModelError("Promocion.FechaFinVista", "La fecha de fin no puede ser anterior a la de inicio");
                    tieneErrores = true;
                    Console.WriteLine("Error (Modif): Fecha fin anterior a fecha inicio");
                }

                if (string.IsNullOrEmpty(model.Promocion.CombosSeleccionados))
                {
                    ModelState.AddModelError("Promocion.CombosSeleccionados", "Debe seleccionar al menos un combo");
                    tieneErrores = true;
                    Console.WriteLine("Error (Modif): No hay combos seleccionados");
                }

                if (tieneErrores)
                {
                    return await RecargarViewModelYRetornarVista(model);
                }

                if (model.Promocion.id <= 0)
                {
                    TempData["ErrorMessage"] = "ID de promoción no válido";
                    return await RecargarViewModelYRetornarVista(model);
                }

                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    var promocionExistente = await _context.Promociones
                        .Include(p => p.ComboPromociones)
                        .FirstOrDefaultAsync(p => p.id == model.Promocion.id && p.estado == 1);

                    if (promocionExistente == null)
                    {
                        TempData["ErrorMessage"] = "Promoción no encontrada o ya eliminada";
                        return await RecargarViewModelYRetornarVista(model);
                    }

                    promocionExistente.nombre = model.Promocion.nombre;
                    promocionExistente.descripcion = model.Promocion.descripcion;
                    promocionExistente.descuento = model.Promocion.descuento;
                    promocionExistente.categoria_id = model.Promocion.categoria_id;

                    var nuevosComboIds = model.Promocion.CombosSeleccionados
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(int.Parse)
                        .Distinct()
                        .ToList();

                    var combosExistentes = await _context.Combos
                        .Where(c => nuevosComboIds.Contains(c.Id) && c.Estado == 1)
                        .Select(c => c.Id)
                        .ToListAsync();

                    if (combosExistentes.Count != nuevosComboIds.Count)
                    {
                        ModelState.AddModelError("Promocion.CombosSeleccionados", "Uno o más combos seleccionados no existen");
                        return await RecargarViewModelYRetornarVista(model);
                    }

                    var combosAEliminar = promocionExistente.ComboPromociones
                        .Where(cp => !nuevosComboIds.Contains(cp.combo_id))
                        .ToList();

                    _context.ComboPromocion.RemoveRange(combosAEliminar);

                    var combosExistentesIds = promocionExistente.ComboPromociones.Select(cp => cp.combo_id).ToList();
                    var combosAAgregar = nuevosComboIds
                        .Where(id => !combosExistentesIds.Contains(id))
                        .Select(comboId => new ComboPromocion
                        {
                            combo_id = comboId,
                            promocion_id = promocionExistente.id,
                            fecha_inicio = fechaInicio,
                            fecha_fin = fechaFin,
                            estado = 1
                        }).ToList();

                    await _context.ComboPromocion.AddRangeAsync(combosAAgregar);

                    foreach (var comboExistente in promocionExistente.ComboPromociones)
                    {
                        comboExistente.fecha_inicio = fechaInicio;
                        comboExistente.fecha_fin = fechaFin;
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    TempData["SuccessMessage"] = "Promoción modificada exitosamente!";
                    return RedirectToAction("VerPromociones");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    TempData["ErrorMessage"] = $"Error al modificar: {ex.Message}";
                    Console.WriteLine($"Error en transacción (Modif): {ex.Message}");
                    return await RecargarViewModelYRetornarVista(model);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error inesperado: {ex.Message}";
                Console.WriteLine($"Error inesperado (Modif): {ex.Message}");
                return await RecargarViewModelYRetornarVista(model);
            }
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> EliminarPromocion(int id)
        //{
        //    using var transaction = await _context.Database.BeginTransactionAsync();

        //    try
        //    {
        //        var promocion = await _context.Promociones
        //            .Include(p => p.ComboPromociones)
        //            .FirstOrDefaultAsync(p => p.id == id);

        //        if (promocion == null)
        //        {
        //            TempData["ErrorMessage"] = "La promoción no existe";
        //            return RedirectToAction("VerPromociones");
        //        }

        //        promocion.estado = 0;

        //        foreach (var comboPromo in promocion.ComboPromociones)
        //        {
        //            comboPromo.estado = 0;
        //        }

        //        await _context.SaveChangesAsync();
        //        await transaction.CommitAsync();

        //        TempData["SuccessMessage"] = "Promoción modificada exitosamente!";
        //        return RedirectToAction("VerPromociones");
        //    }
        //    catch (Exception ex)
        //    {
        //        await transaction.RollbackAsync();
        //        TempData["ErrorMessage"] = $"Error al eliminar: {ex.Message}";
        //    }

        //    return RedirectToAction("VerPromociones");
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarPromocion(int id)
        {
            try
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                var promocion = await _context.Promociones
                    .Include(p => p.ComboPromociones)
                    .FirstOrDefaultAsync(p => p.id == id);

                if (promocion == null)
                {
                    TempData["ErrorMessage"] = "La promoción no existe";
                    return RedirectToAction("VerPromociones");
                }

                promocion.estado = 0;

                foreach (var comboPromo in promocion.ComboPromociones)
                {
                    comboPromo.estado = 0;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                TempData["SuccessMessage"] = "Promoción eliminada exitosamente!";
                return RedirectToAction("VerPromociones");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al eliminar: {ex.Message}";
                return RedirectToAction("VerPromociones");
            }
        }


        private async Task<List<Models.Combos>> ObtenerCombosDisponiblesAsync()
        {
            return await _context.Combos
                .Where(c => c.Estado == 1)
                .AsNoTracking()
                .ToListAsync();
        }


        private async Task<List<Promocion>> ObtenerPromocionesActivasAsync()
        {
            var hoy = DateTime.Now.Date;

            return await _context.Promociones
                .Include(p => p.ComboPromociones)
                .Include(p => p.Categoria)
                .Where(p => p.estado == 1 &&
                       p.ComboPromociones.Any(cp => cp.estado == 1 && cp.fecha_fin >= hoy))
                .Select(p => new Promocion
                {
                    id = p.id,
                    nombre = p.nombre,
                    descripcion = p.descripcion,
                    descuento = p.descuento,
                    categoria_id = p.categoria_id,
                    FechaInicioVista = p.ComboPromociones.Min(cp => cp.fecha_inicio),
                    FechaFinVista = p.ComboPromociones.Max(cp => cp.fecha_fin),
                    Categoria = p.Categoria
                })
                .AsNoTracking()
                .ToListAsync();
        }

        //private async Task<IActionResult> RecargarViewModelYRetornarVista(PromocionViewModel model)
        //{
        //    var viewModel = new PromocionViewModel
        //    {
        //        Promocion = model.Promocion,
        //        CombosDisponibles = await ObtenerCombosDisponiblesAsync(),
        //        PromocionesActivas = await ObtenerPromocionesActivasAsync(),
        //        Categorias = await _context.Categoria
        //            .Where(c => c.estado == 1)
        //            .AsNoTracking()
        //            .ToListAsync()
        //    };

        //    return View("/Views/Promociones/CrearPromociones.cshtml", viewModel);
        //}

        private async Task<IActionResult> RecargarViewModelYRetornarVista(PromocionViewModel model)
        {
            var viewModel = new PromocionViewModel
            {
                Promocion = model.Promocion,
                CombosDisponibles = await ObtenerCombosDisponiblesAsync(),
                PromocionesActivas = await ObtenerPromocionesActivasAsync(),
                Categorias = await _context.Categoria
                    .Where(c => c.estado == 1)
                    .AsNoTracking()
                    .ToListAsync()
            };

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState)
                {
                    if (error.Value.Errors.Any())
                    {
                        TempData[$"Error_{error.Key}"] = error.Value.Errors.First().ErrorMessage;
                    }
                }
            }

            return View("/Views/Promociones/CrearPromociones.cshtml", viewModel);
        }
    }
}