using Loteria.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Loteria.Web.Controllers
{
    public class AjaxResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
    public class HomeController : Controller
    {
        public static LoteriaModels Loteria { get; set; }


        public string ObterURLAplicacao()
        {
            var strUrl = HttpContext.Request.Url.GetLeftPart(UriPartial.Authority) + HttpContext.Request.ApplicationPath;
            if (strUrl.EndsWith("/"))
                strUrl = strUrl.Substring(0, strUrl.Length - 1);
            return strUrl;
        }
        public ActionResult Index()
        {
            if (Loteria == null)
            {
                Loteria = new LoteriaModels();
                Loteria.JogosCadastrados = new List<JogoModels>();
                var megaSena = new JogoModels();
                megaSena.Id = 1;
                megaSena.Nome = "Mega Sena";
                megaSena.NumeroInicial = 1;
                megaSena.QuantidadeNumeros = 60;
                megaSena.QtdeMinNumeroMarcadoVolante = 6;
                megaSena.QtdeMaxNumeroMarcadoVolante = 15;
                megaSena.QtdeNumeroSorteados = 6;
                Loteria.JogosCadastrados.Add(megaSena);
            }
            return View(Loteria);
        }

        public ActionResult Apostas(int pId = -1)
        {
            try
            {                
                if (pId != -1)
                {
                    var jogo = Loteria.JogosCadastrados.Where(p => p.Id == pId).FirstOrDefault();
                    ViewBag.Resultado = null;
                    return View(jogo);
                } else
                    return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Cadastro(int pId = 0)
        {
            JogoModels jogo = new JogoModels();
            if (pId > 0)
            {
                jogo = Loteria.JogosCadastrados.Where(p => p.Id == pId).FirstOrDefault();
            }
            return View(jogo);
        }

        public ActionResult CadastrarJogo( JogoModels pModel)
        {
            try
            {
                pModel.Id = Loteria.JogosCadastrados.Count() + 1;
                Loteria.JogosCadastrados.Add(pModel);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }

        }


        [HttpPost]
        public ActionResult InserirAposta(int pId, string pNumeros)
        {
            try
            {
                var jogo = Loteria.JogosCadastrados.Where(p => p.Id == pId).FirstOrDefault();
                if (jogo.ApostasFeitas == null)
                    jogo.ApostasFeitas = new List<ApostaModels>();
                ApostaModels apostas = new ApostaModels();
                apostas.DataCadastro = DateTime.Now;
                apostas.Id = jogo.ApostasFeitas.Count() + 1;
                apostas.NumerosSelecionados = new List<int>();
                var numeros = pNumeros.Split(';').ToList();
                for (int i = 0; i < numeros.Count; i++)
                {
                    apostas.NumerosSelecionados.Add(int.Parse(numeros[i]));
                }
                jogo.ApostasFeitas.Add(apostas);

                return PartialView("Volantes", jogo);
            }
            catch (Exception)
            {                
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Não foi possível inserir uma nova aposta.");
            }

        }

        [HttpPost]
        public ActionResult LimparApostas(int pId)
        {
            try
            {
                var jogo = Loteria.JogosCadastrados.Where(p => p.Id == pId).FirstOrDefault();
                if (jogo.ApostasFeitas != null)
                    jogo.ApostasFeitas = null;

                return PartialView("Volantes", jogo);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Não foi possível inserir uma nova aposta.");
            }

        }


        [HttpPost]
        public ActionResult Conferir(int pId, string pNumeros)
        {
            try
            {
                var jogo = Loteria.JogosCadastrados.Where(p => p.Id == pId).FirstOrDefault();
                var resultado = new ResultadoModels();
                resultado.NumerosSorteados = new List<int>();
                var numeros = pNumeros.Split(';').ToList();
                for (int i = 0; i < numeros.Count; i++)
                {
                  resultado.NumerosSorteados.Add(int.Parse(numeros[i]));
                }
                ViewBag.Resultado = resultado;
                return PartialView("Volantes", jogo);
            }
            catch (Exception)
            {                
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Não foi possível conferir o resultado.");
            }

        }
    }
}