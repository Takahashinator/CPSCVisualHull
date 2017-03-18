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
            Tuple<bool,string> pass = new Tuple<bool, string>(false, "");

            pass = TestConvertBinary();
            AddLine("Test Convert Binary... " + (pass.Item1? "success!" : "failed!"));
            if (pass.Item1 == false)
                AddLine(pass.Item2);

            pass = TestBoundingSquares();
            AddLine("Test Bounding Squares... " + (pass.Item1 ? "success!" : "failed!"));
            if (pass.Item1 == false)
                AddLine(pass.Item2);

        }

        private void AddLine(string text)
        {
            textBox1.Text += text + Environment.NewLine;
        }

        private Tuple<bool, string> TestConvertBinary()
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
                Tuple<bool, string> success = new Tuple<bool, string>(false, "");
                // Read in test image
                Bitmap testImage = new Bitmap(Directory.GetCurrentDirectory() + "testBitmap.png");
                // Convert
                int[,] intArray = ImageAnalysis.ConvertBinary(testImage);
                // Check for failures

                return success;
            }
            catch (Exception e)
            {
                AddLine("Error in TestConvertBinary!");
                return new Tuple<bool, string>(false, e.Message);
            }
        }

        private Tuple<bool, string> TestBoundingSquares()
        {
            try
            {
                Tuple<bool, string> success = new Tuple<bool, string>(false, "");

            }
            catch (Exception e)
            {
                AddLine("Error in TestBoundingSquares!");
                return new Tuple<bool, string>(false, e.Message);
            }
        }
    }
}
