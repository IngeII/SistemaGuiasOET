using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GuiasOET.Models;
using System.Diagnostics;

namespace GuiasOET.Controllers
{
    public class AdministracionUsuariosController : Controller
    {
        private Entities baseDatos = new Entities();

        // GET: AdministracioUsuarios
        public ActionResult AdministracionUsuarios()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        // GET: /Seguridad/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        // POST: /AdministracionRecursos/Login
        [HttpPost, ActionName("Login")]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string usuario, string contrasena)
        {

            //Se retorna la primera aparicion del empleado que tiene como usuario el especificado por el usuario,
            //como el usuario es un campo único no habrán problemas. 
            GUIAS_EMPLEADO Guia = baseDatos.GUIAS_EMPLEADO.FirstOrDefault(i => i.USUARIO == usuario);

            /*     GUIAS_EMPLEADO Guia  = (GUIAS_EMPLEADO) from empleado in baseDatos.GUIAS_EMPLEADO
                                       where empleado.USUARIO.Equals(usuario)
                                       select empleado; */

            if (Guia != null)
            {
                //Verifica contra la contraseña encriptada
                if (Guia.CONTRASENA == contrasena)
                {
                    ModelState.Clear();
                    return RedirectToAction("AdministracionUsuarios");
                }
                else
                {
                    ModelState.AddModelError("", "La contraseña ingresada es inválida");
                    return View();
                }
            }

            else
            {

                ModelState.Clear();
                ModelState.AddModelError("", "El usuario no se encuentra en el sistema");
                return View();
            }


        }


        //
        // GET: /Seguridad/ReestablecerContraseña
        [AllowAnonymous]
        public ActionResult ReestablecerContraseña()
        {
            return View();
        }



    }
}