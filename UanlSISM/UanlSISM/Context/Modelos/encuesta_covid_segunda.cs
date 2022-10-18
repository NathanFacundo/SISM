namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class encuesta_covid_segunda
    {
        public int id { get; set; }

        [StringLength(255)]
        public string nombre { get; set; }

        public int? edad { get; set; }

        [StringLength(255)]
        public string expediente { get; set; }

        [Column(TypeName = "text")]
        public string antecedentes_medicos { get; set; }

        public int? fiebre { get; set; }

        public int? tos { get; set; }

        public int? anosmia { get; set; }

        public int? diarrea { get; set; }

        public int? mialgias_atralgias { get; set; }

        public int? hta_cardivascular { get; set; }

        public int? diabetes_mellitus { get; set; }

        public int? tabaquismo { get; set; }

        public int? enf_inmunologica { get; set; }

        public int? enf_reumatologia { get; set; }

        public int? enf_neoplasica { get; set; }

        public int? irc { get; set; }

        public int? edad_mayo { get; set; }

        public int? fr { get; set; }

        public int? disnea { get; set; }

        public int? saturacion_oxigeno { get; set; }

        public int? hipotension { get; set; }

        public int? alteracion_estado { get; set; }

        public int? alteracion_pulmonares { get; set; }

        public DateTime? fecha { get; set; }

        [Column(TypeName = "text")]
        public string notas { get; set; }
    }
}
