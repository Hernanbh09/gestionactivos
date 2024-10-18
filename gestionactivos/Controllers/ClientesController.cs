using gestionactivos.Data;
using gestionactivos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace gestionactivos.Controllers
{
    [Authorize(Policy = "GlobalPolicy")]
    [Authorize(Roles = "Administrador")]

    public class ClientesController : Controller
    {

        ClientesData _ClientesDatos = new ClientesData();

        public IActionResult Listar()
        {
            var idUsuarioClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            int? idUsuario = idUsuarioClaim != null ? (int?)Convert.ToInt32(idUsuarioClaim.Value) : null;

            var oLista = _ClientesDatos.Listar(idUsuario);
            return View(oLista);
        }

        public IActionResult Guardar()
        {
            return View();

        }

        [HttpPost]
        public IActionResult Guardar(ClientesModel oClientes)
        {
            if (!ModelState.IsValid)
                return View();

            var idUsuarioClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            int? idUsuario = idUsuarioClaim != null ? (int?)Convert.ToInt32(idUsuarioClaim.Value) : null;

            string mensajeError;
            var respuesta = _ClientesDatos.Guardar(oClientes, out mensajeError, idUsuario);

            if (respuesta)
            {
                TempData["Message"] = mensajeError; // Mensaje de éxito
                TempData["MessageType"] = "success";
                return RedirectToAction("Listar");
            }
            else
            {
                ViewBag.ErrorMessage = mensajeError; // Mensaje de error
                return View();
            }
        }

        public IActionResult Editar(int idClientes)
        {
            var idUsuarioClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            int? idUsuario = idUsuarioClaim != null ? (int?)Convert.ToInt32(idUsuarioClaim.Value) : null;
            string mensajeError;

            var oUsuario = _ClientesDatos.Obtener(idClientes, out mensajeError, idUsuario);

            if (oUsuario == null || oUsuario.idClientes == 0)
            {
                ViewBag.ErrorMessage = "Cliente no encontrado.";
                return RedirectToAction("Listar");
            }

            return View(oUsuario);
        }

        [HttpPost]
        public IActionResult Editar(ClientesModel oClientes)
        {
            var idUsuarioClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            int? idUsuario = idUsuarioClaim != null ? (int?)Convert.ToInt32(idUsuarioClaim.Value) : null;
            string mensajeError;

            if (!ModelState.IsValid)
                return View(oClientes); // Asegúrate de devolver el modelo con errores

            // Recibir un objeto y guardarlo en la base de datos
            var respuesta = _ClientesDatos.Editar(oClientes, out mensajeError, idUsuario);

            if (respuesta)
            {
                ViewBag.SuccessMessage = mensajeError;

            }
            else
            {
                ViewBag.ErrorMessage = mensajeError; // Mensaje de error // Pasar el mensaje de error a la vista
            }
            return View(oClientes);
        }

        public IActionResult Eliminar(int idClientes)
        {
            var idUsuarioClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            int? idUsuario = idUsuarioClaim != null ? (int?)Convert.ToInt32(idUsuarioClaim.Value) : null;
            string mensajeError;
            var oClientes = _ClientesDatos.Obtener(idClientes, out mensajeError, idUsuario);
            return View(oClientes);
        }

        [HttpPost]
        public IActionResult Eliminar(ClientesModel oClientes)
        {
            var idUsuarioClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            int? idUsuario = idUsuarioClaim != null ? (int?)Convert.ToInt32(idUsuarioClaim.Value) : null;
            string mensajeError;
 
            var respuesta = _ClientesDatos.Eliminar(oClientes.idClientes, out mensajeError, idUsuario); // Llamada al método Eliminar de la capa de datos
            if (respuesta)
                return RedirectToAction("Listar");
            else
                return View();
        }



    }
}
