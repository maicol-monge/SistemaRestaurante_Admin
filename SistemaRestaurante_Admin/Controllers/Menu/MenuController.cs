using Microsoft.AspNetCore.Mvc;
using Sistema_de_Restaurante___Modulo_de_Administracion.Models;
using SistemaRestaurante_Admin.Models;
using static SistemaRestaurante_Admin.Models.Menu;

namespace SistemaRestaurante_Admin.Controllers.Menu
{
    public class MenuController : Controller
    {
        private readonly restauranteDbContext _context;

        public MenuController(restauranteDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult VerMenus()
        {
            var listaMenus = (from menu in _context.Menu
                              where menu.estado == 1
                              select menu).ToList();


            var listaPlatos = (from platos in _context.Platos
                               where platos.Estado ==1
                               select platos).ToList();

            var listaCombos = (from combos in _context.Combos
                               where combos.Estado ==1
                               select combos).ToList();

            var Vistaa = new ListasModelo
            {
                Menus = listaMenus,
                Platos = listaPlatos,
                Combos = listaCombos
                
            };

            return View("/Views/Menu/CrearMenu.cshtml", Vistaa);
        }



        [HttpPost]
        public IActionResult ListasPlatos(int IdMenu)
        {
            var platosDelMenu = (from menu_plato in _context.Menu_Plato
                                 join plato in _context.Platos on menu_plato.plato_id equals plato.Id
                                 join menu in _context.Menu on menu_plato.menu_id equals menu.id
                                 where menu_plato.menu_id == IdMenu && menu_plato.estado == 1 && plato.Estado == 1 && menu.estado == 1
                                 select new
                                 {
                                     PlatoId = plato.Id,
                                     NombrePlato = plato.Nombre,
                                     Descripcion = plato.Descripcion,
                                     Precio = plato.Precio,
                                     Imagen = plato.Imagen
                                 }).ToList();

            return Json(platosDelMenu);
        }


        [HttpPost]
        public IActionResult ListaCombos(int IdMenu)
        {
            var combosDelMenu = (from menu_combo in _context.Menu_Combo
                                 join combo in _context.Combos on menu_combo.combo_id equals combo.Id
                                 join menu in _context.Menu on menu_combo.menu_id equals menu.id
                                 where menu_combo.menu_id == IdMenu && menu_combo.estado ==1 && combo.Estado ==1 && menu.estado ==1
                                 select new
                                 {
                                     ComboId = combo.Id,
                                     NombreCombo = combo.Nombre,
                                     DescripcionCombo = combo.Descripcion,
                                     PrecioCombo= combo.Precio
                                 }).ToList();



            return Json(combosDelMenu);
        }


        public IActionResult ActualizarListaPlatos()
        {
            var listaPlatos = _context.Platos.ToList(); // Obtén la lista actualizada de platos

            // Devuelve los datos como JSON
            return Json(listaPlatos);
        }

        public IActionResult ActualizarListaCombos()
        {
            var listaCombos = _context.Combos.ToList();  

            return Json(listaCombos);  // Devuelve la lista de combos en formato JSON
        }


        [HttpGet]
        public IActionResult ActualizarListaMenus()
        {
            var listaMenus = _context.Menu
                .Where(m => m.estado == 1)
                .Select(m => new
                {
                    id = m.id,
                    tipoMenu = m.tipo_menu,
                    tipoVenta = m.tipo_venta,
                    horaInicio = m.hora_inicio,
                    horaFin = m.hora_fin,
                    fechaInicio = m.fecha_inicio,
                    fechaFin = m.fecha_fin
                })
                .ToList();

            return Json(listaMenus);
        }





        [HttpPost]
        public IActionResult CrearMenu([FromBody] CrearMenuRequest datos)
        {
            //if (datos.PlatosSeleccionados == null || datos.CombosSeleccionados == null ||
            //    string.IsNullOrEmpty(datos.TipoMenu) || string.IsNullOrEmpty(datos.TipoVenta))
            //{
            //    return BadRequest(new { message = "Datos incompletos o inválidos." });
            //}
            
            if (datos == null)
            {
                return BadRequest(new { message = "Datos incompletos o inválidos." });
            }

            // Crear un nuevo objeto Menu con los datos del parámetro
            var nuevoMenu = new Models.Menu
            {
                tipo_menu = datos.TipoMenu,
                tipo_venta = datos.TipoVenta,
                hora_inicio = datos.HoraInicio,
                hora_fin = datos.HoraFin,
                fecha_inicio = datos.FechaInicio,
                fecha_fin = datos.FechaFin,
                estado = 1
            };

            _context.Menu.Add(nuevoMenu);
            _context.SaveChanges(); // Guardar para obtener el ID generado

            // Insertar los platos relacionados con el nuevo menú
            if (datos.PlatosSeleccionados.Any())
            {
                foreach (var platoId in datos.PlatosSeleccionados)
                {
                    var menuPlato = new MenuPlato
                    {
                        menu_id = nuevoMenu.id,
                        plato_id = platoId,
                        estado = 1
                    };
                    _context.Menu_Plato.Add(menuPlato);
                }
            }

            // Insertar los combos relacionados con el nuevo menú
            if (datos.CombosSeleccionados.Any())
            {
                foreach (var comboId in datos.CombosSeleccionados)
                {
                    var menuCombo = new MenuCombo
                    {
                        menu_id = nuevoMenu.id,
                        combo_id = comboId,
                        estado = 1
                    };
                    _context.Menu_Combo.Add(menuCombo);
                }
            }

            _context.SaveChanges();

            return Ok(new { message = "Menú creado exitosamente" });
        }


        [HttpPost]
        public IActionResult EditarMenu([FromBody] CrearMenuRequest datos)
        {
            if (datos.id <= 0)
            {
                return BadRequest(new { message = "ID inválido." });
            }

            var menuExistente = _context.Menu.FirstOrDefault(m => m.id == datos.id);
            if (menuExistente == null)
            {
                return NotFound(new { message = "Menú no encontrado." });
            }

            // Actualizar solo los campos que vienen con valor

            if (!string.IsNullOrEmpty(datos.TipoMenu))
            {
                menuExistente.tipo_menu = datos.TipoMenu;
            }

            if (!string.IsNullOrEmpty(datos.TipoVenta))
            {
                menuExistente.tipo_venta = datos.TipoVenta;
            }

            if (datos.HoraInicio.HasValue)
            {
                menuExistente.hora_inicio = datos.HoraInicio.Value;
            }

            if (datos.HoraFin.HasValue)
            {
                menuExistente.hora_fin = datos.HoraFin.Value;
            }

            if (datos.FechaInicio.HasValue)
            {
                menuExistente.fecha_inicio = datos.FechaInicio.Value;
            }

            if (datos.FechaFin.HasValue)
            {
                menuExistente.fecha_fin = datos.FechaFin.Value;
            }

            if (datos.PlatosSeleccionados != null && datos.PlatosSeleccionados.Any())
            {
                var platosExistentes = _context.Menu_Plato.Where(mp => mp.menu_id == menuExistente.id).ToList();
                var idsPlatosExistentes = platosExistentes.Select(mp => mp.plato_id).ToList();
                var platosAEliminar = platosExistentes.Where(mp => !datos.PlatosSeleccionados.Contains(mp.plato_id)).ToList();
                _context.Menu_Plato.RemoveRange(platosAEliminar);
                foreach (var platoId in datos.PlatosSeleccionados)
                {
                    if (!idsPlatosExistentes.Contains(platoId))
                    {
                        _context.Menu_Plato.Add(new MenuPlato
                        {
                            menu_id = menuExistente.id,
                            plato_id = platoId,
                            estado = 1
                        });
                    }
                }
                _context.SaveChanges();
            }


            if (datos.CombosSeleccionados != null && datos.CombosSeleccionados.Any())
            {
                var combosExistentes = _context.Menu_Combo.Where(mc => mc.menu_id == menuExistente.id).ToList();
                var idsCombosExistentes = combosExistentes.Select(mc => mc.combo_id).ToList();
                var combosAEliminar = combosExistentes.Where(mc => !datos.CombosSeleccionados.Contains(mc.combo_id)).ToList();
                _context.Menu_Combo.RemoveRange(combosAEliminar);
                foreach (var comboId in datos.CombosSeleccionados)
                {
                    if (!idsCombosExistentes.Contains(comboId))
                    {
                        _context.Menu_Combo.Add(new MenuCombo
                        {
                            menu_id = menuExistente.id,
                            combo_id = comboId,
                            estado = 1
                        });
                    }
                }
                _context.SaveChanges();
            }


            _context.SaveChanges();

            return Ok(new { message = "Menú editado exitosamente" });
        }


        [HttpPost]
        public IActionResult EliminarMenu([FromBody] CrearMenuRequest datos)
        {
            var menuExistente = _context.Menu.FirstOrDefault(menu => menu.id == datos.id);  
            menuExistente.estado = 0;
            try
            {
                // Guardar los cambios en la base de datos
                _context.SaveChanges();
                return Json(new { message = "Menú eliminado exitosamente" });
            }
            catch (Exception ex)
            {
                return Json(new { message = "Error al eliminar el menú: " + ex.Message });
            }
        }




    }
}
