using System;
using System.Collections;
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

            //Test02(); // Lambda expression

            //Test03(); // les événements

            Test04(); // les evenement bis

            Console.ReadKey();
        }

        // BEGIN Test04 -------------------------------------------------------------------------
        private static void Test04()
        {
            new DemoEvenementBis().Demo();
        }

        public class VoitureBis
        {
            /*
            // définissons un délégué qui ne retourne rien et qui prend en paramètre un décimal.
            public delegate void DelegateDeChangementDePrix(decimal nouveauPrix);

            // Nous définissons ensuite un événement basé sur ce délégué
            public event DelegateDeChangementDePrix ChangementDePrix;
            */

            // Rafinons l'exemple en utilisant EventHandler dans notre classe
            public event EventHandler<ChangementDePrixEventsArgs> ChangementDePrix;

            public decimal Prix { get; set; }

            public void PromoSurLePrix()
            {
                Prix = Prix / 2;
                //  nous notifions les éventuels objets qui se seraient abonnés à cet événement 
                // en invoquant l’événement et en lui fournissant en paramètre le nouveau prix.
                if (ChangementDePrix != null) //  testons d’abord s’il y a un abonné à l’événement
                {
                    /*ChangementDePrix(Prix);*/
                    ChangementDePrix(this, new ChangementDePrixEventsArgs { Prix = Prix });
                }
            }
        }

        public class DemoEvenementBis
        {
            public DemoEvenementBis()
            {
            }

            public void Demo()
            {
                VoitureBis voiture = new VoitureBis { Prix = 10000 };
                /*
                voiture.ChangementDePrix += Voiture_ChangementDePrix;
                voiture.ChangementDePrix += Voiture_ChangementDePrix1;
                */
                voiture.ChangementDePrix += Voiture_ChangementDePrix2;
                voiture.PromoSurLePrix();
            }

            private void Voiture_ChangementDePrix2(object sender, ChangementDePrixEventsArgs e)
            {
                Console.WriteLine($"Le nouveau prix est de {e.Prix}");
            }

            private void Voiture_ChangementDePrix1(decimal nouveauPrix)
            {
                Console.WriteLine($"Dans VoitureBis Voiture_ChangementDePrix1 : on vien de changer le prix à : {nouveauPrix}");
            }

            private void Voiture_ChangementDePrix(decimal nouveauPrix)
            {
                Console.WriteLine($"Dans VoitureBis on vien de changer le prix à : {nouveauPrix}");
            }
        }

        public class ChangementDePrixEventsArgs : EventArgs
        {
            public decimal Prix { get; set; }
        }
        // END test04 ------------------------------------------------------------------------------
        private static void Test03()
        {
            // Les événements sont un mécanisme du C# permettant à une classe d'être notifiée d'un changement.
            // La base des événements est le délégué. On pourra stocker dans un événement un ou plusieurs délégués
            // qui pointent vers des méthodes respectant la signature de l'événement.
            // Un événement est défini grâce au mot clef event:

            new DemoEvenement().Demo();
        }

        public class DemoEvenement
        {
            public DemoEvenement()
            {
            }

            public void Demo()
            {
                // Etape 1 : créer une voiture
                Voiture voiture = new Voiture { Prix = 10000 };

                // Etape 2 : créons un délégué du même type que l’événement.
                Voiture.DelegateDeChangementDePrix delegateDeChangementDePrix = voiture_ChangementDePrix;

                // Etape 3 : faisons pointer vers une méthode qui respecte la signature du délégué
                voiture.ChangementDePrix += delegateDeChangementDePrix;

                voiture.PromoSurLePrix();
            }

            private void voiture_ChangementDePrix(decimal nouveauPrix)
            {
                Console.WriteLine("Le nouveau prix est de : " + nouveauPrix);
            }
        }

        public class Voiture
        {
            public delegate void DelegateDeChangementDePrix(decimal nouveauPrix);
            public event DelegateDeChangementDePrix ChangementDePrix;
            public decimal Prix { get; set; }

            public void PromoSurLePrix()
            {
                Prix = Prix / 2;
                if (ChangementDePrix != null)
                {
                    ChangementDePrix(Prix);
                }
            }
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

            Func<double, double, double> division = (x, y) => x / y;
            double result = division(8, 2);
            Console.WriteLine($"Résultat de la division de 8 par 2 : {result} ");

            int[] nombres = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            // Calcule le nombre d'éléments pairs dans la liste
            int pairs = nombres.Count(n => n % 2 == 0);
            Console.WriteLine($"Il y a  {pairs} éléments pairs dans la liste");

            var moyenne = nombres.Average(n => n);
            Console.WriteLine($"Dans la liste, la moyenne est  {moyenne}.");

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
            tri += TriDescendantEtAffiche; // L'opérateur += ajoute la méthode  "TriDescendantEtAffiche" au délégué "tri"
            tri(pTableau);
        }

        /// <summary>
        /// Illustre la cas DemoTri03 avec des définitions des méthodes intégrées
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
