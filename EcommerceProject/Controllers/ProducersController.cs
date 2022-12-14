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
    public class ProducersController : Controller
    {
        //Variables
        private readonly EcommerceDbContext _context;


        //Constructor
        public ProducersController(EcommerceDbContext context)
        {
            _context = context;
        }


        //Obtener todos los productores en una lista
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Producers.ToListAsync());
        }


        //Obtener un productor en especifico
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var producer = await _context.Producers
                .FirstOrDefaultAsync(m => m.ProducerId == id);
            if (producer == null)
            {
                return View("NotFound");
            }

            return View(producer);
        }


        //Ir a la pagina de crear un productor
        public IActionResult Create()
        {
            return View();
        }


        //Método para agregar un productor a la BD
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProducerId,Imagen,Nombre,Biografia")] Producer producer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(producer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(producer);
        }


        //Ir a la página de editar un productor
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var producer = await _context.Producers.FindAsync(id);
            if (producer == null)
            {
                return View("NotFound");
            }
            return View(producer);
        }


        //Método para actualizar un productor a la BD
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProducerId,Imagen,Nombre,Biografia")] Producer producer)
        {
            if (id != producer.ProducerId)
            {
                return View("NotFound");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProducerExists(producer.ProducerId))
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
            return View(producer);
        }


        //Ir a la página de eliminar un productor
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var producer = await _context.Producers
                .FirstOrDefaultAsync(m => m.ProducerId == id);
            if (producer == null)
            {
                return View("NotFound");
            }

            return View(producer);
        }


        //Método para eliminar un productor a la BD
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producer = await _context.Producers.FindAsync(id);
            _context.Producers.Remove(producer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        //Método para ver si existe un productor
        private bool ProducerExists(int id)
        {
            return _context.Producers.Any(e => e.ProducerId == id);
        }
    }
}
