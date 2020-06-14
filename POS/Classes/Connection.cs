using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace POS.Classes
{
    class Connection
    {
       SqlConnection cn;
       public Connection()
        {
            try
            {
                cn = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=POS;Integrated Security=True");
                cn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        public bool ExecuteQuery(string query)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(query, cn);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return false;
        }
        public DataSet GetDataSetQuery(string query)
        {
            try
            {
                var dataAdapter = new SqlDataAdapter(query,cn);
                var commandBuilder = new SqlCommandBuilder(dataAdapter);
                var ds = new DataSet();
                dataAdapter.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return null;
        }
        public bool chekIfAlreadyExist(string query)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(query, cn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                    if (dr.HasRows == true)
                    {
                        dr.Close();
                        return true;
                    }
                dr.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return false;
        }
        public double getDoubleValue(string query)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(query, cn);
                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    Decimal v = reader.GetDecimal(0);
                    reader.Close();
                    return Convert.ToDouble(v);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return 0.0;
        }
        public int getIntValue(string query)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(query, cn);
                using (var reader = cmd.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        try
                        {
                            int v = reader.GetInt32(0);
                            reader.Close();
                            return v;
                        }
                        catch(Exception ex)
                        {
                            return 0;
                        }
                    }
                    else
                    {
                        MessageBox.Show("O");
                    }
                }
            }
           catch (Exception ex)
            {
               MessageBox.Show(ex.Message.ToString());
            }
            return 0;
        }
        public string getStringValue(string query)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(query, cn);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (reader.HasRows)
                        {
                            string v = reader.GetString(0);
                            reader.Close();
                            return v;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("O"+ex.Message.ToString());
            }
            return "0";
        }
        public string getDateValue(string query)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(query, cn);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (reader.HasRows)
                        {
                            string v = reader.GetDateTime(0).ToString();
                            reader.Close();
                            return v;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return "0";
        }
        ~Connection()
        {
            try
            {
                SqlConnection.ClearPool(cn);
            }
            catch(Exception ex)
            {

            }
        }
    }
}
