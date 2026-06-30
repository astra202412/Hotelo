using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotelo.Web.Areas.Housekeeping.Controllers;

[Area("Housekeeping")]
[Authorize]
public class HomeController : Controller
{
    public IActionResult Index() => View();
}
