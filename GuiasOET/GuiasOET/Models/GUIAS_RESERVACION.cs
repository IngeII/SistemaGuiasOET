//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GuiasOET.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    public partial class GUIAS_RESERVACION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GUIAS_RESERVACION()
        {
            this.GUIAS_ASIGNACION = new HashSet<GUIAS_ASIGNACION>();
        }
    
        [Display(Name = "Número:")]
        public string NUMERORESERVACION { get; set; }
        [Display(Name = "Nombre:")]
        public string NOMBRERESERVACION { get; set; }
        [Display(Name = "Pack:")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:###}")]
        public Nullable<decimal> NUMEROPERSONAS { get; set; }
        public string HORAMODIFICACION { get; set; }
        public Nullable<System.DateTime> FECHAMODIFICACION { get; set; }
        [Display(Name = "Fecha:")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode =true, DataFormatString = "{0:M/d/yyyy}")]
        public Nullable<System.DateTime> FECHA { get; set; }
        [Display(Name = "Hora:")]
        public string HORA { get; set; }
        [Display(Name = "Estación")]
        public string NOMBREESTACION { get; set; }
        public Nullable<decimal> CONFIRMACION { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GUIAS_ASIGNACION> GUIAS_ASIGNACION { get; set; }
    }
}
