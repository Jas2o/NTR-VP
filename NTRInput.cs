using BrandonPotter.XBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NTR_ViewerPlus {
    public static class NTRInput {

        private static Thread threadXbox;
        private static bool Running = false;

        private static bool _luma = false;
        private static double s = Math.Sin(-45);
        private static double c = Math.Cos(-45);

        private static XBoxController controller;

        private static uint oldbuttons, oldtouch, oldcircle, oldcstick;//, oldspecial;
        private static byte oldz;

        private static Dictionary<int, byte> DPadTranslate = new Dictionary<int, byte>() {
            { 0x00, 0xF }, //Idle
            { 0x04, 0xB }, //Up
            { 0x08, 0xA }, //UR
            { 0x0C, 0xE }, //Right
            { 0x10, 0x6 }, //DR
            { 0x14, 0x7 }, //Down
            { 0x18, 0x5 }, //DL
            { 0x1C, 0xD }, //Left
            { 0x20, 0x9 }  //UL
        };

        public static void Open(bool luma = false) {
            _luma = luma;

            oldbuttons = 0xff0f0000;
            oldtouch = 0x00000002;
            oldcircle = 0x00088000;

            oldcstick = 0x81008080;
            //oldspecial = 0x00000000;

            XBoxControllerWatcher watcher = new BrandonPotter.XBox.XBoxControllerWatcher();
            watcher.ControllerConnected += (c) => { controller = c; };
            watcher.ControllerDisconnected += (c) => { c = null; };

            threadXbox = new Thread(() => {
                Running = true;
                while (Running) {
                    CheckState();
                }
            });
            threadXbox.Start();
        }

        public static void Close() {
            Running = false;
            if (threadXbox != null) {
                threadXbox.Abort();
                threadXbox.Join();
            }
        }

        private static void CheckState() {
            if (controller != null && Network.tcpClientStream != null && Network.tcpClientStream.CanWrite) {
                byte b0 = (byte)(
                    ((controller.ButtonDownPressed ? 0x0 : 0x1) << 7) //Down
                    + ((controller.ButtonUpPressed ? 0x0 : 0x1) << 6) //Up
                    + ((controller.ButtonLeftPressed ? 0x0 : 0x1) << 5) //Left
                    + ((controller.ButtonRightPressed ? 0x0 : 0x1) << 4) //Right
                    + ((controller.ButtonStartPressed ? 0x0 : 0x1) << 3) //Start
                    + ((controller.ButtonBackPressed ? 0x0 : 0x1) << 2) //Select
                    + ((controller.ButtonAPressed ? 0x0 : 0x1) << 1) //B
                    + ((controller.ButtonBPressed ? 0x0 : 0x1)) //A
                );

                byte b1 = (byte)(
                    ((controller.ButtonXPressed ? 0x0 : 0x1) << 3) //Y
                    + ((controller.ButtonYPressed ? 0x0 : 0x1) << 2) //X
                    + ((controller.ButtonShoulderLeftPressed ? 0x0 : 0x1) << 1) //L
                    + ((controller.ButtonShoulderRightPressed ? 0x0 : 0x1)) //R
                );

                uint buttons = (uint)((b0 << 8) + b1);

                uint leftX = (uint)(controller.ThumbLeftX * 40d);
                uint leftY = (uint)((100 - controller.ThumbLeftY) * 40d);
                if (leftX > 0x600 && leftX < 0xA00) leftX = 0x7FF;
                if (leftY > 0x600 && leftY < 0xA00) leftY = 0x7FF;
                uint circle = ((0xFFF - leftY) << 12) + leftX;
                byte[] bCircle = BitConverter.GetBytes(circle);
                bCircle[3] = 0x01;

                uint rightX = (uint)(controller.ThumbRightX * 2d);
                uint rightY = (uint)((controller.ThumbRightY * 2d)); //Needs to be flipped //0xFF - 
                if (rightX > 0x60 && rightX < 0xA0) rightX = 0x7F;
                if (rightY > 0x60 && rightY < 0xA0) rightY = 0x7F;

                // The C-Stick needs to be rotated 45 degrees...
                // https://stackoverflow.com/questions/2259476/rotating-a-point-about-another-point-2d
                double pX = rightX - 127; // translate point back to origin:
                double pY = rightY - 127; // (rotate point)
                byte rotatedCX = (byte)((pX * c - pY * s) + 127); // translate point back:
                byte rotatedCY = (byte)((pX * s + pY * c) + 127);
                uint cstick = (uint)(rotatedCX << 8) + (rotatedCY);
                //Left currently doesn't work at all

                byte z = (byte)( ((controller.TriggerRightPressed ? 0x0 : 0x1) << 2) //ZR
                    + ((controller.TriggerLeftPressed ? 0x0 : 0x1) << 1) ); //ZL

                uint touch = 0x00000002;

                if (oldbuttons != buttons || /*oldtouch != touch ||*/ oldcircle != circle || oldz != z || oldcstick != cstick) {
                    //Don't compare touch, it's not managed by this loop

                    if (_luma) {
                        byte[] inputredirection = new byte[] { b0, b1, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x02,
                            0x00, 0x08, 0x80, 0x00,
                            0x81, 0x00, 0x80, 0x80, //CStick
                            0x00, 0x00, 0x00, 0x00 //Special
                        };
                        bCircle.CopyTo(inputredirection, 8);

                        inputredirection[13] = z;
                        inputredirection[14] = (byte)rotatedCX;
                        inputredirection[15] = (byte)rotatedCY;

                        NetworkLuma.Write(inputredirection);

                    } else {
                        //Console.WriteLine(b0.ToString("X2") + " " + b1.ToString("X2"));

                        byte[] inputredirection = new byte[] { b0, b1, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x02,
                            0x00, 0x08, 0x80, 0x00 };
                        bCircle.CopyTo(inputredirection, 8);

                        NTRPacket ntrIR = NTRPacket.NewWriteMemory(1000, 16, 1105696, 12, 12);
                        Network.tcpClientStream.Write(ntrIR.GetBytes(), 0, 84);
                        Network.tcpClientStream.Write(inputredirection, 0, inputredirection.Length);
                    }

                    oldbuttons = buttons;
                    oldtouch = touch;
                    oldcircle = circle;
                    oldcstick = cstick;
                    oldz = z;
                } else
                    Console.WriteLine(".");

                //Console.WriteLine(leftX.ToString("X") + " " + leftY.ToString("X"));

                Thread.Sleep(1);
            }
        }

        public static void TouchscreenClick(int X, int Y) {
            X = (int)Math.Round(((double)X / 319) * 4095);
            Y = (int)Math.Round(((double)Y / 239) * 4095);
            uint touch = (uint)(X | (Y << 12) | (0x01 << 24));

            //Don't compare to oldtouch as you might be clicking the same pixel again.
            byte[] bValue = BitConverter.GetBytes(touch);

            if (_luma) {
                byte[] inputredirection = new byte[] { 0xFF, 0x0F, 0x00, 0x00, /**/ 0x00, 0x00, 0x00, 0x02, /**/ 0x00, 0x08, 0x80, 0x00,
                        0x81, 0x00, 0x80, 0x80, //CStick
                        0x00, 0x00, 0x00, 0x00 //Special
                    };
                bValue.CopyTo(inputredirection, 4);

                NetworkLuma.Write(inputredirection);
            } else {
                if (Network.tcpClientStream != null && Network.tcpClientStream.CanWrite) {
                    byte[] inputredirection = new byte[] { 0xFF, 0x0F, 0x00, 0x00, /**/ 0x00, 0x00, 0x00, 0x02, /**/ 0x00, 0x08, 0x80, 0x00 };
                    bValue.CopyTo(inputredirection, 4);
                    NTRPacket ntrIR = NTRPacket.NewWriteMemory(1000, 16, 1105696, 12, 12);
                    Network.tcpClientStream.Write(ntrIR.GetBytes(), 0, 84);
                    Network.tcpClientStream.Write(inputredirection, 0, inputredirection.Length);
                }
            }
            oldtouch = touch;
        }

        public static void TouchscreenRelease() {
            if (_luma) {
                byte[] inputredirection = new byte[] { 0xFF, 0x0F, 0x00, 0x00, /**/ 0x00, 0x00, 0x00, 0x02, /**/ 0x00, 0x08, 0x80, 0x00,
                            0x81, 0x00, 0x80, 0x80, //CStick
                            0x00, 0x00, 0x00, 0x00 //Special
                        };

                NetworkLuma.Write(inputredirection);
            } else {
                if (Network.tcpClientStream != null && Network.tcpClientStream.CanWrite) {
                    byte[] inputredirection = new byte[] { 0xFF, 0x0F, 0x00, 0x00, /**/ 0x00, 0x00, 0x00, 0x02, /**/ 0x00, 0x08, 0x80, 0x00 };
                    //bValue.CopyTo(inputredirection, 4);
                    NTRPacket ntrIR = NTRPacket.NewWriteMemory(1000, 16, 1105696, 12, 12);
                    Network.tcpClientStream.Write(ntrIR.GetBytes(), 0, 84);
                    Network.tcpClientStream.Write(inputredirection, 0, inputredirection.Length);

                    oldtouch = 0x00000002;
                }
            }
        }

        public static void SendHome() {
            if (_luma) {
                byte[] inputredirection = new byte[] { 0xFF, 0x0F, 0x00, 0x00, /**/ 0x00, 0x00, 0x00, 0x02, /**/ 0x00, 0x08, 0x80, 0x00,
                            0x81, 0x00, 0x80, 0x80, //CStick
                            0x00, 0x00, 0x00, 0x00 //Special
                        };

                inputredirection[16] = 0x01; //1=Home, 2=Power, 4=Long Power
                NetworkLuma.Write(inputredirection);

                inputredirection[16] = 0x00; //No special
                NetworkLuma.Write(inputredirection);
            }
        }
    }
}
