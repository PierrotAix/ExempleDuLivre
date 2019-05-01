using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LINQ2008
{
    class Program
    {
        static void Main(string[] args)
        {
            //Test01Chapitre01(); // KO

            Test02Chapitre01();

            Console.WriteLine("Tapez une touche pour sortir.");
            Console.ReadKey();
        }

        private static void Test02Chapitre01()
        {
            XElement books;

            if (false)
            {
                // Lecture du document XML depuis un fichier
                books = XElement.Load("..\\..\\Books.xml"); 
            }
            else
            {
                books = XElement.Parse(
                @"
<books>
<book>
<title>Pro LINQ: Language Integrated Query en C# 2008</title>
<author>Joe Rattz</author>
</book>
<book>
<title>Pro WF: Windows Workflow en .NET 3.0</title>
<author>Bruce Bukovics</author>
</book>
<book>
<title>Pro VB: Windows Workflow en .NET 3.0</title>
<author>Andrew Troelsens</author>
</book>
</books>", LoadOptions.PreserveWhitespace);
            }

            IEnumerable<XElement> iebooks = books.Elements();

            // filtrage
            var titles = from book in books.Elements("book") //books
                         where (string) book.Element("author") == "Joe Rattz"
                         select book.Element("title");

            // Affichage
            foreach (var title in titles)
            {
                Console.WriteLine("titre: " + title.Value);
            }


        }

        private static void Test01Chapitre01()
        {
            //page 4
            XElement books = XElement.Parse(
                @"
<books>
<book>
<title>Pro LINQ: Language Integrated Query en C# 2008</title>
<author>Joe Rattz</author>
</book>
<book>
<title>Pro WF: Windows Workflow en .NET 3.0</title>
<author>Bruce Bukovics</author>
</book>
<book>
<title>Pro VB: Windows Workflow en .NET 3.0</title>
<author>Andrew Troelsens</author>
</book>
</books>", LoadOptions.PreserveWhitespace);

            Console.WriteLine($"books: {books}");

            int whiteSpaceNodes = books
                .DescendantNodesAndSelf()
                .OfType<XText>()
                .Where(tNode => tNode.ToString().Trim().Length == 0)
                .Count();
            Console.WriteLine("Count of white space nodes (preserving whitespace): {0}",
                whiteSpaceNodes);

            var titles = books.Descendants()
                ;
            if (false)
            { 
                titles = from book in books.Elements("books")
                                 //where (string) book.Element("author") == "Joe Rattz"
                             select book.Element("title");
            }

            foreach (var title in titles)
            {
                Console.WriteLine(title.Value);
            }

            Console.WriteLine("fin de Test01Chapitre01");
        }
    }
}
