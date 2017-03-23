using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualHullReconstruction
{
    static class ImageAnalysis
    {
        /// <summary>
        /// Extracts the silhouette image with bounding squares calculated
        /// </summary>
        /// <param name="refImage">The reference image of the background</param>
        /// <param name="image">The image with the object present</param>
        /// <returns>The ready to use sihouette image with bounding squares calculated</returns>
        static public Image ExtractSilhouette(Image refImage, Image image)
        {
            // Set to grayscale?
            // foreach pixel in image
            // subtract background value (anything below threshold set to 0)
            // anything left should be set to binary1

            // might as well do bounding square calculation as well...
            // image = boundingSquareCalc(silhouetteIm);
            return null;
        }

        /// <summary>
        /// Calculates the max bounding square size for each pixel
        /// </summary>
        /// <param name="im">The Binary silhouette image</param>
        /// <returns>the image where the value of the pixel indicates the max square size</returns>
        static public int[,] BoundingSquaresCalc(int[,] im, int width, int height)
        {
            for (int i = width-2; i >= 0; i--)
            {
                for (int j = 1; j < height; j++)
                {
                    if (im[j, i] == 1)
                    {
                        im[j, i] = Min(im[j - 1, i], im[j, i + 1], im[j - 1, i + 1]) + 1;
                    }
                }
            }
            return im;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="im"></param>
        /// <returns></returns>
        static public int[,] ConvertBinary(Bitmap im)
        {
            int[,] imarray = new int[im.Height,im.Width];
            for (int i = 0; i < im.Width; i++)
            {
                for (int j = 0; j < im.Height; j++)
                {
                    if (im.GetPixel(i, j).Name.ToLower() != "ffffffff")
                    {
                        imarray[j, i] = 1;
                    }
                }
            }
            return imarray;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        static public int Min(int a, int b, int c)
        {
            return Math.Min(a, Math.Min(b, c));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        /// <remarks>http://stackoverflow.com/questions/2265910/convert-an-image-to-grayscale</remarks>
        static public Bitmap ConvertGreyscale(Bitmap original)
        {
            //create a blank bitmap the same size as original
            var newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][] 
                  {
                     new float[] {.3f, .3f, .3f, 0, 0},
                     new float[] {.59f, .59f, .59f, 0, 0},
                     new float[] {.11f, .11f, .11f, 0, 0},
                     new float[] {0, 0, 0, 1, 0},
                     new float[] {0, 0, 0, 0, 1}
                  });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }

        /// <summary>
        /// Calculates the camera location based on the given angle
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="camInitialPosition"></param>
        /// <returns></returns>
        static public Point3D Calculate3DPosition(double angle, Point3D camInitialPosition)
        {
            double angleRad = (Math.PI / 180) * (180 - angle);

            double x = Math.Abs(camInitialPosition.Z) * Math.Sin(angleRad);             
            double z = Math.Abs(camInitialPosition.Z) * Math.Cos(angleRad);

            //check for very small inputs
            x = (x <= 1e-5 && x >= -1e-5) ? 0 : x;
            z = (z <= 1e-5 && z >= -1e-5) ? 0 : z;
            return new Point3D(x, camInitialPosition.Y, z);
        }

        /// <summary>
        /// Converts a 3D point in space to a 2D pixel location on an image plane
        /// </summary>
        /// <param name="p">point in 3d space to project onto pixel coordinates</param>
        /// <param name="viewPoint">viewpoint of the camera</param>
        /// <param name="kmatrix">camera calibration matrix</param>
        /// <returns>A pixel location</returns>
        static public Point To2DPoint(Point3D p, ViewPoint viewPoint, double[,] kmatrix)
        {
            Point point2D = new Point();
            double beta = (Math.PI / 180)*viewPoint.ViewAngle; // beta = y axis rotation angle
            double dec = -(Math.PI/180)*viewPoint.Declination;

            // R Matrix Variables
            double R11 = Math.Cos(beta);
            double R12 = Math.Sin(beta) * Math.Sin(dec);
            double R13 = Math.Sin(beta) * Math.Cos(dec);
            double R21 = 0;
            double R22 = Math.Cos(dec);
            double R23 = -Math.Sin(dec);
            double R31 = -Math.Sin(beta);
            double R32 = Math.Cos(beta)*Math.Sin(dec);
            double R33 = Math.Cos(beta)*Math.Cos(dec);

            // Multiply K Matrix to get P variables
            double p11 = kmatrix[0, 0]*R11 + kmatrix[0,1]*R21 + kmatrix[0, 2]*R31;
            double p12 = kmatrix[0, 0]*R12 + kmatrix[0, 1]*R22 + kmatrix[0, 2]*R32;
            double p13 = kmatrix[0, 0]*R13 + kmatrix[0, 1]*R23 + kmatrix[0, 2]*R33;
            double p14 = kmatrix[0, 0]*viewPoint.Position.X + kmatrix[0, 1]*viewPoint.Position.Y + kmatrix[0, 2]*viewPoint.Position.Z;
            double p21 = kmatrix[0, 1]*R21 + kmatrix[0, 2]*R31;
            double p22 = kmatrix[0, 1]*R22 + kmatrix[0, 2]*R32;
            double p23 = kmatrix[0, 1]*R23 + kmatrix[0, 2]*R33;
            double p24 = kmatrix[0, 1]*viewPoint.Position.Y + kmatrix[0, 2]*viewPoint.Position.Z;
            double p31 = R31;
            double p32 = R32;
            double p33 = R33;
            double p34 = viewPoint.Position.Z;

            // Perform calculations for x and y pixels
            point2D.X = Convert.ToInt32(p11*p.X + p12*p.Y + p13*p.Z + p14);
            point2D.Y = Convert.ToInt32(p21*p.X + p22*p.Y + p23*p.Z + p24);
            bool check = (1 == Convert.ToInt32(p31*p.X + p32*p.Y + p33*p.Z + p34));

            return point2D;
        }
    }
}
