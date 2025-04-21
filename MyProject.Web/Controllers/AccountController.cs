// AccountController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyProject.Core.Models;
using MyProject.Web.ViewModels;

public class AccountController : Controller
{
    private readonly SignInManager<User> _signInManager;
    public AccountController(SignInManager<User> signInManager)
        => _signInManager = signInManager;
    [AllowAnonymous]
    [HttpGet]
    public IActionResult Login() => View();
    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(UserLoginViewModel m)
    {
        if (!ModelState.IsValid) return View(m);

        var result = await _signInManager.PasswordSignInAsync(
            m.Email, m.Password, isPersistent: false, lockoutOnFailure: false);

        if (result.Succeeded) return RedirectToAction("Account", "Login");

        ModelState.AddModelError("", "E‑posta veya şifre yanlış.");
        return View(m);
    }
    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }
}
