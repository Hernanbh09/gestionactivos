using gestionactivos.Configurations;
using gestionactivos.Models;
using gestionactivos.Data;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

using System.Data;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using gestionactivos.pdf;

namespace gestionactivos.Controllers
{
    public class AsignacionController : Controller
    {



        private readonly SmtpSettings _smtpSettings;

        public AsignacionController(IConfiguration configuration)
        {
            _smtpSettings = configuration.GetSection("Smtp").Get<SmtpSettings>();
        }

        public IActionResult Asignacion()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ResultadoCedula(string Cedula)
        {
            AsignacionData data = new AsignacionData();
            AsignacionModel funcionario = data.ConsultarCedula(Cedula);

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
            List<AsignacionModel> articulos = data2.ConsultarPlaca(Placa);

            if (articulos != null && articulos.Count > 0)
            {
                return Json(new { success = true, data2 = articulos });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public IActionResult ResultadoCedulaContra(string cedulaContra)
        {
            AsignacionData data3 = new AsignacionData();

            AsignacionModel contratista = data3.ConsultarCedulaContra(cedulaContra);
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
            AsignacionModel adicionales = data4.ConsultarPlacaAdicional(PlacaAdicional);
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
            bool resultado = data4.AgregarAdicional(IdArticulo, IdAdicionalAgregar);
            if (resultado)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
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
        public IActionResult AsignarEquipo(int idFuncionario, int idArticulo, int idFuncionarioContra)
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
                AsignarEquipoTermi = data7.AsignacionEquipoTerminada(idFuncionario, idArticulo, idFuncionarioContra, idUsuario);

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

                // Proceder con la lógica de EnviarIdAsignacion
                //AsignacionData data8 = new AsignacionData();

                // Enviar el correo electrónico
                bool envioExitoso = EnviarCorreo(idAsignacion);

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
                // La asignación de equipo no fue exitosa, no se puede proceder con EnviarIdAsignacion
                return Json(new { success = false, errorMessage = "La asignación de equipo debe completarse correctamente antes de enviar la asignación." });
            }
        }

        private bool EnviarCorreo(int idAsignacion)
        {
            try
            {
                // Generar el PDF
                generarpdf pdfGenerator = new generarpdf();
                string pdfPath = pdfGenerator.GeneratePDF(idAsignacion); //archivo generado

                using (var client = new SmtpClient(_smtpSettings.Host, 587)) // Cambiado a 587
                {
                    client.EnableSsl = _smtpSettings.EnableSsl;
                    client.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);

                    var mensaje = new MailMessage
                    {
                        From = new MailAddress(_smtpSettings.From, _smtpSettings.FromName),
                        Subject = "Asunto del correo",
                        Body = $"Se ha creado un documento con id de asignación: {idAsignacion}",
                        IsBodyHtml = true
                    };

                    mensaje.To.Add("hernandezbrayan697@gmail.com");

                    // Adjuntar el PDF al correo
                    Attachment pdfAttachment = new Attachment(pdfPath);
                    mensaje.Attachments.Add(pdfAttachment);

                    client.Send(mensaje);
                }

                return true;
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"Error al enviar el correo electrónico: {ex.Message}");
                Console.WriteLine($"Código de error SMTP: {ex.StatusCode}");
                Console.WriteLine($"Detalles: {ex}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general al enviar el correo electrónico: {ex.Message}");
                return false;
            }
        }










    }
}
