using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cosmos.Core;
using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;
using CosmosTTF;
using Forgium;
using OpenTerm;

namespace ForgiumTwo
{
    public class BitmapRenderingSurface : IRenderingSurface
    {
        public Bitmap image;
        TTFBitmapSurface surface;
        public BitmapRenderingSurface(uint width, uint height)
        {
            image = new Bitmap(width, height, ColorDepth.ColorDepth32);
            surface = new(image);
            MemoryOperations.Fill(image.RawData, Color.White.ToArgb());
        }

        public int width { get { return (int)image.Width; } set {} }
        public int height { get { return (int)image.Height; } set {} }

        public Dictionary<string, TTFFont> fonts = new();

        public Size CaculateTextSize(string text, Dictionary<string, object[]> CSSClass)
        {
            if (fonts.ContainsKey((string)CSSClass["font-family"][0]+ (string)CSSClass["font-weight"][0]))
            {
                var w = fonts[(string)CSSClass["font-family"][0]+ (string)CSSClass["font-weight"][0]].CalculateWidth(text, (int)CSSClass["font-size"][0]);
                var h = (int)CSSClass["font-size"][0];
                return new Size(w,h);
            }
            else return Size.Empty;
        }

        public void Clear(Dictionary<string, object[]> CSSClass)
        {
            Color col = (Color)CSSClass["background-color"][0];
            MemoryOperations.Fill(image.RawData, col.ToArgb());
        }

        public void DrawRectangle(Dictionary<string, object[]> CSSClass, int X, int Y, Size region)
        {
            image.DrawFilledRoundedRectangle(X, Y, region.Width, region.Height, 0, (Color)CSSClass["background-color"][0]);
            Kernel.PrintDebug(Y.ToString());
            Kernel.PrintDebug(region.Height.ToString());
        }

        public void DrawText(Dictionary<string, object[]> CSSClass, int X, int Y, string text, Size region)
        {
            fonts[(string)CSSClass["font-family"][0]+ (string)CSSClass["font-weight"][0]].DrawToSurface(
                surface,
                (int)CSSClass["font-size"][0],
                X + (int)CSSClass["padding-left"][0],
                Y+ ((int)CSSClass["font-size"][0]- (int)CSSClass["font-size"][0] / 5) + (int)CSSClass["padding-top"][0],
                text,
                (Color)CSSClass["color"][0]
                );
        }
    }
}
