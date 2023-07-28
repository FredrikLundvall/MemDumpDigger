using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MemDumpDigger;
using System.Drawing;
using System.IO;

namespace TestBitArcheologist
{
    [TestClass]
    public class TestBitmapFinder
    {
        [TestMethod]
        public void TestGetBit()
        {
            BitmapFinder bmpFinder = new BitmapFinder(new MemoryStream(new byte[] { 0b01010101 }));
            Assert.AreEqual(0, bmpFinder.GetBit(0));
            Assert.AreEqual(1, bmpFinder.GetBit(1));
            Assert.AreEqual(0, bmpFinder.GetBit(2));
            Assert.AreEqual(1, bmpFinder.GetBit(3));
            Assert.AreEqual(0, bmpFinder.GetBit(4));
            Assert.AreEqual(1, bmpFinder.GetBit(5));
            Assert.AreEqual(0, bmpFinder.GetBit(6));
            Assert.AreEqual(1, bmpFinder.GetBit(7));
        }
        [TestMethod]
        public void TestSetBit()
        {
            Stream stream = new MemoryStream(new byte[] { 0b00000000 });
            BitmapFinder bmpFinder = new BitmapFinder(stream);
            bmpFinder.SetBit(0, 0);
            bmpFinder.SetBit(1, 1);
            bmpFinder.SetBit(2, 0);
            bmpFinder.SetBit(3, 1);
            bmpFinder.SetBit(4, 0);
            bmpFinder.SetBit(5, 1);
            bmpFinder.SetBit(6, 0);
            bmpFinder.SetBit(7, 1);
            stream.Position = 0;
            var byte1 = stream.ReadByte();
            Assert.AreEqual(0b01010101, byte1);
        }
        [TestMethod]
        public void Test1x8x1()
        {
            BitmapFinder bmpFinder = new BitmapFinder(new MemoryStream(new byte[] { 0b01010101 }));
            bmpFinder.Width = 8;
            bmpFinder.Height = 1;
            bmpFinder.PixelBits = 1;
            bmpFinder.Interleaved = false;
            bmpFinder.UsePalette = true;
            bmpFinder.PaletteColor.Add(0, Color.Black);
            bmpFinder.PaletteColor.Add(1, Color.White);
            Assert.AreEqual(Color.Black, bmpFinder.GetColorFromPixelValue(bmpFinder.GetPixelValue(0)));
            Assert.AreEqual(Color.White, bmpFinder.GetColorFromPixelValue(bmpFinder.GetPixelValue(1)));
            Assert.AreEqual(Color.Black, bmpFinder.GetColorFromPixelValue(bmpFinder.GetPixelValue(2)));
            Assert.AreEqual(Color.White, bmpFinder.GetColorFromPixelValue(bmpFinder.GetPixelValue(3)));
            Assert.AreEqual(Color.Black, bmpFinder.GetColorFromPixelValue(bmpFinder.GetPixelValue(4)));
            Assert.AreEqual(Color.White, bmpFinder.GetColorFromPixelValue(bmpFinder.GetPixelValue(5)));
            Assert.AreEqual(Color.Black, bmpFinder.GetColorFromPixelValue(bmpFinder.GetPixelValue(6)));
            Assert.AreEqual(Color.White, bmpFinder.GetColorFromPixelValue(bmpFinder.GetPixelValue(7)));
        }
        [TestMethod]
        public void Test1x8x2()
        {
            BitmapFinder bmpFinder = new BitmapFinder(new MemoryStream(new byte[] { 0b00011011, 0b11100100 }));
            bmpFinder.Width = 8;
            bmpFinder.Height = 1;
            bmpFinder.PixelBits = 2;
            bmpFinder.Interleaved = false;
            bmpFinder.UsePalette = true;
            bmpFinder.PaletteColor.Add(0, Color.Black);
            bmpFinder.PaletteColor.Add(1, Color.White);
            bmpFinder.PaletteColor.Add(2, Color.Red);
            bmpFinder.PaletteColor.Add(3, Color.Blue);
            Assert.AreEqual(Color.Black, bmpFinder.GetColorFromPixelValue(bmpFinder.GetPixelValue(0)));
            Assert.AreEqual(Color.White, bmpFinder.GetColorFromPixelValue(bmpFinder.GetPixelValue(1)));
            Assert.AreEqual(Color.Red, bmpFinder.GetColorFromPixelValue(bmpFinder.GetPixelValue(2)));
            Assert.AreEqual(Color.Blue, bmpFinder.GetColorFromPixelValue(bmpFinder.GetPixelValue(3)));
            Assert.AreEqual(Color.Blue, bmpFinder.GetColorFromPixelValue(bmpFinder.GetPixelValue(4)));
            Assert.AreEqual(Color.Red, bmpFinder.GetColorFromPixelValue(bmpFinder.GetPixelValue(5)));
            Assert.AreEqual(Color.White, bmpFinder.GetColorFromPixelValue(bmpFinder.GetPixelValue(6)));
            Assert.AreEqual(Color.Black, bmpFinder.GetColorFromPixelValue(bmpFinder.GetPixelValue(7)));
        }
        [TestMethod]
        public void Test1x8x2_interleaved()
        {
            BitmapFinder bmpFinder = new BitmapFinder(new MemoryStream(new byte[] { 0b00111100, 0b01011010 }));
            bmpFinder.Width = 8;
            bmpFinder.Height = 1;
            bmpFinder.PixelBits = 2;
            bmpFinder.Interleaved = true;
            bmpFinder.UsePalette = true;
            bmpFinder.PaletteColor.Add(0, Color.Black);
            bmpFinder.PaletteColor.Add(1, Color.White);
            bmpFinder.PaletteColor.Add(2, Color.Red);
            bmpFinder.PaletteColor.Add(3, Color.Blue);
            Assert.AreEqual(Color.Black, bmpFinder.GetColorFromPixelValue(bmpFinder.GetPixelValue(0)));
            Assert.AreEqual(Color.White, bmpFinder.GetColorFromPixelValue(bmpFinder.GetPixelValue(1)));
            Assert.AreEqual(Color.Red, bmpFinder.GetColorFromPixelValue(bmpFinder.GetPixelValue(2)));
            Assert.AreEqual(Color.Blue, bmpFinder.GetColorFromPixelValue(bmpFinder.GetPixelValue(3)));
            Assert.AreEqual(Color.Blue, bmpFinder.GetColorFromPixelValue(bmpFinder.GetPixelValue(4)));
            Assert.AreEqual(Color.Red, bmpFinder.GetColorFromPixelValue(bmpFinder.GetPixelValue(5)));
            Assert.AreEqual(Color.White, bmpFinder.GetColorFromPixelValue(bmpFinder.GetPixelValue(6)));
            Assert.AreEqual(Color.Black, bmpFinder.GetColorFromPixelValue(bmpFinder.GetPixelValue(7)));
        }
        [TestMethod]
        public void Test320x200x8()
        {
            uint width = 320;
            uint height = 200;
            byte pixelBits = 8;
            byte value = 32;
            BitmapFinder bmpFinder = new BitmapFinder(new MemoryStream(createBitmap(width, height, pixelBits, value)));
            bmpFinder.Width = width;
            bmpFinder.Height = height;
            bmpFinder.PixelBits = pixelBits;
            bmpFinder.Interleaved = false;
            bmpFinder.UsePalette = true;
            bmpFinder.PaletteColor.Add(0, Color.Gray);
            bmpFinder.PaletteColor.Add(value, Color.Gold);
            bmpFinder.PaletteColor.Add(255, Color.White);
            Assert.AreEqual(Color.Gold, bmpFinder.GetColorFromPixelValue(bmpFinder.GetPixelValue(0)));
            Assert.AreEqual(Color.Gold, bmpFinder.GetColorFromPixelValue(bmpFinder.GetPixelValue((width * height) - 1)));
        }
        private byte[] createBitmap(uint aWidth, uint aHeight, uint aColorBits, byte aValue)
        {
            var bitmapToFind = new byte[aWidth * aHeight * aColorBits / 8];
            for (uint x = 0; x < aWidth; x++)
            {
                for (uint y = 0; y < aHeight; y++)
                {
                    bitmapToFind[(x + y * aWidth) * aColorBits / 8] = aValue;
                }
            }
            return bitmapToFind;
        }

    }
}
