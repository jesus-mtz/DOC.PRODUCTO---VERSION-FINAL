using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace prueba.Models
{
    public class Insertar
    {
        public int turno { get; set; }
        public string nParte { get; set; } 
        public string Otrabajo { get; set; }
        public List<MapFolder> Folders { get; set; }

        
    }

}