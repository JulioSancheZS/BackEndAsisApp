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
    public class AsistenciasController : Controller
    {
        private DBAsisApp db = new DBAsisApp();

        // GET: Asistencias
        public ActionResult Index()
        {
            var asistencias = db.Asistencias.Include(a => a.Alumno).Include(a => a.Docente).Include(a => a.EmunAsi);
            return View(asistencias.ToList());
        }

        // GET: Asistencias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asistencia asistencia = db.Asistencias.Find(id);
            if (asistencia == null)
            {
                return HttpNotFound();
            }
            return View(asistencia);
        }

        // GET: Asistencias/Create
        public ActionResult Create()
        {
            ViewBag.IdAlumno = new SelectList(db.Alumnoes, "IdAlumno", "Nombre");
            ViewBag.IdDocente = new SelectList(db.Docentes, "IdDocente", "Apellido");
            ViewBag.IdEnumAsis = new SelectList(db.EmunAsis, "IdEnumAsis", "Descripcion");
            return View();
        }

        // POST: Asistencias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdAsistencia,Fecha,IdAlumno,IdEnumAsis,IdDocente")] Asistencia asistencia)
        {
            if (ModelState.IsValid)
            {
                db.Asistencias.Add(asistencia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdAlumno = new SelectList(db.Alumnoes, "IdAlumno", "Nombre", asistencia.IdAlumno);
            ViewBag.IdDocente = new SelectList(db.Docentes, "IdDocente", "Apellido", asistencia.IdDocente);
            ViewBag.IdEnumAsis = new SelectList(db.EmunAsis, "IdEnumAsis", "Descripcion", asistencia.IdEnumAsis);
            return View(asistencia);
        }

        // GET: Asistencias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asistencia asistencia = db.Asistencias.Find(id);
            if (asistencia == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdAlumno = new SelectList(db.Alumnoes, "IdAlumno", "Nombre", asistencia.IdAlumno);
            ViewBag.IdDocente = new SelectList(db.Docentes, "IdDocente", "Apellido", asistencia.IdDocente);
            ViewBag.IdEnumAsis = new SelectList(db.EmunAsis, "IdEnumAsis", "Descripcion", asistencia.IdEnumAsis);
            return View(asistencia);
        }

        // POST: Asistencias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdAsistencia,Fecha,IdAlumno,IdEnumAsis,IdDocente")] Asistencia asistencia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(asistencia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdAlumno = new SelectList(db.Alumnoes, "IdAlumno", "Nombre", asistencia.IdAlumno);
            ViewBag.IdDocente = new SelectList(db.Docentes, "IdDocente", "Apellido", asistencia.IdDocente);
            ViewBag.IdEnumAsis = new SelectList(db.EmunAsis, "IdEnumAsis", "Descripcion", asistencia.IdEnumAsis);
            return View(asistencia);
        }

        // GET: Asistencias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asistencia asistencia = db.Asistencias.Find(id);
            if (asistencia == null)
            {
                return HttpNotFound();
            }
            return View(asistencia);
        }

        // POST: Asistencias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Asistencia asistencia = db.Asistencias.Find(id);
            db.Asistencias.Remove(asistencia);
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
