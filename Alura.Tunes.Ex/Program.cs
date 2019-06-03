using Alura.Tunes.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alura.Tunes.Ex
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var contexto = new AluraTunesEntities())
            {
                var query = contexto.NotasFiscais;


                query.ToList().ForEach(q => Console.WriteLine(q.Cliente));
            }
        }
    }
}
