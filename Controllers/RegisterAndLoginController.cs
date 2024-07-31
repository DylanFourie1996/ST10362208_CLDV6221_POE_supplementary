using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ST10362208_CLDV6221_POE.REDO.Models;
using System;
using System.Linq;

namespace ST10362208_CLDV6221_POE.REDO.Controllers
{
    public class RegisterAndLoginController : Controller
    {
        private readonly Db _context;

        public RegisterAndLoginController(Db context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register(string registerUsername, string registerEmail, string registerPassword, string registerType)
        {
            if (ModelState.IsValid)
            {
                if (_context.Users.Any(u => u.Email == registerEmail))
                {
                    // User with this email already exists, show error message
                    ViewBag.Error = "User with this email already exists.";
                    return RedirectToAction("Index", "Home");
                }

                bool isAdmin = registerType == "admin"; // Determine if user is registering as admin

                var newUser = new Client
                {
                    Username = registerUsername,
                    Email = registerEmail,
                    Password = registerPassword,
                    IsAdmin = isAdmin, // Set IsAdmin property based on registration type
                    CreatedAt = DateTime.Now,
                };

                _context.Users.Add(newUser);
                _context.SaveChanges();

                // Redirect to home page after successful registration
                return RedirectToAction("Index", "Home");
            }

            // If registration fails (ModelState invalid), return to the registration page with validation errors
            ViewBag.Error = "Invalid registration details.";
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public IActionResult Login(string loginUsername, string loginPassword)
        {
            // Check if user exists by username or email and password
            var user = _context.Users.FirstOrDefault(u =>
                (u.Username == loginUsername || u.Email == loginUsername) &&
                u.Password == loginPassword);

            if (user != null)
            {
                // Check if the user is admin
                bool isAdmin = user.IsAdmin;

                // Generate a persistent token (could be a GUID or any unique identifier)
                string persistentToken = Guid.NewGuid().ToString();

                // Store the token and admin status in cookies
                HttpContext.Response.Cookies.Append("persistentToken", persistentToken);

                // Set isAdmin cookie to "1" if user is admin
                if (isAdmin)
                {
                    HttpContext.Response.Cookies.Append("isAdmin", "1");
                }
                else
                {
                    // Clear isAdmin cookie if user is not admin
                    Response.Cookies.Delete("isAdmin");
                }

                // Set session variable to indicate user is logged in
                HttpContext.Session.SetString("LoggedInUser", user.Username);

                // Redirect to home page after successful login
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Handle invalid credentials
                ViewBag.Error = "Invalid username or password.";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult Logout()
        {
            try
            {
                // Clear session
                HttpContext.Session.Clear();
                Console.WriteLine("Session cleared");

                // Delete cookies if present
                if (Request.Cookies["isAdmin"] != null)
                {
                    Response.Cookies.Delete("isAdmin");
                    Console.WriteLine("isAdmin cookie deleted");
                }
                if (Request.Cookies["persistentToken"] != null)
                {
                    Response.Cookies.Delete("persistentToken");
                    Console.WriteLine("persistentToken cookie deleted");
                }

                // Redirect to home page
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Logout error: " + ex.Message);
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
//ASP.Net Core: 04 - User Login Create Manage ASP.Net Core. 2018. YouTube video, added by AngelSix.[Online].Available at: https://www.youtube.com/watch?v=WQywatfis6s [Accessed: 22 June 2024].

//ASP.Net Core: 05 - Client Login Token Authentication. 2018. YouTube video, added by AngelSix.[Online].Available at: https://www.youtube.com/watch?v=h5Y7GMVHptQ [Accessed: 22 June 2024].

//Introduction to ASP.NET Core MVC (.NET 8). 2023. YouTube video, added by DotNetMastery.[Online].Available at: https://www.youtube.com/watch?v=AopeJjkcRvU [Accessed: 22 June 2024].

//Stack Overflow. 2018. Cookie authentication early expiration. .[Online].Available at: https://stackoverflow.com/questions/49908610/cookie-authentication-early-expiration  [Accessed: 22 June 2024].ASP.Net Core: 04 - User Login Create Manage ASP.Net Core. 2018. YouTube video, added by AngelSix.[Online].Available at: https://www.youtube.com/watch?v=WQywatfis6s [Accessed: 22 June 2024].

//ASP.Net Core: 05 - Client Login Token Authentication. 2018. YouTube video, added by AngelSix.[Online].Available at: https://www.youtube.com/watch?v=h5Y7GMVHptQ [Accessed: 22 June 2024].

//Introduction to ASP.NET Core MVC (.NET 8). 2023. YouTube video, added by DotNetMastery.[Online].Available at: https://www.youtube.com/watch?v=AopeJjkcRvU [Accessed: 22 June 2024].

//Stack Overflow. 2018. Cookie authentication early expiration. .[Online].Available at: https://stackoverflow.com/questions/49908610/cookie-authentication-early-expiration  [Accessed: 22 June 2024].