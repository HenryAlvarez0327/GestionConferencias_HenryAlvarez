using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestionConferencias.Models;

namespace GestionConferencias.Controllers
{
    public class ConferenciasController : Controller
    {
        private readonly ConferenciasDbContext _context;

        public ConferenciasController(ConferenciasDbContext context)
        {
            _context = context;
        }

        // GET: Conferencias
        public async Task<IActionResult> Index(string buscar)
        {
            ViewData["Buscar"] = buscar;

            var query = _context.Conferencias.AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                buscar = buscar.Trim();

                query = query.Where(c =>
                    (c.Nombre ?? "").Contains(buscar) ||
                    (c.Ubicacion ?? "").Contains(buscar) ||
                    (c.Descripcion ?? "").Contains(buscar)
                );
            }

            return View(await query.OrderByDescending(c => c.Fecha).ToListAsync());
        }

        // GET: Conferencias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var conferencia = await _context.Conferencias
                .Include(c => c.Registros)
                    .ThenInclude(r => r.Asistente)
                .FirstOrDefaultAsync(m => m.ConferenciaId == id);

            if (conferencia == null) return NotFound();

            return View(conferencia);
        }

        // GET: Conferencias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Conferencias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConferenciaId,Nombre,Fecha,Ubicacion,Descripcion")] Conferencia conferencia)
        {
            if (ModelState.IsValid)
            {
                _context.Add(conferencia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(conferencia);
        }

        // GET: Conferencias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conferencia = await _context.Conferencias.FindAsync(id);
            if (conferencia == null)
            {
                return NotFound();
            }
            return View(conferencia);
        }

        // POST: Conferencias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ConferenciaId,Nombre,Fecha,Ubicacion,Descripcion")] Conferencia conferencia)
        {
            if (id != conferencia.ConferenciaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(conferencia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConferenciaExists(conferencia.ConferenciaId))
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
            return View(conferencia);
        }

        // GET: Conferencias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conferencia = await _context.Conferencias
                .FirstOrDefaultAsync(m => m.ConferenciaId == id);
            if (conferencia == null)
            {
                return NotFound();
            }

            return View(conferencia);
        }

        // POST: Conferencias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var conferencia = await _context.Conferencias.FindAsync(id);
            if (conferencia != null)
            {
                _context.Conferencias.Remove(conferencia);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConferenciaExists(int id)
        {
            return _context.Conferencias.Any(e => e.ConferenciaId == id);
        }
    }
}
