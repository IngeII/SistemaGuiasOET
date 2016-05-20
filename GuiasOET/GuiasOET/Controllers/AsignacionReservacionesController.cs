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

        [HttpGet]
        public ActionResult ConsultarAsignacion()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ConsultarAsignacion(DateTime semanaABuscar)
        {
            System.DayOfWeek diaDeLaSemana = semanaABuscar.DayOfWeek;
            switch (diaDeLaSemana)
            {
                case System.DayOfWeek.Tuesday:
                    semanaABuscar.Subtract(new TimeSpan(1, 0, 0, 0));
                    break;
                case System.DayOfWeek.Wednesday:
                    semanaABuscar.Subtract(new TimeSpan(2, 0, 0, 0));
                    break;
            }
            GUIAS_EMPLEADO empleado = baseDatos.GUIAS_EMPLEADO.Find(Session["IdUsuarioLogueado"]);
            switch (empleado.TIPOEMPLEADO)
            {

            }
            return View();
        }

        //mostrar la lista de reservaciones del usuario correspondiente
        public ActionResult ConsultarAsignacionDetallada(int? id, string fecha)
        {

            ReservacionesModelos modelo;
            List<GUIAS_RESERVACION> lista = null;
            //IQueryable<GUIAS_ASIGNAINTERNO> listaPersonasUno;
            //IQueryable<GUIAS_ASOCIAEXTERNO> listaPersonasDos;
            GUIAS_EMPLEADO empleado = baseDatos.GUIAS_EMPLEADO.Find(id.ToString());
            GUIAS_RESERVACION reservacion;
            DateTime date = Convert.ToDateTime(fecha);
                try
                {
                    IEnumerable<GUIAS_ASIGNACION> listaPersonasUno = baseDatos.GUIAS_ASIGNACION.Where(i => i.CEDULAGUIA == id.ToString());

                    if (listaPersonasUno != null && listaPersonasUno.Count() != 0)
                    {
                        foreach (GUIAS_ASIGNACION guia in listaPersonasUno)
                        {
                            reservacion = baseDatos.GUIAS_RESERVACION.Find(guia.NUMERORESERVACION);
                            if (reservacion != null && reservacion.FECHA == date)
                            {
                                lista.Add(reservacion);
                            }
                        }

                    }
                }catch(Exception e)
                {
                    Console.Write(e);
                }

            modelo = new ReservacionesModelos
            {
                ListReservaciones = lista,
                modeloEmpleado = empleado
            };

            return View(modelo);
           
            //return View();
        }






    }
}