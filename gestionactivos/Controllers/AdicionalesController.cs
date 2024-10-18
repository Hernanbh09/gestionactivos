using gestionactivos.Data;
using gestionactivos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace gestionactivos.Controllers
{
    [Authorize(Policy = "GlobalPolicy")]
    [Authorize(Roles = "Administrador,Coordinador")] 

    public class AdicionalesController : Controller
    {


        AdicionalesData _AdicionalesData = new AdicionalesData();

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
           
            var oLista = _AdicionalesData.Listar(idUsuario);

            return View(oLista);
        }
        public IActionResult Guardar()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Guardar(AdicionalesModel oAdicional)
        {
            

            if (!ModelState.IsValid)
                return View();

            var mensaje = _AdicionalesData.Guardar(oAdicional, idUsuario);
            if (string.IsNullOrEmpty(mensaje))
                return RedirectToAction("Listar");
            else
                ViewBag.ErrorMessage = mensaje;
            return View();
        }


        public IActionResult Editar(int idAdicional)
        {
            var oadicional = _AdicionalesData.Obtener(idAdicional, idUsuario);
            return View(oadicional);
        }
        [HttpPost]
        public IActionResult Editar(AdicionalesModel oAdicional)
        {
            if (!ModelState.IsValid)
                return View();



            //Recibir un objeto y guardarlo en la base de datos
            var respuesta = _AdicionalesData.Editar(oAdicional, idUsuario);
            if (respuesta)
                return RedirectToAction("Listar");
            else

                return View();
        }
        public IActionResult Eliminar(int idAdicional)
        {
            var oAdicional = _AdicionalesData.Obtener(idAdicional, idUsuario);
            return View(oAdicional);

        }
        [HttpPost]
        public IActionResult Eliminar(AdicionalesModel oAdicional)
        {
            var respuesta = _AdicionalesData.Eliminar(oAdicional.idAdicional, idUsuario);
            if (respuesta)
                return RedirectToAction("Listar");
            else
                return View();
        }



    }
}
