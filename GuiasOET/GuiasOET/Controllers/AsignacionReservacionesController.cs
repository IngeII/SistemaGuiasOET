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

        public ActionResult ConsultarAsignacion()
        {
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

            if (empleado.TIPOEMPLEADO.Contains("Guía Interno")==true)
            {

                try
                {
                    IEnumerable<GUIAS_ASIGNAINTERNO> listaPersonasUno = baseDatos.GUIAS_ASIGNAINTERNO.Where(i => i.CEDULAINTERNO == id.ToString());

                    if (listaPersonasUno != null && listaPersonasUno.Count() != 0)
                    {
                        foreach (GUIAS_ASIGNAINTERNO guia in listaPersonasUno)
                        {
                            lista.Add(baseDatos.GUIAS_RESERVACION.Find(guia.NUMERORESERVACION));
                        }

                    }
                }catch(Exception e)
                {
                    Console.Write(e);
                }
            }else if (empleado.TIPOEMPLEADO.Contains("Guía Externo") == true)
            {
                try
                {
                    IEnumerable<GUIAS_ASOCIAEXTERNO> listaPersonasDos = baseDatos.GUIAS_ASOCIAEXTERNO.Where(i => i.CEDULAEXTERNO == id.ToString());
                    // string a = listaPersonasDos.ElementAt(0).CEDULAEXTERNO.ToString();
                    if (listaPersonasDos != null && listaPersonasDos.Count() != 0)
                    {
                        foreach (GUIAS_ASOCIAEXTERNO guia in listaPersonasDos)
                        {
                            lista.Add(baseDatos.GUIAS_RESERVACION.Find(guia.NUMERORESERVACION));
                        }

                    }
                }catch (Exception e)
                {
                    Console.Write(e);
                }
            }

            modelo = new ReservacionesModelos
            {
                ListReservaciones = lista,
                modeloEmpleado = empleado
            };

            return View(modelo);
        }



        // GET: ListaUsuarios
        /*Método GET de la pantalla Consultar asignaciones*/
        /*public ActionResult ConsultarAsignacionDetallada(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.Fecha = "15/05/2016";
            ViewBag.Nombre = "LUIS LEANDRO";
            ViewBag.CurrentSort = sortOrder;
            ViewBag.ReservacionSortParm = String.IsNullOrEmpty(sortOrder) ? "Reservacion" : "";
            ViewBag.NombreSortParmSortParm = String.IsNullOrEmpty(sortOrder) ? "Nombre" : "";
            ViewBag.EstacionSortParm = String.IsNullOrEmpty(sortOrder) ? "Estacion" : "";
            ViewBag.PersonasSortParm = String.IsNullOrEmpty(sortOrder) ? "Estacion" : "";
            ViewBag.HoraSortParm = String.IsNullOrEmpty(sortOrder) ? "Hora" : "";
            ViewBag.TurnoSortParm = String.IsNullOrEmpty(sortOrder) ? "Turno" : "";

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

           /** if (!String.IsNullOrEmpty(searchString))
            {
                empleados = empleados.Where(e => e.APELLIDO1.Contains(searchString)
                                       || e.NOMBREEMPLEADO.Contains(searchString) || e.APELLIDO2.Contains(searchString)
                                       || e.NOMBREESTACION.Contains(searchString) || e.TIPOEMPLEADO.Contains(searchString)
                                       || e.USUARIO.Contains(searchString) || e.EMAIL.Contains(searchString));
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
            }**/

           // int pageSize = 8;
           // int pageNumber = (page ?? 1);
           // return View(empleados.ToPagedList(pageNumber, pageSize));
       // }




    }
}