using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Dynamic;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class resrvationsController : Controller
    {
        private Database1Entities1 db = new Database1Entities1();

        // GET: resrvations
        public ActionResult Index()
        {
            if (Session["userId"] == null || int.Parse(Session["userId"].ToString()) != 1
              && int.Parse(Session["userId"].ToString()) != 2)
            {
                return RedirectToAction("index", "Home");
            }

            return View(db.resrvations.ToList());
        }

        // GET: resrvations/Details/5
        public ActionResult Details(int? id)
        {
     

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            resrvation resrvation = db.resrvations.Find(id);
       
           

            if (resrvation == null)
            {
                return HttpNotFound();
            }
          
            return View(resrvation);
        }

        // GET: resrvations/Create
        public ActionResult Create()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("index", "Home");
            }
            return View();
        }

        // POST: resrvations/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,car_name_,car_number_plate_,person_name,person_email,long_time,start_date,finish_date")] resrvation resrvation)
        {
            if (ModelState.IsValid)
            {

   var diffrence_between_date = (resrvation.finish_date - resrvation.start_date).TotalDays;
                if ((resrvation.long_time == true && diffrence_between_date > 5) || (resrvation.long_time == false && diffrence_between_date < 5))
                {


                    Car car = db.Cars.Find(resrvation.car_number_plate_);
                    car.reserve = true;
                    db.resrvations.Add(resrvation);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(resrvation);
        }

        // GET: resrvations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            resrvation resrvation = db.resrvations.Find(id);
            if (resrvation == null)
            {
                return HttpNotFound();
            }
            return View(resrvation);
        }

        // POST: resrvations/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,car_name_,car_number_plate_,person_name,person_email,long_time,start_date,finish_date")] resrvation resrvation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(resrvation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(resrvation);
        }

        // GET: resrvations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            resrvation resrvation = db.resrvations.Find(id);
            if (resrvation == null)
            {
                return HttpNotFound();
            }
            return View(resrvation);
        }

        // POST: resrvations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            resrvation resrvation = db.resrvations.Find(id);
            Car car = db.Cars.Find(resrvation.car_number_plate_);
            car.reserve = false;
            db.resrvations.Remove(resrvation);
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
