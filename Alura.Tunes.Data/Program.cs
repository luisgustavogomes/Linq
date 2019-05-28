using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alura.Tunes.Data
{
    class Program
    {
        public static void Main(string[] args)
        {
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
