using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cosmos.System.Graphics;
using CosmosTTF;
using OpenTerm;

namespace ForgiumTwo
{
    internal class TTFBitmapSurface : ITTFSurface
    {
        Bitmap image;
        public TTFBitmapSurface(Bitmap bmp)
        {
            image = bmp;
        }

        public void DrawBitmap(Bitmap bmp, int x, int y)
        {
            image.DrawImageAlpha(bmp,x,y);
        }
    }
}
