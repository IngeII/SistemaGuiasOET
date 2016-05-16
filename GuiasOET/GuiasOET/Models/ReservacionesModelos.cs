using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuiasOET.Models
{
    public class ReservacionesModelos
    {
        public IEnumerable<GUIAS_RESERVACION> ListReservaciones { get; set; }
        public GuiasOET.Models.GUIAS_EMPLEADO modeloEmpleado { get; set; }

    }
}