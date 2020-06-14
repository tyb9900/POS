using System;
using System.Windows.Forms;

namespace POS.Classes
{
    public struct SaleInfo
    {
        private string article;
        private string company;
        private int size;
        private int pairs;
        private double price;
        private double amount;
        private double discount;

        public string Article
        {
            get
            {
                return article;
            }

            set
            {
                article = value;
            }
        }

        public string Company
        {
            get
            {
                return company;
            }

            set
            {
                company = value;
            }
        }

        public int Size
        {
            get
            {
                return size;
            }

            set
            {
                size = value;
            }
        }

        public int Pairs
        {
            get
            {
                return pairs;
            }

            set
            {
                pairs = value;
            }
        }

        public double Price
        {
            get
            {
                return price;
            }

            set
            {
                price = value;
            }
        }

        public double Amount
        {
            get
            {
                return amount;
            }

            set
            {
                amount = value;
            }
        }

        public double Discount
        {
            get
            {
                return discount;
            }

            set
            {
                discount = value;
            }
        }
    }
    class Sale
    {
        Connection SaleConnection;
        string orderid;
        string date;
        double totalAmount;
        int staffcode;
        SaleInfo[] SaleInfo;
        public Sale()
        {
            SaleConnection = new Connection();
            SaleInfo = null;
            orderid = null;
            date = null;
            staffcode = 0;
        }
        public Sale(string orderid,string date,double totalAmount,int staffcode,SaleInfo[] SaleInfo)
        {
            SaleConnection = new Connection();
            this.orderid = orderid;
            this.date = date;
            this.totalAmount = totalAmount;
            this.staffcode = staffcode;
            this.SaleInfo = SaleInfo;
        }
        public bool InsertIntoDB()
        {
            try
            {
                string query = "INSERT INTO SALE VALUES('" + orderid + "','" + date + "'," + totalAmount + ","+staffcode+")";
                SaleConnection.ExecuteQuery(query);
                for (int i = 0; i < SaleInfo.Length; i++)
                {
                    SaleConnection.ExecuteQuery("INSERT INTO SALEINFO VALUES('"+orderid+"','" + SaleInfo[i].Article + "','"+SaleInfo[i].Company+"'," + SaleInfo[i].Size + "," + SaleInfo[i].Pairs + "," + SaleInfo[i].Price + "," + SaleInfo[i].Discount + "," + SaleInfo[i].Amount + ")");
                    int currentPairs=SaleConnection.getIntValue("SELECT PAIRS FROM SIZEINFO WHERE ARTICLE='" + SaleInfo[i].Article + "' AND SIZE=" + SaleInfo[i].Size);
                    SaleConnection.ExecuteQuery("UPDATE STOCK SET PAIRS=" + (currentPairs - SaleInfo[i].Pairs) + "WHERE ARTICLE='" + SaleInfo[i].Article + "'");
                    SaleConnection.ExecuteQuery("UPDATE SIZEINFO SET PAIRS=" + (currentPairs - SaleInfo[i].Pairs)+ "WHERE ARTICLE='" + SaleInfo[i].Article + "' AND SIZE=" + SaleInfo[i].Size);
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return false;
        }
        public string getNextOrder()
        {
            string s = null;
           
            try
            {
                s = SaleConnection.getStringValue("SELECT MAX(OrderID) FROM SALE");
                string d = SaleConnection.getDateValue("SELECT MAX(DATE) FROM SALE");
                if (d == null || d == String.Empty || d == "0")
                {
                    return string.Format("{0:yyMMdd}", DateTime.Now) + "0001";
                }
                DateTime dt1 = DateTime.Parse(d);
                DateTime dt2 = DateTime.Now;

                if (dt1.Date < dt2.Date)
                {
                    s = string.Format("{0:yyMMdd}", DateTime.Now) + "0001";
                }
                else if(dt1.Date==dt2.Date)
                {
                    return (Convert.ToInt32(s) + 1).ToString();
                }
                else
                {
                    s = SaleConnection.getStringValue("SELECT MAX(OrderID) FROM SALE WHERE DATE=CONVERT(DATE,GETDATE())");
                    if(s==null || s==String.Empty || s=="0" || s=="")
                    {
                        s = string.Format("{0:yyMMdd}", DateTime.Now) + "0001";
                    }
                    else
                    {
                        return (Convert.ToInt32(s) + 1).ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                s = string.Format("{0:yyMMdd}", DateTime.Now) + "0001";
            }
            return s;
        }
        public System.Data.DataSet getGridViewOfSale(string type,string fromDate,string toDate)
        {
            try
            {
            if (type == "All")
            {
                return SaleConnection.GetDataSetQuery("SELECT S.DATE AS Date ,S.OrderID AS Order#,(SELECT SUM(Pairs) FROM SALEINFO WHERE OrderID=S.OrderID)AS Pairs,(SELECT SUM(Amount) FROM SALEINFO WHERE OrderID=S.OrderID) AS Amount FROM SALE S WHERE DATE<='"+toDate+"' AND DATE>='"+fromDate+"'");
            }
            else
            {
                return SaleConnection.GetDataSetQuery("SELECT S.Date,S.OrderID  AS Order#,SI.Article,SI.Size,SI.Pairs,SI.Price,SI.Discount,SI.Amount FROM SALE S, SaleInfo SI WHERE S.OrderId=SI.OrderID AND COMPANY='"+type+"' AND DATE<='" + toDate + "' AND DATE>='" + fromDate + "'");
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return null;
        }
        public System.Data.DataSet getGridViewOfSaleBy(string type, string fromDate, string toDate)
        {
            try
            {
            if(type=="Staff")
                return SaleConnection.GetDataSetQuery("SELECT S.StaffCode,(SELECT E.NAME FROM EMPLOYEE E WHERE S.StaffCode=E.ID) AS Name,SUM(SI.Pairs) AS TotalPairs,SUM(SI.Amount) AS TotalAmount FROM SALEINFO SI, SALE S WHERE S.OrderId=SI.OrderID AND S.DATE<='" + toDate + "' AND S.DATE>='" + fromDate + "'" + "GROUP BY S.StaffCode");
            else
                return SaleConnection.GetDataSetQuery("SELECT SI."+type+ ",SUM(SI.Pairs) AS TotalPairs,SUM(SI.Amount) AS TotalAmount FROM SALEINFO SI, SALE S WHERE S.OrderId=SI.OrderID AND S.DATE<='" + toDate + "' AND S.DATE>='" + fromDate + "'"+"GROUP BY SI."+type);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return null;
        }
        public System.Data.DataSet getGridViewOfSaleBy(string date)
        {
            try
            {
                string[] d = date.Split('-');
                return SaleConnection.GetDataSetQuery("SELECT S.DATE As Date,SUM(SI.PAIRS) As Pairs,(SUM(SI.AMOUNT)+SUM(SI.Discount)) As Amount,SUM(SI.Discount) As Disount,(SUM(SI.AMOUNT)) As TotalAmount FROM SALEINFO SI,SALE S WHERE S.OrderID=SI.OrderID AND MONTH(S.Date)=" + d[1] + " AND YEAR(S.Date)=" + d[0] + " GROUP BY S.DATE");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return null;
        }
        public System.Data.DataSet getGridViewOfArticleSaleBy(string type)
        {
            try
            {
            if(type=="best")
            {
                return SaleConnection.GetDataSetQuery("SELECT SI.ARTICLE,SUM(SI.PAIRS) AS SoldPairs,SUM(S.Pairs) As StockPairs FROM SALEINFO SI, STOCK S WHERE S.Article=SI.Article GROUP BY SI.Article,S.Article ORDER BY SUM(SI.PAIRS) DESC");
            }
            else if(type=="slow")
            {
                return SaleConnection.GetDataSetQuery("SELECT SI.ARTICLE,SUM(SI.PAIRS) AS SoldPairs,SUM(S.Pairs) As StockPairs FROM SALEINFO SI, STOCK S WHERE S.Article=SI.Article GROUP BY SI.Article ORDER BY SUM(SI.PAIRS) ASC");
            }
            else
            {
                return SaleConnection.GetDataSetQuery("SELECT S.ARTICLE AS Article ,0 AS SoldPairs,S.PAIRS As StockPairs FROM STOCK S LEFT JOIN SALEINFO SI ON  S.ARTICLE = SI.Article WHERE  SI.ARTICLE IS NULL AND S.ARTICLE IS NOT NULL");
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return null;
        }

        public System.Data.DataSet getGridViewOfOrder(string orderID)
        {
            try
            {
            return SaleConnection.GetDataSetQuery("SELECT Article,Size,Pairs FROM SALEINFO WHERE OrderID='" + orderID + "'");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return null;
        }
        public int getArticleSoldInMonth(string art,string date)
        {
            try
            { 
            string[] d = date.Split('-');
            return SaleConnection.getIntValue("SELECT SUM(SI.PAIRS) AS PAIRS FROM SALEINFO SI,SALE S WHERE  SI.OrderID=S.OrderID AND SI.ARTICLE='"+art+ "' AND MONTH(S.DATE)=" + d[1]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return 0;
        }
        public int getArtilceRefundInMonth(string art, string date)
        {
            try
            {
                string[] d = date.Split('-');
                return SaleConnection.getIntValue("SELECT SUM(SI.PAIRS) AS PAIRS FROM SALEINFO SI,SALE S WHERE  SI.OrderID=S.OrderID AND SI.ARTICLE='" + art + "' AND SI.PAIRS<0 AND MONTH(S.DATE)=" + d[1]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return 0;
        }
    }
}