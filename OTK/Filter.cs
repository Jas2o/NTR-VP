using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NTR_ViewerPlus {
    class Filter {

        private static Bitmap bmpDespeckleTop = new Bitmap(240, 400, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
        private static Bitmap bmpDespeckleBottom = new Bitmap(240, 320, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

        public enum Type { None, AForgeLite, Magick };
        private static BilateralSmoothing AForgeFilter = new BilateralSmoothing();
        private static GaussianSharpen AForgeFilter2 = new GaussianSharpen();
        private static ImageMagick.MagickImage magick;

        public static int UseFilter = 0;

        public static void Run(ref byte[] decomp, int width) {
            BitmapData bmpDespeckle;

            if (width == 400)
                bmpDespeckle = bmpDespeckleTop.LockBits(new Rectangle(0, 0, bmpDespeckleTop.Width, bmpDespeckleTop.Height), ImageLockMode.ReadWrite, bmpDespeckleTop.PixelFormat);
            else
                bmpDespeckle = bmpDespeckleBottom.LockBits(new Rectangle(0, 0, bmpDespeckleBottom.Width, bmpDespeckleBottom.Height), ImageLockMode.ReadWrite, bmpDespeckleBottom.PixelFormat);

            Marshal.Copy(decomp, 0, bmpDespeckle.Scan0, decomp.Length);

            if ((Type)UseFilter == Type.AForgeLite) {
                // Very close to Magick and faster!
                AForgeFilter.KernelSize = 3;
                AForgeFilter.ColorFactor = 10;

                AForgeFilter.ApplyInPlace(bmpDespeckle);

                // Don't make a new array for decomp, will make it slow.
                Marshal.Copy(bmpDespeckle.Scan0, decomp, 0, decomp.Length);

            } else if ((Type)UseFilter == Type.Magick) {
                //Magick is SLOWWWW
                magick = new ImageMagick.MagickImage();

                if (width == 400)
                    magick.Read(bmpDespeckleTop);
                else
                    magick.Read(bmpDespeckleBottom);

                magick.Despeckle();
                decomp = magick.ToByteArray(ImageMagick.MagickFormat.Bgr); //Use in place of bmpData.Scan0, bloody slow

                magick.Dispose();
            }

            if (width == 400)
                bmpDespeckleTop.UnlockBits(bmpDespeckle);
            else
                bmpDespeckleBottom.UnlockBits(bmpDespeckle);
        }

    }
}
