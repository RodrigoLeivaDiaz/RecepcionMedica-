using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecepcionMedica.Data;
using RecepcionMedica.Models;
using recepcionMedica.ViewModels;
using RecepcionMedica.Services;
using Microsoft.AspNetCore.Authorization;

namespace recepcionMedica.Controllers
{
    [Authorize(Roles = "Recepcionista, Administrador")]
    public class PacienteController : Controller
    {
        private readonly MvcMedicoContext _context;
        private IPacienteService _pacienteService;

        public PacienteController(IPacienteService pacienteService,MvcMedicoContext context)
        {
            _pacienteService = pacienteService;
            _context = context;
        }

        // GET: Paciente
        public async Task<IActionResult> Index(string NameFilter)
        {
            try
            {
            var query = from Paciente in _context.Paciente.Include(p => p.Medico) select Paciente;

            if (!string.IsNullOrEmpty(NameFilter)) {
                query = query.Where(x => x.NombreCompleto.ToLower().Contains(NameFilter.ToLower()) ||
                x.Medico.NombreCompleto.ToLower().Contains(NameFilter.ToLower()) ||
                x.Edad.ToString() == NameFilter ||
                x.ObraSocial.ToLower().Contains(NameFilter.ToLower()));
                //x.Sexo.ToString() == NameFilter);
            }

            var model =new PacienteViewModel();

            model.Pacientes = await query.ToListAsync();

            return View(model);

            }
              catch(Exception ex) {

              return View("Error");
              }
          {
        }
        }

        // GET: Paciente/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || _context.Paciente == null)
            {
                return NotFound();
            }

            var paciente = _pacienteService.Details(id);
            if (paciente == null)
            {
                return NotFound();
            }

            return View(paciente);
        }

        // GET: Paciente/Create
        public IActionResult Create()
        {
            ViewData["Medico"] = new SelectList(_context.Medico, "Id", "NombreCompleto");
            return View();
        }

        // POST: Paciente/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NombreCompleto,Sexo,ObraSocial,Edad,telefono,MedicoId")] Paciente paciente)
        {
            if (ModelState.IsValid)
            {
                _pacienteService.Create(paciente);
                
                return RedirectToAction(nameof(Index));
            }
            ViewData["MedicoId"] = new SelectList(_context.Set<Medico>(), "Id", "Id", paciente.MedicoId);
            return View(paciente);
        }

        // GET: Paciente/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || _context.Paciente == null)
            {
                return NotFound();
            }

            var paciente = _pacienteService.Edit(id);
            if (paciente == null)
            {
                return NotFound();
            }
            ViewData["Medico"] = new SelectList(_context.Set<Medico>(), "Id", "NombreCompleto", paciente.MedicoTratante);
            return View(paciente);
        }

        // POST: Paciente/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NombreCompleto,Sexo,ObraSocial,Edad,telefono,MedicoId")] Paciente paciente)
        {
            if (id != paciente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paciente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PacienteExists(paciente.Id))
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
            ViewData["MedicoId"] = new SelectList(_context.Set<Medico>(), "Id", "Id", paciente.MedicoId);
            return View(paciente);
        }

        // GET: Paciente/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Paciente == null)
            {
                return NotFound();
            }

            var paciente = await _context.Paciente
                .Include(p => p.Medico)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paciente == null)
            {
                return NotFound();
            }

            return View(paciente);
        }

        // POST: Paciente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
         
            if (_context.Paciente == null)
            {
                return Problem("Entity set 'MvcMedicoContext.Paciente'  is null.");
            }
            
            var paciente = _pacienteService.GetById(id);

            if (paciente != null)
            {
                _pacienteService.Delete(paciente);
            }
            
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool PacienteExists(int id)
        {
          return (_context.Paciente?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
