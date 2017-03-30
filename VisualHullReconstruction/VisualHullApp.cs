using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace VisualHullReconstruction
{
    // Unused
    public struct BoundingBox
    {
        public Point Point;
        public int Size;
    }

    public partial class VisualHullApp : Form
    {
        //Octospace constants
        //Note: units of distance are given in millimeters
        //Coordinate Axis X = left-, +right,
        //Y = up+ and -down 
        // Z = forwards+, -backwards
        private const double Sidelength = 150; // Max sidelength of the volume cube (189 should work but throws exceptions)
        private const double MinSideLength = 1;
        private double currentSideLength = Sidelength;
        private static readonly Point3D CameraInitialPosition = new Point3D(0, 100, -370);
        private const double dec = 10.3;
        private Stack<OctNode> nodeStack = new Stack<OctNode>(); //Stack used for exporting octree


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

            var colHeader3 = new ColumnHeader
            {
                Text = "Filename",
                Width = 100
            };

            var colHeader4 = new ColumnHeader
            {
                Text = "Path",
                Width = 700
            };

            listViewOrig.Columns.AddRange(new[] { colHeader1, colHeader2 });
            listViewEdited.Columns.AddRange(new[] { colHeader3, colHeader4 });

            // Start with 4x4 octree
            _root.Split();
            currentSideLength/=2;
            _root.State = OctoState.Ambiguous;;
            foreach (var child in _root.Children)
            {
                child.Split();
                child.State = OctoState.Ambiguous;
            }
            currentSideLength /= 2;
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
                        //binImage = ImageAnalysis.BoundingSquaresCalc(binImage, tempBitmap.Width, tempBitmap.Height);

                        // Get Camera angle contained in the filename
                        string angleString = string.Join(string.Empty, Regex.Matches(name, @"\d+").OfType<Match>().Select(m => m.Value));
                        double angle = Convert.ToDouble(angleString);

                        // Camera Declination is constant across all images

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
            if (listViewEdited.Items.Count == 0 || _viewPointList.Count == 0)
            {
                MessageBox.Show("No Sillhouette images loaded!");
                return;
            }

            // Do the main octree work
            var octoQueue = EnqueueTree();
            //int splitcounter = 0;
            while (octoQueue.Count != 0 && currentSideLength >= MinSideLength)
            {
                OctNode node = octoQueue.Dequeue();
                bool allOut = true;
                bool allIn = true;
                foreach (var image in _viewPointList)
                {
                    OctoState state = ImageAnalysis.IsInSillhouette(node, image, _kMatrix);
                    if (state == OctoState.Full)
                        allOut = false;
                    else
                        allIn = false;
                }

                if (allIn && !allOut)
                    node.State = OctoState.Full;
                else if (allOut && !allIn)
                    node.State = OctoState.Empty;
                else
                {
                    //splitcounter++;
                    node.State = OctoState.Ambiguous;
                    node.Split();
                    currentSideLength = node.Children[0].SideLength < currentSideLength? node.Children[0].SideLength : currentSideLength;
                    foreach (var child in node.Children)
                    {
                        octoQueue.Enqueue(child);
                    }
                }
            }

            _root.Compact();
            EnqueueChildren(_root);
            ExportTree();

            // By this point we should have a full octree!
            string book = "awesome";
            string awesome = "I am" + book;
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

        private Queue<OctNode> EnqueueTree()
        {
            var queue = new Queue<OctNode>();
            foreach (var child in _root.Children.SelectMany(child1 => child1.Children))
            {
                queue.Enqueue(child);
            }
            return queue;
        }

        private void EnqueueChildren(OctNode node)
        {
            if (node.Children != null)
                foreach (var child in node.Children)
                {
                    EnqueueChildren(child);
                }
            else
            {
                if (node.State == OctoState.Full)
                    nodeStack.Push(node);
            }
        }

        /// <summary>
        /// Exports the octree so it can be visualized in OpenGL
        /// </summary>
        public void ExportTree()
        {
            // Need size length and origin position
            // Only care about full cubes
            if (nodeStack.Count <= 0)
                return;
            using (StreamWriter file = new StreamWriter(@"C:\Users\Takahashi Home\Documents\Visual Studio 2013\Projects\VisualHullReconstruction\Octree.txt"))
            {
                while (nodeStack.Count > 0)
                {
                    OctNode node = nodeStack.Pop();
                    string line = node.SideLength.ToString(CultureInfo.InvariantCulture) + " " +
                                  node.Point.X.ToString(CultureInfo.InvariantCulture) + " " +
                                  (node.Point.Y + node.SideLength / 2).ToString(CultureInfo.InvariantCulture) + " " +
                                  node.Point.Z.ToString(CultureInfo.InvariantCulture);
                    file.WriteLine(line);
                }
            }
        }
    }
}
