using RecepcionMedica.Data;
using RecepcionMedica.Models;
using Microsoft.EntityFrameworkCore;

namespace RecepcionMedica.Services;

public class MedicoService : IMedicoService
{
    private readonly MvcMedicoContext _MvcMedicoContext;

    public MedicoService(MvcMedicoContext context)
    {
        _MvcMedicoContext = context;
    }

    public void Create(Medico medico)
    {
        _MvcMedicoContext.Add(medico);
        _MvcMedicoContext.SaveChanges();
    }

    public void Delete(Medico medico)
    {
        if (medico != null){
            _MvcMedicoContext.Remove(medico);
            _MvcMedicoContext.SaveChanges();
        }
    }

    public Medico Details(int? id)
    {
        var medico = _MvcMedicoContext.Medico
                .Include(m => m.Especialidad)
                .FirstOrDefault(m => m.Id == id);
            
        if (medico == null)
    {
        return null;
    }

        return medico;
    }

    public Medico? Edit(int? id)
    {
        var medico =  _MvcMedicoContext.Medico.Find(id);

        return medico;
    }

    public Medico? GetById(int id)
    {

        var medico = _MvcMedicoContext.Medico.Find(id);

        return medico;
    }

    private IQueryable<Medico> GetQuery()
    {
        return from Medico in _MvcMedicoContext.Medico select Medico;
    }
}