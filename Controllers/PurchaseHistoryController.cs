using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ST10362208_CLDV6221_POE.REDO.Controllers
{
    public class PurchaseHistoryController : Controller
    {
        private readonly Db _context;

        public PurchaseHistoryController(Db context)
        {
            _context = context;
        }

        public async Task<IActionResult> purchaseHistory()
        {
            var loggedInUser = HttpContext.Session.GetString("LoggedInUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loggedInUser);
            if (user == null)
            {//(see  ASP.Net Core: 04 - User Login Create Manage ASP.Net Core. 2018).  

                return RedirectToAction("Index", "Home");
            }

            var transactions = await _context.Transactions
                .Where(t => t.UserID == user.UserID)
                .ToListAsync();

            return View(transactions); // Return purchase history view
        }

        // Other actions related to purchase history can be added here
    }
}
//ASP.Net Core: 04 - User Login Create Manage ASP.Net Core. 2018. YouTube video, added by AngelSix.[Online].Available at: https://www.youtube.com/watch?v=WQywatfis6s [Accessed: 22 June 2024].

//ASP.Net Core: 05 - Client Login Token Authentication. 2018. YouTube video, added by AngelSix.[Online].Available at: https://www.youtube.com/watch?v=h5Y7GMVHptQ [Accessed: 22 June 2024].

//Introduction to ASP.NET Core MVC (.NET 8). 2023. YouTube video, added by DotNetMastery.[Online].Available at: https://www.youtube.com/watch?v=AopeJjkcRvU [Accessed: 22 June 2024].

//Stack Overflow. 2018. Cookie authentication early expiration. .[Online].Available at: https://stackoverflow.com/questions/49908610/cookie-authentication-early-expiration  [Accessed: 22 June 2024].ASP.Net Core: 04 - User Login Create Manage ASP.Net Core. 2018. YouTube video, added by AngelSix.[Online].Available at: https://www.youtube.com/watch?v=WQywatfis6s [Accessed: 22 June 2024].

//ASP.Net Core: 05 - Client Login Token Authentication. 2018. YouTube video, added by AngelSix.[Online].Available at: https://www.youtube.com/watch?v=h5Y7GMVHptQ [Accessed: 22 June 2024].

//Introduction to ASP.NET Core MVC (.NET 8). 2023. YouTube video, added by DotNetMastery.[Online].Available at: https://www.youtube.com/watch?v=AopeJjkcRvU [Accessed: 22 June 2024].

//Stack Overflow. 2018. Cookie authentication early expiration. .[Online].Available at: https://stackoverflow.com/questions/49908610/cookie-authentication-early-expiration  [Accessed: 22 June 2024].