using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace SchetsEditor
{

    public abstract class Tekening
    {
        public Point startpunt;
        public Pen pen;

        public abstract void Teken(Graphics g);
        public abstract bool isAtPoint(Point p);

        public override string ToString()
        {
            return startpunt.X + ", " + startpunt.Y;
        }

    }


    public class TekstTekening : Tekening
    {
        public String tekst;

        public TekstTekening(Point p1, Pen pen, String tekst)
        {
            this.startpunt = p1;
            this.pen = pen;
            this.tekst = tekst;
        }

        public override void Teken(Graphics g)
        {

        }

        public override bool isAtPoint(Point p)
        {
            return false; // TODO
        }
    }

    public class VierkantTekening : Tekening
    {
        public Point eindpunt;

        public VierkantTekening(Point p1, Point p2, Pen pen)
        {
            this.startpunt = p1;
            this.eindpunt = p2;
            this.pen = pen;
        }


        public override void Teken(Graphics g)
        {
            g.DrawRectangle(pen, TweepuntTool.Punten2Rechthoek(this.startpunt, this.eindpunt));
        }

        public override bool isAtPoint(Point p)
        {
            return TweepuntTool.Punten2Rechthoek(this.startpunt, this.eindpunt).Contains(p);
        }
    }

    public class VolVierkantTekening : VierkantTekening
    {
        public VolVierkantTekening(Point p1, Point p2, Pen pen) : base(p1, p2, pen)
        {

        }

        public override void Teken(Graphics g)
        {
            g.FillRectangle(pen.Brush, TweepuntTool.Punten2Rechthoek(this.startpunt, this.eindpunt));
        }
    }

    public class LijnTekening : Tekening
    {
        public Point eindpunt;

        public LijnTekening(Point p1, Point p2, Pen pen)
        {
            this.startpunt = p1;
            this.eindpunt = p2;
            this.pen = pen;
        }

        public override void Teken(Graphics g)
        {
            g.DrawLine(pen, this.startpunt, this.eindpunt);
        }

        public override bool isAtPoint(Point p)
        {
            int xMin = Math.Min(startpunt.X, eindpunt.X);
            int xMax = Math.Max(startpunt.X, eindpunt.X);
            int yMin = Math.Min(startpunt.Y, eindpunt.Y);
            int yMax = Math.Max(startpunt.Y, eindpunt.Y);

            // ax + b = y
            double a = ((double) (startpunt.Y - eindpunt.Y)) / (startpunt.X - eindpunt.X);
            // ax + b = y  -->  b = y - ax
            double b = startpunt.Y - a * startpunt.X;

            Debug.WriteLine("Hello there.");
            Debug.WriteLine(Math.Abs(a * p.X + b - p.Y));
            return (p.X >= xMin) && (p.X <= xMax) && (p.Y >= yMin) && (p.Y <= yMax) && (Math.Abs(a * p.X + b - p.Y) <= 6);
        }
    }

    public class PenTekening : Tekening
    {
        public List<Point> points;

        public PenTekening(List<Point> points, Pen pen)
        {
            this.points = points;
            this.pen = pen;
        }

        public override void Teken(Graphics g)
        {
            for (int i = 0; i < points.Count - 1; i++)
            {
                g.DrawLine(pen, points[i], points[i + 1]);
            }
        }

        public override bool isAtPoint(Point p)
        {
            for (int i = 0; i < points.Count; i++)
            {
                if (Math.Abs(points[i].X - p.X) <= 3 && Math.Abs(points[i].Y - p.Y) <= 3)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class CirkelTekening : VierkantTekening
    {
        public CirkelTekening(Point p1, Point p2, Pen pen) : base(p1, p2, pen)
        {

        }

        public override void Teken(Graphics g)
        {
            g.DrawEllipse(pen, TweepuntTool.Punten2Rechthoek(this.startpunt, this.eindpunt));
        }

    }

    public class VolCirkelTekening : CirkelTekening
    {
        public VolCirkelTekening(Point p1, Point p2, Pen pen) : base(p1, p2, pen)
        {

        }

        public override void Teken(Graphics g)
        {
            g.FillEllipse(pen.Brush, TweepuntTool.Punten2Rechthoek(this.startpunt, this.eindpunt));
        }

    }
}
