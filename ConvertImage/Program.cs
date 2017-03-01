using ImageMagick;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertImage
{
    class Program
    {
        static void Main(string[] args)
        {
            ImageProcessor imgProcessor = new ImageProcessor();
            //imgProcessor.ProcessImages(Environment.CurrentDirectory);
            imgProcessor.ProcessImagesMultiThreaded(Environment.CurrentDirectory);
        }        
    }
}
