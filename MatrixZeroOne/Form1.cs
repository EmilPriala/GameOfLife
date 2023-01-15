using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MatrixZeroOne
{
    public partial class Form1 : Form
    {
        Graphics g;
        Bitmap b;
        List<int> data = new List<int>();
        int m, n;
        int[,] a;
        int steps = 0;
        public void LoadFile(string fs)
        {
            TextReader load = new StreamReader(fs);
            string buffer;
            List<string> T = new List<string>();
            while ((buffer = load.ReadLine()) != null)
            {
                T.Add(buffer);
            }
            load.Close();
            n = T.Count;
            m = (T[0].Split(' ')).Length;
            a = new int [n, m];
            for (int i = 0; i < n; i++) // CITIM VALORILE DIN LISTA IN MATRICE
            {
                string[] l = T[i].Split(' ');
                for (int j = 0; j < m; j++)
                {
                    a[i, j] = int.Parse(l[j]);
                }
            }

        }
         public void Draw()
         {
            g.Clear(Color.Black);
            float dx = (float)pictureBox1.Width / m;
            float dy = (float)pictureBox1.Height / n;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    if (a[i, j] == 0)
                        g.FillRectangle(Brushes.White, j * dx, i * dy, dx, dy);
                    else
                        g.FillRectangle(Brushes.MediumPurple, j * dx, i * dy, dx, dy);
                    g.DrawRectangle(Pens.Black, j * dx, i * dy, dx, dy);
                }
            }
            
         }
        public void Initialise()
        {
            b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(b);
            timer1.Start();
        }
        public int Count()
        {
            int nr = 0;
            for (int i = 0;i<n;i++)
                for (int j = 0; j <m; j++)
                {
                    if (a[i, j] == 1) nr++;
                }
            return nr;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Initialise();
            LoadFile("input.txt");
            Draw();
            pictureBox1.Image = b;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled == true)
                timer1.Enabled = false;
            else
                timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            data.Add(Count());
            TickChange();
            steps++;
            Draw();
            pictureBox1.Image = b;
            if (steps == 100)
            { 
                timer1.Enabled = false;
                Save(@"Dataou.txt");
            }
        }

        private void Save(string fn)
        {
            TextWriter f = new StreamWriter(fn);
            foreach(int i in data)
            {
                f.WriteLine(i);
            }
        }

        public void TickChange()
        {
            int[,] M = new int[n, m];
            int s;
            for (int i = 0; i < n; i++)
            {
                s = 0;
                /* i - 1, j-1 ; i-1,j; i-1, j+1
                 * i,j-1 ; i , j+1; 
                 * i+1, j-1; i+1, j ; i+1 , j+1;
                 * */
                for (int j = 0; j < m; j++)
                {
                    //M = Conditia1(i,j,M);
                    //M = Conditia2(i, j, M);
                    M = Conditia3(i, j, M);

                }
            }
            a = M;

        }
        public int[,] Conditia3(int i, int j, int[,] M)
        {
            int s = 0;
            //PRIMA LINIE
            if (i - 1 >= 0 && j - 1 >= 0) if (a[i - 1, j - 1] == 1) s++;
            if (i - 1 >= 0 && j >= 0) if (a[i - 1, j] == 1) s++;
            if (i - 1 >= 0 && j + 1 < m) if (a[i - 1, j + 1] == 1) s++;
            //LINIA MIJLOC
            if (i >= 0 && j - 1 >= 0) if (a[i, j - 1] == 1) s++;
            if (i >= 0 && j + 1 < m) if (a[i, j + 1] == 1) s++;
            //ULTIMA LINIE
            if (i + 1 < n && j - 1 >= 0) if (a[i + 1, j - 1] == 1) s++;
            if (i + 1 < n && j >= 0) if (a[i + 1, j] == 1) s++;
            if (i + 1 < n && j + 1 < m) if (a[i + 1, j + 1] == 1) s++;
            if (a[i, j] == 1)
            {
                if (s < 2)
                    M[i, j] = 0;
                if (s == 2 || s == 3)
                    M[i, j] = 1;
                if (s > 3)
                    M[i, j] = 0;
            }
            else
                if (s == 3) M[i, j] = 1;
            return M;
        }
        public int[,] Conditia2(int i, int j, int[,] M)
        {
            int s = 0;
            //PRIMA LINIE
            if (i - 1 >= 0 && j - 1 >= 0) if (a[i - 1, j - 1] == 1) s++;
            if (i - 1 >= 0 && j >= 0) if (a[i - 1, j] == 1) s++;
            if (i - 1 >= 0 && j + 1 < m) if (a[i - 1, j + 1] == 1) s++;
            //LINIA MIJLOC
            if (i >= 0 && j - 1 >= 0) if (a[i, j - 1] == 1) s++;
            if (i >= 0 && j + 1 < m) if (a[i, j + 1] == 1) s++;
            //ULTIMA LINIE
            if (i + 1 < n && j - 1 >= 0) if (a[i + 1, j - 1] == 1) s++;
            if (i + 1 < n && j >= 0) if (a[i + 1, j] == 1) s++;
            if (i + 1 < n && j + 1 < m) if (a[i + 1, j + 1] == 1) s++;

            if (s != 0)
                M[i, j] = 1;
            else
                M[i, j] = 0;
            return M;
        }
        public int[,] Conditia1(int i, int j, int[,] M)
        {
            int s = 0;
            //PRIMA LINIE
            if (i - 1 >= 0 && j - 1 >= 0) if (a[i - 1, j - 1] == 1) s++;
            if (i - 1 >= 0 && j >= 0) if (a[i - 1, j] == 1) s++;
            if (i - 1 >= 0 && j + 1 < m) if (a[i - 1, j + 1] == 1) s++;
            //LINIA MIJLOC
            if (i >= 0 && j - 1 >= 0) if (a[i, j - 1] == 1) s++;
            if (i >= 0 && j + 1 < m) if (a[i, j + 1] == 1) s++;
            //ULTIMA LINIE
            if (i + 1 < n && j - 1 >= 0) if (a[i + 1, j - 1] == 1) s++;
            if (i + 1 < n && j >= 0) if (a[i + 1, j] == 1) s++;
            if (i + 1 < n && j + 1 < m) if (a[i + 1, j + 1] == 1) s++;

            if (s % 2 == 0)
                M[i, j] = 0;
            else
                M[i, j] = 1;
            return M;
        }

    }
    
}
