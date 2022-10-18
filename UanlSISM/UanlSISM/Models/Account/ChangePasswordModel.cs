using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UanlSISM.Models.Account
{
    public class ChangePasswordModel
    {
        /*[Display(Name = "Contraseña anterior:")]
        [Required(ErrorMessage = "Contraseña anterior es obligatoria.")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }*/

        [Display(Name = "Contraseña nueva:")]
        [Required(ErrorMessage = "Contraseña nueva es obligatoria.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name = "Confirmar contraseña nueva:")]
        [Required(ErrorMessage = "Confirmar contraseña nueva es obligatoria.")]
        [Compare(otherProperty:"NewPassword", ErrorMessage = 
            "La nueva contraseña no coinciden.")]
        [DataType(DataType.Password)]
        public string ConfirmNewPassword { get; set; }
    }
}