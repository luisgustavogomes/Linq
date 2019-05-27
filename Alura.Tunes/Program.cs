using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Alura.Tunes
{
    public class Program
    {
        static void Main(string[] args)
        {
            TrabalhandoComLinqToXml();
        }

        private static void TrabalhandoComLinqToXml()
        {
            var linqToXml = new LinqToXml();
            linqToXml.LacoLinqNativoComXmlSimples();
            linqToXml.LacoLinqNativoComXmlJoin();
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
