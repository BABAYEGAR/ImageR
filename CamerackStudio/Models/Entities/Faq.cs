using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CamerackStudio.Models.Entities
{
    public class Faq : Transport
    {
        public long FaqId { get; set; }
        public string Text { get; set; }
    }
}
