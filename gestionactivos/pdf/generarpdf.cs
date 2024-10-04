using gestionactivos.Data;
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
            // Obtener los datos de la base de datos
            GenerarpdfData data = new GenerarpdfData();
            List<GenerarpdfModel> asignaciones = data.GenerarDocumento(idAsignacion);


            var FechaMovimiento = asignaciones[0].FechaMovimiento;
            string fechaMovimientoStr = FechaMovimiento.ToString("dd_MM_yyyy");
            if (asignaciones[0].EventoMovimiento == "Asignación - Histórica" || asignaciones[0].EventoMovimiento == "Devolucion - Historica")
            {
                asignaciones[0].EventoMovimiento = "Devolución";
            }else if(asignaciones[0].EventoMovimiento == "Pendiente Firmar")
            {
                asignaciones[0].EventoMovimiento = "Pendiente";
            }

            var NombrePdf = $"{asignaciones[0].EventoMovimiento}_{asignaciones[0].SerialArticulo}_{fechaMovimientoStr}_{asignaciones[0].idAsignacion}.pdf";

            // Ruta de salida del PDF
            string outputPath = Path.Combine(@"C:\Users\TI1\Documents\RepositorioGestionActivos", NombrePdf);

            // Instanciar el generador de plantillas y pasar los datos
            Template template = new Template();
            template.GeneratePdf(outputPath, asignaciones); // Pasar la lista de asignaciones a Template


            byte[] pdfBytes = File.ReadAllBytes(outputPath);
            string pdfBase64 = Convert.ToBase64String(pdfBytes);


            var EnviarArchivo = data.Guardarpdf(idAsignacion,outputPath, NombrePdf, pdfBase64);

            // Abrir el PDF en el navegador predeterminado
           // Process.Start(new ProcessStartInfo(outputPath) { UseShellExecute = true });

            return outputPath;
        }
    }
}
