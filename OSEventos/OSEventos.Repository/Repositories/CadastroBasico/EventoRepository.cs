using OSEventos.Repository.Repository;
using OSEventos.Domain.Entities.CadastroBasico;
using OSEventos.Repository.Context;
using OSEventos.Repository.Interafaces.CadastroBasico;

namespace OSEventos.Repository.Repositoies.CadastroBasico
{
    public class EventoRepository: BaseRepository<Evento>, IEventoRepository
    {
        public EventoRepository(OSEventosContext context) : base(context)
        {  }
    }
}
