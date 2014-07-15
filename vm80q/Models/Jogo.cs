using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace vm80q.Models
{
    public class Jogo
    {
        public Utilizador Jogador { get; set; }
        public int Vidas { get; set; }
        public int Pontuacao { get; set; }
        public int Dificuldade {get; set;}
        public DateTime Tempo { get; set; }
        public DateTime TempoStart { get; set; }
        public List<Pais> Paisesvisitados { get; set; }
        public List<Area> Areasvisitadas { get; set; }

        public Jogo(Utilizador jogador)
        {
            this.Jogador = jogador;
            this.Vidas = 3;
            this.Pontuacao = 0;
            this.Dificuldade=3;
            this.Paisesvisitados = new List<Pais>();
            this.Areasvisitadas = new List<Area>();
        }

        public void acertarPergunta()
        {
            switch (this.Dificuldade)
            {
                case 1:
                    this.Pontuacao = this.Pontuacao + 1;
                    break;
                case 2:
                    this.Pontuacao = this.Pontuacao + 2;
                    break;
                case 3:
                    this.Pontuacao = this.Pontuacao + 3;
                    break;
                case 4:
                    this.Pontuacao = this.Pontuacao + 5;
                    break;
            }
        
            if (this.Vidas < 3)
                this.Vidas++;
            if (this.Dificuldade < 4)
                this.Dificuldade++;
             
        }

        public void errarPergunta()
        {
            
            if (this.Vidas > 0)
                this.Vidas--;
            if (this.Dificuldade > 1)
                this.Dificuldade--;
        }

    }

}