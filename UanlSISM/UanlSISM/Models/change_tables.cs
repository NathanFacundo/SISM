//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UanlSISM.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class change_tables
    {
        public int object_id { get; set; }
        public Nullable<int> version { get; set; }
        public Nullable<int> source_object_id { get; set; }
        public string capture_instance { get; set; }
        public byte[] start_lsn { get; set; }
        public byte[] end_lsn { get; set; }
        public Nullable<bool> supports_net_changes { get; set; }
        public Nullable<bool> has_drop_pending { get; set; }
        public string role_name { get; set; }
        public string index_name { get; set; }
        public string filegroup_name { get; set; }
        public Nullable<System.DateTime> create_date { get; set; }
        public bool partition_switch { get; set; }
    }
}
