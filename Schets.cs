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
        public void Schoon()
        {
            Graphics gr = Graphics.FromImage(bitmap);
            gr.FillRectangle(Brushes.White, 0, 0, bitmap.Width, bitmap.Height);
        }
        public void Roteer()
        {
            Schoon();
            bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
        }
    }
}
