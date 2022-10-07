using FreeCourse.Web.Models;
using FreeCourse.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FreeCourse.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IIdentityService _identityService;
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SigninInPut signinInPut)
        {
            if (!ModelState.IsValid)
            {
                return View(signinInPut);
            }
            var response = await _identityService.SignIn(signinInPut);
            if (!response.IsSuccessful)
            {
                response.Errors.ForEach(x => {
                    ModelState.AddModelError(String.Empty, x);
                });
                return View();
            }

            return RedirectToAction(nameof(Index), "Home");
        }
    }
}
