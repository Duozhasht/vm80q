using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace vm80q.Models
{
    public class Utilizador
    {
        [Key]
        public int Id_util { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Jogos_totais { get; set; }
        public int Jogos_terminados { get; set; }
        public int Max_pontuacao { get; set; }
        public int Min_tempo { get; set; }
        public int Tipo { get; set; }
        public int Bloqueado { get; set; }
    }
}