using EJERCICIOMVC.Models;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Services.Description;

namespace EJERCICIOMVC.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            if (User.IsInRole("AD") | User.IsInRole("CC")){
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }

        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult LogOff()
        {
            //AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }


        [HttpPost]
        public ActionResult Login(LoginDTO ousuario)
        {
            using (var db = new bibliosafeEntities())
            {
                var user = db.Usuario.FirstOrDefault(a => a.US_Codigo == ousuario.Usuario);
                if (user == null)
                {
                    ViewBag.Mensaje = "Error al ingresar el Nombre de usuario";
                    ViewBag.error = true;
                    return View(ousuario);
                }
                MD5 md5 = MD5.Create();
                byte[] md5bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(ousuario.Password));

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < md5bytes.Length; i++)
                {
                    sb.Append(md5bytes[i].ToString("X2"));
                }
                var password = sb.ToString();
                md5.Dispose();


                if (user.US_Password.ToUpper() != password.ToUpper())
                {
                    ViewBag.Mensaje = "Error al ingresar el password";
                    ViewBag.error = true;
                    return View(ousuario);
                }

                ViewBag.error = false;

                //si todo va bien
                FormsAuthentication.SetAuthCookie(ousuario.Usuario, false);

                var authTicket = new FormsAuthenticationTicket(1, user.US_Codigo, DateTime.Now, DateTime.Now.AddMinutes(20), false, user.US_Tipodedocumento);

                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

                HttpContext.Response.Cookies.Add(authCookie);

                return RedirectToAction("Index", "Home");

            }
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}