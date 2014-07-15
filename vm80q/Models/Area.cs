using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace vm80q.Models
{
    public class Area
    {
        [Key]
        public int Id_area { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        [Range(-85, 85, ErrorMessage = "Fora dos limites do mapa!")]
        public int Coord_x1 { get; set; }
        [Required]
        [Range(-85, 85, ErrorMessage = "Fora dos limites do mapa!")]
        public int Coord_x2 { get; set; }
        [Required]
        [Range(-180, 180, ErrorMessage = "Fora dos limites do mapa!")]
        public int Coord_y1 { get; set; }
        [Required]
        [Range(-180, 180, ErrorMessage = "Fora dos limites do mapa!")]
        public int Coord_y2 { get; set; }


        public virtual ICollection<Pais> Paises { get; set; }

    }
}