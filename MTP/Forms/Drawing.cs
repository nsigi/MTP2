using MTP.Classes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
//отрисовка
namespace MTP.Forms
{
	public partial class Drawing : Form
	{
		public Solver Solver { get; set; }
		public AnaliticSolver analiticSolver { get; set; }
		public Drawing()
		{
			analiticSolver = new AnaliticSolver();
			InitializeComponent();
		}

		public void DrawCov(Chart chart1, Chart chart2)
		{
            double tValue = 20;
			double xValue = 5;

            var Grid = new List<(int I, int K)> { (5, 250) };
            for (int i = 1; i < 6; ++i)
            {
                Grid.Add((Grid[i - 1].I * 2, Grid[i - 1].K * 2));
            }

            for (int j = 0; j < Grid.Count; ++j)
            {
                ParamsHelper.I_x = Grid[j].I;
                ParamsHelper.K_t = Grid[j].K;
				Solver = new Solver();
                chart1.Series[j].Points.Clear();
				chart1.Series[j].LegendText = "I = " + Grid[j].I.ToString() + " K = " + Grid[j].K.ToString();

                chart2.Series[j].Points.Clear();
				chart2.Series[j].LegendText = "I = " + Grid[j].I.ToString() + " K = " + Grid[j].K.ToString();

                for (int i = 0; i < ParamsHelper.I_x + 1; ++i)
                {
                    chart1.Series[j].Points.AddXY(Solver.Xarr[i], Solver.V[(int)(tValue  / Solver.h_t), i]);
                }

                for (int k = 0; k < ParamsHelper.K_t + 1; ++k)
                {
                    chart2.Series[j].Points.AddXY(Solver.Tarr[k], Solver.V[k, (int)(xValue / Solver.h_x)]);
                    //chart2.Series[j].Points.AddXY(Solver.Tarr[k], Solver.V[k, (int)Math.Round(Solver.Xarr[xValues[j]]  * ParamsHelper.I_x)]);
                }
            }

            //аналитическое
            {
                chart1.Series[5].Color = Color.Black;
				chart1.Series[5].LegendText = string.Format("analitic t = {0}", tValue);
				double a = 0, b = ParamsHelper.l_x / 2, h = 0.1, x, y;
				chart1.Series[5].Points.Clear();
				x = b;
				while (x >= a)
				{
					y = analiticSolver.GetFunctionValue(x, tValue);
					chart1.Series[5].Points.AddXY(b - x, y);
					x -= h;
				}
				x = a;
				while (x <= b)
				{
					y = analiticSolver.GetFunctionValue(x, tValue);
					chart1.Series[5].Points.AddXY(b + x, y);
					x += h;
				}
			}
            {
                chart2.Series[5].Color = Color.Black;
				chart2.Series[5].LegendText = string.Format("analitic x = {0}", xValue);
				double a = 0, b = ParamsHelper.T, h = 0.1, x, y;
				chart2.Series[5].Points.Clear();
				x = a;
				while (x <= b)
				{
					y = analiticSolver.GetFunctionValue(xValue - ParamsHelper.l_x / 2, x);
					chart2.Series[5].Points.AddXY(x, y);
					x += h;
				}
			}
		}
        public void DrawDin(Chart chart1, Chart chart2)
		{
			////фиксируем временной слой
			double[] tValues = { 0, 5,  10, 20, 75, 200 };
			for (int j = 0; j < tValues.Length; ++j)
			{

				chart1.Series[j].Points.Clear();
				chart1.Series[j].LegendText = "t = " + tValues[j].ToString();
				var t = (int)(tValues[j]  / Solver.h_t);

				for (int i = 0; i < ParamsHelper.I_x + 1; ++i)
				{
					chart1.Series[j].Points.AddXY(Solver.Xarr[i], Solver.V[t, i]);

				}
			}
			

			//фиксируем точку
			double[] xValues = { 0, 1, 2, 3, 4, 5 };
			for (int j = 0; j < xValues.Length; ++j)
			{
				chart2.Series[j].Points.Clear();
				chart2.Series[j].LegendText = "x = " + xValues[j].ToString();
				for (int k = 0; k < ParamsHelper.K_t + 1; ++k)
				{
					chart2.Series[j].Points.AddXY(Solver.Tarr[k], Solver.V[k, (int)(xValues[j]  / Solver.h_x)]);
					//chart2.Series[j].Points.AddXY(Solver.Tarr[k], Solver.V[k, (int)Math.Round(Solver.Xarr[xValues[j]]  * ParamsHelper.I_x)]);
				}
			}
			
		}

		private void btnDraw_Click(object sender, EventArgs e)
		{
			{
				Solver = new Solver();
				if (Solver.Stability)
				{
					Axis ay1 = new Axis
					{
						Title = "Температура v, °C"
					};
					Axis ay2 = new Axis
					{
						Title = "Температура v, °C"
					};
					Axis ax1 = new Axis
					{
						Title = "Время t, c"
					};
					Axis ax2 = new Axis
					{
						Title = "Координата x, см"
					};

					ay1.TitleFont = ay2.TitleFont = ax1.TitleFont = ax2.TitleFont = new System.Drawing.Font("Microsoft Sans Serif",
						18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);

					SetChart(chart1, ax2, ay2);
					chart1.ChartAreas[0].AxisX.Minimum = 5;
					chart1.ChartAreas[0].AxisX.Maximum = ParamsHelper.l_x;
					SetChart(chart2, ax1, ay1);
					chart2.ChartAreas[0].AxisX.Minimum = 0.0;
					chart2.ChartAreas[0].AxisX.Maximum = ParamsHelper.T;

					//DrawDin(chart1, chart2);
					DrawCov(chart1, chart2);
				}
				else
				{
					MessageBox.Show("Неустойчиво при заданных I и K");
				}
			}

			ErrorSolver errorSolver = new ErrorSolver(analiticSolver);
			foreach (var item in errorSolver.GetSolves())
			{
				Console.WriteLine(item);
			}

		}

		public void SetChart(Chart chart, Axis ax, Axis ay)
		{
			chart.ChartAreas[0].AxisX = ax;
			chart.ChartAreas[0].AxisY = ay;
			//chart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
			//chart.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
			chart.ChartAreas[0].AxisX.LabelStyle.Format = "0";
		}
	}
}