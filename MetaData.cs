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
    public partial class MetaData : Form
    {
        public MetaData()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ViewTableMetadata vf = new ViewTableMetadata();
            vf.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ViewDatabaseMetadata vd = new ViewDatabaseMetadata();
            vd.Show();
        }
    }
}
