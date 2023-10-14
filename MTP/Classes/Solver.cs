
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace MTP.Classes
{
	public class Solver
	{
        public double h_x { get; set; } 
		public double h_t { get; set; }
		public double Beta { get; set; }
        public double Gamma { get; set; }
        public bool Stability { get; set; }
        public double h { get; set; }
        public double[,] V { get; set; }
        public double[] Xarr { get; set; }
        public double[] Tarr { get; set; }

        public bool Init()
        {
            Beta = ParamsHelper.k / ParamsHelper.c;
            h_x = ParamsHelper.l_x / ParamsHelper.I_x;
            h_t  = ParamsHelper.T / ParamsHelper.K_t;
            Gamma = (Beta * h_t) / Math.Pow(h_x, 2);
            h = ParamsHelper.Alpha/ ParamsHelper.k;
            V = new double[ParamsHelper.K_t + 1, ParamsHelper.I_x + 1];
            Xarr = GetArrH(ParamsHelper.I_x + 1, h_x);
            Tarr = GetArrH(ParamsHelper.K_t + 1, h_t);
            //I^2 <= l_x^2 / (2 * beta * T) * K
            return Math.Pow(ParamsHelper.I_x, 2) <= (ParamsHelper.K_t * Math.Pow(ParamsHelper.l_x, 2) / (2 * Beta * ParamsHelper.T));
        }

        public Solver()
        {
            Stability = Init();
            if (Stability)
            {
                //нач условие
                for (int i = 0; i < ParamsHelper.I_x + 1; ++i)
                {
                    V[0, i] = Psi(Xarr[i]);
                }

                for (int k = 1; k < ParamsHelper.K_t + 1; ++k)
                {
                    //внутренние узлы
                    for (int i = 1; i < ParamsHelper.I_x; ++i)
                    {
                        V[k, i] = FindNode(k - 1, i);
                    }

                    // граничные узлы
                    var bordCoef = (1 + h * h_x);
                    // i = 0
                    V[k, 0] = V[k, 1] / bordCoef;
                    // i = I
                    V[k, ParamsHelper.I_x] = V[k, ParamsHelper.I_x - 1] / bordCoef;
                }
            }
        }

        public double FindNode(int k, int i)
        {
            return Gamma * V[k, i + 1] + (1 - 2 * Gamma) * V[k, i] + Gamma * V[k, i - 1];
        }

        public double Psi(double x_i)
        {
            return 5 * Math.Sin((Math.PI * x_i) / ParamsHelper.l_x);
        }

        public double[] GetArrH(int size, double step)
        {
            var cur = 0.0;
            var arr = new double[size];
            arr[0] = cur;
            for (int i = 1; i < size; ++i)
                arr[i] = cur += step;
            return arr;
        }
    }
}
