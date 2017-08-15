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
    public class ResumesController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Resumes
        public ActionResult Index()
        {
            return View(db.Resumes.ToList());
        }

        // GET: Resumes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Resume Resume = db.Resumes.Find(id);
            //Resume Resume = db.Resumes.Include(x => x.Packages).SingleOrDefault(y => y.ResumeId == id);
            Resume Resume = getResume(id);
            //CVLetter CVLetter = db.CVLetters.Find(Resume.Packages.Single().CVLetterId);
            if (Resume == null)
            {
                return HttpNotFound();
            }
            return View(Resume);
        }

        // GET: Resumes/Create
        public ActionResult Create()
        {
            Resume model = new Resume() { Name = "Test " + DateTime.UtcNow.Ticks };
            model.Name = String.Format("Resume - {0}", DateTime.UtcNow.Ticks);

            ViewBag.CVLetters = new MultiSelectList(db.CVLetters.ToList(), "CVLetterId", "Name", model.CVLetters.Select(x => x.CVLetterId).ToArray());

            return View(model);
        }

        // POST: Resumes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,ApplicantName,Email,Phone,Address,Summary,Skill,WorkExperience,StudyExperience,Certificate,CVLetterIds")] Resume Resume, string[] CVLetterIds)
        {
            if (ModelState.IsValid)
            {
                // Target of duplication check is only name, not whether multi player or not
                Resume checkResume = db.Resumes.SingleOrDefault(x => x.Name == Resume.Name);
                if (checkResume == null)
                {
                    //Resume.ResumeId = Guid.NewGuid().ToString();
                    Resume.CreateDate = DateTime.UtcNow;
                    Resume.EditDate = Resume.CreateDate;

                    db.Resumes.Add(Resume);
                    db.SaveChanges();

                    if (CVLetterIds != null)
                    {
                        foreach (string CVLetterId in CVLetterIds)
                        {
                            Package Package = new Package();
                            Package.Name = String.Format("Package - {0}", DateTime.UtcNow.Ticks);

                            //Package.PackageId = Guid.NewGuid().ToString();
                            Package.CreateDate = DateTime.UtcNow;
                            Package.EditDate = Package.CreateDate;

                            Package.ResumeId = Resume.ResumeId;
                            Package.CVLetterId = CVLetterId;
                            //db.Packages.Add(Package);
                            Resume.CVLetters.Add(Package);
                        }

                        db.Entry(Resume).State = EntityState.Modified;

                        db.SaveChanges();
                    }


                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Duplicated Resume detected.");
                }
            }

            ViewBag.CVLetters = new MultiSelectList(db.CVLetters.ToList(), "CVLetterId", "Name", CVLetterIds);

            return View(Resume);
        }

        // GET: Resumes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resume Resume = getResume(id);
            if (Resume == null)
            {
                return HttpNotFound();
            }

            ViewBag.CVLetters = new MultiSelectList(db.CVLetters.ToList(), "CVLetterId", "Name", Resume.CVLetters.Select(x => x.CVLetterId).ToArray());

            return View(Resume);
        }

        // POST: Resumes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ResumeId,Name,ApplicantName,Email,Phone,Address,Summary,Skill,WorkExperience,StudyExperience,Certificate,CVLetterIds")] Resume Resume, string[] CVLetterIds)
        {
            if (ModelState.IsValid)
            {
                //Resume.CreateDate = (DateTime) db.Entry(Resume).Property("CreateDate").CurrentValue;
                Resume tmpResume = getResume(Resume.ResumeId);

                if (tmpResume != null)
                {
                    // Target of duplication check is only name, not whether multi player or not
                    Resume checkResume = db.Resumes.SingleOrDefault(
                        x => x.Name == Resume.Name &&
                        x.ResumeId != Resume.ResumeId);
                    if (checkResume == null)
                    {
                        tmpResume.Name = Resume.Name;
                        tmpResume.ApplicantName = Resume.ApplicantName;
                        tmpResume.Email = Resume.Email;
                        tmpResume.Phone = Resume.Phone;
                        tmpResume.Address = Resume.Address;
                        tmpResume.Summary = Resume.Summary;
                        tmpResume.Skill = Resume.Skill;
                        tmpResume.WorkExperience = Resume.WorkExperience;
                        tmpResume.StudyExperience = Resume.StudyExperience;
                        tmpResume.Certificate = Resume.Certificate;
                        tmpResume.EditDate = DateTime.UtcNow;

                        db.Entry(tmpResume).State = EntityState.Modified;

                        // Items to remove

                        var removeItems = tmpResume.CVLetters.ToList(); // default is deleting all
                        if (CVLetterIds != null)
                        {
                            removeItems = tmpResume.CVLetters.Where(x => !CVLetterIds.Contains(x.CVLetterId)).ToList();
                        }
                        
                        foreach (var removeItem in removeItems)
                        {
                            db.Entry(removeItem).State = EntityState.Deleted;
                        }

                        // Items to add
                        if (CVLetterIds != null)
                        {
                            var addedItems = CVLetterIds.Where(x => !tmpResume.CVLetters.Select(y => y.CVLetterId).Contains(x));

                            foreach (string addedItme in addedItems)
                            {
                                Package Package = new Package();
                                Package.Name = String.Format("Package - {0}", DateTime.UtcNow.Ticks);

                                Package.PackageId = Guid.NewGuid().ToString();
                                Package.CreateDate = DateTime.UtcNow;
                                Package.EditDate = Package.CreateDate;

                                Package.ResumeId = Resume.ResumeId;
                                Package.CVLetterId = addedItme;
                                db.Packages.Add(Package);
                            }
                        }
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Duplicated Resume detected.");
                    }
                }
            }

            ViewBag.CVLetters = new MultiSelectList(db.CVLetters.ToList(), "CVLetterId", "Name", CVLetterIds);
             return View(Resume);
        }

        /*// GET: Resumes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resume Resume = getResume(id);
            if (Resume == null)
            {
                return HttpNotFound();
            }

            return View(Resume);
        }*/

        // POST: Resumes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed([Bind(Include = "ResumeId")] Resume Resume)
        {
            Resume = getResume(Resume.ResumeId);

            // Delete Foreign Key objects
            foreach (var item in Resume.CVLetters.ToList())
            {
                db.Packages.Remove(item);
            }

            db.Resumes.Remove(Resume);

            var deleted = db.ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted);

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

        private Resume getResume(string id)
        {
            Resume Resume = db.Resumes.Include(x => x.CVLetters.Select(g => g.CVLetter)).SingleOrDefault(y => y.ResumeId == id);
            return Resume;
        }
    }
}
