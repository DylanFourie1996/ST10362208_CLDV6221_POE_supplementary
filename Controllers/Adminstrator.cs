using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using ST10362208_CLDV6221_POE.REDO.Models;
using System.Linq;

namespace ST10362208_CLDV6221_POE.REDO.Controllers
{
    public class Adminstrator : Controller
    {
        private readonly Db _db;

        public Adminstrator(Db db)
        {
            _db = db;
        }
        public IActionResult Admin()
        {
            string isAdminCookie = HttpContext.Request.Cookies["isAdmin"];
            bool isAdmin = !string.IsNullOrEmpty(isAdminCookie) && isAdminCookie == "1";// check if user is admin

            if (!isAdmin)
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var clients = _db.Users.ToList();
            var products = _db.Products.ToList();

            var viewModel = new ReceiveAllData
            {
                Clients = clients,
                Products = products,
                NewProduct = new Product()
            };

            return View(viewModel);
        }

        [HttpPost]//Addd product
        public IActionResult AddProduct(Product newProduct, IFormFile ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName); // paths to file to uplaod image

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        ImageFile.CopyTo(stream);
                    }

                    newProduct.ImageUrl = "/images/" + fileName;
                }

                newProduct.ProductKey = Guid.NewGuid();
                newProduct.DateCreated = DateTime.Now;
                _db.Products.Add(newProduct);
                _db.SaveChanges();

                return RedirectToAction("Admin");//(see  ASP.Net Core: 05 - Client Login Token Authentication. 2018).  
            }

            var viewModel = new ReceiveAllData
            {
                Clients = _db.Users.ToList(),
                Products = _db.Products.ToList(),
                NewProduct = newProduct
            };

            return View("Admin", viewModel);
        }
        [HttpPost]//Delete Data
        public IActionResult DeleteProduct(int id)
        {
            var product = _db.Products.Find(id);//looks for prodcut ID in the table
            if (product != null)
            {
                // Find and delete cart items associated with the product
                var cartItems = _db.Cart.Where(c => c.ProductId == id).ToList();
                _db.Cart.RemoveRange(cartItems);

                // Find and delete transactions associated with the product
                var transactions = _db.Transactions.Where(t => t.ProductID == id).ToList();
                _db.Transactions.RemoveRange(transactions);

                // Remove the product
                _db.Products.Remove(product);

                // Save changes
                _db.SaveChanges();
            }
            return RedirectToAction("Admin");
        }
        //(see   Introduction to ASP.NET Core MVC (.NET 8). 2023).  

        [HttpPost]
        public IActionResult DeleteClient(int id)//Detlet Client
        {
            var client = _db.Users.Find(id);//Looks for clinet 
            if (client != null)
            {
                // Find and delete client's cart items
                var cartItems = _db.Cart.Where(c => c.UserId == id).ToList();
                _db.Cart.RemoveRange(cartItems);

                // Find and delete client's transactions
                var transactions = _db.Transactions.Where(t => t.UserID == id).ToList();
                _db.Transactions.RemoveRange(transactions);

                // Remove client
                _db.Users.Remove(client);

                // Save changes
                _db.SaveChanges();
            }
            return RedirectToAction("Admin");
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