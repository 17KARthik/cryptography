using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Client
{
    public partial class ClientLogin : Form
    {
        static DBConnection dbn = new DBConnection();
        string conStr = dbn.conStr;
        
        public ClientLogin()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string un = textBox1.Text;
            string pw = textBox2.Text;

            try
            {
                SqlConnection con1 = new SqlConnection(conStr);
                con1.Open();

                SqlCommand cmd = new SqlCommand("select * from ClientRegister", con1);
                SqlDataReader reader1 = cmd.ExecuteReader();
                int count = 0, foruid = 0;
                while (reader1.Read())
                {
                    foruid++;
                    String un1 = reader1.GetString(0);
                    String pw1 = reader1.GetString(1);
                    String mno1 = reader1.GetString(2);
                    String adrs1 = reader1.GetString(3);
                    if ((un.Equals(un1)) && (pw.Equals(pw1)))
                    {
                        count = 1;
                        MessageBox.Show("Login Successfully!", "Info", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

                        ClientForm muf = new ClientForm(un);
                        muf.Show();
                    }
                }
                con1.Close();
                if (count != 1)
                {
                    MessageBox.Show("UserId and Password is invalid!", "Info", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                }
            }
            catch (Exception e2)
            {
                Console.WriteLine(e2.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClientRegister cr = new ClientRegister();
            cr.Show();
        }
    }
}
