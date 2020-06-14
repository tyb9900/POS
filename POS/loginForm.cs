using POS.Classes;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS
{
    public partial class loginForm : Form
    {
        public loginForm()
        {
            InitializeComponent();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Escape))
            {
                this.Hide();
                if(exit==0)
                {
                    Environment.Exit(0);
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        int exit=0;

        public int Exit
        {
            get
            {
                return exit;
            }

            set
            {
                exit = value;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {

                if (new login().check(userText.Text, textBox1.Text))
                {
                    if (exit == 0)
                        exit = 1;
                    else
                        exit = 2;
                    task();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        public void task()
        {
            try
            {
                if (exit == 1)
                {
                    Hide();
                    var m = new Form1();
                    m.Logedin = true;
                    m.Logedtype = false;
                    if (userText.Text=="MGR")
                    {
                        m.Logedtype = true;
                    }
                    m.Show();

                }
                else
                {
                    Hide();

                    Environment.Exit(0);
                    //this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new System.Net.WebClient())
                using (client.OpenRead("http://clients3.google.com/generate_204"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        private void loginForm_Load(object sender, EventArgs e)
        {
            bool res = Authorize.AuthorizeComputer();
            if(res)
            {
                //Computer Authorized
                label2.Visible = false;
                textBox2.Visible = false;
            }
            else
            {
                this.label1.Visible = false;
                this.label10.Visible = false;
                this.textBox1.Visible = false;
                this.userText.Visible = false;
                if(!CheckForInternetConnection())
                {
                    MessageBox.Show("Internet Connection Required For First Run After Installation!",
                        "No Internet Connection", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    this.Hide();
                    Environment.Exit(0);
                }
            }
        }
        private void chck()
        {
            if (Authorize.AddLicenseKey(textBox2.Text))
            {
                label2.Visible = false;
                textBox2.Visible = false;
                this.label1.Visible = true;
                this.label10.Visible = true;
                this.textBox1.Visible = true;
                this.userText.Visible = true;
            }
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

            if (textBox2.Text.Length==16)
            {
                textBox2.Enabled = false;
                chck();
                textBox2.Enabled = true;
            }
        }
    }
}
