using MyGym.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGym.Data.Entities
{
    [Table("CONSUMO")]
    public class Consumo
    {
        [Key, Column(Order = 1)]
        public int RecomendacionID { get; set; }
        [Key, Column(Order = 2)]
        public int UsuarioID { get; set; }
        [Key, Column(Order = 3)]
        public TiempoComida TiempoComida { get; set; }

        public DateTime Fecha { get; set; }

        [ForeignKey("RecomendacionID")]
        public virtual Recomendacion Recomendacion { get; set; } 
        [ForeignKey("UsuarioID")]
        public virtual Usuario Usuario { get; set; }

    }
}
