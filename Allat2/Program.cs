using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Allat2
{
    class Allat
    {
        //mezok es tulajdonsagok helyett automatikus tulajdonsagok:
        public string Nev { get; private set; }
        public int SzuletesiEv { get; private set; }
        public int RajtSzam { get; private set; }

        public int Szepsegpont { get; private set; }
        public int ViselkedesPont { get; private set; }

        public static int AktualisEv { get; set; }
        public static int KorHatar { get; set; }

        //konstruktor
        // mivel nincsenek mezok ertekadaskor a tulajdonsag kap erteket
        public Allat(int rajtSzam, string nev, int szuletesiEv)
        {
            this.RajtSzam = rajtSzam;
            this.Nev = nev;
            this.SzuletesiEv = szuletesiEv;
        }

        // metodusok
        public int Kor()
        {
            return AktualisEv - SzuletesiEv;
        }

        // orokolhetove kell tenni ezert atirjuk virtual-ra
        // es itt is a tulajdonsagokra hivatkozunk
        public virtual int Pontszam()
        {
            if (Kor() < KorHatar)
            {
                return ViselkedesPont * Kor() + Szepsegpont * (KorHatar - Kor());
            }
            return 0;
        }

        public void Pontozzak(int szepsegPont, int viselkedesPont)
        {
            this.Szepsegpont = szepsegPont;
            this.ViselkedesPont = viselkedesPont;
        }

        //a ToString() igy majd kisbetusen kiirja az osztalyneveket is.
        public override string ToString()
        {
            return RajtSzam + ". " + Nev + " nevű " +
                this.GetType().Name.ToLower() + " pontszáma: "
                + Pontszam() + " pont";
        }


    }

    class Kutya : Allat
    {
        public int GazdaViszonyPont { get; private set; }
        public bool KapottViszonyPontot { get; private set; }

        public Kutya(int rajtSzam, string nev, int szulEv) : base(rajtSzam, nev, szulEv)
        {

        }

        public void ViszonyPontozas(int gazdaViszonyPont)
        {
            this.GazdaViszonyPont = gazdaViszonyPont;
            KapottViszonyPontot = true;
        }

        public override int Pontszam()
        {
            int pont = 0;
            if (KapottViszonyPontot)
            {
                pont = base.Pontszam() + GazdaViszonyPont;
            }
            return pont;
        }
    }

    class Macska : Allat
    {
        public bool VanMacskaSzallitoDoboz { get; set; }

        public Macska(int rajtSzam, string nev, int szulEv, bool vanMacskaSzallitoDoboz) : base(rajtSzam, nev, szulEv)
        {
            this.VanMacskaSzallitoDoboz = vanMacskaSzallitoDoboz;
        }

        public override int Pontszam()
        {
            if (VanMacskaSzallitoDoboz)
            {
                return base.Pontszam();
            }
            return 0;
        }
    }

    class Vezerles
    {
        private List<Allat> allatok = new List<Allat>();

        public void Start()
        {
            Allat.AktualisEv = 2015;
            Allat.KorHatar = 10;

            Regisztracio();
            Kiiratas("\na regiszált versenyzők\n");
            Verseny();
            Kiiratas("\na verseny eredménye\n");

        }
        /*
        private void Proba()
        {
            Allat allat1, allat2;
            string nev1 = "Pamacs", nev2 = "Bolhazsák";

            int szulEv1 = 2010, szulEv2 = 2011;
            bool vanDoboz = true;
            int rajtSzam = 1;

            int szepsegPont = 5, viselkedesPont = 4, viszonyPont = 6;

            allat1 = new Kutya(rajtSzam, nev1, szulEv1);
            rajtSzam++;

            allat2 = new Macska(rajtSzam, nev2, szulEv2, vanDoboz);

            Console.WriteLine("A regisztrált versenyzők");
            Console.WriteLine(allat1);
            Console.WriteLine(allat2);

                // verseny
            allat1.Pontozzak(szepsegPont, viselkedesPont);
            allat2.Pontozzak(szepsegPont, viselkedesPont);

            if (allat1 is Kutya)
            {
                (allat1 as Kutya).ViszonyPontozas(viszonyPont);
            }

            Console.WriteLine("\nA verseny eredménye");
            Console.WriteLine(allat1);
            Console.WriteLine(allat2);

            if (allat1 is Kutya) 
            {
                ((Kutya)allat1).ViszonyPontozas(viszonyPont);
            }

        }
        */
        private void Regisztracio()
        {
            StreamReader olvasocsatorna = new StreamReader("allatok.txt");

            string fajta, nev;
            int rajtSzam = 1, szulEv;
            bool vanDoboz;

            while (!olvasocsatorna.EndOfStream)
            {
                fajta = olvasocsatorna.ReadLine();
                nev = olvasocsatorna.ReadLine();
                szulEv = int.Parse(olvasocsatorna.ReadLine());

                if (fajta == "kutya")
                {
                    allatok.Add(new Kutya(rajtSzam, nev, szulEv));
                }
                else
                {
                    vanDoboz = bool.Parse(olvasocsatorna.ReadLine());
                    allatok.Add(new Macska(rajtSzam, nev, szulEv, vanDoboz));
                }
                rajtSzam++;
            }
            olvasocsatorna.Close();
        }
        private void Verseny()
        {
            Random rand = new Random();
            int hatar = 11;
            foreach (Allat item in allatok)
            {
                if (item is Kutya)
                {
                    (item as Kutya).ViszonyPontozas(rand.Next(hatar));
                }
                item.Pontozzak(rand.Next(hatar), rand.Next(hatar));
            }
        }
        private void Kiiratas(string sor)
        {

            Console.WriteLine(sor);
            foreach (Allat db in allatok)
            {

                Console.WriteLine(db);
            }
        }

        internal class Program
        {
            static void Main(string[] args)
            {
                new Vezerles().Start();

                Console.ReadKey();
            }
        }
    }
}
