using BankID.WebDemo.Models;
using System.Web.Mvc;

namespace BankID.WebDemo.Controllers
{
    [RoutePrefix("")]
    public class SiteController : Controller
    {
        [Route("")]
        [Route("~/site")]
        [Route("authenticate")]
        [Route("~/site/authenticate")]
        [HttpGet]
        public ActionResult Authenticate()
        {
            return View(new PersonalNumberModel());
        }

        [Route("about")]
        [Route("~/site/about")]
        [HttpGet]
        public ActionResult About()
        {
            return View();
        }
    }
}