using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuiasOET.Models
{
    /*  [MetadataType(typeof(EMPLEADOMetadata))]
      public partial class GUIAS_EMPLEADO
      {
      } */

    public partial class GUIAS_EMPLEADO
    {
        public class ReestablecerContraseñaMetadata
        {
            [Required(ErrorMessage = "*Debe ingresar el nombre de usuario ")]
            [Display(Name = "Usuario:")]
            public string USUARIO { get; set; }

            [Required(ErrorMessage = "*Debe ingresar la contraseña")]
            [DataType(DataType.Password)]
            [Display(Name = "Nueva Contraseña: ")]
            public string CONTRASENA { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirmar Contraseña: ")]
            [Compare("CONTRASENA", ErrorMessage = "*La contraseña y la confirmación de la contraseña no coinciden")]
            [NotMapped]
            public string CONFIRMACIONCONTRASENA { get; set; }
        }

        public class LoginMetadata
        {
            [Required(ErrorMessage = "*Debe ingresar el nombre de usuario ")]
            [Display(Name = "Usuario:")]
            public string USUARIO { get; set; }

            [Required(ErrorMessage = "*Debe ingresar la contraseña")]
            [DataType(DataType.Password)]
            [Display(Name = "Contraseña:")]
            public string CONTRASENA { get; set; }
        } 


    }

}
