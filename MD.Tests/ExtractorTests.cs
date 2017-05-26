using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MD.Tests
{
    [TestClass]
    public class ExtractorTests
    {
        private string accountSvgPath;
        private Extractor.Utility.Extractor extractor;

        [TestInitialize]
        public void TestInitialise()
        {
            accountSvgPath = Path.GetFullPath(@"..\..\..\MD.Extractor\Raw SVG\account.svg");
            extractor = new Extractor.Utility.Extractor();

            Assert.IsTrue(File.Exists(accountSvgPath));
        }

        [TestMethod]
        public void CanSvgBeExtracted()
        {
            var result = extractor.GetVectorFromSvg(accountSvgPath);

            var desiredData =
                "M 12,4C 14.2091,4 16,5.79086 16,8C 16,10.2091 14.2091,12 12,12C 9.79086,12 8,10.2091 8,8C 8,5.79086 9.79086,4 12,4 Z M 12,14C 16.4183,14 20,15.7909 20,18L 20,20L 4,20L 4,18C 4,15.7909 7.58172,14 12,14 Z ";

            Assert.AreEqual(desiredData, result);
        }

        [TestMethod]
        public void IsGeometryBuiltCorrectly()
        {
            var result = extractor.GetGeometryFromSvg("account",
                "M 12,4C 14.2091,4 16,5.79086 16,8C 16,10.2091 14.2091,12 12,12C 9.79086,12 8,10.2091 8,8C 8,5.79086 9.79086,4 12,4 Z M 12,14C 16.4183,14 20,15.7909 20,18L 20,20L 4,20L 4,18C 4,15.7909 7.58172,14 12,14 Z ");
            var desiredData =
                "<Geometry x:Key=\"AccountPath\">M 12,4C 14.2091,4 16,5.79086 16,8C 16,10.2091 14.2091,12 12,12C 9.79086,12 8,10.2091 8,8C 8,5.79086 9.79086,4 12,4 Z M 12,14C 16.4183,14 20,15.7909 20,18L 20,20L 4,20L 4,18C 4,15.7909 7.58172,14 12,14 Z </Geometry>";

            Assert.AreEqual(desiredData, result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void OnlyWorkOnSvgFiles()
        {
            var accountSvgPath = Path.GetFullPath(@"C:\Blahblahblah.txt");

            var extractor = new Extractor.Utility.Extractor();

            var result = extractor.GetVectorFromSvg(accountSvgPath);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void MustSupplyConcreteFile()
        {
            var accountSvgPath = Path.GetFullPath(@"C:\Blahblahblah.svg");

            var extractor = new Extractor.Utility.Extractor();

            var result = extractor.GetVectorFromSvg(accountSvgPath);
        }
    }
}