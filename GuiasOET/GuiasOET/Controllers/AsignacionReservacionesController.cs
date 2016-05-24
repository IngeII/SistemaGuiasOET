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
using MvcFlashMessages;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Collections;

namespace GuiasOET.Controllers
{
    public class AsignacionReservacionesController : Controller
    {

        private Entities1 baseDatos = new Entities1();


        public ActionResult AsignarReservacion(string sortOrder, string currentFilter, string fechaDesde, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.ReservacionSortParm = String.IsNullOrEmpty(sortOrder) ? "Reservacion" : "";
            ViewBag.NombreSortParm = String.IsNullOrEmpty(sortOrder) ? "Nombre" : "";
            ViewBag.EstacionSortParm = String.IsNullOrEmpty(sortOrder) ? "Estacion" : "";
            ViewBag.PersonasSortParm = String.IsNullOrEmpty(sortOrder) ? "Personas" : "";
            ViewBag.FechaSortParm = String.IsNullOrEmpty(sortOrder) ? "Fecha" : "";
            ViewBag.HoraSortParm = String.IsNullOrEmpty(sortOrder) ? "Hora" : "";
            ViewBag.TurnoSortParm = String.IsNullOrEmpty(sortOrder) ? "Turno" : "";
            ViewBag.GuiasAsignadosSortParm = String.IsNullOrEmpty(sortOrder) ? "Guías Asignados" : "";

            if (fechaDesde != null)
            {
                page = 1;
            }
            else
            {
                fechaDesde = currentFilter;
            }

            ViewBag.CurrentFilter = fechaDesde;

            var empleados = from e in baseDatos.GUIAS_EMPLEADO select e;

            if (!String.IsNullOrEmpty(fechaDesde))
            {
                empleados = empleados.Where(e => e.APELLIDO1.Contains(fechaDesde)
                                       || e.NOMBREEMPLEADO.Contains(fechaDesde) || e.APELLIDO2.Contains(fechaDesde)
                                       || e.NOMBREESTACION.Contains(fechaDesde) || e.TIPOEMPLEADO.Contains(fechaDesde)
                                       || e.USUARIO.Contains(fechaDesde) || e.EMAIL.Contains(fechaDesde));
            }


            switch (sortOrder)
            {
                case "Reservacion":
                    empleados = empleados.OrderBy(e => e.NOMBREEMPLEADO);
                    break;
                case "Nombre":
                    empleados = empleados.OrderBy(e => e.APELLIDO1);
                    break;
                case "Estacion":
                    empleados = empleados.OrderBy(e => e.APELLIDO2);
                    break;
                case "Personas":
                    empleados = empleados.OrderBy(e => e.NOMBREESTACION);
                    break;
                case "Fecha":
                    empleados = empleados.OrderBy(e => e.TIPOEMPLEADO);
                    break;
                case "Hora":
                    empleados = empleados.OrderBy(e => e.APELLIDO2);
                    break;
                case "Turno":
                    empleados = empleados.OrderBy(e => e.NOMBREESTACION);
                    break;
                case "Guías Asignados":
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
        /**
         * 
         * */
        [HttpGet]
        public ActionResult ConsultarAsignacion()
        {
            var semana = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.fechaLunes = DateTime.Now.DayOfWeek.ToString();
            switch (DateTime.Now.DayOfWeek)
            {
               
                case System.DayOfWeek.Monday:
                    ViewBag.fechaLunes = DateTime.Now.ToString("dd/MM/yyyy");
                    ViewBag.fechaMartes = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
                    ViewBag.fechaMiercoles = DateTime.Now.AddDays(2).ToString("dd/MM/yyyy");
                    ViewBag.fechaJueves = DateTime.Now.AddDays(3).ToString("dd/MM/yyyy");
                    ViewBag.fechaViernes = DateTime.Now.AddDays(4).ToString("dd/MM/yyyy");
                    ViewBag.fechaSabado = DateTime.Now.AddDays(5).ToString("dd/MM/yyyy");
                    ViewBag.fechaDomingo = DateTime.Now.AddDays(6).ToString("dd/MM/yyyy");
                    break;

                case System.DayOfWeek.Tuesday:
                    ViewBag.fechaLunes = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)).ToString("dd/MM/yyyy");
                    ViewBag.fechaMartes = DateTime.Now.ToString("dd/MM/yyyy");
                    ViewBag.fechaMiercoles = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
                    ViewBag.fechaJueves = DateTime.Now.AddDays(2).ToString("dd/MM/yyyy");
                    ViewBag.fechaViernes = DateTime.Now.AddDays(3).ToString("dd/MM/yyyy");
                    ViewBag.fechaSabado = DateTime.Now.AddDays(4).ToString("dd/MM/yyyy");
                    ViewBag.fechaDomingo = DateTime.Now.AddDays(5).ToString("dd/MM/yyyy");
                    break;

                case System.DayOfWeek.Wednesday:
                    ViewBag.fechaLunes = DateTime.Now.Subtract(new TimeSpan(2, 0, 0, 0)).ToString("dd/MM/yyyy");
                    ViewBag.fechaMartes = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)).ToString("dd/MM/yyyy");
                    ViewBag.fechaMiercoles = DateTime.Now.ToString("dd/MM/yyyy");
                    ViewBag.fechaJueves = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
                    ViewBag.fechaViernes = DateTime.Now.AddDays(2).ToString("dd/MM/yyyy");
                    ViewBag.fechaSabado = DateTime.Now.AddDays(3).ToString("dd/MM/yyyy");
                    ViewBag.fechaDomingo = DateTime.Now.AddDays(4).ToString("dd/MM/yyyy");
                    break;

                case System.DayOfWeek.Thursday:
                    ViewBag.fechaLunes = DateTime.Now.Subtract(new TimeSpan(3, 0, 0, 0)).ToString("dd/MM/yyyy");
                    ViewBag.fechaMartes = DateTime.Now.Subtract(new TimeSpan(2, 0, 0, 0)).ToString("dd/MM/yyyy");
                    ViewBag.fechaMiercoles = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)).ToString("dd/MM/yyyy");
                    ViewBag.fechaJueves = DateTime.Now.ToString("dd/MM/yyyy");
                    ViewBag.fechaViernes = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
                    ViewBag.fechaSabado = DateTime.Now.AddDays(2).ToString("dd/MM/yyyy");
                    ViewBag.fechaDomingo = DateTime.Now.AddDays(3).ToString("dd/MM/yyyy");
                    break;

                case System.DayOfWeek.Friday:
                    ViewBag.fechaLunes = DateTime.Now.Subtract(new TimeSpan(4, 0, 0, 0)).ToString("dd/MM/yyyy");
                    ViewBag.fechaMartes = DateTime.Now.Subtract(new TimeSpan(3, 0, 0, 0)).ToString("dd/MM/yyyy");
                    ViewBag.fechaMiercoles = DateTime.Now.Subtract(new TimeSpan(2, 0, 0, 0)).ToString("dd/MM/yyyy");
                    ViewBag.fechaJueves = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)).ToString("dd/MM/yyyy");
                    ViewBag.fechaViernes = DateTime.Now.ToString("dd/MM/yyyy");
                    ViewBag.fechaSabado= DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
                    ViewBag.fechaDomingo = DateTime.Now.AddDays(2).ToString("dd/MM/yyyy");

                    break;

                case System.DayOfWeek.Saturday:
                    ViewBag.fechaLunes = DateTime.Now.Subtract(new TimeSpan(5, 0, 0, 0)).ToString("dd/MM/yyyy");
                    ViewBag.fechaMartes = DateTime.Now.Subtract(new TimeSpan(4, 0, 0, 0)).ToString("dd/MM/yyyy");
                    ViewBag.fechaMiercoles = DateTime.Now.Subtract(new TimeSpan(3, 0, 0, 0)).ToString("dd/MM/yyyy");
                    ViewBag.fechaJueves = DateTime.Now.Subtract(new TimeSpan(2, 0, 0, 0)).ToString("dd/MM/yyyy");
                    ViewBag.fechaViernes = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)).ToString("dd/MM/yyyy");
                    ViewBag.fechaSabado = DateTime.Now.ToString("dd/MM/yyyy");
                    ViewBag.fechaDomingo = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
                    break;

                case System.DayOfWeek.Sunday:
                    ViewBag.fechaLunes = DateTime.Now.Subtract(new TimeSpan(6, 0, 0, 0)).ToString("dd/MM/yyyy");
                    ViewBag.fechaMartes = DateTime.Now.Subtract(new TimeSpan(5, 0, 0, 0)).ToString("dd/MM/yyyy");
                    ViewBag.fechaMiercoles = DateTime.Now.Subtract(new TimeSpan(4, 0, 0, 0)).ToString("dd/MM/yyyy");
                    ViewBag.fechaJueves = DateTime.Now.Subtract(new TimeSpan(3, 0, 0, 0)).ToString("dd/MM/yyyy");
                    ViewBag.fechaViernes = DateTime.Now.Subtract(new TimeSpan(2, 0, 0, 0)).ToString("dd/MM/yyyy");
                    ViewBag.fechaSabado = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)).ToString("dd/MM/yyyy");
                    ViewBag.fechaDomingo = DateTime.Now.ToString("dd/MM/yyyy");
                    break;

            }

            GUIAS_EMPLEADO empleado = baseDatos.GUIAS_EMPLEADO.Find(Session["IdUsuarioLogueado"].ToString());
            var id = Session["IdUsuarioLogueado"];
            if (empleado.TIPOEMPLEADO == "Guía Externo")
            {
                var empleados = baseDatos.GUIAS_EMPLEADO.Where(e=> e.CEDULA == id.ToString());
                empleados = empleados.OrderBy(e => e.NOMBREEMPLEADO);
                int pageSize = 8;
                int pageNumber = 1;
                return View(empleados.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                var empleados = baseDatos.GUIAS_EMPLEADO.Where(e=> e.TIPOEMPLEADO == "Guía Interno" || e.TIPOEMPLEADO == "Guía Externo" || e.TIPOEMPLEADO == "Administrador Local/Guía Interno");
                empleados = empleados.OrderBy(e => e.NOMBREEMPLEADO);
                int pageSize = 8;
                int pageNumber = 1;
                return View(empleados.ToPagedList(pageNumber, pageSize));
            }
           
        }

        [HttpPost]
        public ActionResult ConsultarAsignacion(DateTime semanaABuscar)
        {
            // if (semanaABuscar.)
            ViewBag.fechaLunes = semanaABuscar.ToString("MM/dd/yyyy");
            ViewBag.fechaMartes = semanaABuscar.AddDays(1).ToString("MM/dd/yyyy");
            ViewBag.fechaMiercoles = semanaABuscar.AddDays(2).ToString("MM/dd/yyyy");
            ViewBag.fechaJueves = semanaABuscar.AddDays(3).ToString("MM/dd/yyyy");
            ViewBag.fechaViernes = semanaABuscar.AddDays(4).ToString("MM/dd/yyyy");
            ViewBag.fechaSabado = semanaABuscar.AddDays(5).ToString("MM/dd/yyyy");
            ViewBag.fechaDomingo = semanaABuscar.AddDays(6).ToString("MM/dd/yyyy");

            GUIAS_EMPLEADO empleado = baseDatos.GUIAS_EMPLEADO.Find(Session["IdUsuarioLogueado"].ToString());
            var id = Session["IdUsuarioLogueado"];
            if (empleado.TIPOEMPLEADO == "Guía Externo")
            {
                var empleados = baseDatos.GUIAS_EMPLEADO.Where(e => e.CEDULA == id.ToString());
                empleados = empleados.OrderBy(e => e.NOMBREEMPLEADO);
                int pageSize = 8;
                int pageNumber = 1;
                return View(empleados.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                var empleados = baseDatos.GUIAS_EMPLEADO.Where(e => e.TIPOEMPLEADO == "Guía Interno" || e.TIPOEMPLEADO == "Guía Externo" || e.TIPOEMPLEADO == "Administrador Local/Guía Interno");
                empleados = empleados.OrderBy(e => e.NOMBREEMPLEADO);
                int pageSize = 8;
                int pageNumber = 1;
                return View(empleados.ToPagedList(pageNumber, pageSize));
            }
        }

        public ActionResult AsignarReservacionDetallada()
        {
            int id = 1;
            ViewBag.reserva = id;
            string identificacion = id.ToString();

            AsignacionModelos modelo;

            modelo = new AsignacionModelos(baseDatos.GUIAS_RESERVACION.Find(identificacion));
            ViewBag.fecha = String.Format("{0:M/d/yyyy}", modelo.modeloReservacion.FECHA).Trim();

            ViewBag.cambios = "Ninguno";

            List<string> guias = baseDatos.GUIAS_ASIGNACION.Where(p => p.NUMERORESERVACION.Equals(identificacion)).Select(s => s.CEDULAGUIA).ToList();
            List<GUIAS_EMPLEADO> guiasLibres = baseDatos.GUIAS_EMPLEADO.Where(p => !guias.Contains(p.CEDULA) && p.TIPOEMPLEADO.Contains("Guía") && p.NOMBREESTACION.Equals(modelo.modeloReservacion.NOMBREESTACION) ).ToList();
            List<GUIAS_EMPLEADO> guiasAsociados = baseDatos.GUIAS_EMPLEADO.Where(p => guias.Contains(p.CEDULA) && p.TIPOEMPLEADO.Contains("Guía")).ToList();

            /*Hay que filtrar el maximo de asignaciones para ese día del guía disponible, ya que no pueden ser mas de 4 y que sea un rol de día libre se ponga de otro color*/
            modelo.guiasDisponibles = guiasLibres;
            modelo.guiasAsignados = guiasAsociados;


            if (modelo == null)
            {
                return HttpNotFound();
            }

            return View(modelo);
        }

        public ActionResult agregarGuia(int? id, string reservacion, int rowCount)
        {
            if (id != null)
            {
                GUIAS_ASIGNACION modelo = new GUIAS_ASIGNACION();
                string iden = id.ToString();
                modelo.CEDULAGUIA = iden;
                modelo.NUMERORESERVACION = reservacion;

                baseDatos.GUIAS_ASIGNACION.Add(modelo);
                baseDatos.SaveChanges();
            }

            string cedula = id.ToString();
            GUIAS_EMPLEADO empleado = baseDatos.GUIAS_EMPLEADO.Find(cedula);
            AsignacionModelos mod = new AsignacionModelos();
            mod.guiasDisponibles.Add(empleado);
            
            ViewBag.rowCount = rowCount + 1;
            return View(mod);
        }

        public ActionResult eliminarGuia(int? id, string reservacion, int rowCount)
        {
            if (id != null)
            {
                string identificacion = id.ToString();
                List<GUIAS_ASIGNACION> asignacion = baseDatos.GUIAS_ASIGNACION.Where(p =>  p.CEDULAGUIA.Equals(identificacion) && p.NUMERORESERVACION.Equals(reservacion)).ToList();
                if (asignacion != null && asignacion.Count() == 1)
                {
                    baseDatos.GUIAS_ASIGNACION.Remove(asignacion.ElementAt(0));
                    baseDatos.SaveChanges();
                }
            }

            string cedula = id.ToString();
            GUIAS_EMPLEADO empleado = baseDatos.GUIAS_EMPLEADO.Find(cedula);
            AsignacionModelos mod = new AsignacionModelos();
            mod.guiasAsignados.Add(empleado);

            ViewBag.rowCount = rowCount + 1;
            return View(mod);
        }

        public ActionResult Notificaciones(string sortOrder, string currentFilter, string fechaDesde, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.ReservacionSortParm = String.IsNullOrEmpty(sortOrder) ? "Reservacion" : "";
            ViewBag.NombreSortParm = String.IsNullOrEmpty(sortOrder) ? "Nombre" : "";
            ViewBag.EstacionSortParm = String.IsNullOrEmpty(sortOrder) ? "Estacion" : "";
            ViewBag.PersonasSortParm = String.IsNullOrEmpty(sortOrder) ? "Personas" : "";
            ViewBag.FechaSortParm = String.IsNullOrEmpty(sortOrder) ? "Fecha" : "";
            ViewBag.HoraSortParm = String.IsNullOrEmpty(sortOrder) ? "Hora" : "";
            ViewBag.GuiasAsignadosSortParm = String.IsNullOrEmpty(sortOrder) ? "Guías Asignados" : "";

            if (fechaDesde != null)
            {
                page = 1;
            }
            else
            {
                fechaDesde = currentFilter;
            }

            ViewBag.CurrentFilter = fechaDesde;

            var empleados = from e in baseDatos.GUIAS_EMPLEADO select e;

            if (!String.IsNullOrEmpty(fechaDesde))
            {
                empleados = empleados.Where(e => e.APELLIDO1.Contains(fechaDesde)
                                       || e.NOMBREEMPLEADO.Contains(fechaDesde) || e.APELLIDO2.Contains(fechaDesde)
                                       || e.NOMBREESTACION.Contains(fechaDesde) || e.TIPOEMPLEADO.Contains(fechaDesde)
                                       || e.USUARIO.Contains(fechaDesde) || e.EMAIL.Contains(fechaDesde));
            }


            switch (sortOrder)
            {
                case "Reservacion":
                    empleados = empleados.OrderBy(e => e.NOMBREEMPLEADO);
                    break;
                case "Nombre":
                    empleados = empleados.OrderBy(e => e.APELLIDO1);
                    break;
                case "Estacion":
                    empleados = empleados.OrderBy(e => e.APELLIDO2);
                    break;
                case "Personas":
                    empleados = empleados.OrderBy(e => e.NOMBREESTACION);
                    break;
                case "Fecha":
                    empleados = empleados.OrderBy(e => e.TIPOEMPLEADO);
                    break;
                case "Hora":
                    empleados = empleados.OrderBy(e => e.APELLIDO2);
                    break;
                case "Turno":
                    empleados = empleados.OrderBy(e => e.NOMBREESTACION);
                    break;
                case "Guías Asignados":
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

        //mostrar la reservacion del usuario correspondiente
        public ActionResult ConsultarAsignacionDetallada(int? id, string fecha, string turno)
        {
            
            //string numReservacion = "";
            ReservacionesModelos modelo;
            //List<GUIAS_RESERVACION> lista = null;
            List<string> acompañantes = null; 
            //IQueryable<GUIAS_ASIGNACION> asignacionAxuliar;
            //IQueryable<GUIAS_ASOCIAEXTERNO> listaPersonasDos;
            GUIAS_EMPLEADO empleado = baseDatos.GUIAS_EMPLEADO.Find(id.ToString());
            GUIAS_RESERVACION reservacionAsignada = null;
           // GUIAS_ASIGNACION asignacion;
            DateTime date = Convert.ToDateTime(fecha);
            string nombreGuias = "";
           
            try
            {
                IQueryable<string> numReservacion = baseDatos.GUIAS_ASIGNACION.Where(i => i.CEDULAGUIA == id.ToString() && i.TURNO == turno).Select(a=> a.NUMERORESERVACION);
                if (numReservacion != null && numReservacion.Count() != 0)
                {
                    reservacionAsignada = baseDatos.GUIAS_RESERVACION.Find(numReservacion.ElementAt(0)); // PREGUNTAR POR FECHA
                    IEnumerable<string> cedEmpleados = baseDatos.GUIAS_ASIGNACION.Where(i => i.NUMERORESERVACION == numReservacion.ElementAt(0)).Select(i=> i.CEDULAGUIA) ;
                    if (cedEmpleados != null && cedEmpleados.Count() != 0) {
                        foreach (var ced in cedEmpleados)
                        {
                            IEnumerable<string> estructuraAuxiliar = baseDatos.GUIAS_EMPLEADO.Where(i => i.CEDULA == ced).Select(i=> i.NOMBREEMPLEADO + " " +i.APELLIDO1 + " "+i.APELLIDO2 );
                            if (estructuraAuxiliar != null)
                            {
                                nombreGuias = estructuraAuxiliar.ElementAt(0);
                            }
                        }
                        acompañantes.Add(nombreGuias);   
                    }
                }
            }
            catch(Exception e){
            }
            modelo = new ReservacionesModelos
            {
                reservacion = reservacionAsignada,
                compañeros = acompañantes,
                modeloEmpleado = empleado
            };
            return View(modelo);
            

        }



    }
}