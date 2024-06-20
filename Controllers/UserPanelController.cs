using Euro2024REST.Models;
using Microsoft.AspNetCore.Authorization;
namespace Euro2024REST.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

[Authorize]
public class UserPanelController: Controller
{
    private readonly ILogger<UserPanelController> _logger;

    public UserPanelController(ILogger<UserPanelController> logger)
    {
        _logger = logger;
    }
    
    [HttpGet]
    [Authorize]
    [ActionName("Index")]
    public IActionResult Index()
    {
        ViewBag.returnurl = "User";
        return View("~/Views/UserPanel/Index.cshtml");
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}