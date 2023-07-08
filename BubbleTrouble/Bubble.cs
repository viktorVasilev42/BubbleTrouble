using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleTrouble
{
    public class Bubble
    {
        public Point Center { get; set; }
        public int Radius { get; set; }
        public Color Color { get; set; }
        public Point Direction { get; set; }

        public Bubble(Point center, int radius, Color color) 
        {
            this.Center = center;
            this.Radius = radius;
            this.Color = color;
            this.Direction = new Point(1, 5);
        }

        public Bubble(Point center, int radius, Color color, int dx, int dy)
        {
            this.Center = center;
            this.Radius = radius;
            this.Color = color;
            this.Direction = new Point(dx, dy);
        }

        public void Draw(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color), Center.X - Radius, Center.Y - Radius, 2 * Radius, 2 * Radius);
        }

        public void Move()
        {
            Center = new Point(Center.X + Direction.X, Center.Y + Direction.Y);
        }

        public void groundSwitch()
        {
            Direction = new Point(Direction.X, Direction.Y * (-1));
        }

        public void ceilSwitch()
        {
            if (Direction.Y == 5) return;
            Direction = new Point(Direction.X, Direction.Y + 1);
        }

        public void wallSwitch()
        {
            Direction = new Point(Direction.X * (-1), Direction.Y);
        }

        public Rectangle getBounds()
        {
            return new Rectangle(Center.X - Radius, Center.Y - Radius, 2 * Radius, 2 * Radius);
        }

        public List<Bubble> Split()
        {
            if (Radius < 10) return new List<Bubble>();
            
            Bubble one = new Bubble(this.Center, this.Radius / 2, this.Color, 1, -5);
            Bubble two = new Bubble(this.Center, this.Radius / 2, this.Color, -1, -5);
            return new List<Bubble> { one, two };
        }
    }
}
