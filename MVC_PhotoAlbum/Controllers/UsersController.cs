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

namespace MVC_PhotoAlbum.Controllers
{
    [Authorize(Roles = "Top")]
    public class UsersController : Controller
    {
        private Context db = new Context();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            var userAccountExist = db.Users.Any(u => u.Account == user.Account);

            if (userAccountExist)
            {
                ViewBag.UserAccountExistMessage = "請換一組帳號，該帳號已存在。";
                return View(user);
            }
            if (user.Password != user.ConfirmPassword)
            {
                ViewBag.DifferentPwd = "密碼 及 確認密碼 不相符。";
                return View(user);
            }
            if (ModelState.IsValid)
            {
                string salt = Security.CreateSalt(16);
                user.Salt = salt;

                if (user.ConfirmPassword == user.Password)
                {
                    string hashenPwd = Security.CreatePasswordHash(user.Password, salt);
                    user.Password = hashenPwd;
                    user.ConfirmPassword = hashenPwd;
                }
                else
                {
                    return View(user);
                }

                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, string account, Role role, User user)
        {
            try
            {
                var userInfo = db.Users.Where(u => u.Id == id).FirstOrDefault();

                userInfo.Account = account;
                userInfo.role = role;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(user);
            }
        }



        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.BossCanNotDelete = (string)TempData["BossCanNotDelete"];

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var userInfo = db.Users.Where(u => u.Id == id).FirstOrDefault();

            if (userInfo.role == Role.Top)
            {
                string BossCanNotDelete = "不能刪除老闆身分的帳號";
                TempData["BossCanNotDelete"] = BossCanNotDelete;

                return RedirectToAction("Delete", new { id = id });
            }

            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
