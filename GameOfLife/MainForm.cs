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
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);
        }
        static int w = 200, h = 100;
        static int[,] data = new int[203, 203];
        static int[,] temp = new int[203, 203];
        static int[] kx = { 0, 0, 1, -1, 1, 1, -1, -1 };
        static int[] ky = { 1, -1, 0, 0, 1, -1, 1, -1 };
        static Render render;
        static List<Lifes> lifes = new List<Lifes>();
        static bool cooperate = false;

        private void MainForm_Load(object sender, EventArgs e)
        {
            for (int i = 0; i <= w + 1; i++)
                for (int j = 0; j <= h + 1; j++)
                    data[i, j] = -1;

            render = new Render(panel1, w, h, 5);

            lifes.Add(new Lifes("Game Of Life", Color.Red, 0, "3/23"));
            lifelist.Items.Add("Game Of Life : B3/S23");
            lifes.Add(new Lifes("Wall", Color.Blue, 1, "45678/2345"));
            lifelist.Items.Add("Wall : B45678/S2345");
            lifes.Add(new Lifes("Replicator", Color.Green, 2, "1357/1357"));
            lifelist.Items.Add("Replicator : B1357/S1357");
        }

        private void 开始暂停ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Enabled = !timer1.Enabled;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 1; i <= w; i++)
                for (int j = 1; j <= h; j++)
                {
                    int[] lcnt = new int[lifes.Count + 2]; int total = 0;
                    for (int k = 0; k < 8; k++)
                        if (data[i + kx[k], j + ky[k]] >= 0)
                        {
                            total++;
                            lcnt[data[i + kx[k], j + ky[k]]] += 1;
                        }
                    if (data[i, j] >= 0)//原格子有生命
                        temp[i, j] = lifes[data[i, j]].GetSurvive(cooperate ? total : lcnt[data[i, j]]) ? data[i, j] : -1;
                    else//无生命
                    {
                        int max = -1, index = -1; bool flag = true;
                        for (int k = 0; k < lifes.Count; k++)
                        {
                            if (!lifes[k].GetBore(cooperate ? total : lcnt[k])) continue;
                            if (lcnt[k] == 0) continue;
                            if (lcnt[k] == max) { flag = false; break; }
                            if (lcnt[k] > max) { max = lcnt[k]; index = k; }
                        }
                        temp[i, j] = (flag && index >= 0) ? index : -1;
                    }
                }
            for (int i = 1; i <= w; i++)
                for (int j = 1; j <= h; j++)
                    data[i, j] = temp[i, j];
            DrawMap();
        }
        void DrawMap()
        {
            Color[,] color = new Color[w + 1, h + 1];
            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                    if (data[i + 1, j + 1] >= 0)
                        color[i, j] = lifes[data[i + 1, j + 1]].GetColor();
                    else
                        color[i, j] = Color.White;
            render.Draw(color);
        }
        bool down;
        MouseEventArgs mea;

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            down = true;
            mea = e;
            timer2.Enabled = true;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            mea = e;
            timer2_Tick(sender, new EventArgs());
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            down = false;
            mea = e;
            timer2.Enabled = false;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            Graphics g = panel1.CreateGraphics();
            g.Clear(Color.White);
        }

        private void 重新开始ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= w + 1; i++)
                for (int j = 0; j <= h + 1; j++)
                    data[i, j] = -1;
            DrawMap();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (!down) return;
            try
            {
                if (mea.Button == MouseButtons.Left)//左键负责绘制
                    if (lifelist.SelectedIndex >= 0)
                        data[mea.Location.X / 5, mea.Location.Y / 5] = lifelist.SelectedIndex;
                if (mea.Button == MouseButtons.Right) //右键负责擦除
                    data[mea.Location.X / 5, mea.Location.Y / 5] = -1;
            }
            catch
            {
                down = false;
            }
            DrawMap();
        }
        public static bool flag = false;
        public static string newkeys = "";
        public static string newname = "";
        public static Color newcolor;
        private void 新建生命ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = new AddLife();
            form.ShowDialog();
            if (flag)
            {
                lifes.Add(new Lifes(newname, newcolor, lifes.Count, newkeys));
                lifelist.Items.Add(newname + " : B" + newkeys.Split('/')[0] + "/S" + newkeys.Split('/')[1]);
            }
            form.Close();
            form.Dispose();
            GC.Collect();
        }

        private void panel1_MouseLeave(object sender, EventArgs e)
        {
            down = false;
        }
    }
}
