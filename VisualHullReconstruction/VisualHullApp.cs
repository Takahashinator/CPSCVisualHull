﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisualHullReconstruction
{
    public partial class VisualHullApp : Form
    {
        private const double Sidelength = 1000;
        private OctNode root = new OctNode(Sidelength, new Point3D(0,0,0));
        private ImageList imList; 

        public VisualHullApp()
        {
            imList = new ImageList();
            InitializeComponent();

            ColumnHeader colHeader1 = new ColumnHeader
            {
                Text = "Filename",
                Width = 200
            };

            this.listViewOrig.Columns.AddRange(new[] { colHeader1});
        }

        private void buttonLoadImages_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                Title = "Load Images",
                Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png",
                Multiselect = true
            };


            if (dlg.ShowDialog() == DialogResult.OK)
            {
                foreach (var filename in dlg.FileNames)
                {
                    try
                    {
                        imList.Images.Add(Image.FromFile(filename));

                        // Add the image name to the listview box
                        listViewOrig.Items.Add(new ListViewItem(new[] { Path.GetFileName(filename), Path.GetDirectoryName(filename) }));
                    }
                    catch (SecurityException ex)
                    {
                        // The user lacks appropriate permissions to read files, discover paths, etc.
                        MessageBox.Show("Security error. \n\n" +
                            "Error message: " + ex.Message + "\n\n" +
                            "Details (send to Support):\n\n" + ex.StackTrace
                        );
                    }
                    catch (Exception ex)
                    {
                        // Could not load the image - probably related to Windows file system permissions.
                        MessageBox.Show("Cannot load the image: " + filename.Substring(filename.LastIndexOf('\\'))
                            + ". You may not have permission to read the file, or " +
                            "it may be corrupt.\n\nReported error: " + ex.Message);
                    }
                }
            }
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
