using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace vm80q.Models
{
    public class Pergunta
    {
        [Key]
        public int Id_perg { get; set; }
        [Required]
        public string Pergunta_s { get; set; }
        [Required]
        public string Resposta_1 { get; set; }
        [Required]
        public string Resposta_2 { get; set; }
        [Required]
        public string Resposta_3 { get; set; }
        [Required]
        public string Resposta_4 { get; set; }
        [Required]
        public string Resposta_C { get; set; }

        public string Url_media { get; set; }
        [Required]
        public string Url_content { get; set; }
        [Required]
        [Range(1, 4, ErrorMessage = "Apenas 4 niveis são permitidos!")]
        public int Nivel { get; set; }
        [Required]
        public int Id_pais { get; set; }

        public virtual Pais Pais { get; set; }
    }
}