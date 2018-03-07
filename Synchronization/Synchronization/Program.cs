using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Synchronization
{
    #region FirstVersion
    class FirstVersion
    {
        private List<int> S = new List<int>();
        private List<int> R = new List<int>();

        private Thread A, B, C, D;

        public FirstVersion()
        {
            A = new Thread(AddNumbers);
            B = new Thread(AddSqure);
            C = new Thread(AddDivision);
            D = new Thread(ShowElement);
        }

        public void Start()
        {
            A.Start();
            B.Start();
            C.Start();
            D.Start();

            A.Join();
            B.Join();
            C.Join();
            D.Join();
        }

        private void AddNumbers()
        {
            for (int i = 0; i < 10; i++)
            {
                S.Add(i);
            }
        }

        private void AddSqure()
        {
            while (S.Count <= 10)
            {
                if (S.Count == 0)
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("Лист S пуст(квадрат)");
                }
                else
                {
                    R.Add((int)Math.Pow(S[S.Count - 1], 2));
                    if (S.Count == 10)
                        break;
                }
            }
        }

        private void AddDivision()
        {
            while (S.Count <= 10)
            {
                if (S.Count == 0)
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("Лист S пуст(деление)");
                }
                else
                {
                    R.Add(S[S.Count - 1] / 3);
                    if (S.Count == 10)
                        break;
                }
            }
        }

        private void ShowElement()
        {
            while (S.Count <= 10)
            {
                if (R.Count == 0)
                {
                    Console.WriteLine("Список R пуст");
                }
                else
                {
                    Console.WriteLine(R[R.Count - 1]);
                    if (S.Count == 10)
                        break;
                }
            }
        }

    }
    #endregion

    #region SecondThridVersion
    class SecondThridVersion
    {
        private static readonly object lockerS = new object();
        private static readonly object lockerR = new object();

        private List<int> S = new List<int>();
        private List<int> R = new List<int>();

        private Thread A, B, C, D;

        public SecondThridVersion()
        {
            A = new Thread(AddNumbers);
            B = new Thread(AddSqure);
            C = new Thread(AddDivision);
            D = new Thread(ShowElement);
        }

        public void Start()
        {
            A.Start();
            B.Start();
            C.Start();
            D.Start();

            A.Join();
            B.Join();
            C.Join();
            D.Join();
        }

        private void AddNumbers()
        {
           
             for (int i = 0; i < 10; i++)
             {
                lock(lockerS)
                    S.Add(i);
             }
            
        }

        private void AddSqure()
        {
            while (S.Count <= 10)
            {
                lock (lockerS)
                {
                    if (S.Count == 0)
                    {
                        Thread.Sleep(1000);
                        Console.WriteLine("Лист S пуст(квадрат)");
                    }
                    else
                    {
                        lock (lockerR)
                            R.Add((int)Math.Pow(S[S.Count - 1], 2));
                        if (S.Count == 10)
                            break;
                    }
                }
            }
        }

        private void AddDivision()
        {
            while (S.Count <= 10)
            {
                lock (lockerS)
                {
                    if (S.Count == 0)
                    {
                        Thread.Sleep(1000);
                        Console.WriteLine("Лист S пуст(деление)");
                    }
                    else
                    {
                        lock (lockerR)
                        {
                            R.Add(S[S.Count - 1] / 3);
                        }
                        if (S.Count == 10)
                            break;
                    }
                }
            }
        }

        private void ShowElement()
        {
            while (S.Count <= 10)
            {
                lock (lockerR)
                {
                    if (R.Count == 0)
                    {
                        Console.WriteLine("Список R пуст");
                    }
                    else
                    {
                        Console.WriteLine(R[R.Count - 1]);
                    }
                    if (S.Count == 10)
                        break;
                }
            }
        }
    }
#endregion 

    class Program
    {
        static void Main(string[] args)
        {
            //FirstVersion first = new FirstVersion();
           // first.Start();

            SecondVersion s = new SecondVersion();
            s.Start();
            Console.WriteLine("Main Thread finish");

            Console.ReadLine();
        }
    }
}
