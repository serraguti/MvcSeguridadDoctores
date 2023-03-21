using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using MvcSeguridadDoctores.Models;
using MvcSeguridadDoctores.Repositories;
using System.Security.Claims;

namespace MvcSeguridadDoctores.Controllers
{
    public class ManagedController : Controller
    {
        private RepositoryHospital repo;

        public ManagedController(RepositoryHospital repo)
        {
            this.repo = repo;
        }

        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(string username
            , string password)
        {
            Doctor doctor =
                await this.repo.ExisteDoctor(username, int.Parse(password));
            if (doctor != null)
            {
                ClaimsIdentity identity =
               new ClaimsIdentity
               (CookieAuthenticationDefaults.AuthenticationScheme
               , ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim
                    (new Claim(ClaimTypes.Name, doctor.Apellido));
                identity.AddClaim
                    (new Claim(ClaimTypes.NameIdentifier, doctor.IdDoctor.ToString()));
                identity.AddClaim
                    (new Claim(ClaimTypes.Role, doctor.Especialidad));

                ClaimsPrincipal user = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync
                    (CookieAuthenticationDefaults.AuthenticationScheme
                    , user);
                string controller = TempData["controller"].ToString();
                string action = TempData["action"].ToString();
                string id = TempData["id"].ToString();

                return RedirectToAction(action, controller
                    , new {  id = id });
                //return RedirectToAction("DeleteEnfermo"
                //    , "Doctores", new { id = 63827 });
                //return RedirectToAction("PerfilDoctor"
                //    , "Doctores", new { id = 123 });
            }
            else
            {
                ViewData["MENSAJE"] = "Usuario/Password incorrectos";
                return View();
            }
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync
                (CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Enfermos", "Doctores");
        }
    }
}
