using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MemDumpDigger
{
    public class BitmapFinder
    {
        protected Stream _stream;
        public byte PixelBits = 1; //1 - 64
        public uint Width = 8;
        public uint Height = 8;
        public bool Interleaved = false;
        public bool UsePalette = false;
        public bool UseAlphaChannel = false;
        public bool OverWrite = true;
        public Dictionary<UInt16, Color> PaletteColor = new Dictionary<UInt16, Color>();
        public BitmapFinder(string aPath) : this(new FileStream(aPath, FileMode.Open, FileAccess.Read))
        {
        }
        public BitmapFinder(Stream aStream)
        {
            _stream = aStream;
        }
        ~BitmapFinder()
        {
            Close();
        }
        public long GetNumberOfBits()
        {
            return _stream.Length / 8;
        }
        public long GetNumberOfPixels()
        {
            return GetNumberOfBits() / PixelBits;
        }
        public byte GetBit(UInt64 aBitPosition)
        {         
            _stream.Position = (long)aBitPosition / 8;
            byte bitNumber = (byte) (aBitPosition % 8);
            var existingByte = _stream.ReadByte();
            var byte2 = (existingByte >> (7 - bitNumber));
            byte byteAsBit = (byte) ((byte2) & 1);
            return byteAsBit;
        }
        public void SetBit(UInt64 aBitPosition, byte aByteAsBitValue)
        {
            _stream.Position = (long) aBitPosition / 8;
            byte bitNumber = (byte) (aBitPosition % 8);          
            byte existingByte = 0;
            if(!OverWrite || bitNumber != 0)
                existingByte = (byte) _stream.ReadByte();
            existingByte = (byte) ((existingByte & ~(1 << (7 - bitNumber))) | ( (aByteAsBitValue & 1) << (7 - bitNumber)));
            _stream.Position = (long)aBitPosition / 8;
            _stream.WriteByte((byte) existingByte);
        }
        public UInt64 GetPixelValue(UInt64 aPixelPosition)
        {
            UInt64 position = 0;
            if (Interleaved)
                position = aPixelPosition;
            else
                position = aPixelPosition * PixelBits;
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
        public void SetPixelValue(UInt64 aPixelPosition, UInt64 aPixelValue)
        {
            UInt64 bitPosition = aPixelPosition * PixelBits;
            UInt64 offsetMove = 1;
            if (Interleaved)
                offsetMove = Width * Height;
            for (int b = 0; b < PixelBits; b++)
            {
                SetBit(bitPosition, (byte) (aPixelValue & 1));
                if (b < PixelBits - 1)
                    aPixelValue >>= 1;
                bitPosition += offsetMove;
            }
        }
        public Color GetColorFromPixelValue(UInt64 aPixelValue)
        {
            Color pixel = new Color(0, 0, 0);
            if (UsePalette)
            {
                if (PaletteColor.ContainsKey((UInt16)aPixelValue))
                    pixel = PaletteColor[(UInt16)aPixelValue];
                else if(PaletteColor.Count > 0)
                    PaletteColor[(UInt16)aPixelValue] = PaletteColor.Values.ElementAt(0);
            }
            else
            {
                //Just using 8 bits as the different values
                if (UseAlphaChannel)
                {
                    pixel = new Color((int)((aPixelValue >> 16) & 255), (int)((aPixelValue >> 8) & 255), (int)(aPixelValue & 255), (int)((aPixelValue >> 24) & 255));
                }
                else
                {
                    pixel = new Color((int)((aPixelValue >> 16) & 255), (int)((aPixelValue >> 8) & 255), (int)( aPixelValue & 255));
                }
            }
            return pixel;
        }
        public UInt64 GetPixelValueFromColor(Color aColor)
        {
            UInt64 value = 0;
            if (UsePalette)
            {
                value = PaletteColor.FirstOrDefault(x => x.Value == aColor).Key;
            }
            else
            {
                //Just using 8 bits as the different values
                if (UseAlphaChannel)
                {
                    value = (UInt64)aColor.A << 24 | (UInt64)aColor.R << 16 | (UInt64)aColor.G << 8 | (UInt64)aColor.B;
                }
                else
                {
                    value = (UInt64)aColor.R << 16 | (UInt64)aColor.G << 8 | (UInt64)aColor.B;
                }
            }
            return value;
        }

        public void WriteFileFromArrayOfColors(Color[] aColorArray, UInt64 aStartPixelPosition)
        {
            UInt64 pixelPosition = aStartPixelPosition;
            foreach(Color color in aColorArray)
            {
                SetPixelValue(pixelPosition, GetPixelValueFromColor(color));
                pixelPosition++;
            }
            _stream.Flush();
        }
        public void Close()
        {
            _stream.Close();
            _stream.Dispose();
        }
            
        public static byte ReverseBitsWith7Operations(byte b)
        {
            return (byte)(((b * 0x0802u & 0x22110u) | (b * 0x8020u & 0x88440u)) * 0x10101u >> 16);
        }
    }
}
