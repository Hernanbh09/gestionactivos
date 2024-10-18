using Microsoft.AspNetCore.Mvc;
using gestionactivos.Data;
using gestionactivos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;


namespace gestionactivos.Controllers
{
    [Authorize(Policy = "GlobalPolicy")] // Aplicar la política global
    [Authorize(Roles = "Administrador")]
    public class UsuariosController : Controller
    {
        UsuariosData _UsuariosDatos = new UsuariosData();
        private int? idUsuarioC;

        // Este método se ejecuta antes de cada acción del controlador
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var idUsuarioClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            idUsuarioC = idUsuarioClaim != null ? (int?)Convert.ToInt32(idUsuarioClaim.Value) : null;
            base.OnActionExecuting(context);
        }

        public IActionResult Listar()
        {
            var oLista = _UsuariosDatos.Listar(idUsuarioC);

            return View(oLista);
        }

        public IActionResult Guardar()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Guardar(UsuarioModel oUsuario)
        {
            if (!ModelState.IsValid)
                return View();

            string mensajeError;
            var respuesta = _UsuariosDatos.Guardar(oUsuario, out mensajeError, idUsuarioC);

            if (respuesta)
            {
                return RedirectToAction("Listar");
            }
            else
            {
                ViewBag.ErrorMessage = mensajeError; // Pasar el mensaje de error a la vista
                return View();
            }
        }

        public IActionResult Editar(int idUsuario)
        {
            var osuario = _UsuariosDatos.Obtener(idUsuario);
            return View(osuario);
        }

        [HttpPost]
        public IActionResult Editar(UsuarioModel oUsuario)
        {
            if (!ModelState.IsValid)
                return View();
            //Recibir un objeto y guardarlo en la base de datos
            var respuesta = _UsuariosDatos.Editar(oUsuario);
            if (respuesta)
                return RedirectToAction("Listar");
            else

                return View();

        }

        public IActionResult Eliminar(int idUsuario)
        {
            var oUsuario = _UsuariosDatos.Obtener(idUsuario);
            return View(oUsuario);
        }
        [HttpPost]

        public IActionResult Eliminar(UsuarioModel oUsuario)
        {
            var respuesta = _UsuariosDatos.Eliminar(oUsuario.idUsuario);
            if (respuesta)
                return RedirectToAction("Listar");
            else
                return View();
        }
    }
}
