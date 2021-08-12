using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using ProvaConceito.FrontEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace ProvaConceito.FrontEnd.Controllers
{
    public class TurmaController : Controller
    {
        private IConfiguration _configuration;
        public TurmaController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            IEnumerable<TurmaViewModel> turmas = null;

            using(var client = new HttpClient ())
            {
                client.BaseAddress = new Uri(_configuration.GetConnectionString("BaseURI"));
                var responseTask = client.GetAsync("api/turma");

                if(responseTask.Result.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Result.Content.ReadAsAsync<IList<TurmaViewModel>>();
                    readTask.Wait();
                    turmas = readTask.Result;
                } else
                {
                    turmas = Enumerable.Empty<TurmaViewModel>();
                }
            }
            return View(turmas);
        }

        private List<SelectListItem> GetEscolas()
        {
            IEnumerable<EscolaViewModel> escolas = new List<EscolaViewModel>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration.GetConnectionString("BaseURI"));
                var responseTask = client.GetAsync("api/escola");

                if (responseTask.Result.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Result.Content.ReadAsAsync<IList<EscolaViewModel>>();
                    readTask.Wait();
                    escolas = readTask.Result;
                }
                else
                {
                    escolas = Enumerable.Empty<EscolaViewModel>();
                }
            }

            return escolas.Select(c => new SelectListItem()
            { Text = c.Nome, Value = c.EscolaId.ToString() }).ToList();
        }

        [HttpGet]
        public ActionResult create()
        {
            ViewBag.Escolas = GetEscolas();
            return View();
        }

        [HttpPost]
        public ActionResult create(TurmaViewModel turma)
        {
            if(turma == null)
            {
                return new BadRequestResult();
            }
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration.GetConnectionString("BaseURI"));
                var postTask = client.PostAsJsonAsync<TurmaViewModel>("api/turma", turma);
                postTask.Wait();
                var result = postTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError(string.Empty, "Erro no Servidor. Contacte o Administrador.");

            return View(turma);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            TurmaViewModel turma = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration.GetConnectionString("BaseURI"));

                //HTTP GET
                var responseTask = client.GetAsync("api/turma/" + id.ToString());
                responseTask.Wait();
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<TurmaViewModel>();
                    readTask.Wait();
                    turma = readTask.Result;
                }
            }
            ViewBag.Escolas = GetEscolas();
            return View(turma);
        }

        [HttpPost]
        public ActionResult Edit(TurmaViewModel turma)
        {
            if (turma == null)
            {
                return new BadRequestResult();
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration.GetConnectionString("BaseURI"));
                //HTTP PUT
                var putTask = client.PutAsJsonAsync<TurmaViewModel>("api/turma", turma);
                putTask.Wait();
                var result = putTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(turma);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            TurmaViewModel turma = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration.GetConnectionString("BaseURI"));
                //HTTP DELETE
                var deleteTask = client.DeleteAsync("api/turma/" + id.ToString());
                deleteTask.Wait();
                var result = deleteTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(turma);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }

            TurmaViewModel turma = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration.GetConnectionString("BaseURI"));

                //HTTP GET
                var responseTask = client.GetAsync("api/turma/" + id.ToString());
                responseTask.Wait();
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<TurmaViewModel>();
                    readTask.Wait();
                    turma = readTask.Result;
                }
            }

            return View(turma);
        }

    }
}
