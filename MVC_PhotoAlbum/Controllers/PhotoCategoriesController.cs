using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC_PhotoAlbum.Models;

namespace MVC_PhotoAlbum.Controllers
{
    [Authorize]
    public class PhotoCategoriesController : Controller
    {
        private Context db = new Context();


        // GET: PhotoCategories
        public ActionResult Index()
        {
            ViewBag.CoverPhotos = db.Photos.Where(p => p.IsCover == true).ToList();

            string nameIsNullOrEmpty = (string)TempData["ViewBag.nameIsNullOrEmpty"];
            if (nameIsNullOrEmpty != null)
            {
               ViewBag.NameIsNullOrEmpty = nameIsNullOrEmpty;
            }

            return View(db.PhotoCategories.OrderByDescending(p => p.InitDate).ToList());
        }

        // GET: PhotoCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhotoCategory photoCategory = db.PhotoCategories.Find(id);
            if (photoCategory == null)
            {
                return HttpNotFound();
            }
            return View(photoCategory);
        }

        // GET: PhotoCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PhotoCategories/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string name)
        {
            if (name == null || name == "")
            {
                TempData["ViewBag.nameIsNullOrEmpty"] = "相簿名稱不得為空或null！";

                return RedirectToAction("Index");
            }
            else
            {
                var photoCategory = new PhotoCategory();

                photoCategory.Name = name;
                photoCategory.InitDate = DateTime.Now;

                db.PhotoCategories.Add(photoCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }




        }

        // GET: PhotoCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhotoCategory photoCategory = db.PhotoCategories.Find(id);
            if (photoCategory == null)
            {
                return HttpNotFound();
            }
            var coverPhoto = db.Photos.Where(p => p.PhotoCategoryId == id && p.IsCover == true).FirstOrDefault();
            ViewBag.CoverPhoto = coverPhoto;

            return View(photoCategory);
        }

        // POST: PhotoCategories/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,InitDate")] PhotoCategory photoCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(photoCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(photoCategory);
        }

        // GET: PhotoCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhotoCategory photoCategory = db.PhotoCategories.Find(id);
            if (photoCategory == null)
            {
                return HttpNotFound();
            }
            return View(photoCategory);
        }

        // POST: PhotoCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhotoCategory photoCategory = db.PhotoCategories.Find(id);
            db.PhotoCategories.Remove(photoCategory);
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
    }
}
