using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualHullReconstruction
{
    public class ViewPoint
    {
        public Point3D Position { get; private set; }
        public double ViewAngle { get; private set; }
        public double Declination { get; private set; }
        public Image ImageMap { get; private set; }

        public ViewPoint(Image im, Point3D p, double va, double dec)
        {
            ImageMap = im;
            Position = p;
            ViewAngle = va;
            Declination = dec;
        }
    }
}
