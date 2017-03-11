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
        static public Image BoundingSquaresCalc(Image im)
        {

            return null;
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
    }
}
