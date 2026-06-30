using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotelo.Web.Areas.Reservation.Controllers;

[Area("Reservation")]
[Authorize]
public class HomeController : Controller
{
    public IActionResult Index() => View();
}
