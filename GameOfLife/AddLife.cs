using System;
using System.Drawing;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class AddLife : Form
    {
        CheckBox[] bores = new CheckBox[10];
        CheckBox[] survives = new CheckBox[10];
        public Color c;
        public string keys = string.Empty;
        public string name = string.Empty;
        public AddLife()
        {
            InitializeComponent();
        }

        private void AddLife_Load(object sender, EventArgs e)
        {
            for (int i = 0; i <= 8; i++)
            {
                bores[i] = new CheckBox();
                bores[i].Text = i.ToString();
                bores[i].Location = new Point(75 + (30 + 5) * i, 7);
                bores[i].Size = new Size(30, 16);
                Controls.Add(bores[i]);
            }
            bores[3].Checked = true;
            for (int i = 0; i <= 8; i++)
            {
                survives[i] = new CheckBox();
                survives[i].Text = i.ToString();
                survives[i].Location = new Point(75 + (30 + 5) * i, 32);
                survives[i].Size = new Size(30, 16);
                Controls.Add(survives[i]);
            }
            survives[2].Checked = true;
            survives[3].Checked = true;

            c = Color.Black;
            pictureBox1.BackColor = Color.Black;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Random rd = new Random();
            string s = "", str = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            for (int i = 0; i < 16; i++)
                s += str.Substring(rd.Next(0, str.Length - 1), 1);
            lifename.Text = s;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            ColorDialog color = new ColorDialog();
            color.ShowDialog();
            c = color.Color;
            pictureBox1.CreateGraphics().Clear(c);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            keys = "";
            for (int i = 0; i <= 8; i++)
                keys += bores[i].Checked ? i.ToString() : "";
            keys += "/";
            for (int i = 0; i <= 8; i++)
                keys += survives[i].Checked ? i.ToString() : "";
            name = lifename.Text;
            Close();
        }
    }
}
