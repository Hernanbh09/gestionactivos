using gestionactivos.Data;
using gestionactivos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.IO;

namespace gestionactivos.Controllers
{
    public class DevolucionController : Controller
    {
        private readonly ICompositeViewEngine _viewEngine;

        // Constructor que inyecta el motor de vistas
        public DevolucionController(ICompositeViewEngine viewEngine)
        {
            _viewEngine = viewEngine;
        }

        public IActionResult Devolucion()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ResultadoCedula(string cedula)
        {
            // Validar que la cédula no esté vacía
            if (string.IsNullOrWhiteSpace(cedula))
            {
                return Json(new { success = false, message = "Ingrese una cédula." });
            }

            DevolucionData data = new DevolucionData();
            List<DevolucionModel> funcionarios = data.ConsultarCedula(cedula);

            if (funcionarios != null && funcionarios.Any())
            {
                // Renderizar la vista parcial y devolverla como HTML en la respuesta
                string html = RenderViewToString("_DevolucionTable", funcionarios);
                return Json(new { success = true, html = html });
            }
            else
            {
                return Json(new { success = false, message = "No se encontraron resultados." });
            }
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



    }
}
