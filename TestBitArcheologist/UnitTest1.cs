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
            BitmapFinder bmpFinder = new BitmapFinder(new MemoryStream(new byte[] { 0b0101_0101 }));
            Assert.AreEqual(1, bmpFinder.GetBit(0));
            Assert.AreEqual(0, bmpFinder.GetBit(1));
            Assert.AreEqual(1, bmpFinder.GetBit(2));
            Assert.AreEqual(0, bmpFinder.GetBit(3));
            Assert.AreEqual(1, bmpFinder.GetBit(4));
            Assert.AreEqual(0, bmpFinder.GetBit(5));
            Assert.AreEqual(1, bmpFinder.GetBit(6));
            Assert.AreEqual(0, bmpFinder.GetBit(7));
        }
        [TestMethod]
        public void Test1x8x1()
        {
            uint width = 1;
            uint height = 8;
            byte pixelBits = 1;
            bool interleaved = false;
            bool usePalette = true;
            BitmapFinder bmpFinder = new BitmapFinder(new MemoryStream(new byte[] { 0b1010_1010 }));
            bmpFinder.Width = width;
            bmpFinder.Height = height;
            bmpFinder.PixelBits = pixelBits;
            bmpFinder.Interleaved = interleaved;
            bmpFinder.UsePalette = usePalette;
            Assert.AreEqual(0, bmpFinder.GetBit(0));
            bmpFinder.PaletteColor.Add(0, Color.Black);
            bmpFinder.PaletteColor.Add(1, Color.White);
            Assert.AreEqual(Color.White, bmpFinder.GetColorFromPixelValue(bmpFinder.GetPixelValue(0)));
            Assert.AreEqual(Color.Black, bmpFinder.GetColorFromPixelValue(bmpFinder.GetPixelValue(1)));
            Assert.AreEqual(Color.White, bmpFinder.GetColorFromPixelValue(bmpFinder.GetPixelValue(2)));
            Assert.AreEqual(Color.Black, bmpFinder.GetColorFromPixelValue(bmpFinder.GetPixelValue(3)));
            Assert.AreEqual(Color.White, bmpFinder.GetColorFromPixelValue(bmpFinder.GetPixelValue(4)));
            Assert.AreEqual(Color.Black, bmpFinder.GetColorFromPixelValue(bmpFinder.GetPixelValue(5)));
            Assert.AreEqual(Color.White, bmpFinder.GetColorFromPixelValue(bmpFinder.GetPixelValue(6)));
            Assert.AreEqual(Color.Black, bmpFinder.GetColorFromPixelValue(bmpFinder.GetPixelValue(7)));
        }
        [TestMethod]
        public void Test320x200x8()
        {
            uint width = 320;
            uint height = 200;
            byte pixelBits = 8;
            bool interleaved = false;
            bool usePalette = true;
            byte value = 32;
            BitmapFinder bmpFinder = new BitmapFinder(new MemoryStream(createBitmap(width, height, pixelBits, BitmapFinder.ReverseBitsWith7Operations(value))));
            bmpFinder.Width = width;
            bmpFinder.Height = height;
            bmpFinder.PixelBits = pixelBits;
            bmpFinder.Interleaved = interleaved;
            bmpFinder.UsePalette = usePalette;
            bmpFinder.PaletteColor.Add(value, Color.Gold);
            Assert.AreEqual(Color.Gold, bmpFinder.GetColorFromPixelValue(bmpFinder.GetPixelValue(0)));
        }
        private byte[] createBitmap(uint aWidth, uint aHeight, uint aColorBits, byte aValue)
        {
            var bitmapToFind = new byte[aWidth * aHeight * aColorBits / 8];
            for (uint x = 0; x < aWidth; x++)
            {
                for (uint y = 0; y < aHeight; y++)
                {
                    bitmapToFind[x * y * aColorBits / 8] = aValue;
                }
            }
            return bitmapToFind;
        }

    }
}
