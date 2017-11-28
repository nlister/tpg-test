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
                Console.WriteLine(item.Name);
            }
        }

        private static List<Package> GatherDependencies(string[] dependencies)
        {
            var retVal = new List<Package>();

            for (int i = 0; i < dependencies.Length; i++)
            {
                var item = new Tuple<string, string>(dependencies[i].Split(':')[0], dependencies[i].Split(':')[1].TrimStart(' '));
                retVal.Add(item.Item2.Length > 0 ? new Package(item.Item1, new Package(item.Item2)) : new Package(item.Item1));
            }

            return retVal.OrderBy(x => x.Dependencies.Count()).ToList();
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

        public class Package
        {
            public string Name { get; private set; }
            public Package[] Dependencies { get; private set; }

            public Package(string name, params Package[] dependencies)
            {
                Name = name;
                Dependencies = dependencies;
            }
        }


    }
}
