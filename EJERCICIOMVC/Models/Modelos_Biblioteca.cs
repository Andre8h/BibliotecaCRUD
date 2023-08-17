using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EJERCICIOMVC.Models
{
    [MetadataType(typeof(Usuario_Descripcion))]
    public partial class Usuario
    {
        public string NombreTipoDocumento { get; set; }
    }
    public partial class Usuario_Descripcion
    {
        [DisplayName("CODIGO USUARIO"), Required(ErrorMessage = "El campo Codigo es obligatorio")]
        public string US_Codigo { get; set; }
        [DisplayName("NOMBRE USUARIO"), Required(ErrorMessage = "El campo Codigo es obligatorio")]
        public string US_Nombre { get; set; }
        [DisplayName("CONTRASEÑA")]
        public string US_Password { get; set; }
        [DisplayName("ESTADO")]
        public int US_Estado { get; set; }
        [DisplayName("CORREO ELECTRONICO")]
        public string US_Email { get; set; }
        [DisplayName("TIPO DOCUMENTO")]
        public string US_Tipodedocumento { get; set; }
        [DisplayName("DOCUMENTO")]
        public string US_Documento { get; set; }
    }

    //public class CodLibro
    //{
    //    public string CodigoLibro { get; set; }

    //}

    public class LoginDTO
    {
        public string Usuario { get; set; }
        public string Password {get; set;}

    }
}