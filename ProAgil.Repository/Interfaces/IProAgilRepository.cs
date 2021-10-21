using System.Threading.Tasks;
using ProAgil.Domain.Model;

namespace ProAgil.Repository.Interfaces
{
    public interface IProAgilRepository
    {
         void Add<T>(T entity) where T : class;
         void Update<T>(T entity) where T : class;
         void Delete<T>(T entity) where T : class;

        Task<bool> SaveChangesAsync();

        // Eventos
        Task<Evento[]> GetAllEventoAsyncByTema(string tema, bool includePalestrantes);
        Task<Evento[]> GetAllEventoAsync(bool includePalestrantes);
        Task<Evento> GetAllEventoAsyncById(int EventoId, bool includePalestrantes);
        
        // Palestrantes
        Task<Palestrante[]> GetAllPalestranteAsyncByNome(string nome , bool includeEventos);
        Task<Palestrante[]> GetAllPalestranteAsync(bool includeEventos);
        Task<Palestrante> GetAllPalestranteAsyncById(int PalestranteId, bool includeEventos);
    }
}