using Microsoft.AspNetCore.Mvc;
using gestionactivos.Data;
using gestionactivos.Models;


namespace gestionactivos.Controllers
{
    public class FuncionariosController : Controller
    {
        FuncionariosData _ContactoDatos = new FuncionariosData();

        public IActionResult Listar()
        {
            //La visata de mostar contactos

            var oLista = _ContactoDatos.Listar();

            return View(oLista);
        }

        public IActionResult Guardar()
        {
            var clientes = _ContactoDatos.ListarClientes();

            // Pasar la lista de clientes a la vista usando ViewData
            ViewData["Clientes"] = clientes;

            //Metodo solo devuelve la vista
            return View();
        }
        [HttpPost]
        public IActionResult Guardar(FuncionarioModel oContacto, int idCliente, int idSedes)
        {

            if(!ModelState.IsValid)
                return View();

            oContacto.idClientes = idCliente;
            oContacto.idSedes = idSedes;


            // Guarda el contacto usando la capa de datos
            var data = new FuncionariosData();
            bool resultado = data.Guardar(oContacto, idSedes);

            if (resultado)
            {
                // Redirige a la lista si el guardado fue exitoso
                return RedirectToAction("Listar");
            }
            else
            {
                // Maneja el caso en que el guardado falló
                ModelState.AddModelError("", "Error al guardar el funcionario.");
                return View(oContacto);
            }
        }
        [HttpPost]
        public JsonResult ObtenerSedes(int idCliente)
        {
            var sedes = _ContactoDatos.ListarSedes(idCliente);
            return Json(sedes);
        }


        public IActionResult Editar(int idFuncionario)
        {
            //Metodo solo devuelve la vista
             var ocontacto = _ContactoDatos.Obtener(idFuncionario);
            return View(ocontacto);
        }

        [HttpPost]
        public IActionResult Editar(FuncionarioModel oContacto)
        {

            if (!ModelState.IsValid)
                return View();



            //Recibir un objeto y guardarlo en la base de datos
            var respuesta = _ContactoDatos.Editar(oContacto);
            if (respuesta)
                return RedirectToAction("Listar");
            else

                return View();
        }


        public IActionResult Eliminar(int idFuncionario)
        {
            //Metodo solo devuelve la vista
             var oContacto = _ContactoDatos.Obtener(idFuncionario);
            return View(oContacto);
        }

        [HttpPost]
        public IActionResult Eliminar(FuncionarioModel oContacto)
        {

            //Recibir un objeto y guardarlo en la base de datos
            var respuesta = _ContactoDatos.Eliminar(oContacto.idFuncionario);
            if (respuesta)
                return RedirectToAction("Listar");
            else

                return View();
        }
    }
}
