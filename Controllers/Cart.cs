using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST10362208_CLDV6221_POE.REDO.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ST10362208_CLDV6221_POE.REDO.Controllers
{
    public class CartController : Controller
    {
        private readonly Db _context;

        public CartController(Db context)
        {
            _context = context;
        }

        public IActionResult purchaseHistory()
        {
            return View();
        }

        // POST: /Cart/AddToCart
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId)
        {
            try
            {
                var loggedInUser = HttpContext.Session.GetString("LoggedInUser");
                if (loggedInUser == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loggedInUser);
                if (user == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                var product = await _context.Products.FindAsync(productId);
                if (product == null)
                {
                    return NotFound(); // Handle the case where product is not found
                }

                // Calculate VAT-inclusive price
                decimal productPriceWithVAT = (decimal)product.Price * 1.15m;

                var cartItem = new Cart
                {
                    UserId = user.UserID,
                    CreatedAt = DateTime.Now,
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ProductDescription = product.Description,
                    ProductPrice = (decimal)product.Price,
                    Quantity = 1
                };

                _context.Cart.Add(cartItem);
                await _context.SaveChangesAsync();

                return RedirectToAction("Mywork", "Mywork");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }



        public async Task<IActionResult> Cart()
        {
            var loggedInUser = HttpContext.Session.GetString("LoggedInUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loggedInUser);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var cartItems = await _context.Cart
                .Include(c => c.Product)
                .Where(c => c.UserId == user.UserID)//display cart
                .ToListAsync();

            decimal subtotal = cartItems.Sum(c => c.ProductPrice * c.Quantity);
            decimal vatAmount = cartItems.Sum(c => c.ProductPrice * c.Quantity * 0.15m);
            decimal total = subtotal + vatAmount;

            var viewModel = new ReceiveAllData
            {
                CartItems = cartItems,
                Subtotal = subtotal,
                VatAmount = vatAmount,
                Total = total
            };

            return View(viewModel);
        }


        [HttpGet]
        public async Task<IActionResult> GetCartItemCount()
        {
            var loggedInUser = HttpContext.Session.GetString("LoggedInUser");
            if (loggedInUser == null)
            {
                return Json(0); // Return 0 if user is not logged in or has no cart items
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loggedInUser);
            if (user == null)
            {
                return Json(0); // Return 0 if user is not found
            }

            var cartItemCount = await _context.Cart
                .Where(c => c.UserId == user.UserID)
                .SumAsync(c => c.Quantity);

            return Json(cartItemCount); // Return the total count of cart items
        }

        // POST: /Cart/RemoveFromCart
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            try
            {
                var cartItem = await _context.Cart.FindAsync(cartItemId);
                if (cartItem == null)
                {
                    return NotFound(); // Handle the case where cart item is not found
                }

                _context.Cart.Remove(cartItem);
                await _context.SaveChangesAsync();

                return RedirectToAction("Cart"); // Redirect back to cart after removal
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine(ex);
                throw; // Rethrow the exception to display error page or handle it
            }
        }

        [HttpPost]
        public async Task<IActionResult> CompleteTransaction()
        {
            var loggedInUser = HttpContext.Session.GetString("LoggedInUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Cart");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loggedInUser);
            if (user == null)
            {
                return RedirectToAction("Cart");
            }

            var cartItems = await _context.Cart
                .Where(c => c.UserId == user.UserID)//(see   Introduction to ASP.NET Core MVC (.NET 8). 2023).  

                .ToListAsync();

            if (!cartItems.Any())
            {
                return RedirectToAction("Cart");
            }

            decimal subtotal = cartItems.Sum(c => c.ProductPrice * c.Quantity);
            decimal vatAmount = subtotal * 0.15m;
            decimal total = subtotal + vatAmount;

            // Create transactions and add them to Transactions table
            foreach (var item in cartItems)
            {
                var transaction = new Transactions
                {
                    UserID = item.UserId,
                    ProductID = item.ProductId,
                    Quantity = item.Quantity,
                    TotalPrice = item.ProductPrice * item.Quantity,
                    TransactionDate = DateTime.Now
                };

                _context.Transactions.Add(transaction);
            }

            _context.Cart.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            // Set success message in TempData
            TempData["TransactionCompleted"] = "Transaction completed successfully.";

            // Redirect back to the Cart page after completing transaction
            return RedirectToAction("Cart");
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