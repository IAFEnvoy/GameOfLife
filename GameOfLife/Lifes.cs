using System.Drawing;

namespace GameOfLife
{
    class Lifes
    {
        string name;
        bool[] life = new bool[10];
        bool[] survive = new bool[10];
        Color rendercolor;
        int index;
        public Lifes(string name,Color color, int key, string args)
        {
            this.name = name;
            rendercolor = color;
            index = key;
            string[] data = args.Split('/');
            foreach (char c in data[0])
                life[c - '0'] = true;
            foreach (char c in data[1])
                survive[c - '0'] = true;
        }
        public Color GetColor() { return rendercolor; }
        public int GetKey() { return index; }
        public bool GetBore(int key) { return life[key]; }
        public bool GetSurvive(int key) { return survive[key]; }

    }
}
