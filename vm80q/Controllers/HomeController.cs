using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using vm80q.DAL;
using vm80q.Models;

namespace vm80q.Controllers
{


    public class HomeController : Controller
    {

        public TabuleiroDAL tabuleiro = new TabuleiroDAL();

        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Models.Utilizador model)
        {

            if (ModelState.IsValid)
            {
                
                Utilizador user = isValid(model.Email, model.Password);
                if (user != null)
                {
                    if (user.Bloqueado != 0)
                    {
                        ModelState.AddModelError("", "Conta Bloqueada");
                        return View(model);
                    }
                        if (user.Tipo == 0)
                        {
                            FormsAuthentication.SetAuthCookie(model.Email, false);
                            return RedirectToAction("Index", "Main");
                        }
                        else
                        {
                            FormsAuthentication.SetAuthCookie(model.Email, false);
                            return RedirectToAction("Index", "Admin");
                        }
                }
                else
                    ModelState.AddModelError("", "Os dados introduzidos estão incorrectos");

            }
            else ModelState.AddModelError("", "Os dados introduzidos estão incorrectos");
            return View(model);
        }

        public Utilizador isValid(string email, string password)
        {

            Utilizador user = tabuleiro.Utilizadores.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                if (user.Password == password)
                {
                    return user;
                }
            }
            return null;
        }

        [HttpGet]
        public ActionResult Registar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registar(Models.Utilizador model)
        {
            if (ModelState.IsValid)
            {
                if (tabuleiro.Utilizadores.FirstOrDefault(us => us.Email == model.Email) == null)
                {
                    Utilizador user = new Utilizador();
                    user.Username = model.Username;
                    user.Email = model.Email;
                    user.Password = model.Password;
                    tabuleiro.Utilizadores.Add(user);
                    tabuleiro.SaveChanges();
                    return RedirectToAction("Index", "Main");
                }
                else{
                    ModelState.AddModelError("", "Já existe um utilizador registado com o email fornecido.");
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("", "Dados incorrectos");
                return View(model);
            }


        }

    }
}