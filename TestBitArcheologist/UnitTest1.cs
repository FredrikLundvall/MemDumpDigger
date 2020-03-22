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
            //Assert.AreEqual(1, bmpFinder.GetBit(0));
            Assert.AreEqual(0, bmpFinder.GetBit(1));
            //Assert.AreEqual(1, bmpFinder.GetBit(2));
            Assert.AreEqual(0, bmpFinder.GetBit(3));
            //Assert.AreEqual(1, bmpFinder.GetBit(4));
            Assert.AreEqual(0, bmpFinder.GetBit(5));
            //Assert.AreEqual(1, bmpFinder.GetBit(6));
            Assert.AreEqual(0, bmpFinder.GetBit(7));
        }
        [TestMethod]
        public void Test1x8x1()
        {
            int width = 1;
            int height = 8;
            int pixelBits = 1;
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
            Assert.AreEqual(Color.White, bmpFinder.GetPixel(0));
            Assert.AreEqual(Color.Black, bmpFinder.GetPixel(1));
            Assert.AreEqual(Color.White, bmpFinder.GetPixel(2));
            Assert.AreEqual(Color.Black, bmpFinder.GetPixel(3));
            Assert.AreEqual(Color.White, bmpFinder.GetPixel(4));
            Assert.AreEqual(Color.Black, bmpFinder.GetPixel(5));
            Assert.AreEqual(Color.White, bmpFinder.GetPixel(6));
            Assert.AreEqual(Color.Black, bmpFinder.GetPixel(7));
        }
        [TestMethod]
        public void Test320x200x8()
        {
            int width = 320;
            int height = 200;
            int pixelBits = 8;
            bool interleaved = false;
            bool usePalette = true;
            byte value = 32;
            BitmapFinder bmpFinder = new BitmapFinder(new MemoryStream(createBitmap(width, height, pixelBits, interleaved, value)));
            bmpFinder.Width = width;
            bmpFinder.Height = height;
            bmpFinder.PixelBits = pixelBits;
            bmpFinder.Interleaved = interleaved;
            bmpFinder.UsePalette = usePalette;
            bmpFinder.PaletteColor.Add(value, Color.Gold);
            Assert.AreEqual(Color.Gold, bmpFinder.GetPixel(0));
        }
        private byte[] createBitmap(int aWidth, int aHeight, int aColorBits, bool aInterleaved, byte aValue)
        {
            var bitmapToFind = new byte[aWidth * aHeight * aColorBits / 8];
            int i = 0;
            for (int x = 0; x < aWidth; x++)
            {
                for (int y = 0; y < aHeight; y++)
                {
                    bitmapToFind[0] = aValue;
                }
            }
            return bitmapToFind;
        }
    }
}
