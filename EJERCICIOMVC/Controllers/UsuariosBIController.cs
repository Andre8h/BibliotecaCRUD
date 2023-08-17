using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EJERCICIOMVC.Models;
using System.Security.Cryptography;
using System.Text;

namespace EJERCICIOMVC.Controllers
{
    public class UsuariosBIController : Controller
    {
        private bibliosafeEntities db = new bibliosafeEntities();

        // GET: UsuariosBI
        public ActionResult Index()
        {
            if (User.IsInRole("AD") | User.IsInRole("CC"))
            {
                List<Usuario> objetos = db.Usuario.Where(a => a.US_Estado != 99).ToList();
                return View(objetos);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }

        }

        // GET: UsuariosBI/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // GET: UsuariosBI/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsuariosBI/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,US_Codigo,US_Nombre,US_Password,US_Estado,US_FechaCreacion,US_FechaModificacion,US_UsuarioCreacion,US_UsuarioModificacion,US_Email,US_Tipodedocumento,US_Documento")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.US_Estado = 1;
                usuario.US_FechaCreacion = DateTime.Now;
                usuario.US_FechaModificacion = DateTime.Now;
                usuario.US_UsuarioModificacion = User.Identity.Name;
                usuario.US_UsuarioCreacion = User.Identity.Name;

                MD5 md5 = MD5.Create();
                byte[] md5bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(usuario.US_Password));

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < md5bytes.Length; i++)
                {
                    sb.Append(md5bytes[i].ToString("X2"));
                }
                usuario.US_Password = sb.ToString();
                md5.Dispose();


                db.Usuario.Add(usuario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(usuario);
        }

        // GET: UsuariosBI/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: UsuariosBI/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,US_Codigo,US_Nombre,US_Password,US_Estado,US_FechaCreacion,US_FechaModificacion,US_UsuarioCreacion,US_UsuarioModificacion,US_Email,US_Tipodedocumento,US_Documento")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                var usr = db.Usuario.FirstOrDefault(a => a.US_Codigo == usuario.US_Codigo);
                usr.US_Codigo = usuario.US_Codigo;
                usr.US_Tipodedocumento = usuario.US_Tipodedocumento;
                usr.US_Documento = usuario.US_Documento;
                usr.US_Estado = usuario.US_Estado;
                usr.US_Email = usuario.US_Email;
                usr.US_FechaModificacion = DateTime.Now;
                usr.US_UsuarioModificacion = User.Identity.Name;
                usr.US_Nombre = usuario.US_Nombre;

                if (!string.IsNullOrEmpty(usuario.US_Password))
                {
                    MD5 md5 = MD5.Create();
                    byte[] md5bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(usuario.US_Password));

                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < md5bytes.Length; i++)
                    {
                        sb.Append(md5bytes[i].ToString("X2"));
                    }
                    usr.US_Password = sb.ToString();
                    md5.Dispose();
                }

                db.Entry(usr).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usuario);
        }

        // GET: UsuariosBI/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuario.FirstOrDefault(a => a.Id == id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: UsuariosBI/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Usuario usuario = db.Usuario.Find(id);
            usuario.US_Estado = 99;
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

        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public JsonResult Lista()
        {
            List<Usuario> olista = new List<Usuario>();
            using (bibliosafeEntities db = new bibliosafeEntities())
            {
                olista = (from p in db.Usuario select p).ToList();
            }
            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Obtener(int id)
        {
            Usuario docu = new Usuario();

            using (bibliosafeEntities db = new bibliosafeEntities())
            {

                docu = (from p in db.Usuario.Where(x => x.Id == id)
                        select p).FirstOrDefault();
            }

            return Json(docu, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Guardar(Usuario Docum)
        {

            bool respuesta = true;
            try
            {

                if (Docum.Id == 0)
                {
                    using (bibliosafeEntities db = new bibliosafeEntities())
                    {
                        Docum.US_Estado = 1;
                        Docum.US_FechaCreacion = DateTime.Now;
                        Docum.US_FechaModificacion = DateTime.Now;
                        Docum.US_UsuarioModificacion = User.Identity.Name;
                        Docum.US_UsuarioCreacion = User.Identity.Name;

                        MD5 md5 = MD5.Create();
                        byte[] md5bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(Docum.US_Password));

                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < md5bytes.Length; i++)
                        {
                            sb.Append(md5bytes[i].ToString("X2"));
                        }
                        Docum.US_Password = sb.ToString();
                        md5.Dispose();

                        db.Usuario.Add(Docum);
                        db.SaveChanges();
                    }
                }
                else
                {
                    using (bibliosafeEntities db = new bibliosafeEntities())
                    {
                        Usuario tempDocumento = (from p in db.Usuario
                                                 where p.Id == Docum.Id
                                                 select p).FirstOrDefault();

                        tempDocumento.US_Codigo = Docum.US_Codigo;
                        tempDocumento.US_Tipodedocumento = Docum.US_Tipodedocumento;
                        tempDocumento.US_Documento = Docum.US_Documento;
                        tempDocumento.US_Estado = Docum.US_Estado;
                        tempDocumento.US_Email = Docum.US_Email;
                        tempDocumento.US_FechaModificacion = DateTime.Now;
                        tempDocumento.US_UsuarioModificacion = User.Identity.Name;
                        tempDocumento.US_Nombre = Docum.US_Nombre;

                        if (!string.IsNullOrEmpty(Docum.US_Password))
                        {
                            MD5 md5 = MD5.Create();
                            byte[] md5bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(Docum.US_Password));

                            StringBuilder sb = new StringBuilder();
                            for (int i = 0; i < md5bytes.Length; i++)
                            {
                                sb.Append(md5bytes[i].ToString("X2"));
                            }
                            tempDocumento.US_Password = sb.ToString();
                            md5.Dispose();
                        }

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
                    Usuario Docum = new Usuario();
                    Docum = (from p in db.Usuario.Where(x => x.Id == id)
                             select p).FirstOrDefault();

                    db.Usuario.Remove(Docum);

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
                results = db.Usuario.Where(a => a.US_Estado == 1 && a.US_Nombre.Contains(term)).Select(a => new { text = a.US_Nombre, id = a.US_Codigo }).ToList(),
            };
            return new JsonResult()
            {
                Data = ajxrespose,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }
    }
}
