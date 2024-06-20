using System.Diagnostics;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Euro2024REST.Data;
using Microsoft.AspNetCore.Mvc;
using Euro2024REST.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Euro2024REST.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ContextDb _context;

    public HomeController(ILogger<HomeController> logger, ContextDb context)
    {
        _logger = logger;
        _context = context;
    }
    
    private static string HashPassword(string password, string salt)
    {
        var pbkdf2 = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(salt), 10000, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(20);
        return Convert.ToBase64String(hash);
    }

    [HttpGet]
    //[Authorize]
    [ActionName("Index")]
    public IActionResult Index()
    {
        ViewBag.returnurl = "";
        return View();
    }
    
    [HttpPost]
    [Authorize]
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
    
    [HttpPost]
    public async Task<ActionResult<User>> Index(string username, string password, string action)
    {
        if (action.Equals("Log in")) {
            try
            {
                var user =  await _context.User.FindAsync(username);
                var hashedPassword = HashPassword(password, username);
                if(hashedPassword.Equals(user.Password))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Username)
                    };
                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    
                    // Console.WriteLine(User.Identity.Name);
                    
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity));
                    
                    if (user.Type == UserType.Admin)
                    {
                        ViewBag.returnurl = "Admin";
                        return View("~/Views/UserPanel/Index.cshtml");
                    }
                    else
                    {
                        ViewBag.returnurl = "User";
                        //return View("~/Views/UserPanel/Index.cshtml");
                        return RedirectToAction("Index", "UserPanel");
                    }
                }
                else
                {
                    ViewData["Error"] = "Error message text.";
                    return View();
                }
            }
            catch (Exception e)
            {
                ViewData["Error"] = "Error message text.";
                return View();
            }
            
        }
        else if (action.Equals("Register"))
        {
            User user = new User();
            user.Username = username;
            var hashedPassword = HashPassword(password, username);
            user.Password = hashedPassword;
            user.Type = UserType.User;
            _context.User.Add(user);
            try
            {
                await _context.SaveChangesAsync();
                ViewBag.returnurl = "User";
                return View("~/Views/UserPanel/Index.cshtml");
            }
            catch (DbUpdateException)
            {
                ViewData["Error2"] = "Error message text.";
                return View();
            }
        }
        
        return View();
    }
    
    [HttpPost]
    [ActionName("HomePage")]
    public IActionResult HomePage(string session="")
    {
        if (session.Equals("Admin"))
        {
            ViewBag.returnurl = "Admin";   
        }
        else if (session.Equals("User"))
        {
            ViewBag.returnurl = "User";
        }
        return View("~/Views/UserPanel/Index.cshtml");
    }
    
    [HttpPost]
    [ActionName("Privacy")]
    public IActionResult Privacy(string session="")
    {
        if (session.Equals("Admin"))
        {
            ViewBag.returnurl = "Admin";    
        }
        else if (session.Equals("User"))
        {
            ViewBag.returnurl = "User";
        }
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

