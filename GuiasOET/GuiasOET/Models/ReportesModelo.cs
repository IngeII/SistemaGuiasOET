using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace GuiasOET.Models
{
    public class ReportesModelo
    {

        public List<GUIAS_RESERVACION> reservaciones = new List<GUIAS_RESERVACION>();
        public List<IEnumerable<GUIAS_EMPLEADO>> empleados = new List<IEnumerable<GUIAS_EMPLEADO>>();
        public List<GUIAS_ASIGNACION> reservacionesAsignadas = new List<GUIAS_ASIGNACION>();
        public IPagedList<GUIAS_RESERVACION> totalReservaciones { get; set; }
        public List<DateTime> fechasReservaciones = new List<DateTime>();
        public List<List<int>> subTotales = new List<List<int>>();


        public ReportesModelo()
        {
           
        }
    }
}