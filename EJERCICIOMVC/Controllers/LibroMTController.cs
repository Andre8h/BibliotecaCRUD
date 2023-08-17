using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using EJERCICIOMVC.Models;


namespace EJERCICIOMVC.Controllers
{
    public class LibroMTController : Controller
    {

        private bibliosafeEntities db = new bibliosafeEntities();



        // GET: LibroMT
        public ActionResult Index()
        {
            if (User.IsInRole("AD") | User.IsInRole("CC"))
            {
                List<Libro> objetos = db.Libro.Where(a => a.LI_Estado != 99).ToList();
                return View(objetos);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }

        }


        // GET: LibroMT/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Libro libro = db.Libro.Find(id);
            if (libro == null)
            {
                return HttpNotFound();
            }
            return View(libro);
        }

        // GET: LibroMT/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LibroMT/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,LI_Codigo,LI_Nombre,LI_Autor,LI_Estado,LI_FechaCreacion,LI_RE_FechaModificacion,LI_RE_UsuarioCreacion,LI_RE_UsuarioModificacion")] Libro libro)
        {
            if (ModelState.IsValid)
            {
                var librodatos = db.Libro.FirstOrDefault(a => a.LI_Codigo == libro.LI_Codigo);
                if (librodatos != null)
                {
                    ViewBag.CoError = true;
                    ViewBag.CodMe = " El codigo de libro ingresado ya existe";
                    return View(libro);

                }

                ViewBag.CoError = false;
                libro.LI_Estado = 1;
                libro.LI_RE_UsuarioCreacion = User.Identity.Name;
                libro.LI_RE_UsuarioModificacion = User.Identity.Name;
                libro.LI_FechaCreacion = DateTime.Now;
                libro.LI_RE_FechaModificacion = DateTime.Now;
                db.Libro.Add(libro);
                db.SaveChanges();
                return RedirectToAction("Index");

            }

            return View(libro);
        }

        // GET: LibroMT/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Libro libro = db.Libro.Find(id);
            if (libro == null)
            {
                return HttpNotFound();
            }
            return View(libro);
        }

        // POST: LibroMT/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,LI_Codigo,LI_Nombre,LI_Autor,LI_Estado,LI_FechaCreacion,LI_RE_FechaModificacion,LI_RE_UsuarioCreacion,LI_RE_UsuarioModificacion")] Libro libro)
        {
            if (ModelState.IsValid)
            {
                libro.LI_RE_UsuarioModificacion = User.Identity.Name;
                libro.LI_RE_FechaModificacion = DateTime.Now;
                db.Entry(libro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(libro);
        }

        // GET: LibroMT/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Libro libro = db.Libro.FirstOrDefault(a => a.id == id);
            if (libro == null)
            {
                return HttpNotFound();
            }
            return View(libro);
        }

        // POST: LibroMT/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            Libro libro = db.Libro.Find(id);
            libro.LI_Estado = 99;
            db.Libro.Remove(libro);
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
        public JsonResult CallAjax(string term)
        {
            if (string.IsNullOrEmpty(term))
                term = "";
            var ajxrespose = new
            {
                results = db.Libro.Where(a => a.LI_Estado == 1 && a.LI_Nombre.Contains(term)).Select(a => new { text = a.LI_Nombre, id = a.LI_Codigo }).ToList(),
            };
            return new JsonResult()
            {
                Data = ajxrespose,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }
        
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public JsonResult Lista()
        {
            List<Libro> olista = new List<Libro>();
            using (bibliosafeEntities db = new bibliosafeEntities())
            {
                olista = (from p in db.Libro select p).ToList();
            }
            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Obtener(int id)
        {
            Libro docu = new Libro();

            using (bibliosafeEntities db = new bibliosafeEntities())
            {

                docu = (from p in db.Libro.Where(x => x.id == id)
                        select p).FirstOrDefault();
            }

            return Json(docu, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Guardar(Libro Docum)
        {

            bool respuesta = true;
            try
            {

                if (Docum.id == 0)
                {
                    using (bibliosafeEntities db = new bibliosafeEntities())
                    {
                        Docum.LI_Estado = 1;
                        Docum.LI_RE_UsuarioCreacion = User.Identity.Name;
                        Docum.LI_RE_UsuarioModificacion = User.Identity.Name;
                        Docum.LI_FechaCreacion = DateTime.Now;
                        Docum.LI_RE_FechaModificacion = DateTime.Now;
                        db.Libro.Add(Docum);
                        db.SaveChanges();
                    }
                }
                else
                {
                    using (bibliosafeEntities db = new bibliosafeEntities())
                    {
                        Libro tempDocumento = (from p in db.Libro
                                               where p.id == Docum.id
                                               select p).FirstOrDefault();

                        tempDocumento.LI_Autor = Docum.LI_Autor;
                        tempDocumento.LI_RE_FechaModificacion = DateTime.Now;
                        tempDocumento.LI_RE_UsuarioModificacion = User.Identity.Name;
                        tempDocumento.LI_Codigo = Docum.LI_Codigo;
                        tempDocumento.LI_Nombre = Docum.LI_Nombre;
                        tempDocumento.LI_Estado = Docum.LI_Estado;
                        db.Entry(tempDocumento).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                }
            }
            catch
            {
                respuesta = false;

            }

            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult Eliminar(int id)
        {
            bool respuesta = true;
            try
            {
                using (bibliosafeEntities db = new bibliosafeEntities())
                {
                    Libro Docum = new Libro();
                    Docum = (from p in db.Libro.Where(x => x.id == id)
                             select p).FirstOrDefault();

                    db.Libro.Remove(Docum);

                    db.SaveChanges();
                }
            }
            catch
            {
                respuesta = false;
            }



            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }



    }
}
