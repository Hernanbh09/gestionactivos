using gestionactivos.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace gestionactivos.Controllers
{
    [Authorize(Policy = "GlobalPolicy")] // Aplicar la política global
    [Authorize(Roles = "Administrador,Coordinador,Tecnico")]
    public class MovimientosController : Controller
    {

        MovimientosData _MovimientosData = new MovimientosData();
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
            var oLista = _MovimientosData.Listar(idUsuario);
            return View(oLista);
        }

    }
}
