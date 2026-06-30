using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotelo.Web.Areas.Reservation.Controllers;

[Area("Reservation")]
[Authorize]
public class PackagesController : Controller
{
    public IActionResult Index() => View();
}
