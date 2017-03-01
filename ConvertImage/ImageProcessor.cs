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
    class ImageProcessor
    {
        public void ProcessImages(string pptImageFolder)
        {
            string imgWidth, imgHeight;
            imgWidth = "637";
            imgHeight = "830";
            Stopwatch s = new Stopwatch();
            s.Start();
            foreach (string fileStr in Directory.GetFiles(pptImageFolder, "*.EMF"))
            {
                //s.Restart();
                ConvertImage(fileStr, imgWidth, imgHeight, pptImageFolder, Path.GetFileNameWithoutExtension(fileStr).ToLower());
                //s.Stop();
                //Console.WriteLine(s.ElapsedMilliseconds);
            }
            s.Stop();
            Console.WriteLine(s.ElapsedMilliseconds);
        }

        public void ProcessImagesMultiThreaded(string pptImageFolder)
        {
            string imgWidth, imgHeight;
            imgWidth = "637";
            imgHeight = "830";
            Stopwatch s = new Stopwatch();
            s.Start();
            List<Task> runningTasks = new List<Task>();
            foreach (string fileStr in Directory.GetFiles(pptImageFolder, "*.EMF"))
            {
                runningTasks.Add(Task.Factory.StartNew(new Action(() =>
                {
                    ConvertImage(fileStr, imgWidth, imgHeight, pptImageFolder, Path.GetFileNameWithoutExtension(fileStr).ToLower());
                })));

            }
            Task.WaitAll(runningTasks.ToArray());
            s.Stop();
            Console.WriteLine(s.ElapsedMilliseconds);
        }

        private void ConvertImage(string sourceImgPath, string width, string height, string outputDirectory, string imageFilename)
        {
            // uses Magick.Net (The .NET wrapper for the ImageMagick library)
            // URL: https://magick.codeplex.com/
            //
            //convert the image here
            MagickReadSettings settings = new MagickReadSettings();
            // Settings the density to 600 dpi will create an image with a better quality
            settings.Density = new Density(600);

            // if we are going to resize the image then we need to
            // set OpenCL.IsEnabled = false to resolve the driver related issues;
            OpenCL.IsEnabled = false;

            using (MagickImage image = new MagickImage(sourceImgPath, settings))
            {
                //resize the image
                //When zero is specified for the height, the height will be calculated with the aspect ratio.
                image.Resize(Convert.ToInt32(width), Convert.ToInt32(height));

                // Save as jpg/gif
                image.Write(Path.Combine(outputDirectory, $"{imageFilename}.gif"));
            }

        }
    }
}
