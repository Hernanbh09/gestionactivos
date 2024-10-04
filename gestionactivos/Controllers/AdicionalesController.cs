using gestionactivos.Data;
using gestionactivos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace gestionactivos.Controllers
{
    [Authorize(Policy = "GlobalPolicy")]
    [Authorize(Roles = "Administrador,Coordinador")] 

    public class AdicionalesController : Controller
    {


        AdicionalesData _AdicionalesData = new AdicionalesData();

        public IActionResult Listar()
        {

            var oLista = _AdicionalesData.Listar();

            return View(oLista);
        }
        public IActionResult Guardar()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Guardar(AdicionalesModel oAdicional)
        {
            if (!ModelState.IsValid)
                return View();

            var mensaje = _AdicionalesData.Guardar(oAdicional);
            if (string.IsNullOrEmpty(mensaje))
                return RedirectToAction("Listar");
            else
                ViewBag.ErrorMessage = mensaje;
            return View();
        }


        public IActionResult Editar(int idAdicional)
        {
            var oadicional = _AdicionalesData.Obtener(idAdicional);
            return View(oadicional);
        }
        [HttpPost]
        public IActionResult Editar(AdicionalesModel oAdicional)
        {
            if (!ModelState.IsValid)
                return View();



            //Recibir un objeto y guardarlo en la base de datos
            var respuesta = _AdicionalesData.Editar(oAdicional);
            if (respuesta)
                return RedirectToAction("Listar");
            else

                return View();
        }
        public IActionResult Eliminar(int idAdicional)
        {
            var oAdicional = _AdicionalesData.Obtener(idAdicional);
            return View(oAdicional);

        }
        [HttpPost]
        public IActionResult Eliminar(AdicionalesModel oAdicional)
        {
            var respuesta = _AdicionalesData.Eliminar(oAdicional.idAdicional);
            if (respuesta)
                return RedirectToAction("Listar");
            else
                return View();
        }



    }
}
