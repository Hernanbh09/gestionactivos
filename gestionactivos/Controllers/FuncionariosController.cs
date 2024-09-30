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
            if (!ModelState.IsValid)
            {
                // Cargar los clientes nuevamente antes de devolver la vista
                ViewData["Clientes"] = _ContactoDatos.ListarClientes();
                // Opcionalmente, podrías también cargar las sedes si es necesario
                ViewData["Sedes"] = _ContactoDatos.ListarSedes(idCliente); // Cargar sedes por cliente si es necesario

                return View(oContacto); // Devuelve la vista con el modelo y los datos cargados
            }

            oContacto.idClientes = idCliente;
            oContacto.idSedes = idSedes;

            // Guarda el contacto usando la capa de datos
            var data = new FuncionariosData();
            var resultado = data.Guardar(oContacto, idSedes);

            if (resultado.Resultado)
            {
              
                // Redirige a la lista si el guardado fue exitoso
                return RedirectToAction("Listar");
            }
            else
            {
                // Cargar los clientes nuevamente en caso de fallo
                ViewData["Clientes"] = _ContactoDatos.ListarClientes();
                // Cargar las sedes si es necesario
                ViewData["Sedes"] = _ContactoDatos.ListarSedes(idCliente); // Cargar sedes por cliente

                // Maneja el caso en que el guardado falló
                ModelState.AddModelError("", resultado.Mensaje); // Agrega el mensaje de error al ModelState
                return View(oContacto); // Devuelve la vista con el modelo
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
            // Obtener el funcionario a editar
            var oContacto = _ContactoDatos.Obtener(idFuncionario);

            // Comprobar si oContacto es nulo
            if (oContacto == null)
            {
                return NotFound(); // O redirige a una página de error
            }

            // Obtener las listas de clientes
            var clientes = _ContactoDatos.ListarClientes();
            ViewData["Clientes"] = clientes;

            // Comprobar si idClientes es válido antes de obtener las sedes
            var sedes = oContacto.idClientes.HasValue ? _ContactoDatos.ListarSedes(oContacto.idClientes.Value) : new List<FuncionarioModel>();
            ViewData["Sedes"] = sedes;

            return View(oContacto);
        }


        [HttpPost]
        public IActionResult Editar(FuncionarioModel oContacto)
        {
            if (!ModelState.IsValid)
            {
                // Si la validación falla, recargar las listas para la vista
                ViewData["Clientes"] = _ContactoDatos.ListarClientes();
                ViewData["Sedes"] = _ContactoDatos.ListarSedes(oContacto.idClientes ?? 0);
                return View(oContacto);
            }

            // Llamar al método de edición en la capa de datos
            var respuesta = _ContactoDatos.Editar(oContacto);
            if (respuesta)
                return RedirectToAction("Listar");
            else
            {
                // Si hay un error, recargar las listas y devolver la vista
                ViewData["Clientes"] = _ContactoDatos.ListarClientes();
                ViewData["Sedes"] = _ContactoDatos.ListarSedes(oContacto.idClientes ?? 0);
                return View(oContacto);
            }
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
