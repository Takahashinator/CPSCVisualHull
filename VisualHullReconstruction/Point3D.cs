using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualHullReconstruction
{
    public class Point3D
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Point3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString()
        {
            string temp = "X=";
            temp += X + " Y=";
            temp += Y + "Z= ";
            temp += Z;
            return temp;
        }

        public static bool operator ==(Point3D p1, Point3D p2)
        {
            if (ReferenceEquals(p1, p2))
            {
                return true;
            }
            if (ReferenceEquals(p1, null) ||
                ReferenceEquals(p2, null))
            {
                return false;
            }
            return p1.X == p2.X && p1.Y == p2.Y && p1.X == p2.Z;
        }

        public static bool operator !=(Point3D p1, Point3D p2)
        {
            // Delegate...
            return !(p1 == p2);
        }

        public override bool Equals(object obj)
        {
            var other = obj as Point3D;
            if (other == null)
                return false;
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
