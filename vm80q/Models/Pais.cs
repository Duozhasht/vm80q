using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace vm80q.Models
{
    public class Pais
    {
        [Key]
        public int Id_pais { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        [Range(-85, 85, ErrorMessage = "Fora dos limites do mapa!")]
        public int Coord_x1 { get; set; }
        [Required]
        [Range(-180, 180, ErrorMessage = "Fora dos limites do mapa!")]
        public int Coord_y1 { get; set; }
        [Required]
        public int Id_area {get; set; }

        public virtual Area Area { get; set; }

        public virtual ICollection<Pergunta> Perguntas { get; set; }
    }
}