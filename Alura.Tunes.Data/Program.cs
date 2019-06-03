using Alura.Tunes.Data.Data;
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
                var numeroPagina = 3;
                var numeroNotasFiscais = contexto.NotasFiscais.Count();
                var numeropaginas = numeroNotasFiscais / TAMANHO_PAGINA;
                ImprimirPagina(contexto, numeroPagina);

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
