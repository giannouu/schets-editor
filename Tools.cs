﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Collections.Generic;

namespace SchetsEditor
{
    public interface ISchetsTool
    {
        void MuisVast(SchetsControl s, Point p);
        void MuisDrag(SchetsControl s, Point p);
        void MuisLos(SchetsControl s, Point p);
        void Letter(SchetsControl s, char c);
        void addTekening(SchetsControl s, Point p);
    }

    public abstract class StartpuntTool : ISchetsTool
    {
        protected Point startpunt;
        protected Brush kwast;

        public virtual void MuisVast(SchetsControl s, Point p)
        {
            startpunt = p;
        }
        public virtual void MuisLos(SchetsControl s, Point p)
        {
            kwast = new SolidBrush(s.PenKleur);
        }
        public abstract void MuisDrag(SchetsControl s, Point p);
        public abstract void Letter(SchetsControl s, char c);
        public abstract void addTekening(SchetsControl s, Point p);
    }

    public class TekstTool : StartpuntTool
    {
        public String text = "";

        public override string ToString() { return "tekst"; }

        public override void MuisDrag(SchetsControl s, Point p)
        {
            this.MuisVast(s, p);
        }

        public override void MuisVast(SchetsControl s, Point p)
        {
            base.MuisVast(s, p);
            this.text = "";
            this.addTekening(s, this.startpunt);
        }

        public override void Letter(SchetsControl s, char c)
        {
            if (c >= 32)
            {
                this.text += c.ToString();

                Graphics gr = s.MaakBitmapGraphics();
                Font font = new Font("Tahoma", 40);
                SizeF sz =
                gr.MeasureString(this.text, font, this.startpunt, StringFormat.GenericTypographic);
                gr.DrawString(this.text, font, kwast,
                                              this.startpunt, StringFormat.GenericTypographic);
                //gr.DrawRectangle(Pens.Black, startpunt.X, startpunt.Y, sz.Width, sz.Height);
                //startpunt.X += (int)sz.Width;
                this.updateTekening(s, this.startpunt);
                s.Invalidate();
            }
        }

        public override void MuisLos(SchetsControl s, Point p)
        {
            base.MuisLos(s, p);
        }

        public override void addTekening(SchetsControl s, Point p)
        {
            s.addTekening(new TekstTekening(this.startpunt, new Pen(s.PenKleur), this.text));
            s.Schoon(null, null);
        }

        public void updateTekening(SchetsControl s, Point p)
        {
            s.updateTekening(new TekstTekening(this.startpunt, new Pen(s.PenKleur), this.text));
            s.Schoon(null, null);
        }
    }

    public abstract class TweepuntTool : StartpuntTool
    {
        public static Rectangle Punten2Rechthoek(Point p1, Point p2)
        {
            return new Rectangle(new Point(Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y))
                                , new Size(Math.Abs(p1.X - p2.X), Math.Abs(p1.Y - p2.Y))
                                );
        }
        public static Pen MaakPen(Brush b, int dikte)
        {
            Pen pen = new Pen(b, dikte);
            pen.StartCap = LineCap.Round;
            pen.EndCap = LineCap.Round;
            return pen;
        }
        public override void MuisVast(SchetsControl s, Point p)
        {
            base.MuisVast(s, p);
            kwast = Brushes.Gray;
        }
        public override void MuisDrag(SchetsControl s, Point p)
        {
            s.Refresh();
            this.Bezig(s.CreateGraphics(), this.startpunt, p);
        }
        public override void MuisLos(SchetsControl s, Point p)
        {
            base.MuisLos(s, p);
            this.Compleet(s.MaakBitmapGraphics(), this.startpunt, p);
            this.addTekening(s, p);
            s.Invalidate();
        }
        public override void Letter(SchetsControl s, char c)
        {
        }
        public abstract void Bezig(Graphics g, Point p1, Point p2);

        public virtual void Compleet(Graphics g, Point p1, Point p2)
        {
            this.Bezig(g, p1, p2);
        }
    }

    public class RechthoekTool : TweepuntTool
    {
        public override string ToString() { return "kader"; }

        public override void Bezig(Graphics g, Point p1, Point p2)
        {
            g.DrawRectangle(MaakPen(kwast, 3), TweepuntTool.Punten2Rechthoek(p1, p2));
        }

        public override void addTekening(SchetsControl s, Point p)
        {
            s.addTekening(new VierkantTekening(this.startpunt, p, MaakPen(kwast, 3)));
            s.Schoon(null, null);
        }
    }

    public class VolRechthoekTool : RechthoekTool
    {
        public override string ToString() { return "vlak"; }

        public override void Compleet(Graphics g, Point p1, Point p2)
        {
            g.FillRectangle(kwast, TweepuntTool.Punten2Rechthoek(p1, p2));
        }

        public override void addTekening(SchetsControl s, Point p)
        {
            s.addTekening(new VolVierkantTekening(this.startpunt, p, MaakPen(kwast, 3)));
            s.Schoon(null, null);
        }
    }

    public class LijnTool : TweepuntTool
    {
        public override string ToString() { return "lijn"; }

        public override void Bezig(Graphics g, Point p1, Point p2)
        {
            g.DrawLine(MaakPen(this.kwast, 3), p1, p2);
        }

        public override void addTekening(SchetsControl s, Point p)
        {
            s.addTekening(new LijnTekening(this.startpunt, p, MaakPen(kwast, 3)));
            s.Schoon(null, null);
        }
    }

    public class PenTool : LijnTool
    {
        public List<Point> points;

        public override string ToString() { return "pen"; }

        public override void MuisVast(SchetsControl s, Point p)
        {
            points = new List<Point>();
            points.Add(p);
        }

        public override void MuisDrag(SchetsControl s, Point p)
        {
            points.Add(p);
        }

        public override void MuisLos(SchetsControl s, Point p)
        {
            kwast = new SolidBrush(s.PenKleur);
            this.addTekening(s, p);
            s.Invalidate();
        }

        public override void addTekening(SchetsControl s, Point p)
        {
            s.addTekening(new PenTekening(points, MaakPen(kwast, 3)));
            //s.Schoon(null, null);
        }
    }

    /**
     * Oude Gumtool
     *
    public class GumTool : PenTool
    {
        public override string ToString() { return "gum"; }

        public override void Bezig(Graphics g, Point p1, Point p2)
        {
            g.DrawLine(MaakPen(Brushes.White, 7), p1, p2);
        }
    } */

    public class GumTool : StartpuntTool
    {
        public override string ToString() { return "gum"; }

        public override void MuisDrag(SchetsControl s, Point p) { }
        public override void Letter(SchetsControl s, char c) { }
        public override void addTekening(SchetsControl s, Point p)
        {
            s.Schoon(null, null);
        }

        public override void MuisVast(SchetsControl s, Point p)
        {
            base.MuisVast(s, p);
            s.removeTekening(p);
            s.Schoon(null, null);
        }

    }

    public class CirkelTool : TweepuntTool
    {
        public override string ToString() { return "cirkel"; }

        public override void Bezig(Graphics g, Point p1, Point p2)
        {
            g.DrawEllipse(MaakPen(kwast, 3), TweepuntTool.Punten2Rechthoek(p1, p2));
        }

        public override void addTekening(SchetsControl s, Point p)
        {
            s.addTekening(new CirkelTekening(this.startpunt, p, MaakPen(kwast, 3)));
            s.Schoon(null, null);
        }
    }

    public class VolCirkelTool : CirkelTool
    {
        public override string ToString() { return "volcirkel"; }

        public override void Compleet(Graphics g, Point p1, Point p2)
        {
            g.FillEllipse(kwast, TweepuntTool.Punten2Rechthoek(p1, p2));
        }

        public override void addTekening(SchetsControl s, Point p)
        {
            s.addTekening(new VolCirkelTekening(this.startpunt, p, MaakPen(kwast, 3)));
            s.Schoon(null, null);
        }
    }
}
