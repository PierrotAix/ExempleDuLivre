using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExempleDeleguesEvenementsExpressionsLambdas
{
    class Program
    {
        static void Main(string[] args)
        {

            // pratique de https://openclassrooms.com/fr/courses/2818931-programmez-en-oriente-objet-avec-c/2819111-delegues-evenements-et-expressions-lambdas

            //Test01();

            Test02(); // Lambda expression

            Console.ReadKey();
        }

        delegate void DelegateInstruction(string s);

        private static void Test02()
        {
            /*
            DelegateType division = Calcul((a, b) =>
            {
                return (double)a / (double)b;
            }, 4, 5);
            */

            //https://www.dotnetdojo.com/guide-expressions-lambda-csharp/

            DelegateInstruction myFunc = str => { Console.WriteLine("Entree = {0}", str); };
            myFunc("Hello"); // affiche "Entree = Hello"

            Func<int, int> myDelegate = x => x * x;
            int square = myDelegate(5);

            Console.WriteLine(square); // square vaut 25


        }

        private static void Test01()
        {
            int[] monTableau = { 1, 5, 3, 10, 9, 4 };

            Console.WriteLine("Avant tri: ");
            foreach (var item in monTableau)
            {
                Console.WriteLine("item : " + item);
            }
            Console.WriteLine();

            //TrieurDeTableau monTrieur = new TrieurDeTableau();
            //monTrieur.DemoTri00(monTableau);

            //new TrieurDeTableau().DemoTri01(monTableau);

            //new TrieurDeTableau().DemoTri02(monTableau); // Exemple de la définition des fonctions de tri au moment de l'appel de la méthode

            //new TrieurDeTableau().DemoTri03(monTableau); // illustre le multicast

            //new TrieurDeTableau().DemoTri04(monTableau); // Illustre la cas DemoTri03 avec des dfinitions des méthodes intégrées

            // new TrieurDeTableau().DemoTri05(monTableau); // Illustre la cas DemoTri03 avec des dfinitions des méthodes intégrées

            new TrieurDeTableau().DemoTri06(monTableau);  //Utilisation de fonction lambda



            
        }
    }
    public class TrieurDeTableau
    {
        private delegate void DelegateTri(int[] tableau);

        private void TriAscendant(int[] tableau)
        {
            Array.Sort(tableau);
        }

        private void TriDescendant(int[] tableau)
        {
            Array.Sort(tableau);
            Array.Reverse(tableau);
        }

        public void DemoTri00(int[] pTableau)
        {
            Console.WriteLine("tri ascendant");
            DelegateTri tri = TriAscendant;
            tri(pTableau);
            foreach (int i in pTableau)
            {
                Console.WriteLine(i);
            }
            Console.WriteLine();

            Console.WriteLine("tri descendant");
            tri = TriDescendant;
            tri(pTableau);
            foreach (int i in pTableau)
            {
                Console.WriteLine(i);
            }
            Console.WriteLine();
        }

        private void TrierEtAfficher(int[] pTableau, DelegateTri pMethodeDeTri)
        {
            pMethodeDeTri(pTableau);
            foreach (int i in pTableau)
            {
                Console.WriteLine(i);
            }
        }

        /// <summary>
        /// Exemple de la factorisation du code de DemoTri00
        /// grace à l'utilisation d'une fonction TrierEtAfficher qui utilise un delegué en paramètre.
        /// </summary>
        /// <param name="pTableau"></param>
        public void DemoTri01(int[] pTableau)
        {
            Console.WriteLine("tri ascendant");
            TrierEtAfficher(pTableau, TriAscendant);
            Console.WriteLine();
            Console.WriteLine("tri descendant");
            TrierEtAfficher(pTableau, TriDescendant);
        }

        /// <summary>
        /// Exemple de la définition des fonctions de tri au moment de l'appel de la méthode
        /// </summary>
        /// <param name="pTableau"></param>
        public void DemoTri02(int[] pTableau)
        {
            Console.WriteLine("tri ascendant");
            TrierEtAfficher(pTableau,
                delegate (int[] leTableau)
                {
                    Array.Sort(leTableau);
                });
            Console.WriteLine();
            Console.WriteLine("tri descendant");
            TrierEtAfficher(pTableau,
                delegate (int[] leTableau)
                {
                    Array.Sort(leTableau);
                    Array.Reverse(leTableau);
                });
        }

        // Le delégué peut être multicast, c'est à dire pointer vers plusieurs méthodes.
        private void TriAscendantEtAffiche(int[] pTableau)
        {
            Array.Sort(pTableau);
            foreach (int i in pTableau)
            {
                Console.WriteLine(i);
            }
        }

        private void TriDescendantEtAffiche(int[] pTableau)
        {
            Array.Sort(pTableau);
            Array.Reverse(pTableau);
            foreach (int i in pTableau)
            {
                Console.WriteLine(i);
            }
        }

        public void DemoTri03(int[] pTableau)
        {
            DelegateTri tri = TriAscendantEtAffiche;
            tri += TriDescendantEtAffiche;
            tri(pTableau);
        }

        /// <summary>
        /// Illustre la cas DemoTri03 avec des dfinitions des méthodes intégrées
        /// </summary>
        /// <param name="pTableau"></param>
        public void DemoTri04(int[] pTableau)
        {
            DelegateTri tri = delegate (int[] mTableau)
            {
                Array.Sort(mTableau);
                foreach (int i in mTableau)
                {
                    Console.WriteLine(i);
                }
            };
            tri += delegate (int[] mTableau)
            {
                Array.Sort(pTableau);
                Array.Reverse(pTableau);
                foreach (int i in pTableau)
                {
                    Console.WriteLine(i);
                }
            };
            tri(pTableau);
        }

        // Utilisation de Action ( avant il y avait DelegateTri à la place de Action<int[]>)
        private void TrierEtAfficherAction(int[] pTableau, Action<int[]> pMethodeDeTri)
        {
            pMethodeDeTri(pTableau);
            foreach (int i in pTableau)
            {
                Console.WriteLine(i);
            }
        }

        /// <summary>
        /// Exemple de la définition des fonctions de tri au moment de l'appel de la méthode
        /// </summary>
        /// <param name="pTableau"></param>
        public void DemoTri05(int[] pTableau)
        {
            Console.WriteLine("tri ascendant");
            TrierEtAfficherAction(pTableau,
                delegate (int[] leTableau)
                {
                    Array.Sort(leTableau);
                });
            Console.WriteLine();
            Console.WriteLine("tri descendant");
            TrierEtAfficherAction(pTableau,
                delegate (int[] leTableau)
                {
                    Array.Sort(leTableau);
                    Array.Reverse(leTableau);
                });
        }

        // Utilisation de fonction lambda
        public void DemoTri06(int[] pTableau)
        {
            Console.WriteLine("tri ascendant");
            TrierEtAfficherAction(pTableau,
                (leTableau) =>
                {
                    Array.Sort(leTableau);
                });
            Console.WriteLine();
            Console.WriteLine("tri descendant");
            TrierEtAfficherAction(pTableau,
                (leTableau) =>
                {
                    Array.Sort(leTableau);
                    Array.Reverse(leTableau);
                });
        }


    }
}
