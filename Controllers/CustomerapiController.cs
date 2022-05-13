using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using testmvc.Models;

namespace testmvc.Controllers
{
    public class CustomerapiController : Controller
    {


        private readonly ICustomer _customer;
        private readonly IUser _user;
        private  int? uid=null; 
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerapiController"/> class.
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="user"></param>

        public CustomerapiController(ICustomer customer,IUser user)
        {
           this._customer = customer;
            
            this._user = user;
        }



        // GET: CustomerapiController
        public ActionResult Index()
        {
            return View();
        }

        
        // GET: CustomerapiController/Create
        public ActionResult Create()
        {
            return View();
        }

      
        // GET: CustomerapiController/Edit/5
        public ActionResult Edit(int id)
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

        
        
    
    
    }
}
