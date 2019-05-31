using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Alura.Tunes.Data.Extension
{
    public static class LinqExtension
    {
        public static decimal Mediana<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal>> selector)
        {
            var quantidade = source.Count();

            var funcaoSelector = selector.Compile();

            var queryOrdenada = source.Select(funcaoSelector).OrderBy(q => q);
            decimal resultado = 0;

            if (quantidade % 2 == 0)
            {
                var elementoCentral_1 =  queryOrdenada.Skip(quantidade / 2).First();
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

        public static string Luis(this string valor)
        {
            return "Luis";
        }

    }
}
