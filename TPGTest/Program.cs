using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickGraph.Algorithms;
using QuickGraph.Collections;

namespace TPGTest
{
    public class Program
    {
        static void Main(string[] args)
        {
            //string[] args = { "FileProcessor: ProcessingLibrary", "ProcessingLibrary: FileProcessor" };

            InstallPackages(args);
        }

        public static void InstallPackages(string[] packagesToInstall)
        {
            var output = GatherDependencies(packagesToInstall);

            foreach (var l in OrderDependencies<Package>(output, x => x.Dependencies))
            {
                Console.WriteLine(l.Name);
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

        public static List<Package> OrderDependencies<T>(List<Package> source, Func<Package, IEnumerable<Package>> getDependencies)
        {
            var comparer = new PackageComparer();
            var visiteds = new Dictionary<Package, bool>(comparer);
            var sorted = new List<Package>(source.Count);
            Action<Package> visit = null;

            visit = item => {
                if (visiteds.TryGetValue(item, out bool isMarked))
                {
                    if (isMarked && !(source.Any(x => x.Dependencies.Contains(item, comparer) && item.Dependencies.Contains(x, comparer)))) return;
                    throw new ArgumentException("Dependency specification is invalid, it contains cycles.");
                }
                var dependencies = getDependencies(item);
                if (dependencies != null)
                {
                    visiteds[item] = false;
                    foreach (var itm in dependencies) visit(itm);
                }
                visiteds[item] = true;
                sorted.Add(item);
            };
            foreach (var item in source) visit(item);
            return sorted;
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

        public class PackageComparer : EqualityComparer<Package>
        {
            public override bool Equals(Package x, Package y)
            {
                return (x == null && y == null) || (x != null && y != null && x.Name == y.Name);
            }

            public override int GetHashCode(Package obj)
            {
                return obj == null ? 0 : obj.Name.GetHashCode();
            }
        }


    }
}
