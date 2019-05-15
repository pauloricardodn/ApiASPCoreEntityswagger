using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace OSEventos.DataVO.ValueObjects.CadastroBasico
{
    [DataContract]
    public class EventoVo
    {
        [DataMember(Order = 1, Name = "id", IsRequired = false)]
        public int id { get; set; }
        [DataMember(Order = 2, Name = "id_pessoa_empresa", IsRequired = true)]
        public int id_pessoa_empresa { get; set; }
        [DataMember(Order = 3, Name = "descricao", IsRequired = true)]
        public string descricao { get; set; }
        [DataMember(Order = 4, Name = "data_inclusao", IsRequired = true)]
        public DateTime data_inclusao { get; set; }
        [DataMember(Order = 5, Name = "usuario", IsRequired = true)]
        public string usuario { get; set; }
        [DataMember(Order = 6, Name = "excluido", IsRequired = true)]
        public bool excluido { get; set; }
    }
}

