using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace VisualHullReconstruction
{
    public class OctNode
    {
        public List<OctNode> Children; 
        public Point3D Point; // Cube "point" is defined as the center of the bottom face of the cube
        public readonly double SideLength;
        public OctoState State;
        //public List<Point3D> Corners; 

        public OctNode(double sidelength, Point3D p)
        {
            State = OctoState.Full; // The volume is assumed to be full unless proven otherwise
            SideLength = sidelength;
            Point = p;
            Children = null;
            //Corners = GetCorners();
        }

        /// <summary>
        /// Creates 8 "full" children for the current node
        /// </summary>
        public void Split()
        {
            if (Children != null)
                return;
            State = OctoState.Ambiguous;
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

            //foreach (var child in Children)
            //{
            //    if (child.Children != null)
            //        child.Compact();

            //    if (child.State == OctoState.Full)
            //    {
            //        allEmpty = false;
            //    }
            //    else if (child.State == OctoState.Empty)
            //    {
            //        allFull = false;
            //    }                
            //}

            foreach (var child in Children)
            {
                // Make sure all children are compacted before doing this level
                if (child.Children != null)
                    child.Compact();
            }

            if (Children[0].State == OctoState.Full &&
                Children[1].State == OctoState.Full &&
                Children[2].State == OctoState.Full &&
                Children[3].State == OctoState.Full &&
                Children[4].State == OctoState.Full &&
                Children[5].State == OctoState.Full &&
                Children[6].State == OctoState.Full &&
                Children[7].State == OctoState.Full)
            {
                // kill the children and make me full!
                Children = null;
                State = OctoState.Full;
            }
            else if (Children[0].State == OctoState.Empty &&
                Children[1].State == OctoState.Empty &&
                Children[2].State == OctoState.Empty &&
                Children[3].State == OctoState.Empty &&
                Children[4].State == OctoState.Empty &&
                Children[5].State == OctoState.Empty &&
                Children[6].State == OctoState.Empty &&
                Children[7].State == OctoState.Empty)
            {
                // kill the children and make me empty!
                Children = null;
                State = OctoState.Empty;
            }
            else
                // There is some combination of the 2 so do nothing.
                State = OctoState.Ambiguous;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Point3D> GetCorners()
        {
            var list = new List<Point3D>
            {
                new Point3D(Point.X - SideLength/2, Point.Y, Point.Z - SideLength/2),
                new Point3D(Point.X + SideLength/2, Point.Y, Point.Z - SideLength/2),
                new Point3D(Point.X - SideLength/2, Point.Y + SideLength, Point.Z - SideLength/2),
                new Point3D(Point.X + SideLength/2, Point.Y + SideLength, Point.Z - SideLength/2),
                new Point3D(Point.X - SideLength/2, Point.Y, Point.Z + SideLength/2),
                new Point3D(Point.X + SideLength/2, Point.Y, Point.Z + SideLength/2),
                new Point3D(Point.X - SideLength/2, Point.Y + SideLength, Point.Z + SideLength/2),
                new Point3D(Point.X + SideLength/2, Point.Y + SideLength, Point.Z + SideLength/2)
            };

            return list;
        }

        /// <summary>
        /// Exports the octree so it can be visualized in OpenGL
        /// </summary>
        public static void ExportTree(OctNode node, StreamWriter file)
        {
            // Need size length and origin position
            if (node == null)
                return;

            if (node.State == OctoState.Full)            // Only care about full cubes
            {
                string line = node.SideLength.ToString(CultureInfo.InvariantCulture) + " " +
                    node.Point.X.ToString(CultureInfo.InvariantCulture) + " " +
                    (node.Point.Y + node.SideLength / 2).ToString(CultureInfo.InvariantCulture) + " " +
                    node.Point.Z.ToString(CultureInfo.InvariantCulture);
                file.WriteLine(line);
            }

            if (node.Children == null || node.Children.Count <= 0)
                return;

            foreach (var child in node.Children)
            {
                ExportTree(child, file);
            }

        }
    }
}
