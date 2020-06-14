using System;
using System.Data;
using System.IO;

namespace POS.Classes
{
    class Export
    {
        public static void Stock()
        {
            //FileHelpers.CsvEngine.DataTableToCsv(new Connection().GetDataSetQuery("SELECT Article,Company,Pairs,RetailPrice,SizeRange FROM STOCK ORDER BY Company").Tables[0], @"E:\file.csv");
            CreateCSVFileForStock(new Connection().GetDataSetQuery("SELECT Article,Company,Pairs,RetailPrice,SizeRange FROM STOCK ORDER BY Company").Tables[0], @"E:\StockExport.csv");
        }
        private static void CreateCSVFileForStock(DataTable dt, string strFilePath)
        {
            try
            {
                // Create the CSV file to which grid data will be exported.
                StreamWriter sw = new StreamWriter(strFilePath, false);
                // First we will write the headers.
                //DataTable dt = m_dsProducts.Tables[0];

                int iColCount = dt.Columns.Count;
                for (int i = 0; i < iColCount; i++)
                {
                    sw.Write(dt.Columns[i]);

                    if (i < iColCount - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);

                // Now write all the rows.
                
                foreach (DataRow dr in dt.Rows)
                {
                    using (DataTable temp = (new Stock()).getGridViewOfArticle(dr[0].ToString()).Tables[0])
                    {
                        int tc = temp.Rows.Count;
                        for (int i = 0; i < iColCount; i++)
                        {
                            if (!Convert.IsDBNull(dr[i]))
                            {
                                if (i < iColCount - 1)
                                    sw.Write(dr[i].ToString());
                                else
                                    sw.Write("|" + dr[i].ToString() + "|");
                            }
                            if (i <= iColCount - 1)
                            {
                                sw.Write(",");
                            }
                        }

                        for (int i = 0; i < tc; i++)
                        {
                            DataRow dr1 = temp.Rows[i];
                            if (!Convert.IsDBNull(dr1[1]))
                            {
                                sw.Write(dr1[1].ToString());
                            }
                            if (i < tc - 1)
                            {
                                sw.Write(",");
                            }
                        }
                        sw.Write(sw.NewLine);
                    }
                }
                sw.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void Sale()
        {

            FileHelpers.CsvEngine.DataTableToCsv((new Sale()).getGridViewOfSaleBy((DateTime.Now.ToString("yyyy-MM-dd"))).Tables[0], @"E:\"+ (DateTime.Now.ToString("yyyy-MM-dd"))+ "-MonthSale.csv");
            //CreateCSVFile(new Connection().GetDataSetQuery("SELECT Article,Company,Pairs,RetailPrice,SizeRange FROM STOCK ORDER BY Company").Tables[0], @"E:\file.csv");
        }
    }
}
