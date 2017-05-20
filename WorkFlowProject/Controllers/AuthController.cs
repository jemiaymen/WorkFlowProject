using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WorkFlowProject.Models;

namespace WorkFlowProject.Controllers
{
    [Authorize]
    public class AuthController : ApiController
    {
        public ApplicationSignInManager _signInManager;
        public ApplicationUserManager _userManager;
        public ApplicationDbContext db;

        public AuthController()
        {
            _userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            _signInManager = HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            db = new ApplicationDbContext();
        }

        [AllowAnonymous]
        [Route("api/get/{Email}")]
        [HttpGet]
        public IEnumerable<Object> Get(String Email)
        {
            if (IsInRole(Email,SecurityRole.DAF))
            {
                return db.Achats.Where(a => a.State == StateDemande.DemandeConfirmerFournisseur).Select(a => new { ID = a.ID, Des = a.Des });
            }

            if (IsInRole(Email,SecurityRole.AUDIT))
            {
                return db.Achats.Where(a => a.State == StateDemande.DemandeConfirmerDaf).Select(a => new { ID = a.ID, Des = a.Des });
            }

            if (IsInRole(Email,SecurityRole.DG))
            {
                return db.Achats.Where(a => a.State == StateDemande.DemandeConfirmerAudit).Select(a => new { ID = a.ID, Des = a.Des });
            }
            return null;
        }

        [AllowAnonymous]
        [Route("api/detail/{ID}")]
        [HttpGet]
        public Object Detail(int ID)
        {

            var acha = db.Achats.Find(ID);
            if (acha == null)
            {
                return null;
            }

            var ach = new { Dt = acha.DtAcha.ToShortDateString() , Budget = acha.Department.Budget ,
                            Des = acha.Des ,Dep = acha.Department.Dep , Depence = acha.Department.Depense,
                            Categ = acha.Categ , Lieu = acha.LieuLiv , Qte = acha.Qte , Imp = acha.Imp
            };

            var af = db.AchaFournisseurs.Where(a => a.AchatID == acha.ID).OrderByDescending(a => a.State).Select(a => new { F = a.Fournisseur.Nom_frn, P = a.Price, R = a.Remise , C = a.State  });

            var av = db.Avis.Where(a => a.AchatID == acha.ID).Select(a => new { Des = a.Lbl , Img = a.User.SignatureUser });

            return new { Achat = ach, AF = af, Avis = av };
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IHttpActionResult> Post(LoginViewModel model)
        {

                var resutl = await _signInManager.PasswordSignInAsync(model.Email,model.Password, false, false);
                if(resutl == SignInStatus.Success)
                {
                    return Ok("Login Success");
                }


            return Content(HttpStatusCode.Forbidden, "Login Faild");

        }

        private bool IsInRole(string UserName,string Role)
        {
            var user = _userManager.FindByName(UserName);
            var role = db.Roles.Where(r => r.Name == Role).FirstOrDefault();

            try
            {
                user.Roles.Where(u => u.RoleId == role.Id).First();
                return true;
            }catch(Exception )
            {
                return false;
            }
        }


        [AllowAnonymous]
        [Route("api/accept")]
        [HttpPost]
        public bool Accept(RestAvis model)
        {
            ApplicationUser current = db.Users.Where(u => u.UserName == model.Email).FirstOrDefault();
            Achat a = db.Achats.Find(model.ID);

            if (a == null)
            {
                return false;
            }

            if (IsInRole(model.Email,SecurityRole.DAF))
            {
                Models.Avis avis = new Models.Avis { AchatID = model.ID, Accept = true, Lbl = model.Lbl, Code = AvisCode.daf, User = current };
                a.State = StateDemande.DemandeConfirmerDaf;
                db.Entry(a).State = System.Data.Entity.EntityState.Modified;

                db.Avis.Add(avis);
                Notification notif = new Notification { AchatID = avis.AchatID, Role = SecurityRole.AUDIT, Lbl = "Demande Confirmer par DAF : " + current.UserName };

                db.Notifications.Add(notif);
                db.SaveChanges();

                return true;
            }

            if (IsInRole(model.Email,SecurityRole.AUDIT))
            {
                Models.Avis avis = new Models.Avis { AchatID = model.ID, Accept = true, Lbl = model.Lbl, Code = AvisCode.audit, User = current };
                a.State = StateDemande.DemandeConfirmerAudit;
                db.Entry(a).State = System.Data.Entity.EntityState.Modified;

                db.Avis.Add(avis);
                Notification notif = new Notification { AchatID = avis.AchatID, Role = SecurityRole.DG, Lbl = "Demande Confirmer par Audit : " + current.UserName };

                db.Notifications.Add(notif);
                db.SaveChanges();

                return true;
            }

            if (IsInRole(model.Email,SecurityRole.DG))
            {
                Models.Avis avis = new Models.Avis { AchatID = model.ID, Accept = true, Lbl = model.Lbl, Code = AvisCode.dg, User = current };
                a.State = StateDemande.DemandeAccepter;
                db.Entry(a).State = System.Data.Entity.EntityState.Modified;

                db.Avis.Add(avis);
                Notification notif = new Notification { AchatID = avis.AchatID, Role = SecurityRole.Demandeur, Lbl = "Demande Confirmer par DG : " + current.UserName };
                Notification notif1 = new Notification { AchatID = avis.AchatID, Role = SecurityRole.RespAcha, Lbl = "Demande Confirmer par DG : " + current.UserName };

                db.Notifications.Add(notif);
                db.Notifications.Add(notif1);

                db.SaveChanges();
                return true;
            }
            return false;
        }



        [AllowAnonymous]
        [Route("api/ref")]
        [HttpPost]
        public bool Refuser(RestAvis model)
        {
            ApplicationUser current = db.Users.Where(u => u.UserName == model.Email).FirstOrDefault();
            Achat a = db.Achats.Find(model.ID);

            if (a == null)
            {
                return false;
            }

            if (IsInRole(model.Email, SecurityRole.DAF))
            {
                Models.Avis avis = new Models.Avis { AchatID = model.ID, Accept = false, Lbl = model.Lbl, Code = AvisCode.daf, User = current };
                a.State = StateDemande.DemandeRefuser;
                db.Entry(a).State = System.Data.Entity.EntityState.Modified;

                db.Avis.Add(avis);
                Notification notif = new Notification { AchatID = avis.AchatID, Role = SecurityRole.RespAcha, Lbl = "Demande Refuser par : " + current.UserName };
                Notification notif1 = new Notification { AchatID = avis.AchatID, Role = SecurityRole.Demandeur, Lbl = "Demande Refuser par : " + current.UserName };

                db.Notifications.Add(notif);
                db.Notifications.Add(notif1);
                db.SaveChanges();

                return true;
            }

            if (IsInRole(model.Email,SecurityRole.AUDIT))
            {
                Models.Avis avis = new Models.Avis { AchatID = model.ID, Accept = false, Lbl = model.Lbl, Code = AvisCode.audit, User = current };
                a.State = StateDemande.DemandeRefuser;
                db.Entry(a).State = System.Data.Entity.EntityState.Modified;

                db.Avis.Add(avis);
                Notification notif = new Notification { AchatID = avis.AchatID, Role = SecurityRole.RespAcha, Lbl = "Demande Refuser par : " + current.UserName };
                Notification notif1 = new Notification { AchatID = avis.AchatID, Role = SecurityRole.Demandeur, Lbl = "Demande Refuser par : " + current.UserName };

                db.Notifications.Add(notif);
                db.Notifications.Add(notif1);
                db.SaveChanges();

                return true;
            }

            if (IsInRole(model.Email,SecurityRole.DG))
            {
                Models.Avis avis = new Models.Avis { AchatID = model.ID, Accept = false, Lbl = model.Lbl, Code = AvisCode.dg, User = current };
                a.State = StateDemande.DemandeRefuser;
                db.Entry(a).State = System.Data.Entity.EntityState.Modified;

                db.Avis.Add(avis);
                Notification notif = new Notification { AchatID = avis.AchatID, Role = SecurityRole.RespAcha, Lbl = "Demande Refuser par : " + current.UserName };
                Notification notif1 = new Notification { AchatID = avis.AchatID, Role = SecurityRole.Demandeur, Lbl = "Demande Refuser par : " + current.UserName };

                db.Notifications.Add(notif);
                db.Notifications.Add(notif1);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        
    }
}