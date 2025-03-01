using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SIDWeb.Model;
using System.Threading.Tasks;

namespace SIDWeb.Controllers;
public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(string email, string password)
    {
        var user = new AppUser { UserName = email, Email = email };
        var result = await _userManager.CreateAsync(user, password);
        user.EmailConfirmed = true;
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "User"); // Default role
            return RedirectToAction("Login");
        }

        return View();
    }

    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var result = await _signInManager.PasswordSignInAsync(email, password, false, false);

        if (result.Succeeded)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                // Check if the user is in the Admin role
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
                else
                {
                    return RedirectToAction("Dashboard", "User"); 
                }
            }
        }

        // If login failed, return to the login view with an error message (optional)
        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View();
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }

    public IActionResult AccessDenied() => View();
}
