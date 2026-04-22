using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using static System.Console;
using static System.Math;
using System.Globalization;

namespace programaClase
{
    public class Suscripcion
    {
        private int _idSub;
        private string _tipoSub;
        private DateTime _fechaInicio;
        private DateTime _fechaFin;

        public int IdSub
        {
            get { return _idSub; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("El ID debe ser mayor que 0");
                _idSub = value;
            }
        }

        public string TipoSub
        {
            get { return _tipoSub; }
            set
            {
                if (value != "Mensual" && value != "Anual")
                    throw new ArgumentException("El tipo de suscipción debe ser 'Mensual' o 'Anual'");
                _tipoSub = value;
            }
        }

        public DateTime FechaInicio
        {
            get { return _fechaInicio; }
            set { _fechaInicio = value; }
        }

        public DateTime FechaFin
        {
            get { return _fechaFin; }
            set { _fechaFin = value; }
        }

        public Suscripcion(int id_sub, string tipo_sub, DateTime fecha_inicio, DateTime fecha_fin)
        {
            IdSub = id_sub;
            TipoSub = tipo_sub;
            FechaInicio = fecha_inicio;
            FechaFin = fecha_fin;
        }

        public void calculaFechafin()
        {
            if (TipoSub == "Mensual")
                FechaFin = FechaInicio.AddMonths(1);
            else if (TipoSub == "Anual")
                FechaFin = FechaInicio.AddYears(1);
        }

        public bool estaActiva()
        {
            return DateTime.Now <= FechaFin;
        }
    }

    public class Libro
    {
        private string _isbn;
        private string _nombreLibro;
        private string _autor;
        private string _genero;

        public string Isbn
        {
            get { return _isbn; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("El ISBN es obligatorio");
                _isbn = value;
            }
        }

        public string NombreLibro
        {
            get { return _nombreLibro; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("El nombre del libro es obligatorio");
                _nombreLibro = value;
            }
        }

        public string Autor
        {
            get { return _autor; }
            set { _autor = value; }
        }

        public string Genero
        {
            get { return _genero; }
            set { _genero = value; }
        }

        public Libro(string isbn, string nombre_libro, string autor, string genero)
        {
            Isbn = isbn;
            NombreLibro = nombre_libro;
            Autor = autor;
            Genero = genero;
        }
    }

    public class Usuario
    {
        private int _idUsuario;
        private string _nombreUsuario;
        private Suscripcion _suscripcion;
        private string _infoContacto;

        public int IdUsuario
        {
            get { return _idUsuario; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("El ID del usuario debe ser  mayor que 0");
                _idUsuario = value;
            }
        }

        public string NombreUsuario
        {
            get { return _nombreUsuario; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("El nombre es obligatorio");
                _nombreUsuario = value;
            }
        }

        public Suscripcion Suscripcion
        {
            get { return _suscripcion; }
            set { _suscripcion = value; }
        }

        public string InfoContacto
        {
            get { return _infoContacto; }
            set { _infoContacto = value; }
        }

        public Usuario(int id_usuario, string nombre_usuario, Suscripcion suscripcion, string info_contacto)
        {
            IdUsuario = id_usuario;
            NombreUsuario = nombre_usuario;
            Suscripcion = suscripcion;
            InfoContacto = info_contacto;
        }

        public bool puedeIniciarlectura()
        {
            return Suscripcion != null && Suscripcion.estaActiva();
        }
    }

    public class Lectura
    {
        private int _idLectura;
        private Usuario _usuario;
        private Libro _libro;
        private DateTime _lecFechainicio;
        private DateTime _lecFechafin;

        public int IdLectura
        {
            get { return _idLectura; }
            set { _idLectura = value; }
        }

        public Usuario Usuario
        {
            get { return _usuario; }
            set { _usuario = value; }
        }

        public Libro Libro
        {
            get { return _libro; }
            set { _libro = value; }
        }

        public DateTime LecFechainicio
        {
            get { return _lecFechainicio; }
            set { _lecFechainicio = value; }
        }

        public DateTime LecFechafin
        {
            get { return _lecFechafin; }
            set { _lecFechafin = value; }
        }

        public Lectura(int id_lectura, Usuario usuario, Libro libro, DateTime lec_fechainicio)
        {
            if (usuario == null)
                throw new ArgumentException("Para inciar la lectura es necesario ingresar que usuario leera");

            if (!usuario.puedeIniciarlectura())
                throw new ArgumentException("El usuario no puede iniciar una lectura");

            if (libro == null)
                throw new ArgumentException("Para inciar la lectura es necesario ingresar que libro se leera");

            IdLectura = id_lectura;
            Usuario = usuario;
            Libro = libro;
            LecFechainicio = lec_fechainicio;
            LecFechafin = default(DateTime);
        }

        public void iniciarLectura()
        {
            LecFechainicio = DateTime.Now;
        }

        public void finalizarLectura()
        {
            LecFechafin = DateTime.Now;
        }
    }

    class Biblioteca
    {
        static List<Usuario> listausuarios = new List<Usuario>();
        static List<Libro> listalibros = new List<Libro>();
        static List<Lectura> listalecturas = new List<Lectura>();

        static void Main(string[] args)
        {
            int opcion;

            Usuario usuariovencido = new Usuario(1, "Stiven Mesa", null, "stiven@mail.com");
            Suscripcion s1 = new Suscripcion(1, "Mensual", DateTime.Now, DateTime.Now);
            s1.FechaInicio = new DateTime(2024, 1, 1);
            s1.FechaFin = new DateTime(2024, 2, 1);
            usuariovencido.Suscripcion = s1;
            listausuarios.Add(usuariovencido);

            do
            {
                WriteLine("\n--- BIBLIOTECA DIGITAL ---");
                WriteLine("1. Crear usuario");
                WriteLine("2. Ver usuarios registrados");
                WriteLine("3. Agregar libro");
                WriteLine("4. Ver libros disponibles");
                WriteLine("5. Registrar suscripción");
                WriteLine("6. Iniciar lectura");
                WriteLine("7. FInalizar lectura");
                WriteLine("8. Ver lecturas");
                WriteLine("9. Salir");
                Write("\nSeleccione una opcion: ");

                opcion = int.Parse(ReadLine());

                switch (opcion)
                {
                    case 1:
                        crearUsuario();
                        break;

                    case 2:
                        if (listausuarios.Count == 0)
                        {
                            WriteLine($"\nNo hay usuarios");
                        }
                        foreach (Usuario u in listausuarios)
                        {
                            if (u.Suscripcion == null)
                            {
                                WriteLine($"ID: {u.IdUsuario}, Nombre: {u.NombreUsuario}, no tiene suscripción.");
                            }
                            else if (u.Suscripcion.estaActiva())
                            {
                                WriteLine($"ID: {u.IdUsuario}, Nombre: {u.NombreUsuario}, suscripción vence en: {u.Suscripcion.FechaFin}");
                            }
                            else
                            {
                                WriteLine($"ID: {u.IdUsuario}, Nombre: {u.NombreUsuario}, suscripción vencida.");
                            }
                        }
                        break;

                    case 3:
                        agregarLibro();
                        break;

                    case 4:
                        if (listalibros.Count == 0)
                        {
                            WriteLine("\nNo hay libros");
                        }
                        else
                        {
                            foreach (Libro l in listalibros)
                            {
                                WriteLine($"ISBN: {l.Isbn}, Nombre {l.NombreLibro}, Autor: {l.Autor}, Genero: {l.Genero}.");
                            }
                        }
                        break;

                    case 5:
                        registrarSuscripcion();
                        break;

                    case 6:
                        registrarLectura();
                        break;

                    case 7:
                        finalizarunaLectura();
                        break;

                    case 8:
                        if (listalecturas.Count == 0)
                        {
                            WriteLine("\nNo hay lecturas");
                        }
                        else
                        {
                            foreach (Lectura l in listalecturas)
                            {
                                if (l.LecFechafin == default(DateTime))
                                {
                                    WriteLine($"Lectura ID {l.IdLectura}, del libro {l.Libro.NombreLibro} realizada por {l.Usuario.NombreUsuario}, iniciada el {l.LecFechainicio}.");
                                }
                                else
                                {
                                    WriteLine($"Lectura ID {l.IdLectura}, del libro {l.Libro.NombreLibro} realizada por {l.Usuario.NombreUsuario}, iniciada el {l.LecFechainicio} y finalizada el {l.LecFechafin}.");
                                }
                            }
                        }
                        break;

                    case 9:
                        WriteLine("Saliendo...");
                        break;

                    default:
                        WriteLine("Opción inválida.");
                        break;
                }

            } while (opcion != 9);
        }

        public static void crearUsuario()
        {
            WriteLine("Ingrese los datos del usuario a crear\n");
            WriteLine("ID: ");
            int id = int.Parse(ReadLine());

            if (listausuarios.Exists(u => u.IdUsuario == id))
            {
                WriteLine($"Ya existe un usuario con el ID: {id}.");
                return;
            }

            WriteLine("Nombre: ");
            string nombre = ReadLine();

            WriteLine("Contacto: ");
            string contacto = ReadLine();

            try
            {
                Usuario nuevouser = new Usuario(id, nombre, null, contacto);
                listausuarios.Add(nuevouser);

                WriteLine("Usuario creado con exito");
            }
            catch (ArgumentException e)
            {
                WriteLine($"Error: {e.Message}");
            }
        }

        public static void agregarLibro()
        {
            WriteLine("ISBN: ");
            string isbn = ReadLine();

            if (listalibros.Exists(l => l.Isbn == isbn))
            {
                WriteLine($"Ya existe un libro con el ISBN: {isbn}.");
                return;
            }

            WriteLine("Nombre: ");
            string nombrelib = ReadLine();

            WriteLine("Autor: ");
            string autor = ReadLine();

            WriteLine("Genero: ");
            string genero = ReadLine();

            try
            {
                Libro nuevolibro = new Libro(isbn, nombrelib, autor, genero);
                listalibros.Add(nuevolibro);

                WriteLine("Libro añadido con exito");
            }
            catch (ArgumentException e)
            {
                WriteLine($"Error: {e.Message}");
            }
        }

        public static void registrarSuscripcion()
        {
            WriteLine("A que ID de usuario desea añadir una suscripcion");
            int iduser = int.Parse(ReadLine());

            Usuario usuario = listausuarios.Find(u => u.IdUsuario == iduser);

            if (usuario == null)
            {
                WriteLine($"Usuario con id: {iduser} no encontrado");
                return;
            }

            if (usuario.Suscripcion != null && usuario.Suscripcion.estaActiva())
            {
                WriteLine("El usuario ya tiene una suscripción activa.");
                return;
            }

            WriteLine("Desea que sea suscripcion Mensual o Anual");
            string tiposub = ReadLine();

            WriteLine("Ingrese el ID de la suscripcion");
            int idsuscripcion = int.Parse(ReadLine());

            try
            {
                Suscripcion nuevasub = new Suscripcion(idsuscripcion, tiposub, DateTime.Now, DateTime.Now);
                nuevasub.calculaFechafin();

                usuario.Suscripcion = nuevasub;

                WriteLine($"Suscripción para {usuario.NombreUsuario} ha sido registrada con exito y vence en {nuevasub.FechaFin}.");
            }
            catch (ArgumentException e)
            {
                WriteLine($"Error: {e.Message}");
            }
        }

        public static void registrarLectura()
        {
            if (listalibros.Count == 0)
            {
                WriteLine("\nNo hay libros para leer");
                return;
            }

            WriteLine("Que usuario va a leer? Ingrese su ID: ");
            int iduser = int.Parse(ReadLine());

            Usuario usuario = listausuarios.Find(u => u.IdUsuario == iduser);

            WriteLine("Que libro se leera? Ingrese su nombre");
            string nombrelib = ReadLine();

            Libro libro = listalibros.Find(l => l.NombreLibro == nombrelib);

            if (libro == null)
            {
                WriteLine("Libro no encontrado.");
                return;
            }

            try
            {
                Lectura nuevalectura = new Lectura((listalecturas.Count + 1), usuario, libro, DateTime.Now);
                nuevalectura.iniciarLectura();

                listalecturas.Add(nuevalectura);

                WriteLine($"El usuario con ID: {iduser} inicio la lectura del libro {libro.NombreLibro} con exito.");
            }
            catch (ArgumentException e)
            {
                WriteLine($"Error: {e.Message}");
            }
        }

        public static void finalizarunaLectura()
        {
            if (listalecturas.Count == 0)
            {
                WriteLine("\nNo hay lecturas");
                return;
            }

            WriteLine("Ingrese el ID de la lectura que desea finalizar:");
            int idLectura = int.Parse(ReadLine());

            Lectura lectura = listalecturas.Find(l => l.IdLectura == idLectura);

            if (lectura == null)
            {
                WriteLine("Lectura no encontrada.");
                return;
            }

            lectura.finalizarLectura();

            WriteLine("Lectura finalizada con éxito.");
        }
    }
}