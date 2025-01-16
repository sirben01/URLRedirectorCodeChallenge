using Microsoft.AspNetCore.Mvc;

namespace BlueModus.Web.Controllers
{
    public class CampaignsController : Controller
    {
        [Route("campaigns/targetcampaign")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("campaigns/targetcampaign/channelB")]
        public IActionResult ChannelB()
        {
            return View();
        }
    }
}
