﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UanlSISM.Models
{
    public class SustanciaM
    {
        public int Id { get; set; }
        public short Id_Grupo { get; set; }
        public short Id_Sal { get; set; }
        public short Id_Nivel { get; set; }
        public string clave_presentacion { get; set; }
        public string Presentacion { get; set; }
        public string Clave { get; set; }
        public bool Status { get; set; }
        public string Consultorio { get; set; }
        public string estante_col { get; set; }
        public string estante_nivel { get; set; }
        public Nullable<decimal> pos_frecuencia { get; set; }
        public Nullable<decimal> consumo_semana { get; set; }
        public string cb_mederos { get; set; }
        public Nullable<float> cto_ultimo { get; set; }
        public Nullable<float> cto_promedio { get; set; }
        public Nullable<decimal> estatus_21 { get; set; }
        public string descripcion_21 { get; set; }
        public string clave_21 { get; set; }
        public Nullable<int> id_grupo_21 { get; set; }
        public Nullable<int> id_principioactivo_21 { get; set; }
        public Nullable<int> id_formafarm_21 { get; set; }
        public Nullable<int> id_concentracion_21 { get; set; }
        public Nullable<int> id_unidadeslicitacion_21 { get; set; }
        public Nullable<int> id_unidadesmedida_21 { get; set; }
        public Nullable<int> id_cantidadenvases_21 { get; set; }
        public Nullable<int> id_tipoenvase_21 { get; set; }
        public string codigo_21 { get; set; }
        public Nullable<int> Nivel_21 { get; set; }
        public string SobranteInv2022 { get; set; }
        public string UserId { get; set; }
        public string IP { get; set; }
        public Nullable<System.DateTime> FechaMed { get; set; }
        public Nullable<int> LicitacionStatus { get; set; }
        public string Compendio { get; set; }
        public string Proveedores { get; set; }

        public short CANTIDAD { get; set; }
        public short CANTIDAD_NUEVA { get; set; }
        public short ORDEN { get; set; }
    }
}