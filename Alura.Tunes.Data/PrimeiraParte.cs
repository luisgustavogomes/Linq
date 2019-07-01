using Alura.Tunes.Data.Data;
using Alura.Tunes.Data.Extension;
using System;
using System.Linq;

namespace Alura.Tunes.Data
{
    public static class PrimeiraParte
    {
        public static void MetodosDeExtensao(AluraTunesEntities contexto)
        {
            //contexto.Database.Log = Console.WriteLine;
            var Media = contexto.NotaFiscals.Average(n => n.Total);
            Console.WriteLine(Media);

            var query = contexto.NotaFiscals.Select(t => t.Total);
            var mediana = Mediana(query);
            Console.WriteLine(mediana);

            var resultadoMedidaExtension = query.Mediana(m => m);
            Console.WriteLine(resultadoMedidaExtension);



        }

        public static decimal Mediana(IQueryable<decimal> query)
        {
            var quantidade = query.Count();
            var queryOrdenada = query.OrderBy(q => q);
            decimal resultado = 0;

            if (quantidade % 2 == 0)
            {
                var elementoCentral_1 = queryOrdenada.Skip(quantidade / 2).First();
                return elementoCentral_1;
            }
            else
            {
                var elementoCentral_1 = queryOrdenada.Skip(quantidade / 2).First();
                var elementoCentral_2 = queryOrdenada.Skip((quantidade - 1) / 2).First();
                resultado = ((elementoCentral_1 + elementoCentral_2) / 2);
            }

            return resultado;
        }

        public static void MetodoMatematicos(AluraTunesEntities contexto)
        {
            contexto.Database.Log = Console.WriteLine;
            //var Maximo = contexto.NotasFiscais.Max(n => n.Total);
            //var Minimo = contexto.NotasFiscais.Min(n => n.Total);
            //var Media  = contexto.NotasFiscais.Average(n => n.Total);

            //Console.WriteLine("{0}\t{1}\t{2}",Maximo, Minimo, Media);

            //var query = from nf in contexto.NotasFiscais
            //            group nf by 1 into agrupado
            //            select new
            //            {
            //                Maximo = agrupado.Max(nf => nf.Total),
            //                Minimo = agrupado.Min(nf => nf.Total),
            //                Media = agrupado.Average(nf => nf.Total),
            //            };
            //query
            //    .ToList()
            //    .ForEach(q => Console.WriteLine("{0}\t{1}\t{2}", q.Maximo, q.Minimo, q.Media));

            var query2 = (from nf in contexto.NotaFiscals
                          group nf by 1 into agrupado
                          select new
                          {
                              Maximo = agrupado.Max(nf => nf.Total),
                              Minimo = agrupado.Min(nf => nf.Total),
                              Media = agrupado.Average(nf => nf.Total),
                          }).Single();

            Console.WriteLine("{0}\t{1}\t{2}", query2.Maximo, query2.Minimo, query2.Media);



        }

        public static void MedotoGroupBY(AluraTunesEntities contexto)
        {
            var textoBusca = "Led Zeppelin";

            var query = from i in contexto.ItemNotaFiscals
                        where i.Faixa.Album.Artista.Nome.Equals(textoBusca)
                        group i by i.Faixa.Album into agrupado
                        let somaAgrupado = agrupado.Sum(q => q.Quantidade * q.PrecoUnitario)
                        orderby somaAgrupado descending
                        select new
                        {
                            TituloDoAlbum = agrupado.Key.Titulo
                            ,
                            TotalPorAlbum = somaAgrupado
                        };
            query
                .ToList()
                .ForEach(q => Console.WriteLine("{0}\t{1}",
                                q.TituloDoAlbum.PadRight(40),
                                q.TotalPorAlbum));

            LinqToObjects.Espaco();

            var query2 = contexto.ItemNotaFiscals
                            .Where(q => q.Faixa.Album.Artista.Nome.Equals(textoBusca))
                            .GroupBy(g => g.Faixa.Album)
                            .Select(s => new
                            {
                                TituloDoAlbum = s.Key.Titulo,
                                TotalPorAlbum = s.Sum(soma => soma.PrecoUnitario * soma.Quantidade)
                            })
                            .OrderByDescending(o => o.TotalPorAlbum);
            query2
                .ToList()
                .ForEach(q => Console.WriteLine("{0}\t{1}",
                                q.TituloDoAlbum.PadRight(40),
                                q.TotalPorAlbum));


        }

        public static void MedotoSum(AluraTunesEntities contexto)
        {
            var textoBusca = "Led Zeppelin";

            var query = from i in contexto.ItemNotaFiscals
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

            var query2 = contexto.ItemNotaFiscals
                            .Where(i => i.Faixa.Album.Artista.Nome.Equals(textoBusca));
            query2
                .ToList()
                .ForEach(q =>
                    Console.WriteLine("{0}\t{1}\t{2}\t{3}", q.Faixa.Nome.PadRight(40), q.Quantidade, q.PrecoUnitario, (q.Quantidade * q.PrecoUnitario)));

            Console.WriteLine($"Total: {query2.Sum(q => (q.Quantidade * q.PrecoUnitario))}");

            Console.WriteLine("==========================================================================");

        }

        public static void MedotoCount(AluraTunesEntities contexto)
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

        public static void MedotoComJoin3(AluraTunesEntities contexto)
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
            query = query.OrderBy(q => q.Album.Titulo).ThenBy(q => q.Nome);
            query.ToList().ForEach(q => Console.WriteLine("{0}\t{1}", q.Album.Titulo.PadRight(35), q.Nome));
        }

        public static void MedotoComJoin2(AluraTunesEntities contexto)
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

        public static void MedotoComJoin(AluraTunesEntities contexto)
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

        public static void Metodo(AluraTunesEntities contexto)
        {
            var textoBusca = "l";
            //contexto.Database.Log = Console.WriteLine;
            var query2 = contexto.Artistas.Where(a => a.Nome.Contains(textoBusca));
            query2.ToList().ForEach(q => Console.WriteLine("{0}\t{1}", q.ArtistaId, q.Nome));
        }

        public static void ConsultaQueryComJoin(AluraTunesEntities contexto)
        {
            var faixaEGenero = from g in contexto.Generoes
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

        public static void ConsultaUmaTabelaSimples(AluraTunesEntities contexto)
        {
            var query = from g in contexto.Generoes
                        select g;

            var query2 = contexto.Generoes.Select(g => g).OrderByDescending(g => g.Nome);

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

        public static void ToXml()
        {
            var linqToXml = new LinqToXml();
            linqToXml.LacoLinqNativoComXmlSimples();
            linqToXml.LacoLinqNativoComXmlJoin();
            linqToXml.LacoLinqNativoComXmlSimplesComJoin();

        }

        public static void ToObjects()
        {
            var linqToObjects = new LinqToObjects();
            linqToObjects.LacoComCondicional();
            linqToObjects.LacoComLinqNativoSemWhere();
            linqToObjects.LacoComLinqNativoComWhere();
            linqToObjects.LacoComLinqNativoComJoin();
        }
    }
}
