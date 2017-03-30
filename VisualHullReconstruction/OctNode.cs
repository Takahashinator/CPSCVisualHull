using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualHullReconstruction
{
    class OctNode
    {
        public List<OctNode> Children; 
        public Point3D Point; // Cube "point" is defined as the center of the bottom face of the cube
        public readonly double SideLength;
        public OctoState State;
        public List<Point3D> Corners; 

        public OctNode(double sidelength, Point3D p)
        {
            State = OctoState.Full; // The volume is assumed to be full unless proven otherwise
            SideLength = sidelength;
            Point = p;
            Children = null;
            Corners = GetCorners();
        }

        /// <summary>
        /// Creates 8 "full" children for the current node
        /// </summary>
        public void Split()
        {
            if (Children != null)
                return;

            Children = new List<OctNode>();
            for (int i = 0; i < 8; i++)
            {
                // TODO -> is there a better way to do this?
                double offsetx = (i%2 == 0? -1 : 1) * SideLength / 4;
                double offsety = ((i & (1 << 1)) >> 1) * SideLength/2;
                double offsetz = (i < 4? -1 : 1) * SideLength / 4;
                Children.Add(new OctNode(SideLength/2, 
                    new Point3D(Point.X + offsetx, Point.Y + offsety, Point.Z + offsetz )));
            }
        }

        /// <summary>
        /// Checks the whole sub-tree and compacts any all empty or full nodes into one
        /// </summary>
        public void Compact()
        {
            if (Children == null) return;

            bool allFull = true;
            bool allEmpty = true;

            foreach (var child in Children)
            {
                if (child.Children != null)
                    child.Compact();
                else
                {
                    if (child.State == OctoState.Full)
                    {
                        allEmpty = false;
                    }
                    else
                    {
                        allFull = false;
                    }
                }
            }

            if (allEmpty && !allFull)
            {
                Children = null;
                State = OctoState.Empty;
            }
            else if (allFull && !allEmpty)
            {
                Children = null;
                State = OctoState.Full;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<Point3D> GetCorners()
        {
            var list = new List<Point3D>
            {
                new Point3D(Point.X - SideLength/2, 0, Point.Z - SideLength/2),
                new Point3D(Point.X + SideLength/2, 0, Point.Z - SideLength/2),
                new Point3D(Point.X - SideLength/2, Point.Y + SideLength, Point.Z - SideLength/2),
                new Point3D(Point.X + SideLength/2, Point.Y + SideLength, Point.Z - SideLength/2),
                new Point3D(Point.X - SideLength/2, 0, Point.Z + SideLength/2),
                new Point3D(Point.X + SideLength/2, 0, Point.Z + SideLength/2),
                new Point3D(Point.X - SideLength/2, Point.Y + SideLength, Point.Z + SideLength/2),
                new Point3D(Point.X + SideLength/2, Point.Y + SideLength, Point.Z + SideLength/2)
            };

            return list;
        }
    }
}
