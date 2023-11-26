using System;
using System.Collections.Generic;
using System.Linq;

namespace MTP.Classes
{
    public class AnaliticSolver
    {
        public double T;
        public double lx;
        public double a;
        public double k;
        public double c;
        public double b;
        public int N;
        public static double eps = Math.Pow(10, -12);

        //public static int pCounter = 0;
        public double d = Math.Pow(10, -7);
        public double l = Math.Pow(10, -7);

        public List<double> pn;
        public List<double> gn;
        public List<double> cn;
        public List<Func<double, double, double>> functions;

        public AnaliticSolver()
        {
            T = ParamsHelper.T;
            lx = ParamsHelper.l_x;
            a = ParamsHelper.Alpha;
            k = ParamsHelper.k;
            a /= k;
            c = ParamsHelper.c;
            N = 100;
            b = k / c;
            pn = GetPn();
            gn = GetGn();
            cn = GetCn();
            functions = GetFunstions();
        }

        public double GetRoot(double left, double right, double eps)
        {
            double mid = (left + right) / 2;
            while (Math.Abs(right - left) > 2 * eps)
            {
                mid = (left + right) / 2;
                if (F(mid) * F(right) <= 0)
                    left = mid;
                else
                    right = mid;
            }
            return mid;
        }

        public double F(double x) //2.2.10
        {
            return Math.Tan(lx * x / 2) - a / x;
        }

        public List<double> GetPn()
        {
            var result = new List<double>();
            for (int i = 0; i < N; ++i)
            {
                double rAsymp = Math.PI * (2 * i + 1) / lx;
                result.Add(GetRoot(l, rAsymp - d, eps));
                l = rAsymp + d;
            }
            return result;
        }

        public List<double> GetGn()
        {
            var result = new List<double>();
            for (int i = 0; i < N; ++i)
            {
                result.Add((lx / 4) + Math.Sin(pn[i] * lx) / (4 * pn[i]));
            }
            return result;
        }

        public List<double> GetCn()
        {
            var result = new List<double>();
            for (int i = 0; i < N; ++i)
            {
                result.Add(5 * Math.PI * lx * Math.Cos(pn[i] * lx / 2) / (gn[i] * (Math.PI * Math.PI - lx * lx * pn[i] * pn[i])));
            }
            return result;
        }

        public List<Func<double, double, double>> GetFunstions()
        {
            var result = new List<Func<double, double, double>>();
            for (int i = 0; i < N; ++i)
            {
                var c = cn[i];
                var p = pn[i];
                result.Add((z, t) => c * Math.Exp(-b * p * p * t) * Math.Cos(p * z));
            }
            return result;
        }

        public double GetFunctionValue(double z, double t)
        {
            return functions.Select(f => f(z, t)).Sum();
        }        
    }
}
