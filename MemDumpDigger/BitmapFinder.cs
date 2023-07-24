using BitStream;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace MemDumpDigger
{
    public class BitmapFinder
    {
        protected BitStream.BitStream _stream;
        public byte PixelBits = 1; //1 - 64
        public uint Width = 320;
        public uint Height = 200;
        public bool Interleaved = true;
        public bool UsePalette = true;
        public bool UseAlphaChannel = true;
        public Dictionary<UInt16, Color> PaletteColor = new Dictionary<UInt16, Color>();
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
        public byte GetBit(UInt64 aBitPosition)
        {
            byte byteAsBit;
            _stream.Seek((long)aBitPosition, SeekOrigin.Begin);
            if (_stream.ReadBits(out byteAsBit, (BitNum)1))
                return byteAsBit;
            else
                return 0;
        }
        public UInt64 GetPixelValue(UInt64 aPixelPosition)
        {
            UInt64 position = aPixelPosition * PixelBits;
            UInt64 pixelValue = 0;
            UInt64 offsetMove = 1;
            if(Interleaved)
                offsetMove = Width * Height;
            for (int b = 0; b < PixelBits; b++)
            {
                pixelValue |= GetBit(position);
                if(b < PixelBits - 1)
                    pixelValue <<= 1;
                position += offsetMove;
            }
            return pixelValue;
        }
        public Color GetColorFromPixelValue(UInt64 aPixelValue)
        {
            Color pixel = Color.FromArgb(0, 0, 0);
            if (UsePalette)
            {
                if (!PaletteColor.ContainsKey((UInt16)aPixelValue))
                    PaletteColor[(UInt16)aPixelValue] = PaletteColor[0];
                pixel = PaletteColor[(UInt16)aPixelValue];
            }
            else
            {
                //Just using 8 bits as the different values
                if (UseAlphaChannel)
                {
                    pixel = Color.FromArgb((int)((aPixelValue >> 24) & 255), (int)((aPixelValue >> 16) & 255), (int)((aPixelValue >> 8) & 255), (int)(aPixelValue & 255));
                }
                else
                {
                    pixel = Color.FromArgb((int)((aPixelValue >> 16) & 255), (int)((aPixelValue >> 8) & 255), (int)( aPixelValue & 255));
                }
            }
            return pixel;
        }
        public static byte ReverseBitsWith7Operations(byte b)
        {
            return (byte)(((b * 0x0802u & 0x22110u) | (b * 0x8020u & 0x88440u)) * 0x10101u >> 16);
        }
    }
}
