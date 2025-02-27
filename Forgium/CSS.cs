using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forgium
{
    public static class CSS
    {
        public static Dictionary<string, Dictionary<string, object[]>> Parse(string css)
        {
            var cssDictionary = new Dictionary<string, Dictionary<string, object[]>>();
            var lines = css.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            string currentSelector = null!;

            foreach (var line in lines)
            {
                var trimmedLine = line.Trim();
                if (string.IsNullOrWhiteSpace(trimmedLine))
                    continue;

                if (trimmedLine.EndsWith("{"))
                {
                    currentSelector = trimmedLine.Trim().Substring(1, trimmedLine.Length - 1);
                    cssDictionary[currentSelector] = new Dictionary<string, object[]>();
                }
                else if (trimmedLine.EndsWith("}"))
                {
                    currentSelector = null!;
                }
                else if (currentSelector != null)
                {
                    var splitIndex = trimmedLine.IndexOf(':');
                    if (splitIndex > 0)
                    {
                        var propertyName = trimmedLine.Substring(0, splitIndex).Trim();
                        var propertyValue = trimmedLine.Substring(splitIndex + 1).Trim().TrimEnd(';');
                        cssDictionary[currentSelector][propertyName] = ParsePropertyValues(propertyValue);
                    }
                }
            }

            return cssDictionary;
        }

        public static object[] ParsePropertyValues(string propertyValue)
        {
            var values = new List<object>();
            var parts = propertyValue.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var part in parts)
            {
                if (part.EndsWith("px") && int.TryParse(part.Replace("px", ""), out int pxValue))
                {
                    values.Add(pxValue);
                }
                else if (part.StartsWith("#") && (part.Length == 7 | part.Length == 9))
                {
                    values.Add(ParseHexColor(part));
                }
                else if (part.StartsWith("rgba(") && part.EndsWith(")"))
                {
                    var p = part.Replace("rgba(","").TrimEnd(')').Split(',',StringSplitOptions.TrimEntries);
                    values.Add(Color.FromArgb(int.Parse(p[3]), int.Parse(p[0]), int.Parse(p[1]), int.Parse(p[2])));
                }
                else if (int.TryParse(part, out int val))
                {
                    values.Add(val);
                }
                else
                {
                    values.Add(part);
                }
            }

            return values.ToArray();
        }

        public static Color ParseHexColor(string hexColor)
        {
            if (hexColor.StartsWith("#"))
            {
                hexColor = hexColor.Substring(1);
            }

            if (hexColor.Length == 6)
            {
                int r = int.Parse(hexColor.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                int g = int.Parse(hexColor.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                int b = int.Parse(hexColor.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                return Color.FromArgb(r, g, b);
            }
            else if (hexColor.Length == 8)
            {
                int r = int.Parse(hexColor.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                int g = int.Parse(hexColor.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                int b = int.Parse(hexColor.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                int a = int.Parse(hexColor.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
                return Color.FromArgb(a, r, g, b);
            }
            else
            {
                throw new ArgumentException("Invalid hex color format.");
            }
        }


    }

}
