using RecepcionMedica.Models;

namespace RecepcionMedica.Services;

public interface IEspecialidadService
{
    void Create(Especialidad obj);
    void Delete(Especialidad id);
    Especialidad? GetById(int id);
    Especialidad? Details(int? id);
    Especialidad? Edit(int? id);
    List<Especialidad> GetAll();

}