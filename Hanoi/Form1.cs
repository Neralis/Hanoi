using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Hanoi
{
    public partial class Form1 : Form
    {
        public static List<Button> takeButtons;
        public static List<Button> putButtons;
        public static List<Tower> towers;
        public static PictureBox temp;
        public static int steps;
        private void Form1_Load(object sender, EventArgs e)
        { 
        
        }

        public class Tower
        {
            public int placeIndex = 0;
            public Point placeCenterPoint;
            public List<PictureBox> rings = new List<PictureBox>();

            public Tower(int place)
            {
                this.placeIndex = place;
                this.placeCenterPoint = new Point(85 + placeIndex * 170, 282);
            }

            public void Paint()
            {
                int j = 0;

                foreach (PictureBox ring in rings)
                {
                    int i = ring.Size.Width / 50;
                    int x = placeCenterPoint.X - 25 * i;
                    int y = placeCenterPoint.Y - 48 * j;

                    ring.Location = new Point(x, y);
                    j++;
                }
            }

            public void Take()
            {
                PictureBox takedRing = rings.Last();
                rings.RemoveAt(rings.Count - 1);
                takedRing.Top = 100;

                temp = takedRing;

                CheckTowersPut();
            }
        }

        //Проверка можно ли взять кольцо
        public static void CheckTowersTake()
        {
            for (int i = 0; i < 3; i++)
            {
                if (towers[i].rings.Count > 0)
                {
                    takeButtons[i].Visible = true;
                }
            }
        }

        //Проверка можно ли положить кольцо
        public static void CheckTowersPut()
        {
            for (int i = 0; i < 3; i++)
            {
                if (towers[i].rings.Count == 0 || temp.Width < towers[i].rings.Last().Width)
                {
                    putButtons[i].Visible = true;
                }
            }
        }

        public Form1()
        {
            InitializeComponent();

            takeButtons = new List<Button>() { takeButton1, takeButton2, takeButton3 };
            putButtons = new List<Button>() { putButton1, putButton2, putButton3 };
            towers = new List<Tower>() { new Tower(0), new Tower(1), new Tower(2) };

            //Создание колец при инициализации формы
            Color[] ringColors = { Color.DarkViolet, Color.DarkRed, Color.DarkCyan };

            for (int i = 3; i > 0; i--)
            {
                PictureBox ring = new PictureBox();
                ring.Name = i.ToString();
                ring.BackColor = ringColors[i - 1];
                ring.Size = new Size(50 * i - 10, 30);
                ring.Visible = true;
                ring.Enabled = true;

                this.Controls.Add(ring);
                towers[0].rings.Add(ring);
            }

            CheckTowersTake();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //Создание башен
            Graphics g;
            g = CreateGraphics();
            Pen p = new Pen(Color.Black, 10);
            g.DrawRectangle(p, 100, 155, 10, 150);
            g.DrawRectangle(p, 270, 155, 10, 150);
            g.DrawRectangle(p, 440, 155, 10, 150);

            foreach (Tower tower in towers)
            {
                tower.Paint();
            }
        }

        private void takeButtons_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;

            int index = takeButtons.FindIndex(b => b == button);

            towers[index].Take();

            foreach (Button takeButton in takeButtons)
            {
                takeButton.Visible = false;
            }

            this.Refresh();
        }

        private void putButtons_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;

            int index = putButtons.FindIndex(b => b == button);

            towers[index].rings.Add(temp);

            foreach (Button putButton in putButtons)
            {
                putButton.Visible = false;
            }

            CheckTowersTake();

            this.Refresh();

            steps++;

            if (towers[2].rings.Count == 3)
            {
                MessageBox.Show("Вы решили головоломку 'Ханойская башня' за " + steps + " действий");

                for (int i = 0; i < 3; i++)
                {
                    takeButtons[i].Visible = false;
                    putButtons[i].Visible = false;
                }

            }
        }
    }
}
