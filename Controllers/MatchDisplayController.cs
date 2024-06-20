using System.Drawing.Printing;
using Microsoft.AspNetCore.Mvc;
using Euro2024REST.Data;
using Microsoft.EntityFrameworkCore;

namespace Euro2024REST.Controllers;

public class MatchDisplayController: Controller
{
    private readonly ILogger<MatchDisplayController> _logger;
    private readonly ContextDb _context;

    public MatchDisplayController(ILogger<MatchDisplayController> logger, ContextDb context)
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
        var matches = await _context.Match.ToListAsync();
        ViewData["matchesDisplay"] = matches;
        return View("~/Views/MatchDisplay/Index.cshtml");
    }
}