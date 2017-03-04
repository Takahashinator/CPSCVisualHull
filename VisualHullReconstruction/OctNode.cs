using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualHullReconstruction
{
    class OctNode
    {
        public List<OctNode> Children; 
        public Point3D Point;
        private readonly double _sideLength;
        private OctoState state;

        public OctNode(double sidelength, Point3D p)
        {
            state = OctoState.F;
            _sideLength = sidelength;
            Point = p;
            Children = null;
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
                var offsetx = (i & (1 << 0)) * _sideLength / 2;
                var offsety = ((i & (1 << 1)) >> 1) * _sideLength / 2;
                var offsetz = ((i & (1 << 2)) >> 2) * _sideLength / 2;
                Children.Add(new OctNode(_sideLength/2, 
                    new Point3D(Point.X + offsetx, Point.Y + offsety, Point.Z +offsetz )));
            }
        }

        /// <summary>
        /// Checks the whole tree and compacts any all empty or full nodes into one
        /// </summary>
        public void Compact()
        {
            // TODO
        }
    }
}
