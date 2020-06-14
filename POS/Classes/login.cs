using System;
using System.Windows.Forms;

namespace POS.Classes
{
    class login
    {
        Connection loginConnection;
        private string username;
        private string password;
        private bool logedin;
        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
            }
        }

        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
            }
        }

        public bool Logedin
        {
            get
            {
                return logedin;
            }

            set
            {
                logedin = value;
            }
        }
        public bool check(string user, string pass)
        {
            try
            {
            loginConnection = new Connection();
            return (loginConnection.chekIfAlreadyExist("SELECT *FROM login WHERE USERNAME='" + user + "' AND PASSWORD='" + pass + "'")) ;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return false;
        }

        public bool UpdatePassword(string user,string pass)
        {
            try
            {
                loginConnection = new Connection();
                return loginConnection.ExecuteQuery("UPDATE Login SET PASSWORD='" + pass + "' WHERE USERNAME='" + user + "'");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return false;
        }
    }
}
