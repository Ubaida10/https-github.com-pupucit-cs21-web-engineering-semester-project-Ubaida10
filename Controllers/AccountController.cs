using System.Data;
using System.Net;
using CineTix.Data;
using CineTix.Models.Interfaces;
using CineTix.Models.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CineTix.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _repository;

        public AccountController(IUserRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public IActionResult Login(Login loginModel)
        {
            var userTable = _repository.GetUsers();
            DataRow foundUser = null;
            
            //default value is user
            string role = "user";
            string firstName = null;
            string lastName = null;
            foreach (DataRow user in userTable.Rows)
            {
                if (user["Email"].ToString() == loginModel.Email && user["Password"].ToString() == loginModel.Password)
                {
                    role = user["Role"].ToString();
                    firstName = user["FirstName"].ToString();
                    lastName = user["LastName"].ToString();
                    foundUser = user;
                    break;
                }
            }

            if (foundUser == null)
            {
                // Redirect to a different page with an error message
                TempData["ErrorMessage"] = "Invalid email or password.";
                return RedirectToAction("AccessDenied");
            }

            if (loginModel.RememberMe)
            {
                var cookieOption = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(7)
                };
                Response.Cookies.Append("Email", loginModel.Email, cookieOption);
                Response.Cookies.Append("Password", loginModel.Password, cookieOption);
            }
            // Redirect to a different page indicating successful login
            HttpContext.Session.SetString("UserEmail", loginModel.Email);
            HttpContext.Session.SetString("UserRole", role);
            HttpContext.Session.SetString("FirstName", firstName);
            HttpContext.Session.SetString("LastName", lastName);
            HttpContext.Session.SetString("Password", loginModel.Password);
            
            return RedirectToAction("Index","Home");
        }
        
        // GET
        public IActionResult Login()
        {
            return View(new Login());
        }

        public IActionResult RegisterCompleted()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult LoginComplete()
        {
            return View();
        }

        public IActionResult LoginFailure()
        {
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View();
        }

        [HttpPost]
        public IActionResult Register(Register register)
        {
            if (!register.EmailAddress.Contains("@"))
            {
                return View("Register", register);
            }
            else
            {
                string[] words = register.EmailAddress.Split("@");
                if (words.Length != 2 || !words[1].Contains("."))
                {
                    return View("Register", register);    
                }
            }



            if (register.Password != register.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Passwords do not match.");
                return View("Register", register);
            }

            var userTable = _repository.GetUsers();
            DataRow foundUser = null;
            foreach (DataRow user in userTable.Rows)
            {
                if (user["Email"].ToString() == register.EmailAddress)
                {
                    foundUser = user;
                    break;
                }
            }
            if (foundUser != null)
            {
                ModelState.AddModelError("", "User with this email already exists. Instead Opt for Login.");
                return View("Register", register);
            }
            if (register.RememberMe)
            {
                var cookieOption = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(7)
                };
                Response.Cookies.Append("Email", register.EmailAddress, cookieOption);
                Response.Cookies.Append("Password", register.Password, cookieOption);
            }
            _repository.AddUser(register);
            return RedirectToAction("RegisterCompleted");
        }

        public IActionResult Register()
        {
            return View(new Register());
        }
        public IActionResult Dashboard()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }
        
        public IActionResult Logout()
        {
            //Clear the session
            HttpContext.Session.Clear();
            
            //Redirect to homepage
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ViewProfile()
        {
            return View();
        }
        
        [HttpGet("Account/Edit/{id}")]
        public IActionResult Edit(string id)
        {
            DataTable userTable = _repository.GetUsers();
            foreach (DataRow row in userTable.Rows)
            {
                if(row["Email"].ToString() == id)
                {
                    var editUser = new Register();
                    //editUser.Id = Convert.ToInt32(row["Id"]);
                    editUser.EmailAddress = row["Email"].ToString();
                    editUser.FirstName = row["FirstName"].ToString();
                    editUser.LastName = row["LastName"].ToString();
                    editUser.Password = row["Password"].ToString();
                    return View("Edit", editUser);
                }
            }
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public IActionResult Edit(Register register)
        {
            _repository.Edit(register);
            return RedirectToAction("Index","Home");
        }
        
        public IActionResult Delete()
        {
            if (!String.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail")))
            {
                var user = HttpContext.Session.GetString("UserEmail");
                if (user != null)
                {
                    _repository.Delete(user);
                    HttpContext.Session.Clear();
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
