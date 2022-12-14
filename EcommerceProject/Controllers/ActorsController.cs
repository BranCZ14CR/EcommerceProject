using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcommerceProject.Data;
using EcommerceProject.Models;
using Microsoft.AspNetCore.Authorization;
using EcommerceProject.Data.Static;

namespace EcommerceProject.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class ActorsController : Controller
    {
        //Variables
        private readonly EcommerceDbContext _context;

        //Constructor
        public ActorsController(EcommerceDbContext context)
        {
            _context = context;
        }


        //Obtener todos los Actores en una lista
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Actors.ToListAsync());
        }


        //Obtener un actor en especifico
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var actor = await _context.Actors.FirstOrDefaultAsync(m => m.ActorId == id);

            if (actor == null)
            {
                return View("NotFound");
            }

            return View(actor);
        }


        //Ir a la pagina de crear un actor
        public IActionResult Create()
        {
            return View();
        }


        //Método para agregar un actor a la BD
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ActorId,Imagen,Nombre,Biografia")] Actor actor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(actor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(actor);
        }


        //Ir a la página de editar un actor
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var actor = await _context.Actors.FindAsync(id);
            if (actor == null)
            {
                return View("NotFound");
            }
            return View(actor);
        }


        //Método para actualizar un actor a la BD
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ActorId,Imagen,Nombre,Biografia")] Actor actor)
        {
            if (id != actor.ActorId)
            {
                return View("NotFound");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActorExists(actor.ActorId))
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
            return View(actor);
        }


        //Ir a la página de eliminar un actor
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var actor = await _context.Actors.FirstOrDefaultAsync(m => m.ActorId == id);

            if (actor == null)
            {
                return View("NotFound");
            }

            return View(actor);
        }


        //Método para eliminar un actor a la BD
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actor = await _context.Actors.FindAsync(id);
            _context.Actors.Remove(actor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        //Método para ver si existe un actor
        private bool ActorExists(int id)
        {
            return _context.Actors.Any(e => e.ActorId == id);
        }
    }
}
