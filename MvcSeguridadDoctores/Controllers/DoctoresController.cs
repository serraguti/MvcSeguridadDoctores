﻿using Microsoft.AspNetCore.Mvc;
using MvcSeguridadDoctores.Filters;
using MvcSeguridadDoctores.Models;
using MvcSeguridadDoctores.Repositories;
using System.Collections.Specialized;
using System.Security.Claims;

namespace MvcSeguridadDoctores.Controllers
{
    public class DoctoresController : Controller
    {
        private RepositoryHospital repo;

        public DoctoresController(RepositoryHospital repo)
        {
            this.repo = repo;
        }

        [AuthorizeDoctores]
        public async Task<IActionResult> PerfilDoctor()
        {
            string data =
                HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Doctor doctor =
                await this.repo.FindDoctorAsync(int.Parse(data));
            return View(doctor);
        }

        public async Task<IActionResult> Enfermos()
        {
            List<Enfermo> enfermos =
                await this.repo.GetEnfemosAsync();
            return View(enfermos);
        }

        public async Task<IActionResult> DeleteEnfermo(int id)
        {
            Enfermo enfermo =
                await this.repo.FindEnfermoAsync(id);
            return View(enfermo);
        }

        [HttpPost]
        [ActionName("DeleteEnfermo")]
        public async Task<IActionResult> EliminarEnfermo(int id)
        {
            await this.repo.DeleteEnfermoAsync(id);
            return RedirectToAction("Enfermos");
        }
    }
}
