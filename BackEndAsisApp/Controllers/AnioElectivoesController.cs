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
    [Authorize]
    public class AnioElectivoesController : Controller
    {
        private DBAsisApp db = new DBAsisApp();

        // GET: AnioElectivoes
        public ActionResult Index()
        {
            return View(db.AnioElectivoes.ToList());
        }

        // GET: AnioElectivoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnioElectivo anioElectivo = db.AnioElectivoes.Find(id);
            if (anioElectivo == null)
            {
                return HttpNotFound();
            }
            return View(anioElectivo);
        }

        // GET: AnioElectivoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AnioElectivoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdAnioElectivo,Descripcion,Semestre")] AnioElectivo anioElectivo)
        {
            if (ModelState.IsValid)
            {
                db.AnioElectivoes.Add(anioElectivo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(anioElectivo);
        }

        // GET: AnioElectivoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnioElectivo anioElectivo = db.AnioElectivoes.Find(id);
            if (anioElectivo == null)
            {
                return HttpNotFound();
            }
            return View(anioElectivo);
        }

        // POST: AnioElectivoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdAnioElectivo,Descripcion,Semestre")] AnioElectivo anioElectivo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(anioElectivo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(anioElectivo);
        }

        // GET: AnioElectivoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnioElectivo anioElectivo = db.AnioElectivoes.Find(id);
            if (anioElectivo == null)
            {
                return HttpNotFound();
            }
            return View(anioElectivo);
        }

        // POST: AnioElectivoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AnioElectivo anioElectivo = db.AnioElectivoes.Find(id);
            db.AnioElectivoes.Remove(anioElectivo);
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
