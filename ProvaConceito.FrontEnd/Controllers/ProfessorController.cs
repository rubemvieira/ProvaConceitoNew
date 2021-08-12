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
    public class ProfessorController : Controller
    {
        private IConfiguration _configuration;
        public ProfessorController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            IEnumerable<ProfessorViewModel> professors = null;

            using(var client = new HttpClient ())
            {
                client.BaseAddress = new Uri(_configuration.GetConnectionString("BaseURI"));
                var responseTask = client.GetAsync("api/professor");

                if(responseTask.Result.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Result.Content.ReadAsAsync<IList<ProfessorViewModel>>();
                    readTask.Wait();
                    professors = readTask.Result;
                } else
                {
                    professors = Enumerable.Empty<ProfessorViewModel>();
                }
            }
            return View(professors);
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
        public ActionResult create(ProfessorViewModel professor)
        {
            if(professor == null)
            {
                return new BadRequestResult();
            }

            var selectedIds = Request.Form["Turmas"];

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration.GetConnectionString("BaseURI"));
                var postTask = client.PostAsJsonAsync<ProfessorViewModel>("api/professor", professor);
                postTask.Wait();
                var result = postTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError(string.Empty, "Erro no Servidor. Contacte o Administrador.");

            return View(professor);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }

            ProfessorViewModel professor = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration.GetConnectionString("BaseURI"));

                //HTTP GET
                var responseTask = client.GetAsync("api/professor/" + id.ToString());
                responseTask.Wait();
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ProfessorViewModel>();
                    readTask.Wait();
                    professor = readTask.Result;
                }
            }

            if (professor.Turmas == null) professor.Turmas = new List<ProfessorTurmaViewModel>();
            var filtro = professor.Turmas.Select(t => t.TurmaId ).ToList();
            ViewBag.Turmas = GetTurmas().Where(t => filtro.All(f => f != Int16.Parse(t.Value))).ToList();
            return View(professor);
        }

        [HttpPost]
        public ActionResult Edit(ProfessorViewModel professor)
        {
            if (professor == null)
            {
                return new BadRequestResult();
            }

            var selectedIds = Request.Form["Turmas"];
            var preservedIds = Request.Form["item.TurmaId"];

            professor.Turmas = new List<ProfessorTurmaViewModel>();
            foreach(var turma in selectedIds)
            {
                professor.Turmas.Add(new ProfessorTurmaViewModel {  ProfessorId = professor.ProfessorId, TurmaId = Int16.Parse( turma)  });
            }

            foreach (var turma in preservedIds)
            {
                professor.Turmas.Add(new ProfessorTurmaViewModel { ProfessorId = professor.ProfessorId, TurmaId = Int16.Parse(turma) });
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration.GetConnectionString("BaseURI"));
                //HTTP PUT
                var putTask = client.PutAsJsonAsync<ProfessorViewModel>("api/professor", professor);
                putTask.Wait();
                var result = putTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(professor);
        }

        public ActionResult DeleteTurma(int? ProfessorId, int? TurmaId)
        {
            if (ProfessorId == null || TurmaId == null )
            {
                return new BadRequestResult();
            }

            ProfessorViewModel professor = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration.GetConnectionString("BaseURI"));

                //HTTP GET
                var responseTask = client.GetAsync("api/professor/" + ProfessorId.ToString());
                responseTask.Wait();
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ProfessorViewModel>();
                    readTask.Wait();
                    professor = readTask.Result;
                }
            }

            if (professor.Turmas == null) professor.Turmas = new List<ProfessorTurmaViewModel>();
            var turmaExcluir = professor.Turmas.Single(t => t.TurmaId == TurmaId);
            professor.Turmas.Remove(turmaExcluir);
            var filtro = professor.Turmas.Select(t => t.TurmaId).ToList();
            ViewBag.Turmas = GetTurmas().Where(t => filtro.All(f => f != Int16.Parse(t.Value))).ToList();
            return View("Edit",professor);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            ProfessorViewModel professor = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration.GetConnectionString("BaseURI"));
                //HTTP DELETE
                var deleteTask = client.DeleteAsync("api/professor/" + id.ToString());
                deleteTask.Wait();
                var result = deleteTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(professor);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }

            ProfessorViewModel professor = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration.GetConnectionString("BaseURI"));

                //HTTP GET
                var responseTask = client.GetAsync("api/professor/" + id.ToString());
                responseTask.Wait();
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ProfessorViewModel>();
                    readTask.Wait();
                    professor = readTask.Result;
                }
            }

            return View(professor);
        }

    }
}
