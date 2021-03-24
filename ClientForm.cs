using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Client
{
    public partial class ClientForm : Form
    {
        string cn;

        public ClientForm(string clientName)
        {
            InitializeComponent();
            cn = clientName;
            showLabel();
        }

        public void showLabel()
        {
            label2.Text = "Welcome " + cn;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UploadForm uf = new UploadForm(cn);
            uf.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DownloadForm df = new DownloadForm();
            df.Show();
        }
    }
}
