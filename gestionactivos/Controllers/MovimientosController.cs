using gestionactivos.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace gestionactivos.Controllers
{
    [Authorize(Policy = "GlobalPolicy")] // Aplicar la política global
    [Authorize(Roles = "Administrador,Coordinador,Tecnico")]
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
