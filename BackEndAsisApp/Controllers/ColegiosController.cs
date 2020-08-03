using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BackEndAsisApp.Models;

namespace BackEndAsisApp.Controllers
{
    public class ColegiosController : Controller
    {
        private DBAsisApp db = new DBAsisApp();

        // GET: Colegios
        public ActionResult Index()
        {
            var colegios = db.Colegios.Include(c => c.AnioElectivo).Include(c => c.Departamento).Include(c => c.Director);
            return View(colegios.ToList());
        }

        // GET: Colegios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Colegio colegio = db.Colegios.Find(id);
            if (colegio == null)
            {
                return HttpNotFound();
            }
            return View(colegio);
        }

        // GET: Colegios/Create
        public ActionResult Create()
        {
            ViewBag.IdAnioElectivo = new SelectList(db.AnioElectivoes, "IdAnioElectivo", "Descripcion");
            ViewBag.IdDepartamento = new SelectList(db.Departamentoes, "IdDepartamento", "NomDepartamento");
            ViewBag.IdDirector = new SelectList(db.Directors, "IdDirector", "Nombre");
            return View();
        }

        // POST: Colegios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idColegio,NomColegio,IdAnioElectivo,IdDepartamento,IdDirector")] Colegio colegio)
        {
            if (ModelState.IsValid)
            {
                db.Colegios.Add(colegio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdAnioElectivo = new SelectList(db.AnioElectivoes, "IdAnioElectivo", "Descripcion", colegio.IdAnioElectivo);
            ViewBag.IdDepartamento = new SelectList(db.Departamentoes, "IdDepartamento", "NomDepartamento", colegio.IdDepartamento);
            ViewBag.IdDirector = new SelectList(db.Directors, "IdDirector", "Nombre", colegio.IdDirector);
            return View(colegio);
        }

        // GET: Colegios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Colegio colegio = db.Colegios.Find(id);
            if (colegio == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdAnioElectivo = new SelectList(db.AnioElectivoes, "IdAnioElectivo", "Descripcion", colegio.IdAnioElectivo);
            ViewBag.IdDepartamento = new SelectList(db.Departamentoes, "IdDepartamento", "NomDepartamento", colegio.IdDepartamento);
            ViewBag.IdDirector = new SelectList(db.Directors, "IdDirector", "Nombre", colegio.IdDirector);
            return View(colegio);
        }

        // POST: Colegios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idColegio,NomColegio,IdAnioElectivo,IdDepartamento,IdDirector")] Colegio colegio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(colegio).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdAnioElectivo = new SelectList(db.AnioElectivoes, "IdAnioElectivo", "Descripcion", colegio.IdAnioElectivo);
            ViewBag.IdDepartamento = new SelectList(db.Departamentoes, "IdDepartamento", "NomDepartamento", colegio.IdDepartamento);
            ViewBag.IdDirector = new SelectList(db.Directors, "IdDirector", "Nombre", colegio.IdDirector);
            return View(colegio);
        }

        // GET: Colegios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Colegio colegio = db.Colegios.Find(id);
            if (colegio == null)
            {
                return HttpNotFound();
            }
            return View(colegio);
        }

        // POST: Colegios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Colegio colegio = db.Colegios.Find(id);
            db.Colegios.Remove(colegio);
            db.SaveChanges();
            return RedirectToAction("Index");
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
