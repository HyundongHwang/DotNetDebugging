using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySimpleConApp
{
    class Program
    {
        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += _AppDomain_UnhandledException;

            var p = new MyPerson();
            p.id = 101;
            p.name = "william";

            MySingleton.Current.id = 12345;
            MySingleton.Current.name = "william";
            MySingleton.Current.friends = new List<string> { "brandon", "kevin" };

            Console.WriteLine($@"
commands
----------------
q : exit program
d : create dump
e : create exception

enter command : 
            ");

            while (true)
            {
                var key = Console.ReadKey();

                if (key.Key == ConsoleKey.D)
                {
                    Console.WriteLine("create dump...");
                    MiniDumpWriter.Write();
                }
                else if (key.Key == ConsoleKey.Q)
                {
                    Console.WriteLine("exit program...");
                    break;
                }
                else if (key.Key == ConsoleKey.E)
                {
                    Console.WriteLine("create exception...");
                    throw new Exception("my exception is occured!!!");
                    break;
                }
            }
        }

        private static void _AppDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine($@"e : {e}");
            Console.WriteLine($@"e.ExceptionObject : {e.ExceptionObject}");
        }
    }

    public class MyPerson
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class MySingleton
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<string> friends { get; set; }


        #region 싱글톤
        static MySingleton _Current;
        static public MySingleton Current
        {
            get
            {
                if (_Current == null)
                {
                    _Current = new MySingleton();
                }
                return _Current;
            }
        }
        MySingleton()
        {
        }
        #endregion
    }
}
