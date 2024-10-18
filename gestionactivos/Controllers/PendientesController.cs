using gestionactivos.Data;
using gestionactivos.Models;
using gestionactivos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace gestionactivos.Controllers
{
    [Authorize(Policy = "GlobalPolicy")] // Aplicar la política global
    [Authorize(Roles = "Administrador,Coordinador,Tecnico")]
    public class PendientesController : Controller
    {
        // Método GET para listar los pendientes
        private readonly CorreoService _correoService;
        private int? idUsuario;

        // Este método se ejecuta antes de cada acción del controlador
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var idUsuarioClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            idUsuario = idUsuarioClaim != null ? (int?)Convert.ToInt32(idUsuarioClaim.Value) : null;
            base.OnActionExecuting(context);
        }




        public PendientesController(CorreoService correoService)
        {
            _correoService = correoService;
        }
        public IActionResult Pendientes()
        {
            PendientesData data = new PendientesData();
            List<PendientesModel> listaPendientes = data.Listar(idUsuario); // Llamada al método Listar()
            return View(listaPendientes); // Pasar los datos a la vista
        }

        [HttpPost]
        public IActionResult SaveSignatureFuncionario(int idFuncionario, string dataURL, int idMovimientos)
        {
            try
            {
                PendientesData data5 = new PendientesData();
                bool GuardarFirmaFunc = data5.GuardarFirmaFuncionario(idFuncionario, dataURL);
                if (GuardarFirmaFunc)
                {
                    bool ActuaizarPendite = data5.ActualizaPendiente(idMovimientos);

                    bool envioExitoso = _correoService.EnviarCorreo(idMovimientos);
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "Error al guardar la firma en la base de datos." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
