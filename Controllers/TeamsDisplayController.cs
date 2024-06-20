using Euro2024REST.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Euro2024REST.Controllers;

public class TeamsDisplayController : Controller
{
    private readonly ILogger<TeamsDisplayController> _logger;
    private readonly ContextDb _context;

    public TeamsDisplayController(ILogger<TeamsDisplayController> logger, ContextDb context)
    {
        _logger = logger;
        _context = context;
    }
    
    [HttpPost]
    [ActionName("Index")]
    public async Task<IActionResult> Index(string session)
    {
        if (session.Equals("Admin"))
        {
            ViewBag.returnurl = "Admin";   
        }
        else if (session.Equals("User"))
        {
            ViewBag.returnurl = "User";
        }
        var teams = await _context.Team.ToListAsync();
        ViewData["teamsDisplay"] = teams;
        var players = await _context.Player.ToListAsync();
        ViewData["playersDisplay"] = players;
        return View("~/Views/TeamsDisplay/Index.cshtml");
    }
}