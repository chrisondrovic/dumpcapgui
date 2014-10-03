using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace Dumpcap
{
    /// <summary>
    /// 
    /// </summary>
    public partial class GUI : Form
    {
        /// <summary>
        /// The proc to run the task
        /// </summary>
        private Process proc;

        /// <summary>
        /// Initializes a new instance of the <see cref="GUI"/> class.
        /// </summary>
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

        /// <summary>
        /// Starts the dumpcap process
        /// </summary>
        public void DumpCap()
        {
            proc = new Process();
            proc.StartInfo.FileName = ConfigurationManager.AppSettings["dumpcapLocation"];
            //-w E:\captures\<CHANGENUMBER>-<SERVERNAME>.pcap -i <INTERFACENUMBER> -b filesize:51200 -b files:100
            proc.StartInfo.Arguments = "-w " + tbCapLocation.Text + @"\" + tbCapName.Text + ".pcap -i " + cbInterfaces.SelectedIndex + " -b filesize:" + tbMaxFileSize.Text + " -b files:" + tbMaxFiles.Text;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardInput = true;
            proc.Start();
        }

        /// <summary>
        /// Scheduleds the task.
        /// </summary>
        public void ScheduledTask()
        {
            StreamWriter scheduledtask = new StreamWriter(Environment.GetEnvironmentVariable("USERPROFILE") + "\\Desktop\\" + tbCapName.Text + ".bat");
            scheduledtask.WriteLine('"' + ConfigurationManager.AppSettings["dumpcapLocation"] + '"' + " -w " + tbCapLocation.Text + @"\" + tbCapName.Text + ".pcap -i " + cbInterfaces.SelectedIndex + " -b filesize:" + tbMaxFileSize.Text + " -b files:" + tbMaxFiles.Text);
            scheduledtask.Close();
        }

        /// <summary>
        /// Handles the CheckedChanged event of the rbNow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void rbNow_CheckedChanged(object sender, EventArgs e)
        {
            btnSubmit.Text = "Capture Logs";
        }

        /// <summary>
        /// Handles the CheckedChanged event of the rbLongTerm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void rbLongTerm_CheckedChanged(object sender, EventArgs e)
        {
            btnSubmit.Text = "Create Batch File";
        }

        /// <summary>
        /// Handles the Click event of the btnSubmit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            
            if (cbInterfaces.SelectedItem == null)
            {
                MessageBox.Show("Please select a network interface for the drop down list", "Required Item", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (!String.IsNullOrEmpty(tbMaxFileSize.Text))
                {
                    if (!String.IsNullOrEmpty(tbMaxFiles.Text))
                    {
                        if (!String.IsNullOrEmpty(tbCapName.Text))
                        {
                            if (!String.IsNullOrEmpty(tbCapLocation.Text))
                            {
                                if (rbLongTerm.Checked.Equals(true))
                                {
                                    ScheduledTask();
                                }

                                if (rbNow.Checked.Equals(true))
                                {
                                    DumpCap();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Please select a path to save your captures", "Required Item", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please supply a name for your capture files", "Required Item", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please enter the number of rollover files", "Required Item", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Please enter the max file size", "Required Item", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            
        }

        /// <summary>
        /// Handles the Load event of the GUI control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void GUI_Load(object sender, EventArgs e)
        {
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                cbInterfaces.Items.Add(nic.Name);
            }
        }

        /// <summary>
        /// Handles the Click event of the tbCapLocation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void tbCapLocation_Click(object sender, EventArgs e)
        {
            fbdCapLocation.RootFolder = Environment.SpecialFolder.MyComputer;
            DialogResult result = fbdCapLocation.ShowDialog();
            if (result == DialogResult.OK)
            {
                tbCapLocation.Text = fbdCapLocation.SelectedPath;
            }
        }  
    }
}
