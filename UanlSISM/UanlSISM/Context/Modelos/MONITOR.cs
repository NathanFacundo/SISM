namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MONITOR")]
    public partial class MONITOR
    {
        [Key]
        public int RowNumber { get; set; }

        public int? EventClass { get; set; }

        [Column(TypeName = "ntext")]
        public string TextData { get; set; }

        [StringLength(128)]
        public string NTUserName { get; set; }

        public int? ClientProcessID { get; set; }

        [StringLength(128)]
        public string ApplicationName { get; set; }

        [StringLength(128)]
        public string LoginName { get; set; }

        public int? SPID { get; set; }

        public long? Duration { get; set; }

        public DateTime? StartTime { get; set; }

        public long? Reads { get; set; }

        public long? Writes { get; set; }

        public int? CPU { get; set; }
    }
}
