using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GuiasOET.Models;
using System.Diagnostics;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Web.UI.HtmlControls;
using System.Data.Entity;
using PagedList;
using MvcFlashMessages;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Collections;

namespace GuiasOET.Controllers
{
    public class AsignacionReservacionesController : Controller
    {


     
        public ActionResult ConsultarAsignacion()
        {
            return View();
        }



    }
}