﻿using System;
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
    public partial class GenerateForm : Form
    {
        private VisualHullApp _parent;

        public GenerateForm(VisualHullApp parent)
        {
            _parent = parent;
            ListView listViewOrig = _parent.GetListView();
            InitializeComponent();

            var colHeader1 = new ColumnHeader
            {
                Text = "Filename",
                Width = 100
            };

            var colHeader2 = new ColumnHeader
            {
                Text = "Path",
                Width = 700
            };

            listView1.Columns.AddRange(new[] { colHeader1, colHeader2 });

            foreach (ListViewItem item in listViewOrig.Items)
            {
                listView1.Items.Add((ListViewItem)item.Clone());
                //listView1.Items.Add(new ListViewItem(new[] { item }));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count <= 0)
                MessageBox.Show("Select an item from the list");
            else
            {
                string disp = listView1.SelectedItems[0].SubItems[1].Text;
                MessageBox.Show(disp);

                // Image refImage = staticClass.setRefImage(path);
                // foreach image in listview
                // extractSilhouette(refImage, image);
                // CalculateBoundingSquareInfo(silImage);
                // add image and params to ViewPoint List
                // end foreach
            }

        }
    }
}
