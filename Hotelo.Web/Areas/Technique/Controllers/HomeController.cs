using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotelo.Web.Areas.Technique.Controllers;

[Area("Technique")]
[Authorize]
public class HomeController : Controller
{
    public IActionResult Index() => View();
}
