using Microsoft.AspNetCore.Mvc;
using ST10362208_CLDV6221_POE.REDO.Models;
using System.Linq;

namespace ST10362208_CLDV6221_POE.REDO.Controllers
{
    public class MyworkController : Controller
    {
        private readonly Db _db;

        public MyworkController(Db db)
        {
            _db = db;
        }
      
        public IActionResult Mywork()
        {
            var products = _db.Products.ToList();
            return View(products); // Pass the list of products to the view


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