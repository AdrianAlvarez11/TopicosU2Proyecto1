using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuiaEpisodios.Models
{
    public class Temporada
    {
        public string Nombre { get; set; } = "";
        public int NumeroTemporada { get; set; }
        public string Sinopsis { get; set; } = "";
        public string AñoEstreno { get; set; } = "";

        public string Imagen { get; set; } = "";

        public ObservableCollection<Episodio> Episodios { get; set; } = new();
        //public IEnumerable<ObservableCollection<Episodio>> EpisodiosOrdenados => (IEnumerable<ObservableCollection<Episodio>>)Episodios.OrderBy(ep => ep.NumeroEpisodio).ToList();
        public int TotalEpisodios=> Episodios.Count();
    }
}
