using gestionactivos.Configurations;
using gestionactivos.Models;
using gestionactivos.Data;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.IO;
using System.Data;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using gestionactivos.pdf;
using gestionactivos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace gestionactivos.Controllers
{
    [Authorize(Policy = "GlobalPolicy")]
    [Authorize(Roles = "Administrador,Coordinador,Tecnico")]
    public class AsignacionController : Controller
    {



        private readonly CorreoService _correoService;
        private int? idUsuario;

        // Este método se ejecuta antes de cada acción del controlador
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var idUsuarioClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            idUsuario = idUsuarioClaim != null ? (int?)Convert.ToInt32(idUsuarioClaim.Value) : null;
            base.OnActionExecuting(context);
        }



        public AsignacionController(CorreoService correoService)
        {
            _correoService = correoService;
        }

        public IActionResult Asignacion()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ResultadoCedula(string Cedula)
        {
            AsignacionData data = new AsignacionData();
            AsignacionModel funcionario = data.ConsultarCedula(Cedula, idUsuario);

            if (funcionario != null)
            {
                return Json(new { success = true, data = funcionario });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public IActionResult ResultadoPlaca(string Placa)
        {
            AsignacionData data2 = new AsignacionData();
            List<AsignacionModel> articulos = new List<AsignacionModel>();
            string errorMessage;

            try
            {
                articulos = data2.ConsultarPlaca(Placa, idUsuario, out errorMessage);
            }
            catch (SqlException ex)
            {
                return Json(new { success = false, errorMessage = ex.Message });
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                // Si hay un error en el mensaje
                return Json(new { success = false, errorMessage = errorMessage });
            }

            if (articulos != null && articulos.Count > 0)
            {
                return Json(new { success = true, data2 = articulos });
            }
            else
            {
                return Json(new { success = false, errorMessage = "No se encontraron resultados para la Placa del Artículo." });
            }
        }

        [HttpPost]
        public IActionResult ResultadoCedulaContra(string cedulaContra)
        {
            AsignacionData data3 = new AsignacionData();

            AsignacionModel contratista = data3.ConsultarCedulaContra(cedulaContra, idUsuario);
            if (contratista != null)
            {
                return Json(new { success = true, data = contratista });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public IActionResult ResultadoPlacaAdicional(string PlacaAdicional)
        {
            AsignacionData data4 = new AsignacionData();
            AsignacionModel adicionales = data4.ConsultarPlacaAdicional(PlacaAdicional, idUsuario);
            if (adicionales != null)
            {
                return Json(new { success = true, data = adicionales });
            }
            else
            {
                return Json(new { success = false });

            }

        }

        [HttpPost]
        public IActionResult AgregarPlacaAdicional(int IdArticulo, int IdAdicionalAgregar)
        {
            AsignacionData data4 = new AsignacionData();
            var resultado = data4.AgregarAdicional(IdArticulo, IdAdicionalAgregar, idUsuario);
            return Json(new { success = resultado.Success, message = resultado.Message });
        }



        [HttpPost]
        public IActionResult SaveSignatureFuncionario(int idFuncionario, string dataURL)
        {
            try
            {
                AsignacionData data5 = new AsignacionData();
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
                AsignacionData data6 = new AsignacionData();
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
        public IActionResult AsignarEquipo(int idFuncionario, int idArticulo, int idFuncionarioContra, int? estadoCheckbox)
        {
            // Obtener el idUsuario del Claim en la sesión
            var idUsuarioClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (idUsuarioClaim == null || !int.TryParse(idUsuarioClaim.Value, out int idUsuario))
            {
                // Manejar el caso en que el Claim no esté presente o no sea válido
                return Json(new { success = false, errorMessage = "No se pudo obtener el idUsuario de la sesión." });
            }

            AsignacionData data7 = new AsignacionData();
            AsignacionModel AsignarEquipoTermi = null;
            try
            {
                // Llamar al método AsignacionEquipoTerminada con el idUsuario obtenido de la sesión
                AsignarEquipoTermi = data7.AsignacionEquipoTerminada(idFuncionario, idArticulo, idFuncionarioContra, idUsuario, estadoCheckbox);

                if (AsignarEquipoTermi != null)
                {
                    // Asignación exitosa, activar la validación para EnviarIdAsignacion
                    TempData["AsignacionExitosa"] = true;
                    return Json(new { success = true, data = AsignarEquipoTermi, idAsignacion = AsignarEquipoTermi.IdAsignacion });
                }
                else
                {
                    // Algo salió mal con la asignación
                    return Json(new { success = false, errorMessage = "No se pudo completar la asignación del equipo." });
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = ex.Message; // Captura el mensaje de error SQL
                return Json(new { success = false, errorMessage = errorMessage });
            }
        }


        [HttpPost]
        public IActionResult EnviarIdAsignacion(int idAsignacion)
        {
            // Verificar si la asignación de equipo fue exitosa antes de proceder
            if (TempData["AsignacionExitosa"] != null && (bool)TempData["AsignacionExitosa"])
            {
                // Limpiar TempData después de usarlo
                TempData["AsignacionExitosa"] = false;

                // Enviar el correo electrónico usando CorreoService
                bool envioExitoso = _correoService.EnviarCorreo(idAsignacion);

                if (envioExitoso)
                {
                    return Json(new { success = true, mensaje = "Correo enviado correctamente." });
                }
                else
                {
                    return Json(new { success = false, errorMessage = "No se pudo enviar el correo electrónico." });
                }
            }
            else
            {
                return Json(new { success = false, errorMessage = "La asignación de equipo debe completarse correctamente antes de enviar la asignación." });
            }
        }









    }
}
