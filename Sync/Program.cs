using System;
using System.Diagnostics;
using System.Threading;

namespace Sync
{
    
    class Program
    {
        const int iterlation_size = 1000001;

        static void Main(string[] args)
        {
            //  Ex00.run();
            //  Ex01.run();
            //  Ex02.run();
            //  Ex03.run();
            // Ex04.run();
            Ex05.run();
        }

        public class Ex00{

            private static int sum = 0;

            static void plus(){
                int i;
                for(i = 1;i < iterlation_size; ++i){
                    sum += i;
                }
            }

            static void minus(){
                int i;
                for(i = 1;i < iterlation_size; ++i){
                    sum -= i;
                }
            }

            public static void run(){
                Stopwatch sw = new Stopwatch();
                Console.WriteLine("Start....");
                sw.Start();
                plus();
                minus();
                sw.Stop();
                Console.WriteLine("sum = {0}",sum);
                Console.WriteLine("Time used: " + sw.ElapsedMilliseconds.ToString() + "ms");
            }

        }

        public class Ex01{

            private static int sum = 0;

            static void plus(){
                int i;
                for(i = 1;i < iterlation_size; ++i){
                    sum += i;
                }
            }

            static void minus(){
                int i;
                for(i = 1;i < iterlation_size; ++i){
                    sum -= i;
                }
            }

            public static void run(){

                Thread P = new Thread(new ThreadStart(plus));
                Thread M = new Thread(new ThreadStart(minus));


                Stopwatch sw = new Stopwatch();
                Console.WriteLine("Start....");
                sw.Start();

                P.Start();
                M.Start();

                P.Join();
                M.Join();

                sw.Stop();
                Console.WriteLine("sum = {0}",sum);
                Console.WriteLine("Time used: " + sw.ElapsedMilliseconds.ToString() + "ms");
            }

        }

        public class Ex02{

            private static int sum = 0;
            private static object _Lock = new object();

            static void plus(){
                int i;
                for(i = 1;i < iterlation_size; ++i){
                    lock(_Lock)
                    sum += i;
                
                }
            }

            static void minus(){
                int i;
                for(i = 1;i < iterlation_size; ++i){
                    lock(_Lock)
                    sum -= i;
                }
            }

            public static void run(){

                Thread P = new Thread(new ThreadStart(plus));
                Thread M = new Thread(new ThreadStart(minus));


                Stopwatch sw = new Stopwatch();
                Console.WriteLine("Start....");
                sw.Start();

                P.Start();
                M.Start();

                P.Join();
                M.Join();

                sw.Stop();
                Console.WriteLine("sum = {0}",sum);
                Console.WriteLine("Time used: " + sw.ElapsedMilliseconds.ToString() + "ms");
            }

        }

        public class Ex03{

            private static int sum = 0;
            private static object _Lock = new object();

            static void plus(){
                int i;
                lock(_Lock)
                for(i = 1;i < iterlation_size; ++i){
                    
                    sum += i;
                
                }
            }

            static void minus(){
                int i;
                lock(_Lock)
                for(i = 1;i < iterlation_size; ++i){
                    sum -= i;
                }
            }

            public static void run(){

                Thread P = new Thread(new ThreadStart(plus));
                Thread M = new Thread(new ThreadStart(minus));


                Stopwatch sw = new Stopwatch();
                Console.WriteLine("Start....");
                sw.Start();

                P.Start();
                M.Start();

                P.Join();
                M.Join();

                sw.Stop();
                Console.WriteLine("sum = {0}",sum);
                Console.WriteLine("Time used: " + sw.ElapsedMilliseconds.ToString() + "ms");
            }

        }
    
    
        public class Ex04 {
            private static string x = "";
            private static int exitflag = 0;
            private static object _Lock = new object();

            static void ThReadX(){
                lock(_Lock){
                    while(exitflag == 0){
                        Monitor.Wait(_Lock);
                        if(exitflag == 1) break;
                        Console.WriteLine("X = {0}",x);
                    }
                    Console.WriteLine("Thread 1 exit");
                }
            }

            static void  ThWriteX(){
                string xx;
                while(exitflag == 0){
                    lock(_Lock){
                        Monitor.Pulse(_Lock);
                        Console.Write("Input: ");
                        xx = Console.ReadLine();
                        if(xx == "exit")
                            exitflag = 1;
                        else
                            x = xx;
                    }
                }
            }

            public static void run(){
                Thread A = new Thread(ThReadX);
                Thread B = new Thread(ThWriteX);

                A.Start();
                B.Start();
            }
        }
    
        public class Ex05 {
            private static string x = "";
            private static int exitflag = 0;
            private static object _Lock = new object();

            static void ThReadX(object i){
                lock(_Lock){
                    while(exitflag == 0){
                        Monitor.Wait(_Lock);
                        if(exitflag == 1) break;
                        Console.WriteLine("**Thraed {0} : X = {1}**",i,x);
                    }
                    Console.WriteLine("---Thread {0} exit---",i);
                }
            }

            static void  ThWriteX(){
                string xx;
                while(exitflag == 0){
                    lock(_Lock){
                        Monitor.Pulse(_Lock);
                        Console.Write("Input: ");
                        xx = Console.ReadLine();
                        if(xx == "exit"){
                            exitflag = 1;
                            Monitor.PulseAll(_Lock);
                        }
                        else
                            x = xx;
                    }
                }
            }

            public static void run(){
                Thread A = new Thread(ThWriteX);
                Thread B = new Thread(ThReadX);
                Thread C = new Thread(ThReadX);
                Thread D = new Thread(ThReadX);

                A.Start();
                B.Start(1);
                C.Start(2);
                D.Start(3);
            }
        }
    }
 
}
