using Alura.Tunes.Data.Data;
using Alura.Tunes.Data.Extension;
using System;
using System.Linq;

namespace Alura.Tunes.Data
{
    public partial class Program
    {
        public const int TAMANHO_PAGINA = 10;
        private const string NOME_DA_MUSICA = "Smells Like Teen Spirit";

        public static void Main(string[] args)
        {

            using (var contexto = new AluraTunesEntities())
            {
                
            }

        }

        private static void ExemploDeExecucaoTardia(AluraTunesEntities contexto)
        {
            var mesAniversario = 1;

            while (mesAniversario <= 12)
            {
                Console.WriteLine("Mês: {0}", mesAniversario);

                var lista = (from f in contexto.Funcionarios
                             where f.DataNascimento.Value.Month == mesAniversario
                             orderby f.DataNascimento.Value.Month, f.DataNascimento.Value.Day
                             select f).ToList();

                mesAniversario++;
                lista.ForEach(f => Console.WriteLine("{0:dd/MM}\t{1} {2}", f.DataNascimento, f.PrimeiroNome, f.Sobrenome));

                //Console.WriteLine("==============================");


                //var query2 = contexto.Funcionarios
                //                .OrderBy(f => f.DataNascimento.Value.Month)
                //                .ThenBy(f => f.DataNascimento.Value.Day);

                //query2.ToList().ForEach(f => Console.WriteLine("{0:dd/MM}\t{1} {2}", f.DataNascimento, f.PrimeiroNome, f.Sobrenome));

            }
        }

        public static void SelfJoinSintaxeDeConsulta(AluraTunesEntities contexto)
        {
            var faixaIds = contexto.Faixas
                            .Where(f => f.Nome.Contains(NOME_DA_MUSICA))
                            .Select(f => f.FaixaId);

            faixaIds.ToList().ForEach(f => Console.WriteLine(f));

            //===================================================================//
            var faixasItens1 = contexto.ItensNotaFiscal;
            var faixasItens2 = contexto.ItensNotaFiscal;

            //var query1 = faixasItens1.AsEnumerable()
            //                .Join(faixasItens2.AsEnumerable(),
            //                    f => f.FaixaId,
            //                    ff => ff.FaixaId,
            //                (f, ff) => new
            //                {
            //                    faixasItens1 = f,
            //                    faixasItens2 = ff
            //                })
            //                .Where(q => q.Any);

            //query1.ToList().ForEach(f => Console.WriteLine(f.Impressao()));

            //===================================================================//

            Console.WriteLine();

            var query2 = from c in contexto.ItensNotaFiscal
                         join cc in contexto.ItensNotaFiscal on c.NotaFiscalId equals cc.NotaFiscalId
                         where faixaIds.Contains(c.FaixaId)
                          && c.FaixaId != cc.FaixaId
                         select cc;
            query2.ToList().ForEach(f => Console.WriteLine(f.Impressao()));
        }

        public static void ClienteQueComprouOItemMaisVendido(AluraTunesEntities contexto)
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
