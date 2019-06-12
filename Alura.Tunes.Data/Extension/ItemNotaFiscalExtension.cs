using Alura.Tunes.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alura.Tunes.Data.Extension
{
    public static class ItensNotaFiscalExtension
    {

        public static string Impressao (this ItemNotaFiscal i)
        {
            return $"Id.: {i.NotaFiscalId} | Nome.: {i.Faixa.Nome}";
        }

    }
}
