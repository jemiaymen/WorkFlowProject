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
    [Authorize(Roles = SecurityRole.AUDIT + "," + SecurityRole.DAF + "," + SecurityRole.DG )]
    public class AvisController : WorkController
    {
        #region WebService Rest API

        [HttpGet]
        public JsonResult Api()
        {
            ApplicationUser current = db.Users.Where(u => u.UserName == User.Identity.Name).First();

            if (User.IsInRole(SecurityRole.Demandeur))
            {
                List<Demande> dem = new List<Demande>();

                var achats = db.Achats.Where(a => a.DepartmentID == current.DepartmentID && (a.State == StateDemande.DemandeRefuser || a.State == StateDemande.DemandeConfirmerFournisseur || a.State == StateDemande.DemandeConfirmerDaf || a.State == StateDemande.DemandeConfirmerAudit || a.State == StateDemande.DemandeAccepter)).ToList();
                foreach (Achat ach in achats)
                {
                    Demande d = new Demande { Achat = ach, AF = db.AchaFournisseurs.Where(a => a.AchatID == ach.ID).ToList(), Avis = db.Avis.Where(a => a.AchatID == ach.ID).ToList() };
                    dem.Add(d);

                }

                return Json(dem, JsonRequestBehavior.AllowGet);
            }else if(User.IsInRole(SecurityRole.RespAcha))
            {
                List<Demande> dem = new List<Demande>();

                var achats = db.Achats.Where(a => a.State == StateDemande.DemandeRefuser || a.State == StateDemande.DemandeConfirmerFournisseur || a.State == StateDemande.DemandeConfirmerDaf || a.State == StateDemande.DemandeConfirmerAudit || a.State == StateDemande.DemandeAccepter).ToList();
                foreach (Achat ach in achats)
                {
                    Demande d = new Demande { Achat = ach, AF = db.AchaFournisseurs.Where(a => a.AchatID == ach.ID).ToList(), Avis = db.Avis.Where(a => a.AchatID == ach.ID).ToList() };
                    dem.Add(d);

                }

                return Json(dem, JsonRequestBehavior.AllowGet);
            }else if(User.IsInRole(SecurityRole.DAF))
            {
                List<Demande> dem = new List<Demande>();

                var achats = db.Achats.Where(a => a.State == StateDemande.DemandeConfirmerFournisseur).ToList();
                foreach (Achat ach in achats)
                {
                    Demande d = new Demande { Achat = ach, AF = db.AchaFournisseurs.Where(a => a.AchatID == ach.ID).ToList(), Avis = db.Avis.Where(a => a.AchatID == ach.ID).ToList() };
                    dem.Add(d);

                }

                return Json(dem, JsonRequestBehavior.AllowGet);
            }
            else if (User.IsInRole(SecurityRole.AUDIT))
            {
                List<Demande> dem = new List<Demande>();

                var achats = db.Achats.Where(a => a.State == StateDemande.DemandeConfirmerDaf).ToList();
                foreach (Achat ach in achats)
                {
                    Demande d = new Demande { Achat = ach, AF = db.AchaFournisseurs.Where(a => a.AchatID == ach.ID).ToList(), Avis = db.Avis.Where(a => a.AchatID == ach.ID).ToList() };
                    dem.Add(d);

                }

                return Json(dem, JsonRequestBehavior.AllowGet);
            }
            else if (User.IsInRole(SecurityRole.DG))
            {
                List<Demande> dem = new List<Demande>();

                var achats = db.Achats.Where(a => a.State == StateDemande.DemandeConfirmerAudit).ToList();
                foreach (Achat ach in achats)
                {
                    Demande d = new Demande { Achat = ach, AF = db.AchaFournisseurs.Where(a => a.AchatID == ach.ID).ToList(), Avis = db.Avis.Where(a => a.AchatID == ach.ID).ToList() };
                    dem.Add(d);

                }

                return Json(dem, JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }
        

        [HttpPost]
        public bool ApiAccept(int id, string lbl)
        {
            Achat a = db.Achats.Find(id);

            if (a == null)
            {
                return false;
            }
            ApplicationUser current = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (User.IsInRole(SecurityRole.DAF))
            {
                Models.Avis avis = new Models.Avis { AchatID = id, Accept = true, Lbl = lbl, Code = AvisCode.daf , User = current };
                a.State = StateDemande.DemandeConfirmerDaf;
                db.Entry(a).State = System.Data.Entity.EntityState.Modified;

                db.Avis.Add(avis);

                db.SaveChanges();
                return true;

            }else if(User.IsInRole(SecurityRole.AUDIT))
            {
                Models.Avis avis = new Models.Avis { AchatID = id, Accept = true, Lbl = lbl, Code = AvisCode.audit, User = current };
                a.State = StateDemande.DemandeConfirmerAudit;
                db.Entry(a).State = System.Data.Entity.EntityState.Modified;

                db.Avis.Add(avis);

                db.SaveChanges();
                return true;
            }else if(User.IsInRole(SecurityRole.DG))
            {
                Models.Avis avis = new Models.Avis { AchatID = id, Accept = true, Lbl = lbl, Code = AvisCode.dg, User = current };
                a.State = StateDemande.DemandeAccepter;
                db.Entry(a).State = System.Data.Entity.EntityState.Modified;

                db.Avis.Add(avis);

                db.SaveChanges();
                return true;
            }

            return false;
        }

        [HttpPost]
        public bool ApiDeny(int id, string lbl)
        {
            Achat a = db.Achats.Find(id);

            if (a == null)
            {
                return false;
            }

            ApplicationUser current = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            if (User.IsInRole(SecurityRole.DAF))
            {
                Models.Avis avis = new Models.Avis { AchatID = id, Accept = false, Lbl = lbl, Code = AvisCode.daf, User = current };
                a.State = StateDemande.DemandeRefuser;
                db.Entry(a).State = System.Data.Entity.EntityState.Modified;

                db.Avis.Add(avis);

                db.SaveChanges();
                return true;

            }
            else if (User.IsInRole(SecurityRole.AUDIT))
            {
                Models.Avis avis = new Models.Avis { AchatID = id, Accept = false, Lbl = lbl, Code = AvisCode.audit, User = current };
                a.State = StateDemande.DemandeRefuser;
                db.Entry(a).State = System.Data.Entity.EntityState.Modified;

                db.Avis.Add(avis);

                db.SaveChanges();
                return true;
            }
            else if (User.IsInRole(SecurityRole.DG))
            {
                Models.Avis avis = new Models.Avis { AchatID = id, Accept = false, Lbl = lbl, Code = AvisCode.dg, User = current };
                a.State = StateDemande.DemandeRefuser;
                db.Entry(a).State = System.Data.Entity.EntityState.Modified;

                db.Avis.Add(avis);

                db.SaveChanges();
                return true;
            }

            return false;
        }

        #endregion


        public ActionResult Index()
        {
            ApplicationUser current = db.Users.Where(u => u.UserName == User.Identity.Name).First();

            if (User.IsInRole(SecurityRole.Demandeur))
            {

                var achats = db.Achats.Include(a => a.Department).Where(a => a.DepartmentID == current.DepartmentID && (a.State == StateDemande.DemandeRefuser || a.State == StateDemande.DemandeConfirmerFournisseur || a.State == StateDemande.DemandeConfirmerDaf || a.State == StateDemande.DemandeConfirmerAudit || a.State == StateDemande.DemandeAccepter));
                return View(achats.ToList());
            }
            else if (User.IsInRole(SecurityRole.RespAcha))
            {
                var achats = db.Achats.Include(a => a.Department).Where(a => a.State == StateDemande.DemandeRefuser || a.State == StateDemande.DemandeConfirmerFournisseur || a.State == StateDemande.DemandeConfirmerDaf || a.State == StateDemande.DemandeConfirmerAudit || a.State == StateDemande.DemandeAccepter).ToList();
                return View(achats);
            }
            else if (User.IsInRole(SecurityRole.DAF))
            {
                var achats = db.Achats.Include(a => a.Department).Where(a => a.State == StateDemande.DemandeConfirmerFournisseur).ToList();
                return View(achats);
            }
            else if (User.IsInRole(SecurityRole.AUDIT))
            {
                var achats = db.Achats.Include(a => a.Department).Where(a => a.State == StateDemande.DemandeConfirmerDaf).ToList();
                return View(achats);
            }
            else if (User.IsInRole(SecurityRole.DG))
            {
                var achats = db.Achats.Include(a => a.Department).Where(a => a.State == StateDemande.DemandeConfirmerAudit).ToList();
                return View(achats);
            }

            return View();
        }

        public async Task<ActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Achat acha = await db.Achats.FindAsync(id);
            if (acha == null)
            {
                return HttpNotFound();
            }

           
            var af = db.AchaFournisseurs.Where(a => a.AchatID == acha.ID).ToList();

            var av = db.Avis.Where(a => a.AchatID == acha.ID).ToList();


            return View(new Demande { Achat = acha, AF = af, Avis = av  });

        }

        [HttpPost]
        public ActionResult Accept(int? id, string lbl)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Achat a = db.Achats.Find(id);

            if (a == null)
            {
                return HttpNotFound();
            }

            ApplicationUser current = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (User.IsInRole(SecurityRole.DAF))
            {
                Models.Avis avis = new Models.Avis { AchatID = id.GetValueOrDefault(), Accept = true, Lbl = lbl, Code = AvisCode.daf, User = current  };
                a.State = StateDemande.DemandeConfirmerDaf;
                db.Entry(a).State = System.Data.Entity.EntityState.Modified;

                db.Avis.Add(avis);
                Notification notif = new Notification { AchatID = avis.AchatID, Role = SecurityRole.AUDIT, Lbl = "Demande Confirmer par DAF : " + current.UserName };

                db.Notifications.Add(notif);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            else if (User.IsInRole(SecurityRole.AUDIT))
            {
                Models.Avis avis = new Models.Avis { AchatID = id.GetValueOrDefault(), Accept = true, Lbl = lbl, Code = AvisCode.audit, User = current };
                a.State = StateDemande.DemandeConfirmerAudit;
                db.Entry(a).State = System.Data.Entity.EntityState.Modified;

                db.Avis.Add(avis);
                Notification notif = new Notification { AchatID = avis.AchatID, Role = SecurityRole.DG, Lbl = "Demande Confirmer par Audit : " + current.UserName };

                db.Notifications.Add(notif);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else if (User.IsInRole(SecurityRole.DG))
            {
                Models.Avis avis = new Models.Avis { AchatID = id.GetValueOrDefault(), Accept = true, Lbl = lbl, Code = AvisCode.dg, User = current };
                a.State = StateDemande.DemandeAccepter;
                db.Entry(a).State = System.Data.Entity.EntityState.Modified;

                db.Avis.Add(avis);
                Notification notif = new Notification { AchatID = avis.AchatID, Role = SecurityRole.Demandeur, Lbl = "Demande Confirmer par DG : " + current.UserName };
                Notification notif1 = new Notification { AchatID = avis.AchatID, Role = SecurityRole.RespAcha, Lbl = "Demande Confirmer par DG : " + current.UserName };

                db.Notifications.Add(notif);
                db.Notifications.Add(notif1);

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Deny(int? id, string lbl)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Achat a = db.Achats.Find(id);

            if (a == null)
            {
                return HttpNotFound();
            }

            ApplicationUser current = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            if (User.IsInRole(SecurityRole.DAF))
            {
                Models.Avis avis = new Models.Avis { AchatID = id.GetValueOrDefault(), Accept = false, Lbl = lbl, Code = AvisCode.daf , User = current};
                a.State = StateDemande.DemandeRefuser;
                db.Entry(a).State = System.Data.Entity.EntityState.Modified;

                db.Avis.Add(avis);
                Notification notif = new Notification { AchatID = avis.AchatID, Role = SecurityRole.RespAcha, Lbl = "Demande Refuser par : " + current.UserName };
                Notification notif1 = new Notification { AchatID = avis.AchatID, Role = SecurityRole.Demandeur, Lbl = "Demande Refuser par : " + current.UserName };

                db.Notifications.Add(notif);
                db.Notifications.Add(notif1);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            else if (User.IsInRole(SecurityRole.AUDIT))
            {
                Models.Avis avis = new Models.Avis { AchatID = id.GetValueOrDefault(), Accept = false, Lbl = lbl, Code = AvisCode.audit, User = current };
                a.State = StateDemande.DemandeRefuser;
                db.Entry(a).State = System.Data.Entity.EntityState.Modified;

                db.Avis.Add(avis);
                Notification notif = new Notification { AchatID = avis.AchatID, Role = SecurityRole.RespAcha, Lbl = "Demande Refuser par : " + current.UserName };
                Notification notif1 = new Notification { AchatID = avis.AchatID, Role = SecurityRole.Demandeur, Lbl = "Demande Refuser par : " + current.UserName };

                db.Notifications.Add(notif);
                db.Notifications.Add(notif1);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else if (User.IsInRole(SecurityRole.DG))
            {
                Models.Avis avis = new Models.Avis { AchatID = id.GetValueOrDefault(), Accept = false, Lbl = lbl, Code = AvisCode.dg, User = current };
                a.State = StateDemande.DemandeRefuser;
                db.Entry(a).State = System.Data.Entity.EntityState.Modified;

                db.Avis.Add(avis);
                Notification notif = new Notification { AchatID = avis.AchatID, Role = SecurityRole.RespAcha, Lbl = "Demande Refuser par : " + current.UserName };
                Notification notif1 = new Notification { AchatID = avis.AchatID, Role = SecurityRole.Demandeur, Lbl = "Demande Refuser par : " + current.UserName };

                db.Notifications.Add(notif);
                db.Notifications.Add(notif1);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
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

            n.Etat = true;
            db.Entry(n).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return RedirectToAction("Detail", new { id = n.AchatID });


        }

        #region helper

        public JsonResult CalcStat(int id)
        {
            List<IDictionary<string, int>> result = new List<IDictionary<string, int>>();

            var res = db.Achats.Where(a => a.DepartmentID == id).ToList();

            foreach(var r in res.Select(a => a.Categ).Distinct() )
            {
                IDictionary<string, int> re = new Dictionary<string, int>();
                re.Add(r, res.Where(a => a.Categ == r).Count());
                result.Add(re);
            }
            return Json(result , JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}