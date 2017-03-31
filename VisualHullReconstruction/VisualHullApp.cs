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
        private Queue<OctNode> octoQueue = new Queue<OctNode>(); 

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
            _root.State = OctoState.Ambiguous;
            foreach (var child in _root.Children)
            {
                child.Split();
                child.State = OctoState.Ambiguous;
                foreach (var grandchild in child.Children)
                {
                    grandchild.Split();
                    grandchild.State = OctoState.Ambiguous;
                    foreach (var greatGrandchild in grandchild.Children)
                    {
                        octoQueue.Enqueue(greatGrandchild);
                        currentSideLength = greatGrandchild.SideLength;
                    }
                }
            }
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
                Cursor.Current = Cursors.WaitCursor;
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
                Cursor.Current = Cursors.Default;
            }

        }

        private void buttonHull_Click(object sender, EventArgs e)
        {
            if (listViewEdited.Items.Count == 0 || _viewPointList.Count == 0)
            {
                MessageBox.Show("No Sillhouette images loaded!");
                return;
            }
            Cursor.Current = Cursors.WaitCursor;
            // Do the main octree work
            //octoQueue = EnqueueTree();

            while (octoQueue.Count != 0)
            {
                OctNode node = octoQueue.Dequeue();

                bool allFull = true;
                foreach (var image in _viewPointList)
                {
                    OctoState state = ImageAnalysis.IsInSillhouette(node, image, _kMatrix);
                    if (state == OctoState.Ambiguous)
                    {
                        allFull = false;
                    }                       
                    else if (state == OctoState.Empty)
                    {
                        // Empty means completely outside the sillhouette
                        node.State = OctoState.Empty;
                        allFull = false;
                        break;
                    }
                }    
                
                if (allFull)
                {
                    node.State = OctoState.Full;
                }
                else if (!allFull && node.State != OctoState.Empty)
                { // node is not full or empty: is ("Ambiguous")
                    if (node.SideLength / 2 >= MinSideLength)
                    {
                        node.State = OctoState.Ambiguous;
                        node.Split();
                        foreach (var child in node.Children)
                        {
                            octoQueue.Enqueue(child);
                        }
                    } 
                    else
                    {
                        // no more splitting, but set ambiguous node to EMPTY (could also set to full)
                        node.State = OctoState.Empty;
                    }
                }
            }

            _root.Compact();
            //StackChildren(_root);
            try
            {
                using (StreamWriter file = new StreamWriter(@"C:\Users\tttakaha\Desktop\Octree.txt"))
                {
                    OctNode.ExportTree(_root, file);
                }
                MessageBox.Show("Tree successfully saved to file.");
            }
            catch(IOException E)
            {
                MessageBox.Show("Error when writing tree to file! (Probably need to change the path)\n" + E.Message);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error in export function! \n " + exception.Message);
            }
            Cursor.Current = Cursors.Default;
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

        //private Queue<OctNode> EnqueueTree()
        //{
        //    var queue = new Queue<OctNode>();
        //    foreach (var child in _root.Children)
        //    {
        //        foreach (var grandchild in child.Children)
        //            foreach (var greatGrandchild in grandchild.Children)
        //                queue.Enqueue(greatGrandchild);
        //    }
        //    return queue;
        //}

        //private void StackChildren(OctNode node)
        //{
        //    if (node.Children != null)
        //        foreach (var child in node.Children)
        //        {
        //            StackChildren(child);
        //        }
        //    else
        //    {
        //        if (node.State == OctoState.Full)
        //            nodeStack.Push(node);
        //    }
        //}
    }
}
