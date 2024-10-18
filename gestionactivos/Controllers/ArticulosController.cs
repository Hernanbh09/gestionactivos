using gestionactivos.Data;
using gestionactivos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace gestionactivos.Controllers
{
    [Authorize(Policy = "GlobalPolicy")]
    [Authorize(Roles = "Administrador,Coordinador")]
    public class ArticulosController : Controller
    {
        private readonly ArticulosData _ArticulosData = new ArticulosData();
        private int? idUsuario;

        // Este método se ejecuta antes de cada acción del controlador
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var idUsuarioClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            idUsuario = idUsuarioClaim != null ? (int?)Convert.ToInt32(idUsuarioClaim.Value) : null;
            base.OnActionExecuting(context);
        }

        public IActionResult Listar()
        {
            var olista = _ArticulosData.Listar(idUsuario);
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

            var mensaje = _ArticulosData.Guardar(oArticulo, idUsuario);

            if (string.IsNullOrEmpty(mensaje))
                return RedirectToAction("Listar");
            else
            {
                ViewBag.ErrorMessage = mensaje;
                return View();
            }
        }

        public IActionResult Editar(int idArticulo)
        {
            var oarticulo = _ArticulosData.Obtener(idArticulo, idUsuario);
            return View(oarticulo);
        }

        [HttpPost]
        public IActionResult Editar(ArticulosModel oArticulo)
        {
            if (!ModelState.IsValid)
                return View();

            var respuesta = _ArticulosData.Editar(oArticulo, idUsuario);
            if (respuesta)
                return RedirectToAction("Listar");
            else
                return View();
        }

        public IActionResult Eliminar(int idArticulo)
        {
            var oArticulo = _ArticulosData.Obtener(idArticulo, idUsuario);
            return View(oArticulo);
        }

        [HttpPost]
        public IActionResult Eliminar(ArticulosModel oArticulo)
        {
            var respuesta = _ArticulosData.Eliminar(oArticulo.idArticulo, idUsuario);
            if (respuesta)
                return RedirectToAction("Listar");
            else
                return View();
        }
    }
}
