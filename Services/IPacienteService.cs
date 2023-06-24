using RecepcionMedica.Models;

namespace RecepcionMedica.Services;

public interface IPacienteService
{
    void Create(Paciente obj);
    void Delete(Paciente id);
    Paciente? GetById(int id);
    Task<Paciente> Details(int? id);
    Paciente? Edit(int? id);

}