using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickGraph.Algorithms;
using QuickGraph.Collections;

namespace TPGTest
{
    class Program
    {
        static void Main()
        {
            string[] args = { "FileProcessor: ProcessingLibrary", "ProcessingLibrary: " };

            var output = GatherDependencies(args);
       

            foreach (var item in output)
            {
                Console.WriteLine(item);
            }
        }

        private static List<string> GatherDependencies(string[] dependencies)
        {
            var result = new List<Tuple<string, string>>();

            for (int i = 0; i < dependencies.Length; i++)
            {
                result.Add(new Tuple<string, string>(dependencies[i].Split(':')[0], dependencies[i].Split(':')[1].TrimStart(' ')));
            }

            return OrderDependencies(result);
        }

        private static List<string> OrderDependencies(List<Tuple<string, string>> dependencies)
        {
            var retVal = new List<string>();

            foreach (var d in dependencies)
            {
                if (d.Item2 == string.Empty || d.Item2.Length == 0)
                {
                    retVal.Add(d.Item1);
                }
            }

            foreach (var d in dependencies)
            {
                if (d.Item2.Length > 0 && !retVal.Contains(d.Item2))
                {
                    retVal.Add(d.Item2);
                }
            }

            foreach (var d in dependencies)
            {
                if (!retVal.Contains(d.Item1))
                    retVal.Add(d.Item1);
            }

            return retVal;
        }


    }
}
