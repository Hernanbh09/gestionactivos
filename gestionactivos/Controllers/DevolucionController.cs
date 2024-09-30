using gestionactivos.Data;
using gestionactivos.Models;
using gestionactivos.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.IO;
using System.Security.Claims;

namespace gestionactivos.Controllers
{
    public class DevolucionController : Controller
    {
        private readonly ICompositeViewEngine _viewEngine;
        private readonly CorreoService _correoService;
        // Constructor que inyecta el motor de vistas
        public DevolucionController(ICompositeViewEngine viewEngine, CorreoService correoService)
        {
            _correoService = correoService;
            _viewEngine = viewEngine;
        }

        public IActionResult Devolucion()
        {
            return View();
        }

        [HttpPost]

        public IActionResult Devolucion(string cedula)
        {
            if (!string.IsNullOrWhiteSpace(cedula))
            {
                DevolucionData data = new DevolucionData();
                List<DevolucionModel> funcionarios = data.ConsultarCedula(cedula);

                if (funcionarios != null && funcionarios.Any())
                {
                    ViewBag.Funcionarios = funcionarios;
                }
                else
                {
                    ViewBag.ErrorMessage = "No se encontraron resultados.";
                }
            }

            return View();
        }



        private string RenderViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = _viewEngine.FindView(ControllerContext, viewName, false);
                ViewContext viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    ViewData,
                    TempData,
                    sw,
                    new HtmlHelperOptions()
                );
                viewResult.View.RenderAsync(viewContext).Wait();
                return sw.GetStringBuilder().ToString();
            }
        }



        [HttpPost]
        public IActionResult SaveSignatureFuncionario(int idFuncionario, string dataURL)
        {
            try
            {
                DevolucionData data5 = new DevolucionData();
                bool GuardarFirmaFunc = data5.GuardarFirmaFuncionario(idFuncionario, dataURL);
                if (GuardarFirmaFunc)
                {
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

        [HttpPost]
        public IActionResult SaveSignatureContratista(int idContratista, string dataURLR)
        {
            try
            {
                DevolucionData data6 = new DevolucionData();
                bool GuardarFirmaFunc = data6.GuardarFirmaContratista(idContratista, dataURLR);
                if (GuardarFirmaFunc)
                {
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



        [HttpPost]
        public IActionResult DevolverEquipo(int idFuncionario, int idFuncionarioContra, int idArticulo, int idMovimiento, string Observacion)
        {
            var idUsuarioClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (idUsuarioClaim == null || !int.TryParse(idUsuarioClaim.Value, out int idUsuario))
            {
                return Json(new { success = false, errorMessage = "No se pudo obtener el idUsuario de la sesión." });
            }

            DevolucionData data = new DevolucionData();
            DevolucionModel devolucionEquipoTerminado = null;
            try
            {
                devolucionEquipoTerminado = data.DevolucionEquipoTerminado(idFuncionario, idFuncionarioContra, idArticulo, idUsuario, idMovimiento, Observacion);

                // Utiliza el idMovimientos retornado por el método
                if (devolucionEquipoTerminado.IdMovimientos != 0)
                {
                    bool envioExitoso = _correoService.EnviarCorreo(devolucionEquipoTerminado.IdMovimientos);
                    if (!envioExitoso)
                    {
                        return Json(new { success = false, errorMessage = "El correo no se pudo enviar." });
                    }
                }
                else
                {
                    return Json(new { success = false, errorMessage = "No se pudo obtener el idMovimientos." });
                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message; // Captura el mensaje de error SQL
                return Json(new { success = false, errorMessage = errorMessage });
            }

            return Json(new { success = true, idMovimientos = devolucionEquipoTerminado.IdMovimientos }); // Devuelve el idMovimientos
        }

    }
}
