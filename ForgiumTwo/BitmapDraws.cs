using Cosmos.System.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenTerm
{
    public static class BitmapDraws
    {

        static string ASC16Base64 = "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AAB4zMzM2MzGxsbMAAAAAAAA/sbGwMDAwMDAwAAAAAAAAAAA/mxsbGxsbGwAAAAAAAAA/sZgMBgwYMb+AAAAAAAAAAAAftjY2NjYcAAAAAAAAAAAZmZmZmZ8YGDAAAAAAAAAAHbcGBgYGBgYAAAAAAAAAH4YPGZmZjwYfgAAAAAAAAA4bMbG/sbGbDgAAAAAAAA4bMbGxmxsbGzuAAAAAAAAHjAYDD5mZmZmPAAAAAAAAAAAAH7b29t+AAAAAAAAAAAAAwZ+29vzfmDAAAAAAAAAHDBgYHxgYGAwHAAAAAAAAAB8xsbGxsbGxsYAAAAAAAAAAP4AAP4AAP4AAAAAAAAAAAAYGH4YGAAA/wAAAAAAAAAwGAwGDBgwAH4AAAAAAAAADBgwYDAYDAB+AAAAAAAADhsbGBgYGBgYGBgYGBgYGBgYGBgYGNjY2HAAAAAAAAAAABgYAH4AGBgAAAAAAAAAAAAAdtwAdtwAAAAAAAAAOGxsOAAAAAAAAAAAAAAAAAAAAAAAABgYAAAAAAAAAAAAAAAAAAAAGAAAAAAAAAAADwwMDAwM7GxsPBwAAAAAANhsbGxsbAAAAAAAAAAAAABw2DBgyPgAAAAAAAAAAAAAAAAAfHx8fHx8fAAAAAAAAAAAAAAAAAAAAAAAAAAAAA==";
        static MemoryStream ASC16FontMS = new MemoryStream(Convert.FromBase64String(ASC16Base64));

        public static void DrawString(this Bitmap bmp, string s, int x, int y)
        {
            Color col = Color.White;
            string[] lines = s.Split('\n');
            for (int l = 0; l < lines.Length; l++)
            {
                col = Color.White;
                int Of = 0;
                for (int c = 0; c < lines[l].Length; c++)
                {
                    if (lines[l][c] == '§' && c + 1 < lines[l].Length)
                    {
                        col = GetColorFromCode(lines[l][c + 1]);
                        c++;
                        Of += 16;
                        continue;
                    }

                    int offset = (Encoding.ASCII.GetBytes(lines[l][c].ToString())[0] & 0xFF) * 16;
                    ASC16FontMS.Seek(offset, SeekOrigin.Begin);
                    byte[] fontbuf = new byte[16];
                    ASC16FontMS.Read(fontbuf, 0, fontbuf.Length);

                    for (int i = 0; i < 16; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if ((fontbuf[i] & (0x80 >> j)) != 0)
                            {
                                if (x + c * 8 <= bmp.Width)
                                {
                                    DrawPixel(bmp, (int)((x + j) + (c * 8)) - Of, (int)(y + i + (l * 16)), col);
                                }
                            }
                        }
                    }
                }
            }
        }

        private static Color GetColorFromCode(char code)
        {
            return code switch
            {
                '0' => Color.Black,
                '1' => Color.DarkBlue,
                '2' => Color.DarkGreen,
                '3' => Color.DarkCyan,
                '4' => Color.DarkRed,
                '5' => Color.DarkMagenta,
                '6' => Color.Gold,
                '7' => Color.Gray,
                '8' => Color.DarkGray,
                '9' => Color.Blue,
                'a' => Color.Green,
                'b' => Color.Cyan,
                'c' => Color.Red,
                'd' => Color.Magenta,
                'e' => Color.Yellow,
                'f' => Color.White,
                _ => Color.White,
            };
        }

        public static Color GetPointColor(this Bitmap image, int X, int Y)
        {
            return Color.FromArgb(image.RawData[X + Y * image.Width]);
        }
        public static void SetPixel(this Bitmap bmp,Color color, int X, int Y)
        {
            bmp.RawData[X + Y * bmp.Width] = color.ToArgb();
        }
        public static Bitmap FromBMPRegion(Bitmap canvas, int X, int Y, ushort W, ushort H)
        {
            Bitmap bitmap = new Bitmap(W, H, canvas.Depth);
            for (int i = X; i < W + X; i++)
            {
                for (int j = Y; j < H + Y; j++)
                {
                    bitmap.SetPixel(canvas.GetPointColor(i, j), i - X, j - Y);
                }
            }

            return bitmap;
        }
        public static void DrawPixel(this Bitmap bmp, int X, int Y, Color color)
        {
            if (color.A <255)
            {
                color = AlphaBlend(color, Color.FromArgb(bmp.RawData[X + Y * bmp.Width]),color.A);
            }
            bmp.RawData[X + Y * bmp.Width] = color.ToArgb();
            
        }
        public static void DrawFilledRoundedRectangle(
        this Bitmap bmp,
        int left, int top,
        int width, int height,
        int rTL, int rTR, int rBR, int rBL,
        Color color)
        {
            int right = left + width;
            int bottom = top + height;

            // Pre-clamp radii so they never exceed half the rect dimensions
            rTL = Math.Max(0, Math.Min(rTL, Math.Min(width, height) / 2));
            rTR = Math.Max(0, Math.Min(rTR, Math.Min(width, height) / 2));
            rBR = Math.Max(0, Math.Min(rBR, Math.Min(width, height) / 2));
            rBL = Math.Max(0, Math.Min(rBL, Math.Min(width, height) / 2));

            // For each scanline...
            for (int y = top; y < bottom; y++)
            {
                // Compute how much to inset on the left
                int insetL = 0;
                if (y < top + rTL)
                {
                    // Top-left quarter-circle
                    int dy = (top + rTL) - y;
                    insetL = rTL - (int)Math.Floor(Math.Sqrt(rTL * (long)rTL - dy * (long)dy));
                }
                else if (y >= bottom - rBL)
                {
                    // Bottom-left quarter-circle
                    int dy = y - (bottom - rBL - 1);
                    insetL = rBL - (int)Math.Floor(Math.Sqrt(rBL * (long)rBL - dy * (long)dy));
                }

                // Compute how much to inset on the right
                int insetR = 0;
                if (y < top + rTR)
                {
                    // Top-right quarter-circle
                    int dy = (top + rTR) - y;
                    insetR = rTR - (int)Math.Floor(Math.Sqrt(rTR * (long)rTR - dy * (long)dy));
                }
                else if (y >= bottom - rBR)
                {
                    // Bottom-right quarter-circle
                    int dy = y - (bottom - rBR - 1);
                    insetR = rBR - (int)Math.Floor(Math.Sqrt(rBR * (long)rBR - dy * (long)dy));
                }

                // Fill the span [left+insetL, right-insetR)
                int xStart = left + insetL;
                int xEnd = right - insetR;
                for (int x = xStart; x < xEnd; x++)
                {
                    DrawPixel(bmp, x, y, color);
                }
            }
        }

        public static void DrawRectangle(this Bitmap bmp, int left, int top, int width, int height, Color color)
        {

            bmp.DrawLine(color,left,top,width,top);
            bmp.DrawLine(color,width,top,width,height);
            bmp.DrawLine(color,left,height,width,height);
            bmp.DrawLine(color,left,top,left,height);
        }

        public static void DrawLine(this Bitmap bmp,Color color, int x1, int y1, int x2, int y2)
        {

            // Bresenham's line algorithm
            int dx = Math.Abs(x2 - x1);
            int dy = Math.Abs(y2 - y1);
            int sx = x1 < x2 ? 1 : -1;
            int sy = y1 < y2 ? 1 : -1;
            int err = dx - dy;

            while (true)
            {

                DrawPixel(bmp, x1, y1, color);

                if (x1 == x2 && y1 == y2)
                    break;

                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    x1 += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y1 += sy;
                }
            }
        }

        public static void DrawImageAlpha(this Bitmap bmp, Bitmap image, int x, int y)
        {
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    Color color = Color.FromArgb(image.RawData[i + j * image.Width]);
                    if (color.A > 0)
                    {
                        if(color.A < 255)
                        {
                            color = AlphaBlend(color, Color.FromArgb(bmp.RawData[(x + i) + (y + j) * bmp.Width]), color.A);
                        }
                        bmp.RawData[(x + i) + (y + j) * bmp.Width] = color.ToArgb();
                    }
                        
                }
            }
        }
        public static void DrawImagePart(this Bitmap bmp, Bitmap image, int x, int y,int w,int h,int ox = 0,int oy = 0)
        {
            for (int i = ox; i < w; i++)
            {
                for (int j = oy; j < h; j++)
                {
                    Color color = Color.FromArgb(image.RawData[i + j * image.Width]);
                    if (color.A > 0)
                    {
                        if (color.A < 255)
                        {
                            color = AlphaBlend(color, Color.FromArgb(bmp.RawData[(x + i) + (y + j) * bmp.Width]), color.A);
                        }
                        bmp.RawData[(x + i) + (y + j) * bmp.Width] = color.ToArgb();
                    }

                }
            }
        }
        public static void FillCircle(this Bitmap bmp,int cx, int cy,int r,Color color)
        {
            int rSq = r * r;
            for (int dy = -r; dy <= r; dy++)
            {
                int yy = cy + dy;
                int dxLimit = (int)Math.Floor(Math.Sqrt(rSq - dy * dy));  // avoid sqrt artifacts :contentReference[oaicite:2]{index=2}
                for (int dx = -dxLimit; dx <= dxLimit; dx++)
                {
                    DrawPixel(bmp, cx + dx, yy, color);
                }
            }
        }
        public static void Clear(this Bitmap bmp, Color col)
        {
            int argb = col.ToArgb();

            for (int i = 0; i < bmp.RawData.Length; i++)
            {
                bmp.RawData[i] = argb;
            }
        }
        public static Color AlphaBlend(Color to, Color from, byte alpha)
        {
            byte R = (byte)(((to.R * alpha) + (from.R * (255 - alpha))) >> 8);
            byte G = (byte)(((to.G * alpha) + (from.G * (255 - alpha))) >> 8);
            byte B = (byte)(((to.B * alpha) + (from.B * (255 - alpha))) >> 8);
            return Color.FromArgb(R, G, B);
        }

        public static void resize(this Bitmap bmp,uint NewW, uint NewH)
        {
            bmp.RawData = ScaleImage(bmp, (int)NewW, (int)NewH);
        }

        private static int[] ScaleImage(Image image, int newWidth, int newHeight)
        {
            int[] rawData = image.RawData;
            int width = (int)image.Width;
            int height = (int)image.Height;
            int[] array = new int[newWidth * newHeight];
            int num = (width << 16) / newWidth + 1;
            int num2 = (height << 16) / newHeight + 1;
            for (int i = 0; i < newHeight; i++)
            {
                for (int j = 0; j < newWidth; j++)
                {
                    int num3 = j * num >> 16;
                    int num4 = i * num2 >> 16;
                    array[i * newWidth + j] = rawData[num4 * width + num3];
                }
            }

            return array;
        }

    }
}
