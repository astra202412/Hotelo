using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotelo.Web.Controllers;

[Authorize]
public class HomeController : Controller
{
    public IActionResult Index()
        => RedirectToAction("Index", "Dashboard");
}
