using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuiaEpisodios.Models
{
    public class Episodio
    {
        public string Titulo { get; set; } = "";   
        public int NumeroEpisodio { get; set; }
        public string Imagen { get; set; } = "";      
        public string Descripcion { get; set; } = "";
        public string Duracion { get; set; } = "";
    }
}
