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
    public class RegistrosController : Controller
    {
        private readonly ConferenciasDbContext _context;

        public RegistrosController(ConferenciasDbContext context)
        {
            _context = context;
        }

        // GET: Registros
        public async Task<IActionResult> Index(string buscar)
        {
            ViewData["Buscar"] = buscar;

            var query = _context.Registros
                .Include(r => r.Asistente)
                .Include(r => r.Conferencia)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                buscar = buscar.Trim();

                query = query.Where(r =>
                    (r.Asistente != null &&
                        (
                            (r.Asistente.Nombre ?? "").Contains(buscar) ||
                            (r.Asistente.Apellido ?? "").Contains(buscar) ||
                            (r.Asistente.Email ?? "").Contains(buscar) ||
                            (r.Asistente.Telefono ?? "").Contains(buscar)
                        )
                    )
                    ||
                    (r.Conferencia != null &&
                        (
                            (r.Conferencia.Nombre ?? "").Contains(buscar) ||
                            (r.Conferencia.Ubicacion ?? "").Contains(buscar) ||
                            (r.Conferencia.Descripcion ?? "").Contains(buscar)
                        )
                    )
                );
            }
            return View(await query.OrderByDescending(r => r.FechaRegistro).ToListAsync());
        }

        // GET: Registros/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registro = await _context.Registros
                .Include(r => r.Asistente)
                .Include(r => r.Conferencia)
                .FirstOrDefaultAsync(m => m.RegistroId == id);
            if (registro == null)
            {
                return NotFound();
            }

            return View(registro);
        }

        // GET: Registros/Create
        public IActionResult Create(int? conferenciaId)
        {
          
            ViewData["AsistenteId"] = new SelectList(_context.Asistentes, "AsistenteId", "Email");

            if (conferenciaId.HasValue)
            {
               
                var conf = _context.Conferencias.FirstOrDefault(c => c.ConferenciaId == conferenciaId.Value);
                if (conf == null) return NotFound();

                ViewBag.ConferenciaFija = true;
                ViewBag.ConferenciaIdFija = conf.ConferenciaId;
                ViewBag.ConferenciaNombreFija = conf.Nombre;
            }
            else
            {
                
                ViewBag.ConferenciaFija = false;
                ViewData["ConferenciaId"] = new SelectList(_context.Conferencias, "ConferenciaId", "Nombre");
            }

            return View();
        }

        // POST: Registros/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RegistroId,ConferenciaId,AsistenteId")] Registro registro, int? conferenciaId)
        {
            if (conferenciaId.HasValue)
                registro.ConferenciaId = conferenciaId.Value;

            bool yaExiste = await _context.Registros.AnyAsync(r =>
                r.ConferenciaId == registro.ConferenciaId &&
                r.AsistenteId == registro.AsistenteId
            );

            if (yaExiste)
                ModelState.AddModelError("", "Este asistente ya está registrado en esta conferencia.");

            if (ModelState.IsValid)
            {
                registro.FechaRegistro = DateTime.Now;
                _context.Add(registro);
                await _context.SaveChangesAsync();

              
                if (conferenciaId.HasValue)
                    return RedirectToAction("Details", "Conferencias", new { id = conferenciaId.Value });

                return RedirectToAction(nameof(Index));
            }

            ViewData["AsistenteId"] = new SelectList(_context.Asistentes, "AsistenteId", "Email", registro.AsistenteId);

            if (conferenciaId.HasValue)
            {
                var conf = await _context.Conferencias.FirstOrDefaultAsync(c => c.ConferenciaId == conferenciaId.Value);
                ViewBag.ConferenciaFija = true;
                ViewBag.ConferenciaIdFija = conferenciaId.Value;
                ViewBag.ConferenciaNombreFija = conf?.Nombre ?? "";
            }
            else
            {
                ViewBag.ConferenciaFija = false;
                ViewData["ConferenciaId"] = new SelectList(_context.Conferencias, "ConferenciaId", "Nombre", registro.ConferenciaId);
            }

            return View(registro);
        }

        // GET: Registros/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registro = await _context.Registros.FindAsync(id);
            if (registro == null)
            {
                return NotFound();
            }
            ViewData["AsistenteId"] = new SelectList(_context.Asistentes, "AsistenteId", "Email", registro.AsistenteId);
            ViewData["ConferenciaId"] = new SelectList(_context.Conferencias, "ConferenciaId", "Nombre", registro.ConferenciaId);
            return View(registro);
        }

        // POST: Registros/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RegistroId,ConferenciaId,AsistenteId")] Registro registro)
        {
            if (id != registro.RegistroId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var original = await _context.Registros.AsNoTracking()
                        .FirstOrDefaultAsync(r => r.RegistroId == id);

                    if (original != null)
                        registro.FechaRegistro = original.FechaRegistro;

                    _context.Update(registro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistroExists(registro.RegistroId))
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

            ViewData["AsistenteId"] = new SelectList(_context.Asistentes, "AsistenteId", "Email", registro.AsistenteId);
            ViewData["ConferenciaId"] = new SelectList(_context.Conferencias, "ConferenciaId", "Nombre", registro.ConferenciaId);
            return View(registro);
        }
        // POST: Registros/Quitar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Quitar(int registroId, int conferenciaId)
        {
            var registro = await _context.Registros.FindAsync(registroId);

            if (registro != null)
            {
                _context.Registros.Remove(registro);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Details", "Conferencias", new { id = conferenciaId });
        }

        // GET: Registros/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registro = await _context.Registros
                .Include(r => r.Asistente)
                .Include(r => r.Conferencia)
                .FirstOrDefaultAsync(m => m.RegistroId == id);
            if (registro == null)
            {
                return NotFound();
            }

            return View(registro);
        }

        // POST: Registros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var registro = await _context.Registros.FindAsync(id);
            if (registro != null)
            {
                _context.Registros.Remove(registro);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegistroExists(int id)
        {
            return _context.Registros.Any(e => e.RegistroId == id);
        }
    }
}
