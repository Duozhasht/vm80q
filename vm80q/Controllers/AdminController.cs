using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using vm80q.DAL;
using vm80q.Models;


namespace vm80q.Controllers
{
    public class AdminController : Controller
    {
        
        TabuleiroDAL tabuleiro = new TabuleiroDAL();


        // GET: /Admin/
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                var ticket = FormsAuthentication.Decrypt(cookie.Value);
                string email = ticket.Name;
                Utilizador user = tabuleiro.Utilizadores.FirstOrDefault(u => u.Email == email);
                if (user.Tipo==1)
                    return View();
                else
                    return RedirectToAction("Index", "Main");
            }
            else
                return RedirectToAction("Index","Home");
        }

        [HttpPost]
        public ActionResult Index(Models.Utilizador model)
        {
            return View();
        }

        [HttpGet]
        public ActionResult Area()
        {
            return View(tabuleiro.Areas.ToList());
        }


        [HttpGet]
        public ActionResult AdicionarArea()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdicionarArea(Area area)
        {
            if (ModelState.IsValid)
            {
                Area aux = new Area();
                aux.Nome = area.Nome;
                aux.Coord_x1 = area.Coord_x1;
                aux.Coord_x2 = area.Coord_x2;
                aux.Coord_y1 = area.Coord_y1;
                aux.Coord_y2 = area.Coord_y2;

                tabuleiro.Areas.Add(aux);
                tabuleiro.SaveChanges();
                return RedirectToAction("Area", "Admin");
            }

            else
                return View(area);
            
        }

        [HttpGet]
        public ActionResult RemoverArea(int id_area)
        {
            if (ModelState.IsValid)
            {

                Area area = tabuleiro.Areas.FirstOrDefault(ar => ar.Id_area == id_area);
                if (area.Paises.Count == 0)
                {
                    tabuleiro.Areas.Remove(area);
                    tabuleiro.SaveChanges();
                    return RedirectToAction("Area", "Admin");
                }
                else
                {
                    return View(tabuleiro.Areas.ToList());
                }

                
            }
            else return RedirectToAction("Area","Admin");
        }

        public ActionResult Pais()
        {
            return View(tabuleiro.Paises.ToList());
        }

        [HttpGet]
        public ActionResult AdicionarPais()
        {
            return View();
        }

        public ActionResult AdicionarPais(Pais pais)
        {
            if (ModelState.IsValid)
            {

                if (tabuleiro.Areas.FirstOrDefault(ar => ar.Id_area == pais.Id_area) == null)
                {
                    ModelState.AddModelError("","Não existe uma area para o ID fornecido");
                    return View();
                }
                Pais paisaux = new Pais();
                paisaux.Id_pais = pais.Id_pais;
                paisaux.Nome = pais.Nome;
                paisaux.Coord_x1 = pais.Coord_x1;
                paisaux.Coord_y1 = pais.Coord_y1;
                paisaux.Id_area = pais.Id_area;

                tabuleiro.Paises.Add(paisaux);
                tabuleiro.SaveChanges();

                return RedirectToAction("Pais","Admin");
            }
            else return View(pais);
        }

        [HttpGet]
        public ActionResult RemoverPais(int id_pais)
        {
            if (ModelState.IsValid)
            {

                    Pais pais = tabuleiro.Paises.FirstOrDefault(pa => pa.Id_pais == id_pais);
            if (pais.Perguntas.Count == 0)
                {
                    tabuleiro.Paises.Remove(pais);
                    tabuleiro.SaveChanges();
                    return RedirectToAction("Pais", "Admin");
                }
            else
            {
                return View(tabuleiro.Paises.ToList());
            }
            }
            else return RedirectToAction("Pais", "Admin");
        }

        [HttpGet]
        public ActionResult Utilizador()
        {
            return View(tabuleiro.Utilizadores.ToList());
        }

        [HttpGet]
        public ActionResult ToggleBloquear(int id_util)
        {
            if (ModelState.IsValid)
            {
                Utilizador util = tabuleiro.Utilizadores.FirstOrDefault(ut => ut.Id_util == id_util);
                if (util.Bloqueado == 0)
                    util.Bloqueado = 1;
                else util.Bloqueado = 0;

                tabuleiro.Entry(util).State = EntityState.Modified;
                tabuleiro.SaveChanges();
            }

            return RedirectToAction("Utilizador","Admin");
        }

        [HttpGet]
        public ActionResult TogglePromover(int id_util)
        {
            if (ModelState.IsValid)
            {
                Utilizador util = tabuleiro.Utilizadores.FirstOrDefault(ut => ut.Id_util == id_util);
                if (util.Tipo == 0)
                    util.Tipo = 1;
                else util.Tipo = 0;

                tabuleiro.Entry(util).State = EntityState.Modified;
                tabuleiro.SaveChanges();
            }

            return RedirectToAction("Utilizador", "Admin");
        }

        [HttpGet]
        public ActionResult Pergunta()
        {
            return View(tabuleiro.Perguntas.ToList());
        }



        [HttpGet]
        public ActionResult RemoverPergunta(int id_pergunta)
        {
            if (ModelState.IsValid)
            {
                Pergunta pergunta = tabuleiro.Perguntas.FirstOrDefault(pr => pr.Id_perg == id_pergunta);
                tabuleiro.Perguntas.Remove(pergunta);
                tabuleiro.SaveChanges();
                return RedirectToAction("Pergunta", "Admin");
            }
            else
                return RedirectToAction("Pergunta", "Admin");
        }

        [HttpGet]
        public ActionResult AdicionarPergunta()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdicionarPergunta(Pergunta pergunta)
        {
            if (ModelState.IsValid)
            {
                if (tabuleiro.Paises.FirstOrDefault(pa => pa.Id_pais == pergunta.Id_pais) == null)
                {
                    ModelState.AddModelError("", "Não existe um pais para o ID fornecido");
                    return View(pergunta);
                }
                Pergunta perg = new Pergunta();
                perg.Id_pais = pergunta.Id_pais;
                perg.Pergunta_s = pergunta.Pergunta_s;
                perg.Resposta_1 = pergunta.Resposta_1;
                perg.Resposta_2 = pergunta.Resposta_2;
                perg.Resposta_3 = pergunta.Resposta_3;
                perg.Resposta_4 = pergunta.Resposta_4;
                perg.Resposta_C = pergunta.Resposta_C;
                perg.Url_media = pergunta.Url_media;
                perg.Url_content = pergunta.Url_content;
                perg.Nivel = pergunta.Nivel;
                perg.Id_pais = pergunta.Id_pais;

                tabuleiro.Perguntas.Add(perg);
                tabuleiro.SaveChanges();

                return RedirectToAction("Pergunta", "Admin");

            }
            else return View(pergunta);
        }








	}
}