using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

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
            var path = root.Descendants("{http://www.w3.org/2000/svg}path");

            if (path.Count() > 1)
            {
                Debug.WriteLine("${file} has more than one path.");
            }
            else
            {
                return path.First().Attribute("d").Value;
            }
           
            return "";
        }

        public string GetGeometryFromSvg(string key, string vector)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (vector == null) throw new ArgumentNullException(nameof(vector));

            TextInfo textInfo = new CultureInfo("en-GB", false).TextInfo;
            if (key.Contains("-"))
            {
                var items = key.Split('-');
                items.ToList().ForEach(x => textInfo.ToTitleCase(x));
                key = string.Join("", items);
            }
            else
            {
                key = textInfo.ToTitleCase(key);
            }

            var element = new XElement("Geometry", vector);
            element.Add(new XAttribute("Key", key + "Path"));

            //Todo: get the xaml attribute to build properly instead of this hack
            return element.ToString().Replace("Key=\"", "x:Key=\"");
        }
    }
}