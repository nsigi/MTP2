using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTP.Classes
{
    public class ErrorSolver
    {
        AnaliticSolver anSolver { get; set; }
        Solver solver { get; set; }
        List<(int I, int K)> Grid { get; set; }
        public ErrorSolver(AnaliticSolver analiticSolver)
        {
            anSolver = analiticSolver;
            Grid = new List<(int, int)> { (5, 250) };
            for (int i = 1; i < 6; ++i)
            {
                Grid.Add((Grid[i - 1].I * 2, Grid[i - 1].K * 2));
            }
        }

        public double GetZ(double x)
        {
            return x - ParamsHelper.l_x / 2;
        }

        public List<double> GetSolves()
        {
            var results = new List<double>();
            for (int c = 0; c < Grid.Count; ++c)
            {
                ParamsHelper.I_x = Grid[c].I;
                ParamsHelper.K_t = Grid[c].K;
                solver = new Solver();
                var max = 0.0;
                for (int i = (int)ParamsHelper.I_x / 2; i < (int)ParamsHelper.I_x + 1; ++i) // i = I/2, I
                {
                    for (int k = 0; k < ParamsHelper.K_t + 1; ++k) // k = 1,K  
                    {
                        var diff = anSolver.GetFunctionValue(GetZ(solver.Xarr[i]), solver.Tarr[k]) - solver.V[k, i];
                        var diffAbs = Math.Abs(diff);
                        if (diffAbs > max)
                            max = diffAbs;
                    }

                }
                results.Add(max);
            }
            for (int i = 1; i < results.Count; ++i)
            {
                Console.WriteLine((results[i - 1] / results[i]).ToString());
            }
            Console.WriteLine();
            return results;
        }

    }
}
