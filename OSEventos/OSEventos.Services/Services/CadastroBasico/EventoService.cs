using OSEventos.DataVO.Converters.CadastroBasico;
using OSEventos.DataVO.ValueObjects.CadastroBasico;
using OSEventos.Repository.Interafaces.CadastroBasico;
using OSEventos.Services.Interfaces.CadastroBasico;
using System.Collections.Generic;

namespace OSEventos.Services.Services.CadastroBasico
{
    public class EventoService : IEventoService
    {
        /// <summary>
        /// Declaração das interfaces
        /// </summary>
        private readonly IEventoRepository _EventoRepository;
        private readonly EventoConverter _EventoConverter;
        /// <summary>
        /// Método Contrutor
        /// </summary>
        /// <param name="EventoRepository"></param>
        /// <param name="EventoConverter"></param>
        public EventoService(IEventoRepository EventoRepository,
                             EventoConverter EventoConverter)
        {
            _EventoRepository = EventoRepository;
            _EventoConverter = EventoConverter;
        }


        /// <summary>
        /// Método Adiciona um evento
        /// </summary>
        /// <param name="EventoVo"></param>
        /// <returns></returns>
        public EventoVo Add(EventoVo EventoVo)
        {
            var EventoEntity = _EventoConverter.Parse(EventoVo);
            var Evento = _EventoRepository.Add(EventoEntity);

            return _EventoConverter.Parse(Evento);
        }

        /// <summary>
        /// Métoro altera Evento
        /// </summary>
        /// <param name="EventoVo"></param>
        /// <returns></returns>
        public EventoVo Update(EventoVo EventoVo)
        {
            var EventoEntity = _EventoConverter.Parse(EventoVo);
            _EventoRepository.Update(EventoEntity);

            return EventoVo;
        }

        /// <summary>
        /// Método Remove Evento
        /// </summary>
        /// <param name="EventoVo"></param>
        /// <returns></returns>
        public EventoVo Remove(EventoVo EventoVo)
        {
            var EventoEntity = _EventoConverter.Parse(EventoVo);
            _EventoRepository.Remove(EventoEntity);

            return _EventoConverter.Parse(EventoEntity);
        }


        /// <summary>
        /// Restorna Evento por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EventoVo GetById(int id)
        {
            var EventoEntity = _EventoRepository.GetById(id);
            return _EventoConverter.Parse(EventoEntity);
        }

        /// <summary>
        /// Retorna todos eventos
        /// </summary>
        /// <returns></returns>
        public List<EventoVo> GetAll()
        {
            var EventoContaEntity = _EventoRepository.GetAll();
            return _EventoConverter.ParseList(EventoContaEntity);
        }
    }
}
