using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Numerics;

namespace Client
{
    public partial class DownloadForm : Form
    {
        static DBConnection dbn = new DBConnection();
        string conStr = dbn.conStr; 
        
        public DownloadForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string fn=textBox1.Text;
            Test tt = new Test();
            string don = tt.getMsgResult("Enter the Data Owner Name: ");

            try
            {
                SqlConnection con1 = new SqlConnection(conStr);
                con1.Open();
                int count = 0,cou=0;
                SqlCommand cmd = new SqlCommand("select * from UploadDetails", con1);
                SqlDataReader reader1 = cmd.ExecuteReader();                
                while (reader1.Read())
                {
                    String cn1 = reader1.GetString(0);
                    String fd1 = reader1.GetString(1);
                    String fn1 = reader1.GetString(2);
                    String df1 = reader1.GetString(3);
                    String fs1 = reader1.GetString(4);
                    String ek1 = reader1.GetString(5);
                    String dk1 = reader1.GetString(6);
                    String dt1 = reader1.GetString(7);
                    String et1 = reader1.GetString(8);
                    String fc1 = reader1.GetString(9);
                    Console.WriteLine(cn1.Trim()+"<--->"+don.Trim());
                    if (cn1.Trim().Equals(don.Trim()))
                    {
                        count = 1;
                        Console.WriteLine("fc1 is "+fc1);
                        if (fc1.Equals("COL"))
                        {
                            if (dk1.Contains("@"))
                            {
                                string[] deckeys = dk1.Split('@');
                                string seckeysforfileName = deckeys[1];
                                string seckeysforfile = deckeys[2];

                                MessageBox.Show("Secret Key: " + seckeysforfile);
                                Console.WriteLine("Secret Key: " + seckeysforfile);

                                String[] s = seckeysforfileName.Split(',');
                                string d = s[0];
                                string n = s[1];

                                    //Decrypting, M' = C^d mod n
                                
                                /*First Decrypting File Name*/

                                String[] cip = fn1.Split(',');
                                string filename = "";
                                for (int i = 0; i < cip.Length; i++)
                                {
                                    string dec = BigInteger.ModPow(BigInteger.Parse(cip[i]), BigInteger.Parse(d), BigInteger.Parse(n)).ToString();
                                    Console.WriteLine("dec is "+dec);
                                    int ascii = Convert.ToInt32(dec);
                                    string ori = ((char)ascii).ToString();
                                    filename = filename + ori;
                                }
                                if (filename.Equals(fn))
                                {
                                    cou = 1;

                                    String[] sk = seckeysforfile.Split(',');
                                    string d2 = sk[0];
                                    string n2 = sk[1];
                                    /*Second Decrypting file*/

                                    String[] cip1 = df1.Split(',');
                                    string file = "";
                                    for (int i = 0; i < cip1.Length; i++)
                                    {
                                        string dec = BigInteger.ModPow(BigInteger.Parse(cip1[i]), BigInteger.Parse(d2), BigInteger.Parse(n2)).ToString();
                                        int ascii = Convert.ToInt32(dec);
                                        string ori = ((char)ascii).ToString();
                                        file = file + ori;
                                    }
                                    MessageBox.Show("Successfully Downloaded!");
                                    richTextBox1.Text = file;
                                }                                
                            }
                        }
                        if (fc1.Equals("MCOL"))
                        {
                            if (dk1.Contains("@"))
                            {
                                string[] deckeys = dk1.Split('@');
                                string seckeys = deckeys[0];
                                string seckeysforfile = deckeys[1];

                                MessageBox.Show("Secret Key: " + seckeys);

                                String[] s = seckeys.Split(',');
                                string d = s[0];
                                string n = s[1];

                                //Decrypting, M' = C^d mod n

                                /*First Decrypting File Name*/

                                String[] cip = fn1.Split(',');
                                string filename = "";
                                for (int i = 0; i < cip.Length; i++)
                                {
                                    string dec = BigInteger.ModPow(BigInteger.Parse(cip[i]), BigInteger.Parse(d), BigInteger.Parse(n)).ToString();
                                    int ascii = Convert.ToInt32(dec);
                                    string ori = ((char)ascii).ToString();
                                    filename = filename + ori;
                                }
                                if (filename.Equals(fn))
                                {
                                    cou = 1;
                                    /*Second Decrypting file*/
                                    String[] sk = seckeysforfile.Split(',');
                                    string d2 = sk[0];
                                    string n2 = sk[1];

                                    String[] cip1 = df1.Split(',');
                                    string file = "";
                                    for (int i = 0; i < cip1.Length; i++)
                                    {
                                        string dec = BigInteger.ModPow(BigInteger.Parse(cip1[i]), BigInteger.Parse(d2), BigInteger.Parse(n2)).ToString();
                                        int ascii = Convert.ToInt32(dec);
                                        string ori = ((char)ascii).ToString();
                                        file = file + ori;
                                    }
                                    MessageBox.Show("Successfully Downloaded!");
                                    richTextBox1.Text = file;
                                }
                            }
                        }
                        if (fc1.Equals("DBC"))
                        {
                            if (!(dk1.Contains("@")))
                            {                                
                                string seckeys = dk1;

                                MessageBox.Show("Secret Key: " + seckeys);

                                String[] s = seckeys.Split(',');
                                string d = s[0];
                                string n = s[1];

                                //Decrypting, M' = C^d mod n

                                /*First Decrypting File Name*/

                                String[] cip = fn1.Split(',');
                                string filename = "";
                                for (int i = 0; i < cip.Length; i++)
                                {
                                    string dec = BigInteger.ModPow(BigInteger.Parse(cip[i]), BigInteger.Parse(d), BigInteger.Parse(n)).ToString();
                                    int ascii = Convert.ToInt32(dec);
                                    string ori = ((char)ascii).ToString();
                                    filename = filename + ori;
                                }
                                if (filename.Equals(fn))
                                {
                                    cou = 1;
                                    /*Second Decrypting file*/

                                    String[] cip1 = df1.Split(',');
                                    string file = "";
                                    for (int i = 0; i < cip1.Length; i++)
                                    {
                                        string dec = BigInteger.ModPow(BigInteger.Parse(cip1[i]), BigInteger.Parse(d), BigInteger.Parse(n)).ToString();
                                        int ascii = Convert.ToInt32(dec);
                                        string ori = ((char)ascii).ToString();
                                        file = file + ori;
                                    }
                                    MessageBox.Show("Downloaded Successfully!");
                                    richTextBox1.Text = file;
                                }
                            }
                        }
                    }
                }
                if ((count != 1)||(cou!=1))
                {
                    MessageBox.Show("Both file Name & data Owner names are invalid!");
                }
            }
            catch (Exception e2)
            {
                Console.WriteLine(e2.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            richTextBox1.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
