using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoFDI.v2.Models;

namespace ProyectoFDI.v2.Controllers
{
    public class PuntajeBloquesController : Controller
    {
        private readonly ProyectoFdiV2Context _context;

        public PuntajeBloquesController(ProyectoFdiV2Context context)
        {
            _context = context;
        }

        // GET: PuntajeBloques
        public async Task<IActionResult> Index()
        {
            var proyectoFdiV2Context = _context.PuntajeBloques.Include(p => p.IdComNavigation).Include(p => p.IdDepNavigation);
            return View(await proyectoFdiV2Context.ToListAsync());
        }

        // GET: PuntajeBloques/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PuntajeBloques == null)
            {
                return NotFound();
            }

            var puntajeBloque = await _context.PuntajeBloques
                .Include(p => p.IdComNavigation)
                .Include(p => p.IdDepNavigation)
                .FirstOrDefaultAsync(m => m.IdBloPts == id);
            if (puntajeBloque == null)
            {
                return NotFound();
            }

            return View(puntajeBloque);
        }

        // GET: PuntajeBloques/Create
        public IActionResult Create()
        {
            ViewData["IdCom"] = new SelectList(_context.Competencia, "IdCom", "IdCom");
            ViewData["IdDep"] = new SelectList(_context.Deportista, "IdDep", "IdDep");
            return View();
        }

        // POST: PuntajeBloques/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdBloPts,IdCom,IdDep,NumeroBloque,IntentosTops,IntentosZonas,Etapa")] PuntajeBloque puntajeBloque)
        {
            if (ModelState.IsValid)
            {
                _context.Add(puntajeBloque);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCom"] = new SelectList(_context.Competencia, "IdCom", "IdCom", puntajeBloque.IdCom);
            ViewData["IdDep"] = new SelectList(_context.Deportista, "IdDep", "IdDep", puntajeBloque.IdDep);
            return View(puntajeBloque);
        }

        // GET: PuntajeBloques/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PuntajeBloques == null)
            {
                return NotFound();
            }

            var puntajeBloque = await _context.PuntajeBloques.FindAsync(id);
            if (puntajeBloque == null)
            {
                return NotFound();
            }
            ViewData["IdCom"] = new SelectList(_context.Competencia, "IdCom", "IdCom", puntajeBloque.IdCom);
            ViewData["IdDep"] = new SelectList(_context.Deportista, "IdDep", "IdDep", puntajeBloque.IdDep);
            return View(puntajeBloque);
        }

        // POST: PuntajeBloques/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdBloPts,IdCom,IdDep,NumeroBloque,IntentosTops,IntentosZonas,Etapa")] PuntajeBloque puntajeBloque)
        {
            if (id != puntajeBloque.IdBloPts)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(puntajeBloque);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PuntajeBloqueExists(puntajeBloque.IdBloPts))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCom"] = new SelectList(_context.Competencia, "IdCom", "IdCom", puntajeBloque.IdCom);
            ViewData["IdDep"] = new SelectList(_context.Deportista, "IdDep", "IdDep", puntajeBloque.IdDep);
            return View(puntajeBloque);
        }

        // GET: PuntajeBloques/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PuntajeBloques == null)
            {
                return NotFound();
            }

            var puntajeBloque = await _context.PuntajeBloques
                .Include(p => p.IdComNavigation)
                .Include(p => p.IdDepNavigation)
                .FirstOrDefaultAsync(m => m.IdBloPts == id);
            if (puntajeBloque == null)
            {
                return NotFound();
            }

            return View(puntajeBloque);
        }

        // POST: PuntajeBloques/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PuntajeBloques == null)
            {
                return Problem("Entity set 'ProyectoFdiV2Context.PuntajeBloques'  is null.");
            }
            var puntajeBloque = await _context.PuntajeBloques.FindAsync(id);
            if (puntajeBloque != null)
            {
                _context.PuntajeBloques.Remove(puntajeBloque);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PuntajeBloqueExists(int id)
        {
          return _context.PuntajeBloques.Any(e => e.IdBloPts == id);
        }
    }
}
