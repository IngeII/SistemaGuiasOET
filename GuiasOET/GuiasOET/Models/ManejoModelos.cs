using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuiasOET.Models
{
    public class ManejoModelos
    {
        public GuiasOET.Models.GUIAS_EMPLEADO modeloEmpleado { get; set; }
        public GuiasOET.Models.GUIAS_TELEFONO modeloTelefono { get; set; }

        public ManejoModelos(GuiasOET.Models.GUIAS_EMPLEADO empleado)
        {
            modeloEmpleado = empleado;
        }

    }
}