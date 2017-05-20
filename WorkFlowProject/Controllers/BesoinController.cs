using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WorkFlowProject.Models;

namespace WorkFlowProject.Controllers
{
    [Authorize]
    public class BesoinController : WorkController
    {

        public async Task<ActionResult> Notification(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notification n = await db.Notifications.FindAsync(id);
            if (n == null)
            {
                return HttpNotFound();
            }


            n.Etat = true;
            db.Entry(n).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return RedirectToAction("IndexDem");
        }

        public async Task<ActionResult> Accept(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Achat achat = await db.Achats.FindAsync(id);
            if (achat == null)
            {
                return HttpNotFound();
            }

            achat.Type = TypeDemande.Demande;
            achat.State = StateDemande.DemandeCreer;
            ApplicationUser current = db.Users.Where(u => u.UserName == User.Identity.Name).First();
            Notification notif = new Notification { Achat = achat, Lbl = "Demande Confirmer Par : " + current.Login, Role = SecurityRole.Demandeur };
            db.Entry(achat).State = EntityState.Modified;
            db.Notifications.Add(notif);
            await db.SaveChangesAsync();

            return RedirectToAction("IndexDem");
        }

        public async Task<ActionResult> Reject(int? id)
        {


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Achat achat = await db.Achats.FindAsync(id);
            if (achat == null)
            {
                return HttpNotFound();
            }


            achat.State = StateDemande.BesoinRefuser;
            ApplicationUser current = db.Users.Where(u => u.UserName == User.Identity.Name).First();
            Notification notif = new Notification { Achat = achat, Lbl = "Demande Refuser Par : " + current.Login, Role = SecurityRole.Demandeur };
            db.Entry(achat).State = EntityState.Modified;
            db.Notifications.Add(notif);
            await db.SaveChangesAsync();

            return RedirectToAction("IndexDem");
        }

        public async Task<ActionResult> Index()
        {
            ApplicationUser current = db.Users.Where(u => u.UserName == User.Identity.Name).First();
            var achats = db.Achats.Include(a => a.Department).Where(a => a.Type == TypeDemande.Besoin && a.DepartmentID == current.DepartmentID && a.State == StateDemande.BesoinCreer);
            return View(await achats.ToListAsync());
        }

        public async  Task<ActionResult> IndexDem()
        {
            return View("Index",await db.Achats.Where(a => a.Type == TypeDemande.Besoin && a.State == StateDemande.BesoinCreer ).ToListAsync());
        }


        public async Task<ActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Achat Achat = await db.Achats.FindAsync(id);
            if (Achat == null)
            {
                return HttpNotFound();
            }
            return View(new Demande { Achat = Achat });
        }

        public ActionResult Ajout()
        {
            List<string> lieu = new List<string>();
            lieu.Add("Cité el kahdra");
            lieu.Add("siege lac");
            lieu.Add("el Agba");

            ViewBag.LieuLiv = new SelectList(lieu);
            ViewBag.DepartmentID = new SelectList(db.Departments, "ID", "Dep");
            ViewBag.Categ = new SelectList(db.Categories, "Lbl", "Lbl");
            ApplicationUser current = db.Users.Where(u => u.UserName == User.Identity.Name).First();

            ViewBag.depd = current.Department.Budget;
            ViewBag.depdt = current.Department.Depense;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Ajout([Bind(Include = "ID,Des,Categ,Creation,LieuLiv,Imp,Qte")] Achat achat)
        {
            ApplicationUser current = db.Users.Where(u => u.UserName == User.Identity.Name).First();
            achat.DepartmentID = current.DepartmentID;
            achat.Department = current.Department;


            

            if (ModelState.IsValid)
            {
                if (achat.Department.Depense >= achat.Department.Budget)
                {
                    ModelState.AddModelError("depdt", "Solde insufisant !");
                    ViewBag.Categ = new SelectList(db.Categories, "Lbl", "Lbl", achat.Categ);

                    ViewBag.depd = current.Department.Budget;
                    ViewBag.depdt = current.Department.Depense;

                    List<string> lieu = new List<string>();
                    lieu.Add("Cité el kahdra");
                    lieu.Add("siege lac");
                    lieu.Add("el Agba");

                    ViewBag.LieuLiv = new SelectList(lieu);

                    return View(achat);
                }

                db.Achats.Add(achat);
                Notification notif = new Notification { Achat = achat, Lbl = "Besoin Créer Par : " + current.Login , Role = SecurityRole.RespAcha  };
                db.Notifications.Add(notif);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Categ = new SelectList(db.Categories, "Lbl", "Lbl", achat.Categ);

            ViewBag.depd = current.Department.Budget;
            ViewBag.depdt = current.Department.Depense;

            return View(achat);
        }

        // GET: Besoin/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Achat achat = await db.Achats.FindAsync(id);
            if (achat == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentID = new SelectList(db.Departments, "ID", "Dep", achat.DepartmentID);
            return View(achat);
        }

        // POST: Besoin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,DepartmentID,Des,Categ,DtAcha,Creation,LieuLiv,Imp,Qte,Type,State")] Achat achat)
        {
            if (ModelState.IsValid)
            {

                db.Entry(achat).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentID = new SelectList(db.Departments, "ID", "Dep", achat.DepartmentID);
            return View(achat);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}