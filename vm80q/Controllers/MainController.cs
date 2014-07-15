using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Mvc;
using System.Web.Security;
using vm80q.DAL;
using vm80q.Models;
using System.Data.Entity;

namespace vm80q.Controllers
{
    public class MainController : Controller
    {

        public TabuleiroDAL tabuleiro = new TabuleiroDAL();
       
        public static Jogo jogo { get; set; }

        public static Jogo getJogo()
        {
            return MainController.jogo;
        }

        public static void setJogo(Jogo jogo)
        {
            MainController.jogo = jogo;
        }



        //GET: /Main/Index
        public ActionResult Index()
        {
            if (Request.IsAuthenticated){
                if (MainController.getJogo() == null)
                {
                    var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                    var ticket = FormsAuthentication.Decrypt(cookie.Value);
                    string email = ticket.Name;
                    Utilizador user = tabuleiro.Utilizadores.FirstOrDefault(u => u.Email == email);
                    MainController.setJogo(new Jogo(user));
                }

                ViewBag.Jogador = MainController.getJogo().Jogador;
                return View();
            }
            else
                return RedirectToAction("Login","Home");
        }

        public ActionResult Jogo()
        {
            
            if(MainController.getJogo().Paisesvisitados.Count == 0)
            {
                MainController.getJogo().Jogador.Jogos_totais++;
                tabuleiro.Entry(MainController.getJogo().Jogador).State = EntityState.Modified;
                tabuleiro.SaveChanges();
            }
            
            Boolean flag = false;
            ViewBag.Jogo = MainController.getJogo();
            var areasporvisitar = tabuleiro.Areas.ToList();
            foreach (var areavisitada in MainController.getJogo().Areasvisitadas)
            {
                foreach (var area in areasporvisitar)
                {
                    if (area.Id_area.Equals(areavisitada.Id_area))
                    {
                        areasporvisitar.Remove(area);
                        break;
                    }
                }
            }
            foreach (var paisvisitado in MainController.getJogo().Paisesvisitados)
            {
                foreach (var areaux in areasporvisitar)
                {

                    foreach (var paisaux in areaux.Paises)
                    {
                        if(paisaux.Id_pais.Equals(paisvisitado.Id_pais)) {
                            areaux.Paises.Remove(paisaux);
                            flag = true;
                            break;
                        }

                    }
                    if (flag == true)
                        break;
                }

            }


            return View(areasporvisitar);
        }


        [HttpGet]
        public ActionResult Pergunta(int id_pais)
        {
            ViewBag.Jogo = MainController.getJogo();
            int dif = MainController.getJogo().Dificuldade;
            var pergunta = tabuleiro.Perguntas.FirstOrDefault(p => p.Id_pais == id_pais && p.Nivel == dif);
            if (pergunta.Url_media == null){
                return RedirectToAction("PgString","Main",new {pergunta_id = pergunta.Id_perg});
                }
            if (pergunta.Url_media.Contains("youtube"))
                return RedirectToAction("PgVideo", "Main", new { pergunta_id = pergunta.Id_perg });
            else
                return RedirectToAction("PgImage", "Main", new { pergunta_id = pergunta.Id_perg });
        }

        [HttpGet]
        public ActionResult PgString(int pergunta_id)
        {
            MainController.getJogo().TempoStart = DateTime.Now;
            var pergunta = tabuleiro.Perguntas.FirstOrDefault(p => p.Id_perg == pergunta_id);
            ViewBag.Jogo = MainController.getJogo();
            ViewBag.Pergunta = pergunta;
            
            return View();
        }

        public ActionResult PgImage(int pergunta_id)
        {
            var pergunta = tabuleiro.Perguntas.FirstOrDefault(p => p.Id_perg == pergunta_id);
            ViewBag.Pergunta = pergunta;
            ViewBag.Pergunta.Url_media = pergunta.Url_media+".jpg";
            ViewBag.Jogo = MainController.getJogo();
            return View();
        }

        public ActionResult PgVideo(int pergunta_id)
        {
            var pergunta = tabuleiro.Perguntas.FirstOrDefault(p => p.Id_perg == pergunta_id);
            ViewBag.Pergunta = pergunta;
            ViewBag.Jogo = MainController.getJogo();
            return View();
        }



        public ActionResult ValidarPergunta(string resposta_X, int pergunta_id)
        {
          
            Boolean op;
            var pergunta = tabuleiro.Perguntas.FirstOrDefault(p => p.Id_perg == pergunta_id);
            if (resposta_X.Equals(pergunta.Resposta_C))
                op = true;
            else
                op = false;

            if (op == true) 
            {
                MainController.getJogo().Areasvisitadas.Add(pergunta.Pais.Area);
                MainController.getJogo().Paisesvisitados.Add(pergunta.Pais);
                List<Pais> aux = MainController.getJogo().Paisesvisitados;
                MainController.getJogo().acertarPergunta();
                if (ganharJogo())
                    return RedirectToAction("Ganhar","Main");
                return RedirectToAction("Acertar", "Main", new { link = pergunta.Url_content });

            }
            else
            {
                MainController.getJogo().errarPergunta();
                MainController.getJogo().Paisesvisitados.Add(pergunta.Pais);
                
                
                if(terminarJogo() || terminarJogo(MainController.getJogo().Paisesvisitados,pergunta.Pais.Id_area))
                    return RedirectToAction("Perder","Main");



                return RedirectToAction("Errar", "Main", new { link = pergunta.Url_content });
            }
        }

        public Boolean terminarJogo(List<Pais> paisvisitados, int id_area)
        {
            Boolean flag = false;
            var paisesaux = paisvisitados.FindAll(pais => pais.Id_area == id_area);
            if (paisesaux.Count >= 3)
                flag = true;
            return flag;
        }

        public Boolean terminarJogo()
        {
            Boolean flag = false;
            if (MainController.getJogo().Vidas == 0)
                flag = true;
            return flag;
        }

        public Boolean ganharJogo()
        {
            if (MainController.getJogo().Areasvisitadas.Count == 15)
                return true;
            else
                return false;
        }

        public ActionResult Acertar(String link)
        {
            ViewBag.Jogo = MainController.getJogo();
            ViewBag.Link = link;
            return View();
        }

        public ActionResult Errar(String link)
        {
            ViewBag.Jogo = MainController.getJogo();
            ViewBag.Link = link;
            return View();
        }

        public ActionResult Pontuacoes()
        {
            ViewBag.Jogador = MainController.getJogo().Jogador;
            ViewBag.JP = tabuleiro.Utilizadores.OrderByDescending(jo => jo.Max_pontuacao).Take(7);
            ViewBag.JT = tabuleiro.Utilizadores.OrderByDescending(jt => jt.Jogos_terminados).Take(7);
            ViewBag.JTT = tabuleiro.Utilizadores.OrderByDescending(jtt => jtt.Jogos_totais).Take(7);
            return View(tabuleiro.Utilizadores.ToList());
        }

        public ActionResult Logout() 
        { 
            FormsAuthentication.SignOut();
            MainController.setJogo(null);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Ganhar()
        {
            if (MainController.jogo.Areasvisitadas.Count == tabuleiro.Areas.Count())
            {
                ViewBag.Jogo = MainController.jogo;
                Utilizador user = MainController.jogo.Jogador;
                if(MainController.jogo.Pontuacao > user.Max_pontuacao)
                {
                    user.Max_pontuacao = MainController.jogo.Pontuacao;
                    tabuleiro.Utilizadores.Attach(user);
                    var entry = tabuleiro.Entry(user);
                    entry.Property(us => us.Max_pontuacao).IsModified = true;
                    tabuleiro.SaveChanges();

                }
                user.Jogos_terminados++;
                tabuleiro.Utilizadores.Attach(user);
                var entry2 = tabuleiro.Entry(user);
                entry2.Property(us => us.Jogos_terminados).IsModified = true;
                tabuleiro.SaveChanges();
                
                MainController.setJogo(new Jogo(user));
                return View();
            }
            else
                return RedirectToAction("Jogo", "Main");


        }

        public ActionResult Perder()
        {
            ViewBag.Jogo = MainController.jogo;
            Utilizador user = MainController.jogo.Jogador;
            MainController.setJogo(new Jogo(user));
            return View();
        }
        

	}
}