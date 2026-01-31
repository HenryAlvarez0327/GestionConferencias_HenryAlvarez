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
    public class AsistentesController : Controller
    {
        private readonly ConferenciasDbContext _context;

        public AsistentesController(ConferenciasDbContext context)
        {
            _context = context;
        }

        // GET: Asistentes
        public async Task<IActionResult> Index(string buscar)
        {
            var query = _context.Asistentes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                buscar = buscar.Trim();

                query = query.Where(a =>
                    a.Nombre.Contains(buscar) ||
                    a.Apellido.Contains(buscar) ||
                    a.Email.Contains(buscar) ||
                    a.Telefono.Contains(buscar)
                );
            }

            ViewData["Buscar"] = buscar;
            return View(await query.ToListAsync());
        }

        // GET: Asistentes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asistente = await _context.Asistentes
                .FirstOrDefaultAsync(m => m.AsistenteId == id);
            if (asistente == null)
            {
                return NotFound();
            }

            return View(asistente);
        }

        // GET: Asistentes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Asistentes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AsistenteId,Nombre,Apellido,Email,Telefono")] Asistente asistente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(asistente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(asistente);
        }

        // GET: Asistentes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asistente = await _context.Asistentes.FindAsync(id);
            if (asistente == null)
            {
                return NotFound();
            }
            return View(asistente);
        }

        // POST: Asistentes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AsistenteId,Nombre,Apellido,Email,Telefono")] Asistente asistente)
        {
            if (id != asistente.AsistenteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(asistente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AsistenteExists(asistente.AsistenteId))
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
            return View(asistente);
        }

        // GET: Asistentes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asistente = await _context.Asistentes
                .FirstOrDefaultAsync(m => m.AsistenteId == id);
            if (asistente == null)
            {
                return NotFound();
            }

            return View(asistente);
        }

        // POST: Asistentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var asistente = await _context.Asistentes.FindAsync(id);
            if (asistente != null)
            {
                _context.Asistentes.Remove(asistente);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AsistenteExists(int id)
        {
            return _context.Asistentes.Any(e => e.AsistenteId == id);
        }
    }
}
