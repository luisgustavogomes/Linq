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
                TrabalhandoComLinqSintaxeDeMedotoSum(contexto);
            }
        }

        public static void TrabalhandoComLinqSintaxeDeMedotoSum(AluraTunesEntities contexto)
        {
            var textoBusca = "Led Zeppelin";

            var query = from i in contexto.ItensNotaFiscal
                        where i.Faixa.Album.Artista.Nome.Equals(textoBusca)
                        select new
                        {
                            i.Faixa.Nome,
                            i.Quantidade,
                            i.PrecoUnitario,
                            TotalDoItem = i.Quantidade * i.PrecoUnitario
                        };            
            query.ToList().ForEach(q => Console.WriteLine("{0}\t{1}\t{2}\t{3}", q.Nome.PadRight(40), q.Quantidade, q.PrecoUnitario, q.TotalDoItem));
            Console.WriteLine($"Total: {query.Sum(q => q.TotalDoItem)}");

            LinqToObjects.Espaco();
            Console.WriteLine("==========================================================================");

            var query2 = contexto.ItensNotaFiscal
                            .Where(i => i.Faixa.Album.Artista.Nome.Equals(textoBusca));
            query2
                .ToList()
                .ForEach(q => 
                    Console.WriteLine("{0}\t{1}\t{2}\t{3}", q.Faixa.Nome.PadRight(40), q.Quantidade, q.PrecoUnitario, (q.Quantidade*q.PrecoUnitario)));

            Console.WriteLine($"Total: {query2.Sum(q => (q.Quantidade * q.PrecoUnitario))}");

            Console.WriteLine("==========================================================================");

        }


        public static void TrabalhandoComLinqSintaxeDeMedotoCount(AluraTunesEntities contexto)
        {
            var textoBusca = "Led Zeppelin";
            var query = from f in contexto.Faixas
                        where f.Album.Artista.Nome == textoBusca
                        select f;
            var quantidadeSintaxeConsulta = query.Count();
            query.ToList().ForEach(q => Console.WriteLine("{0}\t{1}", q.Album.Titulo.PadRight(35), q.Nome));
            Console.WriteLine($"Qtde: {quantidadeSintaxeConsulta}");

            LinqToObjects.Espaco();

            var query2 = contexto.Faixas.Where(f => f.Album.Artista.Nome.Equals(textoBusca));
            var quantidadeSintaxeMetodo = query.Count();
            var quantidadeSintaxeMetodo2 = contexto.Faixas.Count(f => f.Album.Artista.Nome.Equals(textoBusca));
            query2.ToList().ForEach(q => Console.WriteLine("{0}\t{1}", q.Album.Titulo.PadRight(35), q.Nome));
            Console.WriteLine($"Qtde: {quantidadeSintaxeMetodo}");
            Console.WriteLine($"Qtde2: {quantidadeSintaxeMetodo2}");

        }

        public static void TrabalhandoComLinqSintaxeDeMedotoComJoin3(AluraTunesEntities contexto)
        {
            var busaArtista = "led";
            var buscaAlbum = "";
            GetFaixas2(contexto, busaArtista, buscaAlbum);
        }

        public static void GetFaixas2(AluraTunesEntities contexto, string busaArtista, string buscaAlgum)
        {
            var query = contexto.Faixas.Where(f => f.Album.Artista.Nome.Contains(busaArtista));
            if (!string.IsNullOrEmpty(buscaAlgum))
            {
                query = query.Where(q => q.Album.Titulo.Contains(buscaAlgum));
            }
            query = query.OrderBy(q => q.Album.Titulo).ThenBy(q=>q.Nome);
            query.ToList().ForEach(q => Console.WriteLine("{0}\t{1}", q.Album.Titulo.PadRight(35), q.Nome));
        }

        public static void TrabalhandoComLinqSintaxeDeMedotoComJoin2(AluraTunesEntities contexto)
        {
            var busaArtista = "led";
            var buscaAlbum = "Coda";
            GetFaixas(contexto, busaArtista, buscaAlbum);
        }

        public static void GetFaixas(AluraTunesEntities contexto, string busaArtista, string buscaAlgum)
        {
            var query = contexto.Faixas.Where(f => f.Album.Artista.Nome.Contains(busaArtista));
            if (!string.IsNullOrEmpty(buscaAlgum))
            {
                query = query.Where(q => q.Album.Titulo.Contains(buscaAlgum));
            }
            query.ToList().ForEach(q => Console.WriteLine("{0}\t{1}", q.Album.Titulo.PadRight(35), q.Nome));
        }

        public static void TrabalhandoComLinqSintaxeDeMedotoComJoin(AluraTunesEntities contexto)
        {
            var textoDeBusca = "Led";

            var query = from a in contexto.Artistas
                        join alb in contexto.Albums
                             on a.ArtistaId equals alb.ArtistaId
                        where a.Nome.Contains(textoDeBusca)
                        select new { a, alb };

            query.ToList().ForEach(q => Console.WriteLine("{0}\t{1}\t{2}", q.a.ArtistaId, q.a.Nome, q.alb.Titulo));

            Console.WriteLine();

            var query2 = from alb in contexto.Albums
                         where alb.Artista.Nome.Contains(textoDeBusca)
                         select new
                         {
                             NomeArtista = alb.Artista.Nome,
                             NomeAlbum = alb.Titulo
                         };

            query2.ToList().ForEach(q => Console.WriteLine("{0}\t{1}", q.NomeArtista, q.NomeAlbum));
        }

        public static void TrabalhandoComLinqSintaxeDeMetodo(AluraTunesEntities contexto)
        {
            var textoBusca = "l";
            //contexto.Database.Log = Console.WriteLine;
            var query2 = contexto.Artistas.Where(a => a.Nome.Contains(textoBusca));
            query2.ToList().ForEach(q => Console.WriteLine("{0}\t{1}", q.ArtistaId, q.Nome));
        }

        public static void TrabalhandoComLinqSintaxeDeConsultaQueryComJoin(AluraTunesEntities contexto)
        {
            var faixaEGenero = from g in contexto.Generos
                               join f in contexto.Faixas
                                    on g.GeneroId equals f.GeneroId
                               where g.Nome.Contains("Drama")
                               select new { f, g };

            contexto.Database.Log = Console.WriteLine;
            faixaEGenero = faixaEGenero.Take(10);

            foreach (var item in faixaEGenero)
            {
                Console.WriteLine("{0}\t{1}", item.f.Nome, item.g.Nome);
            }
        }

        public static void TrabalhandoComLinqSintaxeDeConsultaUmaTabelaSimples(AluraTunesEntities contexto)
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
