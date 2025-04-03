using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_de_Restaurante___Modulo_de_Administracion.Models;
using SistemaRestaurante_Admin.Models.Servicios;

namespace SistemaRestaurante_Admin.Controllers.Account
{
    public class AccountController : Controller
    {
        public readonly restauranteDbContext _context;
        public AccountController(restauranteDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            //// Verificar si la variable de sesión "UsuarioId" ya existe
            //if (HttpContext.Session.GetString("UsuarioId") != null)
            //{
            //    // El usuario ya está logueado, redirigir al menú principal
            //    return RedirectToAction("VerMenuAdmin", "Home");
            //}
            return View("/Views/Account/LoginAdmin.cshtml");
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Authenticate(int codigoEmpleado, string contrasena)
        {
            var empleado = _context.Empleados
                .FirstOrDefault(e => e.codigo == codigoEmpleado);

            if (empleado != null && empleado.contrasena == contrasena)
            {
                HttpContext.Session.SetString("UsuarioId", empleado.id.ToString());
                HttpContext.Session.SetString("NombreUsuario", empleado.nombre + " " + empleado.apellido);
                HttpContext.Session.SetInt32("RolUsuario", empleado.cargo_Id); // Guardar el nombre del cargo o "Usuario" si no tiene cargo

                ViewBag.NombreUsuario = empleado.nombre + " " + empleado.apellido;

                return RedirectToAction("VerMenuAdmin", "Home");
            }
            else
            {
                TempData["Error"] = "Código de empleado o contraseña incorrectos";
                return RedirectToAction("Login");
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
