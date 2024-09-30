using gestionactivos.Data;
using Microsoft.AspNetCore.Mvc;

namespace gestionactivos.Controllers
{
    public class MovimientosController : Controller
    {

        MovimientosData _MovimientosData = new MovimientosData();

        public IActionResult Listar()
        {
            var oLista = _MovimientosData.Listar();
            return View(oLista);
        }

    }
}
