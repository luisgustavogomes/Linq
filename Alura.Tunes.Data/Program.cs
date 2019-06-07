﻿using Alura.Tunes.Data.Data;
using Alura.Tunes.Data.Extension;
using System;
using System.Linq;

namespace Alura.Tunes.Data
{
    public partial class Program
    {
        public const int TAMANHO_PAGINA = 10;

        public static void Main(string[] args)
        {
            using (var contexto = new AluraTunesEntities())
            {

                //contexto.Database.Log = Console.WriteLine;
                var query = contexto.Faixas
                    .Where(w => w.ItemNotaFiscals.Count > 0)
                    .Select(s => new
                    {
                        Id = s.FaixaId,
                        s.Nome,
                        Total = s.ItemNotaFiscals.Sum(i => (i.Quantidade * i.PrecoUnitario))
                    })
                    .OrderByDescending(o => o.Total);


                //query
                //    .AsParallel()
                //    .ToList()
                //    .ForEach(q => Console.WriteLine("{0}\t{1}\t{2}\t", q.Id, q.Nome.PadRight(40), q.Total));

                var primeiroItem = query.First();
                var queryCliente = contexto.ItensNotaFiscal
                                    .Where(w => w.FaixaId == primeiroItem.Id)
                                    .Select(s => new
                                    {
                                        NomeCliente = s.NotaFiscal.Cliente.PrimeiroNome + " " + s.NotaFiscal.Cliente.Sobrenome
                                    });

                queryCliente.ToList()
                    .OrderBy(o => o.NomeCliente)
                    .ToList()
                    .ForEach(l => Console.WriteLine(l.NomeCliente));


                //===========================================================================================================//

                var query2 = from f in contexto.Faixas
                             where f.ItemNotaFiscals.Count > 0
                             let TotalVendas = f.ItemNotaFiscals.Sum(i => i.Quantidade * i.PrecoUnitario)
                             orderby TotalVendas descending
                             select new
                             {
                                 f.FaixaId,
                                 f.Nome,
                                 Total = TotalVendas
                             };
                //query2
                //    .AsParallel()
                //    .ToList()
                //    .ForEach(q => Console.WriteLine("{0}\t{1}\t{2}\t", q.Id, q.Nome.PadRight(40), q.Total));

                var primeiroItem2 = query.First();




            }

        }

        public static void ImprimirRelatorio(AluraTunesEntities contexto)
        {
            var numeroNotasFiscais = contexto.NotasFiscais.Count();
            var numeropaginas = Math.Ceiling((decimal)numeroNotasFiscais / TAMANHO_PAGINA);

            for (int i = 1; i <= numeropaginas; i++)
            {
                Console.WriteLine("===================================");
                ImprimirPagina(contexto, i);
                Console.WriteLine($"Página {i} de {numeropaginas}".PadLeft(20));
            }
        }

        public static void ImprimirPagina(AluraTunesEntities contexto, int numeroPagina)
        {
            var query = contexto.NotasFiscais.Select(s => new
            {
                Numero = s.NotaFiscalId,
                Data = s.DataNotaFiscal,
                Cliente = s.Cliente.PrimeiroNome + " " + s.Cliente.Sobrenome,
                s.Total
            });

            var numeroDePulos = (numeroPagina - 1) * TAMANHO_PAGINA;

            query = query.OrderBy(o => o.Numero);

            query = query.Skip(numeroDePulos);
            query = query.Take(TAMANHO_PAGINA);

            foreach (var nf in query)
            {
                Console.WriteLine("{0}\t{1}\t{2}\t{3}", nf.Numero, nf.Data, nf.Cliente.PadRight(20), nf.Total);
            }
        }

    }
}
