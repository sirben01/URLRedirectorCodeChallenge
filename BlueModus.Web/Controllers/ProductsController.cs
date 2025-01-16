using Microsoft.AspNetCore.Mvc;

namespace BlueModus.Web.Controllers
{
    public class ProductsController : Controller
    {
        [Route("products/{category}/{type}/{item}")]
        public IActionResult Index(string category, string type, string item)
        {
            ViewBag.Category = category;
            ViewBag.Type = type;
            ViewBag.Item = item;

            return View();
        }
    }
}
