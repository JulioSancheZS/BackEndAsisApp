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
    
    public class AlumnoesController : Controller
    {
        private DBAsisApp db = new DBAsisApp();

        // GET: Alumnoes
        public ActionResult Index()
        {
            var alumnoes = db.Alumnoes.Include(a => a.Grado).Include(a => a.Grupo).Include(a => a.Tutor);
            return View(alumnoes.ToList());
        }

        // GET: Alumnoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alumno alumno = db.Alumnoes.Find(id);
            if (alumno == null)
            {
                return HttpNotFound();
            }
            return View(alumno);
        }

        // GET: Alumnoes/Create
        public ActionResult Create()
        {
            ViewBag.IdGrado = new SelectList(db.Gradoes, "IdGrado", "Descripcion");
            ViewBag.IdGrupo = new SelectList(db.Grupoes, "IdGrupo", "NomGrupo");
            ViewBag.IdTutor = new SelectList(db.Tutors, "IdTutor", "Nombre");
            return View();
        }

        // POST: Alumnoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdAlumno,Nombre,Apellido,Sexo,Telefono,Email,IdGrado,IdGrupo,IdTutor")] Alumno alumno)
        {
            if (ModelState.IsValid)
            {
                db.Alumnoes.Add(alumno);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdGrado = new SelectList(db.Gradoes, "IdGrado", "Descripcion", alumno.IdGrado);
            ViewBag.IdGrupo = new SelectList(db.Grupoes, "IdGrupo", "NomGrupo", alumno.IdGrupo);
            ViewBag.IdTutor = new SelectList(db.Tutors, "IdTutor", "Nombre", alumno.IdTutor);
            return View(alumno);
        }

        // GET: Alumnoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alumno alumno = db.Alumnoes.Find(id);
            if (alumno == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdGrado = new SelectList(db.Gradoes, "IdGrado", "Descripcion", alumno.IdGrado);
            ViewBag.IdGrupo = new SelectList(db.Grupoes, "IdGrupo", "NomGrupo", alumno.IdGrupo);
            ViewBag.IdTutor = new SelectList(db.Tutors, "IdTutor", "Nombre", alumno.IdTutor);
            return View(alumno);
        }

        // POST: Alumnoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdAlumno,Nombre,Apellido,Sexo,Telefono,Email,IdGrado,IdGrupo,IdTutor")] Alumno alumno)
        {
            if (ModelState.IsValid)
            {
                db.Entry(alumno).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdGrado = new SelectList(db.Gradoes, "IdGrado", "Descripcion", alumno.IdGrado);
            ViewBag.IdGrupo = new SelectList(db.Grupoes, "IdGrupo", "NomGrupo", alumno.IdGrupo);
            ViewBag.IdTutor = new SelectList(db.Tutors, "IdTutor", "Nombre", alumno.IdTutor);
            return View(alumno);
        }

        // GET: Alumnoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alumno alumno = db.Alumnoes.Find(id);
            if (alumno == null)
            {
                return HttpNotFound();
            }
            return View(alumno);
        }

        // POST: Alumnoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Alumno alumno = db.Alumnoes.Find(id);
            db.Alumnoes.Remove(alumno);
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
