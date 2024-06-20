using Euro2024REST.Data;
using Euro2024REST.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Euro2024REST.Controllers;

public class PredictionsController : Controller
{
    private readonly ILogger<PredictionsController> _logger;
    private readonly ContextDb _context;

    public PredictionsController(ILogger<PredictionsController> logger, ContextDb context)
    {
        _logger = logger;
        _context = context;
    }
    

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> UpdateOrder(string sortedData)
    {
        var sortedPredictions = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SortedPrediction>>(sortedData);
        foreach (var el in sortedPredictions)
        {
            var prediction = await _context.Predictions
                .Where(m => m.Team == el.team).Where(
                    u => u.Username == User.Identity.Name)
                .ToListAsync();
            foreach(var p in prediction)
            {
                p.Place = el.place;
                await _context.SaveChangesAsync();
                //_context.Predictions.Update(p);
            }
        }
        
        if(User.Identity.Name.Equals("admin"))
        {
            ViewBag.returnurl = "Admin";
        }
        else
        {
            ViewBag.returnurl = "User";
        }

        return View("~/Views/UserPanel/Index.cshtml");
    }

    public class SortedPrediction
    {
        public string team { get; set; }
        public string phase { get; set; }
        public int place { get; set; }
    }


    
    [HttpPost]
    [Authorize]
    [ActionName("Index")]
    public async Task<IActionResult> Index(string session)
    {
        Console.WriteLine(User.Identity.Name);
        if (session.Equals("Admin"))
        {
            ViewBag.returnurl = "Admin";   
        }
        else if (session.Equals("User"))
        {
            ViewBag.returnurl = "User";
        }
        
        var predictions = await _context.Predictions.Where(U => U.Username == User.Identity.Name).OrderBy(m => m.Phase).ToListAsync();
        if (predictions.Count()>0)
        {
            ViewData["predictions"] = predictions;
            Console.WriteLine("not null");
        }
        else
        {
            Console.WriteLine("null");
            var distinctTeamsAndPhases = await _context.Match
                .Where(m => m.Team1 != "TBD")
                .Select(m => new { m.Team1, m.Phase })
                .Distinct()
                .OrderBy(m => m.Phase)
                .ToListAsync();
            int i = 0;
            foreach (var pair in distinctTeamsAndPhases)
            {
                var Prediction = new Predictions();
                Prediction.Username = User.Identity.Name;
                Prediction.Team = pair.Team1;
                Prediction.Phase = pair.Phase;
                Prediction.Place = i%4+1;
                i += 1;
                _context.Predictions.Add(Prediction);
                await _context.SaveChangesAsync();
            }
            predictions = await _context.Predictions.Where(U => U.Username == User.Identity.Name).ToListAsync();
            ViewData["predictions"] = predictions;
        }
        
        return View("~/Views/Predictions/Index.cshtml");
    }
}