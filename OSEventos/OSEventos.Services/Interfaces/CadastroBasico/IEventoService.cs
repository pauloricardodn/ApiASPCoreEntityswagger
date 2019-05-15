using OSEventos.DataVO.ValueObjects.CadastroBasico;
using System;
using System.Collections.Generic;
using System.Text;

namespace OSEventos.Services.Interfaces.CadastroBasico
{
    public interface IEventoService
    {
        /// <summary>
        /// Método Adiciona um evento
        /// </summary>
        /// <param name="EventoVo"></param>
        /// <returns></returns>
        EventoVo Add(EventoVo EventoVo);

        /// <summary>
        /// Métoro altera Evento
        /// </summary>
        /// <param name="EventoVo"></param>
        /// <returns></returns>
        EventoVo Update(EventoVo EventoVo);

        /// <summary>
        /// Método Remove Evento
        /// </summary>
        /// <param name="EventoVo"></param>
        /// <returns></returns>
        EventoVo Remove(EventoVo EventoVo);

        /// <summary>
        /// Restorna Evento por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        EventoVo GetById(int id);

        /// <summary>
        /// Retorna todos eventos
        /// </summary>
        /// <returns></returns>
        List<EventoVo> GetAll();
    }
}
