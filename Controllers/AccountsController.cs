using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class AccountsController : Controller
    {
        private Database1Entities1 db = new Database1Entities1();

        // GET: Accounts
        public ActionResult Index()
        {
           
          
                return View(db.Accounts.ToList());
           
        }

        // GET: Accounts/Details/5
        public ActionResult Details(string email)
        {
            if (email == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
       Account account = db.Accounts.Where(a => a.email.Equals(email)).FirstOrDefault();

            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

      

        // GET: Accounts/Edit/5
        public ActionResult Edit(int? id,string email)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id , email);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Accounts/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,name,email,pas,birthday,tel")] Account account, 
            HttpPostedFileBase file ,HttpPostedFileBase file1)
        {
            if (ModelState.IsValid)
            {

                if (file.ContentLength > 0 && file1.ContentLength >0)
                {
                    MemoryStream target = new MemoryStream();
                    MemoryStream target1 = new MemoryStream();
                    file.InputStream.CopyTo(target);
                    file1.InputStream.CopyTo(target1);
                    byte[] data = target.ToArray();
                    byte[] data1 = target1.ToArray();
                    account.cin = data;
                    account.Dlicense = data1;

                }





                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                TempData["CarName"] = TempData["CarName"];
                TempData["CarNumber"] = TempData["CarNumber"];
                return RedirectToAction("Create","resrvations");
            }
            return View(account);
        }

        // GET: Accounts/Delete/5
        public ActionResult Delete(int? id,string email)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id , email);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id,string email)
        {
            Account account = db.Accounts.Find(id,email);
            db.Accounts.Remove(account);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [ActionName("login")]
        public ActionResult login_get()
        {
            Session["user"] = null;
            Session["UserEmail"] = null;
            Session["UserId"] = null;
            return View();
        }

        [HttpPost, ActionName("login")]
        [ValidateAntiForgeryToken] 
        public ActionResult login_post(FormCollection obj)
        {
            
            var addr = Convert.ToString(obj["email"]);
            var p = Convert.ToString(obj["pas"]);
           
            var f = Convert.ToString(obj[3]);
            
            var pas = System.Text.Encoding.UTF8.GetBytes(p);
            var sha = new System.Security.Cryptography.SHA1Managed().ComputeHash(pas);
            var hash = string.Empty;
               
            foreach (var b in sha)
                {
                    hash += b.ToString("X2");
                }

            if (f.Equals("login"))
            {

                Account account = db.Accounts.Where(a => a.email.Equals(addr) && a.pas.Equals(hash)
                ).FirstOrDefault();

                if (account != null)
                {

                    Session["user"] = account.name;
                    Session["UserEmail"] = account.email;
                    Session["UserId"] = account.Id;
                    return RedirectToAction("CarDisplay", "Cars");
                }
                else
                {
                    ViewBag.message = "password or email are wrong";

                }
            }
            else
            {
                if (ModelState.IsValid)
                {

                    Account Valid = db.Accounts.Where(a => a.email.Equals(addr)
                    ).FirstOrDefault();

                    if (Valid == null)
                    {

                        var name = Convert.ToString(obj["name"]);

                        db.Accounts.Add(new Account
                        {
                            name = name,
                            email = addr,
                            pas = hash


                        });
                        db.SaveChanges();
                        Session["user"] = name;
                        Session["UserEmail"] = addr;
                        Session["UserId"] = db.Accounts.Max(a => a.Id);
                        return RedirectToAction("CarDisplay", "Cars");
                    }else
                    {
                        ViewBag.message = "email already exist";
                    }

                }



            }


            return View();
            
            
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
