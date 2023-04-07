﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ModelGSB;
using ORMGSB;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace FrontGSB.Controllers
{
    public class MedecinsController : Controller
    {
        private GsbOrm db = new GsbOrm();

        // GET: Medecins
        public async Task<ActionResult> Index()
        {
            string url = "https://localhost:44333/api/Medecins";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("token", "azerty");
                HttpResponseMessage response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception();
                }

                var medecins = await response.Content.ReadAsAsync<IEnumerable<Medecin>>();

                return View(medecins);
            }
            //var medecins = db.Medecins.Include(m => m.Departement);
            //return View(await medecins.ToListAsync());
        }

        // GET: Medecins/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string url = "https://localhost:44333/api/Medecins/" + id;
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception();
                }

                var medecin = await response.Content.ReadAsAsync<Medecin>();
                return View(medecin);
            }
        }

        // GET: Medecins/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.IdDepartement = new SelectList(db.Departements, "IdDepartement", "NomDep");
            return View();
        }

        // POST: Medecins/Create
        // Pour vous protéger des attaques par survalidation, activez les propriétés spécifiques auxquelles vous souhaitez vous lier. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "IdMedecin,NomMed,PrenomMed,AdresseMed,TelephoneMed,SpecialiteComplementaire,IdDepartement")] Medecin medecin)
        {
            if (ModelState.IsValid)
            {
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(medecin);

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("token", "azerty");
                    using (var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44333/api/Medecins"))
                    {
                        request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                        // envoie des infos
                        var send = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

                        if (!send.IsSuccessStatusCode)
                            throw new Exception();

                        send.EnsureSuccessStatusCode();
                        ViewBag.IdDepartement = new SelectList(db.Departements, "IdDepartement", "NomDep", medecin.IdDepartement);
                        return RedirectToAction("Index");

                    }
                }
            }

            return View(medecin);
        }

        // GET: Medecins/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string url = "https://localhost:44333/api/Medecins/" + id;
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception();
                }

                var medecin = await response.Content.ReadAsAsync<Medecin>();
                ViewBag.IdDepartement = new SelectList(db.Departements, "IdDepartement", "NomDep", medecin.IdDepartement);
                return View(medecin);
            }
        }

        // POST: Medecins/Edit/5
        // Pour vous protéger des attaques par survalidation, activez les propriétés spécifiques auxquelles vous souhaitez vous lier. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "IdMedecin,NomMed,PrenomMed,AdresseMed,TelephoneMed,SpecialiteComplementaire,IdDepartement")] Medecin medecin)
        {
            if (ModelState.IsValid)
            {
                string json = JsonConvert.SerializeObject(medecin);
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("token", "azerty");
                    HttpContent cont = new StringContent(json, Encoding.UTF8, "application/json");
                    // envoie des infos
                    var send = await client.PutAsync("https://localhost:44333/api/Medecins/" + medecin.IdMedecin, cont).ConfigureAwait(false);

                    if (!send.IsSuccessStatusCode)
                        throw new Exception();

                    send.EnsureSuccessStatusCode();
                    ViewBag.IdDepartement = new SelectList(db.Departements, "IdDepartement", "NomDep", medecin.IdDepartement);
                    return RedirectToAction("Index");
                }
            }
            return View(medecin);
        }

        // GET: Medecins/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string url = "https://localhost:44333/api/Medecins/" + id;
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception();
                }

                var medecin = await response.Content.ReadAsAsync<Medecin>();
                return View(medecin);
            }
        }

        // POST: Medecins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            string url = "https://localhost:44333/api/Medecins/" + id;

            //controle indispensable
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("token", "azerty");
                HttpResponseMessage response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception();
                }

                //récupere medecein en question
                var medecin = await response.Content.ReadAsAsync<Medecin>();
                return RedirectToAction("Index");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
