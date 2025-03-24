using Microsoft.AspNetCore.Mvc;

namespace SistemaRestaurante_Admin.Controllers.Platos
{
    public class PlatosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult VerPlatos()
        {
            ViewBag.PlatillosExistentes = new List<Object>
        {
            new { Nombre = "Tacos", Descripcion = "Deliciosos tacos preparados con tortillas de maíz recién hechas, rellenos de carne asada, carnitas o al pastor, acompañados de cebolla, cilantro y salsa al gusto.", Precio = 1.99, Imagen = "https://upload.wikimedia.org/wikipedia/commons/thumb/7/73/001_Tacos_de_carnitas%2C_carne_asada_y_al_pastor.jpg/800px-001_Tacos_de_carnitas%2C_carne_asada_y_al_pastor.jpg" },

            new { Nombre = "Burritos", Descripcion = "Un platillo clásico de la cocina mexicana que consiste en una tortilla de harina rellena de carne, frijoles, arroz y otros ingredientes como queso, guacamole o crema, enrollado para mantener todos los sabores en su interior.", Precio = 2.99, Imagen = "https://upload.wikimedia.org/wikipedia/commons/4/44/Burrito_chihuahuense_de_chile_verde.jpg" },

            new { Nombre = "Quesadillas", Descripcion = "Tortillas de maíz o harina dobladas a la mitad y rellenas de queso derretido, a veces acompañadas de hongos, chicharrón o flor de calabaza, servidas con salsa y crema.", Precio = 3.99, Imagen = "https://upload.wikimedia.org/wikipedia/commons/7/75/Quesadilla_2.jpg" },

            new { Nombre = "Enchiladas", Descripcion = "Tortillas de maíz bañadas en una salsa roja o verde, rellenas de pollo o queso, y gratinadas con queso derretido, servidas con crema, lechuga y rodajas de cebolla.", Precio = 4.99, Imagen = "https://mojo.generalmills.com/api/public/content/MJYMoQ0lUkezBkK5ql2cSg_gmi_hi_res_jpeg.jpeg?v=c5e8a159&t=16e3ce250f244648bef28c5949fb99ff" },

            new { Nombre = "Nachos", Descripcion = "Crujientes totopos de maíz cubiertos con queso derretido, frijoles, jalapeños, carne deshebrada o chorizo, y decorados con crema y guacamole.", Precio = 5.99, Imagen = "https://mojo.generalmills.com/api/public/content/MJYMoQ0lUkezBkK5ql2cSg_gmi_hi_res_jpeg.jpeg?v=c5e8a159&t=16e3ce250f244648bef28c5949fb99ff" },

            new { Nombre = "Tamales", Descripcion = "Masa de maíz rellena de carne, chiles, queso o dulce, envuelta en hojas de maíz o plátano y cocida al vapor, acompañada de atole o champurrado.", Precio = 6.99, Imagen = "https://comedera.com/wp-content/uploads/sites/9/2017/08/tamales-mexicanos.jpg?w=500&h=466&crop=1" },

            new { Nombre = "Chiles Rellenos", Descripcion = "Chiles poblanos asados y rellenos de queso o picadillo, cubiertos con una ligera capa de huevo batido y fritos, servidos con una deliciosa salsa de jitomate.", Precio = 7.99, Imagen = "https://cdn-ilddihb.nitrocdn.com/MgqZCGPEMHvMRLsisMUCAIMWvgGMxqaj/assets/images/optimized/rev-1fbbd4e/www.goya.com/wp-content/uploads/2023/10/stuffed-chileschiles-rellenos.jpg" },

            new { Nombre = "Pozole", Descripcion = "Un caldo espeso y sustancioso elaborado con granos de maíz cacahuazintle, carne de cerdo o pollo, sazonado con especias y acompañado de lechuga, rábano, orégano y limón.", Precio = 8.99, Imagen = "https://www.cocinavital.mx/wp-content/uploads/2024/05/pozole-rojo-1-634x420.jpg" },

            new { Nombre = "Sopes", Descripcion = "Pequeñas tortillas gruesas de maíz con los bordes levantados, cubiertas con frijoles, carne deshebrada, queso fresco, crema y salsa.", Precio = 9.99, Imagen = "https://patijinich.com/es/wp-content/uploads/sites/3/2017/12/610-sopes.jpg" },

            new { Nombre = "Tostadas", Descripcion = "Tortillas de maíz fritas hasta quedar crujientes, cubiertas con frijoles, pollo o carne deshebrada, lechuga, queso rallado, crema y salsa.", Precio = 10.99, Imagen = "https://guerrerotortillas.com/wp-content/uploads/2021/04/beef-tostadas.jpg" }
        };
            return View("/Views/Platos/CrearPlatos.cshtml");
        }
    }
}
