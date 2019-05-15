using OSEventos.DataVO.Interfaces;
using OSEventos.DataVO.ValueObjects.CadastroBasico;
using OSEventos.Domain.Entities.CadastroBasico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OSEventos.DataVO.Converters.CadastroBasico
{
    public class EventoConverter: IParser<EventoVo, Evento>, IParser<Evento, EventoVo>
    {
        public Evento Parse(EventoVo origin)
        {
            if (origin == null) return new Evento();
            return new Evento
            {
                ID = origin.id,
                ID_PESSOA_EMPRESA = origin.id_pessoa_empresa,
                DESCRICAO = origin.descricao,
                DATA_INCLUSAO = origin.data_inclusao,
                USUARIO = origin.usuario,
                EXCLUIDO = origin.excluido
            };
        }

        public EventoVo Parse(Evento origin)
        {
            if (origin == null) return new EventoVo();
            return new EventoVo
            {
                id = origin.ID,
                id_pessoa_empresa = origin.ID_PESSOA_EMPRESA,
                descricao = origin.DESCRICAO,
                data_inclusao = origin.DATA_INCLUSAO,
                usuario = origin.USUARIO,
                excluido = origin.EXCLUIDO
            };
        }

        public List<Evento> ParseList(List<EventoVo> origin)
        {
            if (origin == null) return new List<Evento>();
            return origin.Select(item => Parse(item)).ToList();
        }

        public List<EventoVo> ParseList(List<Evento> origin)
        {
            if (origin == null) return new List<EventoVo>();
            return origin.Select(item => Parse(item)).ToList();
        }
    }
}
