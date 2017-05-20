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
using Microsoft.AspNet.Identity.EntityFramework;
namespace WorkFlowProject.Controllers
{
    public class DemandeController : WorkController
    {

        #region ends

        public ActionResult Avancement()
        {
            if (User.IsInRole(SecurityRole.Demandeur))
            {
                IList<Avancement> avancement = new List<Avancement>();

                ApplicationUser current = db.Users.Where(u => u.UserName == User.Identity.Name).First();
                var achat = db.Achats.Where(a => a.DepartmentID == current.DepartmentID).ToList();


                foreach (var n in achat)
                {
                    if (n.State == StateDemande.DemandeAccepter)
                    {
                        avancement.Add(new Avancement { Achat = n, Dg = true });
                    }
                    else if (n.State == StateDemande.DemandeConfirmerDaf)
                    {
                        avancement.Add(new Avancement { Achat = n, Daf = true });
                    }



                    switch (n.State)
                    {
                        case StateDemande.DemandeAccepter:
                            {
                                avancement.Add(new Avancement { Achat = n, Dg = true });
                            } break;
                        case StateDemande.BesoinCreer:
                            {
                                avancement.Add(new Avancement { Achat = n, Dem = true });
                            } break;
                        case StateDemande.DemandeAConfirmerFournisseur:
                            {
                                avancement.Add(new Avancement { Achat = n, Dem = true });
                            } break;
                        case StateDemande.DemandeAModifier:
                            {
                                avancement.Add(new Avancement { Achat = n, RespA = true });
                            } break;
                        case StateDemande.DemandeConfirmerAudit:
                            {
                                avancement.Add(new Avancement { Achat = n, Audit = true });
                            } break;
                        case StateDemande.DemandeConfirmerDaf:
                            {
                                avancement.Add(new Avancement { Achat = n, Daf = true });
                            } break;
                        case StateDemande.DemandeConfirmerFournisseur:
                            {
                                avancement.Add(new Avancement { Achat = n, Dem = true });
                            } break;
                        case StateDemande.DemandeCreer:
                            {
                                avancement.Add(new Avancement { Achat = n, RespA = true });
                            } break;
                        case StateDemande.BesoinRefuser:
                            {
                                avancement.Add(new Avancement { Achat = n, RespA = true, valid = false });
                            } break;
                        case StateDemande.DemandeRefuser:
                            {
                                WorkFlowProject.Models.Avis av = db.Avis.Where(a => a.AchatID == n.ID && a.Accept == false).First();

                                switch (av.Code)
                                {
                                    case AvisCode.init:
                                        {
                                            avancement.Add(new Avancement { Achat = n, RespA = true, valid = false });
                                        } break;

                                    case AvisCode.demandeur:
                                        {
                                            avancement.Add(new Avancement { Achat = n, Dem = true, valid = false });
                                        } break;
                                    case AvisCode.audit:
                                        {
                                            avancement.Add(new Avancement { Achat = n, Audit = true, valid = false });
                                        } break;
                                    case AvisCode.daf:
                                        {
                                            avancement.Add(new Avancement { Achat = n, Daf = true, valid = false });
                                        } break;
                                    case AvisCode.dg:
                                        {
                                            avancement.Add(new Avancement { Achat = n, Dg = true, valid = false });
                                        } break;
                                }

                            } break;

                    }

                }


                return View(avancement);

            }
            else if (User.IsInRole(SecurityRole.RespAcha))
            {
                IList<Avancement> avancement = new List<Avancement>();

                var achat = db.Achats.ToList();


                foreach (var n in achat)
                {
                    if (n.State == StateDemande.DemandeAccepter)
                    {
                        avancement.Add(new Avancement { Achat = n, Dg = true });
                    }
                    else if (n.State == StateDemande.DemandeConfirmerDaf)
                    {
                        avancement.Add(new Avancement { Achat = n, Daf = true });
                    }



                    switch (n.State)
                    {
                        case StateDemande.DemandeAccepter:
                            {
                                avancement.Add(new Avancement { Achat = n, Dg = true });
                            } break;
                        case StateDemande.BesoinCreer:
                            {
                                avancement.Add(new Avancement { Achat = n, Dem = true });
                            } break;
                        case StateDemande.DemandeAConfirmerFournisseur:
                            {
                                avancement.Add(new Avancement { Achat = n, Dem = true });
                            } break;
                        case StateDemande.DemandeAModifier:
                            {
                                avancement.Add(new Avancement { Achat = n, RespA = true });
                            } break;
                        case StateDemande.DemandeConfirmerAudit:
                            {
                                avancement.Add(new Avancement { Achat = n, Audit = true });
                            } break;
                        case StateDemande.DemandeConfirmerDaf:
                            {
                                avancement.Add(new Avancement { Achat = n, Daf = true });
                            } break;
                        case StateDemande.DemandeConfirmerFournisseur:
                            {
                                avancement.Add(new Avancement { Achat = n, Dem = true });
                            } break;
                        case StateDemande.DemandeCreer:
                            {
                                avancement.Add(new Avancement { Achat = n, RespA = true });
                            } break;
                        case StateDemande.BesoinRefuser:
                            {
                                avancement.Add(new Avancement { Achat = n, RespA = true, valid = false });
                            } break;
                        case StateDemande.DemandeRefuser:
                            {
                                WorkFlowProject.Models.Avis av = db.Avis.Where(a => a.AchatID == n.ID && a.Accept == false).First();

                                switch (av.Code)
                                {
                                    case AvisCode.init:
                                        {
                                            avancement.Add(new Avancement { Achat = n, RespA = true, valid = false });
                                        } break;

                                    case AvisCode.demandeur:
                                        {
                                            avancement.Add(new Avancement { Achat = n, Dem = true, valid = false });
                                        } break;
                                    case AvisCode.audit:
                                        {
                                            avancement.Add(new Avancement { Achat = n, Audit = true, valid = false });
                                        } break;
                                    case AvisCode.daf:
                                        {
                                            avancement.Add(new Avancement { Achat = n, Daf = true, valid = false });
                                        } break;
                                    case AvisCode.dg:
                                        {
                                            avancement.Add(new Avancement { Achat = n, Dg = true, valid = false });
                                        } break;
                                }

                            } break;

                    }

                }


                return View(avancement);
            }
            return View();
        }


        public ActionResult Historique()
        {
            if (User.IsInRole(SecurityRole.Demandeur))
            {
                Historique h = new Historique();
                ApplicationUser current = db.Users.Where(u => u.UserName == User.Identity.Name).First();
                var accept = db.Achats.Where(a => a.DepartmentID == current.DepartmentID && a.State == StateDemande.DemandeAccepter).ToList();
                var refu = db.Achats.Where(a => a.DepartmentID == current.DepartmentID && a.State == StateDemande.DemandeRefuser).ToList();

                IList<Refused> refused = new List<Refused>();


                foreach (var r in refu)
                {
                    refused.Add(new Refused { Avis = db.Avis.Where(a => a.AchatID == r.ID && a.Accept == false).First(), Achat = r });
                }

                h.Refused = refused;
                h.Accepted = accept;

                return View(h);

            }
            else if (User.IsInRole(SecurityRole.RespAcha))
            {
                Historique h = new Historique();
                var accept = db.Achats.Where(a => a.State == StateDemande.DemandeAccepter).ToList();
                var refu = db.Achats.Where(a => a.State == StateDemande.DemandeRefuser).ToList();

                IList<Refused> refused = new List<Refused>();


                foreach (var r in refu)
                {
                    refused.Add(new Refused { Avis = db.Avis.Where(a => a.AchatID == r.ID && a.Accept == false).First(), Achat = r });
                }

                h.Refused = refused;
                h.Accepted = accept;

                return View(h);
            }
            return View();
        }


        public async Task<ActionResult> Finau()
        {
            return View("Index", await db.Achats.Where(a => a.Type == TypeDemande.Demande && a.State == StateDemande.DemandeAccepter).ToListAsync());
        }

        public async Task<ActionResult> BC(int? id)
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

            achat.Type = TypeDemande.BonCommande;
            db.Entry(achat).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return RedirectToAction("Finau");

        }

        public async Task<ActionResult> Index()
        {
            if (User.IsInRole(SecurityRole.Demandeur))
            {
                ApplicationUser current = db.Users.Where(u => u.UserName == User.Identity.Name).First();
                return View(await db.Achats.Where(a => a.DepartmentID == current.DepartmentID && a.Type == TypeDemande.Demande && a.State != StateDemande.DemandeAccepter).ToListAsync());
            }
            else
            {
                return View(await db.Achats.Where(a => a.Type == TypeDemande.Demande && a.State != StateDemande.DemandeAccepter).ToListAsync());
            }
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

        public async Task<ActionResult> Detail(int? id)
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

            var four = db.CategoryInFournisseur.Where(c => c.Category.Lbl == achat.Categ).Select(f => f.Fournisseur).Distinct().ToList();

            var af = db.AchaFournisseurs.Where(a => a.AchatID == achat.ID).ToList();

            var av = db.Avis.Where(a => a.AchatID == achat.ID).ToList();

            ViewBag.FournisseurID = new SelectList(four, "ID", "Nom_frn");

            return View(new Demande { Achat = achat, AF = af, Avis = av });

        }

        public async Task<ActionResult> Del(int? id)
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

            db.Achats.Remove(achat);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Achat n = await db.Achats.FindAsync(id);
            if (n == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentID = new SelectList(db.Departments, "ID", "Dep", n.DepartmentID);
            return View(n);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,DepartmentID,Des,Categ,DtAcha,Creation,LieuLiv,Imp,Qte,Type,State")] Achat achat)
        {
            if (ModelState.IsValid)
            {

                achat.State = StateDemande.DemandeCreer;
                db.Entry(achat).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Detail", new { id = achat.ID });
            }
            ViewBag.DepartmentID = new SelectList(db.Departments, "ID", "Dep", achat.DepartmentID);
            return View(achat);
        }

        [HttpPost]
        public async Task<ActionResult> FEdit([Bind(Include = "AchatID,FournisseurID,Price,Remise,Delais,State")] AchaFournisseur af, int nid, int fid)
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
        public async Task<ActionResult> FAdd([Bind(Include = "AchatID,FournisseurID,Price,Remise,Delais")] AchaFournisseur af, int achatid, int fournisseurid)
        {
            if (ModelState.IsValid && ApteAddFournisseur(achatid) && ExistFournisseur(achatid, fournisseurid) && af.Price > af.Remise)
            {
                af.AchatID = achatid;
                af.FournisseurID = fournisseurid;
                db.AchaFournisseurs.Add(af);
                await db.SaveChangesAsync();
            }

            return RedirectToAction("Detail", new { id = achatid });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Submit(int? id, string lbl)
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

            achat.State = StateDemande.DemandeAConfirmerFournisseur;
            db.Entry(achat).State = EntityState.Modified;
            await db.SaveChangesAsync();

            Notification notif = new Notification { AchatID = achat.ID, Role = SecurityRole.Demandeur, Lbl = "vous devez saisir votre avis technique et choisir un fournisseur Pour la Demande : " + achat.Des };

            db.Notifications.Add(notif);
            ApplicationUser current = db.Users.Where(u => u.UserName == User.Identity.Name).First();
            var A = new Models.Avis { AchatID = achat.ID, Accept = true, Lbl = lbl, User = current, Code = AvisCode.init };

            db.Avis.Add(A);

            await db.SaveChangesAsync();


            return RedirectToAction("Detail", new { id = achat.ID });
        }

        [HttpPost]
        public async Task<ActionResult> Fournisseur(int? id, int? fid, string lbl)
        {
            AchaFournisseur af = await db.AchaFournisseurs.FindAsync(fid);
            Achat a = await db.Achats.FindAsync(id);
            ApplicationUser current = db.Users.Where(u => u.UserName == User.Identity.Name).First();

            if (a == null || af == null)
            {
                return HttpNotFound();
            }


            af.State = StateFournisseur.Chose;

            a.State = StateDemande.DemandeConfirmerFournisseur;

            db.Entry(af).State = EntityState.Modified;

            db.Entry(a).State = EntityState.Modified;


            var A = new Models.Avis { AchatID = a.ID, Accept = true, Lbl = lbl, User = current, Code = AvisCode.demandeur };

            Notification notif = new Notification { AchatID = af.AchatID, Role = SecurityRole.DAF, Lbl = "Demande choisir Fournisseur" };

            db.Notifications.Add(notif);

            db.Avis.Add(A);

            await db.SaveChangesAsync();

            return RedirectToAction("Detail", new { id = a.ID });
        }


        #endregion

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DDel(int id, string lbl)
        {
            Achat achat = await db.Achats.FindAsync(id);
            if (achat == null)
            {
                return HttpNotFound();
            }

            achat.State = StateDemande.DemandeAModifier;
            db.Entry(achat).State = EntityState.Modified;
            Notification notif = new Notification { AchatID = achat.ID, Role = SecurityRole.RespAcha, Lbl = "vous avez une demande de suppresion pour " + lbl };
            db.Notifications.Add(notif);

            await db.SaveChangesAsync();

            return RedirectToAction("Detail", new { id = achat.ID });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DEdit(int id, string lbl)
        {
            Achat achat = await db.Achats.FindAsync(id);
            if (achat == null)
            {
                return HttpNotFound();
            }

            achat.State = StateDemande.DemandeAModifier;
            db.Entry(achat).State = EntityState.Modified;
            Notification notif = new Notification { AchatID = achat.ID, Role = SecurityRole.RespAcha, Lbl = "Demande Modification : " + lbl };
            db.Notifications.Add(notif);

            await db.SaveChangesAsync();

            return RedirectToAction("Detail", new { id = achat.ID });
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
            catch (Exception)
            {
                return true;
            }
        }



        #endregion

    }

}