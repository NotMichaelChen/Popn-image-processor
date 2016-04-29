/*
 * User: guest1
 * Date: 4/28/2016
 */
using System;
using Popn_image_processor.ImageProcessor;

namespace Popn_image_processor
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            
            ImageDetector id = new ImageDetector("C:\\Users\\guest1\\Downloads\\parapara[hyper].png");
            
            Console.Write("Press any key to continue . . . ");
            Console.ReadKey(true);
        }
    }
}