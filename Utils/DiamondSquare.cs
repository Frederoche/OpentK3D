using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class DiamondSquare
    {
        public int Size { get; set; }

        private Random _random;
        public double[,] Array;

        public DiamondSquare(int size)
        {
            Size = size;
            _random = new Random();
            Array = new double[Size, Size];
        }

        public void Execute()
        {
            MakeNoise();
            Generate(Size);
        }

        private void MakeNoise()
        {
            for (int i = 0; i < Size; i += 1)
            {
                for (int j = 0; j < Size; j += 1)
                {
                    Array[i, j] = _random.NextDouble() * 2 - 1;
                }
            }
        }

        private double Sample(int x, int y)
        {
            return Array[x & (Size - 1), y & (Size - 1)];
        }

        private void Square(int x, int y, int stepSize, double value)
        {
            int hs = stepSize / 2;

            double a = Sample(x - hs, y - hs);
            double b = Sample(x - hs, y + hs);
            double c = Sample(x + hs, y + hs);
            double d = Sample(x + hs, y - hs);

            Array[x & (Size - 1), y & (Size - 1)] = (a + b + c + d) / 4 + value;
        }

        private void MakeSquare(int stepSize, double scale)
        {
            int hs = stepSize / 2;

            for (int i = hs; i < Size + hs; i += stepSize)
            {
                for (int j = hs; j < Size + hs; j += stepSize)
                {
                    Square(i, j, stepSize, (_random.NextDouble() * 2 - 1) * 0.65 * scale);
                }
            }

            for (int i = 0; i < Size; i += stepSize)
            {
                for (int j = 0; j < Size; j += stepSize)
                {
                    Diamond(i + hs, j, stepSize, (_random.NextDouble() * 2 - 1) * 0.65 * scale);
                    Diamond(i, j + hs, stepSize, (_random.NextDouble() * 2 - 1) * 0.65 * scale);
                }
            }
        }

        private void Generate(int size)
        {
            double scale = 1.0;

            while (size > 1)
            {
                MakeSquare(size, scale);
                size = size / 2;
                scale = scale / 2;
            }
        }

        private void Diamond(int x, int y, int stepSize, double value)
        {
            int hs = stepSize / 2;

            double a = Sample(x - hs, y);
            double b = Sample(x + hs, y);
            double c = Sample(x, y - hs);
            double d = Sample(x, y + hs);

            Array[x & (Size - 1), y & (Size - 1)] = (a + b + c + d) / 4 + value;
        }
    }
}
