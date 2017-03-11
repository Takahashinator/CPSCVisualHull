using System;
using System.Collections.Generic;
using System.Drawing;
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
            
        }
    }
}
