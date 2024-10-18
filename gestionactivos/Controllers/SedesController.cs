using gestionactivos.Data;
using gestionactivos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace gestionactivos.Controllers
{
    [Authorize(Policy = "GlobalPolicy")]
    [Authorize(Roles = "Administrador")]
    public class SedesController : Controller
    {

        SedesData _SedesData = new SedesData();


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
            var oLista = _SedesData.Listar(idUsuario);

            return View(oLista);
        }
        public IActionResult Guardar()
        {
            var listaClientes = _SedesData.ListarC(); // Obtén la lista de clientes
            ViewData["Clientes"] = listaClientes;
            return View();
        }
        [HttpPost]
        public IActionResult Guardar(SedesModel oSedes)
        {
            string mensajeError;
            var resultado = _SedesData.Guardar(oSedes, out mensajeError, null);

            if (!resultado)
            {
                ViewBag.ErrorMessage = mensajeError;
                var listaClientes = _SedesData.ListarC(); // Vuelve a cargar la lista de clientes si falla el guardado
                ViewData["Clientes"] = listaClientes;
                return View(oSedes);
            }

            return RedirectToAction("Listar"); // Redirige a la lista de sedes después de guardar exitosamente
        }


        public IActionResult Editar(int idSedes)
        {
            var idUsuarioClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            int? idUsuario = idUsuarioClaim != null ? (int?)Convert.ToInt32(idUsuarioClaim.Value) : null;
            string mensajeError;

            var oSede = _SedesData.Obtener(idSedes, out mensajeError, idUsuario);

            if (oSede == null || oSede.idSedes == 0)
            {
                ViewBag.ErrorMessage = "Sede no encontrada.";
                return RedirectToAction("Listar");
            }

            // Cargar la lista de clientes para el select
            var listaClientes = _SedesData.ListarC();
            ViewData["Clientes"] = listaClientes;

            return View(oSede);
        }



        [HttpPost]
        public IActionResult Editar(SedesModel oSedes)
        {
            var idUsuarioClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            int? idUsuario = idUsuarioClaim != null ? (int?)Convert.ToInt32(idUsuarioClaim.Value) : null;
            string mensajeError;

            if (!ModelState.IsValid)
                return View(oSedes); // Asegúrate de devolver el modelo con errores

            // Recibir un objeto y guardarlo en la base de datos
            var respuesta = _SedesData.Editar(oSedes, out mensajeError, idUsuario);

            if (respuesta)
            {
                ViewBag.SuccessMessage = mensajeError;
            }
            else
            {
                ViewBag.ErrorMessage = mensajeError; // Mensaje de error // Pasar el mensaje de error a la vista
            }
            return View(oSedes);
        }



        public IActionResult Eliminar(int idSedes)
        {
            var idUsuarioClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            int? idUsuario = idUsuarioClaim != null ? (int?)Convert.ToInt32(idUsuarioClaim.Value) : null;
            string mensajeError;
            var oSedes = _SedesData.Obtener(idSedes, out mensajeError, idUsuario);
            return View(oSedes);
        }

        [HttpPost]
        public IActionResult Eliminar(SedesModel oSedes)
        {
            var idUsuarioClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            int? idUsuario = idUsuarioClaim != null ? (int?)Convert.ToInt32(idUsuarioClaim.Value) : null;
            string mensajeError;

            var respuesta = _SedesData.Eliminar(oSedes.idSedes, out mensajeError, idUsuario); // Llamada al método Eliminar de la capa de datos
            if (respuesta)
                return RedirectToAction("Listar");
            else
                return View();


        }



    }
}
