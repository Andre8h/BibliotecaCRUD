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

    public class AutorMTController : Controller
    {
        private bibliosafeEntities db = new bibliosafeEntities();

        // GET: AutorMT
        public ActionResult Index()
        {
            if (User.IsInRole("AD") | User.IsInRole("CC"))
            {
                List<Autor> objetos = db.Autor.Where(a => a.AU_Estado != 99).ToList();
                return View(objetos);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }

        }

        public JsonResult CallAjax(string term)
        {
            if (string.IsNullOrEmpty(term))
                term = "";
            var ajxrespose = new
            {
                results = db.Autor.Where(a => a.AU_Estado == 1 && a.AU_Nombre.Contains(term)).Select(a => new { text = a.AU_Nombre, id = a.AU_Identificacion }).ToList(),
            };
            return new JsonResult()
            {
                Data = ajxrespose,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }

        //// GET: AutorMT/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Autor autor = db.Autor.Find(id);
        //    if (autor == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(autor);
        //}

        //// GET: AutorMT/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: AutorMT/Create
        //// Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        //// más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "id,AU_TipoIdentificador,AU_Identificacion,AU_Nombre,AU_FechaCreacion,AU_FechaModificacion,AU_UsuarioCreacion,AU_UsuarioModificacion,AU_Estado")] Autor autor)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        autor.AU_TipoIdentificador = "Codigo";
        //        autor.AU_FechaCreacion = DateTime.Now;
        //        autor.AU_UsuarioCreacion = User.Identity.Name;
        //        autor.AU_Estado = 1;
        //        autor.AU_UsuarioModificacion = User.Identity.Name;
        //        db.Autor.Add(autor);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(autor);
        //}

        //// GET: AutorMT/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Autor autor = db.Autor.Find(id);
        //    if (autor == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(autor);
        //}

        //// POST: AutorMT/Edit/5
        //// Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        //// más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "id,AU_TipoIdentificador,AU_Identificacion,AU_Nombre,AU_FechaCreacion,AU_FechaModificacion,AU_UsuarioCreacion,AU_UsuarioModificacion,AU_Estado")] Autor autor)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        autor.AU_FechaModificacion = DateTime.Now;
        //        autor.AU_UsuarioModificacion = User.Identity.Name;
        //        db.Entry(autor).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(autor);
        //}

        //// GET: AutorMT/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Autor autor = db.Autor.FirstOrDefault(a => a.id == id);
        //    if (autor == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(autor);
        //}

        //// POST: AutorMT/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Autor autor = db.Autor.Find(id);
        //    autor.AU_Estado = 99; 
        //    db.Autor.Remove(autor);
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
            List<Autor> olista = new List<Autor>();
            using (bibliosafeEntities db = new bibliosafeEntities())
            {
                olista = (from p in db.Autor select p).ToList();
            }
            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Obtener(int id)
        {
            Autor docu = new Autor();

            using (bibliosafeEntities db = new bibliosafeEntities())
            {

                docu = (from p in db.Autor.Where(x => x.id == id)
                        select p).FirstOrDefault();
            }

            return Json(docu, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Guardar(Autor Docum)
        {

            bool respuesta = true;
            try
            {

                if (Docum.id == 0)
                {
                    using (bibliosafeEntities db = new bibliosafeEntities())
                    {
                        Docum.AU_Estado = 1;
                        Docum.AU_FechaCreacion = DateTime.Now;
                        Docum.AU_FechaModificacion = DateTime.Now;
                        Docum.AU_UsuarioModificacion = User.Identity.Name;
                        Docum.AU_UsuarioCreacion = User.Identity.Name;
                        Docum.AU_TipoIdentificador = "Codigo";
                        db.Autor.Add(Docum);
                        db.SaveChanges();
                    }
                }
                else
                {
                    using (bibliosafeEntities db = new bibliosafeEntities())
                    {
                        Autor tempDocumento = (from p in db.Autor
                                               where p.id == Docum.id
                                               select p).FirstOrDefault();


                        tempDocumento.AU_FechaModificacion = DateTime.Now;
                        tempDocumento.AU_UsuarioModificacion = User.Identity.Name;
                        tempDocumento.AU_Nombre = Docum.AU_Nombre;
                        tempDocumento.AU_Identificacion = Docum.AU_Identificacion;
                        tempDocumento.AU_Estado = Docum.AU_Estado;
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
                    Autor Docum = new Autor();
                    Docum = (from p in db.Autor.Where(x => x.id == id)
                             select p).FirstOrDefault();

                    db.Autor.Remove(Docum);

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
