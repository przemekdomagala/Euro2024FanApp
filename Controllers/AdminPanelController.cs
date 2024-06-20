using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Drawing.Printing;
using Euro2024REST.Data;
using Euro2024REST.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace Euro2024REST.Controllers;

public class AdminPanelController: Controller
{
    private readonly ILogger<AdminPanelController> _logger;
    private readonly ContextDb _context;
    
    public AdminPanelController(ILogger<AdminPanelController> logger, ContextDb context)
    {
        _logger = logger;
        _context = context;
    }
    
    [HttpPost]
    [ActionName("Index")]
    public IActionResult Index(string session="")
    {
        if (session.Equals("Admin"))
        {
            ViewBag.returnurl = "Admin";
        }
        return View("~/Views/AdminPanel/Index.cshtml");
    }

    [HttpPost]
    [ActionName("Tables")]
    public async Task<ActionResult<IEnumerable<User>>> Tables(string toggle)
    {
        Console.WriteLine(toggle);
        if (toggle.Equals("View Users"))
        {
            ViewData["matches"] = null;
            ViewData["teams"] = null;
            ViewData["players"] = null;
            var users = await _context.User.ToListAsync();
            ViewData["users"] = users;
        }
        else if (toggle.Equals("View Matches"))
        {
            ViewData["users"] = null;
            ViewData["teams"] = null;
            ViewData["players"] = null;
            var matches = await _context.Match.ToListAsync();
            ViewData["matches"] = matches;
        }
        else if (toggle.Equals("View Teams"))
        {
            ViewData["users"] = null;
            ViewData["matches"] = null;
            ViewData["players"] = null;
            var teams = await _context.Team.ToListAsync();
            ViewData["teams"] = teams;
        }
        else if (toggle.Equals("View Players"))
        {
            ViewData["users"] = null;
            ViewData["matches"] = null;
            ViewData["teams"] = null;
            var players = await _context.Player.ToListAsync();
            ViewData["players"] = players;
        }
        ViewBag.returnurl = "Admin";
        return View("~/Views/AdminPanel/Index.cshtml");
    }
    
    [HttpPost]
    [ActionName("DeleteUser")]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        var user = await _context.User.FindAsync(userId);
        if (user != null)
        {
            ViewData["Success"] = "deleted user " + userId;
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
        }
        ViewBag.returnurl = "Admin";
        return View("~/Views/AdminPanel/Index.cshtml");
    }
    
    [HttpPost]
    [ActionName("ModifyUser")]
    public async Task<IActionResult> ModifyUser(string username, string password, UserType type, string additionalParam)
    {
        try
        {
            var user = await _context.User.FindAsync(additionalParam);

            if (user == null)
            {
                Console.WriteLine($"User with username {additionalParam} not found.");
                ViewBag.ErrorMessage = "User not found.";
                return View("~/Views/AdminPanel/Index.cshtml");
            }

            var newUser = new User
            {
                Username = username,
                Password = HashPassword(password, username),
                Type = type,
            };

            _context.User.Remove(user);
            _context.User.Add(newUser);
            ViewData["success"] = "modified user "+additionalParam;
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            ViewData["Error"] = "Error message text.";
        }

        ViewBag.returnurl = "Admin";
        return View("~/Views/AdminPanel/Index.cshtml");
    }

    [HttpPost]
    [ActionName("ModifyMatch")]
    public async Task<IActionResult> ModifyMatch(string team1, string team2, int score1, int score2, DateTime date, string venue, 
        string additionalParam, string phase)
    {
        try
        {
            Console.WriteLine(additionalParam);
            //string actualId = additionalParam[0..^1];
            int id = int.Parse(additionalParam);
            var match = await _context.Match.FindAsync(id);

            if (match == null)
            {
                Console.WriteLine($"Match with id {id} not found.");
                ViewBag.ErrorMessage = "Match not found.";
                return View("~/Views/AdminPanel/Index.cshtml");
            }

            // match.Team1 = team1;
            // match.Team2 = team2;
            match.Team1 = _context.Team.FindAsync(team1).Result.Name;
            match.Team2 = _context.Team.FindAsync(team2).Result.Name;
            match.Score1 = score1;
            match.Score2 = score2;
            match.Date = date;
            match.Venue = venue;
            match.Phase = phase;
            ViewData["success"] = "modified match "+additionalParam;
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            Console.WriteLine("BLOND");
            ViewData["Error"] = "Error message text.";
        }

        ViewBag.returnurl = "Admin";
        return View("~/Views/AdminPanel/Index.cshtml");
    }
    
    [HttpPost]
    [ActionName("DeleteMatch")]
    public async Task<IActionResult> DeleteMatch(int matchId)
    {
        var match = await _context.Match.FindAsync(matchId);
        if (match != null)
        {
            ViewData["Success"] = "deleted match "+match.Team1 + " - " + match.Team2;
            _context.Match.Remove(match);
            await _context.SaveChangesAsync();
        }
        ViewBag.returnurl = "Admin";
        return View("~/Views/AdminPanel/Index.cshtml");
    }
    
    [HttpPost]
    [ActionName("ModifyTeam")]
    public async Task<IActionResult> ModifyTeam(string name, int rating, string additionalParam)
    {
        try
        {
            Console.WriteLine(additionalParam);
            var team = await _context.Team.FindAsync(additionalParam);

            if (team == null)
            {
                Console.WriteLine($"Team with name {additionalParam} not found.");
                ViewBag.ErrorMessage = "Team not found.";
                return View("~/Views/AdminPanel/Index.cshtml");
            }

            var newTeam = new Team
            {
                Name = name,
                Rating = rating,
            };

            var oldTeam = team.Name;

            _context.Team.Remove(team);
            _context.Team.Add(newTeam);

            var matches = await _context.Match.ToListAsync();
            foreach (var match in matches)
            {
                if (match.Team1.Equals(oldTeam))
                {
                    match.Team1 = newTeam.Name;
                    await _context.SaveChangesAsync();
                }
                else if (match.Team2.Equals(oldTeam))
                {
                    match.Team2 = newTeam.Name;
                    await _context.SaveChangesAsync();
                }
            }

            var players = await _context.Player.ToListAsync();
            foreach (var player in players)
            {
                if (player.Team.Equals(oldTeam))
                {
                    player.Team = newTeam.Name;
                    await _context.SaveChangesAsync();
                }
            }
            ViewData["success"] = "modified team "+additionalParam;
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            ViewData["Error"] = "Error message text.";
        }

        ViewBag.returnurl = "Admin";
        return View("~/Views/AdminPanel/Index.cshtml");
    }
    
    [HttpPost]
    [ActionName("DeleteTeam")]
    public async Task<IActionResult> DeleteTeam(string teamId)
    {
        var team = await _context.Team.FindAsync(teamId);
        if (team != null)
        {
            ViewData["Success"] = "deleted team "+teamId;
            _context.Team.Remove(team);
            await _context.SaveChangesAsync();
        }
        ViewBag.returnurl = "Admin";
        return View("~/Views/AdminPanel/Index.cshtml");
    }
    
    [HttpPost]
    [ActionName("ModifyPlayer")]
    public async Task<IActionResult> ModifyPlayer(string name, string additionalParam, int age, int height, int marketvalue, string team, string position,
        string club)
    {
        try
        {
            string trimmedName = additionalParam;
            Console.WriteLine(trimmedName);
            var player = await _context.Player.FindAsync(trimmedName);

            if (player == null)
            {
                Console.WriteLine($"Player with name {trimmedName} not found.");
                ViewBag.ErrorMessage = "Player not found.";
                return View("~/Views/AdminPanel/Index.cshtml");
            }

            var newPlayer = new Player
            {
                Name = name,
                Age = age,
                Height = height,
                MarketValue = marketvalue,
                Team = team,
                Position = position,
                Club = club,
            };

            _context.Player.Remove(player);
            _context.Player.Add(newPlayer);
            ViewData["success"] = "modified player "+additionalParam;
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            ViewData["Error"] = "Error message text.";
        }

        ViewBag.returnurl = "Admin";
        return View("~/Views/AdminPanel/Index.cshtml");
    }
    
    [HttpPost]
    [ActionName("DeletePlayer")]
    public async Task<IActionResult> DeletePlayer(string playerId)
    {
        var player = await _context.Player.FindAsync(playerId);
        if (player != null)
        {
            ViewData["Success"] = "deleted player "+playerId;
            _context.Player.Remove(player);
            await _context.SaveChangesAsync();
        }
        ViewBag.returnurl = "Admin";
        return View("~/Views/AdminPanel/Index.cshtml");
    }
    
    [HttpPost]
    [ActionName("ChooseModify")]
    public async Task<IActionResult> ChooseModify(string arg)
    {
        if (arg.Contains("ModifyUser"))
        {
            ViewData["modifyMatch"] = null;
            ViewData["modifyTeam"] = null;
            ViewData["modifyPlayer"] = null;
            string name = arg.Split(" ")[1];
            ViewData["modifyUser"] = _context.User.FindAsync(name).Result;
        }
        else if (arg.Contains("ModifyMatch"))
        {
            ViewData["modifyUser"] = null;
            ViewData["modifyTeam"] = null;
            ViewData["modifyPlayer"] = null;
            
            string prefix = "ModifyMatch ";
            if (arg.StartsWith(prefix)) {
                string idPart = arg.Substring(prefix.Length);
                if (int.TryParse(idPart, out int id))
                {
                    ViewData["modifyMatch"] = _context.Match.FindAsync(id).Result;
                }
                else
                {
                    Console.WriteLine("No valid ID found in the string.");
                }
            }
            else
            {
                Console.WriteLine("The input string does not start with the expected prefix.");
            }
        }
        else if (arg.Contains("ModifyTeam"))
        {
            var name = arg.Split(" ")[1];
            ViewData["modifyUser"] = null;
            ViewData["modifyMatch"] = null;
            ViewData["modifyPlayer"] = null;
            ViewData["modifyTeam"] = _context.Team.FindAsync(name).Result;
            Console.WriteLine(_context.Team.FindAsync(arg).Result);
        }
        else if (arg.Contains("ModifyPlayer"))
        {
            var name = arg.Replace("ModifyPlayer ", "");
            ViewData["modifyUser"] = null;
            ViewData["modifyMatch"] = null;
            ViewData["modifyTeam"] = null;
            ViewData["modifyPlayer"] = _context.Player.FindAsync(name).Result;
            var player = ViewData["modifyPlayer"] as Player;
            Console.WriteLine(player.Name);
        }
        ViewBag.returnurl = "Admin";
        return View("~/Views/AdminPanel/Index.cshtml");
    }

    [HttpPost]
    [ActionName("ChooseAdd")]
    public async Task<IActionResult> ChooseAdd(string arg)
    {
        if (arg.Equals("AddUser"))
        {
            ViewData["addUser"] = "true";
        }
        else if (arg.Equals("AddMatch"))
        {
            ViewData["addMatch"] = "true";
        }
        else if (arg.Equals("AddTeam"))
        {
            ViewData["addTeam"] = "true";
        }
        else if (arg.Equals("AddPlayer"))
        {
            ViewData["addPlayer"] = "true";
        }
        ViewBag.returnurl = "Admin";
        return View("~/Views/AdminPanel/Index.cshtml");
    }
    
    private static string HashPassword(string password, string salt)
    {
        var pbkdf2 = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(salt), 10000, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(20);
        return Convert.ToBase64String(hash);
    }
    
    [HttpPost]
    [ActionName("AddUser")]
    public async Task<IActionResult> AddUser(string username, string password, UserType type)
    {
        User user = new User();
        user.Username = username;
        user.Password = HashPassword(password, username);
        user.Type = type;
        _context.User.Add(user);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            ViewData["Error"] = "Error message text.";
        }
        ViewBag.returnurl = "Admin";
        return View("~/Views/AdminPanel/Index.cshtml");
    }
    
    [HttpPost]
    [ActionName("AddTeam")]
    public async Task<IActionResult> AddTeam(string name, int rating)
    {
        Team team = new Team();
        team.Name = name;
        team.Rating = rating;
        _context.Team.Add(team);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            ViewData["Error"] = "Error message text.";
        }
        ViewBag.returnurl = "Admin";
        return View("~/Views/AdminPanel/Index.cshtml");
    }

    [HttpPost]
    [ActionName("AddPlayer")]
    public async Task<IActionResult> AddPlayer(string name, int age, int height, int marketvalue, string team, string position, string club)
    {
        try {
            Player player = new Player();
            player.Name = name;
            player.Age = age;
            player.Height = height;
            player.MarketValue = marketvalue;
            player.Team = _context.Team.FindAsync(team).Result.Name;
            player.Position = position;
            player.Club = club;
            _context.Player.Add(player);
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            ViewData["Error"] = "Error message text.";
        }
        ViewBag.returnurl = "Admin";
        return View("~/Views/AdminPanel/Index.cshtml");
    }

    [HttpPost]
    [ActionName("AddMatch")]
    public async Task<IActionResult> AddMatch(int id, string team1, string team2, int score1, int score2, DateTime date, string venue, string phase)
    {
        try
        {
            Match match = new Match();
            match.Id = id;
            match.Team1 = _context.Team.FindAsync(team1).Result.Name;
            match.Team2 = _context.Team.FindAsync(team2).Result.Name;
            match.Score1 = score1;
            match.Score2 = score2;
            match.Phase = phase;
            // Ensure the DateTime is in UTC
            if (date.Kind == DateTimeKind.Unspecified)
            {
                date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
            }
            else if (date.Kind != DateTimeKind.Utc)
            {
                date = date.ToUniversalTime();
            }
            match.Date = date;
            match.Venue = venue;
            _context.Match.Add(match);

            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            ViewData["Error"] = "Error message text.";
        }
        ViewBag.returnurl = "Admin";
        return View("~/Views/AdminPanel/Index.cshtml");
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}