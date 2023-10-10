using MTP.Classes;
using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MTP.Forms
{
	public partial class Drawing : Form
	{
		public Solver Solver { get; set; }
		public Drawing()
		{
			InitializeComponent();
		}
		public void Draw1(Chart chart1, Chart chart2)
		{
			////фиксируем временной слой
			int[] tValues = { 0, 5, 20, 75, 200 };
			for (int j = 0; j < tValues.Length; ++j)
			{
				chart1.Series[j].Points.Clear();
				chart1.Series[j].LegendText = "t = " + tValues[j].ToString();
				for (int i = 0; i < ParamsHelper.I_x + 1; ++i)
				{
					chart1.Series[j].Points.AddXY(Solver.Xarr[i], Solver.V[tValues[j], i]);
				}
			}

			//фиксируем точку
			int[] xValues = { 0, 1, 3, 5, 10 };
			for (int j = 0; j < xValues.Length; ++j)
			{
				chart2.Series[j].Points.Clear();
				chart2.Series[j].LegendText = "x = " + xValues[j].ToString();
				for (int k = 0; k < ParamsHelper.K_t + 1; ++k)
				{
					chart2.Series[j].Points.AddXY(Solver.Tarr[k], Solver.V[k, xValues[j]]);
				}
			}

			var c = 1;
		}

		private void btnDraw_Click(object sender, EventArgs e)
		{
			Solver = new Solver();

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
				15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
	
			SetChart(chart1, ax2, ay2);
			SetChart(chart2, ax1, ay1);
			//chart1.ChartAreas[0].AxisX = ax;
			//chart1.ChartAreas[0].AxisY = ay;
			//chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
			//chart1.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
			//chart1.ChartAreas[0].AxisX.LabelStyle.Format = "0";

			//chart2.ChartAreas[0].AxisX = ax;
			//chart2.ChartAreas[0].AxisY = ay;
			//chart2.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
			//chart2.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
			//chart2.ChartAreas[0].AxisX.LabelStyle.Format = "0";

			Draw1(chart1, chart2);
		}

		public void SetChart(Chart chart, Axis ax, Axis ay)
		{
            chart.ChartAreas[0].AxisX = ax;
            chart.ChartAreas[0].AxisY = ay;
            chart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            chart.ChartAreas[0].AxisX.LabelStyle.Format = "0";
        }
	}
}
