using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using testmvc.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace testmvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IUser _user = null;
        private readonly ICustomer _customer = null;
        private int? uid;
        public HomeController(ILogger<HomeController> logger, IUser user,ICustomer customer)
        {

            _logger = logger;
            _user = user;
            _customer = customer;
            

        }

        public IActionResult Index()
        {
           uid = HttpContext.Session.GetInt32("userId");

            if (uid == null || uid == 0)
            {
                TempData["message"] = "you must login";
                return RedirectToAction("Login");
            }
            ViewBag.FirstName = _user.GetUserById(uid).FirstName;

        
            return View(_user.GetUsers());

        }




        public IActionResult CustomFilter()
        {
            uid = HttpContext.Session.GetInt32("userId");

            if (uid == null || uid == 0)
            {
                TempData["message"] = "you must login";
                return RedirectToAction("Login", "Authentication");
            }

            ViewBag.FirstName = _user.GetUserById(uid).FirstName;
            return View();
        }



        //customer list 
        public IActionResult Dynemictablejson()
        {



            uid = HttpContext.Session.GetInt32("userId");

            
            return Json(_customer.GetALLAttachments((int)uid));
        }


        public IActionResult CustomerDynemicTable(FilterDto filter)
        {
            filter.CreatedBy =(int)HttpContext.Session.GetInt32("userId");
            return Json(_customer.GetCustomers(filter));
        }

        public int getSessionValue()
        {
            
            return (int)HttpContext.Session.GetInt32("userId");
        }


        // create 

        public IActionResult Create()
        {
            uid = HttpContext.Session.GetInt32("userId");

            if (uid == null || uid == 0)
            {
                TempData["message"] = "you must login";
                return RedirectToAction("Login");
            }
            ViewBag.FirstName = _user.GetUserById(uid).FirstName;

            return View();

        }
        [HttpPost]
        public IActionResult Create([Bind]User u)
        {
            if (ModelState.IsValid)
            {
              if ( _user.AddUser(u)>0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(u);

        }

        // details 

        public IActionResult Details(int? id)
        {
            uid = HttpContext.Session.GetInt32("userId");

            if (uid == null || uid == 0)
            {
                TempData["message"] = "you must login";
                return RedirectToAction("Login");
            }
            ViewBag.FirstName = _user.GetUserById(uid).FirstName;
            if (id == null)
            {
                return NotFound();
            }
            User u = _user.GetUserById(id);
            if (u == null)
                return NotFound();
            return View(u);
        }
    
        // delete
        public IActionResult Delete(int? id)
        {
            uid = HttpContext.Session.GetInt32("userId");

            if (uid == null || uid == 0)
            {
                TempData["message"] = "you must login";
                return RedirectToAction("Login");
            }
            ViewBag.FirstName = _user.GetUserById(uid).FirstName;
            if (id == null)
            {
                return NotFound();
            }
            User u = _user.GetUserById(id);
            if (u == null)
                return NotFound();
            return View(u);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            
          
            if (_user.DeleteUser(id) > 0)
            {
                return RedirectToAction("Index");
            }
            User u = _user.GetUserById(id);
            if (u == null)
                return NotFound();
            return View(u);
        }



        // update 
      
        public IActionResult Edit(int? id)
        {

            uid = HttpContext.Session.GetInt32("userId");

            if (uid == null || uid == 0)
            {
                TempData["message"] = "you must login";
                return RedirectToAction("Login");
            }
            ViewBag.FirstName = _user.GetUserById(uid).FirstName;
            if (id == null)
            {
                return NotFound();
            }
            User u= _user.GetUserById(id);
            if (u == null)
                return NotFound();
            return View(u);
        }
        [HttpPost]
       
        public IActionResult Edit(int id,[Bind]User u )
        {
            if (id!= u.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (_user.UpdateUser(u) > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(u);
        }

        public IActionResult Dashboard()
        {
            uid = HttpContext.Session.GetInt32("userId");

            if (uid == null || uid == 0)
            {
                TempData["message"] = "you must login";
                return RedirectToAction("Login");
            }
            ViewBag.FirstName = _user.GetUserById(uid).FirstName;
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
