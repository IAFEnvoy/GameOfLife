using System;
using System.Collections.Generic;
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
        int w = 200, h = 100;
        bool[,] data = new bool[203, 203];
        int[] kx = { 0, 0, 1, -1, 1, 1, -1, -1 };
        int[] ky = { 1, -1, 0, 0, 1, -1, 1, -1 };
        Render render;
        List<Lifes> lifes = new List<Lifes>();

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

            render = new Render(panel1, w, h, 5);

            lifes.Add(new Lifes(Color.Red, 0,"3/23"));
            lifelist.Items.Add("Game Of Life : B3/S23");
        }

        private void 开始暂停ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Enabled = !timer1.Enabled;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            bool[,] temp = new bool[w+3, h+3];
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
            drawMap();
        }
        void drawMap()
        {
            Color[,] color = new Color[w + 1, h + 1];
            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                    if (data[i + 1, j + 1])
                        color[i, j] = Color.Black;
                    else 
                        color[i, j] = Color.White;
            render.Draw(color);
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
                    data[e.Location.X / 5, e.Location.Y / 5] = true;
                if (e.Button == MouseButtons.Right) //右键负责擦除
                    data[e.Location.X / 5, e.Location.Y / 5] = false;
            }
            catch { 
                down = false;
            }
            drawMap();
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

        private void 重新开始ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 100; i++)
                for (int j = 0; j < 50; j++)
                    data[i, j] = false;
            drawMap();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void panel1_MouseLeave(object sender, EventArgs e)
        {
            down = false;
        }
    }
}
