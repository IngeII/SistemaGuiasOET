﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GuiasOET.Models;
using System.Diagnostics;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Web.UI.HtmlControls;
using System.Data.Entity;

namespace GuiasOET.Controllers
{
    public class AdministracionUsuariosController : Controller
    {

        private Entities baseDatos = new Entities();

        /*Método para cargar el combobox de estación*/
        private void CargarEstacionesDropDownList(object estacionSeleccionada = null)
        {
            var EstacionesQuery = from d in baseDatos.GUIAS_ESTACION
                                   orderby d.NOMBREESTACION
                                   select d;
            ViewBag.NOMBREESTACION = new SelectList(EstacionesQuery, "NOMBREESTACION", "NOMBREESTACION", estacionSeleccionada);
        }

        // GET: /Seguridad/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        // POST: /AdministracionRecursos/Login
        [HttpPost]
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
                    Session["IdUsuarioLogueado"] = Guia.CEDULA.ToString();
                    Session["NombreUsuarioLogueado"] = Guia.NOMBREEMPLEADO.ToString() + " " + Guia.APELLIDO1.ToString() + " " + Guia.APELLIDO2.ToString();
                    Session["RolUsuarioLogueado"] = Guia.TIPOEMPLEADO.ToString();
                    // FormsAuthentication.SetAuthCookie(Guia.USUARIO, false);
                    return RedirectToAction("ListaUsuarios");
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
        /*Método GET de la pantalla ListaUsuarios*/
        public ActionResult ListaUsuarios()
        {

            /*    if (Session["NombreUsuarioLogueado"] != null)
                {
                    return View(baseDatos.GUIAS_EMPLEADO.ToList());
                }
                else
                {
                    return RedirectToAction("Login");
                }  */

            return View(baseDatos.GUIAS_EMPLEADO.ToList());

        }

       /*Método get de la pantalla InsertarUsuario*/
        public ActionResult InsertarUsuario()
        {
            CargarEstacionesDropDownList();
            return View();
        }

        // POST: /AdministracionUsuarios/Insertar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InsertarUsuario(ManejoModelos nuevoUsuario, string tipoUsuario)
        {
            nuevoUsuario.modeloEmpleado.TIPOEMPLEADO = tipoUsuario;

            if (nuevoUsuario.modeloEmpleado.TIPOEMPLEADO.Contains("Guía"))
            {
                if (nuevoUsuario.modeloEmpleado.NOMBREEMPLEADO == null || nuevoUsuario.modeloEmpleado.APELLIDO1 == null || nuevoUsuario.modeloEmpleado.APELLIDO2 == null || nuevoUsuario.modeloEmpleado.DIRECCION == null)
                {
                    ModelState.AddModelError("", "Para agregar un guía los datos correspondientes a nombre, apellidos y dirección son obligatorios.");
                }
                else
                {

                    /*validar estación*/
                    nuevoUsuario.modeloTelefono.CEDULAEMPLEADO = nuevoUsuario.modeloEmpleado.CEDULA;
                    nuevoUsuario.modeloEmpleado.NOMBREESTACION = "No existe";
                    nuevoUsuario.modeloEmpleado.CONFIRMAREMAIL = 0;

                    if (ModelState.IsValid)
                    {
                        ViewBag.Message = "Nuevo usuario creado con éxito.";
                        baseDatos.GUIAS_EMPLEADO.Add(nuevoUsuario.modeloEmpleado);
                        baseDatos.SaveChanges();
                        CargarEstacionesDropDownList();
                        return RedirectToAction("InsertarUsuarios");
                    }
                    else
                    {
                        
                        ModelState.AddModelError("", "Ya existe otro usuario con esta cédula o nombre de usuario en el sistema");
                        CargarEstacionesDropDownList();
                        return View(nuevoUsuario);
                    }
                }
            }

            /*Si el rol no es ni guía externo ni interno*/
            else
            {
                /*validar estación*/
                nuevoUsuario.modeloTelefono.CEDULAEMPLEADO = nuevoUsuario.modeloEmpleado.CEDULA;
                nuevoUsuario.modeloEmpleado.NOMBREESTACION = "No existe";
                nuevoUsuario.modeloEmpleado.CONFIRMAREMAIL = 0;

                if (ModelState.IsValid)
                {
                    ViewBag.Message = "Nuevo usuario creado con éxito.";
                    baseDatos.GUIAS_EMPLEADO.Add(nuevoUsuario.modeloEmpleado);
                    baseDatos.SaveChanges();
                    CargarEstacionesDropDownList();
                    return RedirectToAction("InsertarUsuario");
                }
                else
                {
                    ModelState.AddModelError("", "Ya existe otro usuario con esta cédula o nombre de usuario en el sistema");
                    CargarEstacionesDropDownList();
                    return View(nuevoUsuario);
                }
            }

            CargarEstacionesDropDownList();
            return View();    
        }

        // GET: Modificar usuario
        public ActionResult ModificarUsuario(int? id)
        {

            string identificacion;
            ManejoModelos modelo;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            identificacion = id.ToString();

            modelo = new ManejoModelos (baseDatos.GUIAS_EMPLEADO.Find(identificacion));

           // modelo.modeloEmpleado.ESTADO = baseDatos.GUIAS_EMPLEADO.Find(identificacion).ESTADO;
            modelo.modeloEmpleado.CONFIRMACIONCONTRASENA = modelo.modeloEmpleado.CONTRASENA;
            if (modelo == null)
            {
                return HttpNotFound();
            }
            return View(modelo);
        }

        [HttpPost, ActionName("ModificarUsuario")]
        [ValidateAntiForgeryToken]
        public ActionResult ModificarPost(int? id)
        {
            ManejoModelos modelo;
            String tipoEmpleado = "";
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var empleado = baseDatos.GUIAS_EMPLEADO.Find(id.ToString());

            tipoEmpleado = empleado.TIPOEMPLEADO.ToString();
            modelo = new ManejoModelos(empleado);
            var employeeToUpdate = modelo;
            if (tipoEmpleado.Equals("Guía"))
            {
                if (employeeToUpdate.modeloEmpleado.NOMBREEMPLEADO == null || employeeToUpdate.modeloEmpleado.APELLIDO1 == null || employeeToUpdate.modeloEmpleado.APELLIDO2 == null || employeeToUpdate.modeloEmpleado.DIRECCION == null)
                {
                    ModelState.AddModelError("", "Para modificar un guía los datos correspondientes a nombre, apellidos y dirección son obligatorios.");
                }

                if (TryUpdateModel(employeeToUpdate))
                {
                    try
                    {
                        ViewBag.Message = "Usuario modificado con éxito.";
                        baseDatos.SaveChanges();
                    }
                    catch (RetryLimitExceededException /* dex */)
                    {
                        //Log the error (uncomment dex variable name and add a line here to write a log.
                        ModelState.AddModelError("", "No es posible modificar en este momento, intente más tarde");
                    }
                }else
                {
                    ModelState.AddModelError("", "Verifique que los campos obligatorios, posean datos");

                }
            }
            else
            {
                if (TryUpdateModel(employeeToUpdate))
                {
                    try
                    {
                        ViewBag.Message = "Usuario modificado con éxito.";
                        baseDatos.SaveChanges();
                    }
                    catch (RetryLimitExceededException /* dex */)
                    {
                        //Log the error (uncomment dex variable name and add a line here to write a log.
                        ModelState.AddModelError("", "No es posible modificar en este momento, intente más tarde");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Verifique que los campos obligatorios, posean datos");

                }
            }
            return View(employeeToUpdate);
        }

        


        //
        // GET: /Seguridad/ReestablecerContraseña
        [AllowAnonymous]
        public ActionResult ReestablecerContraseña()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CerrarSesionLogin()
        {
            Session["IdUsuarioLogueado"] = null;
            Session["NombreUsuarioLogueado"] = null;
            Session["RolUsuarioLogueado"] = null;
            //  FormsAuthentication.SignOut();

            /*     Response.Cache.SetCacheability(HttpCacheability.NoCache);
                 Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
                 Response.Cache.SetNoStore(); */
            return RedirectToAction("Login");
        }



    }
}