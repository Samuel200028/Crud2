
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static CrudVM.MainCrud;
using static System.Net.Mime.MediaTypeNames;

namespace CrudVM
{
    public class MainCrud : ViewModelBase
    {
        private ICommand guardar, nuevo;
        public ICommand Eliminar { get; }
        public ICommand Guardar { get; }
        public ICommand SeleccionarPersona { get; }
        bool actualizar = false;

        public ObservableCollection<Persona> Personas { get; set; }

        private int id;
            private int edad;
            private string nombre;
            private string correo;

            public int Id
            {
                get { return id; }
                set
                {
                    if (id != value)
                    {
                        id = value;
                        OnPropertyChanged("Id");
                    }

                }
            }
            public int Edad
            {
                get { return edad; }
                set
                {
                    if (edad != value)
                    {
                        edad = value;
                        OnPropertyChanged("Id");
                    }

                }
            }
            public string Nombre
            {
                get { return nombre; }
                set
                {
                    if (nombre != value)
                    {
                        nombre = value;
                        OnPropertyChanged("Nombre");
                    }

                }
            }
            public string Correo
            {
                get { return correo; }
                set
                {
                    if (correo != value)
                    {
                        correo = value;
                        OnPropertyChanged("Correo");
                    }

                }
            }
        public void Save(object parameter)
        {
            if (!(!string.IsNullOrEmpty(Nombre) && !string.IsNullOrEmpty(Correo) && Id != 0 && Edad > 0 && Edad < 100))
            {
                MessageBox.Show("Datos incompletos");
            }
            else if (Personas.Any(p => p.Id == Id) && !actualizar) { MessageBox.Show("Id repetidos"); }
            else 
            {
                if (actualizar)
                {
                    MessageBoxResult result = MessageBox.Show("¿Actualizar elemento?", "Si", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        Persona persona = Personas.FirstOrDefault(p => p.Id == id);
                        if (persona != null)
                        {
                            persona.Correo = Correo;
                            persona.Nombre = Nombre;
                            persona.Edad = Edad;
                            OnPropertyChanged(nameof(Personas));
                        }
                        actualizar = false;
                    }
                    New();
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show("¿Agregar elemento?", "Si", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        Persona nuevaPersona = new Persona
                        {
                            Id = Id,
                            Edad = Edad,
                            Nombre = Nombre,
                            Correo = Correo
                        };
                        Personas.Add(nuevaPersona);
                    }
                    New();
                }
            }
            actualizar = false;
        }
        public ICommand Nuevo
        {
            get
            {
                if (nuevo == null)
                {
                    nuevo = new RelayCommand(p => this.New());
                }
                return nuevo;
            }
        }
        public void New()
        {
            Id = 0;
            Edad = 0;
            Nombre = "";
            Correo = "";
            OnPropertyChanged("");
        }

        private void Select(object parameter)
        {
            if (parameter is Persona personaSeleccionada)
            {
                Id = personaSeleccionada.Id;
                Edad = personaSeleccionada.Edad;
                Nombre = personaSeleccionada.Nombre;
                Correo = personaSeleccionada.Correo;
                OnPropertyChanged("");
                actualizar = true;
            }
        }

        private void Delete(object parameter)
        {
            if (parameter is Persona personaSeleccionada)
            {
                MessageBoxResult result = MessageBox.Show("¿Eliminar elemento?", "Si", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    Personas.Remove(personaSeleccionada);
                    New();
                }
            }
        }

        public MainCrud()
        {
            SeleccionarPersona = new RelayCommand(Select);
            //Seleccionar = new RelayCommand(Delete);
            Eliminar = new RelayCommand(Delete);
            Guardar = new RelayCommand(Save);
            Personas = new ObservableCollection<Persona>();

            Personas.Add(new Persona { Id = 1, Edad = 23, Nombre = "John Doe", Correo = "johndoe@example.com" });
            Personas.Add(new Persona { Id = 2, Edad = 23, Nombre = "Jane Smith", Correo = "janesmith@example.com" });
            Personas.Add(new Persona { Id = 3, Edad = 23, Nombre = "Bob Johnson", Correo = "bobjohnson@example.com" });
        }

    }

    public class Persona
    {
        public int Id { get; set; }
        public int Edad { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public Persona() { }
    }
    }
