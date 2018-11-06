using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SchetsEditor
{

    abstract class Tekening
    {
        Point startpunt;

        public abstract void Teken();
    }

    class TekstTekening : Tekening
    {
        public override void Teken()
        {

        }
    }

    class VierkantTekening : Tekening
    {
        Point eindpunt;

        public override void Teken()
        {

        }
    }

    class VolVierkantTekening : VierkantTekening
    {
        public override void Teken()
        {

        }
    }

    class LijnTekening : Tekening
    {
        public override void Teken()
        {

        }
    }

    class PenTekening : LijnTekening
    {
        public override void Teken()
        {

        }
    }

    class CirkelTekening : VierkantTekening
    {
        public override void Teken()
        {

        }
    }

    class VolCirkelTekening : CirkelTekening
    {
        public override void Teken()
        {

        }
    }
}
