using AForge.Imaging.Filters;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using turbojpegCLI;

namespace NTR_ViewerPlus {

    public class QueueTexture {

        public int width;
        public int height;
        public byte[] data;

        public QueueTexture() {
        }

        public void Load(int width, int height, byte[] data) {
            this.width = width;
            this.height = height;
            if (width > 200) Filter.Run(ref data, width);
            this.data = data;
        }

    }
}