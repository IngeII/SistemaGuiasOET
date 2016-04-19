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
        public GuiasOET.Models.GUIAS_TELEFONO modeloTelefono2 { get; set; }
        public GuiasOET.Models.GUIAS_TELEFONO modeloTelefono3 { get; set; }
        public GuiasOET.Models.GUIAS_TELEFONO modeloTelefono4 { get; set; }

        public ManejoModelos(GuiasOET.Models.GUIAS_EMPLEADO empleado)
        {
            modeloEmpleado = empleado;
        }

        public ManejoModelos(GuiasOET.Models.GUIAS_EMPLEADO empleado, GuiasOET.Models.GUIAS_TELEFONO telefono)
        {

            modeloEmpleado = empleado;
        }

        public ManejoModelos(GuiasOET.Models.GUIAS_EMPLEADO empleado, GuiasOET.Models.GUIAS_TELEFONO telefono, GuiasOET.Models.GUIAS_TELEFONO telefono2, GuiasOET.Models.GUIAS_TELEFONO telefono3, GuiasOET.Models.GUIAS_TELEFONO telefono4)
        {

            modeloEmpleado = empleado;
        }

        public ManejoModelos()
        {

        }

    }
}