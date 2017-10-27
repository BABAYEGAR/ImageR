using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Image.Models.Entities
{
    public class Invoice: Transport
    {
        public long InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public long? Amount { get; set; }
    }
}
