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
    public class PrestamoUsuariosController : Controller
    {
        private bibliosafeEntities db = new bibliosafeEntities();

        // GET: PrestamoUsuarios
        public ActionResult Index()
        {
            return View(db.PrestamoUsuario.ToList());
        }

        //// GET: PrestamoUsuarios/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    PrestamoUsuario prestamoUsuario = db.PrestamoUsuario.Find(id);
        //    if (prestamoUsuario == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(prestamoUsuario);
        //}

        //// GET: PrestamoUsuarios/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: PrestamoUsuarios/Create
        //// Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        //// más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "id,PU_CodigoUsuario,PU_NombreEjemplar,PU_CodigoEjemplar,PU_FechaExtraccion,PU_FechaDevolucion,PU_PrestamoActual,PU_EstadoDevolucion,PU_FechaCreacion,PU_FechaModificacion,PU_UsuarioCreacion,PU_UsuarioModificacion,PU_Estado")] PrestamoUsuario prestamoUsuario)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.PrestamoUsuario.Add(prestamoUsuario);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(prestamoUsuario);
        //}

        //// GET: PrestamoUsuarios/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    PrestamoUsuario prestamoUsuario = db.PrestamoUsuario.Find(id);
        //    if (prestamoUsuario == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(prestamoUsuario);
        //}

        //// POST: PrestamoUsuarios/Edit/5
        //// Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        //// más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "id,PU_CodigoUsuario,PU_NombreEjemplar,PU_CodigoEjemplar,PU_FechaExtraccion,PU_FechaDevolucion,PU_PrestamoActual,PU_EstadoDevolucion,PU_FechaCreacion,PU_FechaModificacion,PU_UsuarioCreacion,PU_UsuarioModificacion,PU_Estado")] PrestamoUsuario prestamoUsuario)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(prestamoUsuario).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(prestamoUsuario);
        //}

        //// GET: PrestamoUsuarios/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    PrestamoUsuario prestamoUsuario = db.PrestamoUsuario.Find(id);
        //    if (prestamoUsuario == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(prestamoUsuario);
        //}

        //// POST: PrestamoUsuarios/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    PrestamoUsuario prestamoUsuario = db.PrestamoUsuario.Find(id);
        //    db.PrestamoUsuario.Remove(prestamoUsuario);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// ////////////////////////////////////////////////////////////

        public JsonResult Lista()
        {
            List<PrestamoUsuario> olista = new List<PrestamoUsuario>();
            using (bibliosafeEntities db = new bibliosafeEntities())
            {
                olista = (from p in db.PrestamoUsuario select p).ToList();
            }
            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Obtener(int id)
        {
            PrestamoUsuario docu = new PrestamoUsuario();

            using (bibliosafeEntities db = new bibliosafeEntities())
            {

                docu = (from p in db.PrestamoUsuario.Where(x => x.id == id)
                        select p).FirstOrDefault();
            }

            return Json(docu, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Guardar(PrestamoUsuario Docum)
        {

            bool respuesta = true;
            try
            {

                if (Docum.id == 0)
                {
                    using (bibliosafeEntities db = new bibliosafeEntities())
                    {
                        Docum.PU_Estado = 1;
                        Docum.PU_FechaCreacion = DateTime.Now;
                        Docum.PU_FechaModificacion = DateTime.Now;
                        Docum.PU_UsuarioModificacion = User.Identity.Name;
                        Docum.PU_UsuarioCreacion = User.Identity.Name;

                        db.PrestamoUsuario.Add(Docum);
                        db.SaveChanges();
                    }
                }
                else
                {
                    using (bibliosafeEntities db = new bibliosafeEntities())
                    {
                        PrestamoUsuario tempDocumento = (from p in db.PrestamoUsuario
                                                  where p.id == Docum.id
                                                  select p).FirstOrDefault();


                        tempDocumento.PU_FechaModificacion = DateTime.Now;
                        tempDocumento.PU_UsuarioModificacion = User.Identity.Name;
                        tempDocumento.PU_CodigoEjemplar = Docum.PU_CodigoEjemplar;
                        tempDocumento.PU_CodigoUsuario = Docum.PU_CodigoUsuario;
                        tempDocumento.PU_NombreEjemplar = Docum.PU_NombreEjemplar;
                        tempDocumento.PU_FechaExtraccion = Docum.PU_FechaExtraccion;
                        tempDocumento.PU_FechaDevolucion = Docum.PU_FechaDevolucion;
                        tempDocumento.PU_PrestamoActual = Docum.PU_PrestamoActual;
                        tempDocumento.PU_EstadoDevolucion = Docum.PU_EstadoDevolucion;
                        tempDocumento.PU_Estado = Docum.PU_Estado;

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
                    PrestamoUsuario Docum = new PrestamoUsuario();
                    Docum = (from p in db.PrestamoUsuario.Where(x => x.id == id)
                             select p).FirstOrDefault();

                    db.PrestamoUsuario.Remove(Docum);

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





