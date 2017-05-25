using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD.Extractor.Utility
{
    public class Extractor
    {
        public string GetScalarFromSvg(string accountSvgPath)
        {
            if (accountSvgPath.EndsWith(".svg") == false)
            {
                throw new InvalidOperationException("Must be an SVG file.");
            }

            return "";
        }
    }
}
