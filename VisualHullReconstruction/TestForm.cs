using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisualHullReconstruction
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();

            textBox1.Clear();
        }

        private void buttonRunTests_Click(object sender, EventArgs e)
        {
            AddLine("Starting Unit Tests...");
            bool pass = false;

            AddLine("Starting TestConvertBinary...");
            pass = TestConvertBinary();
            AddLine("Test Convert Binary... " + (pass ? "success!" : "failed!"));

            pass = false;
            AddLine("");
            AddLine("Starting TestBoundingSquares...");
            pass = TestBoundingSquares();
            AddLine("Test Bounding Squares... " + (pass ? "success!" : "failed!"));

            pass = false;
            AddLine("");
            AddLine("Starting TestCalculatePosition...");
            pass = TestCalculatePoistion();
            AddLine("Test Position Calculations... " + (pass ? "success!" : "failed!"));

            pass = false;
            AddLine("");
            AddLine("Starting TestOctnodeSplit...");
            pass = TestOctnodeSplit();
            AddLine("Test Octnode Split... " + (pass ? "success!" : "failed!"));

            pass = false;
            AddLine("");
            AddLine("Starting Test3D2D...");
            pass = Test3D2D();
            AddLine("Test Test3D2D... " + (pass ? "success!" : "failed!"));

            pass = false;
            AddLine("");
            AddLine("Starting TestOctCorners...");
            pass = TestOctCorners();
            AddLine("Test TestOctCorners... " + (pass ? "success!" : "failed!"));

            pass = false;
            AddLine("");
            AddLine("Starting TestOctCompact...");
            pass = TestOctCompact();
            AddLine("Test TestOctCompact... " + (pass ? "success!" : "failed!"));

            pass = false;
            AddLine("");
            AddLine("Starting TestOctExport...");
            pass = TestOctExport();
            AddLine("Test TestOctExport... " + (pass ? "success!" : "failed!"));

            pass = false;
            AddLine("");
            AddLine("Starting TestInSillhouette...");
            pass = TestInSillhouette();
            AddLine("Test TestInSillhouette... " + (pass ? "success!" : "failed!"));

            pass = false;
            AddLine("");
            AddLine("Starting TestMultiImageConsistency...");
            pass = TestMultiImageConsistency();
            AddLine("Test TestMultiImageConsistency... " + (pass ? "success!" : "failed!"));

        }

        private void AddLine(string text)
        {
            textBox1.Text += text + Environment.NewLine;
        }

        private bool TestConvertBinary()
        {
            // Test Image:
            /* 
             * 10000000000000000011
             * 11000000000000000001
             * 00000000000000000000
             * 00000000000000000000
             * 00000000000000000000
             * 00000011111111100000
             * 00000011111111100000
             * 00000011111111100000
             * 00000000011100000000
             * 00000000011100000000
             * 00000000011100000000
             * 00000000011100000000
             * 00000000011100000000
             * 00000000011100000000
             * 00000000011100000000
             * 00000000011100000000
             * 00000000000000000000
             * 00000000000000000000
             * 11000000000000000011
             * 11000000000000000001
             */
            try
            {
                bool success = false;

                string path = Directory.GetCurrentDirectory() + "\\testBitmap.png";
                // Read in test image
                Bitmap testImage = new Bitmap(path);
                // Convert
                int[,] intArray = ImageAnalysis.ConvertBinary(testImage);
                // Check for failures

                List<bool> tests = new List<bool>
                {
                    intArray[0, 0] == 1,
                    intArray[0, 1] == 0,
                    intArray[3, 5] == 0,
                    intArray[6, 6] == 1,
                    intArray[7, 11] == 1,
                    intArray[8, 8] == 0,
                    intArray[8, 9] == 1,
                    intArray[15, 8] == 0,
                    intArray[15, 9] == 1,
                    intArray[19, 19] == 1,
                    intArray[0, 0] == 1,
                    intArray[18, 18] == 1
                };


                int fails = 0;
                for (int i = 0; i < tests.Count; i++)
                {
                    if (tests[i] == false)
                    {
                        AddLine("Fail on test # " + i);
                        fails++;
                    }
                }

                AddLine("TestConvertBinary completed. Found " + fails + " failures.");
                if (fails == 0)
                {
                    success = true;
                }

                return success;
            }
            catch (Exception e)
            {
                AddLine("Error in TestConvertBinary!");
                AddLine(e.Message);
                return false;
            }
        }

        private bool TestBoundingSquares()
        {   // Test Image:
            /* 
             * 10000000000000000011
             * 11000000000000000001
             * 00000000000000000000
             * 00000000000000000000
             * 00000000000000000000
             * 00000011111111100000
             * 00000011111111100000
             * 00000011111111100000
             * 00000000011100000000
             * 00000000011100000000
             * 00000000011100000000
             * 00000000011100000000
             * 00000000011100000000
             * 00000000011100000000
             * 00000000011100000000
             * 00000000011100000000
             * 00000000000000000000
             * 00000000000000000000
             * 11000000000000000011
             * 11000000000000000001
             */

            try
            {
                bool success = false;
                // Create Custom Binary Image
                int[,] testBinaryImage =
                {
                    /*0 */{1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1},
                    /*1 */{1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                    /*2 */{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                    /*3 */{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                    /*4 */{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                    /*5 */{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,0,0,0,0,0},
                    /*6 */{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,0,0,0,0,0},
                    /*7 */{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,0,0,0,0,0},
                    /*8 */{0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0},
                    /*9 */{0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0},
                    /*10*/{0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0},
                    /*11*/{0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0},
                    /*12*/{0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0},
                    /*13*/{0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0},
                    /*14*/{0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0},
                    /*15*/{0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0},
                    /*16*/{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                    /*17*/{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                    /*18*/{1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1},
                    /*19*/{1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                };
                // Convert
                int[,] intArray = ImageAnalysis.BoundingSquaresCalc(testBinaryImage, 20, 20);
                // Check for failures

                List<bool> tests = new List<bool>
                {
                    intArray[1, 0] == 1, //0
                    intArray[0, 1] == 0, //1
                    intArray[7, 6] == 3, //2 
                    intArray[7, 9] == 3, //3
                    intArray[7, 13] == 2, //4
                    intArray[6, 13] == 2, //5
                    intArray[10, 9] == 3, //6
                    intArray[10, 10] == 2, //7
                    intArray[8, 12] == 0, //8
                    intArray[19, 0] == 2, //9
                    intArray[18, 1] == 1, //10
                    intArray[19, 18] == 0, //11
                };


                int fails = 0;
                for (int i = 0; i < tests.Count; i++)
                {
                    if (tests[i] == false)
                    {
                        AddLine("Fail on test # " + i);
                        fails++;
                    }
                }

                AddLine("TestBoundingSquares completed. Found " + fails + " failures.");
                if (fails == 0)
                {
                    success = true;
                }

                return success;
            }
            catch (Exception e)
            {
                AddLine("Error in TestBoundingSquares!");
                AddLine(e.Message);
                return false;
            }
        }

        private bool TestCalculatePoistion()
        {
            try
            {
                bool success = false;

                List<bool> tests = new List<bool>();
                Point3D initialPosition = new Point3D(0, 10, -50);
                Point3D point = ImageAnalysis.Calculate3DPosition(0, initialPosition);
                tests.Add(point.X == initialPosition.X && point.Y == initialPosition.Y && point.Z == initialPosition.Z);

                point = ImageAnalysis.Calculate3DPosition(90, initialPosition);
                tests.Add(point.X == 50 && point.Y == initialPosition.Y && point.Z == 0);

                point = ImageAnalysis.Calculate3DPosition(180, initialPosition);
                tests.Add(point.X == 0 && point.Y == initialPosition.Y && point.Z == 50);

                point = ImageAnalysis.Calculate3DPosition(270, initialPosition);
                tests.Add(point.X == -50 && point.Y == initialPosition.Y && point.Z == 0);

                int fails = 0;
                for (int i = 0; i < tests.Count; i++)
                {
                    if (tests[i] == false)
                    {
                        AddLine("Fail on test # " + i);
                        fails++;
                    }
                }

                AddLine("TestCalculatePosition completed. Found " + fails + " failures.");
                if (fails == 0)
                {
                    success = true;
                }

                return success;
            }
            catch (Exception e)
            {
                AddLine("Error in TestConvertBinary!");
                AddLine(e.Message);
                return false;
            }
        }

        private bool TestOctnodeSplit()
        {
            try
            {
                bool success = false;

                OctNode root = new OctNode(100, new Point3D(50,0,50)); // Test based on a corner coordinate system for simplicity
                root.Split();

                List<bool> tests = new List<bool>();

                tests.Add(root.Children.Count != 0);
                tests.Add(root.Children.Count == 8);
                tests.Add(root.Children[0].Point.X == 25);
                tests.Add(root.Children[0].Point.Y == 0);
                tests.Add(root.Children[0].Point.Z == 25);
                tests.Add(root.Children[7].Point.X == 75);
                tests.Add(root.Children[7].Point.Y == 50);
                tests.Add(root.Children[7].Point.Z == 75);

                root.Children[1].Split();

                tests.Add(root.Children[1].Children.Count != 0);
                tests.Add(root.Children[1].Children.Count == 8);
                tests.Add(root.Children[1].Children[1].Point.X == 87.5);
                tests.Add(root.Children[1].Children[1].Point.Y == 0);
                tests.Add(root.Children[1].Children[1].Point.Z == 12.5);
                tests.Add(root.Children[1].Children[6].Point.X == 62.5);
                tests.Add(root.Children[1].Children[6].Point.Y == 25);
                tests.Add(root.Children[1].Children[6].Point.Z == 37.5);

                int fails = 0;
                for (int i = 0; i < tests.Count; i++)
                {
                    if (tests[i] == false)
                    {
                        AddLine("Fail on test # " + i);
                        fails++;
                    }
                }

                AddLine("TestOctnodeSplit completed. Found " + fails + " failures.");
                if (fails == 0)
                {
                    success = true;
                }

                return success;
            }
            catch (Exception e)
            {
                AddLine("Error in TestOctnodeSplit!");
                AddLine(e.Message);
                return false;
            }
        }

        private bool Test3D2D()
        {
            try
            {
                double dec = 10.3;

                // Constants
                Point3D cameraInitialPosition = new Point3D(0, 100, -370);
                double[,] kMatrix = new double[3, 3] {{1095.826, 0, 351.289}, {0, 1112.102, 678.420}, {0, 0, 1}};

                // Call 3D->2D on an image with a known pixel location
                string path = Directory.GetCurrentDirectory() + "\\calibrationimg.jpg";
                // Read in test image
                Bitmap testImage = new Bitmap(path);

                double angle = 0;
                Point3D spaceLocation = new Point3D(0, 0, -94.5); // Point to follow
                //Point3D cameraPosition = ImageAnalysis.Calculate3DPosition(angle, cameraInitialPosition); 
                ViewPoint vp = new ViewPoint(null, cameraInitialPosition, angle, dec);
                Point point2D = ImageAnalysis.To2DPoint(spaceLocation, vp, kMatrix);

                if (point2D.X < 0 || point2D.X > testImage.Width || point2D.Y < 0 || point2D.Y > testImage.Height)
                    return false;

                // Draw a little marker at the given location
                int size = -10;
                for (int i = 0; i < -size; i++)
                {
                    for (int j = 0; j < -size; j++)
                    {
                        testImage.SetPixel(point2D.X + i, point2D.Y + j, Color.Blue);
                    }
                }

                pictureBox1.Image = testImage;

                DialogResult result = MessageBox.Show("Dot at correct Location?", "Check Dialog", MessageBoxButtons.YesNo);
                var success = result != DialogResult.No;

                if (!success) 
                    return false;

                for (int i = 1; i < 360; i += 1)
                {
                    // Do further testing with rotations
                    Point3D cameraPosition = ImageAnalysis.Calculate3DPosition(i, cameraInitialPosition);
                    vp = new ViewPoint(null, cameraPosition, i, dec);
                    point2D = ImageAnalysis.To2DPoint(spaceLocation, vp, kMatrix);

                    if (point2D.X < 0 || point2D.X > testImage.Width || point2D.Y < 0 || point2D.Y > testImage.Height)
                    {
                        AddLine("Tracking dots left image!");
                        return false;
                    }

                    size = -5;
                    // Draw a little marker at the given location
                    for (int j = 0; j < -size; j++)
                    {
                        for (int k = 0; k < -size; k++)
                        {
                            testImage.SetPixel(point2D.X + j, point2D.Y + k, Color.Blue);
                        }
                    }

                    //Invoke((MethodInvoker)(() =>{
                    //    pictureBox1.Image = testImage;
                    //    pictureBox1.Update();
                    //}));
                    //pictureBox1.Image = testImage;
                    //System.Threading.Thread.Sleep(1);
                }
                pictureBox1.Image = testImage;
                result = MessageBox.Show("Do the dots follow the right trajectory?", "Check Dialog", MessageBoxButtons.YesNo);
                success = result != DialogResult.No;

                return success;

            }
            catch (Exception e)
            {
                AddLine("Error in Test2D3D!");
                AddLine(e.Message);
                return false;
            }
        }

        private bool TestOctCorners()
        {
            try
            {
                bool success = false;

                OctNode root = new OctNode(100, new Point3D(0,0,0));
                //root.Split();
                List<Point3D> corners = root.GetCorners();

                List<bool> tests = new List<bool>
                {
                    corners.Contains(new Point3D(-50, 0, -50)),
                    corners.Contains(new Point3D(-50, 0, 50)),
                    corners.Contains(new Point3D(-50, 100, -50)),
                    corners.Contains(new Point3D(-50, 100, 50)),
                    corners.Contains(new Point3D(50, 0, -50)),
                    corners.Contains(new Point3D(50, 0, 50)),
                    corners.Contains(new Point3D(50, 100, -50)),
                    corners.Contains(new Point3D(50, 100, 50))
                };


                int fails = 0;
                for (int i = 0; i < tests.Count; i++)
                {
                    if (tests[i] == false)
                    {
                        AddLine("Fail on test # " + i);
                        fails++;
                    }
                }

                AddLine("TestOctCorners completed. Found " + fails + " failures.");
                if (fails == 0)
                {
                    success = true;
                }


                return success;
            }
            catch (Exception e)
            {
                AddLine("Error in TestOctCorners!");
                AddLine(e.Message);
                return false;
            }
        }

        private bool TestOctCompact()
        {
            try
            {
                bool success = false;

                OctNode root = new OctNode(100, new Point3D(0, 0, 0));
                root.Split();
                root.State = OctoState.Ambiguous;
                for (int i = 0; i < root.Children.Count; i++)
                {
                    switch (i)
                    {
                        case 0:
                            root.Children[i].State = OctoState.Full;
                            break;
                        case 1:
                            root.Children[i].State = OctoState.Empty;
                            break;
                        case 2:
                            root.Children[i].State = OctoState.Ambiguous;
                            root.Children[i].Split();
                            fillFull(root.Children[i]);
                            break;
                        case 3:
                            root.Children[i].State = OctoState.Ambiguous;
                            root.Children[i].Split();
                            fillEmpty(root.Children[i]);
                            break;
                        case 4:
                            root.Children[i].State = OctoState.Full;
                            break;
                        case 5:
                            root.Children[i].Split(); // Split but hold off on doing anything just yet
                            break;
                        case 6:
                            root.Children[i].Split();
                            foreach (var child in root.Children[i].Children)
                            {
                                child.Split();
                                fillFull(child); // Split one more time but have them all FULL
                            }
                            break;
                        case 7:
                            root.Children[i].Split();
                            foreach (var child in root.Children[i].Children)
                            {
                                child.Split();
                                fillEmpty(child); // Split one more time but have them all Empty
                            }
                            break;
                        default:
                            throw new Exception("Node has too many children!");
                    }                   
                }

                // Node 5's Children
                for (int i = 0; i < root.Children[5].Children.Count; i++)
                {
                    switch (i)
                    {
                        case 0:
                            root.Children[5].Children[i].State = OctoState.Ambiguous;
                            root.Children[5].Children[i].Split();
                            fillFull(root.Children[5].Children[i]);
                            break;
                        case 1:
                            root.Children[5].Children[i].State = OctoState.Ambiguous;
                            root.Children[5].Children[i].Split();
                            fillEmpty(root.Children[5].Children[i]);
                            break;
                        case 2:
                            root.Children[5].Children[i].State = OctoState.Full;
                            break;
                        case 3:
                            root.Children[5].Children[i].State = OctoState.Empty;
                            break;
                        case 4:
                            root.Children[5].Children[i].State = OctoState.Ambiguous;
                            root.Children[5].Children[i].Split();
                            fillEmpty(root.Children[5].Children[i]);
                            root.Children[5].Children[i].Children[0].State = OctoState.Full; // all but one is empty
                            break;
                        case 5:
                            root.Children[5].Children[i].State = OctoState.Ambiguous;
                            root.Children[5].Children[i].Split();
                            fillFull(root.Children[5].Children[i]);
                            root.Children[5].Children[i].Children[1].State = OctoState.Empty; // all but one is empty
                            break;
                        case 6:
                            root.Children[5].Children[i].Split();
                            foreach (var child in root.Children[5].Children[i].Children)
                            {
                                child.Split();
                                fillFull(child); // Split one more time but have them all FULL
                            }
                            break;
                        case 7:
                            root.Children[5].Children[i].Split();
                            foreach (var child in root.Children[5].Children[i].Children)
                            {
                                child.Split();
                                fillEmpty(child); // Split one more time but have them all Empty
                            }
                            break;
                        default:
                            throw new Exception("Node has too many children!");
                    }
                }

                root.Compact();

                // Setup tests
                List<bool> tests = new List<bool>();

                tests.Add(root.Children.Count == 8);
                tests.Add(root.State == OctoState.Ambiguous);
                tests.Add(root.Children[0].State == OctoState.Full);
                tests.Add(root.Children[0].Children == null);
                tests.Add(root.Children[1].State == OctoState.Empty);
                tests.Add(root.Children[1].Children == null);
                tests.Add(root.Children[2].State == OctoState.Full);
                tests.Add(root.Children[2].Children == null);
                tests.Add(root.Children[3].State == OctoState.Empty);
                tests.Add(root.Children[3].Children == null);
                tests.Add(root.Children[4].State == OctoState.Full);
                tests.Add(root.Children[4].Children == null);
                tests.Add(root.Children[5].State == OctoState.Ambiguous);
                tests.Add(root.Children[5].Children.Count == 8);
                tests.Add(root.Children[5].Children[0].State == OctoState.Full);
                tests.Add(root.Children[5].Children[0].Children == null);
                tests.Add(root.Children[5].Children[1].State == OctoState.Empty);
                tests.Add(root.Children[5].Children[1].Children == null);
                tests.Add(root.Children[5].Children[2].State == OctoState.Full);
                tests.Add(root.Children[5].Children[2].Children == null);
                tests.Add(root.Children[5].Children[3].State == OctoState.Empty);
                tests.Add(root.Children[5].Children[3].Children == null);
                tests.Add(root.Children[5].Children[4].State == OctoState.Ambiguous);
                tests.Add(root.Children[5].Children[4].Children.Count == 8);
                tests.Add(root.Children[5].Children[4].Children[0].State == OctoState.Full);
                tests.Add(root.Children[5].Children[4].Children[1].State == OctoState.Empty);
                tests.Add(root.Children[5].Children[5].State == OctoState.Ambiguous);
                tests.Add(root.Children[5].Children[5].Children.Count == 8);
                tests.Add(root.Children[5].Children[5].Children[0].State == OctoState.Full);
                tests.Add(root.Children[5].Children[5].Children[1].State == OctoState.Empty);
                tests.Add(root.Children[5].Children[6].State == OctoState.Full);
                tests.Add(root.Children[5].Children[6].Children == null);
                tests.Add(root.Children[5].Children[7].State == OctoState.Empty);
                tests.Add(root.Children[5].Children[7].Children == null);
                tests.Add(root.Children[6].State == OctoState.Full);
                tests.Add(root.Children[6].Children == null);
                tests.Add(root.Children[7].State == OctoState.Empty);
                tests.Add(root.Children[7].Children == null);


                int fails = 0;
                for (int i = 0; i < tests.Count; i++)
                {
                    if (tests[i] == false)
                    {
                        AddLine("Fail on test # " + i);
                        fails++;
                    }
                }

                AddLine("TestOctCompact completed. Found " + fails + " failures.");
                if (fails == 0)
                {
                    success = true;
                }

                return success;
            }
            catch (Exception e)
            {
                AddLine("Error in TestOctCompact!");
                AddLine(e.Message);
                return false;
            }
        }

        private bool TestOctExport()
        {
            try
            {
                bool success = false;

                OctNode root = new OctNode(100, new Point3D(0, 0, 0));
                root.Split();
                root.State = OctoState.Ambiguous;
                root.Children[0].State = OctoState.Full;
                root.Children[1].State = OctoState.Empty;
                root.Children[2].State = OctoState.Full;
                root.Children[3].State = OctoState.Full;
                root.Children[4].State = OctoState.Empty;
                root.Children[5].State = OctoState.Full;
                root.Children[6].State = OctoState.Full;
                root.Children[7].State = OctoState.Full;                

                using(StreamWriter file = new StreamWriter(@"C:\Users\tttakaha\Desktop\OctreeTest.txt"))
                {
                    OctNode.ExportTree(root, file);
                }


                DialogResult result = MessageBox.Show("Take a look at the file generated on the desktop. If everything looks ok then click OK", "Check Dialog", MessageBoxButtons.YesNo);
                success = result != DialogResult.No;

                return success;
            }
            catch (Exception e)
            {
                AddLine("Error in TestOctExport!");
                AddLine(e.Message);
                return false;
            }
        }

        private bool TestInSillhouette()
        {
            try
            {
                bool success = false;

                double dec = 10.3;

                // Constants
                Point3D cameraInitialPosition = new Point3D(0, 100, -370);
                double[,] kMatrix = new double[3, 3] { { 1095.826, 0, 351.289 }, { 0, 1112.102, 678.420 }, { 0, 0, 1 } };

                // Call 3D->2D on an image with a known pixel location
                string path = @"C:\Users\tttakaha\Downloads\Visual Hull Project\Sillhouettes\P270S.png";
                // Read in test image
                Bitmap testImage = new Bitmap(path);
                int[,] binImage = ImageAnalysis.ConvertBinary(testImage);

                ViewPoint vp = new ViewPoint(binImage, cameraInitialPosition, 0, dec);

                // Do the main octree work
                var octoQueue = new Queue<OctNode>();

                OctNode root = new OctNode(150, new Point3D(0, 0, 0));
                double currentSideLength = 150;
                double MinSideLength = 1;

                // Add octree nodes and enqueue
                root.Split();
                foreach (var child in root.Children)
                {
                    child.Split();
                    foreach (var grandchild in child.Children)
                    {
                        grandchild.Split();
                        foreach (var greatGrandchild in grandchild.Children)
                        {
                            greatGrandchild.Split();
                            octoQueue.Enqueue(greatGrandchild);                            
                        }
                    }
                }

                OctoState state;
                int size = -3;
                while (octoQueue.Count != 0)
                {
                    OctNode node = octoQueue.Dequeue();
                    currentSideLength = node.SideLength;
                    if (currentSideLength <= MinSideLength)
                        break;

                    // Extracted is in sillhouette method for visualization
                    bool allIn = true;
                    bool allOut = true;
                    foreach (var corner in node.GetCorners())
                    {
                        Point p = ImageAnalysis.To2DPoint(corner, vp, kMatrix);
                        if (p.Y > vp.ImageMap.GetLength(0) || p.X > vp.ImageMap.GetLength(1))
                            throw new IndexOutOfRangeException();

                        if (vp.ImageMap[p.Y, p.X] >= 1)
                        {
                            for (int i = 0; i < -size; i++)
                            {
                                for (int j = 0; j < -size; j++)
                                {
                                    testImage.SetPixel(p.X + i, p.Y + j, Color.Blue);
                                }
                            }
                            //The Point is in sillhouette region
                            allOut = false;
                        }
                        else
                        {
                            for (int i = 0; i < -size; i++)
                            {
                                for (int j = 0; j < -size; j++)
                                {
                                    testImage.SetPixel(p.X + i, p.Y + j, Color.Red);
                                }
                            }
                            allIn = false;
                        }
                        if (!allIn && !allOut)
                            break;
                    }

                    //Invoke((MethodInvoker)(() => {
                    //    pictureBox1.Image = testImage;
                    //    pictureBox1.Update();
                    //}));
                    //pictureBox1.Image = testImage;
                    //System.Threading.Thread.Sleep(1);

                    if (allIn && !allOut)
                    {
                        node.State = OctoState.Full;
                    }
                    else 
                        node.State = OctoState.Empty;

                }
                pictureBox1.Image = testImage;
                // end extraction
                using (StreamWriter file = new StreamWriter(@"C:\Users\tttakaha\Desktop\OctreeTest2.txt"))
                {
                    OctNode.ExportTree(root, file);
                }

                var result = MessageBox.Show("All blue dots are within the sillhouette image?", "Check Dialog", MessageBoxButtons.YesNo);
                success = result != DialogResult.No;

                return success;

            }
            catch (Exception e)
            {
                AddLine("Error in TestInSillhouette!");
                AddLine(e.Message);
                return false;
            }
        }

        private bool TestMultiImageConsistency()
        {
            try
            {
                bool success = false;

                double dec = 10.3;

                // Constants
                Point3D cameraInitialPosition = new Point3D(0, 100, -370);
                double[,] kMatrix = new double[3, 3] { { 1095.826, 0, 351.289 }, { 0, 1112.102, 678.420 }, { 0, 0, 1 } };

                string path1 = @"C:\Users\tttakaha\Downloads\Visual Hull Project\Sillhouettes\P0S.png";
                string path2 = @"C:\Users\tttakaha\Downloads\Visual Hull Project\Sillhouettes\P30S.png";
                string path3 = @"C:\Users\tttakaha\Downloads\Visual Hull Project\Sillhouettes\P100S.png";
                string path4 = @"C:\Users\tttakaha\Downloads\Visual Hull Project\Sillhouettes\P210S.png";

                string path5 = @"C:\Users\tttakaha\Downloads\Visual Hull Project\Sillhouettes\P0.jpg";
                string path6 = @"C:\Users\tttakaha\Downloads\Visual Hull Project\Sillhouettes\P30.jpg";
                string path7 = @"C:\Users\tttakaha\Downloads\Visual Hull Project\Sillhouettes\P100.jpg";
                string path8 = @"C:\Users\tttakaha\Downloads\Visual Hull Project\Sillhouettes\P210.jpg";

                Bitmap testImage1 = new Bitmap(path1);
                Bitmap testImage2 = new Bitmap(path2);
                Bitmap testImage3 = new Bitmap(path3);
                Bitmap testImage4 = new Bitmap(path4);
                Bitmap testImage5 = new Bitmap(path5);
                Bitmap testImage6 = new Bitmap(path6);
                Bitmap testImage7 = new Bitmap(path7);
                Bitmap testImage8 = new Bitmap(path8);

                int[,] binImage1 = ImageAnalysis.ConvertBinary(testImage1);
                int[,] binImage2 = ImageAnalysis.ConvertBinary(testImage2);
                int[,] binImage3 = ImageAnalysis.ConvertBinary(testImage3);
                int[,] binImage4 = ImageAnalysis.ConvertBinary(testImage4);

                Point3D position1 = ImageAnalysis.Calculate3DPosition(0, cameraInitialPosition);
                Point3D position2 = ImageAnalysis.Calculate3DPosition(30, cameraInitialPosition);
                Point3D position3 = ImageAnalysis.Calculate3DPosition(100, cameraInitialPosition);
                Point3D position4 = ImageAnalysis.Calculate3DPosition(210, cameraInitialPosition);

                ViewPoint vp1 = new ViewPoint(binImage1, position1, 0, dec);
                ViewPoint vp2 = new ViewPoint(binImage2, position2, 30, dec);
                ViewPoint vp3 = new ViewPoint(binImage3, position3, 100, dec);
                ViewPoint vp4 = new ViewPoint(binImage4, position4, 210, dec);

                // Do the main octree work
                OctNode root = new OctNode(150, new Point3D(0, 0, 0));

                // Add octree nodes and enqueue
                root.Split();
                foreach (var child in root.Children)
                {
                    child.Split();
                    foreach (var grandchild in child.Children)
                    {
                        grandchild.Split();
                        foreach (var greatGrandchild in grandchild.Children)
                        {
                            greatGrandchild.Split();
                        }
                    }
                }

                int size = -5;
                OctNode node = root.Children[0];

                foreach (var corner in node.GetCorners())
                {
                    Point p1 = ImageAnalysis.To2DPoint(corner, vp1, kMatrix);
                    Point p2 = ImageAnalysis.To2DPoint(corner, vp2, kMatrix);
                    Point p3 = ImageAnalysis.To2DPoint(corner, vp3, kMatrix);
                    Point p4 = ImageAnalysis.To2DPoint(corner, vp4, kMatrix);

                    for (int i = 0; i < -size; i++)
                    {
                        for (int j = 0; j < -size; j++)
                        {
                            testImage5.SetPixel(p1.X + i, p1.Y + j, Color.Blue);
                        }
                    }
                    for (int i = 0; i < -size; i++)
                    {
                        for (int j = 0; j < -size; j++)
                        {
                            testImage6.SetPixel(p2.X + i, p2.Y + j, Color.Blue);
                        }
                    }
                    for (int i = 0; i < -size; i++)
                    {
                        for (int j = 0; j < -size; j++)
                        {
                            testImage7.SetPixel(p3.X + i, p3.Y + j, Color.Blue);
                        }
                    }
                    for (int i = 0; i < -size; i++)
                    {
                        for (int j = 0; j < -size; j++)
                        {
                            testImage8.SetPixel(p4.X + i, p4.Y + j, Color.Blue);
                        }
                    }
                }
                pictureBox1.Image = testImage5;
                pictureBox2.Image = testImage6;
                pictureBox3.Image = testImage7;
                pictureBox4.Image = testImage8;

                var result = MessageBox.Show("Blue square appears in correct location in all images?", "Check Dialog", MessageBoxButtons.YesNo);
                success = result != DialogResult.No;

                return success;

            }
            catch (Exception e)
            {
                AddLine("Error in TestMultiImageConsistency!");
                AddLine(e.Message);
                return false;
            }
        }

        #region TestHelperFunctions
        /// <summary>
        /// Helper function for filling a nodes children with the "full" state
        /// </summary>
        /// <remarks>
        /// Children should be generated before calling this function
        /// </remarks>
        private void fillFull(OctNode node)
        {
            if (node.Children == null)
                return;
            foreach(var child in node.Children)
            {
                child.State = OctoState.Full;
            }
        }

        /// <summary>
        /// Helper function for filling a nodes children with the "empty" state
        /// </summary>
        /// <remarks>
        /// Children should be generated before calling this function
        /// </remarks>
        private void fillEmpty(OctNode node)
        {
            if (node.Children == null)
                return;
            foreach (var child in node.Children)
            {
                child.State = OctoState.Empty;
            }
        }

        /// <summary>
        /// Helper function for filling a nodes children with a random state
        /// </summary>
        /// <remarks>
        /// Children should be generated before calling this function
        /// </remarks>
        private void fillRandom(OctNode node)
        {
            Random rand = new Random();

            if (node.Children == null)
                return;
            foreach (var child in node.Children)
            {
                int number = rand.Next(1, 1000);
                switch (number % 3)
                {
                    case 0:
                        child.State = OctoState.Full;
                        break;
                    case 1:
                        child.State = OctoState.Empty;
                        break;
                    case 2:
                        child.State = OctoState.Ambiguous;
                        break;
                    default:
                        child.State = OctoState.Full;
                        break;
                }
            }
        }
        #endregion
    }
}
