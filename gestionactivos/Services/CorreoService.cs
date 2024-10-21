using gestionactivos.Configurations;
using gestionactivos.Data;
using gestionactivos.pdf;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace gestionactivos.Services
{
    public class CorreoService
    {
        private readonly SmtpSettings _smtpSettings;

        // Constructor que inyecta IOptions<SmtpSettings>
        public CorreoService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;  // Aquí obtenemos los valores de configuración de SMTP
        }

        public bool EnviarCorreo(int idAsignacion)
        {
            try
            {
                // Crear instancia de AsignacionData para obtener los correos
                AsignacionData data = new AsignacionData();
                var correos = data.ObtenerCorreoPorAsignacion(idAsignacion);

                // Asegúrate de que las propiedades sean accesibles
                //string correoEncargado = correos.CorreoEncargado;
                //string correoResponsable = correos.CorreoResponsable;

                string evento = correos.evento;



                string correo = "coordinador_parquecomputacional@mintic.gov.co";
                string correo2 = "bhernandezm.sumimas@gmail.com";
                string correoCopia = "Soporte_equipos@mintic.gov.co";
                


                // Verificar que las propiedades de SMTP no sean nulas
                if (_smtpSettings == null || string.IsNullOrEmpty(_smtpSettings.From) || string.IsNullOrEmpty(_smtpSettings.FromName))
                {
                    Console.WriteLine("Configuración de SMTP incompleta.");
                    return false;
                }

                // Generar el PDF
                generarpdf pdfGenerator = new generarpdf();
                string pdfPath = pdfGenerator.GeneratePDF(idAsignacion); // Archivo generado con datos

                using (var client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port))
                {
                    client.EnableSsl = _smtpSettings.EnableSsl;
                    client.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);

                    var mensaje = new MailMessage
                    {
                        From = new MailAddress(_smtpSettings.From, _smtpSettings.FromName),

                        IsBodyHtml = true


                        //Body = "Body del correo aquí",  // Cambia según sea necesario
                        //IsBodyHtml = true

                    };
                    // Verificar el tipo de evento para definir el cuerpo del correo

                    // Verificar el tipo de evento para definir el Subject y el Body del correo
                    if (evento == "Devolucion - Historica")
                    {
                        mensaje.Subject = "Devolución de equipo";
                        mensaje.Body = $"<html><body>" +
                                       $"<p>Estimado usuario,</p>" +
                                       $"<p>Le informamos que se ha realizado una nueva asignación de equipo.</p>" +
                                       $"<p>Revisa el adjunto para más detalles </p>" +
                                       $"<img src='https://i.ibb.co/WynBGXj/Devolucion.png' alt='Devolución' />" +
                                       $"<p>Saludos,</p>" +
                                       $"</body></html>";
                    }
                    else
                    {
                        mensaje.Subject = "Asignación de equipo";
                        mensaje.Body = $"<html><body>" +
                                       $"<p>Estimado usuario,</p>" +
                                       $"<p>Le informamos que se ha realizado una nueva asignación de equipo.</p>" +
                                        $"<p>Revisa el adjunto para más detalles  </p>" +
                                       $"<img src='https://i.ibb.co/B2XqFgm/Asignacion.png' alt='Asignación' />" +
                                       $"<p>Saludos,</p>" +
                                       $"</body></html>";
                    }
                    // Verificar y agregar destinatario
                    //if (!string.IsNullOrEmpty(correoEncargado))
                    //{
                    //    mensaje.To.Add(correoEncargado);

                    //}
                    //else
                    //{
                    //    Console.WriteLine("No se encontró el correo del encargado.");
                    //    return false;
                    //}

                    //// Agregar correo del responsable como CC si existe
                    //if (!string.IsNullOrEmpty(correoResponsable))
                    //{
                    //    mensaje.CC.Add(correoResponsable);
                    //}
                    mensaje.To.Add(correo);
                    mensaje.To.Add(correo2);
                    mensaje.CC.Add(correoCopia);

                    // Adjuntar el PDF al correo
                    Attachment pdfAttachment = new Attachment(pdfPath);
                    mensaje.Attachments.Add(pdfAttachment);

                    // Enviar correo
                    client.Send(mensaje);
                }

                return true;
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"Error al enviar el correo electrónico: {ex.Message}");
                Console.WriteLine($"Código de error SMTP: {ex.StatusCode}");
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
