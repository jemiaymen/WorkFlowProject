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

    //[Authorize(Roles = "Responsable Achat")]
    public class FourController : WorkController
    {

        // GET: Fournisseurs
        public async Task<ActionResult> Index()
        {
            return View(await db.Fournisseurs.ToListAsync());
        }

        // GET: Fournisseurs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fournisseur fournisseur = await db.Fournisseurs.FindAsync(id);
            if (fournisseur == null)
            {
                return HttpNotFound();
            }
            return View(fournisseur);
        }

        // GET: Fournisseurs/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Lbl");
            return View();
        }

        // POST: Fournisseurs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Nom_frn,Adress_frn,Mail_frn,Tel_frn")] Fournisseur fournisseur, int[] CategoryID)
        {
            if (ModelState.IsValid)
            {
                db.Fournisseurs.Add(fournisseur);
                await db.SaveChangesAsync();

                foreach (int i in CategoryID)
                {
                    var cinf = new CategoryInFournisseur();
                    cinf.CategoryID = i;
                    cinf.FournisseurID = fournisseur.ID;
                    db.CategoryInFournisseur.Add(cinf);
                    await db.SaveChangesAsync();
                }

                return RedirectToAction("Index");
                
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Lbl", CategoryID);
            return View(fournisseur);
        }

        // GET: Fournisseurs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Fournisseur fournisseur = await db.Fournisseurs.FindAsync(id);
            
            if (fournisseur == null)
            {
                return HttpNotFound();
            }

            var cinf = await db.CategoryInFournisseur.Where(c => c.FournisseurID == fournisseur.ID).ToListAsync();
            List<int> CID = new List<int>();

            foreach (var ob in cinf)
            {
                CID.Add(  ob.CategoryID );
            }
            ViewBag.CategoryID = new MultiSelectList(db.Categories, "ID", "Lbl", CID);
            return View(fournisseur);
        }

        // POST: Fournisseurs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Nom_frn,Adress_frn,Mail_frn,Tel_frn")] Fournisseur fournisseur, int[] CategoryID)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fournisseur).State = EntityState.Modified;

                var CINF = await db.CategoryInFournisseur.Where(c => c.FournisseurID == fournisseur.ID).ToListAsync();
                
                foreach(var c in CINF)
                {
                    db.CategoryInFournisseur.Remove(c);
                }
                await db.SaveChangesAsync();

                foreach (int i in CategoryID)
                {
                    var cinf = new CategoryInFournisseur();
                    cinf.CategoryID = i;
                    cinf.FournisseurID = fournisseur.ID;
                    db.CategoryInFournisseur.Add(cinf);
                    
                }
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(fournisseur);
        }

        // GET: Fournisseurs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fournisseur fournisseur = await db.Fournisseurs.FindAsync(id);
            if (fournisseur == null)
            {
                return HttpNotFound();
            }
            return View(fournisseur);
        }

        // POST: Fournisseurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {

            Fournisseur fournisseur = await db.Fournisseurs.FindAsync(id);
            var CINF = await db.CategoryInFournisseur.Where(c => c.FournisseurID == fournisseur.ID).ToListAsync();
            foreach (var c in CINF)
            {
                db.CategoryInFournisseur.Remove(c);
            }
            db.Fournisseurs.Remove(fournisseur);

            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
