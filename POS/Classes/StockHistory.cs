using System;
using System.Data;
using System.Windows.Forms;

namespace POS.Classes
{
    class StockHistory
    {
        Connection StockHistoryConnection;
        string date;
        string invoice;
        string party;
        int pairs;
        double costAmount;
        double retailAmount;
        Stock[] stock;
        public StockHistory()
        {
            try
            {
            StockHistoryConnection = new Connection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        public StockHistory(string date,string invoice,string party,int pairs,double costAmount,double retailAmount,Stock[] stock)
        {
            try
            {
            StockHistoryConnection = new Connection();
            this.date = date;
            this.invoice = invoice;
            this.party = party;
            this.pairs = pairs;
            this.costAmount = costAmount;
            this.retailAmount = retailAmount;
            this.stock = stock;
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
                string query = "INSERT INTO STOCKHISTORY VALUES('" + date + "','" + invoice + "','" + party + "'," + pairs + "," + costAmount + "," + retailAmount + ")";
                StockHistoryConnection.ExecuteQuery(query);
                for (int i = 0; i < stock.Length; i++)
                {
                    StockHistoryConnection.ExecuteQuery("INSERT INTO STOCKHISTORYDETAILS VALUES('" + invoice + "','" + stock[i].Article + "','" + stock[i].Company + "'," + stock[i].CostPrice + "," + stock[i].RetailPrice + ",'" + stock[i].SizeRange + "'," + stock[i].TotalPairs + "," + stock[i].CostAmount + "," + stock[i].RetailAmount + ")");
                    for(int j=0;j<stock[i].Sizes.Length;j++)
                    {
                        StockHistoryConnection.ExecuteQuery("INSERT INTO STOCKHISTORYARTICLEDETAILS VALUES('" + invoice + "','" + stock[i].Article + "'," + stock[i].Sizes[j].size + "," + stock[i].Sizes[j].pairs + ")");
                    }
                    if (!stock[i].isAlreadyInDB())
                        stock[i].InsertIntoDB();
                    else
                        stock[i].UpdateIntoDB();
                }
                return true;
            }
               catch (Exception ex)
            {
                      MessageBox.Show(ex.Message.ToString());
            }
            return false;
        }
        public System.Data.DataSet getGridViewOfStockHistory()
        {
            try
            {
                return StockHistoryConnection.GetDataSetQuery("SELECT *FROM STOCKHISTORY");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return null;
        }
        public System.Data.DataSet getGridViewOfStockHistoryDetails(string type)
        {
            try
            {
            if (type == "All")
            {
                return StockHistoryConnection.GetDataSetQuery("SELECT *FROM STOCKHISTORYDETAILS");
            }
             else
            {
                return StockHistoryConnection.GetDataSetQuery("SELECT *FROM STOCKHISTORYDETAILS WHERE COMPANY='"+type+"'");
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return null;
        }
        public System.Data.DataSet getGridViewOfStockHistoryBy(string type)
        {
            try {
            return StockHistoryConnection.GetDataSetQuery("SELECT " + type + ",COUNT(Invoice) AS TotalInvoices,Sum(InvoicePairs) AS TotalPairs,Sum(InvoiceCostAmount) As TotalCostAmount,Sum(InvoiceRetailAmount) AS TotalRetailAmount FROM STOCKHISTORY GROUP BY " + type);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return null;
        }
        public DataSet getGridViewOfStockHistoryDetailsBy(string type)
        {
            try {
            return StockHistoryConnection.GetDataSetQuery("SELECT "+type+",Sum(ArticlePairs) AS TotalPairs,Sum(ArticleCostAmount) As TotalCostAmount,Sum(ArticleRetailAmount) AS TotalRetailAmount FROM STOCKHISTORYDETAILS GROUP BY "+type);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return null;
        }
        public string Date { get { return date; } set { date = value; } }
        public string Invoice { get {return invoice; } set {invoice= value; } }
        public string Party { get { return party; } set { Party = value; } }
        public Stock[] Stock { get { return stock; } set { Stock = value; } }

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

        public double CostAmount
        {
            get
            {
                return costAmount;
            }

            set
            {
                costAmount = value;
            }
        }

        public double ReatilAmount
        {
            get
            {
                return retailAmount;
            }

            set
            {
                retailAmount = value;
            }
        }
    }
}
