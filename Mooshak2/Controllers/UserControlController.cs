﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Mooshak2.DAL;
using Mooshak2.Services;
using Mooshak2.Models;

namespace Mooshak2.Controllers
{
    [Authorize]
    public class UserControlController : Controller
    {
        private Context db = new Context();

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        private UserService uService = new UserService();

        public UserControlController()
        {
        }

        public UserControlController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: UserControler
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.AspNetUsers.ToList());
        }

        // GET: UserControler/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            UserDetailViewModel model = new UserDetailViewModel();
            model = uService.GetUserDetailView(aspNetUser);
            return View(model);
        }

        // GET: UserControler/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserControler/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Some fields aren't filled out or are wrongly filled out.");
                return View(model);
            }

            var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
            var result = await UserManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var currentUser = UserManager.FindByName(user.UserName);
                var roleresult = UserManager.AddToRole(currentUser.Id, model.Role);
            }

            return RedirectToAction("Index", "UserControl");
        }

        // GET: UserControler/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // POST: UserControler/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,Hometown,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,RoleId")] AspNetUser aspNetUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aspNetUser);
        }

        // GET: UserControler/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // POST: UserControler/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AspNetUser user = db.AspNetUsers.Find(id);
            uService.UserDelete(user.UserName, user.Id);
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

        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult AddCourse(string id)
        {
            AddCourseViewModel model = new AddCourseViewModel();
            model = uService.GetDropDownListCourses();
            model.UserName = UserManager.FindById(Convert.ToString(id)).UserName;
            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult AddCourse(AddCourseViewModel model)
        {
            bool checker = false;
            if (ModelState.IsValid)
            {
                List<StudentCourseList> studentCourseList = new List<StudentCourseList>();
                studentCourseList = uService.GetStudentCourseListByUserName(model.UserName);
                foreach (var entry in studentCourseList)
                {
                    if (entry.UserName == model.UserName && entry.courseId == model.courseId)
                    {
                        checker = true;
                    }
                }
                if (checker != true)
                {
                    var count = uService.GetAllStudentCourseList().LastOrDefault();
                    int id;
                    if (count == null)
                        id = 0;
                    else
                        id = count.Id + 1;
                    db.StudentCourseList.Add(new StudentCourseList
                    {
                        Id = id,
                        UserName = model.UserName,
                        courseId = model.courseId
                    });
                    db.SaveChanges();
                    return RedirectToAction("Details", UserManager.FindByName(model.UserName).Id);
                }
            }
            return RedirectToAction("AddCourse", "UserControl", UserManager.FindByName(model.UserName).Id);
        }

        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult RemoveCourse(string userName, int courseId)
        {
            RemoveCourseViewModel model = new RemoveCourseViewModel();
            model.userId = UserManager.FindByName(userName).Id;
            model.UserName = userName;
            model.courseId = courseId;
            model.courseName = db.Course.Find(courseId).Name;
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult RemoveCourse(RemoveCourseViewModel model)
        {
            uService.StudentCourseListDelete(model);
            return RedirectToAction("Index", "UserControl");
        }

    }
}
