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
    
    public partial class TipoDocumento
    {
        public int id { get; set; }
        [DisplayName("CODIGO")]
        public string TD_Codigo { get; set; }
        [DisplayName("NOMBRE")]
        public string TD_Nombre { get; set; }
        [DisplayName("ESTADO")]
        public int TD_Estado { get; set; }
        public System.DateTime TD_FechaCreacion { get; set; }
        public System.DateTime TD_FechaModificacion { get; set; }
        public string TD_UsuarioModificacion { get; set; }
        public string TD_UsuarioCreacion { get; set; }
    }
}