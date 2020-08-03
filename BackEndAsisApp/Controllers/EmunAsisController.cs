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
    public class EmunAsisController : Controller
    {
        private DBAsisApp db = new DBAsisApp();

        // GET: EmunAsis
        public ActionResult Index()
        {
            return View(db.EmunAsis.ToList());
        }

        // GET: EmunAsis/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmunAsi emunAsi = db.EmunAsis.Find(id);
            if (emunAsi == null)
            {
                return HttpNotFound();
            }
            return View(emunAsi);
        }

        // GET: EmunAsis/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmunAsis/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdEnumAsis,Descripcion")] EmunAsi emunAsi)
        {
            if (ModelState.IsValid)
            {
                db.EmunAsis.Add(emunAsi);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(emunAsi);
        }

        // GET: EmunAsis/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmunAsi emunAsi = db.EmunAsis.Find(id);
            if (emunAsi == null)
            {
                return HttpNotFound();
            }
            return View(emunAsi);
        }

        // POST: EmunAsis/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdEnumAsis,Descripcion")] EmunAsi emunAsi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(emunAsi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(emunAsi);
        }

        // GET: EmunAsis/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmunAsi emunAsi = db.EmunAsis.Find(id);
            if (emunAsi == null)
            {
                return HttpNotFound();
            }
            return View(emunAsi);
        }

        // POST: EmunAsis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EmunAsi emunAsi = db.EmunAsis.Find(id);
            db.EmunAsis.Remove(emunAsi);
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
