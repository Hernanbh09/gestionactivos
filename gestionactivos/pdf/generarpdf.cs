using gestionactivos.Data;
using gestionactivos.Error;
using gestionactivos.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace gestionactivos.pdf
{
    public class generarpdf
    {
        public string GeneratePDF(int idAsignacion)
        {
            var errorLogger = new ErrorLogger();
            try
            {
                // Obtener los datos de la base de datos
                GenerarpdfData data = new GenerarpdfData();
                List<GenerarpdfModel> asignaciones = data.GenerarDocumento(idAsignacion);

                if (asignaciones == null || asignaciones.Count == 0)
                {
                    throw new Exception("No se encontraron asignaciones para el ID proporcionado.");
                }

                var FechaMovimiento = asignaciones[0].FechaMovimiento;
                string fechaMovimientoStr = FechaMovimiento.ToString("dd_MM_yyyy");
                if (asignaciones[0].EventoMovimiento == "Asignación - Histórica" || asignaciones[0].EventoMovimiento == "Devolucion - Historica")
                {
                    asignaciones[0].EventoMovimiento = "Devolución";
                }
                else if (asignaciones[0].EventoMovimiento == "Pendiente Firmar")
                {
                    asignaciones[0].EventoMovimiento = "Pendiente";
                }

                var NombrePdf = $"{asignaciones[0].EventoMovimiento}_{asignaciones[0].SerialArticulo}_{fechaMovimientoStr}_{asignaciones[0].idAsignacion}.pdf";

                // Ruta de salida del PDF usando Documentos del usuario
                string documentosPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                // Cambia la definición de la ruta del repositorio a D:\
                string repositorioPath = @"D:\RepositorioGestionActivos";

                // Crea la carpeta si no existe
                if (!Directory.Exists(repositorioPath))
                {
                    Directory.CreateDirectory(repositorioPath);
                }

                // Define la ruta completa del PDF
                string outputPath = Path.Combine(repositorioPath, NombrePdf);


                // Instanciar el generador de plantillas y pasar los datos
                Template template = new Template();
                template.GeneratePdf(outputPath, asignaciones); // Pasar la lista de asignaciones a Template

                byte[] pdfBytes = File.ReadAllBytes(outputPath);
                string pdfBase64 = Convert.ToBase64String(pdfBytes);

                var EnviarArchivo = data.Guardarpdf(idAsignacion, outputPath, NombrePdf, pdfBase64);

                // Abrir el PDF en el navegador predeterminado
                // Process.Start(new ProcessStartInfo(outputPath) { UseShellExecute = true });

                return outputPath;
            }
            catch (Exception ex)
            {
                // Manejar la excepción según sea necesario
                errorLogger.RegistrarError(ex, "Generarpdf Template Metodo:" + nameof(GeneratePDF));
                throw; // Esto lanzará de nuevo la excepción después de registrarla
            }
        }

    }
}
