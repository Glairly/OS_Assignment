using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace Problem01
{
    class Program
    {
        static byte[] Data_Global = new byte[1000000000];
        static long[] Sum_Global = new long[4];
        static int G_index = 0;
        static Thread t1,t2,t3,t4;
        // 30667ms

        static int ReadData()
        {
            int returnData = 0;
            FileStream fs = new FileStream("Problem01.dat", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();

            try 
            {
                Data_Global = (byte[]) bf.Deserialize(fs);
            }
            catch (SerializationException se)
            {
                Console.WriteLine("Read Failed:" + se.Message);
                returnData = 1;
            }
            finally
            {
                fs.Close();
            }

            return returnData;
        }
        static void sum(ref long psum, int idx)
        {
            // time reduced by changed data ref method
            int val = Data_Global[idx];
            if (val % 2 == 0)
            {
                psum -= val;
            }
            else if (val % 3 == 0)
            {
                psum += (val * 2);
            }
            else if (val % 5 == 0)
            {
                psum += (val / 2);
            }
            else if (val % 7 == 0)
            {
                psum += (val / 3);
            }
            Data_Global[idx] = 0;
        }

        static void task(ref long psum,int start,int stop){
                for(int i = start;i < stop;++i)
                {
                     sum(ref psum,i);
                }
        }


        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            int y;
            const long correct = 888701676;
            const int  approxDefaultRuntime = 30000;

            /* Read data from file */
            Console.Write("Data read...");
            y = ReadData();
            if (y == 0)
            {
                Console.WriteLine("Complete.");
            }
            else
            {
                Console.WriteLine("Read Failed!");
            }

            var cores = 4;
            var divider = 1000000000/cores;

            // var t_array = new Thread[cores];

            t1 = new Thread(() => {
                task(ref Sum_Global[0],0,divider);
            });

            t2 = new Thread(() => {
                task(ref Sum_Global[1],divider,divider * 2);
            });

            t3 = new Thread(() => {
                task(ref Sum_Global[2],divider * 2 ,divider * 3);
            });

            t4 = new Thread(() => {
                task(ref Sum_Global[3],divider * 3 ,divider * 4);
            });
         

            /* Start */
            Console.Write("\n\nWorking...");
            sw.Start();

          
            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();

            t1.Join();
            t2.Join();
            t3.Join();
            t4.Join();


            sw.Stop();
            Console.WriteLine("Done.");

            /* Result */
            long s = 0;
            Array.ForEach(Sum_Global, i => s += i);
            Console.WriteLine("Summation result: {0}", s);
            Console.WriteLine("Time used: " + sw.ElapsedMilliseconds.ToString() + "ms");
            Console.WriteLine("Time Reduced: {0}%", (1 - (sw.ElapsedMilliseconds * 1.0)/(approxDefaultRuntime * 1.0)).ToString("N2") );
            Console.WriteLine("Summation Correctness: {0}", s == correct);
        }
    }
}
