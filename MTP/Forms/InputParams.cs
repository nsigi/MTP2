using MTP.Classes;
using System;
using System.Windows.Forms;

namespace MTP.Forms
{
	public partial class InputParams : Form
	{
		public InputParams()
		{
			InitializeComponent();
			tb_T.Text = ParamsHelper.T.ToString(); //200
			tb_lx.Text = ParamsHelper.l_x.ToString();//"10"
			tb_a.Text = ParamsHelper.Alpha.ToString();//"0,004"
			tb_k.Text = ParamsHelper.k.ToString(); //"0,13";
			tb_c.Text = ParamsHelper.c.ToString(); //"1,84";
			tb_u0.Text = ParamsHelper.u_0.ToString(); //"0";
            tb_I.Text = ParamsHelper.I_x.ToString(); //"100";
            tb_KK.Text = ParamsHelper.K_t.ToString(); //"200";
        }

		private void btnConfirm_Click(object sender, EventArgs e)
		{
			ParamsHelper.T = double.Parse(tb_T.Text);
            ParamsHelper.l_x = double.Parse(tb_lx.Text);
            ParamsHelper.Alpha = double.Parse(tb_a.Text);
            ParamsHelper.k = double.Parse(tb_k.Text);
            ParamsHelper.c = double.Parse(tb_c.Text);
            ParamsHelper.u_0 = double.Parse(tb_u0.Text);
            ParamsHelper.I_x = int.Parse(tb_I.Text);
            ParamsHelper.K_t = int.Parse(tb_KK.Text);
			Close();
		}

		private void btnDecline_Click(object sender, EventArgs e)
		{
			Close();
		}
    }
}
