using Microsoft.AspNetCore.Mvc;
using gestionactivos.Data;
using gestionactivos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;


namespace gestionactivos.Controllers
{
    [Authorize(Policy = "GlobalPolicy")]
    [Authorize(Roles = "Administrador,Coordinador")]
    public class FuncionariosController : Controller
    {
        FuncionariosData _ContactoDatos = new FuncionariosData();
        private int? idUsuario;

        // Este método se ejecuta antes de cada acción del controlador
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var idUsuarioClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            idUsuario = idUsuarioClaim != null ? (int?)Convert.ToInt32(idUsuarioClaim.Value) : null;
            base.OnActionExecuting(context);
        }

        public IActionResult Listar()
        {
            //La visata de mostar contactos

            var oLista = _ContactoDatos.Listar(idUsuario);

            return View(oLista);
        }

        // Método GET para cargar la vista de guardado
        public IActionResult Guardar()
        {
            // Cargar los clientes para el dropdown
            var clientes = _ContactoDatos.ListarClientes(idUsuario);
            ViewData["Clientes"] = clientes;

            // Retornar la vista vacía
            return View();
        }

        // Método POST para procesar el guardado
        [HttpPost]
        public IActionResult Guardar(FuncionarioModel oContacto, int idCliente, int idSedes)
        {
            // Validar el modelo y las selecciones de cliente y sede
            if (!ModelState.IsValid || idCliente == 0 || idSedes == 0)
            {
                // Agregar un mensaje de error si no se seleccionó cliente o sede
                if (idCliente == 0 || idSedes == 0)
                {
                    ModelState.AddModelError("", "Debes seleccionar la sede y el cliente.");
                }

                // Cargar los clientes y sedes nuevamente
                ViewData["Clientes"] = _ContactoDatos.ListarClientes(idUsuario);
                ViewData["Sedes"] = _ContactoDatos.ListarSedes(idCliente, idUsuario); // Cargar sedes basadas en cliente

                return View(oContacto); // Devuelve la vista con los errores y datos
            }

            // Asignar los valores seleccionados
            oContacto.idClientes = idCliente;
            oContacto.idSedes = idSedes;

            // Guardar el contacto
            var data = new FuncionariosData();
            var resultado = data.Guardar(oContacto, idSedes, idCliente, idUsuario);

            // Si el guardado es exitoso
            if (resultado.Resultado)
            {
                return RedirectToAction("Listar");
            }
            else
            {
                // Cargar los clientes y sedes nuevamente en caso de fallo
                ViewData["Clientes"] = _ContactoDatos.ListarClientes(idUsuario);
                ViewData["Sedes"] = _ContactoDatos.ListarSedes(idCliente, idUsuario);

                // Agregar el mensaje de error al ModelState
                ModelState.AddModelError("", resultado.Mensaje);

                // Retornar la vista con el error
                return View(oContacto);
            }
        }


        [HttpPost]
        public JsonResult ObtenerSedes(int idCliente)
        {
            var sedes = _ContactoDatos.ListarSedes(idCliente, idUsuario);
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
            var clientes = _ContactoDatos.ListarClientes(idUsuario);
            ViewData["Clientes"] = clientes;

            // Comprobar si idClientes es válido antes de obtener las sedes
            var sedes = oContacto.idClientes.HasValue ? _ContactoDatos.ListarSedes(oContacto.idClientes.Value, idUsuario) : new List<FuncionarioModel>();
            ViewData["Sedes"] = sedes;

            return View(oContacto);
        }


        [HttpPost]
        public IActionResult Editar(FuncionarioModel oContacto)
        {
            if (!ModelState.IsValid)
            {
                // Si la validación falla, recargar las listas para la vista
                ViewData["Clientes"] = _ContactoDatos.ListarClientes(idUsuario);
                ViewData["Sedes"] = _ContactoDatos.ListarSedes(oContacto.idClientes ?? 0, idUsuario);
                return View(oContacto);
            }

            // Llamar al método de edición en la capa de datos
            var respuesta = _ContactoDatos.Editar(oContacto, idUsuario);
            if (respuesta)
                return RedirectToAction("Listar");
            else
            {
                // Si hay un error, recargar las listas y devolver la vista
                ViewData["Clientes"] = _ContactoDatos.ListarClientes(idUsuario);
                ViewData["Sedes"] = _ContactoDatos.ListarSedes(oContacto.idClientes ?? 0, idUsuario);
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
