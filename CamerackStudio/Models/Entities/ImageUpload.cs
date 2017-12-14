using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CamerackStudio.Models.Entities
{
    public class ImageUpload
    {
        public string File { get; set; }
        public Image Image { get; set; }
    }
}
