using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Alura.Tunes
{
    public class LinqToXml
    {
        public XElement Root { get; set; }

        public LinqToXml()
        {
            Root = XElement.Load(@"Data\AluraTunes.xml");
        }

        public void LacoLinqNativoComXmlSimples()
        {

            Console.WriteLine("LacoLinqNativoComXmlSimples");
            var queryGeneros = from g in Root.Element("Generos").Elements("Genero")
                               select g;
            foreach (var item in queryGeneros)
            {
                Console.WriteLine("{0}\t{1}", item.Element("GeneroId").Value, item.Element("Nome").Value);
            }
            LinqToObjects.Espaco();
        }

        public void LacoLinqNativoComXmlJoin()
        {
            Console.WriteLine("LacoLinqNativoComXmlJoin");
            var query = from g in Root.Element("Generos").Elements("Genero")
                        join m in Root.Element("Musicas").Elements("Musica") 
                            on g.Element("GeneroId").Value equals m.Element("GeneroId").Value
                        select new
                        {
                            Musica = m.Element("Nome").Value,
                            Genero = g.Element("Nome").Value
                        };

            foreach (var item in query)
            {
                Console.WriteLine("{0}\t{1}", item.Musica, item.Genero);
            }
            LinqToObjects.Espaco();
        }

        public void LacoLinqNativoComXmlSimplesComJoin()
        {

            Console.WriteLine("LacoLinqNativoComXmlSimplesComJoin");
            var query = from g in Root.Element("Generos").Elements("Genero")
                        join m in Root.Element("Musicas").Elements("Musica")
                            on g.Element("GeneroId").Value equals m.Element("GeneroId").Value
                        select new
                        {
                            Id = m.Element("MusicaId").Value,
                            Musica = m.Element("Nome").Value,
                            Genero = g.Element("Nome").Value
                        };
            foreach (var item in query)
            {
                Console.WriteLine("{0}\t{1}\t{2}", item.Id, item.Musica, item.Genero);
            }
            LinqToObjects.Espaco();
        }


    }
}
