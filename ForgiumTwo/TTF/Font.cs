using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cosmos.System.Graphics;

namespace ForgiumTwo.TTF
{
    public class Font
    {
        byte[] data;
        BinaryReader reader;
        MemoryStream stream;

        uint sfntVersion;
        ushort TableCount,GlyphCount;
        TableRecord[] tables;
        TableRecord head, maxp, loca, glyf;

        short indexToLocFormat;
        uint version,fontRevision,checksum,magic,flags,unitsPerEm;

        Dictionary<char, RawGliph> RawGliphs = new();
        Dictionary<char, Bitmap> CachedGliphs = new();

        public Font(byte[] data)
        {
            this.data   = data;
            stream      = new MemoryStream(data);
            reader      = new(stream);
            sfntVersion = reader.ReadUInt32(); // read sfnt
            TableCount  = reader.ReadUInt16(); // read table count

            reader.ReadUInt16(); // skip searchRange
            reader.ReadUInt16(); // skip entrySelector
            reader.ReadUInt16(); // skip rangeShift

            tables = new TableRecord[TableCount]; // create tables
            for (int i = 0; i < TableCount; i++) // populate tables
            {
                tables[i].Tag      = Encoding.ASCII.GetString(reader.ReadBytes(4));
                tables[i].CheckSum = reader.ReadUInt32();
                tables[i].Offset   = reader.ReadUInt32();
                tables[i].Length   = reader.ReadUInt32();

                if (tables[i].Tag == "head") head      = tables[i]; // set head
                else if (tables[i].Tag == "maxp") maxp = tables[i]; // set maxp
                else if (tables[i].Tag == "loca") loca = tables[i]; // set loca
                else if (tables[i].Tag == "glyf") glyf = tables[i]; // set glyf
            }

            version      = reader.ReadUInt32(); // read version
            fontRevision = reader.ReadUInt32(); // read font revision
            checksum     = reader.ReadUInt32(); // read checksum adjustment
            magic        = reader.ReadUInt32(); // read magic number
            flags        = reader.ReadUInt16(); // read flags
            unitsPerEm   = reader.ReadUInt16(); // read units per em

            FetchGlyphs();
        }

        void FetchGlyphs()
        {
            stream.Seek(maxp.Offset + 4,SeekOrigin.Begin);
            GlyphCount = reader.ReadUInt16();

            stream.Seek(head.Offset + 50, SeekOrigin.Begin);
            indexToLocFormat = reader.ReadInt16();

            stream.Seek(loca.Offset, SeekOrigin.Begin);
            uint[] offsets = new uint[GlyphCount + 1];

            if (indexToLocFormat == 0)
            {
                for (int i = 0; i <= GlyphCount; i++)
                    offsets[i] = reader.ReadUInt16() * 2u;
            }
            else
            {
                for (int i = 0; i <= GlyphCount; i++)
                    offsets[i] = reader.ReadUInt32();
            }

            for (int i = 0; i < GlyphCount; i++)
            {
                uint start = glyf.Offset + offsets[i];
                uint end = glyf.Offset + offsets[i + 1];
                if (start == end)
                {
                    continue;
                }

                stream.Seek(start, SeekOrigin.Begin);
                short contours = reader.ReadInt16();
                short xMin = reader.ReadInt16(), yMin = reader.ReadInt16();
                short xMax = reader.ReadInt16(), yMax = reader.ReadInt16();

                var points = ContourExtractor.ExtractContours(reader,contours);

                var gliph = new RawGliph
                {
                    Min = new(xMin,yMin),
                    Max = new(xMax,yMax),
                    Points = points
                };
                RawGliphs.Add('s',gliph);
            }
        }
    }
}
