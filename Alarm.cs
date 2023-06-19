using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;

namespace MemorIt_by_IMLL
{
    public partial class Alarm : Form
    {
        public Alarm()
        {
            InitializeComponent();
        }

        private void Alarm_Load(object sender, EventArgs e)
        {

            var processInfo = new ProcessStartInfo
            {
                FileName = Application.StartupPath + "\\Sonneries\\" + Form1.NameSonnerie,
                Arguments = "a",
                WindowStyle= ProcessWindowStyle.Minimized,
                CreateNoWindow = false,
                UseShellExecute = true,
                RedirectStandardError = false,
                RedirectStandardOutput = false
            };
            label1.Text = this.Text;
            richTextBox1.Text = Form1.DescriptionSonnerie;
            if(Form1.NameSonnerie != "None")
            {
                if (!File.Exists(Application.StartupPath + "\\Sonneries\\" + Form1.NameSonnerie))
                {
                    processInfo.FileName = Application.StartupPath + "\\Sonneries\\BasicSon.mp3";
                }
                Process.Start(processInfo);
            }
        }
    }
}
