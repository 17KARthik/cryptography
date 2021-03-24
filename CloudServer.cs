using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CloudServer
{
    public partial class CloudServer : Form
    {
        public CloudServer()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ViewEncryptedData ve = new ViewEncryptedData();
            ve.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MetaData md = new MetaData();
            md.Show();
        }

        private void CloudServer_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
