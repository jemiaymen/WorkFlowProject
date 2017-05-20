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
    public class DemandeOldController : WorkController
    {
        public async Task<ActionResult> Index()
        {

            var achat = await db.Notifications.Where(a => a.Achat.Type == TypeDemande.Demande && a.Achat.State == StateDemande.DemandeCreer && a.Role == SecurityRole.RespAcha).ToListAsync();
            return View(achat);
        }

        public ActionResult Modification()
        {
            ApplicationUser current = db.Users.Where(u => u.UserName == User.Identity.Name).First();
            var achats = db.Achats.Include(a => a.Department).Where(a => a.Type == TypeDemande.Demande && a.DepartmentID == current.DepartmentID &&  a.State == StateDemande.DemandeCreer  );
            ViewBag.AchatID = new SelectList(achats, "ID", "Des");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modification([Bind(Include = "ID,AchatID,Lbl")] Notification notification)
        {
            notification.Role = SecurityRole.RespAcha;
            notification.Etat = false;
            notification.Dt = DateTime.Now;

            if (ModelState.IsValidField("Lbl"))
            {
                Achat a = db.Achats.Find(notification.AchatID);
                a.State = StateDemande.DemandeAModifier;
                db.Entry(a).State = EntityState.Modified;
                db.SaveChanges();
                db.Notifications.Add(notification);
                db.SaveChanges();
                return RedirectToAction("Modification");
            }

            ApplicationUser current = db.Users.Where(u => u.UserName == User.Identity.Name).First();
            var achats = db.Achats.Include(a => a.Department).Where(a => a.Type == TypeDemande.Demande && a.DepartmentID == current.DepartmentID);

            ViewBag.AchatID = new SelectList(achats, "ID", "Des",notification.AchatID);
            return View(notification);
        }

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

            if (n.Achat.State == StateDemande.DemandeAModifier)
            {
                return View(new Demande { Achat = n.Achat, Notification = n });
            }

            return View("Detail", new Demande { Achat = n.Achat, Notification = n });
            
        }

        public async Task<ActionResult> Edit(int? id)
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
            ViewBag.DepartmentID = new SelectList(db.Departments, "ID", "Dep", n.Achat.DepartmentID);
            return View(n);
        }

        // POST: Besoin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,DepartmentID,Des,Categ,DtAcha,Creation,LieuLiv,Imp,Qte,Type,State")] Achat achat,int nid)
        {
            if (ModelState.IsValid)
            {
                Notification n = await db.Notifications.FindAsync(nid);
                n.Etat = true;

                achat.State = StateDemande.DemandeCreer;

                db.Entry(n).State = EntityState.Modified;
                await db.SaveChangesAsync();

                db.Entry(achat).State = EntityState.Modified;
                await db.SaveChangesAsync();
                
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentID = new SelectList(db.Departments, "ID", "Dep", achat.DepartmentID);
            return View(achat);
        }


        public async Task<ActionResult> Detail(int? id)
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

            var four = db.CategoryInFournisseur.Where(c => c.Category.Lbl == n.Achat.Categ).Select(f => f.Fournisseur).Distinct().ToList();

            var af = db.AchaFournisseurs.Where(a => a.AchatID == n.AchatID).ToList();

            var av = db.Avis.Where(a => a.AchatID == n.AchatID).ToList();

            ViewBag.FournisseurID = new SelectList(four, "ID", "Nom_frn");

            return View("Detail", new Demande { Achat = n.Achat, Notification = n , AF = af , Avis = av });

        }


        [HttpPost]
        public async Task<ActionResult> FEdit([Bind(Include = "AchatID,FournisseurID,Price,Remise,Delais,State")] AchaFournisseur af,int nid,int fid)
        {
            if (ModelState.IsValid)
            {
                af.ID = fid;
                db.Entry(af).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }

            return RedirectToAction("Detail", new { id = nid });
        }
         

        [HttpPost]
        public async Task<ActionResult> FAdd([Bind(Include = "AchatID,FournisseurID,Price,Remise,Delais")] AchaFournisseur af,int achatid,int fournisseurid, int nid)
        {
            if (ModelState.IsValid && ApteAddFournisseur(achatid) && ExistFournisseur(achatid,fournisseurid) )
            {
                af.AchatID = achatid;
                af.FournisseurID = fournisseurid;
                db.AchaFournisseurs.Add(af);
                await db.SaveChangesAsync();
            }

            return RedirectToAction("Detail", new { id = nid });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Submit(int? id,string lbl)
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

            n.Achat.State = StateDemande.DemandeAConfirmerFournisseur;
            db.Entry(n).State = EntityState.Modified;
            await db.SaveChangesAsync();

            Notification notif = new Notification { AchatID = n.AchatID , Role = SecurityRole.Demandeur  , Lbl = "Choisir Fournisseur Pour Demande : " + n.Achat.Des  };

            db.Notifications.Add(notif);
            ApplicationUser current = db.Users.Where(u => u.UserName == User.Identity.Name).First();
            var A = new Models.Avis {AchatID = n.AchatID , Accept = true, Lbl  = lbl , User = current , Code = AvisCode.init  };

            db.Avis.Add(A);

            await db.SaveChangesAsync();


            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Fournisseur(int? id,int? fid,string lbl)
        {
                Notification notification = await db.Notifications.FindAsync(id);
                AchaFournisseur af = await db.AchaFournisseurs.FindAsync(fid);
                Achat a = await db.Achats.FindAsync(notification.AchatID);
                ApplicationUser current = db.Users.Where(u => u.UserName == User.Identity.Name).First();

                if (notification == null || af == null)
                {
                    return HttpNotFound();
                }

                notification.Etat = true;

                af.State = StateFournisseur.Chose;

                a.State = StateDemande.DemandeConfirmerFournisseur;

                db.Entry(notification).State = EntityState.Modified;

                db.Entry(af).State = EntityState.Modified;

                db.Entry(a).State = EntityState.Modified;


                var A = new Models.Avis { AchatID = notification.AchatID, Accept = true, Lbl = lbl, User = current , Code = AvisCode.demandeur };

                db.Avis.Add(A);

                await db.SaveChangesAsync();

                return RedirectToAction("Fournisseur");

          
            
        }



        public async Task<ActionResult> Fournisseur(int? id)
        {
            if (id == null)
            {
                ApplicationUser current = db.Users.Where(u => u.UserName == User.Identity.Name).First();
                var notif = await db.Notifications.Where(n => n.Etat == false && n.Achat.DepartmentID == current.DepartmentID && n.Achat.Type == TypeDemande.Demande && n.Achat.State == StateDemande.DemandeAConfirmerFournisseur).ToListAsync();
                return View("Index", notif);
            }
            Notification notification = await db.Notifications.FindAsync(id);
            return RedirectToAction("Detail", new { id = notification.ID });

        }

        #region helper
        private bool ApteAddFournisseur(int? id)
        {
            if (id == null)
            {
                return false;
            }

            var af = db.AchaFournisseurs.Where(a => a.AchatID == id).ToList();

            return (af.Count < 3);
        }

        private bool ExistFournisseur(int? aid, int? fid)
        {
            if (aid == null || fid == null)
            {
                return false;
            }

            try
            {
                var af = db.AchaFournisseurs.Where(a => a.AchatID == aid && a.FournisseurID == fid).First();
                return false;
            }
            catch (Exception )
            {
                return true;
            }
        }

        

        #endregion

    }
    
}