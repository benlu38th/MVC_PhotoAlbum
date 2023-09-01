using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC_PhotoAlbum.Models;
using MVC_PhotoAlbum.Function;
using System.IO;

namespace MVC_PhotoAlbum.Controllers
{
    [Authorize]
    public class PhotosController : Controller
    {
        private Context db = new Context();

        // GET: Photos
        public ActionResult Index(int? id)
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

            ViewBag.CategoryId = id;
            ViewBag.PhotoCategory = db.PhotoCategories.Where(p => p.Id == id).ToList()[0].Name;
            var photos = db.Photos.Where(p => p.PhotoCategoryId == id);
            return View(photos.ToList());
        }

        // GET: Photos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photos.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }

            ViewBag.PhotoCategoryIdNum = db.Photos.Where(p => p.Id == id).ToList()[0].PhotoCategoryId;
            ViewBag.PhotoCategory = db.Photos.Where(p => p.Id == id).ToList()[0].MyPhotoCategory.Name;

            return View(photo);
        }

        // GET: Photos/Create
        public ActionResult Create(int? id)
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

            ViewBag.PhotoCategoryIdNum = db.PhotoCategories.Where(p => p.Id == id).ToList()[0].Id;
            ViewBag.PhotoCategory = db.PhotoCategories.Where(p => p.Id == id).ToList()[0].Name;
            return View();
        }

        // POST: Photos/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase[] ImageFiles, int AlbumId)
        {
            bool success = false;

            foreach (HttpPostedFileBase ImageFile in ImageFiles)
            {
                Photo photo = new Photo();

                if (ImageFile != null)//如果有上傳成功
                {
                    if (General.IsPicture(ImageFile.FileName))//如果是檔案尾標符合條件
                    {
                        if (General.IsImage(ImageFile) != null)//如果是圖片檔案
                        {
                            if (ImageFile.ContentLength > 0)//如果是檔案大小>0
                            {
                                // 從上傳的圖片擷取檔名（不含副檔名）
                                string fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);

                                // 取得圖片的副檔名
                                string extension = Path.GetExtension(ImageFile.FileName);

                                // 組合新的檔名：原始檔名 + 現在時間（年月日時分秒毫秒） + 副檔名
                                fileName = fileName + "_" + DateTime.Now.ToString("yyMMddHHmmssfff") + "_" + photo.PhotoCategoryId + extension;

                                // 組合完整的檔案路徑
                                string savsAsPath = Path.Combine(Server.MapPath("~/Upload/Photos/"), fileName);

                                // 儲存圖片到伺服器上的指定路徑
                                ImageFile.SaveAs(savsAsPath);

                                // 設定圖片的取得路徑（相對路徑）
                                photo.PhotoUrl = "/Upload/Photos/" + fileName;

                                //塞時間
                                photo.InitDate = DateTime.Now;

                                photo.PhotoCategoryId = AlbumId;

                                photo.Title = Path.GetFileNameWithoutExtension(ImageFile.FileName);

                                // 將圖片資訊存入資料庫
                                db.Photos.Add(photo);

                                // 執行資料庫變更，將圖片資訊寫入資料庫
                                db.SaveChanges();

                                // 清除模型狀態，以準備進行下一次操作
                                ModelState.Clear();

                                success = true;
                            }
                            else
                            {
                                ViewBag.PhotoCategoryIdNum = db.PhotoCategories.Where(p => p.Id == AlbumId).ToList()[0].Id;
                                ViewBag.PhotoCategory = db.PhotoCategories.Where(p => p.Id == AlbumId).ToList()[0].Name;

                                ModelState.AddModelError("ImageFile", "Please choose a file to upload.");
                                return View();
                            }
                        }
                        else
                        {
                            ViewBag.PhotoCategoryIdNum = db.PhotoCategories.Where(p => p.Id == AlbumId).ToList()[0].Id;
                            ViewBag.PhotoCategory = db.PhotoCategories.Where(p => p.Id == AlbumId).ToList()[0].Name;

                            ModelState.AddModelError("ImageFile", "Please choose a file to upload.");
                            return View();
                        }
                    }
                    else
                    {
                        ViewBag.PhotoCategoryIdNum = db.PhotoCategories.Where(p => p.Id == AlbumId).ToList()[0].Id;
                        ViewBag.PhotoCategory = db.PhotoCategories.Where(p => p.Id == AlbumId).ToList()[0].Name;

                        ModelState.AddModelError("ImageFile", "Please choose a file to upload.");
                        return View();
                    }
                }
                else
                {
                    ViewBag.PhotoCategoryIdNum = db.PhotoCategories.Where(p => p.Id == AlbumId).ToList()[0].Id;
                    ViewBag.PhotoCategory = db.PhotoCategories.Where(p => p.Id == AlbumId).ToList()[0].Name;

                    ModelState.AddModelError("ImageFile", "Please choose a file to upload.");
                    return View();
                }

            }

            if (success)
            {
                // 返回到預設的視圖（可能是指示操作已完成的訊息頁面）
                string newUrl = Url.Action("Index", "Photos", new { id = AlbumId });
                return Redirect(newUrl);
            }

            ViewBag.PhotoCategoryIdNum = db.PhotoCategories.Where(p => p.Id == AlbumId).ToList()[0].Id;
            ViewBag.PhotoCategory = db.PhotoCategories.Where(p => p.Id == AlbumId).ToList()[0].Name;
            return View();
        }

        // GET: Photos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photos.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }

            return View(photo);
        }

        // POST: Photos/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,PhotoUrl,IsCover,InitDate,PhotoCategoryId")] Photo photo)
        {
            if (ModelState.IsValid)
            {

                //取出全部相片
                var photos = db.Photos.Where(p => p.PhotoCategoryId == photo.PhotoCategoryId).ToList();

                //判斷是否已經找到上一個isCover = true
                bool isFindLastTrue = false;

                //欲編輯的相片是否已經編輯過
                bool isEdited = false;

                foreach (var eachPhoto in photos)
                {
                    if (eachPhoto.Id == photo.Id)//找到欲編輯的相片
                    {
                        eachPhoto.Title = photo.Title;
                        eachPhoto.Description = photo.Description;
                        eachPhoto.PhotoUrl = photo.PhotoUrl;
                        eachPhoto.IsCover = photo.IsCover;
                        eachPhoto.InitDate = photo.InitDate;
                        eachPhoto.PhotoCategoryId = photo.PhotoCategoryId;
                        isEdited = true;
                    }
                    else//不是欲編輯的相片，先判斷isCover是否為true
                    {
                        if (eachPhoto.IsCover == true) //找到了上一個isCover = true
                        {
                            eachPhoto.IsCover = false;
                            isFindLastTrue = true;
                        }
                    }

                    if (isFindLastTrue == true && isEdited == true)
                    {
                        break;
                    }
                }

                db.SaveChanges();
                return RedirectToAction("Index", new { id = photo.PhotoCategoryId });
            }
            ViewBag.PhotoCategoryId = new SelectList(db.PhotoCategories, "Id", "Name", photo.PhotoCategoryId);
            return View(photo);
        }

        // GET: Photos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photos.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            return View(photo);
        }

        // POST: Photos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Photo photo = db.Photos.Find(id);
            var albumId = photo.PhotoCategoryId;
            db.Photos.Remove(photo);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = albumId });
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
