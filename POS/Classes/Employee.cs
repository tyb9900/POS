using System;
using System.Windows.Forms;

namespace POS.Classes
{
    class Employee
    {
        Connection EmployeeConnection;
        int id;
        string name;
        string cnic;
        string contact;
        double salary;
        string joindate;

        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public string Cnic
        {
            get
            {
                return cnic;
            }

            set
            {
                cnic = value;
            }
        }

        public string Contact
        {
            get
            {
                return contact;
            }

            set
            {
                contact = value;
            }
        }

        public double Salary
        {
            get
            {
                return salary;
            }

            set
            {
                salary = value;
            }
        }

        public string Joindate
        {
            get
            {
                return joindate;
            }

            set
            {
                joindate = value;
            }
        }

        public Employee()
        {
            try
            {
                EmployeeConnection = new Connection();
                Id = 0;
                Name = null;
                Cnic = null;
                Contact = null;
                Salary = 0.0;
                Joindate = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        public Employee(int id,string name,string cnic,string contact,double salary,string joindate)
        {
            try
            {
            EmployeeConnection = new Connection();
            this.Id = id;
            this.Name = name;
            this.Cnic = cnic;
            this.Contact = contact;
            this.Salary = salary;
            this.Joindate = joindate;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        public bool InsertIntoDB()
        {
            try
            {
                string query = "INSERT INTO EMPLOYEE VALUES(" + Id + ",'" + Name + "','" + Cnic + "','" + Contact + "',"+Salary+",'"+Joindate+"')";
                EmployeeConnection.ExecuteQuery(query);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return false;
        }
        public bool UpdatetIntoDB()
        {
            try
            {
                string query = "UPDATE SALE SET NAME='"+ Name + "',CNIC='" + Cnic + "',CONTACT='" + Contact + "',SALARY=" + Salary + ",JOINDATE='" + Joindate + "' WHERE ID="+Id+")";
                EmployeeConnection.ExecuteQuery(query);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return false;
        }
        public string getNameBy(string ID)
        {
            try
            {
            return EmployeeConnection.getStringValue("SELECT Name FROM EMPLOYEE WHERE ID='"+ID+"'");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return null;
        }
        public System.Data.DataSet getGridViewOfEmployee()
        {
            try
            {
            return EmployeeConnection.GetDataSetQuery("SELECT *FROM EMPLOYEE");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return null;
        }
        public int getLatestID()
        {
            try
            {
            return EmployeeConnection.getIntValue("SELECT MAX(ID) + 1 FROM EMPLOYEE");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return 0;
        }
    }
}
