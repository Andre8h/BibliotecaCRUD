using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using EJERCICIOMVC.Models;

namespace EJERCICIOMVC.Controllers
{
    public class EjemplaresController : Controller
    {
        private bibliosafeEntities db = new bibliosafeEntities();

        // GET: Ejemplares
        public ActionResult Index()
        {

            if (User.IsInRole("AD") | User.IsInRole("CC"))
            {
                List<Ejemplar> objetos = db.Ejemplar.Where(a => a.EJ_Estado != 99).ToList();
                return View(objetos);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        // GET: Ejemplares/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ejemplar ejemplar = db.Ejemplar.Find(id);
            if (ejemplar == null)
            {
                return HttpNotFound();
            }
            return View(ejemplar);
        }

        // GET: Ejemplares/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Ejemplares/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,EJ_NombreLibro,EJ_CodigoEjemplar,EJ_CodigoLibro,EJ_Estado,EJ_FechaCreacion,EJ_FechaModificacion,EJ_UsuarioCreacion,EJ_UsuarioModificacion,EJ_TipoEjemplar,EJ_EstadoSistema")] Ejemplar ejemplar)
        {
            if (ModelState.IsValid)
            {
                ejemplar.EJ_Estado = 1;
                ejemplar.EJ_UsuarioModificacion = User.Identity.Name;
                ejemplar.EJ_UsuarioCreacion = "01";
                ejemplar.EJ_FechaCreacion = DateTime.Now;
                ejemplar.EJ_FechaModificacion = DateTime.Now;
                db.Ejemplar.Add(ejemplar);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ejemplar);
        }

        // GET: Ejemplares/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ejemplar ejemplar = db.Ejemplar.Find(id);
            if (ejemplar == null)
            {
                return HttpNotFound();
            }
            return View(ejemplar);
        }

        // POST: Ejemplares/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EJ_NombreLibro,EJ_CodigoEjemplar,EJ_CodigoLibro,EJ_Estado,EJ_FechaCreacion,EJ_FechaModificacion,EJ_UsuarioCreacion,EJ_UsuarioModificacion,EJ_TipoEjemplar,EJ_EstadoSistema")] Ejemplar ejemplar)
        {
            if (ModelState.IsValid)
            {
                ejemplar.EJ_UsuarioModificacion = User.Identity.Name;
                ejemplar.EJ_FechaModificacion = DateTime.Now;
                db.Entry(ejemplar).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ejemplar);
        }

        // GET: Ejemplares/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ejemplar ejemplar = db.Ejemplar.FirstOrDefault(a => a.Id == id);
            if (ejemplar == null)
            {
                return HttpNotFound();
            }
            return View(ejemplar);
        }

        // POST: Ejemplares/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ejemplar ejemplar = db.Ejemplar.Find(id);
            ejemplar.EJ_Estado = 99;
            db.Ejemplar.Remove(ejemplar);
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
        //////////////////////////////////////////////////////////////////////////////////////////////////
        public JsonResult Lista()
        {
            List<Ejemplar> olista = new List<Ejemplar>();
            using (bibliosafeEntities db = new bibliosafeEntities())
            {
                olista = (from p in db.Ejemplar select p).ToList();
            }
            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Obtener(int id)
        {
            Ejemplar docu = new Ejemplar();

            using (bibliosafeEntities db = new bibliosafeEntities())
            {

                docu = (from p in db.Ejemplar.Where(x => x.Id == id)
                        select p).FirstOrDefault();
            }

            return Json(docu, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Guardar(Ejemplar Docum)
        {

            bool respuesta = true;
            try
            {

                if (Docum.Id == 0)
                {
                    using (bibliosafeEntities db = new bibliosafeEntities())
                    {
                        Docum.EJ_EstadoSistema = true;
                        Docum.EJ_FechaCreacion = DateTime.Now;
                        Docum.EJ_FechaModificacion = DateTime.Now;
                        Docum.EJ_UsuarioModificacion = User.Identity.Name;
                        Docum.EJ_UsuarioCreacion = User.Identity.Name;

                        db.Ejemplar.Add(Docum);
                        db.SaveChanges();
                    }
                }
                else
                {
                    using (bibliosafeEntities db = new bibliosafeEntities())
                    {
                        Ejemplar tempDocumento = (from p in db.Ejemplar
                                                  where p.Id == Docum.Id
                                                  select p).FirstOrDefault();


                        tempDocumento.EJ_FechaModificacion = DateTime.Now;
                        tempDocumento.EJ_UsuarioModificacion = User.Identity.Name;
                        tempDocumento.EJ_NombreLibro = Docum.EJ_NombreLibro;
                        tempDocumento.EJ_CodigoLibro = Docum.EJ_CodigoLibro;
                        tempDocumento.EJ_CodigoEjemplar = Docum.EJ_CodigoEjemplar;
                        tempDocumento.EJ_TipoEjemplar = Docum.EJ_TipoEjemplar;
                        tempDocumento.EJ_EstadoSistema = Docum.EJ_EstadoSistema;
                        tempDocumento.EJ_Estado = Docum.EJ_Estado;
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
                    Ejemplar Docum = new Ejemplar();
                    Docum = (from p in db.Ejemplar.Where(x => x.Id == id)
                             select p).FirstOrDefault();

                    db.Ejemplar.Remove(Docum);

                    db.SaveChanges();
                }
            }
            catch
            {
                respuesta = false;
            }



            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CallAjax(string term)
        {
            if (string.IsNullOrEmpty(term))
                term = "";
            var ajxrespose = new
            {
                results = db.Ejemplar.Where(a => a.EJ_EstadoSistema == true && a.EJ_NombreLibro.Contains(term)).Select(a => new { text = a.EJ_CodigoEjemplar, id = a.EJ_CodigoEjemplar }).ToList(),
            };
            return new JsonResult()
            {
                Data = ajxrespose,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }
       

        public JsonResult CallAjax2(string term)
        {
            if (string.IsNullOrEmpty(term))
                term = "";
            var ajxrespose = new
            {
                results = db.Ejemplar.Where(a => a.EJ_EstadoSistema == true && a.EJ_NombreLibro.Contains(term)).Select(a => new { text = a.EJ_NombreLibro + " " + a.EJ_CodigoEjemplar, id = a.EJ_NombreLibro }).ToList(),
            };
            return new JsonResult()
            {
                Data = ajxrespose,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }
    }
}
