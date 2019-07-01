using Alura.Tunes.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alura.Tunes.Data.Extension
{
    public static class sp_Vendas_Por_cliente_ResultExtension
    {
        public static string Impressao(this sp_Vendas_Por_cliente_Result i)
        {
            return $"Id.: {i.FaixaId} | Nome.: {i.nome}";
        }
    }
}
