using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotelo.Web.Areas.Finances.Controllers;

[Area("Finances")]
[Authorize]
public class HomeController : Controller
{
    public IActionResult Index() => View();
}
