using System;
using System.Collections.Generic;
using System.Drawing;

namespace Utils
{
    public class Voronoi
    {
        public int Size { get; set; }

        private readonly Random _random;
        public int[,] Array;
        private List<Tuple<int, int>> _pointList;

        public int N { get; set; }

        public Voronoi(int size, int n)
        {
            Size = size;

            _random = new Random();
            _pointList = new List<Tuple<int, int>>();
            Array = new int[Size, Size];

            N = n;
        }

        public void Execute()
        {
            Tuple<int, int> closestPoint = null;
            for(int i = 0; i < N; i++)
            {
                _pointList.Add(new Tuple<int, int>(_random.Next(0, Size), _random.Next(0, Size)));
            }

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    closestPoint = FindClosestPoint(i,j);
                    Array[i,j] = Distance(closestPoint.Item1, closestPoint.Item2, i, j);
                }
            }
        }

        private Tuple<int,int> FindClosestPoint(int x, int y)
        {
            int min = Size*2;
            Tuple<int, int> closestPoint = null;

            foreach(var point in _pointList)
            {
                if(Distance(point.Item1, point.Item2, x, y) < min)
                {
                    min = Distance(point.Item1, point.Item2, x, y);
                    closestPoint = new Tuple<int, int>(point.Item1, point.Item2);
                }
            }
            return closestPoint;
        }

        private int Distance(int x1, int y1, int x2, int y2)
        {
            return (int) Math.Sqrt((y2 - y1) * (y2 - y1) + (x2 - x1) * (x2 - x1));
        }

        public void Save(string path)
        {
            Bitmap image = new Bitmap(Size - 1, Size - 1);

            for (int i = 0; i < Size - 1; i++)
            {
                for (int j = 0; j < Size - 1; j++)
                {
                    int c = Array[i, j];
                    image.SetPixel(i, j, Color.FromArgb(c, c, c));
                }
            }
            image.Save(path);
        }
    }
}


