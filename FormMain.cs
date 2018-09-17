using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace NTR_ViewerPlus {
    public partial class FormMain : Form {

        //NTR Viewer Plus
        private static Thread threadOpenTK1, threadOpenTK2;
        public static ViewerTop viewerTop;
        public static ViewerBottom viewerBottom;

        public FormMain() {
            InitializeComponent();
        }

        private void BtnRun_Click(object sender, EventArgs e) {
            if(chkLumaInputRedirection.Checked) {
                NetworkLuma.Start(txtIP.Text);
                Console.WriteLine("Luma UDP started.");
            }

            int priority = (chkOnly.Checked ? 0 : (int)numericPriority.Value);

            Network.Start(txtIP.Text, priority, (chkPriorityTop.Checked ? 1 : 0), (int)numQuality.Value, (int)numQoS.Value);
            if (Network.tcpClientStream != null) {
                NTRInput.Open(chkLumaInputRedirection.Checked);

                if (!chkOnly.Checked || (chkOnly.Checked && chkPriorityTop.Checked)) {
                    threadOpenTK1 = new Thread(() => {
                        viewerTop = new ViewerTop(470, 280);
                        viewerTop.DisplayFPS(chkDisplayFPS.Checked);
                        viewerTop.Run();
                    });
                    threadOpenTK1.Start();
                }

                if (!chkOnly.Checked || (chkOnly.Checked && !chkPriorityTop.Checked)) {
                    threadOpenTK2 = new Thread(() => {
                        viewerBottom = new ViewerBottom(470, 280);
                        viewerBottom.DisplayFPS(chkDisplayFPS.Checked);
                        viewerBottom.Run();
                    });
                    threadOpenTK2.Start();
                }

                while (viewerTop == null && (!chkOnly.Checked || (chkOnly.Checked && chkPriorityTop.Checked)))
                    Thread.Yield();

                while (viewerTop == null && (!chkOnly.Checked || (chkOnly.Checked && !chkPriorityTop.Checked)))
                    Thread.Yield();
            }
        }

        private void BtnPatchNFCSM_Click(object sender, EventArgs e) {
            Network.DoCONNandHB();

            //Sun/Moon
            //int addr1 = 0x3DFFD0; //v1.0
            int addr2 = 0x3E14C0; //v1.1, seems to work for v1.2

            if (Network.dictionaryProcess.ContainsKey("niji_loc")) {
                int pid = Network.dictionaryProcess["niji_loc"];// 0x2b;
                Console.WriteLine(pid);
                uint data = 0xE3A01000;
                byte[] bdata = BitConverter.GetBytes(data);

                //NTRPacket ntr1 = NTRPacket.NewWriteMemory(1000, pid, addr1, bdata.Length, bdata.Length);
                NTRPacket ntr2 = NTRPacket.NewWriteMemory(1000, pid, addr2, bdata.Length, bdata.Length);
                //tcpClientStream.Write(ntr1.GetBytes(), 0, 84);
                //tcpClientStream.Write(bdata, 0, bdata.Length);
                Network.tcpClientStream.Write(ntr2.GetBytes(), 0, 84);
                Network.tcpClientStream.Write(bdata, 0, bdata.Length);

                btnPatchNFCSM.Text = "SM Patched";
            }
        }

        private void BtnPatchNFCOther_Click(object sender, EventArgs e) {
            DialogResult dialogResult = MessageBox.Show("Break Input Redirection and reconnecting?", "NTR-VP NFC Patch", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (dialogResult == DialogResult.Yes) {
                int addr1 = 0x00105B00;

                int pid = 0x1a;
                byte[] buffer = new byte[] { 0x70, 0x47 };

                NTRPacket ntr1 = NTRPacket.NewWriteMemory(1000, pid, addr1, buffer.Length, buffer.Length);
                Network.tcpClientStream.Write(ntr1.GetBytes(), 0, 84);
                Network.tcpClientStream.Write(buffer, 0, buffer.Length);
            }
        }

        private void chkOnly_CheckedChanged(object sender, EventArgs e) {
            numericPriority.Enabled = !chkOnly.Checked;
        }

        private void btnSendHome_Click(object sender, EventArgs e) {
            NTRInput.SendHome();
        }

        private void chkDisplayFPS_CheckedChanged(object sender, EventArgs e) {
            if(viewerTop != null)
                viewerTop.DisplayFPS(chkDisplayFPS.Checked);
            if(viewerBottom != null)
                viewerBottom.DisplayFPS(chkDisplayFPS.Checked);
        }

        private void chkDespeckle_CheckedChanged(object sender, EventArgs e) {
            
        }

        private void FormMain_Load(object sender, EventArgs e) {
            cmbFilter.SelectedIndex = 1;
            if (chkAutoScan.Checked)
                btnScan_Click(sender, e);
        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e) {
            Filter.UseFilter = cmbFilter.SelectedIndex;
        }

        private void btnScan_Click(object sender, EventArgs e) {
            List<NetworkScan.MacIpPair> list = NetworkScan.GetAllMacAddressesAndIppairs();
            foreach (NetworkScan.MacIpPair pair in list) {
                if(pair.MacAddress == txtMAC.Text) {
                    txtIP.Text = pair.IpAddress;
                    break;
                }
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e) {
            if (threadOpenTK1 != null) {
                threadOpenTK1.Abort();
                threadOpenTK1.Join();
            }

            if (threadOpenTK2 != null) {
                threadOpenTK2.Abort();
                threadOpenTK2.Join();
            }

            NTRInput.Close();

            Environment.Exit(0);
        }

    }
}
