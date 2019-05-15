using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OSEventos.Domain.Entities.CadastroBasico
{
    [Table("EVENTO")]
    public class Evento
    {
        [Key]
        public int ID { get; set; }
        public int ID_PESSOA_EMPRESA { get; set; }
        public string DESCRICAO { get; set; }
        public DateTime DATA_INCLUSAO { get; set; }
        public string USUARIO { get; set; }
        public bool EXCLUIDO { get; set; }
    }
}
