/*
 * User: guest1
 * Date: 4/28/2016
 */
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Popn_image_processor.ImageProcessor
{
    /// <summary>
    /// Extracts notes from a pop'n music image chart
    /// </summary>
    public class ImageDetector
    {
        private Bitmap chart;
        //Uses bit-mask of length 9 to store notes in 1/4 intervals
        private List<int> notes;
        
        /// <summary>
        /// Constructs an object to extract notes from a pop'n image
        /// </summary>
        /// <param name="imagefile">Path of the image file</param>
        public ImageDetector(string imagefile)
        {
            chart = new Bitmap(imagefile);
            notes = new List<int>();
        }
        
        /// <summary>
        /// Gets all of the notes in the image file
        /// </summary>
        /// <returns>An array of bitmasks in 1/4 intervals</returns>
        public int[] GetNotes()
        {
            throw new NotImplementedException();
        }
    }
}
