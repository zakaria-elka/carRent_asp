using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<string> data =new List<string>();
            data.Add("a");
            data.Add("b");
            data.Add("c");



            ViewBag.vb = data;
            ViewData["name"] = data;
            TempData["lname"] = data;
            Session["code"] = data;


            return View();
        }

 
        

        


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        




    }


}