using System;
using System.Collections.Generic;
using System.IO;

namespace MD.Extractor
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Generating Path file");

            var di = new DirectoryInfo(@"..\..\Raw SVG");
            var extractor = new Utility.Extractor();

            var processFailures = new List<string>();

            var validGeometries = new List<string>();
            var validImage = new List<string>();

            validGeometries.Add("<ResourceDictionary xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"\r\n                    xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\">");
            validImage.Add("<ResourceDictionary xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"\r\n                    xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\">\r\n    <ResourceDictionary.MergedDictionaries>\r\n        <ResourceDictionary Source=\"PathResourceDictionary.xaml\" />\r\n    </ResourceDictionary.MergedDictionaries>");

            foreach (var fileInfo in di.GetFiles())
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileInfo.Name);

                var vector = extractor.GetVectorFromSvg(fileInfo.FullName);
                if (string.IsNullOrEmpty(vector) == false)
                {
                    var pathData = extractor.GetGeometryFromVector(fileNameWithoutExtension, vector);
                    validGeometries.Add("\t" + pathData);

                    var geometry =
                        $"     <DrawingImage x:Key=\"{extractor.GetCamelCaseKey(fileNameWithoutExtension)}Image\">\r\n        <DrawingImage.Drawing>\r\n            <GeometryDrawing Brush=\"{{StaticResource FlatButtonBrush}}\" Geometry=\"{{StaticResource {extractor.GetCamelCaseKey(fileNameWithoutExtension)}Path}}\" />\r\n        </DrawingImage.Drawing>\r\n    </DrawingImage>";
                    validImage.Add(geometry);
                }
                else
                {
                    Console.WriteLine($"Unable to process {fileInfo.Name}");
                    processFailures.Add(fileNameWithoutExtension);
                }
            }

            validGeometries.Add("</ResourceDictionary>");
            validImage.Add("</ResourceDictionary>");

            File.WriteAllLines(@"c:\temp\PathResourceDictionary.xaml", validGeometries);
            File.WriteAllLines(@"c:\temp\ImageResourceDictionary.xaml", validImage);

            Console.WriteLine("Unable to process " + processFailures.Count);
            Console.ReadKey();
        }
    }
}