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
        readonly Bitmap chart;
        //Contains useful colors in each pop'n image
        readonly Dictionary<string, Color> colors;
        
        /// <summary>
        /// Constructs an object to extract notes from a pop'n image
        /// </summary>
        /// <param name="imagefile">Path of the image file</param>
        public ImageDetector(string imagefile)
        {
            chart = new Bitmap(imagefile);
            colors = new Dictionary<string, Color>();
            this.GenerateColors();
            
            int[] test = GetNotes();
        }
        
        /// <summary>
        /// Gets all of the notes in the image file
        /// </summary>
        /// <returns>An array of bitmasks in 1/4 intervals</returns>
        public int[] GetNotes()
        {
            //Uses bit-mask of length 9 to store notes in 1/4 intervals
            List<int> notes = new List<int>();
            
            Point leftstart = new Point(30, 0);
            Point movingstart;
            
            while(leftstart.Y < chart.Height && leftstart.X > 0)
            {
                leftstart = GetStartingPoint(leftstart);
                movingstart = leftstart;
                
                while(movingstart.X < chart.Width)
                {
                    if(chart.GetPixel(movingstart.X, movingstart.Y) == colors["WHITE"])
                    {
                        while(movingstart.Y > 0 && chart.GetPixel(movingstart.X, movingstart.Y) == colors["WHITE"])
                            movingstart.Y--;
                        
                        if(movingstart.Y <= 0)
                            break;
                    }
                    
                    movingstart = FindBottom(movingstart);
                    notes.AddRange(GetTrackNotes(GetColumns(movingstart)));
                    movingstart.X += 130;
                }
                
                leftstart = FindLowerLine(leftstart);
            }
            
            return notes.ToArray();
        }
        
        //Fills up our dictionary of colors
        private void GenerateColors()
        {
            colors["GREY"] = Color.FromArgb(195, 195, 195);
            colors["BLACK"] = Color.FromArgb(0, 0, 0);
            colors["WHITE"] = Color.FromArgb(255, 255, 255);
        }
        
        //Finds the bottom-left most point on the track
        private Point GetStartingPoint(Point searchstart)
        {   
            int row = searchstart.Y;
            //In column searchstart, move down the image until a (horizontal) line is found
            while(chart.GetPixel(searchstart.X, row) != colors["GREY"] &&
                  chart.GetPixel(searchstart.X + 1, row) != colors["GREY"] &&
                  chart.GetPixel(searchstart.X - 1, row) != colors["GREY"])
            {
                bool test = chart.GetPixel(searchstart.X, row) == colors["GREY"];
                row++;
            }
            
            int column = searchstart.X;
            
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
        
        //Gets a list of every column in the current track, given the start point
        //(the leftmost point of the first line on the track)
        private Point[] GetColumns(Point start)
        {
            List<Point> columns = new List<Point>();
            
            //First point is a special case, so add it separately
            start.X += 8;
            columns.Add(new Point(start.X, start.Y));
            
            for(int i = 0; i < 8; i++)
            {
                start.X += 13;
                columns.Add(new Point(start.X, start.Y));
            }
            
            return columns.ToArray();
        }
        
        //Gets all of the notes on the given track and returns a list of bitmasks
        //for each 1/4 step
        private int[] GetTrackNotes(Point[] columns)
        {
            List<int> tracknotes = new List<int>();
            
            if(columns.Length != 9)
                throw new ArgumentException("Error: invalid number of columns: " + columns.Length);
            
            int firstcol = columns[0].X;
            int line = columns[0].Y;
            
            //While there are still lines in the track
            while(chart.GetPixel(firstcol, line) == colors["BLACK"])
            {
                //Holds the bitmask for the current line
                int notesum = 0;
                //Move each column to the next line and check for a note
                for(int i = 0; i < columns.Length; i++)
                {
                    columns[i].Y = line;
                    //Use the position in the array to determine which index of the bitmaks to assign to
                    if(chart.GetPixel(columns[i].X + 1, columns[i].Y + 3) == colors["BLACK"])
                        notesum += (int)Math.Pow(2.0, i);
                }
                tracknotes.Add(notesum);
                
                //Move up a line (minus is moving up)
                line -= 8;
            }
            
            return tracknotes.ToArray();
        }
        
        //Finds the bottom-most line of a track
        //"edge" MUST be on a line, if it's not then an exception is thrown
        private Point FindBottom(Point edge)
        {
            if(chart.GetPixel(edge.X, edge.Y) != colors["BLACK"])
                throw new ArgumentException("Error: adjacent track not found");
            
            while(chart.GetPixel(edge.X, edge.Y + 1) == colors["BLACK"])
                edge.Y++;
            
            return edge;
        }
        
        //Finds the track below the current track, noted by start
        //returns an invalid point if there is no bottom track 
        private Point FindLowerLine(Point start)
        {
            start.Y++;
            while(start.Y < chart.Height && chart.GetPixel(start.X, start.Y) != colors["BLACK"])
                    start.Y++;
            if(start.Y == chart.Height)
                return new Point(-1, -1);
            else
                return start;
        }
    }
}
