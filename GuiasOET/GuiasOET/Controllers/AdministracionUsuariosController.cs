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
            //CargarEstacionesDropDownList();
            return View();
        }

        private void CargarEstacionesDropDownList(object estacionSeleccionada = null)
        {
            var EstacionesQuery = from d in baseDatos.GUIAS_ESTACION
                                   orderby d.NOMBREESTACION
                                   select d;
            ViewBag.NOMBREESTACION =  new SelectList(EstacionesQuery, "NOMBREESTACION", "NOMBREESTACION", estacionSeleccionada);
        }

        // GET: /Seguridad/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        // POST: /AdministracionUsuarios/Login
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

        // GET: ListaUsuarios
        public ActionResult ListaUsuarios()
        {
            return View(baseDatos.GUIAS_EMPLEADO.ToList());
        }

        // GET: ListaUsuarios
        public ActionResult InsertarUsuario()
        {
            //CargarEstacionesDropDownList();
            return View();
        }

        // POST: /AdministracionUsuarios/Insertar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InsertarUsuario(ManejoModelos nuevoUsuario)
        {
                nuevoUsuario.modeloTelefono.CEDULAEMPLEADO = nuevoUsuario.modeloEmpleado.CEDULA;
                nuevoUsuario.modeloEmpleado.ESTADO = 1;
                nuevoUsuario.modeloEmpleado.TIPOEMPLEADO = "Administrador Global";
                nuevoUsuario.modeloEmpleado.NOMBREESTACION = "No existe";
                nuevoUsuario.modeloEmpleado.CONFIRMAREMAIL = 0;

                if (ModelState.IsValid)
                {
                    ViewBag.Message = "Nuevo usuario creado con éxito.";
                    baseDatos.GUIAS_EMPLEADO.Add(nuevoUsuario.modeloEmpleado);
                    baseDatos.SaveChanges();
                    return RedirectToAction("AdministracionUsuarios");
                }
                else
                {
                    
                    ModelState.AddModelError("", "El usuario insertado no es válido");
                }

            //CargarEstacionesDropDownList();
            return View(nuevoUsuario);
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