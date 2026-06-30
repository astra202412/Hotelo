using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotelo.Web.Areas.BellDesk.Controllers;

[Area("BellDesk")]
[Authorize]
public class HomeController : Controller
{
    public IActionResult Index() => View();
}
