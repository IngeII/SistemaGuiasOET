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
    
    public partial class GUIAS_ESTACION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GUIAS_ESTACION()
        {
            this.GUIAS_EMPLEADO = new HashSet<GUIAS_EMPLEADO>();
        }
    
        public string NOMBREESTACION { get; set; }
        public string UBICACION { get; set; }
        public string NOMBRECOMPANIA { get; set; }
    
        public virtual GUIAS_COMPANIA GUIAS_COMPANIA { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GUIAS_EMPLEADO> GUIAS_EMPLEADO { get; set; }
    }
}