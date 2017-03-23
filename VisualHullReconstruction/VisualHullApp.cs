using System;
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
using System.Text.RegularExpressions;

namespace VisualHullReconstruction
{
    public partial class VisualHullApp : Form
    {
        //Octospace constants
        //Note: units of distance are given in millimeters
        //Coordinate Axis X = left-, +right,
        //Y = up+ and -down 
        // Z = forwards+, -backwards
        private const double Sidelength = 160; // Max sidelength of the volume cube
        private static readonly Point3D CameraInitialPosition = new Point3D(0, 100, -370);


        private OctNode _root = new OctNode(Sidelength, new Point3D(0,0,0));
        private List<ViewPoint> _viewPointList;
        private static readonly double[,] _kMatrix = new double[3,3] { {1095.826, 0, 351.289}, { 0, 1112.102, 678.420}, { 0, 0, 1} };

        private GenerateForm genForm;
        private TestForm testForm;

        public VisualHullApp()
        {
            _viewPointList = new List<ViewPoint>();
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

            listViewOrig.Columns.AddRange(new[] { colHeader1, colHeader2 });
        }

        private void buttonLoadImages_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Title = "Load Images",
                Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png",
                Multiselect = true
            };


            if (dlg.ShowDialog() != DialogResult.OK) return;

            foreach (string filename in dlg.FileNames)
            {
                try
                {
                    //imList.Images.Add(Image.FromFile(filename));

                    // Add the image name to the listview box
                    listViewOrig.Items.Add(new ListViewItem(new[] { Path.GetFileName(filename), Path.GetFullPath(filename) }));
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

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Load Sillhouette images from file?", "Load Dialog", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
            {
                genForm = new GenerateForm(this);
                genForm.Show();
            }
            else
            {
                // Load from file
                var dlg = new OpenFileDialog
                {
                    Title = "Load Images",
                    Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png",
                    Multiselect = true
                };


                if (dlg.ShowDialog() != DialogResult.OK) return;

                foreach (string filename in dlg.FileNames)
                {
                    try
                    {
                        string name = Path.GetFileNameWithoutExtension(filename);
                        Bitmap tempBitmap = new Bitmap(filename);
                        int[,] binImage = ImageAnalysis.ConvertBinary(tempBitmap);
                        binImage = ImageAnalysis.BoundingSquaresCalc(binImage, tempBitmap.Width, tempBitmap.Height);

                        // Get Camera angle contained in the filename
                        string angleString = string.Join(string.Empty, Regex.Matches(name, @"\d+").OfType<Match>().Select(m => m.Value));
                        double angle = Convert.ToDouble(angleString);

                        // Camera Declination is constant across all images
                        double dec = 11.55;

                        // Get Camera position in 3D space
                        Point3D position = ImageAnalysis.Calculate3DPosition(angle, CameraInitialPosition); 

                        _viewPointList.Add(new ViewPoint(binImage, position, angle, dec));

                        // Add the image name to the listview box
                        listViewEdited.Items.Add(new ListViewItem(new[] { Path.GetFileName(filename), Path.GetFullPath(filename) }));
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

        private void buttonHull_Click(object sender, EventArgs e)
        {
            _root.Split();
            if (listViewEdited.Items.Count == 0 || _viewPointList.Count == 0)
            {
                MessageBox.Show("No Sillhouette images loaded!");
            }


            // Test Code
            //root.Split();
            //foreach (var listViewItem in root.Children.Select(node => new ListViewItem(node.Point.ToString())))
            //{
            //    listViewEdited.Items.Add(listViewItem);
            //}
        }

        /// <summary>
        /// Used by Generate Form to get listview info
        /// </summary>
        /// <returns></returns>
        public ListView GetListView()
        {
            return this.listViewOrig;
        }

        private void buttonOpenTester_Click(object sender, EventArgs e)
        {
            testForm = new TestForm();
            testForm.Show();
        }
    }
}
