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

		private void ReturnToTask()
		{
			if (actionButton != null)
				actionButton = null;
			lblTitle.Text = taskButton.Text;
		}

		private void Reset()
		{
			lblTitle.Text = "Моделирование теплового процесса";
			panelTitleBar.BackColor = Color.FromArgb(0, 150, 136);
			panelLogo.BackColor = Color.FromArgb(39, 39, 58);
		}

		private void SelectTaskButton(object sender)
		{
			if (taskButton == null)
			{
				taskButton = sender as Button;
			}
			else
			{
				if (taskButton == sender)
				{
					ReturnToTask();
					taskButton = null;
					Reset();
					if (activeForm != null)
					{
						activeForm.Close();
						activeForm = null;
					}
				}
				else if (taskButton != sender)
				{
					ReturnToTask();
					taskButton = sender as Button;
					if (activeForm != null)
					{
						activeForm.Close();
						activeForm = null;
					}
				}
			}
		}

		private void btnParams_Click(object sender, EventArgs e)
		{
			var InputForm = new InputParams();
			InputForm.Show();
		}

		private void btnDraw_Click(object sender, EventArgs e)
		{
			OpenChildForm(new Drawing(), sender);
		}
		private void btnRollUp_Click(object sender, EventArgs e)
		{
			if (menuIsHided)
			{
				panelMenu.Visible = true;
				menuIsHided = false;
			}
			else
			{
				panelMenu.Visible = false;
				menuIsHided = true;
			}
		}

		private void btnMinimaze_Click(object sender, EventArgs e)
		{
			WindowState = FormWindowState.Minimized;
		}

		private void btnMaximize_Click(object sender, EventArgs e)
		{
			if (WindowState == FormWindowState.Normal)
			{
				WindowState = FormWindowState.Maximized;
			}
			else
			{
				WindowState = WindowState = FormWindowState.Normal;
			}
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

        private void panelTitleBar_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CheckButtons()
		{
			actionButton = null;
			taskButton = null;
			Reset();
		}
	}
}
