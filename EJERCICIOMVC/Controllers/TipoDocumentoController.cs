using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using EJERCICIOMVC.Models;

namespace EJERCICIOMVC.Controllers
{

    public class TipoDocumentoController : Controller
    {
        private bibliosafeEntities db = new bibliosafeEntities();

        // GET: TipoDocumento
        public ActionResult Index()
        {

            if (User.IsInRole("AD") | User.IsInRole("CC"))
            {
                List<TipoDocumento> objetos = db.TipoDocumento.Where(a => a.TD_Estado != 99).ToList();
                return View(objetos);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public JsonResult TDOC(string term)
        {
            if (string.IsNullOrEmpty(term))
                term = "";
            var ajxrespose = new
            {
                results = db.TipoDocumento.Where(a => a.TD_Estado == 1 && a.TD_Nombre.Contains(term)).Select(a => new { text = a.TD_Nombre, id = a.TD_Codigo }).ToList(),
            };
            return new JsonResult()
            {
                Data = ajxrespose,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        // GET: TipoDocumento/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TipoDocumento tipoDocumento = db.TipoDocumento.Find(id);
        //    if (tipoDocumento == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tipoDocumento);
        //}

        //// GET: TipoDocumento/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: TipoDocumento/Create
        //// Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        //// más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "id,TD_Codigo,TD_Nombre,TD_Estado,TD_FechaCreacion,TD_FechaModificacion,TD_UsuarioModificacion,TD_UsuarioCreacion")] TipoDocumento tipoDocumento)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        tipoDocumento.TD_Estado = 1;
        //        tipoDocumento.TD_FechaCreacion = DateTime.Now;
        //        tipoDocumento.TD_FechaModificacion = DateTime.Now;
        //        tipoDocumento.TD_UsuarioModificacion = User.Identity.Name;
        //        tipoDocumento.TD_UsuarioCreacion = User.Identity.Name;
        //        db.TipoDocumento.Add(tipoDocumento);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(tipoDocumento);
        //}

        //// GET: TipoDocumento/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TipoDocumento tipoDocumento = db.TipoDocumento.Find(id);
        //    if (tipoDocumento == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tipoDocumento);
        //}

        //// POST: TipoDocumento/Edit/5
        //// Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        //// más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "id,TD_Codigo,TD_Nombre,TD_Estado,TD_FechaCreacion,TD_FechaModificacion,TD_UsuarioModificacion,TD_UsuarioCreacion")] TipoDocumento tipoDocumento)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        tipoDocumento.TD_FechaModificacion = DateTime.Now;
        //        tipoDocumento.TD_UsuarioModificacion = User.Identity.Name;
        //        db.Entry(tipoDocumento).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(tipoDocumento);
        //}

        //// GET: TipoDocumento/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TipoDocumento tipoDocumento = db.TipoDocumento.FirstOrDefault(a => a.id == id);
        //    if (tipoDocumento == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tipoDocumento);
        //}

        //// POST: TipoDocumento/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    TipoDocumento tipoDocumento = db.TipoDocumento.Find(id);
        //    tipoDocumento.TD_Estado = 99;
        //    db.TipoDocumento.Remove(tipoDocumento);
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
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public JsonResult Lista()
        {
            List<TipoDocumento> olista = new List<TipoDocumento>();
            using (bibliosafeEntities db = new bibliosafeEntities())
            {
                olista = (from p in db.TipoDocumento select p).ToList();
            }
            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Obtener(int id)
        {
            TipoDocumento docu = new TipoDocumento();

            using (bibliosafeEntities db = new bibliosafeEntities())
            {

                docu = (from p in db.TipoDocumento.Where(x => x.id == id)
                        select p).FirstOrDefault();
            }

            return Json(docu, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Guardar(TipoDocumento Docum)
        {

            bool respuesta = true;
            try
            {

                if (Docum.id == 0)
                {
                    using (bibliosafeEntities db = new bibliosafeEntities())
                    {
                        Docum.TD_Estado = 1;
                        Docum.TD_FechaCreacion = DateTime.Now;
                        Docum.TD_FechaModificacion = DateTime.Now;
                        Docum.TD_UsuarioModificacion = User.Identity.Name;
                        Docum.TD_UsuarioCreacion = User.Identity.Name;
                        db.TipoDocumento.Add(Docum);
                        db.SaveChanges();
                    }
                }
                else
                {
                    using (bibliosafeEntities db = new bibliosafeEntities())
                    {
                        TipoDocumento tempDocumento = (from p in db.TipoDocumento
                                                       where p.id == Docum.id
                                                       select p).FirstOrDefault();


                        tempDocumento.TD_FechaModificacion = DateTime.Now;
                        tempDocumento.TD_UsuarioModificacion = User.Identity.Name;
                        tempDocumento.TD_Nombre = Docum.TD_Nombre;
                        tempDocumento.TD_Codigo = Docum.TD_Codigo;
                        tempDocumento.TD_Estado = Docum.TD_Estado;
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
                    TipoDocumento Docum = new TipoDocumento();
                    Docum = (from p in db.TipoDocumento.Where(x => x.id == id)
                             select p).FirstOrDefault();

                    db.TipoDocumento.Remove(Docum);

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
