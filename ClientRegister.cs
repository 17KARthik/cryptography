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
    public partial class ClientRegister : Form
    {
        static DBConnection dbn = new DBConnection();
        string conStr = dbn.conStr;
        
        public ClientRegister()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string un = textBox1.Text;
            string pw = textBox2.Text;
            string mno = textBox3.Text;
            string adrs = textBox4.Text;

            try
            {
                SqlConnection con1 = new SqlConnection(conStr);
                con1.Open();

                SqlCommand cmd = new SqlCommand("select * from ClientRegister", con1);
                SqlDataReader reader1 = cmd.ExecuteReader();
                int count = 0;
                while (reader1.Read())
                {
                    String un1 = reader1.GetString(0);
                    String pw1 = reader1.GetString(1);
                    String mno1 = reader1.GetString(2);
                    String adrs1 = reader1.GetString(3);
                    if ((un.Equals(un1)) && (pw.Equals(pw1)) && (mno.Equals(mno1)) && (adrs.Equals(adrs1)))
                    {
                        count = 1;
                    }
                }
                con1.Close();

                if (count != 1)
                {
                    SqlConnection con2 = new SqlConnection(conStr);
                    con2.Open();

                    String qry = "insert into ClientRegister values('" + un + "','" + pw + "','" + mno + "','" + adrs + "')";
                    Console.WriteLine(qry);
                    SqlCommand ins1 = con2.CreateCommand();
                    ins1.CommandText = qry;
                    ins1.ExecuteNonQuery();

                    con2.Close();
                    MessageBox.Show("Registered Successfully!", "Info", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Username and Password is already existed!", "Info", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                }
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.ToString());
            }
        }
    }
}
