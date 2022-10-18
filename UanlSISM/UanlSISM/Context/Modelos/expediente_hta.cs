namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class expediente_hta
    {
        [Key]
        [StringLength(8)]
        public string numemp { get; set; }

        [StringLength(5)]
        public string medico { get; set; }

        [StringLength(2)]
        public string clave { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? fecha { get; set; }

        [StringLength(1)]
        public string sexo { get; set; }

        [StringLength(3)]
        public string edad { get; set; }

        [StringLength(1)]
        public string antfam { get; set; }

        [StringLength(1)]
        public string tabaquismo { get; set; }

        [StringLength(3)]
        public string tasistolica { get; set; }

        [StringLength(3)]
        public string tadiastolica { get; set; }

        [StringLength(3)]
        public string indicecadera { get; set; }

        [StringLength(1)]
        public string pulsos { get; set; }

        [StringLength(1)]
        public string acv { get; set; }

        [StringLength(1)]
        public string infarto { get; set; }

        [StringLength(1)]
        public string nefrop { get; set; }

        [StringLength(1)]
        public string ateros { get; set; }

        [StringLength(6)]
        public string pesokg { get; set; }

        [StringLength(4)]
        public string tallamts { get; set; }

        [StringLength(10)]
        public string imc { get; set; }

        [StringLength(5)]
        public string elec_na { get; set; }

        [StringLength(5)]
        public string elec_cl { get; set; }

        [StringLength(5)]
        public string elec_pt { get; set; }

        [StringLength(4)]
        public string glucosaayuno { get; set; }

        [StringLength(5)]
        public string creatinina { get; set; }

        [StringLength(5)]
        public string nitr_urea { get; set; }

        [StringLength(5)]
        public string ac_urico { get; set; }

        [StringLength(5)]
        public string mircroal { get; set; }

        [StringLength(4)]
        public string colt { get; set; }

        [StringLength(4)]
        public string trig { get; set; }

        [StringLength(8)]
        public string rel_albcr { get; set; }

        [StringLength(4)]
        public string chdl { get; set; }

        [StringLength(4)]
        public string cldl { get; set; }

        [StringLength(1)]
        public string dieta { get; set; }

        [StringLength(1)]
        public string ejercicio { get; set; }

        [StringLength(2)]
        public string tratmed1 { get; set; }

        [StringLength(2)]
        public string tratmed2 { get; set; }

        [StringLength(5)]
        public string dental { get; set; }

        [StringLength(1)]
        public string dbm { get; set; }

        [StringLength(1)]
        public string obes { get; set; }

        [StringLength(1)]
        public string cardiopizq { get; set; }

        [StringLength(1)]
        public string hiperlip { get; set; }

        [StringLength(1)]
        public string ecg { get; set; }

        [Column(TypeName = "ntext")]
        public string nota { get; set; }
    }
}
