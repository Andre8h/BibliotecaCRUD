using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EJERCICIOMVC.Models;

namespace EJERCICIOMVC.Controllers
{
    public class ReparacionesController : Controller
    {
        private bibliosafeEntities db = new bibliosafeEntities();

        // GET: Reparaciones
        public ActionResult Index()
        {
            return View(db.Reparacion.ToList());
        }

        // GET: Reparaciones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reparacion reparacion = db.Reparacion.Find(id);
            if (reparacion == null)
            {
                return HttpNotFound();
            }
            return View(reparacion);
        }

        // GET: Reparaciones/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Reparaciones/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,RE_EjemplarCodigo,RE_EstadoInicial,RE_InicioReparacion,RE_FinalizacionReparacion,RE_EstadoActualFisico,RE_Estado,RE_FechaReIncorporacion,RE_FechaCreacion,RE_FechaModificacion,RE_UsuarioCreacion,RE_UsuarioModificacion")] Reparacion reparacion)
        {
            if (ModelState.IsValid)
            {
                db.Reparacion.Add(reparacion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(reparacion);
        }

        // GET: Reparaciones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reparacion reparacion = db.Reparacion.Find(id);
            if (reparacion == null)
            {
                return HttpNotFound();
            }
            return View(reparacion);
        }

        // POST: Reparaciones/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,RE_EjemplarCodigo,RE_EstadoInicial,RE_InicioReparacion,RE_FinalizacionReparacion,RE_EstadoActualFisico,RE_Estado,RE_FechaReIncorporacion,RE_FechaCreacion,RE_FechaModificacion,RE_UsuarioCreacion,RE_UsuarioModificacion")] Reparacion reparacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reparacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reparacion);
        }

        // GET: Reparaciones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reparacion reparacion = db.Reparacion.Find(id);
            if (reparacion == null)
            {
                return HttpNotFound();
            }
            return View(reparacion);
        }

        // POST: Reparaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reparacion reparacion = db.Reparacion.Find(id);
            db.Reparacion.Remove(reparacion);
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


        ///////////////////////////////////////////////////////////////////////////////////////
        public JsonResult Lista()
        {
            List<Reparacion> olista = new List<Reparacion>();
            using (bibliosafeEntities db = new bibliosafeEntities())
            {
                olista = (from p in db.Reparacion select p).ToList();
            }
            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Obtener(int id)
        {
            Reparacion docu = new Reparacion();

            using (bibliosafeEntities db = new bibliosafeEntities())
            {

                docu = (from p in db.Reparacion.Where(x => x.id == id)
                        select p).FirstOrDefault();
            }

            return Json(docu, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Guardar(Reparacion Docum)
        {

            bool respuesta = true;
            try
            {

                if (Docum.id == 0)
                {
                    using (bibliosafeEntities db = new bibliosafeEntities())
                    {
                        Docum.RE_Estado = 1;
                        Docum.RE_FechaCreacion = DateTime.Now;
                        Docum.RE_FechaModificacion = DateTime.Now;
                        Docum.RE_UsuarioModificacion = User.Identity.Name;
                        Docum.RE_UsuarioCreacion = User.Identity.Name;
                        db.Reparacion.Add(Docum);
                        db.SaveChanges();
                    }
                }
                else
                {
                    using (bibliosafeEntities db = new bibliosafeEntities())
                    {
                        Reparacion tempDocumento = (from p in db.Reparacion
                                               where p.id == Docum.id
                                               select p).FirstOrDefault();
                        tempDocumento.RE_FechaReIncorporacion = Docum.RE_FechaReIncorporacion;
                        tempDocumento.RE_EjemplarCodigo = Docum.RE_EjemplarCodigo;
                        tempDocumento.RE_EstadoInicial = Docum.RE_EstadoInicial;
                        tempDocumento.RE_InicioReparacion = Docum.RE_InicioReparacion;
                        tempDocumento.RE_FinalizacionReparacion = Docum.RE_FinalizacionReparacion;
                        tempDocumento.RE_EstadoActualFisico = Docum.RE_EstadoActualFisico;
                        tempDocumento.RE_FechaModificacion = DateTime.Now;
                        tempDocumento.RE_UsuarioModificacion = User.Identity.Name;
                        tempDocumento.RE_Estado = Docum.RE_Estado;
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
                    Reparacion Docum = new Reparacion();
                    Docum = (from p in db.Reparacion.Where(x => x.id == id)
                             select p).FirstOrDefault();

                    db.Reparacion.Remove(Docum);

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
