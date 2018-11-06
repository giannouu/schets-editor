using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;

namespace SchetsEditor
{
    public class Schets
    {
        private Bitmap bitmap;
        public List<Tekening> tekeningList;

        public Schets()
        {
            bitmap = new Bitmap(1, 1);
            tekeningList = new List<Tekening>();
        }

        public Graphics BitmapGraphics
        {
            get { return Graphics.FromImage(bitmap); }
        }
        public void VeranderAfmeting(Size sz)
        {
            if (sz.Width > bitmap.Size.Width || sz.Height > bitmap.Size.Height)
            {
                Bitmap nieuw = new Bitmap( Math.Max(sz.Width,  bitmap.Size.Width)
                                         , Math.Max(sz.Height, bitmap.Size.Height)
                                         );
                Graphics gr = Graphics.FromImage(nieuw);
                gr.FillRectangle(Brushes.White, 0, 0, sz.Width, sz.Height);
                gr.DrawImage(bitmap, 0, 0);
                bitmap = nieuw;
            }
        }
        public void Teken(Graphics gr)
        {
            gr.DrawImage(bitmap, 0, 0);

            Debug.WriteLine("Items in tekeningList:");
            for (int i = 0; i < tekeningList.Count; i++)
            {
                this.tekeningList[i].Teken(gr);
                Debug.WriteLine(tekeningList[i]);
            }
        }

        // Schoon cleart de tijdelijke tekeningen op de bitmap.
        public void Schoon()
        {
            Graphics gr = Graphics.FromImage(bitmap);
            gr.FillRectangle(Brushes.White, 0, 0, bitmap.Width, bitmap.Height);
        }

        public void Clear()
        {
            Schoon();
            tekeningList.Clear();
        }

        public void Roteer()
        {
            Schoon();
            bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);

            Point mid = new Point(bitmap.Width / 2, bitmap.Height / 2);

            for (int i = 0; i < tekeningList.Count; i++)
            {
                //tekeningList.Roteer(mid);
            }
        }
    }
}
