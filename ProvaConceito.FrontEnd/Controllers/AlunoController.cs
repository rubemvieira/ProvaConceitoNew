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
    public class AlunoController : Controller
    {
        private IConfiguration _configuration;
        public AlunoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            IEnumerable<AlunoViewModel> alunos = null;

            using(var client = new HttpClient ())
            {
                client.BaseAddress = new Uri(_configuration.GetConnectionString("BaseURI"));
                var responseTask = client.GetAsync("api/aluno");

                if(responseTask.Result.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Result.Content.ReadAsAsync<IList<AlunoViewModel>>();
                    readTask.Wait();
                    alunos = readTask.Result;
                } else
                {
                    alunos = Enumerable.Empty<AlunoViewModel>();
                }
            }
            return View(alunos);
        }

        private List<SelectListItem> GetTurmas()
        {
            IEnumerable<TurmaViewModel> turmas = new List<TurmaViewModel>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration.GetConnectionString("BaseURI"));
                var responseTask = client.GetAsync("api/turma/withchildren");

                if (responseTask.Result.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Result.Content.ReadAsAsync<IList<TurmaViewModel>>();
                    readTask.Wait();
                    turmas = readTask.Result;
                }
                else
                {
                    turmas = Enumerable.Empty<TurmaViewModel>();
                }
            }

            return turmas.Select(c => new SelectListItem()
            { Text = c.Escola.Nome + "/" + c.Descricao, Value = c.TurmaId.ToString() }).ToList();
        }

        [HttpGet]
        public ActionResult create()
        {
            ViewBag.Turmas = GetTurmas();
            return View();
        }

        [HttpPost]
        public ActionResult create(AlunoViewModel aluno)
        {
            if(aluno == null)
            {
                return new BadRequestResult();
            }
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration.GetConnectionString("BaseURI"));
                var postTask = client.PostAsJsonAsync<AlunoViewModel>("api/aluno", aluno);
                postTask.Wait();
                var result = postTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError(string.Empty, "Erro no Servidor. Contacte o Administrador.");

            return View(aluno);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            AlunoViewModel aluno = null;
            
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration.GetConnectionString("BaseURI"));

                //HTTP GET
                var responseTask = client.GetAsync("api/aluno/" + id.ToString());
                responseTask.Wait();
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<AlunoViewModel>();
                    readTask.Wait();
                    aluno = readTask.Result;
                }
            }
            ViewBag.Turmas = GetTurmas();
            return View(aluno);
        }

        [HttpPost]
        public ActionResult Edit(AlunoViewModel aluno)
        {
            if (aluno == null)
            {
                return new BadRequestResult();
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration.GetConnectionString("BaseURI"));
                //HTTP PUT
                var putTask = client.PutAsJsonAsync<AlunoViewModel>("api/aluno", aluno);
                putTask.Wait();
                var result = putTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(aluno);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            AlunoViewModel aluno = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration.GetConnectionString("BaseURI"));
                //HTTP DELETE
                var deleteTask = client.DeleteAsync("api/aluno/" + id.ToString());
                deleteTask.Wait();
                var result = deleteTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(aluno);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }

            AlunoViewModel aluno = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration.GetConnectionString("BaseURI"));

                //HTTP GET
                var responseTask = client.GetAsync("api/aluno/" + id.ToString());
                responseTask.Wait();
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<AlunoViewModel>();
                    readTask.Wait();
                    aluno = readTask.Result;
                }
            }

            return View(aluno);
        }

    }
}
