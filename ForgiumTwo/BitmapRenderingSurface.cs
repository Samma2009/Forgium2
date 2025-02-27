using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cosmos.Core;
using Cosmos.System.Graphics;
using Forgium;

namespace ForgiumTwo
{
    public class BitmapRenderingSurface : Image, IRenderingSurface
    {
        public BitmapRenderingSurface(uint width, uint height, ColorDepth color) : base(width, height, color)
        {
        }

        public int width { get { return (int)Width; } set { Width = (uint)value; } }
        public int height { get { return (int)Height; } set { Height = (uint)value; } }

        public void Clear(Dictionary<string, object[]> CSSClass)
        {
            Color col = (Color)CSSClass["background-color"][0];
            MemoryOperations.Fill(RawData, col.ToArgb());
        }

        public void DrawRectangle(Dictionary<string, object[]> CSSClass, int X, int Y)
        {
            
        }

        public void DrawText(Dictionary<string, object[]> CSSClass, int X, int Y, string text)
        {
            
        }
    }
}
