using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Chapitre24.Serialisation
{
    class Program
    {
        private static string myRootPath = @"C:\Temp"; // TO BE CONFIGURATE according to where is your solution directory
        private static string myOutputXMLFileFullname = myRootPath + @"\ExempleDuLivre\Chapitre24.Serialisation\Tests\XML\ReplacedField.xml";
        private static string myOutputXMLZIPFileFullname = myRootPath + @"\ExempleDuLivre\Chapitre24.Serialisation\Tests\XMLZIP\ReplacedField.xml.zip";

        static void Main(string[] args)
        {
            TestSerialisationXMLToCompressedFile(); // page 382

            TestDeserialisationCompressedXMLFileToObject(); // page 383

            TestLINQtoXML(); //page 347

            TestXMLAdd(); // https://docs.microsoft.com/fr-fr/dotnet/csharp/programming-guide/concepts/linq/adding-elements-attributes-and-nodes-to-an-xml-tree

            testXMLAdd2();

            //TestConvertToCSV(); //https://www.codeproject.com/Questions/171508/how-to-convert-xml-to-csv-in-c-net

            Console.WriteLine("Press a keyboard key to exit");
            Console.ReadKey();
        }







        /// <summary>
        /// Serialization XML of an objet into a compressed file.
        /// </summary>
        private static void TestSerialisationXMLToCompressedFile()
        {

            // If the output file alreary exists, delete it
            FileInfo fileInfo = new FileInfo(myOutputXMLFileFullname);
            fileInfo.Delete();
            fileInfo = new FileInfo(myOutputXMLZIPFileFullname);
            fileInfo.Delete();

            // Object instanciation and initialization.
            ReplacedField O = new ReplacedField();


            // XML serializer creation.
            XmlSerializer serializer = new XmlSerializer(typeof(ReplacedField));

            // Flux creation.
            Stream stream = new FileStream(myOutputXMLFileFullname, FileMode.Create);

            // Object serialisation into the flux.
            serializer.Serialize(stream, O);
            stream.Close();
            stream.Dispose();

            // File compression.
            fileInfo = new FileInfo(myOutputXMLFileFullname);
            String startPath = fileInfo.DirectoryName;
            String zipPath = myOutputXMLZIPFileFullname;
            ZipFile.CreateFromDirectory(startPath, zipPath);
        }

        /// <summary>
        /// Deserialization XML of an objet from a compressed file
        /// </summary>
        private static void TestDeserialisationCompressedXMLFileToObject()
        {

            // If the output uncompressed file alreary exists, delete it
            FileInfo fileInfo = new FileInfo(myOutputXMLFileFullname);
            fileInfo.Delete();

            // File decompression
            String zipPath = myOutputXMLZIPFileFullname;
            fileInfo = new FileInfo(myOutputXMLFileFullname);
            String extractPath = fileInfo.DirectoryName;
            ZipFile.ExtractToDirectory(zipPath, extractPath);

            // XML serializer creation
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ReplacedField));

            // Flux creation
            Stream stream = new FileStream(myOutputXMLFileFullname, FileMode.Open);

            // Object deserialization from the flux
            ReplacedField O = (ReplacedField)xmlSerializer.Deserialize(stream);
            stream.Close();
            stream.Dispose();

            // To check, display of object
            Console.WriteLine($"Pattern : {O.Pattern} \n" +
                $"Field : {O.Field} \n" +
                $"HasChanged : {O.HasChanged}");


        }

        /// <summary>
        /// To test LINQ to XML
        /// </summary>
        private static void TestLINQtoXML()
        {
            // Load a XML document into memory
            XDocument xDocument = XDocument.Load(myOutputXMLFileFullname);

            string s1 = xDocument.Root.Name.ToString();
            string s2 = xDocument.FirstNode.NodeType.ToString();
            Console.WriteLine($"s1:{s1} \n s2:{s2}");

            // Example of a request
            var query = from ms in xDocument.Descendants("Enregistrement")
                        select new { Enregistrement = ms.Element("Enregistrement").Value };

            foreach (var item in query)
            {
                Console.WriteLine(item);
            }

            //https://docs.microsoft.com/fr-fr/dotnet/csharp/programming-guide/concepts/linq/linq-to-xml-overview
            // Try with XElement instead of XDocument
            XElement purchaseOrder = XElement.Load(@"C:\Temp\ExempleDuLivre\Chapitre24.Serialisation\Tests\PurchaseOrder.xml");

            Console.WriteLine("\nListe de valeur d'attribut de numéro de référence pour chaque " +
                "élément de la commande fournisseur");
            //IEnumerable<string> partNos = from item in purchaseOrder.Descendants("Item")
            //                                     select (string)item.Attribute("PartNumber");
            IEnumerable<string> partNos = purchaseOrder.Descendants("Item")
                .Select(x => (string)x.Attribute("PartNumber"));
            foreach (var part in partNos)
            {
                Console.WriteLine($"part: {part}");
            }

            Console.WriteLine("\nListe des éléments avec une valeur supérieure à $ 100, triés par numéro de référence.");
            //IEnumerable<XElement> pricesByPartNos = from item in purchaseOrder.Descendants("Item")
            //                                      where (int)item.Element("Quantity") * (decimal)item.Element("USPrice") > 100
            //                                      orderby (string)item.Element("PartNumber")
            //                                      select item;
            IEnumerable<XElement> pricesByPartNos = purchaseOrder.Descendants("Item")
                .Where(item => (int)item.Element("Quantity") * (decimal)item.Element("USPrice") > 100)
                .OrderBy(order => order.Element("PartNumber"));

            foreach (var pricesByPartNo in pricesByPartNos)
            {
                Console.WriteLine($"{pricesByPartNo.Element("ProductName").Value}");
            }

            Console.WriteLine("\nListe des villes où d'attribut du l'adresse est Billing");
            var cities = purchaseOrder.Descendants("Address")
                .Where(item => (string)item.Attribute("Type") == "Billing")
                .Select(x => (string)x.Element("City"));
            foreach (var city in cities)
            {
                Console.WriteLine($"city: {city}");
            }

            Console.WriteLine("\nListe des villes où le pays est USA");
            var uscities = purchaseOrder.Descendants("Address")
                .Where(item => (string)item.Element("Country") == "USA")
                .Select(x => (string)x.Element("City"));
            foreach (var city in uscities)
            {
                Console.WriteLine($"USA city: {city}");
            }

        }

        /// <summary>
        /// Concatenate into an XML
        /// https://docs.microsoft.com/fr-fr/dotnet/csharp/programming-guide/concepts/linq/adding-elements-attributes-and-nodes-to-an-xml-tree
        /// </summary>
        private static void TestXMLAdd()
        {
            // Configuration and reset
            string outputFile = @"C:\Temp\ExempleDuLivre\Chapitre24.Serialisation\Tests\PurchaseOrderAdded.xml";
            FileInfo fileInfo = new FileInfo(outputFile);
            fileInfo.Delete();

            // Load in menory the XML example
            XDocument xDocument = XDocument.Load(@"C:\Temp\ExempleDuLivre\Chapitre24.Serialisation\Tests\PurchaseOrder.xml");

            // XML to add
            XElement xElement1 = new XElement("NouvelEnfant1", "Contenu de l'enfant 1.");
            XElement xElement2 = new XElement("NouvelEnfant2", "Contenu de l'enfant 2.");
            XElement xElement3 = new XElement("NouvelElement", xElement1, xElement2);


            // Adding
            xDocument.Root.Add(xElement3);

            xDocument.Save(outputFile);
            Console.WriteLine($"The output file is {outputFile}");
            /* On retouve en bas du fichier xml:
               </Items>
                    <NouvelElement>
                        <NouvelEnfant1>Contenu de l'enfant 1.</NouvelEnfant1>
                        <NouvelEnfant2>Contenu de l'enfant 2.</NouvelEnfant2>
                    </NouvelElement>
            </PurchaseOrder>* 
             * */

        }

        private static void testXMLAdd2()
        {
            // Configuration and reset
            string outputFile = @"C:\Temp\ExempleDuLivre\Chapitre24.Serialisation\Tests\PurchaseOrderAdded2.xml";
            FileInfo fileInfo = new FileInfo(outputFile);
            fileInfo.Delete();

            // Load in menory the XML example
            XDocument xDocument = XDocument.Load(@"C:\Temp\ExempleDuLivre\Chapitre24.Serialisation\Tests\PurchaseOrder.xml");

            // XML to add
            // Object instanciation and initialization.
            ReplacedField O = new ReplacedField();

            // XML serializer creation.
            XmlSerializer serializer = new XmlSerializer(typeof(ReplacedField));
            //XElement xElement = (XElement)serializer;

            // Adding
            //xDocument.Root.Add(xElement);

            xDocument.Save(outputFile);
            Console.WriteLine($"The output file is {outputFile}");

        }
    }

    public class Observation
    {
        private DateTime date;

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        private string enregistrement;

        public string Enregistrement
        {
            get { return enregistrement; }
            set { enregistrement = value; }
        }


    }

    [XmlRoot(ElementName = "myRootElement", Namespace = "myNamespace")]
    public class ReplacedField
    {
        #region Fields
        private string pattern;
        private string field;
        private bool hasChanged;

        public List<Observation> myObservations;
        #endregion

        #region Constructor
        public ReplacedField()
        {
            Pattern = "myPattern";
            Field = "myField";
            HasChanged = true;

            myObservations = new List<Observation>();
            myObservations.Add(new Observation { Date = DateTime.Now, Enregistrement = "premier enregistrement" });
            myObservations.Add(new Observation { Date = DateTime.Now, Enregistrement = "deuxieme enregistrement" });
            myObservations.Add(new Observation { Date = DateTime.Now, Enregistrement = "troisieme enregistrement" });
            myObservations.Add(new Observation { Date = DateTime.Now, Enregistrement = "quatrieme enregistrement" });
            myObservations.Add(new Observation { Date = DateTime.Now, Enregistrement = "cinquieme enregistrement" });
        }
        #endregion

        #region Properties
        [XmlElement(ElementName = "PatternElement")]
        public string Pattern
        {
            get { return pattern; }
            set { pattern = value; }
        }


        public string Field
        {
            get { return field; }
            set { field = value; }
        }



        public bool HasChanged
        {
            get { return hasChanged; }
            set { hasChanged = value; }
        }
        #endregion

    }
}
