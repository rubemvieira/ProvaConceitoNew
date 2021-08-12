using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProvaConceito.FrontEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace ProvaConceito.FrontEnd.Controllers
{
    public class EscolaController : Controller
    {
        private IConfiguration _configuration;
        public EscolaController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            IEnumerable<EscolaViewModel> escolas = null;

            using(var client = new HttpClient ())
            {
                client.BaseAddress = new Uri(_configuration.GetConnectionString("BaseURI"));
                var responseTask = client.GetAsync("api/escola");

                if(responseTask.Result.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Result.Content.ReadAsAsync<IList<EscolaViewModel>>();
                    readTask.Wait();
                    escolas = readTask.Result;
                } else
                {
                    escolas = Enumerable.Empty<EscolaViewModel>();
                }
            }
            return View(escolas);
        }

        [HttpGet]
        public ActionResult create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult create(EscolaViewModel escola)
        {
            if(escola == null)
            {
                return new BadRequestResult();
            }
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration.GetConnectionString("BaseURI"));
                var postTask = client.PostAsJsonAsync<EscolaViewModel>("api/escola", escola);
                postTask.Wait();
                var result = postTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError(string.Empty, "Erro no Servidor. Contacte o Administrador.");

            return View(escola);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            EscolaViewModel escola = null;
            
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration.GetConnectionString("BaseURI"));

                //HTTP GET
                var responseTask = client.GetAsync("api/escola/" + id.ToString());
                responseTask.Wait();
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<EscolaViewModel>();
                    readTask.Wait();
                    escola = readTask.Result;
                }
            }
            return View(escola);
        }

        [HttpPost]
        public ActionResult Edit(EscolaViewModel escola)
        {
            if (escola == null)
            {
                return new BadRequestResult();
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration.GetConnectionString("BaseURI"));
                //HTTP PUT
                var putTask = client.PutAsJsonAsync<EscolaViewModel>("api/escola", escola);
                putTask.Wait();
                var result = putTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(escola);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            EscolaViewModel escola = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration.GetConnectionString("BaseURI"));
                //HTTP DELETE
                var deleteTask = client.DeleteAsync("api/escola/" + id.ToString());
                deleteTask.Wait();
                var result = deleteTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(escola);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }

            EscolaViewModel escola = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration.GetConnectionString("BaseURI"));

                //HTTP GET
                var responseTask = client.GetAsync("api/escola/" + id.ToString());
                responseTask.Wait();
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<EscolaViewModel>();
                    readTask.Wait();
                    escola = readTask.Result;
                }
            }

            return View(escola);
        }

    }
}
