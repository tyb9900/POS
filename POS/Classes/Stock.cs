using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace POS.Classes
{
    public struct SizeInformation
    {
        public int size;
        public int pairs;
    }
    class Stock
    {
        Connection StockConnection;
        string article;
        string company;
        double costPrice;
        double retailPrice;
        string sizeRange;
        int totalPairs;
        double costAmount;
        double retailAmount;
        SizeInformation []sizes;

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

        public double CostPrice
        {
            get
            {
                return costPrice;
            }

            set
            {
                costPrice = value;
            }
        }

        public double RetailPrice
        {
            get
            {
                return retailPrice;
            }

            set
            {
                retailPrice = value;
            }
        }

        public string SizeRange
        {
            get
            {
                return sizeRange;
            }

            set
            {
                sizeRange = value;
            }
        }

        public SizeInformation[] Sizes
        {
            get
            {
                return sizes;
            }

            set
            {
                sizes = value;
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

        public double RetailAmount
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

        public int TotalPairs
        {
            get
            {
                return totalPairs;
            }

            set
            {
                totalPairs = value;
            }
        }

        public Stock()
        {
            try
            {
            StockConnection = new Connection();
            Article = "";
            Company = "";
            CostPrice = 0.0;
            RetailPrice = 0.0;
            SizeRange = "";
            Sizes = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        public Stock(string article,string company,double costPrice,double retailPrice,string sizeRange,int totalPairs,double costAmount,double retailAmount,SizeInformation[] sizes)
        {
            try
            {
                StockConnection = new Connection();
                this.Article = article;
                this.Company = company;
                this.CostPrice = costPrice;
                this.RetailPrice = retailPrice;
                this.SizeRange = sizeRange;
                this.Sizes = sizes;
                this.totalPairs = totalPairs;
                this.costAmount = costAmount;
                this.retailAmount = retailAmount;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        public bool InsertIntoDB()
        {
            try
            {
                string query = "INSERT INTO STOCK VALUES('" + Article + "','" + Company + "'," + CostPrice + "," + RetailPrice + ",'" + SizeRange + "'," + totalPairs + "," + costAmount + "," + retailAmount + ")";
                StockConnection.ExecuteQuery(query);
                for (int i = 0; i < Sizes.Length; i++)
                {
                    StockConnection.ExecuteQuery("INSERT INTO SIZEINFO VALUES('" + Article + "'," + Sizes[i].size + "," + Sizes[i].pairs + ")");
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return false;
        }
        public bool UpdateArticleInfo()
        {
            try
            {
                StockConnection.ExecuteQuery("Update Stock Set CostPrice='"+this.CostPrice+"',RetailPrice = '"+this.RetailPrice+"' WHERE Article='"+this.Article+"'");
                for (int i = 0; i < Sizes.Length; i++)
                {
                    StockConnection.ExecuteQuery("UPDATE SIZEINFO SET Pairs='" + Sizes[i].pairs + "' WHERE Size='" + Sizes[i].size + "' AND ARTICLE='" + Article+"'");
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return false;
        }
        public bool UpdateIntoDB()
        {
            try
            {
                for (int i = 0; i < Sizes.Length; i++)
                {
                    Sizes[i].pairs += StockConnection.getIntValue("SELECT Pairs FROM SIZEINFO WHERE Article='" + Article + "' AND Size=" + Sizes[i].size);
                    StockConnection.ExecuteQuery("UPDATE SIZEINFO SET Pairs=" + Sizes[i].pairs + "Where Article='" + Article + "' AND Size=" + Sizes[i].size);
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return false;
        }
        public bool isAlreadyInDB()
        {
            try
            {
                if (StockConnection.chekIfAlreadyExist("SELECT *FROM STOCK WHERE Article='" + Article + "'"))
                {
                    this.TotalPairs = StockConnection.getIntValue("SELECT SUM(PAIRS) FROM SIZEINFO WHERE Article='" + Article + "'");
                    StockConnection.ExecuteQuery("UPDATE STOCK SET PAIRS='"+TotalPairs+"' WHERE Article='"+Article+"'");
                    this.Company = StockConnection.getStringValue("SELECT Company FROM STOCK WHERE Article='" + Article + "'");
                    this.CostPrice = StockConnection.getDoubleValue("SELECT CostPrice FROM STOCK WHERE Article='" + Article + "'");
                    this.RetailPrice = StockConnection.getDoubleValue("SELECT RetailPrice FROM STOCK WHERE Article='" + Article + "'");
                    this.SizeRange = StockConnection.getStringValue("SELECT sizeRange FROM STOCK WHERE Article='" + Article + "'");
                  //  this.TotalPairs = StockConnection.getIntValue("SELECT Pairs FROM STOCK WHERE Article='" + Article + "'");

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return false;
        }
        public int getPairsOfArtileBySize(int size)
        {
            try
            {
            return StockConnection.getIntValue("SELECT Pairs FROM SIZEINFO WHERE Article = '" + Article + "' AND Size="+size);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return 0;
        }
        public System.Data.DataSet getGridViewOfStock(string type)
        {
            try
            {
            if(type=="All")
            {
                return StockConnection.GetDataSetQuery("SELECT S.Article,S.Company,S.RetailPrice,(SELECT SUM(Pairs) FROM SIZEINFO WHERE ARTICLE=S.Article) AS Pairs,S.RetailPrice*(SELECT SUM(Pairs) FROM SIZEINFO WHERE ARTICLE=S.Article) AS Amount FROM STOCK S");
            }
            else
            {
                return StockConnection.GetDataSetQuery("SELECT S.Article,S.Company,S.RetailPrice,(SELECT SUM(Pairs) FROM SIZEINFO WHERE ARTICLE=S.Article) AS Pairs,S.RetailPrice*(SELECT SUM(Pairs) FROM SIZEINFO WHERE ARTICLE=S.Article) AS Amount FROM STOCK S WHERE COMPANY='"+type+"'");
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return null;
        }
        public System.Data.DataSet getGridViewOfArticle(string article)
        {
            try
            {
                return StockConnection.GetDataSetQuery("SELECT Size,Pairs FROM SIZEINFO WHERE ARTICLE='"+article+"'");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return null;
        }
        public System.Data.DataSet getGridViewOfStockPriceDetails()
        {
            try
            {
                    return StockConnection.GetDataSetQuery("SELECT S.Article,"
                    +"S.Pairs AS C_Pairs, SH.ArticlePairs AS INV_Pairs,"
                    +"S.CostPrice AS C_CostPrice, SH.CostPrice AS INV_CostPrice,"
                    +"S.RetailPrice AS C_RetailPrice, SH.RetailPrice AS INV_RetailPrice,"
                    + "S.CostAmount*S.Pairs AS C_CostAmount, SH.ArticleCostAmount AS INV_CostAmount,"
                    + "S.RetailAmount*S.Pairs AS C_RetailAmount, SH.ArticleRetailAmount AS INV_RetailAmount "
                    + "FROM STOCK S, STOCKHISTORYDETAILS SH WHERE S.Article = SH.Article AND S.RetailPrice != SH.RetailPrice");
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return null;
        }
        ~Stock()
        {
            //
        }
    }
}