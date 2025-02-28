using System;

class Program
{
    static void Main()
    {
        // Maximale Anzahl der Schüler
        int maxStudents = 5;
        string[] students = new string[maxStudents];
        int count = 0;

        while (true)
        {
            Console.WriteLine("Menü:");
            Console.WriteLine("1 - Schüler hinzufügen");
            Console.WriteLine("2 - Schüler anzeigen");
            Console.WriteLine("3 - Schüler alphabetisch sortieren");
            Console.WriteLine("4 - Beenden");
            Console.Write("Auswahl: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    if (count < maxStudents)
                    {
                        Console.Write("Gib den Namen des Schülers ein: ");
                        students[count] = Console.ReadLine();
                        count++;
                        Console.WriteLine("Schüler wurde hinzugefügt!");
                    }
                    else
                    {
                        Console.WriteLine("Die Liste ist voll!");
                    }
                    break;

                case "2":
                    Console.WriteLine("Liste der Schüler:");
                    for (int i = 0; i < count; i++)
                    {
                        Console.WriteLine((i + 1) + ". " + students[i]);
                    }
                    break;

                case "3":
                    Array.Sort(students, 0, count);
                    Console.WriteLine("Schüler wurden alphabetisch sortiert!");
                    break;

                case "4":
                    Console.WriteLine("Programm wird beendet...");
                    return;

                default:
                    Console.WriteLine("Ungültige Eingabe! Bitte wähle eine Option von 1-4.");
                    break;
            }
        }
    }
}

