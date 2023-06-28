using RecepcionMedica.Data;
using RecepcionMedica.Models;
using Microsoft.EntityFrameworkCore;

namespace RecepcionMedica.Services;

public class PacienteService : IPacienteService
{
    private readonly MvcMedicoContext _MvcMedicoContext;

    public PacienteService(MvcMedicoContext context)
    {
        _MvcMedicoContext = context;
    }

    public void Create(Paciente paciente)
    {
        _MvcMedicoContext.Add(paciente);
        _MvcMedicoContext.SaveChanges();
    }

    public void Delete(Paciente obj)
    {
        
        if (obj != null){
            _MvcMedicoContext.Remove(obj);
            _MvcMedicoContext.SaveChanges();
        }
    }

    public async Task<Paciente> Details(int? id)
{
    var paciente = await _MvcMedicoContext.Paciente
            .Include(p => p.Medico)
            .FirstOrDefaultAsync(m => m.Id == id);

    if (paciente == null)
    {
        // Handle null case here
        return null;
    }

    return paciente;                
}

    public Paciente? Edit(int? id)
{
    if(id == null)
    {
        // Handle null case here
        return null;
    }

    var paciente = _MvcMedicoContext.Paciente.Find(id);

    if (paciente == null)
    {
        // Handle null case here
        return null;
    }

    return paciente;  
}

    public async Task<Paciente> GetById(int id)
    {
    var paciente = await _MvcMedicoContext.Paciente.FindAsync(id);
    
    return paciente;
    }

    Paciente? IPacienteService.GetById(int id)
    {
        var paciente = _MvcMedicoContext.Paciente.Find(id);

        return paciente;
    }

    private IQueryable<Paciente> GetQuery()
    {
        return from Paciente in _MvcMedicoContext.Paciente select Paciente;
    }
}