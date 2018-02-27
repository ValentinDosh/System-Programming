using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessUser
{
    class Program
    {
        static void Main(string[] args)
        {
            ProccesUser process = new ProccesUser();
            menu();
            int result = Convert.ToInt32(Console.ReadLine());

            while (result != 0)
            {
                switch (result)
                {
                    case 1:
                        Console.Clear();
                        menu();
                        process.DisplayProcceces();    
                        break;
                    case 2:
                        Console.Clear();
                        menu();
                        Console.Write("Введите Id: ");
                        int id = Convert.ToInt32(Console.ReadLine());
                        process.IdProcess(id);
                        break;
                    case 3:
                        Console.Clear();
                        menu();
                        Console.Write("Введите путь к файлу: ");
                        string path = Console.ReadLine();
                        process.ProcessStart(path);
                        break;
                    case 4:
                        Console.Clear();
                        menu();
                        Console.Write("Введите Id: ");
                        int id2 = Convert.ToInt32(Console.ReadLine());
                        process.Stop(id2);
                        break;
                    case 5:
                        Console.Clear();
                        menu();
                        process.DisplayThreads();
                        break;
                    case 6:
                        Console.Clear();
                        menu();
                        process.DisplayModules();
                        break;
                    default:
                        break;
                }
                result = Convert.ToInt32(Console.ReadLine());
            }
        }

        static void menu()
        {
            Console.WriteLine("Выберите операцию:\n");
            Console.WriteLine("1 - Список всех процессов");
            Console.WriteLine("2 - Выбор процесса по PID");
            Console.WriteLine("3 - Запустить процесс");
            Console.WriteLine("4 - Остановить процесс");
            Console.WriteLine("5 - Информация о потоках выбранного процесса");
            Console.WriteLine("6 - Информация о модулях выбранного процесса");
            Console.WriteLine("0 - Выход");
        }
    }
}
