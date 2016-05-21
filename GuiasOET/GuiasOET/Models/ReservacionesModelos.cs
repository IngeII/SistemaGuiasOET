using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuiasOET.Models
{
    public class ReservacionesModelos
    {
        public GuiasOET.Models.GUIAS_EMPLEADO modeloEmpleado { get; set; }
        public GUIAS_RESERVACION reservacion { get; set; }
        public IEnumerable<string> compañeros { get; set; }
        public string TURNO { get; set; }

    }
}