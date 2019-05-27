using System;
using System.Collections.Generic;
using System.Linq;

namespace Alura.Tunes
{
    public class Program
    {
        static void Main(string[] args)
        {
            var generos = new List<Genero>
            {
                new Genero (1, "Rock"),
                new Genero (2, "Reggae"),
                new Genero (3, "Rock Progressivo"),
                new Genero (4, "Punk Rock"),
                new Genero (5, "Clássica"),
            };

            // Laço com filtro
            foreach (var item in generos)
            {
                if (item.Nome.Contains("Rock"))
                {
                    Impressao(item);
                }
            }

            Espaco();

            var query = from g in generos select g;

            foreach (var item in query)
            {
                Impressao(item);
            }

            Espaco();

            var queryLinq = from g in generos where g.Nome.Contains("Rock") select g;

            foreach (var item in queryLinq)
            {
                Impressao(item);
            }

        }

        private static void Impressao(Object item)
        {
            Console.WriteLine(item);
        }

        private static void Espaco()
        {
            Console.WriteLine();
        }
    }

    public class Genero
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public Genero(int id, string nome)
        {
            Id = id;
            Nome = nome;
        }

        public override string ToString()
        {
            return $"Id: {Id} \t Nome: {Nome}";
        }


    }
}
