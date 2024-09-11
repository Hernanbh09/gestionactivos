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
        LoginData Data = new LoginData();
        LoginModel Login = Data.ConsultarUsuario(Correo, Clave);

        if (Login != null)
        {
            // Configurar el ticket de autenticación
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, Login.NombreCompleto),
            new Claim(ClaimTypes.Role, Login.Rol),
            new Claim(ClaimTypes.NameIdentifier, Login.IdUsuario.ToString())
        };
            

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));



        // Guardar el mensaje de éxito en TempData después de la autenticación exitosa
        TempData["Message"] = "Inicio de sesión exitoso. Bienvenido, " + Login.NombreCompleto;
        TempData["MessageType"] = "success";
            TempData.Clear();
            // Redirigir a la vista principal o a otra página
            return RedirectToAction("Index", "Home");
        }
        else
        {
            // Guardar el mensaje de error en TempData si la autenticación falla
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


        TempData.Clear();

        // Redirigir a la página de inicio de sesión
        return RedirectToAction("Login", "Login");
    }

}