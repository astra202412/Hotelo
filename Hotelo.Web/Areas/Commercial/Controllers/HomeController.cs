using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotelo.Web.Areas.Commercial.Controllers;

[Area("Commercial")]
[Authorize]
public class HomeController : Controller
{
    public IActionResult Index() => View();
}
