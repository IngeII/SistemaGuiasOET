using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using GuiasOET.Models;
using System.Collections;

namespace GuiasOET.Controllers
{
    public class ReportesController : Controller
    {
        // GET: Reportes
        public ActionResult Index()
        {
            return View();
        }

        private Entities1 baseDatos = new Entities1();

        [HttpGet]
        public ActionResult ReportesReservaciones(string fechaDesde, string fechaHasta, int? page)
        {

            /* Se define tamaño de la pagina */
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            ViewBag.pageNumber = pageNumber;

            //Todas las reservaciones del sistema
            var reservacion = from r in baseDatos.GUIAS_RESERVACION select r;

            return View();
        }

        public ActionResult PersonasAtendidas(string fechaDesde, string fechaHasta)
        {
            ReportesModelo reportes;
            DateTime fechaD = Convert.ToDateTime(fechaHasta);
            DateTime fechaH = Convert.ToDateTime(fechaHasta);
            List<GUIAS_RESERVACION> reservaciones = baseDatos.GUIAS_RESERVACION.Where(e => e.FECHA < fechaH && e.FECHA > fechaD).ToList();

            reservaciones = reservaciones.OrderBy(r=> r.FECHA).ToList();

            var asignaciones = from r in baseDatos.GUIAS_ASIGNACION select r;



            return View("ReportesReservaciones");
        }

        public ActionResult Reservaciones()
        {




            return View("ReportesReservaciones");
        }


        public void calcularSubTotales()
        {

        }
    }
}