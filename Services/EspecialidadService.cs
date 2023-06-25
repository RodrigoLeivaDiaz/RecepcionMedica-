using RecepcionMedica.Data;
using RecepcionMedica.Models;
using Microsoft.EntityFrameworkCore;

namespace RecepcionMedica.Services;

public class EspecialidadService : IEspecialidadService
{
    private readonly MvcMedicoContext _MvcMedicoContext;

    public EspecialidadService(MvcMedicoContext context)
    {
        _MvcMedicoContext = context;
    }

    public void Create(Especialidad especialidad)
    {
        _MvcMedicoContext.Add(especialidad);
        _MvcMedicoContext.SaveChanges();
    }

    public void Delete(Especialidad especialidad)
    {
        if (especialidad != null){
            _MvcMedicoContext.Remove(especialidad);
            _MvcMedicoContext.SaveChanges();
        }
    }

    public Especialidad? Details(int? id)
    {
        var especialidad = _MvcMedicoContext.Especialidad
                .FirstOrDefault(m => m.Id == id);

                return especialidad;
    }

    public Especialidad? Edit(int? id)
    {
        var especialidad =_MvcMedicoContext.Especialidad.Find(id);

        return especialidad;
    }

    public List<Especialidad> GetAll()
    {
        var query = GetQuery();
        return query.ToList();
    }

    public Especialidad? GetById(int id)
    {

        var especialidad = _MvcMedicoContext.Especialidad.Find(id);

        return especialidad;
    }

    private IQueryable<Especialidad> GetQuery()
    {
        return from Especialidad in _MvcMedicoContext.Especialidad select Especialidad;
    }
}