﻿using Microsoft.AspNetCore.Mvc;

namespace SistemaRestaurante_Admin.Controllers.Menu
{
    public class MenuController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult VerMenus()
        {
            return View("/Views/Menu/CrearMenu.cshtml");
        }
    }
}
