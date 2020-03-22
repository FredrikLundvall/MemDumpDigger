using BitStream;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace MemDumpDigger
{
    public class BitmapFinder
    {
        protected Random rand = new Random();
        protected BitStream.BitStream _stream;
        public int PixelBits = 1; //1 - 32
        public int Width = 320;
        public int Height = 200;
        public bool Interleaved = true;
        public bool UsePalette = true;
        public Dictionary<uint, Color> PaletteColor = new Dictionary<uint, Color>();
        public BitmapFinder(string aPath) : this(new FileStream(aPath, FileMode.Open, FileAccess.Read))
        {
            //_stream = new BitStream.BitStream();
        }
        public BitmapFinder(Stream aStream)
        {
            _stream = new BitStream.BitStream(aStream);
        }
        public long GetNumberOfBits()
        {
            return _stream.Length;
        }
        public long GetNumberOfPixels()
        {
            return _stream.Length / PixelBits;
        }
        public byte GetBit(long aBitPosition)
        {
            byte byteAsBit;
            _stream.Seek(aBitPosition, SeekOrigin.Begin);
            if (_stream.ReadBits(out byteAsBit, (BitNum)1))
                return byteAsBit;
            else
                return 0;
        }
        public Color GetPixel(long aPixelPosition)
        {
            long position = aPixelPosition * PixelBits;
            uint pixelBits = 0;
            long offsetMove = 1;
            if(Interleaved)
                offsetMove = Width * Height;
            for (int b = 0; b < PixelBits; b++)
            {
                pixelBits |= GetBit(position);
                if(b < PixelBits - 1)
                    pixelBits <<= 1;
                position += offsetMove;
            }

            Color pixel = Color.FromArgb(0, 0, 0);
            if (UsePalette)
            {
                if (!PaletteColor.ContainsKey(pixelBits))
                    PaletteColor[pixelBits] = Color.FromArgb(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255));
                pixel = PaletteColor[pixelBits];
            }
            return pixel;
        }
     }
}
