using System;
using System.Drawing;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        CheckBox[] bores = new CheckBox[10];
        CheckBox[] survives = new CheckBox[10];
        bool[,] data = new bool[200, 100];
        int w = 100, h = 50;
        int[] kx = { 0, 0, 1, -1, 1, 1, -1, -1 };
        int[] ky = { 1, -1, 0, 0, 1, -1, 1, -1 };

        private void MainForm_Load(object sender, EventArgs e)
        {
            for (int i = 0; i <= 8; i++)
            {
                bores[i] = new CheckBox();
                bores[i].Text = i.ToString();
                bores[i].Location = new Point(75 + (30 + 5) * i, 25);
                bores[i].Size = new Size(30, 16);
                this.Controls.Add(bores[i]);
            }
            bores[3].Checked = true;
            for (int i = 0; i <= 8; i++)
            {
                survives[i] = new CheckBox();
                survives[i].Text = i.ToString();
                survives[i].Location = new Point(75 + (30 + 5) * i, 50);
                survives[i].Size = new Size(30, 16);
                this.Controls.Add(survives[i]);
            }
            survives[2].Checked = true;
            survives[3].Checked = true;
        }

        private void 开始暂停ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Enabled = !timer1.Enabled;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            bool[,] temp = new bool[200, 100];
            for (int i = 1; i <= w; i++)
                for (int j = 1; j <= h; j++)
                {
                    int lcnt = 0;
                    for (int k = 0; k < 8; k++)
                        lcnt += data[i + kx[k], j + ky[k]] ? 1 : 0;
                    temp[i, j] = data[i, j] ? survives[lcnt].Checked : bores[lcnt].Checked;
                }
            for (int i = 1; i <= w; i++)
                for (int j = 1; j <= h; j++)
                    data[i, j] = temp[i, j];
            Graphics g = panel1.CreateGraphics();
            g.Clear(Color.White);
            for (int i = 0; i < 100; i++)
                for (int j = 0; j < 50; j++)
                    if (data[i, j])
                        g.FillRectangle(Brushes.Black, i * 10 + 1, j * 10 + 1, 8, 8);
        }

        bool down;

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            down = true;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!down) return;
            try
            {
                if (e.Button == MouseButtons.Left)//左键负责绘制
                    data[e.Location.X / 10, e.Location.Y / 10] = true;
                if (e.Button == MouseButtons.Right) //右键负责擦除
                    data[e.Location.X / 10, e.Location.Y / 10] = false;
            }
            catch { 
                down = false; 
            }
            Graphics g = panel1.CreateGraphics();
            g.Clear(Color.White);
            for (int i = 0; i < 100; i++)
                for (int j = 0; j < 50; j++)
                    if (data[i, j])
                        g.FillRectangle(Brushes.Black, i * 10 + 1, j * 10 + 1, 8, 8);
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            down = false;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            Graphics g = panel1.CreateGraphics();
            g.Clear(Color.White);
        }

        private void panel1_MouseLeave(object sender, EventArgs e)
        {
            down = false;
        }
    }
}
