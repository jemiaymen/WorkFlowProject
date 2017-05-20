using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using WorkFlowProject.Models;


namespace WorkFlowProject.Controllers
{
    public class WorkController : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var demandeur = db.Notifications.Where( n => n.Etat == false && n.Role == SecurityRole.Demandeur).ToList();
            var respacha = db.Notifications.Where( n => n.Etat == false && n.Role == SecurityRole.RespAcha).ToList();

            var daf = db.Notifications.Where(n => n.Etat == false && n.Role == SecurityRole.DAF).ToList();

            var audit = db.Notifications.Where(n => n.Etat == false && n.Role == SecurityRole.AUDIT).ToList();

            var dg = db.Notifications.Where(n => n.Etat == false && n.Role == SecurityRole.DG).ToList();

            ViewBag.Demandeur = demandeur;
            ViewBag.RespAcha = respacha;
            ViewBag.DAF = daf;
            ViewBag.AUDIT = audit;
            ViewBag.DG = dg;

            base.OnActionExecuting(filterContext);
        }
    }
}