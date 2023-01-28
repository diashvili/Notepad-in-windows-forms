using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace G15_Notepad
{
	public partial class Form1 : Form
	{
		private bool _isModified = false;

		private string FilePath { get; set; }

		public Form1()
		{
			InitializeComponent();
			lblStatus.Text = GetStatusInfo();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void toolbarToolStripMenuItem_Click(object sender, EventArgs e)
		{
			toolStrip1.Visible = !toolStrip1.Visible;
		}

		private void backgroundToolStripMenuItem_Click(object sender, EventArgs e)
		{
			dlgColor.Color = txtContent.BackColor;
			if (dlgColor.ShowDialog() == DialogResult.OK)
			{
				txtContent.BackColor = dlgColor.Color;
			}
		}

		private void newToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!ConfirmSave())
			{
				return;
			}
			txtContent.Clear();
			FilePath = null;
			_isModified = false;
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!ConfirmSave())
			{
				return;
			}
			dlgOpen.FileName = string.Empty;
			if (dlgOpen.ShowDialog() == DialogResult.OK)
			{
				try
				{
					txtContent.Text = File.ReadAllText(dlgOpen.FileName);
					FilePath = dlgOpen.FileName;
					_isModified = false;
				}
				catch (Exception exception)
				{
					MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFile();
		}

		private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFile(true);
		}

		private bool SaveFile(bool isSaveAs = false)
		{
			if (FilePath == null || isSaveAs)
			{
				dlgSave.FileName = FilePath;
				if (dlgSave.ShowDialog() != DialogResult.OK)
				{
					return false;
				}
				FilePath = dlgSave.FileName;
			}

			try
			{
				File.WriteAllText(FilePath, txtContent.Text);
				_isModified = false;
				return true;
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
		}

		private void txtContent_TextChanged(object sender, EventArgs e)
		{
			if (!_isModified)
			{
				_isModified = true;
			}

			lblStatus.Text = GetStatusInfo();
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!ConfirmSave())
			{
				e.Cancel = true;
			}
		}

		private bool ConfirmSave()
		{
			if (!_isModified)
			{
				return true;
			}

			DialogResult result = MessageBox.Show(
				"Do you want to save changes?",
				"Confirmation",
				MessageBoxButtons.YesNoCancel,
				MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button3);

			switch (result)
			{
				case DialogResult.Yes:
					return SaveFile();
				case DialogResult.No:
					return true;
				default:
					return false;
			}
		}

		private string GetStatusInfo()
		{
			int charCount = txtContent.TextLength;
			return $"Ready...\tCharecters Count: {charCount}";
		}
	}
}
