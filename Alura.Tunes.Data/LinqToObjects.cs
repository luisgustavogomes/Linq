using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alura.Tunes
{
    public class LinqToObjects
    {
        public List<Genero> Generos { get; set; }
        public List<Musica> Musicas { get; set; }
        public LinqToObjects()
        {
            Generos = new List<Genero>
            {
                new Genero (1, "Rock"),
                new Genero (2, "Reggae"),
                new Genero (3, "Rock Progressivo"),
                new Genero (4, "Punk Rock"),
                new Genero (5, "Clássica"),
            };

            Musicas = new List<Musica>
            {
                new Musica ( 1, "Sweet Child O'Mine" , 1),
                new Musica ( 2, "I Shoot The Sheriff", 2),
                new Musica ( 3, "Danúbio Azul"       , 5)
            };
        }


        public void LacoComLinqNativoComJoin()
        {
            Console.WriteLine("LacoComLinqNativoComJoin");
            var query = from m in Musicas
                        join g in Generos on m.GeneroId equals g.Id
                        select new { m, g };

            foreach (var item in query)
            {
                Console.WriteLine("{0}\t{1}\t{2}", item.m.Id, item.m.Nome, item.g.Nome);
            }
            Espaco();
        }

        public void LacoComLinqNativoComWhere()
        {
            Console.WriteLine("laço com linq nativo com where");
            var queryLinq = from g in Generos where g.Nome.Contains("Rock") select g;
            foreach (var item in queryLinq)
            {
                Impressao(item);
            }
            Espaco();
        }

        public void LacoComLinqNativoSemWhere()
        {
            Console.WriteLine("laço com linq nativo ");
            var query = from g in Generos select g;

            foreach (var item in query)
            {
                Impressao(item);
            }
            Espaco();
        }

        public void LacoComCondicional()
        {
            Console.WriteLine("Laço com Condicional");
            foreach (var item in Generos)
            {
                if (item.Nome.Contains("Rock"))
                {
                    Impressao(item);
                }
            }
            Espaco();
        }

        public static void Impressao(Object item)
        {
            Console.WriteLine(item);
        }

        public static void Espaco()
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

    public class Musica
    {

        public int Id { get; set; }
        public String Nome { get; set; }
        public int GeneroId { get; set; }


        public Musica(int id, string nome, int generoId)
        {
            Id = id;
            Nome = nome;
            GeneroId = generoId;
        }


        public override string ToString()
        {
            return $"Id: {Id} \t Nome: {Nome} \t IdGenero: {GeneroId}";
        }
    }
}
