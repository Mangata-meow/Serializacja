using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;

namespace Serializacja
{
    public class Osoba
    {
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public int Wiek { get; set; }
        public DateTime Dataurodzenia { get; set; }
    }

    public class Student : Osoba
    {
        public string NumerIndeksu { get; set; }
        public string NumerGrupy { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var osoby = new List<Osoba>
            {
                new Osoba { Imie = "Julia", Nazwisko = "Svensson", Wiek = 21, Dataurodzenia = new DateTime(2004, 10, 26)},
                new Osoba { Imie = "Albin", Nazwisko = "Svensson", Wiek = 30, Dataurodzenia = new DateTime(1995, 8, 14)},
                new Osoba { Imie = "Bono", Nazwisko = "Svensson", Wiek = 4, Dataurodzenia = new DateTime(2021, 5, 15)},
                new Osoba { Imie = "Dan", Nazwisko = "Reynolds", Wiek = 38, Dataurodzenia = new DateTime(1987, 7, 14)},
                new Student { Imie = "Post", Nazwisko = "Malone", Wiek = 30, Dataurodzenia = new DateTime(1995, 7, 4), NumerIndeksu = "a12345", NumerGrupy = "K2"},
                new Student { Imie = "Till", Nazwisko = "Lindemann", Wiek = 62, Dataurodzenia = new DateTime(1963, 1, 4), NumerIndeksu = "b12345", NumerGrupy = "A7"},
                new Student { Imie = "Jere", Nazwisko = "Pöyhönen", Wiek = 32, Dataurodzenia = new DateTime(1993, 10, 21), NumerIndeksu = "c12345", NumerGrupy = "Z6"},
                new Student { Imie = "Filip", Nazwisko = "Szcześniak", Wiek = 35, Dataurodzenia = new DateTime(1990, 7, 29), NumerIndeksu = "d12345", NumerGrupy = "V8"}
            };

            SerializujDoXML(osoby, "osoby.xml");
            Console.WriteLine("Obiekty zostały zserializowane do pliku 'osoby.xml'.");

            SerializujDoJSON(osoby, "osoby.json");
            Console.WriteLine("Obiekty zostały zserializowane do pliku 'osoby.json'.");

            Console.WriteLine("\nWczytane dane z pliku XML:");
            var osobyZXml = DeserializujZXML("osoby.xml");
            WyswietlOsoby(osobyZXml);

            Console.WriteLine("\nWczytane dane z pliku JSON:");
            var osobyZJson = DeserializujZJSON("osoby.json");
            WyswietlOsoby(osobyZJson);

            Console.WriteLine("\nNaciśnij Enter, aby zakończyć...");
            Console.ReadLine();
        }

        static void SerializujDoXML(List<Osoba> osoby, string plik)
        {
            var serializer = new XmlSerializer(typeof(List<Osoba>), new Type[] { typeof(Student) });
            using (var writer = new StreamWriter(plik))
            {
                serializer.Serialize(writer, osoby);
            }
        }

        static void SerializujDoJSON(List<Osoba> osoby, string plik)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var jsonString = JsonSerializer.Serialize(osoby, options);
            File.WriteAllText(plik, jsonString);
        }

        static List<Osoba> DeserializujZXML(string plik)
        {
            var serializer = new XmlSerializer(typeof(List<Osoba>), new Type[] { typeof(Student) });
            using (var reader = new StreamReader(plik))
            {
                return (List<Osoba>)serializer.Deserialize(reader);
            }
        }

        static List<Osoba> DeserializujZJSON(string plik)
        {
            var jsonString = File.ReadAllText(plik);
            return JsonSerializer.Deserialize<List<Osoba>>(jsonString);
        }

        static void WyswietlOsoby(List<Osoba> osoby)
        {
            foreach (var osoba in osoby)
            {
                Console.WriteLine($"{osoba.Imie} {osoba.Nazwisko}, Wiek: {osoba.Wiek}, Data Urodzenia: {osoba.Dataurodzenia.ToShortDateString()}");
                if (osoba is Student student)
                {
                    Console.WriteLine($"Numer Indeksu: {student.NumerIndeksu}, Numer Grupy: {student.NumerGrupy}");
                }
                Console.WriteLine();
            }
        }
    }
}
