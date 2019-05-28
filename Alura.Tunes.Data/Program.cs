using Alura.Tunes.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alura.Tunes.Data
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var contexto = new AluraTunesEntities())
            {
                var faixaEGenero = from g in contexto.Generos
                                   join f in contexto.Faixas
                                        on g.GeneroId equals f.GeneroId
                                   where g.Nome.Contains("Drama")
                                   select new { f, g };

                foreach (var item in faixaEGenero)
                {
                    Console.WriteLine("{0}\t{1}", item.f.Nome, item.g.Nome);
                }
            }
        }

        public static void TrabalhandoComLinqEntitiesQueryUmaTabelaSimples(AluraTunesEntities contexto)
        {
            var query = from g in contexto.Generos
                        select g;

            var query2 = contexto.Generos.Select(g => g).OrderByDescending(g => g.Nome);

            Console.WriteLine("\nLinqNativo");
            foreach (var item in query)
            {
                Console.WriteLine("{0}\t{1}", item.GeneroId, item.Nome);
            }

            Console.WriteLine("\nLinq");
            foreach (var item in query2)
            {
                Console.WriteLine("{0}\t{1}", item.GeneroId, item.Nome);
            }
        }

        public static void TrabalhandoComLinqToXml()
        {
            var linqToXml = new LinqToXml();
            linqToXml.LacoLinqNativoComXmlSimples();
            linqToXml.LacoLinqNativoComXmlJoin();
            linqToXml.LacoLinqNativoComXmlSimplesComJoin();

        }

        public static void TrabalhandoComLinqToObjects()
        {
            var linqToObjects = new LinqToObjects();
            linqToObjects.LacoComCondicional();
            linqToObjects.LacoComLinqNativoSemWhere();
            linqToObjects.LacoComLinqNativoComWhere();
            linqToObjects.LacoComLinqNativoComJoin();
        }
    }
}
