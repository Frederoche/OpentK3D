using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Wave
{
    public class Wave
    {

        public int L { get; set; }
        public int N { get; set; }

        public Wave(int l, int n)
        {
            N = n;
            L = l;
        }

        public Tuple<float,float>[] GenerateWaveNumberGrid()
        {
            float[,] K = new float[N,N]; 

            for (int i = 0; i < N; i++)
            {
                for(int j=0; j< N; j++)
                {
                     K[i][j] = 
                }
            }
        }


    }
}
