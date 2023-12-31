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

namespace RecepcionMedica.Controllers
{
     [Authorize(Roles = "Administrador")]
    public class EspecialidadController : Controller
    {
        private MvcMedicoContext _context;
        private IEspecialidadService _especialidadService;

        public EspecialidadController(IEspecialidadService especialidadService,MvcMedicoContext context)
        {
            _especialidadService = especialidadService;
            _context = context;            
        }

        // GET: Especialidad
        public IActionResult Index()
        {
            var list = _especialidadService.GetAll();
            return View(list);
        }

        // GET: Especialidad/Details/5

        public async Task<IActionResult> Details(int? id)
        {
             if (id == null || _context.Especialidad == null)
            {
                return NotFound();
            }

            var especialidad = _especialidadService.Details(id);
            if (especialidad == null)
            {
                return NotFound();
            }
            var query = from Medico in _context.Medico where Medico.EspecialidadId == id select Medico;

            var viewModel = new EspecialidadesViewModel();

            viewModel.Especialidad = especialidad.NombreEspecialidad;          
            viewModel.Medicos = await query.ToListAsync();
            viewModel.Id = especialidad.Id;

            return View(viewModel);
        }

        // GET: Especialidad/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Especialidad/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NombreEspecialidad")] Especialidad especialidad)
        {
            if (ModelState.IsValid)
            {
                _especialidadService.Create(especialidad);
                return RedirectToAction(nameof(Index));
            }
            return View(especialidad);
        }

        // GET: Especialidad/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Especialidad == null)
            {
                return NotFound();
            }

            var especialidad = _especialidadService.Edit(id);
            if (especialidad == null)
            {
                return NotFound();
            }
            return View(especialidad);
        }

        // POST: Especialidad/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NombreEspecialidad")] Especialidad especialidad)
        {
            if (id != especialidad.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(especialidad);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EspecialidadExists(especialidad.Id))
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
            return View(especialidad);
        }

        // GET: Especialidad/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Especialidad == null)
            {
                return NotFound();
            }

            var especialidad = await _context.Especialidad
                .FirstOrDefaultAsync(m => m.Id == id);
            if (especialidad == null)
            {
                return NotFound();
            }

            return View(especialidad);
        }

        // POST: Especialidad/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Especialidad == null)
            {
                return Problem("Entity set 'MvcMedicoContext.Especialidad'  is null.");
            }
            var especialidad = _especialidadService.GetById(id);
            if (especialidad != null)
            {
                _especialidadService.Delete(especialidad);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EspecialidadExists(int id)
        {
          return (_context.Especialidad?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
