using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CamerackStudio.Models.Entities
{
    public class AboutUs : Transport
    {
        public long AboutUsId { get; set; }
        public string Text { get; set; }
        public string File { get; set; }
    }
}
