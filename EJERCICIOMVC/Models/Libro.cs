//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EJERCICIOMVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Web.Mvc;
    using System.Web.UI.WebControls;

    public partial class Libro
    {
        public int id { get; set; }
        [DisplayName("CODIGO DEL LIBRO")]
        public string LI_Codigo { get; set; }

        [DisplayName("NOMBRE DEL LIBRO")]
        public string LI_Nombre { get; set; }

        [DisplayName("CODIGO DEL AUTOR")]
        public string LI_Autor { get; set; }
        [DisplayName("ESTADO")]
        public int LI_Estado { get; set; }
        public System.DateTime LI_FechaCreacion { get; set; }
        public System.DateTime LI_RE_FechaModificacion { get; set; }
        public string LI_RE_UsuarioCreacion { get; set; }
        public string LI_RE_UsuarioModificacion { get; set; }
    }
}
