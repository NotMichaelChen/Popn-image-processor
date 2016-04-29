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
        Bitmap chart;
        //Uses bit-mask of length 9 to store notes in 1/4 intervals
        List<int> notes;
        //Contains useful colors in each pop'n image
        readonly Dictionary<string, Color> colors;
        
        /// <summary>
        /// Constructs an object to extract notes from a pop'n image
        /// </summary>
        /// <param name="imagefile">Path of the image file</param>
        public ImageDetector(string imagefile)
        {
            chart = new Bitmap(imagefile);
            notes = new List<int>();
            colors = new Dictionary<string, Color>();
            this.GenerateColors();
        }
        
        /// <summary>
        /// Gets all of the notes in the image file
        /// </summary>
        /// <returns>An array of bitmasks in 1/4 intervals</returns>
        public int[] GetNotes()
        {
            throw new NotImplementedException();
        }
        
        //Fills up our dictionary of colors
        private void GenerateColors()
        {
            colors["GREY"] = Color.FromArgb(195, 195, 195);
            colors["BLACK"] = Color.FromArgb(0, 0, 0);
            colors["WHITE"] = Color.FromArgb(255, 255, 255);
        }
        
        //Finds the bottom-left most point on the first track
        private Point GetStartingPoint()
        {
            //Pixel column to start searching from
            const int searchstart = 30;
            
            int row = 0;
            //In column searchstart, move down the image until a (horizontal) line is found
            while(chart.GetPixel(searchstart, row) != colors["GREY"] ||
                  chart.GetPixel(searchstart+1, row) != colors["GREY"] ||
                  chart.GetPixel(searchstart-1, row) != colors["GREY"])
            {
                row++;
            }
            
            int column = searchstart;
            
            //Go right until a column is found
            while(chart.GetPixel(column, row) != colors["BLACK"])
            {
                column++;
            }
            
            //Go down until the last line is found
            while(row + 8 < chart.Height && chart.GetPixel(column, row + 8) != colors["WHITE"])
            {
                row += 8;
            }
            
            if(row > chart.Height - 8)
                throw new ArgumentException("Error: image provided is invalid");
            
            //Go left until the last pixel in the line is found
            while(chart.GetPixel(column-1, row) == colors["BLACK"])
            {
                column--;
            }
            
            return new Point(column, row);
        }
    }
}
