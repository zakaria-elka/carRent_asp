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
    public class CarsController : Controller
    {
        private Database1Entities1 db = new Database1Entities1();

        // GET: Cars
        public ActionResult Index()
        {
            if (Session["userId"] == null || int.Parse(Session["userId"].ToString()) != 1
              && int.Parse(Session["userId"].ToString()) != 2)
            {
                return RedirectToAction("index", "Home");
            }
            return View(db.Cars.ToList());
        }

        // GET: Cars/Details/5
        public ActionResult Details(string num )
        {
            if (num == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(num);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // GET: Cars/Create
        public ActionResult Create()
        {
            if (Session["userId"] == null || int.Parse(Session["userId"].ToString()) != 1
               && int.Parse(Session["userId"].ToString()) != 2)
            {
                return RedirectToAction("index", "Home");
            }
            return View();
        }

        // POST: Cars/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,categories,brand,price,Rnum,cdate,carburant,models")] Car car , HttpPostedFileBase file )
        {
            
            if (ModelState.IsValid)
            {


                if (file.ContentLength > 0)
                {
                    MemoryStream target = new MemoryStream();
                    file.InputStream.CopyTo(target);
                    byte[] data = target.ToArray();
                    car.image = data;

                }



                db.Cars.Add(car);
                db.SaveChanges();
                return RedirectToAction("Index");
            }



            return View(car);
        }

        // GET: Cars/Edit/5
        public ActionResult Edit(string Rnum)
        {

            if (Rnum == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(Rnum);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // POST: Cars/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,categories,brand,price,Rnum,reserve,cdate,carburant,models,image")] Car car)
        {
            if (ModelState.IsValid)
            {
                
                db.Entry(car).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(car);
        }

        // GET: Cars/Delete/5
        public ActionResult Delete(string num)
        {
            if (num == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(num);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string num)
        {
            Car car = db.Cars.Find(num);
            db.Cars.Remove(car);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [ActionName("CarDisplay")]
        public ActionResult CarDisplay_get()
        {

            return View(db.Cars.ToList());

        }


        [HttpPost]
        [ActionName("CarDisplay")]
        public ActionResult CarDisplay_post(string num , string name )
        {
            var UserEmail = Session["UserEmail"];
            var UserId = Session["UserId"];

         
            if (UserEmail != null)
            {
                Account account = db.Accounts.Find(UserId, UserEmail);
                resrvation resrvation = db.resrvations.Where(a => a.person_email.Equals(UserEmail.ToString())
                ).FirstOrDefault();

                if (resrvation == null)
                {
                    if (account.cin != null)
                    {
                        TempData["CarName"] = name;
                        TempData["CarNumber"] = num;
                        return RedirectToAction("Create", "resrvations");
                    }
                    else
                    {
                        TempData["CarName"] = name;
                        TempData["CarNumber"] = num;
                        return RedirectToAction("Edit", "Accounts", new { id = UserId, email = UserEmail });

                    }
                }
                else
                {
                    ViewBag.message = "Aleardy you have reservation";
                }

                }

            return View(db.Cars.ToList());

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
