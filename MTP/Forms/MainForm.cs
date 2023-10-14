using MTP.Classes;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MTP.Forms
{
	public partial class MainForm : Form
	{
		private Button taskButton, actionButton;
		private Form activeForm;
		private Color curColor;
		private bool menuIsHided;

		public MainForm()
		{
			InitializeComponent();
			CustomizeDesign();
			Text = string.Empty;
			ControlBox = false;
			MaximizedBounds = Screen.FromHandle(Handle).WorkingArea;
		}

		[DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
		private extern static void ReleaseCapture();

		[DllImport("user32.DLL", EntryPoint = "SendMessage")]
		private extern static void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

		private void CustomizeDesign()
		{
			WindowState = FormWindowState.Maximized;
			Text = string.Empty;
			ControlBox = false;
			MaximizedBounds = Screen.FromHandle(Handle).WorkingArea;
			menuIsHided = false;
			OpenChildForm(new Drawing(), this);
		}

		private void btnParams_Click(object sender, EventArgs e)
		{
			var InputForm = new InputParams();
			InputForm.Show();
		}

		private void btnCLose_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void panelTitleBar_MouseDown(object sender, MouseEventArgs e)
		{
			ReleaseCapture();
			SendMessage(Handle, 0x112, 0xf012, 0);
		}

		private void OpenChildForm(Form ChildForm, object sender)
		{
			if (activeForm != null) activeForm.Close();
			activeForm = ChildForm;
			ChildForm.TopLevel = false;
			ChildForm.FormBorderStyle = FormBorderStyle.None;
			ChildForm.Dock = DockStyle.Fill;
			panelDesktopPane.Controls.Add(ChildForm);
			panelDesktopPane.Tag = ChildForm;
			ChildForm.BringToFront();
			ChildForm.Show();
			lblTitle.Text = ChildForm.Text;
		}
	}
}
