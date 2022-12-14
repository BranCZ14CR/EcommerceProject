using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EcommerceProject.Data;
using EcommerceProject.Models;
using Microsoft.AspNetCore.Authorization;
using EcommerceProject.Data.Static;

namespace EcommerceProject.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class CinemasController : Controller
    {
        //Variables
        private readonly EcommerceDbContext _context;

        //Constructor
        public CinemasController(EcommerceDbContext context)
        {
            _context = context;
        }


        //Obtener todos los cines en una lista
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cinemas.ToListAsync());
        }


        //Obtener un cine en especifico
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var cinema = await _context.Cinemas
                .FirstOrDefaultAsync(m => m.CinemaId == id);
            if (cinema == null)
            {
                return View("NotFound");
            }

            return View(cinema);
        }


        //Ir a la pagina de crear un cine
        public IActionResult Create()
        {
            return View();
        }


        //Método para agregar un cine a la BD
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CinemaId,Logo,Nombre,Descripcion")] Cinema cinema)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cinema);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cinema);
        }


        //Ir a la página de editar un cine
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var cinema = await _context.Cinemas.FindAsync(id);
            if (cinema == null)
            {
                return View("NotFound");
            }
            return View(cinema);
        }


        //Método para actualizar un cine a la BD
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CinemaId,Logo,Nombre,Descripcion")] Cinema cinema)
        {
            if (id != cinema.CinemaId)
            {
                return View("NotFound");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cinema);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CinemaExists(cinema.CinemaId))
                    {
                        return View("NotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cinema);
        }


        //Ir a la página de eliminar un cine
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var cinema = await _context.Cinemas
                .FirstOrDefaultAsync(m => m.CinemaId == id);
            if (cinema == null)
            {
                return View("NotFound");
            }

            return View(cinema);
        }


        //Método para eliminar un cine a la BD
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cinema = await _context.Cinemas.FindAsync(id);
            _context.Cinemas.Remove(cinema);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        //Método para ver si existe un cine
        private bool CinemaExists(int id)
        {
            return _context.Cinemas.Any(e => e.CinemaId == id);
        }
    }
}
