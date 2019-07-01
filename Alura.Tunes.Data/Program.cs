using Alura.Tunes.Data.Data;
using Alura.Tunes.Data.Extension;
using System;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using ZXing;

namespace Alura.Tunes.Data
{
    public partial class Program
    {
        public const int TAMANHO_PAGINA = 10;
        private const string NOME_DA_MUSICA = "Smells Like Teen Spirit";
        private const string PASTA = "Imagens";

        public static void Main(string[] args)
        {
            int clienteId = 17;

            using (var contexto = new AluraTunesEntities())
            {
                var query =
                from v in contexto.sp_Vendas_Por_cliente(clienteId)
                group v by new { v.DataNotaFiscal.Year, v.DataNotaFiscal.Month } into a
                orderby a.Key.Year, a.Key.Month
                select new
                {
                    Ano = a.Key.Year,
                    Mes = a.Key.Month,
                    Total = a.Sum(c => c.Total)
                };

                query.ToList().ForEach(f => Console.WriteLine("{0}\t{1}\t{2}",f.Ano,f.Mes,f.Total));

            }

        }

        public static void QrCode()
        {
            var barcodeWhiter = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 300,
                    Height = 300
                }
            };


            if (!Directory.Exists(PASTA))
                Directory.CreateDirectory(PASTA);


            using (var contexto = new AluraTunesEntities())
            {
                var query = contexto.Faixas;
                var lista = query.ToList();

                var stopwatch = Stopwatch.StartNew();

                var queryCodigos = lista
                    .AsParallel()
                    .Select(s => new
                    {
                        Arquivo = string.Format("{0}\\{1}.{2}", PASTA, s.FaixaId, "jpeg"),
                        Imagem = barcodeWhiter.Write(string.Format("aluratunes.com/faixa/{0}", s.FaixaId))
                    });

                var qtde = queryCodigos.Count();
                stopwatch.Stop();

                Console.WriteLine("Códigos gerados: {0} em {1} segundos ", qtde, stopwatch.ElapsedMilliseconds/1000.0);
                // Gerou em 14,885 seg old
                // Gerou em 7,524  seg old

                stopwatch = Stopwatch.StartNew();

                //queryCodigos.AsParallel().ToList().ForEach(i => i.Imagem.Save(i.Arquivo, ImageFormat.Jpeg));
                queryCodigos.ForAll(i => i.Imagem.Save(i.Arquivo, ImageFormat.Jpeg));

                qtde = queryCodigos.Count();
                stopwatch.Stop();
                Console.WriteLine("Imagens salvas: {0} em {1} segundos ", qtde, stopwatch.ElapsedMilliseconds / 1000.0);
                // Gravou em 58,108 seg old
                // Gravou em 34,423 seg sem for all
                // Gravou em 25,73 seg com for all



            }
        }

        public static void ExemploDeExecucaoTardia(AluraTunesEntities contexto)
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

            var query1 = contexto.ItemNotaFiscals.AsEnumerable()
                                 .Join(contexto.ItemNotaFiscals.AsEnumerable(),
                                 i1 => i1.ItemNotaFiscalId,
                                 i2 => i2.ItemNotaFiscalId,
                                 (i2, i1) => i1);

            query1.ToList().ForEach(f => Console.WriteLine(f.Impressao()));

            //===================================================================//

            Console.WriteLine();

            var query2 = from c in contexto.ItemNotaFiscals
                         join cc in contexto.ItemNotaFiscals on c.NotaFiscalId equals cc.NotaFiscalId
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
            var queryCliente = contexto.ItemNotaFiscals
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
            var numeroNotasFiscais = contexto.NotaFiscals.Count();
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
            var query = contexto.NotaFiscals.Select(s => new
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
