using System;
using System.Collections.Generic;

namespace BibliotecaBusqueda
{
    class Book
    {
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public int Anio { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }

        public Book(string titulo, string autor, int anio, string codigo, string descripcion)
        {
            Titulo = titulo;
            Autor = autor;
            Anio = anio;
            Codigo = codigo;
            Descripcion = descripcion;
        }

        public override string ToString()
        {
            return $"{Titulo} — {Autor} ({Anio}) [{Codigo}]";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<Book> libros = CrearBibliotecaDeEjemplo();
            List<string> autores = ExtraerAutoresYOrdenar(libros); // devuelve lista ordenada para búsqueda binaria

            while (true)
            {
                MostrarMenu();
                string opcion = Console.ReadLine();
                if (opcion == "0") break;

                switch (opcion)
                {
                    case "1":
                        Console.Write("Ingrese título o parte del título: ");
                        string titulo = Console.ReadLine();
                        var pos = BusquedaLinealPorTitulo(libros, titulo);
                        if (pos >= 0) Console.WriteLine($"Encontrado: {libros[pos]} (posición {pos})");
                        else Console.WriteLine("No se encontró ningún libro con ese título.");
                        break;

                    case "2":
                        Console.Write("Ingrese nombre de autor a buscar (exacto o parcial): ");
                        string autorBusq = Console.ReadLine();
                        int idx = BusquedaBinariaAutores(autores, autorBusq);
                        if (idx >= 0) Console.WriteLine($"Autor encontrado: {autores[idx]} (posición {idx} en lista de autores ordenada)");
                        else Console.WriteLine("Autor no encontrado (buscar con coincidencia exacta).");
                        break;

                    case "3":
                        var (antiguo, reciente) = MasRecienteYMasAntiguo(libros);
                        Console.WriteLine("Libro más antiguo: " + (antiguo != null ? antiguo.ToString() : "N/A"));
                        Console.WriteLine("Libro más reciente: " + (reciente != null ? reciente.ToString() : "N/A"));
                        break;

                    case "4":
                        Console.Write("Ingrese texto a buscar en descripciones: ");
                        string texto = Console.ReadLine();
                        var resultados = BusquedaCoincidenciasEnDescripcion(libros, texto);
                        if (resultados.Count == 0) Console.WriteLine("No hay coincidencias en las descripciones.");
                        else
                        {
                            Console.WriteLine("Coincidencias encontradas:");
                            foreach (var b in resultados) Console.WriteLine(b);
                        }
                        break;

                    case "5":
                        Console.WriteLine("Listado de libros:");
                        for (int i = 0; i < libros.Count; i++) Console.WriteLine($"{i}: {libros[i]}");
                        break;

                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }

                Console.WriteLine("\n--- Presione Enter para continuar ---");
                Console.ReadLine();
                Console.Clear();
            }
        }

        static void MostrarMenu()
        {
            Console.WriteLine("=== Biblioteca Digital - Módulo de Búsqueda ===");
            Console.WriteLine("1. Búsqueda lineal por título");
            Console.WriteLine("2. Búsqueda binaria por autor (lista ordenada)");
            Console.WriteLine("3. Encontrar libro más antiguo y más reciente");
            Console.WriteLine("4. Buscar coincidencias en descripciones");
            Console.WriteLine("5. Mostrar todos los libros");
            Console.WriteLine("0. Salir");
            Console.Write("Seleccione opción: ");
        }

        static List<Book> CrearBibliotecaDeEjemplo()
        {
            return new List<Book>()
            {
                new Book("Fundamentos de Programación", "Ana Perez", 2015, "BP-001", "Introducción a la programación con ejemplos en pseudocódigo."),
                new Book("Estructuras de Datos", "Luis Gómez", 2018, "BP-002", "Listas, pilas, colas, árboles y grafos con ejemplos prácticos."),
                new Book("Algoritmos y Complejidad", "María López", 2020, "BP-003", "Análisis de algoritmos, notación big-O y técnicas de diseño."),
                new Book("Bases de Datos", "Juan Torres", 2016, "BP-004", "Modelado relacional, SQL y modelos NoSQL."),
                new Book("Introducción a la IA", "Sofia Ruiz", 2021, "BP-005", "Conceptos básicos de inteligencia artificial y aprendizaje automático.")
            };
        }

        // Búsqueda lineal por título (coincidencia parcial, case-insensitive)
        static int BusquedaLinealPorTitulo(List<Book> libros, string titulo)
        {
            string t = titulo.ToLower();
            for (int i = 0; i < libros.Count; i++)
            {
                if (libros[i].Titulo.ToLower().Contains(t))
                    return i;
            }
            return -1;
        }

        // Extrae autores únicos y los ordena (ordenamiento simple) para búsqueda binaria
        static List<string> ExtraerAutoresYOrdenar(List<Book> libros)
        {
            List<string> autores = new List<string>();
            for (int i = 0; i < libros.Count; i++)
            {
                string a = libros[i].Autor;
                if (!autores.Contains(a)) autores.Add(a);
            }
            // Ordenamiento por nombre (insertion sort simple)
            for (int i = 1; i < autores.Count; i++)
            {
                string key = autores[i];
                int j = i - 1;
                while (j >= 0 && String.Compare(autores[j], key) > 0)
                {
                    autores[j + 1] = autores[j];
                    j--;
                }
                autores[j + 1] = key;
            }
            return autores;
        }