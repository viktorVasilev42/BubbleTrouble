using BubbleTrouble;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleTroubleTest
{
    public class Player
    {
        public Color Color { get; set; }
        public Point Location { get; set; }
        public Rectangle Arrow { get; set; }

        public Player(Color color, Form1 form1)
        {
            this.Color = color;
            this.Location = new Point(form1.Width / 2 - 20, form1.Height - 80);
            ResetArrow();
        }

        public void Draw(Graphics g)
        {
            g.FillRectangle(new SolidBrush(Color.LightGray), Arrow);
            //g.FillRectangle(new SolidBrush(Color), this.Location.X, this.Location.Y - 30, 30, 50);
            g.DrawImage(BubbleTrouble.Properties.Resources.cannon, this.Location.X, this.Location.Y - 30, 30, 50);
        }

        public void Move(int dx)
        {
            this.Location = new Point(this.Location.X + dx, this.Location.Y);
        }

        public void ArrowRise()
        {
            if (Arrow.Height == 0) ResetArrow();
            Arrow = new Rectangle(Arrow.X, Arrow.Y - 5, 2, Location.Y - Arrow.Y + 5);
        }

        public void ResetArrow()
        {
            this.Arrow = new Rectangle(Location.X + 10, Location.Y, 2, 0);
        }

        public Rectangle getBounds()
        {
            return new Rectangle(Location.X, Location.Y - 30, 30, 50);
        }
    }
}
