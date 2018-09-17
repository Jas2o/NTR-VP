using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Drawing;
using System.Runtime.InteropServices;
using turbojpegCLI;

namespace NTR_ViewerPlus {
    public static class Network {

        public static NetworkStream tcpClientStream;
        private static UdpClient udpClient;
        private static EndPoint udpEP;

        public static Dictionary<string, int> dictionaryProcess;

        public static void Start(string ip, int priority, int priorityScreen, int quality, int qos) {

            if (tcpClientStream != null) {
                tcpClientStream.Close();
                udpClient.Close();
            }

            TcpClient tcpClient = new TcpClient();

            if (tcpClient.ConnectAsync(ip, 8000).Wait(1000)) {
                tcpClientStream = tcpClient.GetStream();

                udpClient = new UdpClient();
                udpClient.DontFragment = true;
                udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, 8001));

                udpEP = new IPEndPoint(IPAddress.Any, 8001);

                NTRPacket ntrRP = NTRPacket.NewRemotePlay(1000, (byte)priority, (byte)priorityScreen, quality, qos);
                tcpClientStream.Write(ntrRP.GetBytes(), 0, 84);

                DoCONNandHB();

                Thread threadNetwork = new Thread(() => {
                    while (true) {
                        DoThing();
                    }
                });
                threadNetwork.Start();
            }
        }

        private static byte[] data = new byte[10000];
        private static byte[] jpegImage = new byte[100000];
        private static TJDecompressor tjd;

        public static void DoThing() {
            try {
                //Console.WriteLine("DoThing");

                int pos = 0;

                bool isTop = false;
                bool iscomplete = false;
                int lastSeq = 0;
                int lastSub = 0;

                bool loop = true;
                while (loop) {
                    int length = udpClient.Client.ReceiveFrom(data, ref udpEP);

                    if (data[3] == 0x00) {
                        lastSeq = data[0];
                        lastSub = -1;
                        pos = 0;
                    }

                    if (data[0] == lastSeq && lastSub + 1 == data[3]) {
                        lastSub = data[3];

                        if (data[1] == 0x01 || data[1] == 0x11)
                            isTop = true;
                        else if(isTop)
                            isTop = false; //LIES!

                        //Need to valid that the entire UDP stream is received before proceeding... Otherwise TDJ crashes.
                        //Console.WriteLine("{0:x2} {1:x2} {2:x2} {3:x2}", data[0], data[1], data[2], data[3]);

                        if (pos + length > jpegImage.Length) {
                            loop = false;
                            pos = 0;
                        } else {
                            Array.Copy(data, 4, jpegImage, pos, length - 4);
                            pos += length - 4;

                            if (data[1] >= 0x10) {
                                iscomplete = true;
                                loop = false;
                                //Console.WriteLine("S");
                            }
                        }
                    } else {
                        loop = false;
                        pos = 0;
                        //Console.WriteLine("FB");
                    }
                }

                if (iscomplete) {
                    byte[] decom;
                    tjd = new TJDecompressor(jpegImage);

                    if (isTop) {
                        decom = new byte[240 * 400 * TJ.getPixelSize(turbojpegCLI.PixelFormat.RGB)];
                        tjd.decompress(decom, turbojpegCLI.PixelFormat.RGB, Flag.NONE);

                        if (FormMain.viewerTop != null)
                            FormMain.viewerTop.LoadTexture(400, 240, decom);
                    } else {
                        decom = new byte[240 * 320 * TJ.getPixelSize(turbojpegCLI.PixelFormat.RGB)];
                        tjd.decompress(decom, turbojpegCLI.PixelFormat.RGB, Flag.NONE);

                        if (FormMain.viewerTop != null)
                            FormMain.viewerBottom.LoadTexture(320, 240, decom);
                    }

                    tjd.Dispose();
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.ToString());
            }
        }

        public static void DoCONNandHB() {
            NTRPacket ntrCON = NTRPacket.NewConnect(2000);
            NTRPacket ntrHB = NTRPacket.NewHeartbeat(3000);

            tcpClientStream.Write(ntrCON.GetBytes(), 0, 84);
            tcpClientStream.Write(ntrHB.GetBytes(), 0, 84);

            byte[] buffer = new byte[84];
            tcpClientStream.Read(buffer, 0, 84);

            NTRPacket ntrRec = new NTRPacket(buffer);

            string test = "";

            byte[] buffer2 = new byte[ntrRec.DataLen];
            tcpClientStream.Read(buffer2, 0, ntrRec.DataLen);
            for (int i = buffer2.Length - 1; i >= 0; i--) {
                if (buffer2[i] != 0x00) {
                    test = test + Encoding.ASCII.GetString(buffer2, 0, i + 1);
                    break;
                }
            }

            if (test.Length < ntrRec.DataLen) {
                byte[] buffer3 = new byte[ntrRec.DataLen];
                tcpClientStream.Read(buffer3, 0, ntrRec.DataLen);
                for (int i = buffer3.Length - 1; i >= 0; i--) {
                    if (buffer3[i] != 0x00) {
                        test = test + Encoding.ASCII.GetString(buffer3, 0, i);
                        break;
                    }
                }
            }

            dictionaryProcess = new Dictionary<string, int>();
            string[] lines = test.Split('\n');
            //pid: 0x00000030, pname: niji_loc, tid: 0004000000164800, kpobj: fff7bdb0
            foreach (string line in lines) {
                if (line.StartsWith("pid: ")) {
                    int pos_pid = line.IndexOf("pid: "); //5
                    int pos_pname = line.IndexOf("pname: "); //7
                    int pos_tid = line.IndexOf(", tid: "); //7
                                                           //int pos_kpobj = line.IndexOf("kpobj: "); //7

                    string s_pid = line.Substring(pos_pid + 5, 10);
                    int pid = (int)new System.ComponentModel.Int32Converter().ConvertFromString(s_pid);
                    string pname = line.Substring(pos_pname + 7, pos_tid - 7 - pos_pname).Trim();
                    //string tid = line.Substring(pos_tid + 7, 16);

                    dictionaryProcess.Add(pname, pid);
                }
            }
        }
    }
}
