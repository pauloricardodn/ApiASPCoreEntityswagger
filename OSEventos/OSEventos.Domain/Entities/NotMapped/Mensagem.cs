using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Os.Domain.Entities.NotMapped
{
    [NotMapped]
    public class Mensagem
    {
        [Key]
        public long CODE { get; set; } = 200;
        public string MESSEGE { get; set; } = "OK";
    }
}
