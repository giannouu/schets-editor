﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SchetsEditor
{   public class SchetsControl : UserControl
    {   private Schets schets;
        private Color penkleur;

        public Color PenKleur
        { get { return penkleur; }
        }

        public Schets Schets
        { get { return schets; }
        }
       
        public SchetsControl()
        {   this.BorderStyle = BorderStyle.Fixed3D;
            this.schets = new Schets();
            this.Paint += this.teken;
            this.Resize += this.veranderAfmeting;
            this.veranderAfmeting(null, null);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        private void teken(object o, PaintEventArgs pea)
        {   schets.Teken(pea.Graphics);
        }

        private void veranderAfmeting(object o, EventArgs ea)
        {   schets.VeranderAfmeting(this.ClientSize);
            this.Invalidate();
        }

        public Graphics MaakBitmapGraphics()
        {   Graphics g = schets.BitmapGraphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            return g;
        }

        // Schoon cleart de tijdelijke tekeningen op de bitmap.
        public void Schoon(object o, EventArgs ea)
        {   schets.Schoon();
            this.Invalidate();
        }

        public void Clear(object o, EventArgs ea)
        {
            schets.Clear();
            this.Invalidate();
        }

        public void Roteer(object o, EventArgs ea)
        {   schets.VeranderAfmeting(new Size(this.ClientSize.Height, this.ClientSize.Width));
            schets.Roteer();
            this.Invalidate();
        }

        public void VeranderKleur(object obj, EventArgs ea)
        {   string kleurNaam = ((ComboBox)obj).Text;
            penkleur = Color.FromName(kleurNaam);
        }

        public void VeranderDikte(object sender, System.EventArgs e)
        {
            
        }

        public void VeranderKleurViaMenu(object obj, EventArgs ea)
        {   string kleurNaam = ((ToolStripMenuItem)obj).Text;
            penkleur = Color.FromName(kleurNaam);
        }

        public void addTekening(Tekening tekening)
        {
            this.schets.tekeningList.Add(tekening);
        }

        public void removeTekening(Point p)
        {
            for (int i = schets.tekeningList.Count - 1; i >= 0; i--)
            {
                if (schets.tekeningList[i].isAtPoint(p))
                {
                    this.schets.tekeningList.RemoveAt(i);
                    return;
                }
            }
        }

        public void updateTekening(Tekening tekening)
        {
            this.schets.tekeningList[this.schets.tekeningList.Count - 1] = tekening;
        }

    }
}
