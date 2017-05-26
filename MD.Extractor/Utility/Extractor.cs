using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace MD.Extractor.Utility
{
    public class Extractor
    {
        public string GetVectorFromSvg(string file)
        {
            if (file.EndsWith(".svg") == false)
                throw new InvalidOperationException("Must be an SVG file.");

            if (File.Exists(file) == false)
                throw new FileNotFoundException();


            var doc = XDocument.Load(file);
            var root = doc.Root;
            var path = root?.Descendants("{http://www.w3.org/2000/svg}path");

            if (path == null || !path.Any())
                Debug.WriteLine("SVG doesn't have a path");
            else if (path.Count() > 1)
                Debug.WriteLine("${file} has more than one path.");
            else
                return path.First().Attribute("d")?.Value;

            return "";
        }

        public string GetGeometryFromVector(string key, string vector)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (vector == null) throw new ArgumentNullException(nameof(vector));

            key = GetCamelCaseKey(key);

            var element = new XElement("Geometry", vector);
            element.Add(new XAttribute("Key", key + "Path"));

            //Todo: get the xaml attribute to build properly instead of this hack
            return element.ToString().Replace("Key=\"", "x:Key=\"");
        }

        public string GetCamelCaseKey(string key)
        {
            var textInfo = new CultureInfo("en-GB", false).TextInfo;
            if (key.Contains("-"))
            {
                var items = key.Split('-');
                var titleCaseLocation = new List<string>();
                foreach (var item in items)
                {
                    var titleCase = textInfo.ToTitleCase(item);
                    titleCaseLocation.Add(titleCase);
                }
                key = string.Join("", titleCaseLocation);
            }
            else
            {
                key = textInfo.ToTitleCase(key);
            }
            return key;
        }
    }
}