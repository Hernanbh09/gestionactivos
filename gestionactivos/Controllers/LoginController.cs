using gestionactivos.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

public class LoginController : Controller
{
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ResultadoLogin(string Correo, string Clave)
    {
        // Validar si el correo o la clave son nulos o están vacíos
        if (string.IsNullOrEmpty(Correo) || string.IsNullOrEmpty(Clave))
        {
            TempData["Message"] = "Correo y contraseña son obligatorios.";
            TempData["MessageType"] = "error";
            return View("Login"); // Regresar a la vista de login
        }

        LoginData Data = new LoginData();

        // Consultar el usuario en la base de datos
        LoginModel Login = Data.ConsultarUsuario(Correo, Clave);

        if (Login != null)
        {
            // Si el usuario existe, configurar el ticket de autenticación
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, Login.NombreCompleto),
                new Claim(ClaimTypes.Role, Login.Rol),
                new Claim(ClaimTypes.NameIdentifier, Login.IdUsuario.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Autenticar al usuario usando cookies
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            // Guardar mensaje de éxito en TempData
            TempData["Message"] = "Inicio de sesión exitoso. Bienvenido, " + Login.NombreCompleto;
            TempData["MessageType"] = "success";
            TempData.Clear();

            // Redirigir a la página principal u otra página
            return RedirectToAction("Index", "Home");
        }
        else
        {
            // Si la autenticación falla, mostrar mensaje de error
            TempData["Message"] = "Correo o contraseña incorrectos.";
            TempData["MessageType"] = "error";

            // Volver a la vista de login
            return View("Login");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        // Cerrar sesión
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        // Limpiar TempData y redirigir a la página de login
        TempData.Clear();
        return RedirectToAction("Login", "Login");
    }
}
