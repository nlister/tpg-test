using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TPGTest;

namespace TPGProgramTest
{
    [TestClass]
    public class TPGProgramTest
    {
        [TestMethod]
        public void Install_Packages_Valid()
        {
            string[] args = { "SupremeLeaderSnoke: ", "Resistance: LeiaOrgana", "LeiaOrgana: AnakinSkywalker",
            "FirstOrder: SupremeLeaderSnoke", "StarWars: Resistance",  "AnakinSkywalker: "};

            Program.InstallPackages(args);
;        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Dependency specification is invalid, it contains cycles.")]
        public void Install_Packages_Contains_Cycle()
        {
            string[] args = {"SupremeLeaderSnoke: ", "Resistance: LeiaOrgana", "LeiaOrgana: AnakinSkywalker",
            "FirstOrder: SupremeLeaderSnoke", "StarWars: ", "AnakinSkywalker: Resistance", "Resistance: AnakinSkywalker"};

            Program.InstallPackages(args);
        }
    }
}
