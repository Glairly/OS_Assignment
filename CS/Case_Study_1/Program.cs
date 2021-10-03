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
        static long Sum_Global = 0;
        static int G_index = 0;
        static Thread t1,t2,t3,t4;

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
        static void sum()
        {
            int val = Data_Global[G_index];
            int o = 0;
            if (val % 2 == 0)
            {
                o -= val;
            }
            else if (val % 3 == 0)
            {
                o += (val*2);
            }
            else if (val % 5 == 0)
            {
                o += (val / 2);
            }
            else if (val %7 == 0)
            {
                o += (val / 3);
            }
            Data_Global[G_index] = 0;
            Sum_Global += o;
            // Interlocked.Add(ref Sum_Global,o);
            // Interlocked.Increment(ref G_index);  
            G_index++; 
        }

        static void task(){
            for (int i = 0; i < 1000000000/2; i++)
                sum();
        }

        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            int i, y;

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

            t1 = new Thread(task);
            t2 = new Thread(task);
            /* Start */
            Console.Write("\n\nWorking...");
            sw.Start();


            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            sw.Stop();
            Console.WriteLine("Done.");

            /* Result */
            Console.WriteLine("Summation result: {0}", Sum_Global);
            Console.WriteLine("Time used: " + sw.ElapsedMilliseconds.ToString() + "ms");
        }
    }
}
