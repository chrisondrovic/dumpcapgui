using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace Wireshark
{
    public partial class GUI : Form
    {
        private Process proc;
        private string procOutput = string.Empty;
        public GUI()
        {
            InitializeComponent();
        }

        /** Example Dumpcap
         * 
         * 
         * c:\"Program Files"\Wireshark\dumpcap.exe -I 1 -s 1518 -w c:\traces\SR12345678.cap -b filesize:16384 -b files:256 -f "host 192.168.1.1"
         * 
         * 
        **/

        public void DumpCap()
        {
            proc = new Process();
            proc.StartInfo.FileName = ConfigurationManager.AppSettings["dumpcapLocation"];
            //-w E:\captures\<CHANGENUMBER>-<SERVERNAME>.pcap -i <INTERFACENUMBER> -b filesize:51200 -b files:100
            proc.StartInfo.Arguments = "-w " + tbCapLocation.Text + @"\" + ConfigurationManager.AppSettings["capturefileName"]  + ".pcap -i " + cbInterfaces.SelectedIndex + " -b filesize:" + tbMaxFileSize.Text + " -b files:100";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardInput = true;
            proc.Start();
        }

        public void ScheduledTask()
        {

        }

        private void rbNow_CheckedChanged(object sender, EventArgs e)
        {
            btnSubmit.Text = "Capture Logs";
        }

        private void rbLongTerm_CheckedChanged(object sender, EventArgs e)
        {
            btnSubmit.Text = "Create Batch File";
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            
            if (rbNow.Checked.Equals(true))
            {
                DumpCap();
            }
            if (rbLongTerm.Checked.Equals(true))
            {
                ScheduledTask();
            }
        }

        private void GUI_Load(object sender, EventArgs e)
        {
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                cbInterfaces.Items.Add(nic.Name);
            }
        }

        private void tbCapLocation_Click(object sender, EventArgs e)
        {
            fbdCapLocation.RootFolder = Environment.SpecialFolder.MyComputer;
            DialogResult result = fbdCapLocation.ShowDialog();
            if (result == DialogResult.OK)
            {
                tbCapLocation.Text = fbdCapLocation.SelectedPath + @"\";
            }
        }  
    }
}
