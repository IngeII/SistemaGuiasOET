﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuiasOET.Models
{
    public class AsignacionModelos
    {
        public GuiasOET.Models.GUIAS_RESERVACION modeloReservacion { get; set; }
        public List<GuiasOET.Models.GUIAS_EMPLEADO> guiasDisponibles = new List<GuiasOET.Models.GUIAS_EMPLEADO>();
        public List<GuiasOET.Models.GUIAS_EMPLEADO> guiasAsignados = new List<GuiasOET.Models.GUIAS_EMPLEADO>();
        public GuiasOET.Models.GUIAS_ASIGNACION asignacionGuias { get; set; }

        public AsignacionModelos()
        {
        }

        public AsignacionModelos(GuiasOET.Models.GUIAS_RESERVACION reservacion)
        {
            modeloReservacion = reservacion;
        }
    }
}