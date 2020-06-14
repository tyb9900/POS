using System;
using System.Data.SqlClient;
using System.Management;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace POS.Classes
{
    class Authorize
    {
        static public bool AuthorizeComputer()
        {
            return AuthorizeCridentials(GetUniqueKey());
        }
        static private bool AuthorizeCridentials(string IDMBADMAKIDCP)
        {
            try
            {
                Connection cn = new Connection();
                return (cn.chekIfAlreadyExist("SELECT *FROM AUTHORIZE WHERE IDMBADMAKIDCP='" + IDMBADMAKIDCP + "'"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return false;
        }
        static public bool chekIfAlreadyExist(string query)
        {
            try
            {
                SqlConnection cn = new SqlConnection("Server=tcp:licensekeys.database.windows.net,1433;Initial Catalog=LICENSE;Persist Security Info=False;User ID=infinitydevs;Password=03027203844Qf@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
                cn.Open();
                SqlCommand cmd = new SqlCommand(query, cn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                    if (dr.HasRows == true)
                    {
                        dr.Close();
                        return true;
                    }
                dr.Close();
                cn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return false;
        }
        static public bool AddLicenseKey(string key)
        {
            try
            {
                SqlConnection cn = new SqlConnection("Server=tcp:licensekeys.database.windows.net,1433;Initial Catalog=LICENSE;Persist Security Info=False;User ID=infinitydevs;Password=03027203844Qf@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
                cn.Open();
                Connection mycon = new Connection();
                if(chekIfAlreadyExist("SELECT *FROM LICENSEKEY WHERE STATUS='YES' AND KEYS='" + key + "'"))
                {
                    MessageBox.Show("This License Key Already Has Been Used. Contact Infinity Devs To Buy A License Key.",
                        "License Key Expired", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                }
                else if(chekIfAlreadyExist("SELECT *FROM LICENSEKEY WHERE STATUS='NOT' AND KEYS='" + key + "'"))
                {
                    mycon.ExecuteQuery("INSERT INTO AUTHORIZE VALUES('" + key + "','" + GetUniqueKey() + "')");
                    new SqlCommand("UPDATE LICENSEKEY SET STATUS='YES' , USEROF='" + Environment.UserName + "' WHERE KEYS='"+key+"'",cn).ExecuteNonQuery();
                    MessageBox.Show("You've registred infinity devs POS successfully!\nUse username : emp and password : 123 for first use",
                    "Registered Successfully!", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    MessageBox.Show("You've entered a worng license key! Contact Infinity Devs To Buy A License Key.",
                    "Wrong License Key", MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);
                }
                cn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return false;
        }
        static private string GetMotherBoardID()
        {
            try
            {
                ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
                ManagementObjectCollection moc = mos.Get();
                string motherBoard = "";
                foreach (ManagementObject mo in moc)
                {
                    motherBoard = (string)mo["SerialNumber"];
                }
                return motherBoard;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return null;
        }
        static private string GetProcessorID()
        {
            try
            {
                var mbs = new ManagementObjectSearcher("Select ProcessorID From Win32_processor");
                var mbsList = mbs.Get();
                string cpuid=null;
                foreach (ManagementObject mo in mbsList)
                {
                    cpuid = mo["ProcessorID"].ToString();
                }
                return cpuid;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return null;
        }
        static private string GetMacAddress()
        {
            string macAddresses = string.Empty;
            try
            {
                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (nic.OperationalStatus == OperationalStatus.Up)
                    {
                        macAddresses += nic.GetPhysicalAddress().ToString();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return macAddresses;
        }
        static private string GetUniqueKey()
        {
            return GetMotherBoardID() + GetMacAddress() + GetProcessorID();
        }
    }
}
