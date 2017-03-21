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
        public int[,] ImageMap { get; private set; }

        public ViewPoint(int[,] im, Point3D p, double viewAngle, double declination)
        {
            ImageMap = im;
            Position = p;
            ViewAngle = viewAngle;
            Declination = declination;
        }
    }
}
