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



        [HttpGet]
        public ActionResult AsignarReservacion(string sortOrder, string currentFilter1, string currentFilter2, string fechaDesde, string fechaHasta, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.ReservacionSortParm = String.IsNullOrEmpty(sortOrder) ? "Reservacion" : "";
            ViewBag.NombreSortParm = String.IsNullOrEmpty(sortOrder) ? "Nombre" : "";
            ViewBag.EstacionSortParm = String.IsNullOrEmpty(sortOrder) ? "Estacion" : "";
            ViewBag.PersonasSortParm = String.IsNullOrEmpty(sortOrder) ? "Personas" : "";
            ViewBag.FechaSortParm = String.IsNullOrEmpty(sortOrder) ? "Fecha" : "";
            ViewBag.HoraSortParm = String.IsNullOrEmpty(sortOrder) ? "Hora" : "";

            Debug.WriteLine("fecha es: " + fechaDesde);
            string pruebaFecha = ViewBag.CurrentFilter1;
            Debug.WriteLine("fecha ES: " + pruebaFecha);

            AsignacionModelos table = new AsignacionModelos();


            /*    if (fechaDesde != null)
                 {
                     page = 1;
                 }
                 else
                 {
                     fechaDesde = currentFilter;
                 } 

          ViewBag.CurrentFilter = fechaDesde;  */


            /* Se define tamaño de la pagina */
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            ViewBag.pageNumber = pageNumber;

            Debug.WriteLine("la fecha es: " + fechaDesde);

            /*  if (searchString != null)
              {
                  page = 1;
              }
              else
              {
                  searchString = currentFilter;
              }

              ViewBag.CurrentFilter = searchString; */



            //Todas las reservaciones del sistema
            var reservacion = from r in baseDatos.GUIAS_RESERVACION select r;

            //Lista reservaciones guia externo
            List<IQueryable<GUIAS_RESERVACION>> reserv = new List<IQueryable<GUIAS_RESERVACION>>();


            //Todas las reservaciones con guias
            var reservacionAsignada = from p in baseDatos.GUIAS_ASIGNACION select p;

            //Todos los guias del sistema
            var guias = from p in baseDatos.GUIAS_EMPLEADO select p;

            //Lista que contiene todas las reservaciones sin guias 
            List<GUIAS_ASIGNACION> reservacionesConAsignacion = new List<GUIAS_ASIGNACION>();

            //Lista que contiene los guias
            List<IEnumerable<GUIAS_EMPLEADO>> guiasAsignados = new List<IEnumerable<GUIAS_EMPLEADO>>();

            //Lista que contiene los guias de todas las reservaciones
            List<IEnumerable<GUIAS_EMPLEADO>> totalGuiasAsignados = new List<IEnumerable<GUIAS_EMPLEADO>>();

            //Lista que contiene el total de guias para cada reservacion
            IEnumerable<GUIAS_EMPLEADO> totalGuias;


            //Lista que contiene el total de guias para cada reservacion
            IEnumerable<GUIAS_RESERVACION> totalReservasAsignadas;

            List<GUIAS_RESERVACION> listaReservaciones = new List<GUIAS_RESERVACION>();

            DateTime fechaInicio;
            DateTime fechaFin;

            String fecha = "";

            string estacion = "";

            //Lista que contiene los guias de todas las reservaciones
            List<IEnumerable<GUIAS_RESERVACION>> totalReservas = new List<IEnumerable<GUIAS_RESERVACION>>();

            string rol = Session["RolUsuarioLogueado"].ToString();

            if (rol.Contains("Local") || rol.Contains("Interno") || rol.Contains("Global"))
            {
                if (rol.Contains("Local") || rol.Contains("Interno"))
                {
                    estacion = Session["EstacionUsuarioLogueado"].ToString();
                    reservacion = reservacion.Where(e => e.NOMBREESTACION.Equals(estacion));
                }
                //Ninguna fecha es vacía
                if (!String.IsNullOrEmpty(fechaDesde) && !String.IsNullOrEmpty(fechaHasta))
                {

                    fechaInicio = Convert.ToDateTime(fechaDesde);
                    fechaFin = Convert.ToDateTime(fechaHasta);


                    if (rol.Contains("Local") || rol.Contains("Interno"))
                    {
                        reservacion = (reservacion.Where(e => e.FECHA >= fechaInicio && e.FECHA <= fechaFin && e.NOMBREESTACION.Equals(estacion)));
                        Debug.WriteLine("entre a todas fechas vacias ");
                    }
                    else
                    {
                        reservacion = (reservacion.Where(e => e.FECHA >= fechaInicio && e.FECHA <= fechaFin));
                        Debug.WriteLine("entre a todas fechas vacias ");
                    }

                    page = 1;
                }

                //Solo la fecha inicial es vacía
                else if (String.IsNullOrEmpty(fechaDesde) && !(String.IsNullOrEmpty(fechaHasta)))
                {

                    fechaFin = Convert.ToDateTime(fechaHasta);


                    if (rol.Contains("Local") || rol.Contains("Interno"))
                    {
                        reservacion = (reservacion.Where(e => e.FECHA <= fechaFin && e.NOMBREESTACION.Equals(estacion)));
                        Debug.WriteLine("entre a todas fechas vacias ");
                    }
                    else
                    {
                        reservacion = (reservacion.Where(e => e.FECHA <= fechaFin));
                        Debug.WriteLine("entre a todas fechas vacias ");
                    }

                    page = 1;

                }

                //Solo la fecha final es vacía
                else if (!(String.IsNullOrEmpty(fechaDesde)) && String.IsNullOrEmpty(fechaHasta))
                {
                    fechaInicio = Convert.ToDateTime(fechaDesde);
                    if (rol.Contains("Local") || rol.Contains("Interno"))
                    {
                        reservacion = (reservacion.Where(e => e.FECHA >= fechaInicio && e.NOMBREESTACION.Equals(estacion)));
                        Debug.WriteLine("entre a todas fechas vacias ");
                    }
                    else
                    {
                        reservacion = (reservacion.Where(e => e.FECHA >= fechaInicio));
                        Debug.WriteLine("entre a todas fechas vacias ");
                    }

                    page = 1;


                    //   reservacion = (reservacion.Where(e => e.FECHA >= fechaInicio && e.NOMBREESTACION.Equals(estacion)));
                    //     Debug.WriteLine("entre a fecha inicial vacia ");

                }
                else
                {
                    fechaDesde = currentFilter1;
                    fechaHasta = currentFilter2;
                }

                ViewBag.CurrentFilter1 = fechaDesde;
                string xi = ViewBag.CurrentFilter1;
                Debug.WriteLine("CURRENT FILTER 1" + xi);


                ViewBag.CurrentFilter2 = fechaHasta;
                string yi = ViewBag.CurrentFilter2;
                Debug.WriteLine("CURRENT FILTER 2" + yi);


                ViewBag.CurrentFilter3 = "JOE";

                foreach (var row in reservacion)
                {

                    fecha = String.Format("{0:M/d/yyyy}", row.FECHA).Trim();
                    fecha = "1";
                    //      table.fechaReservaciones.Add(fecha);

                    reservacionAsignada = reservacionAsignada.Where(e => e.NUMERORESERVACION.Equals(row.NUMERORESERVACION));

                    GUIAS_ASIGNACION reserva = baseDatos.GUIAS_ASIGNACION.FirstOrDefault(i => i.NUMERORESERVACION.Equals(row.NUMERORESERVACION));

                    if (reserva != null)
                    {
                        reservacionesConAsignacion.Add(reserva);

                        foreach (var row2 in reservacionAsignada)
                        {
                            guias = guias.Where(x => x.CEDULA.Equals(row2.CEDULAGUIA));
                            guiasAsignados.Add(guias);
                            guias = from p in baseDatos.GUIAS_EMPLEADO select p;

                        }

                        totalGuias = guiasAsignados.ElementAt(0);

                        for (int y = 1; y < guiasAsignados.Count(); ++y)
                        {
                            totalGuias = totalGuias.Concat(guiasAsignados.ElementAt(y));
                        }

                        totalGuiasAsignados.Add(totalGuias);

                    }


                    guiasAsignados.Clear();
                    reservacionAsignada = from p in baseDatos.GUIAS_ASIGNACION select p;
                    guias = from p in baseDatos.GUIAS_EMPLEADO select p;

                }


                //Todas las reservaciones del sistema
                table.totalReservaciones = reservacion.OrderBy(e => e.NUMERORESERVACION).ToPagedList(pageNumber, pageSize);

                //Se asigna la cantidad de paginas
                var count = reservacion.Count();
                decimal totalPages = count / (decimal)pageSize;
                ViewBag.TotalPages = Math.Ceiling(totalPages);

                //Todos los guias asociados a las reservaciones
                table.empleados = totalGuiasAsignados.ToPagedList(pageNumber, pageSize);

                //Se guardan las fechas
                //      table.fechasReservaciones = table.fechaReservaciones.ToPagedList(pageNumber, pageSize);

                //Todas las reservaciones que tienen guias
                table.reservacionesAsignadas = reservacionesConAsignacion.ToPagedList(pageNumber, pageSize);



            }
            else if (Session["RolUsuarioLogueado"].ToString().Contains("Externo"))
            {
                string cedula = Session["IdUsuarioLogueado"].ToString();
                var reservacionesAsignadas = baseDatos.GUIAS_ASIGNACION.Where(x => x.CEDULAGUIA.Equals(cedula));


                IQueryable<GUIAS_RESERVACION> todasReservaciones = baseDatos.GUIAS_RESERVACION;


                //Ninguna fecha es vacía
                if (!String.IsNullOrEmpty(fechaDesde) && !String.IsNullOrEmpty(fechaHasta))
                {

                    fechaInicio = Convert.ToDateTime(fechaDesde);
                    fechaFin = Convert.ToDateTime(fechaHasta);
                    todasReservaciones = (baseDatos.GUIAS_RESERVACION.Where(e => e.FECHA >= fechaInicio && e.FECHA <= fechaFin));
                    Debug.WriteLine("entre a todas fechas vacias ");

                }

                //Solo la fecha inicial es vacía
                else if (String.IsNullOrEmpty(fechaDesde) && !(String.IsNullOrEmpty(fechaHasta)))
                {
                    fechaFin = Convert.ToDateTime(fechaHasta);
                    todasReservaciones = (baseDatos.GUIAS_RESERVACION.Where(e => e.FECHA <= fechaFin));

                    //            reservacion = (reservacion.Where(e => e.FECHA <= fechaFin && e.NOMBREESTACION.Equals(estacion)));

                }

                //Solo la fecha final es vacía
                else if (!(String.IsNullOrEmpty(fechaDesde)) && String.IsNullOrEmpty(fechaHasta))
                {
                    fechaInicio = Convert.ToDateTime(fechaDesde);
                    todasReservaciones = (baseDatos.GUIAS_RESERVACION.Where(e => e.FECHA >= fechaInicio));
                    //  reservacion = (reservacion.Where(e => e.FECHA >= fechaInicio && e.NOMBREESTACION.Equals(estacion)));
                    Debug.WriteLine("entre a fecha inicial vacia ");
                }



                IEnumerable<GUIAS_EMPLEADO> guia = baseDatos.GUIAS_EMPLEADO.Where(x => x.CEDULA.Equals(cedula));

                GUIAS_RESERVACION datosReserva;


                foreach (var row in reservacionesAsignadas)
                {

                    datosReserva = todasReservaciones.FirstOrDefault(e => e.NUMERORESERVACION == row.NUMERORESERVACION);

                    if (datosReserva != null)
                    {
                        listaReservaciones.Add(datosReserva);
                        reservacionesConAsignacion.Add(row);
                    }

                    guias = guias.Where(x => x.CEDULA.Equals(row.CEDULAGUIA));
                    guiasAsignados.Add(guias);


                    if (guiasAsignados.Count() > 0)
                    {


                        totalGuias = guiasAsignados.ElementAt(0);

                        for (int y = 1; y < guiasAsignados.Count(); ++y)
                        {
                            totalGuias = totalGuias.Concat(guiasAsignados.ElementAt(y));
                        }

                        totalGuiasAsignados.Add(totalGuias);

                    }

                    guias = from p in baseDatos.GUIAS_EMPLEADO select p;
                    // todasReservaciones = baseDatos.GUIAS_RESERVACION;
                    guiasAsignados.Clear();
                }



                //Todas las reservaciones del sistema
                table.totalReservaciones = listaReservaciones.OrderBy(x => x.NUMERORESERVACION).ToPagedList(pageNumber, pageSize);

                //Se asigna la cantidad de paginas
                var count = listaReservaciones.Count();
                decimal totalPages = count / (decimal)pageSize;
                ViewBag.TotalPages = Math.Ceiling(totalPages);


                //Todos los guias asociados a las reservaciones
                table.empleados = totalGuiasAsignados.ToPagedList(pageNumber, pageSize);

                //Todas las reservaciones que tienen guias
                table.reservacionesAsignadas = reservacionesConAsignacion.ToPagedList(pageNumber, pageSize);


            }

            List<GUIAS_RESERVACION> listaTotalReservaciones = new List<GUIAS_RESERVACION>();
            if (rol.Contains("Local") || rol.Contains("Interno") || rol.Contains("Global"))
            {
                listaTotalReservaciones = reservacion.ToList();
            }
            else
            {
                listaTotalReservaciones = listaReservaciones;
            }

            var datos = listaTotalReservaciones.OrderBy(e => e.NUMERORESERVACION);
            switch (sortOrder)
            {
                case "Reservacion":
                    datos = listaTotalReservaciones.OrderBy(e => e.NUMERORESERVACION);

                    //     listaTotalReservaciones = listaTotalReservaciones.OrderBy(e => e.NUMERORESERVACION);
                    //     table.totalReservaciones = listaTotalReservaciones.OrderBy(e => e.NUMERORESERVACION).ToPagedList(pageNumber, pageSize);
                    //    table.totalReservaciones = table.totalReservaciones.OrderBy(e => e.NUMERORESERVACION).ToPagedList(pageNumber, pageSize);
                    //      table.reservacionesAsignadas = table.reservacionesAsignadas.OrderBy(e => e.NUMERORESERVACION).ToPagedList(pageNumber, pageSize);
                    break;
                case "Nombre":
                    datos = listaTotalReservaciones.OrderBy(e => e.NOMBRERESERVACION);
                    //  table.totalReservaciones = listaTotalReservaciones.OrderBy(e => e.NOMBRERESERVACION).ToPagedList(pageNumber, pageSize);
                    //        table.totalReservaciones = table.totalReservaciones.OrderBy(e => e.NOMBRERESERVACION).ToPagedList(pageNumber, pageSize);
                    //   table.reservaciones = table.reservaciones.OrderBy(e => e.NOMBRERESERVACION).ToPagedList(pageNumber, pageSize);
                    break;
                case "Estacion":
                    datos = listaTotalReservaciones.OrderBy(e => e.NOMBREESTACION);
                    //  table.totalReservaciones = listaTotalReservaciones.OrderBy(e => e.NOMBREESTACION).ToPagedList(pageNumber, pageSize);
                    //     table.totalReservaciones = table.totalReservaciones.OrderBy(e => e.NOMBREESTACION).ToPagedList(pageNumber, pageSize);
                    //   table.reservaciones = table.reservaciones.OrderBy(e => e.NOMBRERESERVACION).ToPagedList(pageNumber, pageSize);
                    break;
                case "Personas":
                    datos = listaTotalReservaciones.OrderBy(e => e.NUMEROPERSONAS);
                    //   table.reservaciones = listaTotalReservaciones.OrderBy(e => e.NUMEROPERSONAS).ToPagedList(pageNumber, pageSize);
                    //     table.reservaciones = table.reservaciones.OrderBy(e => e.NUMEROPERSONAS).ToPagedList(pageNumber, pageSize);
                    //        table.totalReservaciones = table.totalReservaciones.OrderBy(e => e.NUMEROPERSONAS).ToPagedList(pageNumber, pageSize);
                    break;
                case "Fecha":
                    datos = listaTotalReservaciones.OrderBy(e => e.FECHA);
                    //   table.reservaciones = listaTotalReservaciones.OrderBy(e => e.FECHA).ToPagedList(pageNumber, pageSize);
                    //  table.reservaciones = table.reservaciones.OrderBy(e => e.FECHA).ToPagedList(pageNumber, pageSize);
                    //    table.totalReservaciones = table.totalReservaciones.OrderBy(e => e.FECHA).ToPagedList(pageNumber, pageSize);
                    break;
                case "Hora":
                    datos = listaTotalReservaciones.OrderBy(e => e.HORA);
                    //    table.reservaciones = listaTotalReservaciones.OrderBy(e => e.HORA).ToPagedList(pageNumber, pageSize);
                    // table.reservaciones = table.reservaciones.OrderBy(e => e.HORA).ToPagedList(pageNumber, pageSize);
                    //       table.totalReservaciones = table.totalReservaciones.OrderBy(e => e.HORA).ToPagedList(pageNumber, pageSize);
                    break;
                default:
                    datos = listaTotalReservaciones.OrderBy(e => e.NUMERORESERVACION);
                    //  table.totalReservaciones = table.totalReservaciones.OrderBy(e => e.NUMERORESERVACION).ToPagedList(pageNumber, pageSize);
                    //  table.totalReservaciones = listaTotalReservaciones.OrderBy(e => e.NUMERORESERVACION).ToPagedList(pageNumber, pageSize);
                    break;
            }

            table.totalReservaciones = datos.ToPagedList(pageNumber, pageSize);

            ViewBag.MessagesInOnePage = table.totalReservaciones;
            ViewBag.PageNumber = pageNumber;

            return View(table);
        }
        /**
         * 
         * */
        [HttpGet]
        public ActionResult ConsultarAsignacion(int? page)
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
                int pageNumber = (page ?? 1);
                return View(empleados.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                var empleados = baseDatos.GUIAS_EMPLEADO.Where(e=> e.TIPOEMPLEADO == "Guía Interno" || e.TIPOEMPLEADO == "Guía Externo" || e.TIPOEMPLEADO == "Administrador Local/Guía Interno");
                empleados = empleados.OrderBy(e => e.NOMBREEMPLEADO);
                int pageSize = 8;
                int pageNumber = (page ?? 1);
                return View(empleados.ToPagedList(pageNumber, pageSize));
            }


        }
        // GET: Inicio
        /*Método GET de la pantalla ListaUsuarios*/
        public ActionResult Inicio(int? page)
        {
            return View();
        }

        [HttpPost]
        public ActionResult ConsultarAsignacion(DateTime semanaABuscar)
        {
            // if (semanaABuscar.)
            ViewBag.fechaLunes = semanaABuscar.ToString("dd/MM/yyyy");
            ViewBag.fechaMartes = semanaABuscar.AddDays(1).ToString("dd/MM/yyyy");
            ViewBag.fechaMiercoles = semanaABuscar.AddDays(2).ToString("dd/MM/yyyy");
            ViewBag.fechaJueves = semanaABuscar.AddDays(3).ToString("dd/MM/yyyy");
            ViewBag.fechaViernes = semanaABuscar.AddDays(4).ToString("dd/MM/yyyy");
            ViewBag.fechaSabado = semanaABuscar.AddDays(5).ToString("dd/MM/yyyy");
            ViewBag.fechaDomingo = semanaABuscar.AddDays(6).ToString("dd/MM/yyyy");

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

        public ActionResult AsignarReservacionDetallada(int? id)
        {
            
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
            List<string> acompañantes = new List<string>();
            string identificacion = id.ToString();
            GUIAS_ASIGNACION asignacion;
            string reserv;
            GUIAS_EMPLEADO empleado = baseDatos.GUIAS_EMPLEADO.Find(identificacion);
            GUIAS_RESERVACION reservacionAsignada = null;
            DateTime date = Convert.ToDateTime(fecha);
            string nombreGuias = "";
            Boolean indicador = true;
            int indice = 0;

            try
            {

                List<string> numReservacion = baseDatos.GUIAS_ASIGNACION.Where(i => i.CEDULAGUIA.Equals(identificacion) && i.TURNO.Equals(turno)).Select(a => a.NUMERORESERVACION).ToList();
                

         
                    while (numReservacion != null && indicador == true && indice < numReservacion.Count())
                    {
                        reserv = numReservacion.ElementAt(indice);
                        List<GUIAS_RESERVACION> reservacionAuxiliar = baseDatos.GUIAS_RESERVACION.Where(i => i.NUMERORESERVACION == reserv).ToList();       
                        if (reservacionAuxiliar != null && reservacionAuxiliar.Count() != 0)
                        {
                           
                            if (reservacionAuxiliar.ElementAt(0).FECHA.ToString() == date.ToString())
                            {
                                indicador = false;
                                string cedula = "";
                                reservacionAsignada = reservacionAuxiliar.ElementAt(0);
                                List<GUIAS_ASIGNACION> cedEmpleados = baseDatos.GUIAS_ASIGNACION.Where(i => i.NUMERORESERVACION == reservacionAsignada.NUMERORESERVACION).ToList();
                                if (cedEmpleados != null && cedEmpleados.Count() != 0)
                                {
                                    foreach (var ced in cedEmpleados)
                                    {
                                        cedula = ced.CEDULAGUIA.ToString();
                                        List<GUIAS_EMPLEADO> estructuraAuxiliar = baseDatos.GUIAS_EMPLEADO.Where(i => i.CEDULA == cedula).ToList();
                                        if (estructuraAuxiliar != null)
                                        {
                                            nombreGuias = estructuraAuxiliar.ElementAt(0).NOMBREEMPLEADO.ToString() + " " + estructuraAuxiliar.ElementAt(0).APELLIDO1.ToString();
                                        }

                                        acompañantes.Add(nombreGuias);
                                    }

                                }

                        }else
                        {
                            ++indice;
                        }

                                            
                        }else
                        {
                            ++indice;
                        }
                }
            }
            catch(Exception e){
            }
            modelo = new ReservacionesModelos
            {
                reservacion = reservacionAsignada,
                compañeros = acompañantes,
                TURNO = turno,
                modeloEmpleado = empleado
            };
            return View(modelo);
            

        }



    }
}