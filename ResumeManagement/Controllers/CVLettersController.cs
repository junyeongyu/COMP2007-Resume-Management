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
    [Authorize]
    public class CVLettersController : Controller
    {
        private DataContext db = new DataContext();

        // GET: CVLetters
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(db.CVLetters.ToList());
        }

        // GET: CVLetters/Details/5
        [AllowAnonymous]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CVLetter CVLetter = getCVLetter(id);
            if (CVLetter == null)
            {
                return HttpNotFound();
            }
            return View(CVLetter);
        }

        // GET: CVLetters/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CVLetters/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Content")] CVLetter CVLetter)
        {
            if (ModelState.IsValid)
            {
                CVLetter checkCVLetter = db.CVLetters.SingleOrDefault(x => x.Name == CVLetter.Name);
                if (checkCVLetter == null)
                {
                    //CVLetter.CVLetterId = Guid.NewGuid().ToString();
                    CVLetter.CreateDate = DateTime.UtcNow;
                    CVLetter.EditDate = CVLetter.CreateDate;

                    db.CVLetters.Add(CVLetter);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Duplicated CVLetter detected.");
                }
            }

            return View(CVLetter);
        }

        // GET: CVLetters/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CVLetter CVLetter = getCVLetter(id);
            if (CVLetter == null)
            {
                return HttpNotFound();
            }
            return View(CVLetter);
        }

        // POST: CVLetters/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CVLetterId,Name,Content")] CVLetter CVLetter)
        {
            if (ModelState.IsValid)
            {
                CVLetter checkCVLetter = db.CVLetters.SingleOrDefault(
                    x => x.Name == CVLetter.Name &&
                    x.CVLetterId != CVLetter.CVLetterId);
                if (checkCVLetter == null)
                {
                    CVLetter tmpCVLetter = db.CVLetters.Find(CVLetter.CVLetterId);
                    tmpCVLetter.Name = CVLetter.Name;
                    tmpCVLetter.Content = CVLetter.Content;
                    tmpCVLetter.EditDate = DateTime.UtcNow;
                    db.Entry(tmpCVLetter).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Duplicated CVLetter detected.");
                }
            }
            return View(CVLetter);
        }
        
        // POST: CVLetters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed([Bind(Include = "CVLetterId")] CVLetter CVLetter)
        {
            //CVLetter CVLetter = db.CVLetters.Find(id);
            CVLetter = getCVLetter(CVLetter.CVLetterId);
            // Delete Foreign Key objects
            foreach (var item in CVLetter.Resumes.ToList())
            {
                db.Packages.Remove(item);
            }

            var deleted = db.ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted);

            db.CVLetters.Remove(CVLetter);
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

        private CVLetter getCVLetter(string id)
        {
            CVLetter CVLetter = db.CVLetters.Include(x => x.Resumes.Select(g => g.Resume)).SingleOrDefault(y => y.CVLetterId == id);
            return CVLetter;
        }
    }
}
