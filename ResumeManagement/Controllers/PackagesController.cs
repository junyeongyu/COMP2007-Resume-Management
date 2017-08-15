using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ResumeManagement.Models;

namespace ResumeManagement.Controllers
{
    public class PackagesController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Packages
        public ActionResult Index()
        {
            var Packages = db.Packages.Include(g => g.Resume).Include(g => g.CVLetter);
            return View(Packages.ToList());
        }

        // GET: Packages/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Package Package = db.Packages.Find(id);
            Package Package = getPackage(id);
            if (Package == null)
            {
                return HttpNotFound();
            }
            return View(Package);
        }

        // GET: Packages/Create
        public ActionResult Create()
        {
            ViewBag.ResumeId = new SelectList(db.Resumes, "ResumeId", "Name");
            ViewBag.CVLetterId = new SelectList(db.CVLetters, "CVLetterId", "Name");
            return View();
        }

        // POST: Packages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,ResumeId,CVLetterId")] Package Package)
        {
            if (ModelState.IsValid)
            {
                Package tmpPackage = db.Packages.SingleOrDefault(y => y.ResumeId == Package.ResumeId && y.CVLetterId == Package.CVLetterId);
                if (tmpPackage == null)
                {
                    //Package.PackageId = Guid.NewGuid().ToString();
                    Package.CreateDate = DateTime.UtcNow;
                    Package.EditDate = Package.CreateDate;

                    db.Packages.Add(Package);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Duplicate entry found");
                }
            }

            ViewBag.ResumeId = new SelectList(db.Resumes, "ResumeId", "Name", Package.ResumeId);
            ViewBag.CVLetterId = new SelectList(db.CVLetters, "CVLetterId", "Name", Package.CVLetterId);
            return View(Package);
        }

        // GET: Packages/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Package Package = getPackage(id);
            if (Package == null)
            {
                return HttpNotFound();
            }
            ViewBag.ResumeId = new SelectList(db.Resumes, "ResumeId", "Name", Package.ResumeId);
            ViewBag.CVLetterId = new SelectList(db.CVLetters, "CVLetterId", "Name", Package.CVLetterId);
            return View(Package);
        }

        // POST: Packages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PackageId,Name,ResumeId,CVLetterId,PreviousResumeId,PreviousCVLetterId")] Package Package)
        {
            if (ModelState.IsValid)
            {
                Package tmpPackage = db.Packages.SingleOrDefault(y => y.ResumeId == Package.ResumeId && y.CVLetterId == Package.CVLetterId);
                bool isMyself = Package.PreviousResumeId == Package.ResumeId && Package.PreviousCVLetterId == Package.CVLetterId;
                if (tmpPackage == null || isMyself) // duplication should not be allowed but itself needs to be allowed
                {
                    Package tmpGG = db.Packages.Find(Package.PackageId);
                    tmpGG.ResumeId = Package.ResumeId;
                    tmpGG.Name = Package.Name;
                    tmpGG.CVLetterId = Package.CVLetterId;
                    tmpGG.EditDate = DateTime.UtcNow;
                    db.Entry(tmpGG).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Duplicate entry found");
                }
            }
            ViewBag.ResumeId = new SelectList(db.Resumes, "ResumeId", "Name", Package.ResumeId);
            ViewBag.CVLetterId = new SelectList(db.CVLetters, "CVLetterId", "Name", Package.CVLetterId);
            return View(Package);
        }

        // POST: Packages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed([Bind(Include = "PackageId")] Package Package)
        {
            Package = db.Packages.Find(Package.PackageId);
            db.Packages.Remove(Package);
            db.SaveChanges();
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

        private Package getPackage(string id)
        {
            Package Package = db.Packages.Include(x => x.Resume).Include(x => x.CVLetter).SingleOrDefault(y => y.PackageId == id);
            return Package;
        }
    }
}