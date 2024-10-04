using gestionactivos.Data;
using gestionactivos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace gestionactivos.Controllers
{
    [Authorize(Policy = "GlobalPolicy")]
    [Authorize(Roles = "Administrador,Coordinador")]
    public class ArticulosController : Controller
    {

        ArticulosData _ArticulosData = new ArticulosData();



        public IActionResult Listar()
        {
            var olista = _ArticulosData.Listar();
            return View(olista);
        }

        public IActionResult Guardar() 
        {
            return View();
        }
        [HttpPost]
        public IActionResult Guardar(ArticulosModel oArticulo)
        {
            if (!ModelState.IsValid)
                return View();

            var mensaje = _ArticulosData.Guardar(oArticulo);

            if (string.IsNullOrEmpty(mensaje))
                return RedirectToAction("Listar");
            else
            {
                // Pasar el mensaje de error a la vista
                ViewBag.ErrorMessage = mensaje;
                return View();
            }
        }

        public IActionResult Editar(int idArticulo)
        {
            var oarticulo = _ArticulosData.Obtener(idArticulo);
            return View(oarticulo);
        }
        [HttpPost]
        public IActionResult Editar(ArticulosModel oArticulo)
        {
            if (!ModelState.IsValid)
                return View();



            //Recibir un objeto y guardarlo en la base de datos
            var respuesta = _ArticulosData.Editar(oArticulo);
            if (respuesta)
                return RedirectToAction("Listar");
            else

                return View();
        }

        public IActionResult Eliminar(int idArticulo)
        {
            var oArticulo = _ArticulosData.Obtener(idArticulo);
            return View(oArticulo);
             
        }
        [HttpPost]
        public IActionResult Eliminar(ArticulosModel oArticulo)
        {
            var respuesta = _ArticulosData.Eliminar(oArticulo.idArticulo);
            if (respuesta)
                return RedirectToAction("Listar");
            else
                return View();
        }


    }
}
