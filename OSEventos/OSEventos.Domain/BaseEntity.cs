using System;
using System.Collections.Generic;
using System.Text;

namespace Os.Domain
{
    /// <summary>
    /// Contrato entre atributos e estrutura de tabela
    /// </summary>
    public abstract class BaseEntity
    {
        public long ID { get; set; }
    }
}
