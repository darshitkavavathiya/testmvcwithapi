using Microsoft.AspNetCore.Mvc;
using testmvc.Models;
using Microsoft.AspNetCore.Mvc;
using testmvc.Models;
using System;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace testmvc.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IUser _user;

        private readonly ICustomer _customer;

        private int? uid;

        private string uuuuu;



        public CustomerController(IUser user, ICustomer customer)
        {
            _user = user;
            _customer = customer;

        }



        //customer list 
        public IActionResult Index(FilterDto filter)
        {



            uid = HttpContext.Session.GetInt32("userId");

            if (uid == null || uid == 0)
            {
                TempData["message"] = "you must login";
                return RedirectToAction("Login", "Authentication");
            }


            filter.CreatedBy = (int)uid;

            ViewBag.FirstName = _user.GetUserById(uid).FirstName;
            return View(_customer.GetCustomers(filter));
        }

      

        // create new customer 

        public IActionResult Create()
        {

            //CheckLogin();
            uid = HttpContext.Session.GetInt32("userId");

            if (uid == null || uid == 0)
            {
                TempData["message"] = "you must login";
                return RedirectToAction("Login", "Authentication");
            }

            ViewBag.FirstName = _user.GetUserById(uid).FirstName;

            return View();

        }
        [HttpPost]
        public IActionResult Create([Bind] Customer customer)
        {
            uid = HttpContext.Session.GetInt32("userId");
            customer.CreatedBy = (int)uid;
            if (ModelState.IsValid)
            {

                if (_customer.AddCustomer(customer) > 0)
                {

                    TempData["toast"] = string.Format("firecustomtoast('{0}','{1}','{2}')", "", "new Customer is succcessfully created", "success");
                    return RedirectToAction("Index");
                }
            }
            return View(customer);

        }


        public JsonResult GetCustomerStates()
        {

            int active = 0, inactive = 0, total = 0;

            FilterDto filter = new FilterDto();
            filter.CreatedBy = (int)HttpContext.Session.GetInt32("userId");

            IEnumerable<Customer> custlist = _customer.GetCustomers(filter);

            foreach (Customer cust in custlist)
            {
                if ((bool)cust.IsActive)
                {
                    active++;
                }
                else
                {
                    inactive++;
                }
                total++;

            }
            var data = new
            {
                active = active,
                inactive = inactive,
                total = total,
            };
            return Json(data);
        }


        public JsonResult GetCustomerForChart()
        {
            uid = HttpContext.Session.GetInt32("userId");
            return Json(_customer.GetChartData((int)uid));
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
            Customer c = _customer.GetCustomerById(id, (int)uid);
            if (c == null)
                return NotFound();

            return View(c);
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
            Customer customer = _customer.GetCustomerById(id, (int)uid);
            if (customer == null)
                return NotFound();
            return View(customer);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {

            uid = HttpContext.Session.GetInt32("userId");
            if (_customer.DeleteCustomer(id, (int)uid) > 0)
            {
                TempData["toast"] = string.Format("firecustomtoast('{0}','{1}','{2}')", "", " Customer is Deleted", "error");

                return RedirectToAction("Index");
            }
            Customer customer = _customer.GetCustomerById(id, (int)uid);
            return View(customer);
        }




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
            Customer customer = _customer.GetCustomerById(id, (int)uid);
            if (customer == null)
                return NotFound();
            return View(customer);

        }
        [HttpPost]

        public IActionResult Edit(int id, [Bind] Customer customer)
        {

            uid = HttpContext.Session.GetInt32("userId");
            customer.ModifiedBy = (int)uid;
            if (id != customer.CustomerId)
            {
                return NotFound();
            }


            if (_customer.UpdateCustomer(customer) > 0)
            {
                TempData["toast"] = string.Format("firecustomtoast('{0}','{1}','{2}')", "", " Customer Updated Succesfully", "success");

                return RedirectToAction("Index");
            }

            return View(customer);
        }



















        [HttpPost]
        public IActionResult UploadDocument(IFormFile formFile, int customerId)
        {
            uid = HttpContext.Session.GetInt32("userId");
            var ms = new MemoryStream();
            formFile.CopyTo(ms);
            byte[] file = ms.ToArray();

            Attachments attachment = new Attachments {
                DocumentName = formFile.FileName,
                DocumentType = formFile.ContentType,
                CustomerId = customerId,
                UploadedBy = (int)uid,
                Attachment = file
            };

            if (_customer.UploadFile(attachment) > 0)
            {
                return Json(true);
            }
            return Json(false);
        }

        public IActionResult DeleteDocument(int id)
        {

            if (_customer.DeleteCustomerDocument(id) > 0)
            {

                return Json(true);
            }

            return Json(false);

        }



        public IActionResult GetCustomerDocuments(Customer customer)
        {
            return Json(_customer.GetAttachments(customer.CustomerId));
        }

        public IActionResult GetDocumentDataById(int id)
        {
            Attachments temp = new Attachments();

            temp = _customer.GetDocumentById(id);
            var base64 = Convert.ToBase64String(temp.Attachment, 0, temp.Attachment.Length);
            temp.src = string.Format("data:{0};base64,{1}", temp.DocumentType, base64);

            return Json(temp);
        }


        public IActionResult DownloadDocument(int id)
        {

            Attachments temp = _customer.GetDocumentById(id);
            return File(temp.Attachment, temp.DocumentType, temp.DocumentName);
        }



        public JsonResult GetCountrys()
        {
            return Json(_customer.GetCountryNames());
        }

        public JsonResult GetStates(State state)
        {
            return Json(_customer.GetStateNames(state.CountryId));
        }
    }
}
