using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using P1DbContext.Models;
using P1Mvc.Models;
using BusinessLayer;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace P1Mvc.Controllers
{
    public class AccountController : Controller
    {

        public IBusinessModel _BusinessModel;

        public AccountController(IBusinessModel BusinessModel)
        {
            this._BusinessModel = BusinessModel;
        }

        public IActionResult LoginPage()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginLandingPage(Customer loginUser)
        {
            bool loginStatus = _BusinessModel.Login(loginUser.UserName, loginUser.Password);
            if(loginStatus)
            {
                var currentUser = _BusinessModel.GetCurrentUser();
                ViewBag.currentUser = currentUser;
                Dictionary<int, int> userCart = new Dictionary<int, int>();
                HttpContext.Session.SetString("CurrentSessionUser", JsonConvert.SerializeObject(currentUser));
                HttpContext.Session.SetString("CurrentSessionUserCart", JsonConvert.SerializeObject(userCart));

                return View(_BusinessModel.GetLocationsList());
            }
            else
            {
                return RedirectToAction("LoginFailPage", "Account");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LocationSelected(int? storeLocationId)
        {

            Location currentLoc = _BusinessModel.GetLocation((int)storeLocationId);

            HttpContext.Session.SetString("CurrentSessionLocation", JsonConvert.SerializeObject(currentLoc));

            return RedirectToAction("HomePage", "Main");
        }

        public ActionResult CreateAccount()
        {
            return View();
        }

        public ActionResult CreateAccountLanding(Customer newUser)
        {
            _BusinessModel.CreateAccount(newUser);

            return RedirectToAction("LoginPage", "Account");
        }

        public ActionResult LoginFailPage()
        {
            return View();
        }
    }
}
