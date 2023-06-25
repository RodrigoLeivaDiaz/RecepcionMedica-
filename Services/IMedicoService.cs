using RecepcionMedica.Models;

namespace RecepcionMedica.Services;

public interface IMedicoService
{
    void Create(Medico obj);
    void Delete(Medico id);
    Medico? GetById(int id);
    Medico? Details(int? id);
    Medico? Edit(int? id);

}