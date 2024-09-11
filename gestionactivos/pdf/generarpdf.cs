using gestionactivos.Data;
using gestionactivos.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace gestionactivos.pdf
{
    public class generarpdf
    {
        public string GeneratePDF(int idAsignacion)
        {
            // Obtener los datos de la base de datos
            GenerarpdfData data = new GenerarpdfData();
            List<GenerarpdfModel> asignaciones = data.GenerarDocumento(idAsignacion);

            // Generar el contenido HTML
            string htmlContent = GetHtmlContent(asignaciones);

            // Ruta de salida del PDF
            string outputPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                                             string.Format("{0}.pdf", DateTime.Now.ToString("ddMMyyyyHHmmss")));

            // Generar el PDF
            using (FileStream stream = new FileStream(outputPath, FileMode.Create))
            {
                Document pdfDoc = new Document(PageSize.A4, 30f, 30f, 30f, 30f);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();

                using (var stringReader = new StringReader(htmlContent))
                {
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, stringReader);
                }

                pdfDoc.Close();
            }

            return outputPath;
        }

        private string GetHtmlContent(List<GenerarpdfModel> asignaciones)
        {
            // Ruta correcta del archivo HTML basada en el directorio del proyecto
            string projectRootPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\pdf"));
            string templatePath = Path.Combine(projectRootPath, "template.html");

            if (!File.Exists(templatePath))
            {
                throw new FileNotFoundException("No se encontró el archivo template.html", templatePath);
            }

            string htmlTemplate = File.ReadAllText(templatePath);

            // Reemplazar marcador de posición para el ID de asignación
            htmlTemplate = htmlTemplate.Replace("{idAsignacion}", asignaciones[0].idAsignacion.ToString());

            // Generar las filas de la tabla
            StringBuilder rowsBuilder = new StringBuilder();
            foreach (var asignacion in asignaciones)
            {
                rowsBuilder.Append($@"
            <tr>
                <td>{asignacion.idAsignacion}</td>
            </tr>");
            }

            // Reemplazar marcador de posición para las filas de la tabla
            string htmlContent = htmlTemplate.Replace("{rows}", rowsBuilder.ToString());

            return htmlContent;
        }

    }
}
