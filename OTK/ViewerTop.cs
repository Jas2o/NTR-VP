﻿using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using AForge.Imaging.Filters;
using System.Collections.Generic;
using TexLib;

namespace NTR_ViewerPlus {
    public class ViewerTop : GameWindow {

        public static QueueTexture qtTop = new QueueTexture();
        public static QueueTexture qtOverlay2d = new QueueTexture();

        int textureTop, textureOverlay2d;
        private static Font arial = new Font("Arial", 20);

        int screenWidth = 0, screenHeight = 0, virtualWidth = 400, virtualHeight = 240;
        float targetAspectRatio;

        static string title = "NTR Top";
        bool displayFPS;
        Bitmap overlay2d;
        byte[] bData;
        FPSCounter fps;

        Vector2[] vertBufferScreen, vertBufferFps;
        int VBOScreen, VBOFps;

        public ViewerTop(int width, int height) : base(width, height, GraphicsMode.Default, title) {
            screenWidth = width; // Doesn't really matter, gets changed later anyway
            screenHeight = height;

            this.Y -= 140;

            #region Init Textures
            //FPS
            overlay2d = new Bitmap(200, 100);
            textureOverlay2d = GL.GenTexture();

            GL.BindTexture(TextureTarget.Texture2D, textureOverlay2d);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Linear);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, overlay2d.Width, overlay2d.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero); // just allocate memory, so we can update efficiently using TexSubImage2D

            //--

            textureTop = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, textureTop);
            GL.TexImage2D(TextureTarget.Texture2D, 0,
                PixelInternalFormat.Rgba,
                400, 240, 0, //W, H, Border
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                PixelType.UnsignedByte, IntPtr.Zero);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.PixelTexGenQRoundSgix);
            #endregion

            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.TextureCoordArray);
            GL.Enable(EnableCap.Texture2D);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            OTKInput.Initialize(this);
            //this.KeyPress += HandleKeyPress;
            this.KeyDown += HandleKeyDown;

            displayFPS = false;
            fps = new FPSCounter();

            //--

            vertBufferScreen = new Vector2[8] {
                new Vector2(0,0), new Vector2(1, 0),
                new Vector2(400,0), new Vector2(1, 1),
                new Vector2(400,240), new Vector2(0, 1),
                new Vector2(0,240), new Vector2(0, 0)
            };
            VBOScreen = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBOScreen);
            GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, (IntPtr)(Vector2.SizeInBytes * vertBufferScreen.Length), vertBufferScreen, BufferUsageHint.StaticDraw);

            vertBufferFps = new Vector2[8] {
                new Vector2(0,100), new Vector2(0, 1),
                new Vector2(200,100), new Vector2(1, 1),
                new Vector2(200,0), new Vector2(1, 0),
                new Vector2(0,0), new Vector2(0, 0)
            };
            VBOFps = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBOFps);
            GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, (IntPtr)(Vector2.SizeInBytes * vertBufferFps.Length), vertBufferFps, BufferUsageHint.StaticDraw);
        }

        public void DisplayFPS(bool isEnabled) {
            displayFPS = isEnabled;
        }

        public void LoadTexture(int width, int height, byte[] data) {
            qtTop.Load(width, height, data);

            using (Graphics gfx = Graphics.FromImage(overlay2d)) {
                gfx.Clear(Color.Transparent);
                gfx.DrawString("FPS: " + fps.GetFPS().ToString(), arial, Brushes.Lime, new PointF(0, 0)); // Draw as many strings as you need

                BitmapData data2 = overlay2d.LockBits(new Rectangle(0, 0, overlay2d.Width, overlay2d.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                if (bData == null)
                    bData = new byte[Math.Abs(data2.Stride * data2.Height)];
                Marshal.Copy(data2.Scan0, bData, 0, bData.Length);

                qtOverlay2d.Load(overlay2d.Width, overlay2d.Height, bData);
                overlay2d.UnlockBits(data2);
            }
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e) {
            base.OnUpdateFrame(e);

            OTKInput.Update();
        }

        protected override void OnRenderFrame(FrameEventArgs e) {
            base.OnRenderFrame(e);

            if (qtTop != null) {
                //GL.BindTexture(TextureTarget.Texture2D, 0);
                //GL.DeleteTexture(textureTop);

                GL.BindTexture(TextureTarget.Texture2D, textureTop);
                //textureTop = TexUtil.CreateTextureFromBitmap(qtTop.decom);

                GL.TexImage2D(
                    TextureTarget.Texture2D,
                    0, //Level
                    PixelInternalFormat.Rgba,
                    qtTop.height,
                    qtTop.width,
                    0, //Border
                    OpenTK.Graphics.OpenGL.PixelFormat.Rgb,
                    PixelType.UnsignedByte,
                    qtTop.data); //bmpData.Scan0

                GL.BindTexture(TextureTarget.Texture2D, textureOverlay2d);
                GL.TexImage2D(
                    TextureTarget.Texture2D,
                    0, //Level
                    PixelInternalFormat.Rgba,
                    qtOverlay2d.width,
                    qtOverlay2d.height,
                    0, //Border
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                    PixelType.UnsignedByte,
                    qtOverlay2d.data);
            }

            GL.ClearColor(Color.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, virtualWidth, virtualHeight, 0, -1, 1); // Should be 2D

            GL.BindTexture(TextureTarget.Texture2D, textureTop);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBOScreen);
            GL.VertexPointer(2, VertexPointerType.Float, Vector2.SizeInBytes * 2, 0);
            GL.TexCoordPointer(2, TexCoordPointerType.Float, Vector2.SizeInBytes * 2, Vector2.SizeInBytes);
            GL.DrawArrays(PrimitiveType.Quads, 0, vertBufferScreen.Length / 2);

            //--

            if (displayFPS) {
                GL.BindTexture(TextureTarget.Texture2D, textureOverlay2d);
                GL.BindBuffer(BufferTarget.ArrayBuffer, VBOFps);
                GL.VertexPointer(2, VertexPointerType.Float, Vector2.SizeInBytes * 2, 0);
                GL.TexCoordPointer(2, TexCoordPointerType.Float, Vector2.SizeInBytes * 2, Vector2.SizeInBytes);
                GL.DrawArrays(PrimitiveType.Quads, 0, vertBufferFps.Length / 2);
            }

            this.SwapBuffers();
        }

        protected override void OnResize(EventArgs e) {
            base.OnResize(e);

            screenWidth = ClientSize.Width;
            screenHeight = ClientSize.Height;

            targetAspectRatio = (float)virtualWidth / (float)virtualHeight;

            int width = screenWidth;
            int height = (int)((float)width / targetAspectRatio + 0.5f);

            if (height > screenHeight) {
                //Console.WriteLine("PILLARBOX BITCHES");
                //It doesn't fit our height, we must switch to pillarbox then
                height = screenHeight;
                width = (int)((float)height * targetAspectRatio + 0.5f);
            }

            // set up the new viewport centered in the backbuffer
            int vpX = (screenWidth / 2) - (width / 2);
            int vpY = (screenHeight / 2) - (height / 2);

            GL.Viewport(vpX, vpY, width, height);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, virtualWidth, virtualHeight, 0, -1, 1); // Should be 2D

            //GL.MatrixMode(MatrixMode.Modelview);
            //GL.PushMatrix();

            //Now to calculate the scale considering the screen size and virtual size
            float scaleX = (float)screenWidth / (float)virtualWidth;
            float scaleY = (float)screenHeight / (float)virtualHeight;
            GL.Scale(scaleX, scaleY, 1.0f);

            GL.LoadIdentity();
            // From now on, instead of using -1 < 0 < 1 co-ordinates, use pixel ones starting from 0,0 top left

            GL.Disable(EnableCap.DepthTest);
        }

        void HandleKeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e) {
            if (e.Key == OpenTK.Input.Key.F4) {
                this.Exit();
                Environment.Exit(0);
            }
        }

        protected override void Dispose(bool manual) {
            overlay2d.Dispose();

            base.Dispose(manual);
        }
    }
}
