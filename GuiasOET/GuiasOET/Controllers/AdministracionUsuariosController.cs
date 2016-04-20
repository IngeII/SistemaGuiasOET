using System;
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
using PagedList;

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
        public ActionResult ListaUsuarios(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NombreSortParm = String.IsNullOrEmpty(sortOrder) ? "Nombre" : "";
            ViewBag.Ape1SortParm = String.IsNullOrEmpty(sortOrder) ? "Apellido1" : "";
            ViewBag.Ape2SortParm = String.IsNullOrEmpty(sortOrder) ? "Apellido2" : "";
            ViewBag.EstacionSortParm = String.IsNullOrEmpty(sortOrder) ? "Estacion" : "";
            ViewBag.RolSortParm = String.IsNullOrEmpty(sortOrder) ? "Rol" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var empleados = from e in baseDatos.GUIAS_EMPLEADO select e;

            if (!String.IsNullOrEmpty(searchString))
            {
                empleados = empleados.Where(e => e.APELLIDO1.Contains(searchString)
                                       || e.NOMBREEMPLEADO.Contains(searchString) || e.APELLIDO2.Contains(searchString)
                                       || e.NOMBREESTACION.Contains(searchString) || e.TIPOEMPLEADO.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Nombre":
                    empleados = empleados.OrderBy(e => e.NOMBREEMPLEADO);
                    break;
                case "Apellido1":
                    empleados = empleados.OrderBy(e => e.APELLIDO1);
                    break;
                case "Apellido2":
                    empleados = empleados.OrderBy(e => e.APELLIDO2);
                    break;
                case "Estacion":
                    empleados = empleados.OrderBy(e => e.NOMBREESTACION);
                    break;
                case "Rol":
                    empleados = empleados.OrderBy(e => e.TIPOEMPLEADO);
                    break;
                default:
                    empleados = empleados.OrderBy(e => e.NOMBREESTACION);
                    break;
            }

            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(empleados.ToPagedList(pageNumber, pageSize));
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
            /*Se obtiene el tipo de usuario con los radiobutton*/
            nuevoUsuario.modeloEmpleado.TIPOEMPLEADO = tipoUsuario;

            /*Si el tipo de empleado es cualquier tipo de guía hay que verificar que haya dado los datos completos*/
            if (nuevoUsuario.modeloEmpleado.TIPOEMPLEADO.Contains("Guía"))
            {
                /*Si falta algún dato personal del guía no se debe permitir la inserción*/
                if (nuevoUsuario.modeloEmpleado.NOMBREEMPLEADO == null || nuevoUsuario.modeloEmpleado.APELLIDO1 == null || nuevoUsuario.modeloEmpleado.APELLIDO2 == null || nuevoUsuario.modeloEmpleado.DIRECCION == null)
                {
                    ModelState.AddModelError("", "Para agregar un guía los datos correspondientes a nombre, apellidos y dirección son requeridos.");
                }
                else
                {
                    /*Si es un guía pero no se le asoció ninguna estación no debe dejar guardarlo*/
                    if (nuevoUsuario.modeloEmpleado.NOMBREESTACION.Contains("Ninguna"))
                    {
                        ModelState.AddModelError("", "El guía debe tener asociado una estación en el sistema.");
                    }
                    /*Si si se asoció una estación válida al guía se guarda en la base de datos*/
                    else
                    {

                        /*Validación de los telefonos falta*/
                        
                        nuevoUsuario.modeloEmpleado.CONFIRMAREMAIL = 0;

                        /*Si el modelo es válido se guarda en la base como una tupla*/
                        if (ModelState.IsValid)
                        {
                            GUIAS_EMPLEADO usuarios = baseDatos.GUIAS_EMPLEADO.FirstOrDefault(i => i.USUARIO == nuevoUsuario.modeloEmpleado.USUARIO);
                            GUIAS_EMPLEADO cedula = baseDatos.GUIAS_EMPLEADO.FirstOrDefault(i => i.CEDULA == nuevoUsuario.modeloEmpleado.CEDULA);
                            if (usuarios == null && cedula == null)
                            {
                                baseDatos.GUIAS_EMPLEADO.Add(nuevoUsuario.modeloEmpleado);
                                baseDatos.SaveChanges();
                                insertarTelefonos(nuevoUsuario);
                                CargarEstacionesDropDownList();
                                return RedirectToAction("InsertarUsuario");
                            }
                            else if(usuarios != null)
                            {
                                ModelState.AddModelError("", "Ya existe existe nombre de usuario en el sistema.");
                            }
                            else if (cedula != null)
                            {
                                ModelState.AddModelError("", "Ya existe este número de cédula en el sistema.");
                            }
                        }
                        /*Si el modelo es inválido no lo agrega a la base*/
                        else
                        {
                            /*Muestra un mensaje de error al usuario*/
                            if (nuevoUsuario.modeloEmpleado.CONTRASENA == null)
                            {
                                ModelState.AddModelError("", "Debe ingresar una contraseña para este usuario.");
                            }
                            else {
                                if (nuevoUsuario.modeloEmpleado.CONTRASENA.Contains(nuevoUsuario.modeloEmpleado.CONFIRMACIONCONTRASENA))
                                {
                                    ModelState.AddModelError("", "Ya existe otro usuario con esta cédula o nombre de usuario en el sistema.");
                                }
                            }
                            CargarEstacionesDropDownList();
                            ViewBag.tipoUsuario = nuevoUsuario.modeloEmpleado.TIPOEMPLEADO;
                            return View(nuevoUsuario);
                        }
                    } 
                }
            }

            /*Si el rol es cualquier tipo de administrador*/
            else
            {
                /*validar telefonos*/
                nuevoUsuario.modeloEmpleado.CONFIRMAREMAIL = 0;

                /*Si no se escogió ninguna estación y el tipo de empleado es Administrador Local no debe dejar guardar en la base*/
                if (nuevoUsuario.modeloEmpleado.NOMBREESTACION.Contains("Ninguna") && nuevoUsuario.modeloEmpleado.TIPOEMPLEADO.Contains("Local"))
                {
                    ModelState.AddModelError("", "El administrador local debe tener asociado una estación en el sistema.");

                }
                /*Si el usuario es administrador global o escogió una estación entra a este else*/
                else
                {
                    /*Se realiza una ultima verificación que si el tipo de usuario es administrador Global no sea asociado a ninguna estación*/
                    if (nuevoUsuario.modeloEmpleado.TIPOEMPLEADO.Contains("Global"))
                    {
                        nuevoUsuario.modeloEmpleado.NOMBREESTACION = "Ninguna";
                    }

                    /*Si el modelo es válido se guarda en la base como una tupla*/
                    if (ModelState.IsValid)
                    {
                        GUIAS_EMPLEADO usuarios = baseDatos.GUIAS_EMPLEADO.FirstOrDefault(i => i.USUARIO == nuevoUsuario.modeloEmpleado.USUARIO);
                        GUIAS_EMPLEADO cedula = baseDatos.GUIAS_EMPLEADO.FirstOrDefault(i => i.CEDULA == nuevoUsuario.modeloEmpleado.CEDULA);
                        if (usuarios == null && cedula == null)
                        {
                            baseDatos.GUIAS_EMPLEADO.Add(nuevoUsuario.modeloEmpleado);
                            baseDatos.SaveChanges();
                            insertarTelefonos(nuevoUsuario);
                            CargarEstacionesDropDownList();
                            return RedirectToAction("InsertarUsuario");
                        }
                        else if (usuarios != null)
                        {
                            ModelState.AddModelError("", "Ya existe existe nombre de usuario en el sistema.");
                        }
                        else if (cedula != null)
                        {
                            ModelState.AddModelError("", "Ya existe este número de cédula en el sistema.");
                        }
                    }

                    /*Si el modelo no es válido no se guarda en la base de datos*/
                    else
                    {
                        /*Muestra un mensaje de error al usuario*/
                        if (nuevoUsuario.modeloEmpleado.CONTRASENA == null)
                        {
                            ModelState.AddModelError("", "Debe ingresar una contraseña para este usuario.");
                        }
                        else
                        {
                            if (nuevoUsuario.modeloEmpleado.CONTRASENA.Contains(nuevoUsuario.modeloEmpleado.CONFIRMACIONCONTRASENA))
                            {
                                ModelState.AddModelError("", "Ya existe otro usuario con esta cédula o nombre de usuario en el sistema.");
                            }
                        }
                        CargarEstacionesDropDownList();
                        return View(nuevoUsuario);
                    }
                } 
            }

            CargarEstacionesDropDownList();
            return View();    
        }

        public void insertarTelefonos(ManejoModelos telefonos)
        {
            if (telefonos.modeloTelefono.TELEFONO != null)
            {
                telefonos.modeloTelefono.CEDULAEMPLEADO = telefonos.modeloEmpleado.CEDULA;
                baseDatos.GUIAS_TELEFONO.Add(telefonos.modeloTelefono);
                baseDatos.SaveChanges();
            }
            if (telefonos.modeloTelefono2.TELEFONO != null)
            {
                telefonos.modeloTelefono2.CEDULAEMPLEADO = telefonos.modeloEmpleado.CEDULA;
                baseDatos.GUIAS_TELEFONO.Add(telefonos.modeloTelefono2);
                baseDatos.SaveChanges();
            }
            if (telefonos.modeloTelefono3.TELEFONO != null)
            {
                telefonos.modeloTelefono3.CEDULAEMPLEADO = telefonos.modeloEmpleado.CEDULA;
                baseDatos.GUIAS_TELEFONO.Add(telefonos.modeloTelefono3);
                baseDatos.SaveChanges();
            }
            if (telefonos.modeloTelefono4.TELEFONO != null)
            {
                telefonos.modeloTelefono4.CEDULAEMPLEADO = telefonos.modeloEmpleado.CEDULA;
                baseDatos.GUIAS_TELEFONO.Add(telefonos.modeloTelefono4);
                baseDatos.SaveChanges();
            }
        }

        // GET: Modificar usuario
        public ActionResult ConsultarUsuario(int? id)
        {
            CargarEstacionesDropDownList();
            string identificacion;
            ManejoModelos modelo;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            identificacion = id.ToString();

            string consulta = "SELECT * FROM GUIAS_TELEFONO WHERE CedulaEmpleado ='" + identificacion + "'";

            IEnumerable<GUIAS_TELEFONO> telefonos = baseDatos.Database.SqlQuery<GUIAS_TELEFONO>(consulta);

            modelo = new ManejoModelos(baseDatos.GUIAS_EMPLEADO.Find(identificacion), telefonos);

            // modelo.modeloEmpleado.ESTADO = baseDatos.GUIAS_EMPLEADO.Find(identificacion).ESTADO;
            if (modelo == null)
            {
                return HttpNotFound();
            }
            // Genera una variable de tipo lista con opciones para un ListBox.
            bool activo = modelo.modeloEmpleado.ESTADO == 1;
            bool inactivo = modelo.modeloEmpleado.ESTADO == 0;
            ViewBag.opciones = new List<System.Web.Mvc.SelectListItem> {
                new System.Web.Mvc.SelectListItem { Text = "Activo", Value = "1", Selected = activo},
                new System.Web.Mvc.SelectListItem { Text = "Inactivo", Value = "0", Selected = inactivo }
            };

            return View(modelo);
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

            string consulta = "SELECT * FROM GUIAS_TELEFONO WHERE CedulaEmpleado ='" + identificacion + "'";
            
            IEnumerable<GUIAS_TELEFONO> telefonos = baseDatos.Database.SqlQuery<GUIAS_TELEFONO>(consulta);

            modelo = new ManejoModelos (baseDatos.GUIAS_EMPLEADO.Find(identificacion),telefonos);

            // modelo.modeloEmpleado.ESTADO = baseDatos.GUIAS_EMPLEADO.Find(identificacion).ESTADO;
            modelo.modeloEmpleado.CONFIRMACIONCONTRASENA = modelo.modeloEmpleado.CONTRASENA;
            if (modelo == null)
            {
                return HttpNotFound();
            }
            // Genera una variable de tipo lista con opciones para un ListBox.
            bool activo = modelo.modeloEmpleado.ESTADO == 1;
            bool inactivo = modelo.modeloEmpleado.ESTADO == 0;
            ViewBag.opciones = new List<System.Web.Mvc.SelectListItem> {
                new System.Web.Mvc.SelectListItem { Text = "Activo", Value = "1", Selected = activo},
                new System.Web.Mvc.SelectListItem { Text = "Inactivo", Value = "0", Selected = inactivo }
            };

            return View(modelo);
        }

        [HttpPost, ActionName("ModificarUsuario")]
        [ValidateAntiForgeryToken]
        public ActionResult ModificarPost(int? id, string estado)
        {
            ManejoModelos modelo;
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string identificacion = id.ToString();

            string consulta = "SELECT * FROM GUIAS_TELEFONO WHERE CedulaEmpleado ='" + identificacion + "'";

            IEnumerable<GUIAS_TELEFONO> telefonos = baseDatos.Database.SqlQuery<GUIAS_TELEFONO>(consulta);

            modelo = new ManejoModelos(baseDatos.GUIAS_EMPLEADO.Find(identificacion), telefonos);

            string a = modelo.modeloTelefono.CEDULAEMPLEADO;

            var employeeToUpdate = modelo;

            if (employeeToUpdate.modeloEmpleado.TIPOEMPLEADO.Contains("Guía"))
            {
                /*Si falta algún dato personal del guía no se debe permitir la inserción*/
                if (employeeToUpdate.modeloEmpleado.NOMBREEMPLEADO == null || employeeToUpdate.modeloEmpleado.APELLIDO1 == null || employeeToUpdate.modeloEmpleado.APELLIDO2 == null || employeeToUpdate.modeloEmpleado.DIRECCION == null)
                {
                    ModelState.AddModelError("", "Para modificar un guía los datos correspondientes a nombre, apellidos y dirección son requeridos.");
                }
                else
                {
                    /*Si es un guía pero no se le asoció ninguna estación no debe dejar guardarlo*/
                    if (employeeToUpdate.modeloEmpleado.NOMBREESTACION.Contains("Ninguna"))
                    {
                        ModelState.AddModelError("", "El guía debe tener asociado una estación en el sistema.");
                    }
                    /*Si si se asoció una estación válida al guía se guarda en la base de datos*/
                    else
                    {

                        /*Validación de los telefonos falta*/

                        employeeToUpdate.modeloEmpleado.CONFIRMAREMAIL = 0;

                        /*Si el modelo es válido se guarda en la base como una tupla*/
                        if (TryUpdateModel(employeeToUpdate))
                        {
                            try
                            {
                                ViewBag.Message = "Usuario modificado con éxito.";
                                employeeToUpdate.modeloEmpleado.ESTADO = Int32.Parse(estado);
                                string b = employeeToUpdate.modeloTelefono.CEDULAEMPLEADO;
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

                            /*Muestra un mensaje de error al usuario*/
                            if (employeeToUpdate.modeloEmpleado.CONTRASENA == null)
                            {
                                ModelState.AddModelError("", "Debe ingresar una contraseña para este usuario.");
                            }
                            else
                            {
                                if (employeeToUpdate.modeloEmpleado.CONTRASENA.Contains(employeeToUpdate.modeloEmpleado.CONFIRMACIONCONTRASENA))
                                {
                                    ModelState.AddModelError("", "Ya existe otro usuario con esta cédula o nombre de usuario en el sistema.");
                                }
                            }

                        }
                    }
                }
            }

            /*Si el rol es cualquier tipo de administrador*/
            else
            {
                /*validar telefonos*/
                employeeToUpdate.modeloEmpleado.CONFIRMAREMAIL = 0;

                /*Si no se escogió ninguna estación y el tipo de empleado es Administrador Local no debe dejar guardar en la base*/
                if (employeeToUpdate.modeloEmpleado.NOMBREESTACION.Contains("Ninguna") && employeeToUpdate.modeloEmpleado.TIPOEMPLEADO.Contains("Local"))
                {
                    ModelState.AddModelError("", "El administrador local debe tener asociado una estación en el sistema.");

                }
                /*Si el usuario es administrador global o escogió una estación entra a este else*/
                else
                {
                    /*Se realiza una ultima verificación que si el tipo de usuario es administrador Global no sea asociado a ninguna estación*/
                    if (employeeToUpdate.modeloEmpleado.TIPOEMPLEADO.Contains("Global"))
                    {
                        employeeToUpdate.modeloEmpleado.NOMBREESTACION = "Ninguna";
                    }

                    /*Si el modelo es válido se guarda en la base como una tupla*/
                    if (TryUpdateModel(employeeToUpdate))
                    {
                        try
                        {
                            ViewBag.Message = "Usuario modificado con éxito.";
                            employeeToUpdate.modeloEmpleado.ESTADO = Int32.Parse(estado);
                            baseDatos.SaveChanges();
                           
                        }
                        catch (RetryLimitExceededException /* dex */)
                        {
                            //Log the error (uncomment dex variable name and add a line here to write a log.
                            ModelState.AddModelError("", "No es posible modificar en este momento, intente más tarde");
                        }
                    }
                    /*Si el modelo no es válido no se guarda en la base de datos*/
                    else
                    {
                        /*Muestra un mensaje de error al usuario*/
                        if (employeeToUpdate.modeloEmpleado.CONTRASENA == null)
                        {
                            ModelState.AddModelError("", "Debe ingresar una contraseña para este usuario.");
                        }
                        else
                        {
                            if (employeeToUpdate.modeloEmpleado.CONTRASENA.Contains(employeeToUpdate.modeloEmpleado.CONFIRMACIONCONTRASENA))
                            {
                                ModelState.AddModelError("", "Ya existe otro usuario con esta cédula o nombre de usuario en el sistema.");
                            }
                        }

                    }
                }

            }

            
            bool activo = modelo.modeloEmpleado.ESTADO == 1;
            bool inactivo = modelo.modeloEmpleado.ESTADO == 0;
            ViewBag.opciones = new List<SelectListItem> {
                new SelectListItem { Text = "Activo", Value = "1", Selected = activo},
                new SelectListItem { Text = "Inactivo", Value = "0", Selected = inactivo }
            };
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
		
		// GET: Eliminar usuario
        public ActionResult EliminarUsuario(int? id)
        {

            string identificacion;
            ManejoModelos modelo;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            identificacion = id.ToString();

            modelo = new ManejoModelos(baseDatos.GUIAS_EMPLEADO.Find(identificacion));

            // modelo.modeloEmpleado.ESTADO = baseDatos.GUIAS_EMPLEADO.Find(identificacion).ESTADO;
            modelo.modeloEmpleado.CONFIRMACIONCONTRASENA = modelo.modeloEmpleado.CONTRASENA;
            if (modelo == null)
            {
                return HttpNotFound();
            }
            // Genera una variable de tipo lista con opciones para un ListBox.
            bool activo = modelo.modeloEmpleado.ESTADO == 1;
            bool inactivo = modelo.modeloEmpleado.ESTADO == 0;
            ViewBag.opciones = new List<SelectListItem> {
                new SelectListItem { Text = "Activo", Value = "1", Selected = activo},
                new SelectListItem { Text = "Inactivo", Value = "0", Selected = inactivo }
            };
            return View(modelo);
        }

        [HttpPost, ActionName("EliminarUsuario")]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarUsuarioPost(int? id)
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
                }
                else
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



    }
}