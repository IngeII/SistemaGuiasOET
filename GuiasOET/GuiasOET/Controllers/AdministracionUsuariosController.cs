using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GuiasOET.Controllers
{
    public class AdministracionUsuariosController : Controller
    {
        // GET: AdministracioUsuarios
        public ActionResult AdministracionUsuarios()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        // GET: /Seguridad/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        //
        // GET: /Seguridad/ReestablecerContraseña
        [AllowAnonymous]
        public ActionResult ReestablecerContraseña()
        {
            return View();
        }

    }
}