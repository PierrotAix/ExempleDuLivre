using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Singleton
{
    class Program
    {
        static void Main(string[] args)
        {
            //Test01(); // https://h-deb.clg.qc.ca/Sujets/Divers--cdiese/Singleton.html

            //Test02(); // not thread safe https://www.youtube.com/watch?v=XZl3FgkYazI

            Test03(); // not thread safe https://www.youtube.com/watch?v=XZl3FgkYazI

            Console.ReadKey();
        }

        // BEGIN Test03 -------------------------------------------------------
        public class ThreadSafeSingleton
        {
            private static ThreadSafeSingleton _instance;
            private static readonly object _padlock = new object();

            private ThreadSafeSingleton()
            {

            }

            public static ThreadSafeSingleton Instance
            {
                get
                {

                    if (_instance == null)
                    {
                        lock (_padlock)
                        {
                            if (_instance == null)
                            {
                                _instance = new ThreadSafeSingleton();

                                _instance.Name = "Kevin";
                            }  
                        }
                    }

                    return _instance;
                }
            }

            public string Name { get; set; }
            public int NaNodeIdme { get; set; }
        }

        private static void Test03()
        {
            var tss = ThreadSafeSingleton.Instance;

            tss.Name = "Fred";

            Console.WriteLine(tss.Name);
        }


        // END Test03 -------------------------------------------------------

        // BEGIN Test02 -------------------------------------------------------
        // to enforce just one instance
        public class MyClass
        {
            private static MyClass _instance;

            private MyClass()
            {

            }

            public static MyClass Instance
            {
                get
                {
                    if (_instance == null)
                    {
                        _instance = new MyClass();
                    }
                    return _instance;
                }
            }

            public string Name { get; set; }

        }

        private static void Test02()
        {
            var ourClass = MyClass.Instance;

            ourClass.Name = "Fred";

            Console.WriteLine(ourClass.Name);

            // Try to create another instance

            var anotherClass = MyClass.Instance;

            Console.WriteLine(anotherClass.Name); // renvoie "Fred"

            Console.WriteLine("End of Test02");
        }


        // END Test02 -------------------------------------------------------

        // BEGIN Test01 -------------------------------------------------------
        sealed class GénérateurId
        {
            static GénérateurId singleton = null;
            int cur;
            GénérateurId()
            {
                cur = 0;
            }
            public static GénérateurId GetInstance()
            {
                if (singleton == null)
                    singleton = new GénérateurId();
                return singleton;
            }
            public int Next()
            {
                return cur++;
            }
        }

        enum TypeRéponse { OUI, NON };

        static bool EstRéponseValide(char réponse, char[] candidats)
        {
            return candidats.Contains(réponse);
        }

        static string Concaténer<T>(T[] elems)
        {
            string résultat = "";
            if (elems.Length > 0)
            {
                int i = 0;
                résultat += elems[i];
                for (++i; i < elems.Length; ++i)
                {
                    résultat += " " + elems[i];
                }
            }
            return résultat;
        }
        static TypeRéponse LireRéponse(string message)
        {
            char[] réponsesValides = { 'o', 'O', 'n', 'N' };
            Console.Write("{0} ({1}) ", message, Concaténer(réponsesValides));
            char réponse = char.Parse(Console.ReadLine());
            while (!EstRéponseValide(réponse, réponsesValides))
            {
                Console.Write("Erreur: {0}. {1} ({2}) ", réponse, message, Concaténer(réponsesValides));
                réponse = char.Parse(Console.ReadLine());
            }
            return réponse == 'o' || réponse == 'O' ? TypeRéponse.OUI : TypeRéponse.NON;
        }

        private static void Test01()
        {
            int val = GénérateurId.GetInstance().Next();
            Console.WriteLine("Nombre généré: {0}", val);
            while (LireRéponse("Un autre?") == TypeRéponse.OUI)
            {
                val = GénérateurId.GetInstance().Next();
                Console.WriteLine("Nombre généré: {0}", val);
            }
            Console.WriteLine("Au revoir!");
        }

        // END Test01 -------------------------------------------------------

    }
}
