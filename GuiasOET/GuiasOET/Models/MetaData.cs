using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GuiasOET.Models
{
    public class MetaData
    {
    }

    public class EMPLEADOMetadata
    {
        [StringLength(9)]
        [Display(Name = "Cédula:")]
        public string CEDULA;

        [StringLength(20)]
        [Display(Name = "Nombre:")]
        public string NOMBREEMPLEADO;

        [StringLength(20)]
        [Display(Name = "Apellido 1:")]
        public string APELLIDO1;

        [StringLength(20)]
        [Display(Name = "Apellido 2:")]
        public string APELLIDO2;

        [StringLength(30)]
        [Display(Name = "Email:")]
        public string EMAIL;

        [Range(0, 1)]
        [Display(Name = "Estado:")]
        public decimal ESTADO;

        [StringLength(100)]
        [Display(Name = "Dirección:")]
        public string DIRECCION;

        [StringLength(20)]
        [Display(Name = "Usuario:")]
        public string USUARIO;

        [StringLength(30)]
        [Display(Name = "Contraseña:")]
        public string CONTRASENA;

        [Range(1, 5)]
        [Display(Name = "Rol:")]
        public decimal TIPOEMPLEADO;

        [Display(Name = "Estación:")]
        public string NOMBREESTACION;
    }
}