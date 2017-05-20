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
    public class BCController : WorkController
    {
        // GET: BC
        public async Task<ActionResult> Index()
        {
            return View(await db.AchaFournisseurs.Where(a => a.Achat.Type == TypeDemande.BonCommande && a.State == StateFournisseur.Chose).ToListAsync());
        }

        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AchaFournisseur n = db.AchaFournisseurs.Find(id);
            if (n == null)
            {
                return HttpNotFound();
            }

            return View(n);
        }

        public ActionResult Livraison(int? id,int? total)
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

            a.State = StateDemande.Reciption;

            a.Department.Depense = a.Department.Depense + total.GetValueOrDefault() ;

            db.Entry(a).State = EntityState.Modified;

            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}