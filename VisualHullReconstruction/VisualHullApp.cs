using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisualHullReconstruction
{
    public partial class VisualHullApp : Form
    {
        private const double Sidelength = 1000;
        private OctNode root = new OctNode(Sidelength, new Point3D(0,0,0));

        public VisualHullApp()
        {
            InitializeComponent();
        }

        private void buttonLoadImages_Click(object sender, EventArgs e)
        {
            // TODO
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            // Test Code
            //root.Split();
            //foreach (var listViewItem in root.Children.Select(node => new ListViewItem(node.Point.ToString())))
            //{
            //    listViewEdited.Items.Add(listViewItem);
            //}
        }
    }
}
