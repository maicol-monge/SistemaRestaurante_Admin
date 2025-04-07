using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_de_Restaurante___Modulo_de_Administracion.Models;

namespace SistemaRestaurante_Admin.Controllers.Empleados
{
	public class EmpleadosController : Controller
	{
		private readonly restauranteDbContext _context;
		public EmpleadosController(restauranteDbContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			return View();
		}

		public class EmpleadoViewModel
		{
			public int id { get; set; }
			public int codigo { get; set; }
			public string contrasena { get; set; }
			public string nombre { get; set; }
			public string apellido { get; set; }
			public string telefono { get; set; }
			public int cargo_Id { get; set; }
			public int estado { get; set; }
			public string? CargoNombre { get; set; }
		}

		public IActionResult VerEmpleados()
		{
			var empleados = (from e in _context.Empleados
							 join c in _context.cargo on e.cargo_Id equals c.id
							 where e.estado == 1
							 select new EmpleadoViewModel
							 {
								 id = e.id,
								 nombre = e.nombre,
								 apellido = e.apellido,
								 telefono = e.telefono,
								 codigo = e.codigo,
								 contrasena = e.contrasena,
								 cargo_Id = e.cargo_Id,
								 CargoNombre = c.nombre,
							 })
					 .ToList();
			ViewBag.Cargos = _context.cargo.ToList();
			return View("/Views/Empleados/DarDeAltaEmpleados.cshtml", empleados);
		}

		[HttpPost]
		public IActionResult InsertarEmpleado(string nombre, string apellido, string telefono, int codigo, string contrasena, int cargo_Id)
		{
			if (string.IsNullOrEmpty(nombre))
			{
				TempData["Error"] = "El nombre del empleado es requerido.";
				return RedirectToAction("VerEmpleados");
			}

			if (string.IsNullOrEmpty(apellido))
			{
				TempData["Error"] = "El apellido del empleado es requerido.";
				return RedirectToAction("VerEmpleados");
			}

			if (string.IsNullOrEmpty(telefono))
			{
				TempData["Error"] = "El teléfono del empleado es requerido.";
				return RedirectToAction("VerEmpleados");
			}

			if (string.IsNullOrEmpty(codigo.ToString()))
			{
				TempData["Error"] = "El código del empleado es requerido.";
				return RedirectToAction("VerEmpleados");
			}
			if (string.IsNullOrEmpty(contrasena))
			{
				TempData["Error"] = "La contraseña del empleado es requerida.";
				return RedirectToAction("VerEmpleados");
			}



			// Verificar si el empleado ya existe en la base de datos (activo o inactivo)
			var empleadoExistente = _context.Empleados.FirstOrDefault(e => e.codigo == codigo);

			if (empleadoExistente != null)
			{
				if (empleadoExistente.estado == 1)
				{
					TempData["Error"] = $"El empleado con el código {empleadoExistente.codigo} ya existe y está activo.";
				}
				else
				{
					TempData["Error"] = $"El empleado con el código {empleadoExistente.codigo} ya existe pero está inactivo.";
				}
				return RedirectToAction("VerEmpleados");
			}

			// Si no existe, insertar el nuevo empleado
			var nuevoEmpleado = new Models.Empleados
			{
				nombre = nombre,
				apellido = apellido,
				telefono = telefono,
				codigo = codigo,
				contrasena = contrasena,
				cargo_Id = cargo_Id,
				estado = 1  // Activar el empleado
			};

			_context.Empleados.Add(nuevoEmpleado);
			_context.SaveChanges();

			TempData["Success"] = "Empleado creado exitosamente.";
			return RedirectToAction("VerEmpleados");
		}

		// Acción para editar un empleado
		[HttpPost]
		public IActionResult EditarEmpleado(int id, string nombre, string apellido, string telefono, int codigo, string contrasena, int cargo_Id)
		{
			var empleado = _context.Empleados.FirstOrDefault(e => e.id == id);

			if (empleado == null)
			{
				TempData["Error"] = "El empleado no fue encontrado.";
				return RedirectToAction("VerEmpleados");
			}

			

			empleado.nombre = nombre;
			empleado.apellido = apellido;
			empleado.telefono = telefono;
			empleado.codigo = codigo;
			empleado.contrasena = contrasena;
			empleado.cargo_Id = cargo_Id;

			_context.Entry(empleado).State = EntityState.Modified;
			_context.SaveChanges();

			TempData["Success"] = "Empleado editado correctamente.";

			return RedirectToAction("VerEmpleados");
		}

		// Acción para eliminar un empleado
		[HttpPost]
		public IActionResult EliminarEmpleado(int id)
		{
			var empleado = _context.Empleados.FirstOrDefault(e => e.id == id);
			if (empleado == null)
			{
				TempData["Error"] = "El empleado no fue encontrado.";
				return RedirectToAction("VerEmpleados");
			}

			empleado.estado = 0;
			_context.Entry(empleado).State = EntityState.Modified;
			_context.SaveChanges();

			TempData["Success"] = "Empleado eliminado correctamente.";
			return RedirectToAction("VerEmpleados");
		}
	}
}
