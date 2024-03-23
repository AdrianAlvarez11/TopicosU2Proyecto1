using CommunityToolkit.Mvvm.Input;
using GuiaEpisodios.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.IO;
using System.Text.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuiaEpisodios.ViewModels
{
    public enum Vistas  { AgregarTemporada, EditarTemporada, EliminarTemporada, AgregarEpisodio,EditarEpisodio,EliminarEpisodio, Inicio, ListaEpisodios}
    public class SerieViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Temporada> Temporadas { get; set; } = new();

        public ICommand AgregarTemporadaCommand { get; set; }
        public ICommand EditarTemporadaCommand { get; set; }
        public ICommand EliminarTemporadaCommand { get; set; }
        public ICommand AgregarEpisodioCommand { get; set; }
        public ICommand EditarEpisodioCommand { get; set; }
        public ICommand EliminarEpisodioCommand { get; set; }
        public ICommand CambiarVistaCommand { get; set; }

        public SerieViewModel()
        {
            AgregarTemporadaCommand = new RelayCommand(AgregarTemporada);
            EditarTemporadaCommand = new RelayCommand(EditarTemporada);
            EliminarTemporadaCommand = new RelayCommand(EliminarTemporada);
            AgregarEpisodioCommand = new RelayCommand(AgregarEpisodio);
            EditarEpisodioCommand = new RelayCommand(EditarEpisodio);
            EliminarEpisodioCommand = new RelayCommand(EliminarEpisodio);
            CambiarVistaCommand = new RelayCommand<Vistas>(CambiarVista);
            Deserializar();
        }


        public Vistas MostrarVista { get; set; } = Vistas.Inicio;

        public string Error { get; set; } = "";
        public Episodio EpisodioAct { get; set; }
        public Temporada TemporadaAct { get; set; }
        public void OrdenarTemporadas()
        {
            var tempOrdenadas = Temporadas.OrderBy(temporada => temporada.NumeroTemporada).ToList();
            Temporadas.Clear();
            foreach (var item in tempOrdenadas)
            {
                Temporadas.Add(item);
            }

        }
        public void OrdenarEpisodios()
        {
            var epOrdenados = TemporadaAct.Episodios.OrderBy(ep => ep.NumeroEpisodio).ToList();
            TemporadaAct.Episodios.Clear();
            foreach (var item in epOrdenados)
            {
                TemporadaAct.Episodios.Add(item);
            }
        }

        int posAnterior {  get; set; }

        private void CambiarVista(Vistas vistas)
        {
            if(vistas==Vistas.AgregarTemporada)
            {
                TemporadaAct = new();
                Actualizar(nameof(TemporadaAct));
              
            }
            if(vistas==Vistas.AgregarEpisodio)
            {
                EpisodioAct = new();
                Actualizar(nameof(EpisodioAct));

            }
            if (vistas == Vistas.EditarTemporada && TemporadaAct!=null)
            {
                var temporadaClon = new Temporada
                {
                    Nombre= TemporadaAct.Nombre,
                    NumeroTemporada= TemporadaAct.NumeroTemporada,
                    Sinopsis= TemporadaAct.Sinopsis,
                    AñoEstreno=TemporadaAct.AñoEstreno,
                    Imagen= TemporadaAct.Imagen,
                    Episodios= TemporadaAct.Episodios,
                };
                posAnterior = Temporadas.IndexOf(TemporadaAct);
                TemporadaAct=temporadaClon;
            }
            if (vistas == Vistas.EditarEpisodio && TemporadaAct != null && EpisodioAct!=null)
            {
                
                var epClon = new Episodio
                {
                    Titulo = EpisodioAct.Titulo,
                    NumeroEpisodio = EpisodioAct.NumeroEpisodio,
                    Descripcion = EpisodioAct.Descripcion,
                    Duracion = EpisodioAct.Duracion,
                    Imagen=EpisodioAct.Imagen
                    
                };
                
                posAnterior = TemporadaAct.Episodios.IndexOf(EpisodioAct);
                EpisodioAct = epClon;

            }

            Error = "";
            MostrarVista = vistas;
            Actualizar(nameof(EpisodioAct));
            Actualizar(nameof(TemporadaAct));
            Actualizar(nameof(MostrarVista));
            Actualizar(nameof(Error));
            
        }

        private void EliminarEpisodio()
        {
            if(EpisodioAct != null)
            {
                TemporadaAct.Episodios.Remove(EpisodioAct);
                OrdenarEpisodios();
                Serializar();
                CambiarVista(Vistas.ListaEpisodios);
            }
        }

        private void EditarEpisodio()
        {
            if (EpisodioAct != null)
            {
                if (string.IsNullOrWhiteSpace(EpisodioAct.Titulo))
                {
                    Error += "Ingrese el título del capítulo\n";
                }
                if (string.IsNullOrWhiteSpace(EpisodioAct.Descripcion))
                {
                    Error += "Ingrese la descripción del capítulo\n";
                }
                if (string.IsNullOrWhiteSpace(EpisodioAct.Duracion)) 
                {
                    Error += "Ingrese la duración del capítulo\n";
                }
                if (EpisodioAct.NumeroEpisodio < 1)
                {
                    Error += "Ingrese un numero de episodio válido\n";
                }
                if (string.IsNullOrWhiteSpace(EpisodioAct.Imagen) || !EpisodioAct.Imagen.StartsWith("http") || !EpisodioAct.Imagen.EndsWith(".jpg"))
                {
                    Error += "Escriba la direccion de una imagen en formato JPG\n";
                }
               

                if (Error == "")
                {
                    TemporadaAct.Episodios[posAnterior] = EpisodioAct;
                    OrdenarEpisodios();
                    Serializar();
                    CambiarVista(Vistas.ListaEpisodios);
                }

            }
        }

        private void AgregarEpisodio()
        {
            if (EpisodioAct!=null)
            {
                if(string.IsNullOrWhiteSpace(EpisodioAct.Titulo))
                {
                    Error += "Ingrese el título del capítulo\n";
                }
                if (string.IsNullOrWhiteSpace(EpisodioAct.Descripcion))
                {
                    Error += "Ingrese la descripción del capítulo\n";
                }
                if (string.IsNullOrWhiteSpace(EpisodioAct.Duracion))
                {
                    Error += "Ingrese la duración del capítulo\n";
                }
                if (EpisodioAct.NumeroEpisodio < 1)
                {
                    Error += "Ingrese un numero de episodio válido\n";
                }
                if (string.IsNullOrWhiteSpace(EpisodioAct.Imagen) || !EpisodioAct.Imagen.StartsWith("http") || !EpisodioAct.Imagen.EndsWith(".jpg"))
                {
                    Error += "Escriba la direccion de una imagen en formato JPG\n";
                }
               

                var episodioDupe = TemporadaAct.Episodios.Any(x => x.Titulo == EpisodioAct.Titulo);
                var numepisodio = TemporadaAct.Episodios.Any(x => x.NumeroEpisodio == EpisodioAct.NumeroEpisodio);
                if (episodioDupe || numepisodio)
                {
                    Error += "Este episodio ya existe \n";
                }
                Actualizar(nameof(Error));

                if (Error=="")
                {
                    TemporadaAct.Episodios.Add(EpisodioAct);
                    OrdenarEpisodios();
                    Serializar();
                    CambiarVista(Vistas.ListaEpisodios);
                }
                
            }
           
        }

        private void EliminarTemporada()
        {
            if(TemporadaAct!=null)
            {
                Temporadas.Remove(TemporadaAct);
                OrdenarTemporadas();
                Serializar();
                CambiarVista(Vistas.Inicio);
            }
        }

        private void EditarTemporada()
        {
            if (TemporadaAct != null)
            {
                Error = "";

                if (string.IsNullOrWhiteSpace(TemporadaAct.Nombre))
                {
                    Error += "Ingrese el nombre o numero de la temporada\n";


                }
                if (string.IsNullOrWhiteSpace(TemporadaAct.Sinopsis))
                {
                    Error += "Ingrese la sinopsis de la temporada\n";

                }
                if (string.IsNullOrWhiteSpace(TemporadaAct.AñoEstreno))
                {
                    Error += "Ingrese el año de estreno de la temporada\n";

                }
                if (string.IsNullOrWhiteSpace(TemporadaAct.Imagen) || !TemporadaAct.Imagen.StartsWith("http") || !TemporadaAct.Imagen.EndsWith(".jpg"))
                {
                    Error += "Escriba la direccion de una imagen en formato JPG\n";

                }
                if (TemporadaAct.NumeroTemporada < 1)
                {
                    Error += "Introduzca un numero de temporada valido\n";
                }
              
                Actualizar(nameof(Error));
                if (Error == "")
                {
                    Temporadas[posAnterior]=TemporadaAct;
                    OrdenarTemporadas();
                    Serializar();
                    CambiarVista(Vistas.Inicio);

                }

            }
        }

        private void AgregarTemporada()
        {
            if(TemporadaAct != null)
            {
                Error = "";

                if (string.IsNullOrWhiteSpace(TemporadaAct.Nombre))
                {
                    Error += "Ingrese el nombre o numero de la temporada\n";

                   
                }
                if (string.IsNullOrWhiteSpace(TemporadaAct.Sinopsis))
                {
                    Error += "Ingrese la sinopsis de la temporada\n";
                    
                }
                if (string.IsNullOrWhiteSpace(TemporadaAct.AñoEstreno))
                {
                    Error += "Ingrese el año de estreno de la temporada\n";
                   
                }
                if(string.IsNullOrWhiteSpace(TemporadaAct.Imagen) || !TemporadaAct.Imagen.StartsWith("http") || !TemporadaAct.Imagen.EndsWith(".jpg"))
                {
                    Error += "Escriba la direccion de una imagen en formato JPG\n";
                 
                }
                var temporadaDupe = Temporadas.Any(x => x.Nombre == TemporadaAct.Nombre);
                var numtemporada = Temporadas.Any(x => x.NumeroTemporada == TemporadaAct.NumeroTemporada);
                if (temporadaDupe || numtemporada)
                {
                    Error += "Esta temporada ya existe \n";
                }
                Actualizar(nameof(Error));
                if (Error == "")
                {
                    Temporadas.Add(TemporadaAct);
                    OrdenarTemporadas();
                    Serializar();
                    CambiarVista(Vistas.Inicio);
                }

                

            }
        }

        private void Serializar()
        {
            File.WriteAllText("temporadas.json", JsonSerializer.Serialize(Temporadas));
        }

        private void Deserializar()
        {
            if (File.Exists("temporadas.json"))
            {
                var datos = JsonSerializer.Deserialize<ObservableCollection<Temporada>>(File.ReadAllText("temporadas.json"));
                if (datos != null)
                {
                    foreach (var dato in datos)
                    {
                        Temporadas.Add(dato);
                    }
                }
            }
        }


        private void Actualizar(string? nombre)
        {
            PropertyChanged?.Invoke(this, new(nombre));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        
    }
}
