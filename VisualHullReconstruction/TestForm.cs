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
                bool success = false;
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
                success = result != DialogResult.No;

                if (success)
                {
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

                        pictureBox1.Image = testImage;
                    }
                    result = MessageBox.Show("Do the dots follow the right trajectory?", "Check Dialog", MessageBoxButtons.YesNo);
                    success = result != DialogResult.No;
                }

                return success;

            }
            catch (Exception e)
            {
                AddLine("Error in Test2D3D!");
                AddLine(e.Message);
                return false;
            }
        }
    }
}
