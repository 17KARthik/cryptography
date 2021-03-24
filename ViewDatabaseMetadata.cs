using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;

namespace CloudServer
{
    public partial class ViewDatabaseMetadata : Form
    {
        public DataTable table2 = new DataTable();
        static DBConnection dbn = new DBConnection();
        string conStr = dbn.conStr;
        
        public ViewDatabaseMetadata()
        {
            InitializeComponent();
            table2.Columns.Add("Data Owner Name");
            table2.Columns.Add("File Name");
            table2.Columns.Add("Field Confidentiality");
            table2.Columns.Add("Public Keys");
            table2.Columns.Add("Secret Keys");

            Thread td = new Thread(showTable);
            td.Start();
        }

        public void showTable()
        {
            try
            {
                SqlConnection con1 = new SqlConnection(conStr);
                con1.Open();

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

                    string[] row = { cn1, fn1, fc1, ek1, dk1 };
                    table2.Rows.Add(row);
                    dataGridView1.Invoke((Action)(() => dataGridView1.DataSource = table2));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
