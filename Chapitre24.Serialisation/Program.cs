using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Chapitre24.Serialisation
{
    class Program
    {
        private static string myRootPath;
        private static string myOutputXMLFileFullname;
        private static string myOutputXMLZIPFileFullname;

        static void Main(string[] args)
        {
            TestSerialisationXMLToCompressedFile(); // page 382

            Console.WriteLine("Press a keyboard key to exit");
            Console.ReadKey();
        }

        private static void TestSerialisationXMLToCompressedFile()
        {
            // Configuration
            myRootPath = @"C:\Temp"; // a adapter
            myOutputXMLFileFullname = myRootPath + @"\ExempleDuLivre\Chapitre24.Serialisation\Tests\XML\ReplacedField.xml";
            myOutputXMLZIPFileFullname = myRootPath + @"\ExempleDuLivre\Chapitre24.Serialisation\Tests\XMLZIP\ReplacedField.xml.zip";

            // If the output file alreary exists, delete it
            FileInfo fileInfo = new FileInfo(myOutputXMLFileFullname);
            fileInfo.Delete();
            fileInfo = new FileInfo(myOutputXMLZIPFileFullname);
            fileInfo.Delete();

            // Object instanciation and initialization.
            ReplacedField O = new ReplacedField {
                Pattern = "myPattern",
                Field = "myField",
                HasChanged = true};

            // XML serializer creation.
            XmlSerializer serializer = new XmlSerializer(typeof(ReplacedField));

            // Flux creation.
            Stream stream = new FileStream(myOutputXMLFileFullname, FileMode.Create);

            // Object serialisation into the flux.
            serializer.Serialize(stream, O);
            stream.Close();
            stream.Dispose();
        }
    }

    public class ReplacedField
    {
        #region Fields
        private string pattern;
        private string field;
        private bool hasChanged;
        #endregion

        #region Properties
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
