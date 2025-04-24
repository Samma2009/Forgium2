using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgiumTwo.TTF
{
    public class ContourExtractor
    {
        public static List<Point[]> ExtractContours(BinaryReader r,short numberOfContours)
        {
            // 1) Read endPtsOfContours
            var endPts = new ushort[numberOfContours];
            for (int i = 0; i < numberOfContours; i++)
                endPts[i] = r.ReadUInt16();

            // 2) Skip instructions
            ushort instrLen = r.ReadUInt16();
            r.BaseStream.Seek(instrLen, SeekOrigin.Current);

            int pointCount = endPts[^1] + 1;

            // 3) Read flags
            var flags = new byte[pointCount];
            int idx = 0;
            while (idx < pointCount)
            {
                byte flag = r.ReadByte();
                flags[idx++] = flag;

                if ((flag & 0x08) != 0)  // Repeat flag
                {
                    int repeat = r.ReadByte();
                    for (int j = 0; j < repeat; j++)
                        flags[idx++] = flag;
                }
            }

            // 4) Read X deltas
            var xDeltas = new short[pointCount];
            for (int i = 0; i < pointCount; i++)
            {
                bool shortVec = (flags[i] & 0x02) != 0;
                bool sameOrPos = (flags[i] & 0x10) != 0;
                if (shortVec)
                {
                    byte val = r.ReadByte();
                    xDeltas[i] = sameOrPos ? val : (short)-val;
                }
                else if (!sameOrPos)
                    xDeltas[i] = r.ReadInt16();
                // else delta = 0
            }

            // 5) Read Y deltas (analogous)
            var yDeltas = new short[pointCount];
            for (int i = 0; i < pointCount; i++)
            {
                bool shortVec = (flags[i] & 0x04) != 0;
                bool sameOrPos = (flags[i] & 0x20) != 0;
                if (shortVec)
                {
                    byte val = r.ReadByte();
                    yDeltas[i] = sameOrPos ? val : (short)-val;
                }
                else if (!sameOrPos)
                    yDeltas[i] = r.ReadInt16();
            }

            // 6) Accumulate to get absolute coords
            var xs = new short[pointCount];
            var ys = new short[pointCount];
            for (int i = 0; i < pointCount; i++)
            {
                xs[i] = (short)((i > 0 ? xs[i - 1] : 0) + xDeltas[i]);
                ys[i] = (short)((i > 0 ? ys[i - 1] : 0) + yDeltas[i]);
            }

            // 7) Split into contours
            var contours = new List<Point[]>();
            int start = 0;
            foreach (var endPt in endPts)
            {
                var contour = new List<Point>();
                for (int i = start; i <= endPt; i++)
                    contour.Add(new Point(xs[i], ys[i]));
                contours.Add(contour.ToArray());
                start = endPt + 1;
            }

            return contours;
        }
    }

}
