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
                    currentSelector = trimmedLine.TrimStart(' ','{').TrimEnd(' ', '{');
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
                        var val = ParsePropertyValues(propertyValue);
                        switch (propertyName)
                        {
                            case "border-radius":
                                if (val.Length == 1)
                                {
                                    cssDictionary[currentSelector]["border-top-left-radius"] = val;
                                    cssDictionary[currentSelector]["border-top-right-radius"] = val;
                                    cssDictionary[currentSelector]["border-bottom-right-radius"] = val;
                                    cssDictionary[currentSelector]["border-bottom-left-radius"] = val;
                                }
                                else if (val.Length == 2)
                                {
                                    cssDictionary[currentSelector]["border-top-left-radius"] = new object[] { val[0] };
                                    cssDictionary[currentSelector]["border-top-right-radius"] = new object[] { val[1] };
                                    cssDictionary[currentSelector]["border-bottom-left-radius"] = new object[] { val[1] };
                                    cssDictionary[currentSelector]["border-bottom-right-radius"] = new object[] { val[0] };
                                }
                                else if (val.Length == 3)
                                {
                                    cssDictionary[currentSelector]["border-top-left-radius"] = new object[] { val[0] };
                                    cssDictionary[currentSelector]["border-top-right-radius"] = new object[] { val[1] };
                                    cssDictionary[currentSelector]["border-bottom-left-radius"] = new object[] { val[1] };
                                    cssDictionary[currentSelector]["border-bottom-right-radius"] = new object[] { val[2] };
                                }
                                else if (val.Length == 4)
                                {
                                    cssDictionary[currentSelector]["border-top-left-radius"] = new object[] { val[0] };
                                    cssDictionary[currentSelector]["border-top-right-radius"] = new object[] { val[1] };
                                    cssDictionary[currentSelector]["border-bottom-left-radius"] = new object[] { val[3] };
                                    cssDictionary[currentSelector]["border-bottom-right-radius"] = new object[] { val[2] };
                                }
                                break;
                            case "margin":
                                if (val.Length == 1)
                                {
                                    cssDictionary[currentSelector]["margin-top"] = val;
                                    cssDictionary[currentSelector]["margin-right"] = val;
                                    cssDictionary[currentSelector]["margin-bottom"] = val;
                                    cssDictionary[currentSelector]["margin-left"] = val;
                                }
                                else if (val.Length == 2)
                                {
                                    cssDictionary[currentSelector]["margin-top"] = new object[] { val[0] };
                                    cssDictionary[currentSelector]["margin-right"] = new object[] { val[1] };
                                    cssDictionary[currentSelector]["margin-bottom"] = new object[] { val[0] };
                                    cssDictionary[currentSelector]["margin-left"] = new object[] { val[1] };
                                }
                                else if (val.Length == 3)
                                {
                                    cssDictionary[currentSelector]["margin-top"] = new object[] { val[0] };
                                    cssDictionary[currentSelector]["margin-right"] = new object[] { val[1] };
                                    cssDictionary[currentSelector]["margin-bottom"] = new object[] { val[2] };
                                    cssDictionary[currentSelector]["margin-left"] = new object[] { val[1] };
                                }
                                else if (val.Length == 4)
                                {
                                    cssDictionary[currentSelector]["margin-top"] = new object[] { val[0] };
                                    cssDictionary[currentSelector]["margin-right"] = new object[] { val[1] };
                                    cssDictionary[currentSelector]["margin-bottom"] = new object[] { val[2] };
                                    cssDictionary[currentSelector]["margin-left"] = new object[] { val[3] };
                                }
                                break;
                            case "padding":
                                if (val.Length == 1)
                                {
                                    cssDictionary[currentSelector]["padding-top"] = val;
                                    cssDictionary[currentSelector]["padding-right"] = val;
                                    cssDictionary[currentSelector]["padding-bottom"] = val;
                                    cssDictionary[currentSelector]["padding-left"] = val;
                                }
                                else if (val.Length == 2)
                                {
                                    cssDictionary[currentSelector]["padding-top"] = new object[] { val[0] };
                                    cssDictionary[currentSelector]["padding-right"] = new object[] { val[1] };
                                    cssDictionary[currentSelector]["padding-bottom"] = new object[] { val[0] };
                                    cssDictionary[currentSelector]["padding-left"] = new object[] { val[1] };
                                }
                                else if (val.Length == 3)
                                {
                                    cssDictionary[currentSelector]["padding-top"] = new object[] { val[0] };
                                    cssDictionary[currentSelector]["padding-right"] = new object[] { val[1] };
                                    cssDictionary[currentSelector]["padding-bottom"] = new object[] { val[2] };
                                    cssDictionary[currentSelector]["padding-left"] = new object[] { val[1] };
                                }
                                else if (val.Length == 4)
                                {
                                    cssDictionary[currentSelector]["padding-top"] = new object[] { val[0] };
                                    cssDictionary[currentSelector]["padding-right"] = new object[] { val[1] };
                                    cssDictionary[currentSelector]["padding-bottom"] = new object[] { val[2] };
                                    cssDictionary[currentSelector]["padding-left"] = new object[] { val[3] };
                                }
                                break;
                            default:
                                cssDictionary[currentSelector][propertyName] = val;
                                break;
                        }
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
