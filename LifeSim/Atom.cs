using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Raylib_cs;

namespace LifeSim
{
    internal class Atom
    {
        public int X { get; set; }
        public int Y { get; set; }
        public double VelX { get; set; }
        public double VelY { get; set; }

        public int Size { get; set; }

        //Raylib color
        public Color Color { get; set; }

        public Atom(int x, int y, int size, Color color, double vx = 0, double vy = 0)
        {
            X = x;
            Y = y;

            VelX = vx;
            VelY = vy;

            Size = size;

            Color = color;
        }

        public static List<Atom> Create(int amount, int size, Color color)
        {
            List<Atom> res = new List<Atom>();
            Random rand = new Random();
            for (int i = 0; i < amount; i++)
            {
                res.Add(new Atom(rand.Next(900), rand.Next(900), size, color));
            }

            return res;
        }
    }
}
