//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GuiasOET.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class GUIAS_RESERVACION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GUIAS_RESERVACION()
        {
            this.GUIAS_ASIGNACION = new HashSet<GUIAS_ASIGNACION>();
        }
    
        public string NUMERORESERVACION { get; set; }
        public string NOMBRERESERVACION { get; set; }
        public Nullable<decimal> NUMEROPERSONAS { get; set; }
        public string HORAMODIFICACION { get; set; }
        public Nullable<System.DateTime> FECHAMODIFICACION { get; set; }
        public string TURNO { get; set; }
        public Nullable<System.DateTime> FECHA { get; set; }
        public string HORA { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GUIAS_ASIGNACION> GUIAS_ASIGNACION { get; set; }
    }
}
