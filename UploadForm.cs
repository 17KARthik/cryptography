using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using System.Numerics;
using System.Collections;
using System.Data.SqlClient;

namespace Client
{
    public partial class UploadForm : Form
    {
        static DBConnection dbn = new DBConnection();
        string conStr = dbn.conStr;        
        string pubKeys, secKeys, cn, fileSize;
        int noOfCol = 4;
        ArrayList forKeys = new ArrayList();
        
        public UploadForm(string clientName)
        {
            InitializeComponent();
            cn = clientName;
            showFileIdCount();
        }

        public void showFileIdCount()
        {
            try
            {
                SqlConnection con1 = new SqlConnection(conStr);
                con1.Open();

                SqlCommand cmd = new SqlCommand("select * from UploadDetails", con1);
                SqlDataReader reader1 = cmd.ExecuteReader();
                int count = 0;
                while (reader1.Read())
                {
                    String cn1 = reader1.GetString(0);
                    if (cn.Equals(cn1))
                    {
                        count++;
                    }
                }
                textBox2.Text = "" + (count + 1);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog opg = new OpenFileDialog();
            DialogResult result = opg.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                string file = opg.FileName;
                string fileName = Path.GetFileName(file);

                FileInfo f = new FileInfo(file);
                long s1 = f.Length;
                fileSize = "" + s1;                

                try
                {
                    string text = File.ReadAllText(file);
                    textBox1.Text = fileName;
                    richTextBox1.Text = text;
                }
                catch (Exception e1)
                {
                    Console.WriteLine(e1.Message);
                }
            }
            Console.WriteLine(result); 
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            richTextBox1.Text = "";
            comboBox1.SelectedIndex = 0;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            forKeys = new ArrayList();
            if (comboBox1.SelectedItem.ToString().Equals("COL"))
            {
                for (int i = 0; i < noOfCol; i++)
                {
                    string forp = String.Empty;
                    string txtMax = "10000000000";
                    BigInteger p = RandomIntegerBelow(BigInteger.Parse(txtMax));
                    while (!IsProbabilyPrime(p, 20))
                    {
                        p = RandomIntegerBelow(BigInteger.Parse(txtMax));
                    }
                    forp = p.ToString();

                    BigInteger q = RandomIntegerBelow(BigInteger.Parse(txtMax));
                    while (!IsProbabilyPrime(q, 20))
                    {
                        q = RandomIntegerBelow(BigInteger.Parse(txtMax));
                    }
                    string forq = q.ToString();

                    log("totient = (P-1)*(Q-1) = " + (BigInteger.Parse(forp) - 1) + " x " + (BigInteger.Parse(forq) - 1));
                    string forTOT = (BigInteger.Multiply(BigInteger.Parse(forp) - 1, BigInteger.Parse(forq) - 1)).ToString();

                    log("generating e randomely such that gcd(e,totient) = 1");
                    BigInteger temp = 0;
                    while (GCD_Euclidean(temp, BigInteger.Parse(forTOT)) != 1)
                    {
                        temp = RandomIntegerBelow(BigInteger.Parse(forTOT));
                        log("new E =  " + temp);
                    }
                    string fore = temp.ToString();

                    BigInteger[] result = new BigInteger[3];
                    result = Extended_GCD(BigInteger.Parse(forTOT), BigInteger.Parse(fore));
                    if (result[2] < 0)
                        result[2] = result[2] + BigInteger.Parse(forTOT);
                    string ford = result[2].ToString();

                    log("N = P*Q = " + forp + " x " + forq);
                    string forn = (BigInteger.Multiply(BigInteger.Parse(forp), BigInteger.Parse(forq))).ToString();

                    string pubkeys1 = fore + "," + forn;
                    string seckeys1 = ford + "," + forn;

                    forKeys.Add(pubkeys1+"@"+seckeys1);                    
                }
                MessageBox.Show("AES Keys are generated successfully!");
            }
            else if (comboBox1.SelectedItem.ToString().Equals("MCOL"))
            {
                for (int i = 0; i < noOfCol/2; i++)
                {
                    string forp = String.Empty;
                    string txtMax = "10000000000";
                    BigInteger p = RandomIntegerBelow(BigInteger.Parse(txtMax));
                    while (!IsProbabilyPrime(p, 20))
                    {
                        p = RandomIntegerBelow(BigInteger.Parse(txtMax));
                    }
                    forp = p.ToString();

                    BigInteger q = RandomIntegerBelow(BigInteger.Parse(txtMax));
                    while (!IsProbabilyPrime(q, 20))
                    {
                        q = RandomIntegerBelow(BigInteger.Parse(txtMax));
                    }
                    string forq = q.ToString();

                    log("totient = (P-1)*(Q-1) = " + (BigInteger.Parse(forp) - 1) + " x " + (BigInteger.Parse(forq) - 1));
                    string forTOT = (BigInteger.Multiply(BigInteger.Parse(forp) - 1, BigInteger.Parse(forq) - 1)).ToString();

                    log("generating e randomely such that gcd(e,totient) = 1");
                    BigInteger temp = 0;
                    while (GCD_Euclidean(temp, BigInteger.Parse(forTOT)) != 1)
                    {
                        temp = RandomIntegerBelow(BigInteger.Parse(forTOT));
                        log("new E =  " + temp);
                    }
                    string fore = temp.ToString();

                    BigInteger[] result = new BigInteger[3];
                    result = Extended_GCD(BigInteger.Parse(forTOT), BigInteger.Parse(fore));
                    if (result[2] < 0)
                        result[2] = result[2] + BigInteger.Parse(forTOT);
                    string ford = result[2].ToString();

                    log("N = P*Q = " + forp + " x " + forq);
                    string forn = (BigInteger.Multiply(BigInteger.Parse(forp), BigInteger.Parse(forq))).ToString();

                    string pubkeys1 = fore + "," + forn;
                    string seckeys1 = ford + "," + forn;

                    forKeys.Add(pubkeys1 + "@" + seckeys1);
                }
                MessageBox.Show("AES Keys are generated successfully!");
            }
            else if (comboBox1.SelectedItem.ToString().Equals("DBC"))
            {
                string forp = String.Empty;
                string txtMax = "10000000000";
                BigInteger p = RandomIntegerBelow(BigInteger.Parse(txtMax));
                while (!IsProbabilyPrime(p, 20))
                {
                    p = RandomIntegerBelow(BigInteger.Parse(txtMax));
                }
                forp = p.ToString();

                BigInteger q = RandomIntegerBelow(BigInteger.Parse(txtMax));
                while (!IsProbabilyPrime(q, 20))
                {
                    q = RandomIntegerBelow(BigInteger.Parse(txtMax));
                }
                string forq = q.ToString();

                log("totient = (P-1)*(Q-1) = " + (BigInteger.Parse(forp) - 1) + " x " + (BigInteger.Parse(forq) - 1));
                string forTOT = (BigInteger.Multiply(BigInteger.Parse(forp) - 1, BigInteger.Parse(forq) - 1)).ToString();

                log("generating e randomely such that gcd(e,totient) = 1");
                BigInteger temp = 0;
                while (GCD_Euclidean(temp, BigInteger.Parse(forTOT)) != 1)
                {
                    temp = RandomIntegerBelow(BigInteger.Parse(forTOT));
                    log("new E =  " + temp);
                }
                string fore = temp.ToString();

                BigInteger[] result = new BigInteger[3];
                result = Extended_GCD(BigInteger.Parse(forTOT), BigInteger.Parse(fore));
                if (result[2] < 0)
                    result[2] = result[2] + BigInteger.Parse(forTOT);
                string ford = result[2].ToString();

                log("N = P*Q = " + forp + " x " + forq);
                string forn = (BigInteger.Multiply(BigInteger.Parse(forp), BigInteger.Parse(forq))).ToString();

                pubKeys = fore + "," + forn;
                secKeys = ford + "," + forn;

                MessageBox.Show("AES Keys are generated successfully!");
                MessageBox.Show("Public Key: " + pubKeys + "\nSecret Key: " + secKeys, "Info", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("First choose any on Field Confidentiality!");
            }
        } 
        public static string ByteToString(byte[] buff)
        {
            string sbinary = "";

            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); // hex format
            }
            return sbinary;
        }

        string forlog = String.Empty;
        public void log(string s)
        {
            forlog += s + "\r\n";
        }
        public static BigInteger GCD_Loop(BigInteger A, BigInteger B)
        {
            BigInteger R = BigInteger.One;
            while (B != 0)
            {
                R = A % B;
                A = B;
                B = R;
            }
            return A;
        }

        public BigInteger GCD_Euclidean(BigInteger A, BigInteger B)
        {
            log("gcd(" + A + "," + B + ")");
            if (B == 0)
                return A;
            if (A == 0)
                return B;
            if (A > B)
                return GCD_Euclidean(B, A % B);
            else
                return GCD_Euclidean(B % A, A);
        }

        public bool IsProbabilyPrime(BigInteger n, int k)
        {
            bool result = false;
            if (n < 2)
                return false;
            if (n == 2)
                return true;
            // return false if n is even -> divisbla by 2
            if (n % 2 == 0)
                return false;
            //writing n-1 as 2^s.d
            BigInteger d = n - 1;
            BigInteger s = 0;
            while (d % 2 == 0)
            {
                d >>= 1;
                s = s + 1;
            }
            for (int i = 0; i < k; i++)
            {
                BigInteger a;
                do
                {
                    a = RandomIntegerBelow(n - 2);
                }
                while (a < 2 || a >= n - 2);

                if (BigInteger.ModPow(a, d, n) == 1) return true;
                for (int j = 0; j < s - 1; j++)
                {
                    if (BigInteger.ModPow(a, 2 * j * d, n) == n - 1)
                        return true;
                }
                result = false;
            }
            return result;
        }

        public BigInteger RandomIntegerBelow(int n)
        {
            var rng = new RNGCryptoServiceProvider();
            byte[] bytes = new byte[n / 8];

            rng.GetBytes(bytes);

            var msb = bytes[n / 8 - 1];
            var mask = 0;
            while (mask < msb)
                mask = (mask << 1) + 1;

            bytes[n - 1] &= Convert.ToByte(mask);
            BigInteger p = new BigInteger(bytes);
            return p;
        }

        public BigInteger RandomIntegerBelow(BigInteger bound)
        {
            var rng = new RNGCryptoServiceProvider();
            //Get a byte buffer capable of holding any value below the bound
            var buffer = (bound << 16).ToByteArray(); // << 16 adds two bytes, which decrease the chance of a retry later on

            //Compute where the last partial fragment starts, in order to retry if we end up in it
            var generatedValueBound = BigInteger.One << (buffer.Length * 8 - 1); //-1 accounts for the sign bit
            var validityBound = generatedValueBound - generatedValueBound % bound;

            while (true)
            {
                //generate a uniformly random value in [0, 2^(buffer.Length * 8 - 1))
                rng.GetBytes(buffer);
                buffer[buffer.Length - 1] &= 0x7F; //force sign bit to positive
                var r = new BigInteger(buffer);

                //return unless in the partial fragment
                if (r >= validityBound) continue;
                return r % bound;
            }
        }

        public BigInteger[] Extended_GCD(BigInteger A, BigInteger B)
        {
            BigInteger[] result = new BigInteger[3];
            bool reverse = false;
            if (A < B) //if A less than B, switch them
            {
                BigInteger temp = A;
                A = B;
                B = temp;
                reverse = true;
            }
            log("Extended AES");
            BigInteger r = B;
            BigInteger q = 0;
            BigInteger x0 = 1;
            BigInteger y0 = 0;
            BigInteger x1 = 0;
            BigInteger y1 = 1;
            BigInteger x = 0, y = 0;
            log(A + "\t" + " " + "\t" + x0 + "\t" + y0);
            log(B + "\t" + " " + "\t" + x1 + "\t" + y1);
            while (A % B != 0)
            {
                r = A % B;
                q = A / B;
                x = x0 - q * x1;
                y = y0 - q * y1;
                x0 = x1;
                y0 = y1;
                x1 = x;
                y1 = y;
                A = B;
                B = r;
                log(B + "\t" + r + "\t" + x + "\t" + y);
            }
            result[0] = r;
            if (reverse)
            {
                result[1] = y;
                result[2] = x;
            }
            else
            {
                result[1] = x;
                result[2] = y;
            }
            return result;
        }

        public BigInteger Extended_GCD2(BigInteger n, BigInteger m)
        {
            BigInteger[] Quot = new BigInteger[50];
            bool reverse = false;
            if (n < m)
            {
                BigInteger z;
                z = n;
                n = m;
                m = z;
                reverse = true;
            }
            BigInteger originaln = n;
            BigInteger originalm = m;
            int xstep = 1;
            BigInteger r = 1;
            while (r != 0)
            {
                BigInteger q = n / m;
                r = n - m * q;
                log(" " + n + " = " + m + "*" + q + " + " + r);
                n = m;
                m = r;
                Quot[xstep] = q;
                ++xstep;
            }
            //setgcd(n)
            BigInteger gcd = n;
            BigInteger a = 1;
            BigInteger b = 0;
            for (int i = xstep; i > 0; i--)
            {
                BigInteger z = b - Quot[i] * a;
                b = a;
                a = z;
            }

            return a;
        }

        private void button4_Click(object sender, EventArgs e1)
        {
            try
            {

                //Encrypt Metadata with generate AES Key

                string ownerclientname = cn;
                string fieldConfid = comboBox1.SelectedItem.ToString();

                string fileid = textBox2.Text;
                string fileName = textBox1.Text;
                string file = richTextBox1.Text;
                string fSize = fileSize;


                if (fieldConfid.Equals("COL"))
                {
                    //First Encrypt File Id

                    string keys = forKeys[0].ToString();
                    string[] bothkeys = keys.Split('@');
                    string pubkeys = bothkeys[0];
                    string cipfileid = "";
                    if (!(pubkeys.Equals("")))
                    {
                        String[] s = pubkeys.Split(',');
                        string e = s[0];
                        string n = s[1];

                        string ce = "";
                        foreach (char c in fileid)
                        {
                            Console.WriteLine((int)c);
                            string M = String.Empty + (int)c;
                            string ci = BigInteger.ModPow(BigInteger.Parse(M), BigInteger.Parse(e), BigInteger.Parse(n)).ToString();
                            ce = ce + ci + ",";
                        }
                        cipfileid = ce.Substring(0, ce.LastIndexOf(','));
                    }

                    //Second Encrypt File Name

                    string keys1 = forKeys[1].ToString();
                    string[] bothkeys1 = keys1.Split('@');
                    string pubkeys1 = bothkeys1[0];
                    string cipfileName = "";
                    if (!(pubkeys1.Equals("")))
                    {
                        String[] s = pubkeys1.Split(',');
                        string e = s[0];
                        string n = s[1];

                        string ce = "";
                        foreach (char c in fileName)
                        {
                            Console.WriteLine((int)c);
                            string M = String.Empty + (int)c;
                            string ci = BigInteger.ModPow(BigInteger.Parse(M), BigInteger.Parse(e), BigInteger.Parse(n)).ToString();
                            ce = ce + ci + ",";
                        }
                        cipfileName = ce.Substring(0, ce.LastIndexOf(','));
                    }

                    //Third Encrypt File

                    string keys2 = forKeys[2].ToString();
                    string[] bothkeys2 = keys2.Split('@');
                    string pubkeys2 = bothkeys2[0];
                    Console.WriteLine("----------------");
                    Console.WriteLine("pubkeys "+pubkeys2);
                    Console.WriteLine("secretkeys " + bothkeys2[1]);
                    Console.WriteLine("----------------");
                    string cipfile = "";
                    if (!(pubkeys2.Equals("")))
                    {
                        String[] s = pubkeys2.Split(',');
                        string e = s[0];
                        string n = s[1];

                        string ce = "";
                        foreach (char c in file)
                        {
                            Console.WriteLine((int)c);
                            string M = String.Empty + (int)c;
                            string ci = BigInteger.ModPow(BigInteger.Parse(M), BigInteger.Parse(e), BigInteger.Parse(n)).ToString();
                            ce = ce + ci + ",";
                        }
                        cipfile = ce.Substring(0, ce.LastIndexOf(','));
                    }

                    //Fourth Encrypt FileSize

                    string keys3 = forKeys[3].ToString();
                    string[] bothkeys3 = keys3.Split('@');
                    string pubkeys3 = bothkeys3[0];
                    string cipFileSize = "";
                    if (!(pubkeys3.Equals("")))
                    {
                        String[] s = pubkeys3.Split(',');
                        string e = s[0];
                        string n = s[1];

                        string ce = "";
                        foreach (char c in fSize)
                        {
                            Console.WriteLine((int)c);
                            string M = String.Empty + (int)c;
                            string ci = BigInteger.ModPow(BigInteger.Parse(M), BigInteger.Parse(e), BigInteger.Parse(n)).ToString();
                            ce = ce + ci + ",";
                        }
                        cipFileSize = ce.Substring(0, ce.LastIndexOf(','));
                    }

                    string ek = "", dk = "";
                    for (int i = 0; i < forKeys.Count; i++)
                    {
                        string[] pk = forKeys[i].ToString().Split('@');
                        ek = ek + pk[0] + "@";
                        dk = dk + pk[1] + "@";
                    }
                    string encKeys = ek.Substring(0, ek.LastIndexOf('@'));
                    string decKeys = dk.Substring(0, dk.LastIndexOf('@'));

                    string datatype = "String";
                    string enctype = "AES";
                    try
                    {
                        SqlConnection con1 = new SqlConnection(conStr);
                        con1.Open();

                        SqlCommand cmd = new SqlCommand("select * from UploadDetails", con1);
                        SqlDataReader reader1 = cmd.ExecuteReader();
                        int count = 0;
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

                            if ((cn.Equals(cn1)) && (cipfileid.Equals(fd1)) && (cipfileName.Equals(fn1)) && (cipfile.Equals(df1)) && (cipFileSize.Equals(fs1)) && (encKeys.Equals(ek1)) && (decKeys.Equals(dk1)) && (datatype.Equals(dt1)) && (enctype.Equals(et1)) && (fieldConfid.Equals(fc1)))
                            {
                                count = 1;
                            }
                        }
                        con1.Close();

                        if (count != 1)
                        {
                            SqlConnection con2 = new SqlConnection(conStr);
                            con2.Open();

                            String qry = "insert into UploadDetails values('" + cn + "','" + cipfileid + "','" + cipfileName + "','" + cipfile + "','" + cipFileSize + "','" + encKeys + "','" + decKeys + "','" + datatype + "','" + enctype + "','" + fieldConfid + "')";
                            Console.WriteLine(qry);
                            SqlCommand ins1 = con2.CreateCommand();
                            ins1.CommandText = qry;
                            ins1.ExecuteNonQuery();

                            con2.Close();
                            MessageBox.Show("Encrypted Successfully!", "Info", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);                        
                        }
                        else
                        {
                            MessageBox.Show("These Values are already existed!", "Info", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception e2)
                    {
                        Console.WriteLine(e2.ToString());
                    }
                }
                if (fieldConfid.Equals("MCOL"))
                {
                    //First Encrypt File Id

                    string keys = forKeys[0].ToString();
                    string[] bothkeys = keys.Split('@');
                    string pubkeys = bothkeys[0];
                    string cipfileid = "";
                    if (!(pubkeys.Equals("")))
                    {
                        String[] s = pubkeys.Split(',');
                        string e = s[0];
                        string n = s[1];

                        string ce = "";
                        foreach (char c in fileid)
                        {
                            Console.WriteLine((int)c);
                            string M = String.Empty + (int)c;
                            string ci = BigInteger.ModPow(BigInteger.Parse(M), BigInteger.Parse(e), BigInteger.Parse(n)).ToString();
                            ce = ce + ci + ",";
                        }
                        cipfileid = ce.Substring(0, ce.LastIndexOf(','));
                    }

                    //Second Encrypt File Name

                    string cipfileName = "";
                    if (!(pubkeys.Equals("")))
                    {
                        String[] s = pubkeys.Split(',');
                        string e = s[0];
                        string n = s[1];

                        string ce = "";
                        foreach (char c in fileName)
                        {
                            Console.WriteLine((int)c);
                            string M = String.Empty + (int)c;
                            string ci = BigInteger.ModPow(BigInteger.Parse(M), BigInteger.Parse(e), BigInteger.Parse(n)).ToString();
                            ce = ce + ci + ",";
                        }
                        cipfileName = ce.Substring(0, ce.LastIndexOf(','));
                    }

                    //Third Encrypt File

                    string keys2 = forKeys[1].ToString();
                    string[] bothkeys2 = keys2.Split('@');
                    string pubkeys2 = bothkeys2[0];
                    string cipfile = "";
                    if (!(pubkeys2.Equals("")))
                    {
                        String[] s = pubkeys2.Split(',');
                        string e = s[0];
                        string n = s[1];

                        string ce = "";
                        foreach (char c in file)
                        {
                            Console.WriteLine((int)c);
                            string M = String.Empty + (int)c;
                            string ci = BigInteger.ModPow(BigInteger.Parse(M), BigInteger.Parse(e), BigInteger.Parse(n)).ToString();
                            ce = ce + ci + ",";
                        }
                        cipfile = ce.Substring(0, ce.LastIndexOf(','));
                    }

                    //Fourth Encrypt FileSize

                    string cipFileSize = "";
                    if (!(pubkeys2.Equals("")))
                    {
                        String[] s = pubkeys2.Split(',');
                        string e = s[0];
                        string n = s[1];

                        string ce = "";
                        foreach (char c in fSize)
                        {
                            Console.WriteLine((int)c);
                            string M = String.Empty + (int)c;
                            string ci = BigInteger.ModPow(BigInteger.Parse(M), BigInteger.Parse(e), BigInteger.Parse(n)).ToString();
                            ce = ce + ci + ",";
                        }
                        cipFileSize = ce.Substring(0, ce.LastIndexOf(','));
                    }

                    string ek = "", dk = "";
                    for (int i = 0; i < forKeys.Count; i++)
                    {
                        string[] pk = forKeys[i].ToString().Split('@');
                        ek = ek + pk[0] + "@";
                        dk = dk + pk[1] + "@";
                    }
                    string encKeys = ek.Substring(0, ek.LastIndexOf('@'));
                    string decKeys = dk.Substring(0, dk.LastIndexOf('@'));

                    string datatype = "String";
                    string enctype = "RSA";
                    try
                    {
                        SqlConnection con1 = new SqlConnection(conStr);
                        con1.Open();

                        SqlCommand cmd = new SqlCommand("select * from UploadDetails", con1);
                        SqlDataReader reader1 = cmd.ExecuteReader();
                        int count = 0;
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

                            if ((cn.Equals(cn1)) && (cipfileid.Equals(fd1)) && (cipfileName.Equals(fn1)) && (cipfile.Equals(df1)) && (cipFileSize.Equals(fs1)) && (encKeys.Equals(ek1)) && (decKeys.Equals(dk1)) && (datatype.Equals(dt1)) && (enctype.Equals(et1)) && (fieldConfid.Equals(fc1)))
                            {
                                count = 1;
                            }
                        }
                        con1.Close();

                        if (count != 1)
                        {
                            SqlConnection con2 = new SqlConnection(conStr);
                            con2.Open();

                            String qry = "insert into UploadDetails values('" + cn + "','" + cipfileid + "','" + cipfileName + "','" + cipfile + "','" + cipFileSize + "','" + encKeys + "','" + decKeys + "','" + datatype + "','" + enctype + "','" + fieldConfid + "')";
                            Console.WriteLine(qry);
                            SqlCommand ins1 = con2.CreateCommand();
                            ins1.CommandText = qry;
                            ins1.ExecuteNonQuery();

                            con2.Close();
                            MessageBox.Show("Encrypted Successfully!", "Info", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);                        
                        }
                        else
                        {
                            MessageBox.Show("These Values are already existed!", "Info", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception e2)
                    {
                        Console.WriteLine(e2.ToString());
                    }
                }
                if (fieldConfid.Equals("DBC"))
                {
                    //First Encrypt File Id

                    string cipfileid = "";
                    if (!(pubKeys.Equals("")))
                    {
                        String[] s = pubKeys.Split(',');
                        string e = s[0];
                        string n = s[1];

                        string ce = "";
                        foreach (char c in fileid)
                        {
                            Console.WriteLine((int)c);
                            string M = String.Empty + (int)c;
                            string ci = BigInteger.ModPow(BigInteger.Parse(M), BigInteger.Parse(e), BigInteger.Parse(n)).ToString();
                            ce = ce + ci + ",";
                        }
                        cipfileid = ce.Substring(0, ce.LastIndexOf(','));
                    }

                    //Second Encrypt File Name

                    string cipfileName = "";
                    if (!(pubKeys.Equals("")))
                    {
                        String[] s = pubKeys.Split(',');
                        string e = s[0];
                        string n = s[1];

                        string ce = "";
                        foreach (char c in fileName)
                        {
                            Console.WriteLine((int)c);
                            string M = String.Empty + (int)c;
                            string ci = BigInteger.ModPow(BigInteger.Parse(M), BigInteger.Parse(e), BigInteger.Parse(n)).ToString();
                            ce = ce + ci + ",";
                        }
                        cipfileName = ce.Substring(0, ce.LastIndexOf(','));
                    }

                    //Third Encrypt File

                    string cipfile = "";
                    if (!(pubKeys.Equals("")))
                    {
                        String[] s = pubKeys.Split(',');
                        string e = s[0];
                        string n = s[1];

                        string ce = "";
                        foreach (char c in file)
                        {
                            Console.WriteLine((int)c);
                            string M = String.Empty + (int)c;
                            string ci = BigInteger.ModPow(BigInteger.Parse(M), BigInteger.Parse(e), BigInteger.Parse(n)).ToString();
                            ce = ce + ci + ",";
                        }
                        cipfile = ce.Substring(0, ce.LastIndexOf(','));
                    }

                    //Fourth Encrypt FileSize

                    string cipFileSize = "";
                    if (!(pubKeys.Equals("")))
                    {
                        String[] s = pubKeys.Split(',');
                        string e = s[0];
                        string n = s[1];

                        string ce = "";
                        foreach (char c in fSize)
                        {
                            Console.WriteLine((int)c);
                            string M = String.Empty + (int)c;
                            string ci = BigInteger.ModPow(BigInteger.Parse(M), BigInteger.Parse(e), BigInteger.Parse(n)).ToString();
                            ce = ce + ci + ",";
                        }
                        cipFileSize = ce.Substring(0, ce.LastIndexOf(','));
                    }

                    string encKeys = pubKeys;
                    string decKeys = secKeys;

                    string datatype = "String";
                    string enctype = "AES";
                    try
                    {
                        SqlConnection con1 = new SqlConnection(conStr);
                        con1.Open();

                        SqlCommand cmd = new SqlCommand("select * from UploadDetails", con1);
                        SqlDataReader reader1 = cmd.ExecuteReader();
                        int count = 0;
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

                            if ((cn.Equals(cn1)) && (cipfileid.Equals(fd1)) && (cipfileName.Equals(fn1)) && (cipfile.Equals(df1)) && (cipFileSize.Equals(fs1)) && (encKeys.Equals(ek1)) && (decKeys.Equals(dk1)) && (datatype.Equals(dt1)) && (enctype.Equals(et1)) && (fieldConfid.Equals(fc1)))
                            {
                                count = 1;
                            }
                        }
                        con1.Close();

                        if (count != 1)
                        {
                            SqlConnection con2 = new SqlConnection(conStr);
                            con2.Open();

                            String qry = "insert into UploadDetails values('" + cn + "','" + cipfileid + "','" + cipfileName + "','" + cipfile + "','" + cipFileSize + "','" + encKeys + "','" + decKeys + "','" + datatype + "','" + enctype + "','" + fieldConfid + "')";
                            Console.WriteLine(qry);
                            SqlCommand ins1 = con2.CreateCommand();
                            ins1.CommandText = qry;
                            ins1.ExecuteNonQuery();

                            con2.Close();
                            MessageBox.Show("Encrypted Successfully!", "Info", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);                        
                        }
                        else
                        {
                            MessageBox.Show("These Values are already existed!", "Info", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception e2)
                    {
                        Console.WriteLine(e2.ToString());
                    }
                }
            }
            catch (Exception e6)
            {
                Console.WriteLine(e6.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
                //Encrypt Data with generate MAC Key

            string ownerclientname = cn;
            string fieldConfid = comboBox1.SelectedItem.ToString();
            string fileid = textBox2.Text;
            string fileName = textBox1.Text;
            string file = richTextBox1.Text;
            string fSize = fileSize; 

            string message=ownerclientname+"@"+fieldConfid+"@"+fileid+"@"+fileName+"@"+file+"@"+fSize;

            string encryptdata = Encrypt(message);
            string cipher = "",sec="";
            if (encryptdata.Contains("@"))
            {
                string[] ed = encryptdata.Split('@');
                cipher = ed[0];
                sec = ed[1];
            }

            string key=fileName;                        
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(key);

            HMACMD5 hmacmd5 = new HMACMD5(keyByte);
           
            byte[] messageBytes = encoding.GetBytes(message);
            byte[] hashmessage = hmacmd5.ComputeHash(messageBytes);
            string macKey = ByteToString(hashmessage);

           

            try
            {
                SqlConnection con1 = new SqlConnection(conStr);
                con1.Open();

                SqlCommand cmd = new SqlCommand("select * from MetadataStorageTable", con1);
                SqlDataReader reader1 = cmd.ExecuteReader();
                int count = 0;
                while (reader1.Read())
                {
                    String cn1 = reader1.GetString(0);
                    String cip1 = reader1.GetString(1);
                    String mac1 = reader1.GetString(2);

                    if ((cn.Equals(cn1)) && (encryptdata.Equals(cip1)) && (encryptdata.Equals(mac1)))
                    {
                        count = 1;
                    }
                }
                con1.Close();

                if (count != 1)
                {
                    SqlConnection con2 = new SqlConnection(conStr);
                    con2.Open();

                    String qry = "insert into MetadataStorageTable values('" + cn + "','" + encryptdata + "','" + encryptdata + "')";
                    Console.WriteLine(qry);
                    SqlCommand ins1 = con2.CreateCommand();
                    ins1.CommandText = qry;
                    ins1.ExecuteNonQuery();

                    con2.Close();
                    MessageBox.Show("Uploaded Successfully!", "Info", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);                    
                }
                else
                {
                    MessageBox.Show("These Values are already existed!", "Info", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                }
            }
            catch (Exception e2)
            {
                Console.WriteLine(e2.ToString());
            }            
        }

        private string Encrypt(string message)
        {
            string forp = String.Empty;
            string txtMax = "10000000000";
            BigInteger p = RandomIntegerBelow(BigInteger.Parse(txtMax));
            while (!IsProbabilyPrime(p, 20))
            {
                p = RandomIntegerBelow(BigInteger.Parse(txtMax));
            }
            forp = p.ToString();

            BigInteger q = RandomIntegerBelow(BigInteger.Parse(txtMax));
            while (!IsProbabilyPrime(q, 20))
            {
                q = RandomIntegerBelow(BigInteger.Parse(txtMax));
            }
            string forq = q.ToString();

            log("totient = (P-1)*(Q-1) = " + (BigInteger.Parse(forp) - 1) + " x " + (BigInteger.Parse(forq) - 1));
            string forTOT = (BigInteger.Multiply(BigInteger.Parse(forp) - 1, BigInteger.Parse(forq) - 1)).ToString();

            log("generating e randomely such that gcd(e,totient) = 1");
            BigInteger temp = 0;
            while (GCD_Euclidean(temp, BigInteger.Parse(forTOT)) != 1)
            {
                temp = RandomIntegerBelow(BigInteger.Parse(forTOT));
                log("new E =  " + temp);
            }
            string fore = temp.ToString();

            BigInteger[] result = new BigInteger[3];
            result = Extended_GCD(BigInteger.Parse(forTOT), BigInteger.Parse(fore));
            if (result[2] < 0)
                result[2] = result[2] + BigInteger.Parse(forTOT);
            string ford = result[2].ToString();

            log("N = P*Q = " + forp + " x " + forq);
            string forn = (BigInteger.Multiply(BigInteger.Parse(forp), BigInteger.Parse(forq))).ToString();

            string pubkeys1 = fore + "," + forn;
            string seckeys1 = ford + "," + forn;
            string ce = "";
            foreach (char c in message)
            {
                Console.WriteLine((int)c);
                string M = String.Empty + (int)c;
                string ci = BigInteger.ModPow(BigInteger.Parse(M), BigInteger.Parse(fore), BigInteger.Parse(forn)).ToString();
                ce = ce + ci + ",";
            }            
            string cipher = ce.Substring(0, ce.LastIndexOf(','));
            return cipher+"@"+seckeys1;
        }

        private void UploadForm_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

      
    }
}
