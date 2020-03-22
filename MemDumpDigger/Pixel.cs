using System;
using Microsoft.Xna.Framework;

namespace MemDumpDigger
{
    public struct Pixel
    {
        public int X;
        public int Y;
        public Color Col;

        public Pixel(int aX, int aY, Color aCol )
        {
            X = aX;
            Y = aY;
            Col = aCol;
        }
    }
}
