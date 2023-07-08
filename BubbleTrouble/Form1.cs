using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BubbleTrouble
{
    public partial class Form1 : Form
    {
        private Player player;
        private List<Bubble> bubbles;
        private int bubbleTimerCount = 0;
        private int currLevel = 0;
        private int numLevels = 2;

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            this.player = new Player(Color.Blue, this);
            this.arrowTimer.Interval = 15;
            this.bubbles = new List<Bubble>();
            this.bubbleTimer.Interval = 15;
            this.bubbleTimer.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void arrowTimer_Tick(object sender, EventArgs e)
        {
            if (player.Arrow.Y <= 0)
            {
                arrowTimer.Stop();
                player.ResetArrow();
                return;
            }
            player.ArrowRise();
            Invalidate();
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            bubbles.Add(new Bubble(e.Location, 30, Color.Red));
        }

        private void bubbleTimer_Tick(object sender, EventArgs e)
        {
            List<Bubble> toDelete = new List<Bubble>();
            List<Bubble> toAdd = new List<Bubble>();
            foreach (Bubble b in bubbles)
            {
                if (b.Center.Y >= player.Location.Y) b.groundSwitch();
                if (b.Center.X <= 30 || b.Center.X >= this.Width - 30) b.wallSwitch();
                if (b.Center.Y < 100 && bubbleTimerCount % 5 == 0) b.ceilSwitch();

                b.Move();

                if (player.Arrow.Height != 0 && b.getBounds().IntersectsWith(player.Arrow))
                {
                    toDelete.Add(b);
                    toAdd.AddRange(b.Split());
                    player.ResetArrow();
                    arrowTimer.Stop();
                }
                if (b.getBounds().IntersectsWith(player.getBounds()))
                {
                    bubbles = new List<Bubble>();
                    player.ResetArrow();
                    arrowTimer.Stop();
                    MessageBox.Show("Game over");
                    currLevel = 0;
                    return;
                }
            }
            bool hadAny = bubbles.Any();
            toAdd.ForEach(b => bubbles.Add(b));
            toDelete.ForEach(b => bubbles.Remove(b));
            if (currLevel != 0 && hadAny && !bubbles.Any())
            {
                currLevel += 1;
                loadLevel(currLevel);
            }
            bubbleTimerCount = (bubbleTimerCount + 1) % 100;
            Invalidate();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            loadLevel(1);
            currLevel = 1;
        }

        private async void loadLevel(int level)
        {
            this.bubbles = new List<Bubble>();
            if (level > numLevels)
            {
                currLevel = 0;
                MessageBox.Show("Bravo! You completed the game!");
                return;
            }

            string[] lines;
            switch (level)
            {
                case 1: lines = Properties.Resources.Level1.Split('\n'); break;
                case 2: lines = Properties.Resources.Level2.Split('\n'); break;
                default: lines = new string[0]; break;
            }

            foreach (string line in lines)
            {
                string[] lineData = line.Split(',');
                Debug.WriteLine(lineData[3]);
                Point center = new Point(Int32.Parse(lineData[0]), Int32.Parse(lineData[1]));
                int radius = Int32.Parse(lineData[2]);
                Color color;
                switch (lineData[3])
                {
                    case "red": color = Color.Red; break;
                    case "green": color = Color.Green; break;
                    case "yellow": color = Color.Yellow; break;
                    default: color = Color.Black; break;
                }

                bubbles.Add(new Bubble(center, radius, color));
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            player.Draw(e.Graphics);
            foreach (Bubble b in bubbles)
            {
                b.Draw(e.Graphics);
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                player.Move(-5);
            }
            else if (e.KeyCode == Keys.Right)
            {
                player.Move(5);
            }
            else if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Up)
            {
                arrowTimer.Start();
            }
            Invalidate();
        }
    }
}
