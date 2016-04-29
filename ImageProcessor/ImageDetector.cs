/*
 * Created by SharpDevelop.
 * User: guest1
 * Date: 4/28/2016
 * Time: 10:02 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;

namespace Popn_image_processor.ImageProcessor
{
    /// <summary>
    /// Extracts notes from a pop'n music image chart
    /// </summary>
    public class ImageDetector
    {
        private Bitmap chart;
        
        public ImageDetector(string imagefile)
        {
            chart = new Bitmap(imagefile);
        }
    }
}
