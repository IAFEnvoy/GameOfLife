using System.Drawing;
using System.Windows.Forms;

namespace GameOfLife
{
    class Render
    {
        private Graphics g;
        private Color[,] colortemp;
        private readonly int width, height, size;

        public Render(Control c, int width, int height, int size)
        {
            g = c.CreateGraphics();
            g.Clear(Color.White);
            colortemp = new Color[width + 1, height + 1];
            this.width = width;
            this.height = height;
            this.size = size;
        }
        public void Draw(Color[,] data)
        {
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    if (data[i, j] != colortemp[i, j])
                    {
                        colortemp[i, j] = data[i, j];
                        g.FillRectangle(new SolidBrush(colortemp[i, j]), i * size + 1, j * size + 1, size - 2, size - 2);
                    }
        }
    }
}
