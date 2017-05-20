using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkFlowProject.Models;

namespace WorkFlowProject.Controllers
{
    public class StatController : WorkController
    {
        // GET: Stat
        public ActionResult Index()
        {
            ViewBag.choi = new SelectList(db.Departments, "ID", "Dep");
            return View();
        }

        public JsonResult CalcStat(StatGenerale state)
        {

            return null;
        }
    }
}