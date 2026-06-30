using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotelo.Web.Areas.HR.Controllers;

[Area("HR")]
[Authorize]
public class HomeController : Controller
{
    public IActionResult Index() => View();
}
