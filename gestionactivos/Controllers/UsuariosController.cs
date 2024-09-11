﻿using Microsoft.AspNetCore.Mvc;
using gestionactivos.Data;
using gestionactivos.Models;


namespace gestionactivos.Controllers
{
    public class UsuariosController : Controller
    {
        UsuariosData _UsuariosDatos = new UsuariosData();

        public IActionResult Listar()
        {
            var oLista = _UsuariosDatos.Listar();

            return View(oLista);
        }

        public IActionResult Guardar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Guardar(UsuarioModel oUsuario)
        {
            if (!ModelState.IsValid)
                return View();

            var respuesta = _UsuariosDatos.Guardar(oUsuario);
            if (respuesta)
                return RedirectToAction("Listar");
            else

                return View();
        }

        public IActionResult Editar(int idUsuario)
        {
            var osuario = _UsuariosDatos.Obtener(idUsuario);
            return View(osuario);
        }

        [HttpPost]
        public IActionResult Editar(UsuarioModel oUsuario)
        {
            if (!ModelState.IsValid)
                return View();



            //Recibir un objeto y guardarlo en la base de datos
            var respuesta = _UsuariosDatos.Editar(oUsuario);
            if (respuesta)
                return RedirectToAction("Listar");
            else

                return View();

        }

        public IActionResult Eliminar(int idUsuario)
        {
            var oUsuario = _UsuariosDatos.Obtener(idUsuario);
            return View(oUsuario);
        }
        [HttpPost]

        public IActionResult Eliminar(UsuarioModel oUsuario)
        {
            var respuesta = _UsuariosDatos.Eliminar(oUsuario.idUsuario);
            if (respuesta)
                return RedirectToAction("Listar");
            else
                return View();
        }
    }
}