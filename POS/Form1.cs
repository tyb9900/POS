using POS.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
namespace POS
{
    public partial class Form1 : Form
    {
        private bool logedin;
        private bool logedtype;
        public Form1()
        {
            InitializeComponent();   
        }
        /// <summary>
        /// Helping Functions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        List<Stock> addingStock = null;
        public void singlePointInput(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == '.' || e.KeyChar=='-'))
            { e.Handled = true; }
            TextBox txtDecimal = sender as TextBox;
            if (e.KeyChar == '.' && txtDecimal.Text.Contains("."))
            {
                e.Handled = true;
            }
        }
        void funKeys(object sender,EventArgs e,int k)
        {

        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if(keyData == (Keys.PageUp))
            {
                double am = Convert.ToDouble(totalAmount.Text);
                
                if(orderGridView.Rows.Count > 0)
                {
                    am-= Convert.ToDouble(orderGridView.Rows[orderGridView.Rows.Count - 1].Cells[6].Value.ToString());
                    orderGridView.Rows.RemoveAt(orderGridView.Rows.Count - 1);
                }
                totalAmount.Text = String.Format("{0:0.00}", am);
            }
            if (keyData == (Keys.Escape))
            {
                var l = new loginForm();
                l.Exit = 2;
                l.Show();
            }
                if (keyData == (Keys.End))
            {
                if(addStockDownPanel.Visible==true && addingStock!=null && addingStock.Count>0)
                {
                    showCurrentAddedInvoice();
                }
                else
                {
                    MessageBox.Show("No Data Found");
                }
            }

            if (keyData == (Keys.Space))
            {
                if(dailyDownPanel.Visible==true && orderGridView.RowCount>0)
                {
                    this.amountTextBox.Enabled = true;
                    this.amountTextBox.Focus();
                }
            }
            if (keyData == (Keys.Enter))
            {
                if (amountTextBox.Enabled)
                {
                    if (amountTextBox.Text != null && amountTextBox.Text != "")
                    {
                        makeSale();
                    }
                }
                SendKeys.Send("{TAB}");
            }
            if(groupBox20.Enabled==false)
            {
                if (keyData == (Keys.S))
                {
                    groupBox20.Enabled = true;
                    textBox1.Focus();
                }
            }

            if (groupStockInfo.Enabled == false)
            {
                if (keyData == (Keys.E))
                {
                    groupStockInfo.Enabled = true;
                    //textBox1.Focus();
                }
            }

            if (keyData == (Keys.D))
            {
                if(dailyDownPanel.Visible==true)
                {
                    if(dailyCompanyTextBox.Text!="")
                    {
                        saleDiscount.Enabled = true;
                        saleDiscount.Focus();
                    }
                }
            }
            if (keyData == (Keys.M))
            {
                if (logedtype)
                {
                    MGRMPanel.Visible = !MGRMPanel.Visible;


                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        Form form = null;
        DataGridView g1 = null;
        DataGridView g2 = null;
        private void showCurrentAddedInvoice()
        {
            try
            {
                var source = new BindingSource();
                source.DataSource = addingStock;
                form = new Form();
                form.KeyPreview = true;
                form.KeyDown += new KeyEventHandler(onkeyPress);
                form.StartPosition = FormStartPosition.Manual;
                form.Location = new Point(0, 0);
                form.ShowInTaskbar = false;
                form.ShowIcon = false;
                form.ControlBox = false;
                form.FormBorderStyle = FormBorderStyle.FixedSingle;
                form.Width = this.Width;
                form.Height = this.Height - 10;
                Label inv = new Label();
                inv.Text = "INVOICE : " + addStockInvoiceNo.Text;
                inv.Location = new Point(10, 30);
                inv.AutoSize = true;
                inv.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                inv.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(72)))), ((int)(((byte)(54)))));
                //this.label8.Size = new System.Drawing.Size(100, 50);

                Button addbtn = new Button();
                addbtn.Text = "Confirm";
                addbtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(72)))), ((int)(((byte)(54)))));
                addbtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                addbtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                addbtn.ForeColor = System.Drawing.Color.White;
                addbtn.Location = new System.Drawing.Point(300, 20);
                addbtn.Size = new System.Drawing.Size(152, 50);
                //this.Home.UseVisualStyleBackColor = false;
                Button updtbtn = new Button();
                updtbtn.Text = "Update";
                updtbtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(72)))), ((int)(((byte)(54)))));
                updtbtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                updtbtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                updtbtn.ForeColor = System.Drawing.Color.White;
                updtbtn.Location = new System.Drawing.Point(480, 20);
                updtbtn.Size = new System.Drawing.Size(152, 50);
                //this.Home.UseVisualStyleBackColor = false;

                Label c = new Label();
                c.Text = "Press Ctrl+C To Confirm\nPress Ctrl+U To Update";
                c.Location = new Point(700, 20);
                c.AutoSize = true;
                c.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                c.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(72)))), ((int)(((byte)(54)))));
                //this.label8.Size = new System.Drawing.Size(100, 50);


                Panel p1 = new Panel();
                p1.Width = this.Width;
                p1.Height = 100;
                p1.BackColor = Color.White;
                p1.Controls.Add(inv);
                p1.Controls.Add(c);
                p1.Controls.Add(addbtn);
                p1.Controls.Add(updtbtn);
                g1 = new DataGridView();
                g1.AllowUserToAddRows = false;

                //g1.Dock = DockStyle.Fill;
                //g1.Location = new Point(0, 100);
                g1.BackgroundColor = Color.White;
                g1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                g1.Height = this.Height - p1.Height;
                g1.Width = this.Width;
                g1.ColumnCount = 5;
                g1.Columns[0].Name = "Article";
                g1.Columns[1].Name = "Size";
                g1.Columns[2].Name = "Pairs";
                g1.Columns[3].Name = "Cost Price";
                g1.Columns[4].Name = "Retail Price";
                Stock[] arr = addingStock.ToArray();

                for (int i = 0; i < arr.Length; i++)
                {
                    int pairs = 0;
                    for (int j = 0; j < arr[i].Sizes.Length; j++)
                    {
                        pairs += arr[i].Sizes[j].pairs;
                        string[] row = {arr[i].Article,arr[i].Sizes[j].size.ToString(),arr[i].Sizes[j].pairs.ToString(),
                arr[i].CostPrice.ToString(),arr[i].RetailPrice.ToString()};
                        g1.Rows.Add(row);
                    }
                }

                /////////////////////////////////////////////
                //g2 = new DataGridView();
                //g2.Height = 350;
                //g2.Width = 200;
                //g2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                //g2.Location = new Point(850, 50);
                //g2.ColumnCount = 2;
                //g2.Columns[0].Name = "Size";
                //g2.Columns[1].Name = "Pairs";
                //g2.ReadOnly = true;
                //g2.AllowUserToAddRows = false;
                //g2.BackgroundColor = Color.White;
                //g2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                //Label l1 = new Label();
                //l1.Text = "Article : ";
                //l1.Location = new Point(750, 20);
                //Label l2 = new Label();
                //l2.Text = "CHDI0050";
                //l2.Location = new Point(800, 20);

                int tp1 = 0;
                double tcp1 = 0.0;
                double trp1 = 0.0;
                for (int i = 0; i < arr.Length; i++)
                {
                    tp1 += arr[i].TotalPairs;
                    tcp1 += arr[i].CostAmount;
                    trp1 += arr[i].RetailAmount;
                }

                Label c1 = new Label();
                c1.Text = "Total Pairs : " + tp1.ToString() + "\nTotal Cost Amount : " + tcp1.ToString() + "\nTotal Retail Amount : " + trp1.ToString();
                c1.Location = new Point(1000, 10);
                c1.AutoSize = true;
                c1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                c1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(72)))), ((int)(((byte)(54)))));

                TextBox dinvtb = new TextBox();
                dinvtb.Text = "TOTAL";
                dinvtb.Location = new Point(1, 400);
                dinvtb.Width = 135;
                dinvtb.Font = new Font("Times New Roman", 16.0f,
                            FontStyle.Bold);
                TextBox tptb = new TextBox();
                tptb.Text = tp1.ToString();
                tptb.Location = new Point(140, 400);
                tptb.Width = 100;
                tptb.Font = new Font("Times New Roman", 16.0f,
                            FontStyle.Bold);
                TextBox tctb = new TextBox();
                tctb.Text = tcp1.ToString();
                tctb.Location = new Point(250, 400);
                tctb.Width = 100;
                tctb.Font = new Font("Times New Roman", 16.0f,
                            FontStyle.Bold);
                TextBox trtb = new TextBox();
                trtb.Text = trp1.ToString();
                trtb.Location = new Point(360, 400);
                trtb.Width = 135;
                trtb.Font = new Font("Times New Roman", 16.0f,
                            FontStyle.Bold);
                Panel p2 = new Panel();

                p2.Width = this.Width;
                p2.Height = this.Height - p1.Height - 10;
                p2.Location = new Point(0, 100);
                p2.BackColor = Color.FromArgb(0, 255, 255);
                p2.Controls.Add(g1);
                //p2.Controls.Add(g2);
                //p2.Controls.Add(l2);
                //p2.Controls.Add(l1);
                // p2.Controls.Add(dinvtb);
                // p2.Controls.Add(tptb);
                //p2.Controls.Add(tctb);
                //p2.Controls.Add(trtb);
                p1.Controls.Add(c1);

                form.Controls.Add(p1);
                form.Controls.Add(p2);
                //g1.SelectionChanged += (object sender, EventArgs e) =>
                //{
                //    if (g1.SelectedCells.Count > 0)
                //    {
                //        g2.Rows.Clear();
                //        int selectedrowindex = g1.SelectedCells[0].RowIndex;
                //        DataGridViewRow selectedRow = g1.Rows[selectedrowindex];
                //        string a = Convert.ToString(selectedRow.Cells["Article"].Value);
                //        //MessageBox.Show(a);
                //        //l2.Text = a;
                //        for(int i=0;i<arr.Length;i++)
                //        {
                //            if(arr[i].Article==a)
                //            {
                //                for(int j=0;j<arr[i].Sizes.Length;j++)
                //                {
                //                    g2.Rows.Add(arr[i].Sizes[j].size.ToString(), arr[i].Sizes[j].pairs.ToString());
                //                }
                //            }
                //        }
                //        //viewOrderInfoInGridView(a);
                //        //this.viewArticleInfoGridView.Columns[0].Width = 91;
                //        //this.viewArticleInfoGridView.Columns[1].Width = 100;
                //                           g2.RowHeadersVisible = false;

                //    }
                //};
                g1.UserDeletingRow += (object sender, DataGridViewRowCancelEventArgs e) =>
                 {
                     string ra = g1.Rows[e.Row.Index].Cells[0].Value.ToString();
                     addingStock.RemoveAll(s => s.Article == ra);
                 };
                updtbtn.Click += (object sender, EventArgs e) =>
                  {
                      g1.Rows.Clear();
                  //   g2.Rows.Clear();
                  form.Hide();
                  };
                addbtn.Click += (confirmButtonClicked);
                form.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                form.Show();
                g1.Focus();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        void onkeyPress(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Escape)
            {
                g1.Rows.Clear();
                //g2.Rows.Clear();
                form.Hide();
            }
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.C:
                        confirmButtonClicked(sender, e);
                        
                        break;
                    case Keys.U:
                        g1.Rows.Clear();
                  //      g2.Rows.Clear();
                        form.Hide();
                        break;
                    default:
                        break;
                }
            }
        }
        void confirmButtonClicked(object sender, EventArgs e)
        {
            try
            {
                Stock[] tar = addingStock.ToArray();
                int tp = 0;
                double tcp = 0.0;
                double trp = 0.0;
                for (int i = 0; i < tar.Length; i++)
                {
                    tp += tar[i].TotalPairs;
                    tcp += tar[i].CostAmount;
                    trp += tar[i].RetailAmount;
                }
                StockHistory SH = new StockHistory(changeDateForamt(addStockDate.Text), addStockInvoiceNo.Text, addStockParty.Text, tp, tcp, trp, addingStock.ToArray());
                if (SH.InsertIntoDB())
                {
                    MessageBox.Show("Data Entered Successfully!");
                }
                addingStock.Clear();
                g1.Rows.Clear();
                // g2.Rows.Clear();
                form.Hide();
                clearTextBoxes(groupStockInfo);
                groupStockInfo.Enabled = false;
                groupStockInfo.Enabled = true;
                addStockInvoiceNo.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        void hidePanels(Control T,Panel p)
        {
            foreach (Control c in T.Controls)
            {
                if (c is Panel && c!=p)
                {
                    c.Visible = false;
                }

            }
        }

        void clearTextBoxes(Control T)
        {
            foreach (Control c in T.Controls)
            {
                if (c is TextBox)
                {
                    TextBox questionTextBox = c as TextBox;
                    if (questionTextBox != null)
                    {
                        questionTextBox.Text = "";
                    }
                }

            }
        }
        void clearMaskTextBoxes(Control T)
        {
            foreach (Control c in T.Controls)
            {
                if (c is MaskedTextBox)
                {
                    MaskedTextBox questionTextBox = c as MaskedTextBox;
                    if (questionTextBox != null)
                    {
                        questionTextBox.Text = "";
                    }
                }

            }
        }
        void clearRadioButtons(Control T)
        {
            foreach (Control c in T.Controls)
            {
                if (c is RadioButton)
                {
                    RadioButton questionTextBox = c as RadioButton;
                    if (questionTextBox != null)
                    {
                        if (questionTextBox.Checked)
                            questionTextBox.Checked = false;
                    }
                }

            }
        }
        private string changeDateForamt(string date)
        {
            return DateTime.ParseExact(date, "dd'/'MM'/'yyyy", CultureInfo.InvariantCulture).ToString("yyyy'-'MM'-'dd");
        }
        /// <summary>
        /// Bakcend Functions
        /// </summary>
        private void designButtons(Control Parent,Size S,Color BC,Color FC,Font F)
        {
            foreach (Control c in Parent.Controls)
            {
                if (c is Button)
                {
                    Button CB = c as Button;
                    if (CB != null)
                    {
                        CB.Size = S;
                        CB.BackColor = BC;
                        CB.ForeColor = FC;
                        CB.Font = F;
                        CB.FlatStyle = FlatStyle.Flat;
                    }
                }

            }
        }
        private void designSingleGridGroupBox(Control Parent)
        {
            Parent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            Parent.Location = new System.Drawing.Point(0, 0);
            Parent.Size = new System.Drawing.Size(789, 566);
            Parent.TabIndex = 6;
            Parent.TabStop = false;
            foreach (Control c in Parent.Controls)
            {
                if (c is DataGridView)
                {
                    DataGridView CB = c as DataGridView;
                    if (CB != null)
                    {
                        CB.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    }
                }

            }
        }
        private void designPanels(Control Parent,Color C)
        {
            foreach (Control c in Parent.Controls)
            {
                if (c is Panel)
                {
                    Panel CB = c as Panel;
                    if (CB != null)
                    {
                        //if(CB.Name.ToString()== "viewStockHistoryDownPanel")
                        //{
                        //    designSingleGridGroupBox(CB);
                        //}
                        CB.BackColor = C;
                    }
                }

            }
            //this.viewSaleDownPanel.Controls.Add(this.specificSaleViewGroupBox);
            //this.viewSaleDownPanel.Controls.Add(this.groupBox1);
            //this.viewSaleDownPanel.Controls.Add(this.groupBox7);
            //this.viewSaleDownPanel.Controls.Add(this.groupBox8);
            //this.viewSaleDownPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.viewSaleDownPanel.Location = new System.Drawing.Point(0, 0);
            //this.viewSaleDownPanel.Name = "viewSaleDownPanel";
            //this.viewSaleDownPanel.Size = new System.Drawing.Size(1118, 569);
            //this.viewSaleDownPanel.TabIndex = 7;
        }
        private void designSpecificValueGroupBox(Control Parent,Point Loc)
        {
            Parent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            Parent.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Parent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(197)))), ((int)(((byte)(210)))));
            Parent.Location = new System.Drawing.Point(Loc.X,Loc.Y);
            Parent.Size = new System.Drawing.Size(360, 95);
            Parent.TabStop = false;
            int y = 18;
            foreach (Control c in Parent.Controls)
            {
                if (c is Label)
                {
                    Label CB = c as Label;
                    if (CB != null)
                    {
                        designValuesLables(CB, new Point(9, y));
                        y += 41;
                    }
                }

            }
            y = 19;
            foreach (Control c in Parent.Controls)
            {
                if (c is TextBox)
                {
                    TextBox CB = c as TextBox;
                    if (CB != null)
                    {
                        CB.BorderStyle = BorderStyle.None;
                        designValuesTextBoxes(CB, new Point(114, y));
                        y += 38;
                    }
                }

            }

        }
        private void designValuesLables(Control Parent,Point Loc)
        {
            Parent.AutoSize = true;
            Parent.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Parent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(72)))), ((int)(((byte)(54)))));
            Parent.Location = new System.Drawing.Point(Loc.X, Loc.Y);
            Parent.Size = new System.Drawing.Size(68, 29);
        }
        private void designValuesTextBoxes(Control Parent,Point Loc)
        {
            Parent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
           
           // Parent.BorderStyle = BorderStyle.None;
            Parent.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Parent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(72)))), ((int)(((byte)(54)))));
            Parent.BackColor = Color.White;
            Parent.Location = new System.Drawing.Point(Loc.X, Loc.Y);
            Parent.Size = new System.Drawing.Size(150, 28);
            Parent.TabIndex = 1;
        }
        private void designValuesGroupBox(Control Parent)
        {
            Parent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            Parent.BackColor = System.Drawing.Color.White;
            Parent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(72)))), ((int)(((byte)(54)))));
            Parent.Font = new System.Drawing.Font("Segoe UI Black", 22F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Parent.Location = new System.Drawing.Point(818, 9);
            Parent.Size = new System.Drawing.Size(370, 570);
            int y = 45;
            List<GroupBox> g = new List<GroupBox>();
            foreach (Control c in Parent.Controls)
            {
                if (c is GroupBox)
                {
                    GroupBox CB = c as GroupBox;
                    if (CB != null)
                    {
                        g.Add(CB);
                    }
                }
            }
            g = g.OrderBy(o => o.Name).ToList();
            foreach(GroupBox t in g)
            {
                designSpecificValueGroupBox(t, new Point(6, y));
                y += 100;
            }
        }
        //Facade
        private void designForm()
        {
            designButtons(splitContainer1.Panel1, new Size(147, 50), Color.FromArgb(239, 72, 54), Color.White, new Font("Microsoft Sans Serif", 16));
            designButtons(sideBarEMPPanel, new Size(147, 50), Color.FromArgb(239, 72, 54), Color.White, new Font("Microsoft Sans Serif", 16));
            designButtons(MGRMPanel, new Size(147, 50), Color.FromArgb(239, 72, 54), Color.White, new Font("Microsoft Sans Serif", 16));

            designButtons(viewSaleUpPanel, new Size(110, 60), Color.FromArgb(255, 255, 255), Color.FromArgb(239, 72, 54), new Font("Microsoft Sans Serif", 14));
            designValuesGroupBox(this.stockHistoryTotalsGroupBox);
            designPanels(splitContainer2.Panel2, Color.FromArgb(255, 254, 249));
            designButtons(viewStockHistoryUpPanel, new Size(180, 50), Color.FromArgb(255, 255, 255), Color.FromArgb(239, 72, 54), new Font("Microsoft Sans Serif", 18));
            designButtons(viewStockUpPanel, new Size(175, 50), Color.FromArgb(255, 255, 255), Color.FromArgb(239, 72, 54), new Font("Microsoft Sans Serif", 18));
            designValuesGroupBox(this.groupBoxTotals);

        }
        private void Init()
        {
            if(!logedin)
            {
                this.Hide();
                var e = new loginForm();
                e.Show();
            }
            if(logedtype)
            {
                sideBarEMPPanel.Visible = false;
            }
            else
            {
                sideBarEMPPanel.Visible = true;
                SendKeys.Send("{F2}");
            }
            designForm();
            this.orderno.Text = string.Format("{0:yyMMdd}", DateTime.Now);
            this.addStockDate.Text = string.Format("{0:dd/MM/yyyy}", DateTime.Now);
            this.saleDate.Text = string.Format("{0:dd/MM/yyyy}", DateTime.Now);
            this.empDateMaskedBox.Text = string.Format("{0:dd/MM/yyyy}", DateTime.Now);
            this.fromDateBox.Text = string.Format("{0:dd/MM/yyyy}", DateTime.Now);
            this.toDateBox.Text = string.Format("{0:dd/MM/yyyy}", DateTime.Now);
            this.addStockDate.Text = string.Format("{0:dd/MM/yyyy}", DateTime.Now);
            this.ActiveControl = this.addStockInvoiceNo;
            this.addStockDate.TabIndex = 1;
            this.addStockInvoiceNo.TabIndex = 2;
            this.addStockParty.TabIndex = 3;
            this.addStockArticle.TabIndex = 4;
            this.addStockCostPrice.TabIndex = 5;
            this.addStockRetailPrice.TabIndex = 6;
        }
        private string getCompany(string art, TextBox T)
        {
            string Company = "";
            if (art.Length == 1 && (art == "X" || art == "x"))
            {
                Company = "Xara";
            }
            else if (art.Length == 3 && (art == "LOC" || art == "loc" || art == "Loc"))
            {
                Company = "Local";
            }
            else if (art.Length == 3 && art.All(char.IsDigit))
            {
                Company = "Bata";
            }
            else if ((art.Length == 4) && art.All(char.IsLetter))
            {
                Company = "Servis";
            }
            else if((art.Length == 5 || art.Length == 6) && art.All(char.IsLetter))
            {
                Company = "Local";
            }
            else
            {
                Company = T.Text;
            }
            return Company;
        }
        private void makeSizeGrid(object sender, EventArgs e, string range)
        {
            try
            {
                this.addStockArticleLable.Text = this.addStockArticle.Text;
                sizeGrid.Rows.Clear();
                sizeGrid.Visible = true;
                if (range != "09-01")
                {
                    string[] arr = range.Split('-');
                    int min = Convert.ToInt32(arr[0]);
                    int max = Convert.ToInt32(arr[1]);
                    int size = max - min + 1;
                    sizeGrid.ColumnCount = size;
                    for (int i = min, j = 0; i <= max; i++, j++)
                    {
                        sizeGrid.Columns[j].Name = i.ToString();
                    }
                    string[] row = new string[size];
                    for (int i = 0; i < size; i++)
                    {
                        row[i] = "0";
                    }
                    sizeGrid.Rows.Add(row);
                }
                else
                {

                }
                //    sizeGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                //   sizeGrid.AutoResizeColumns();
                //  sizeGrid.AllowUserToResizeColumns = true;
                // sizeGrid.AllowUserToOrderColumns = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        void addSaleToGridView(object sender, EventArgs e)
        {
     //       try
            {
                if(saleDiscount.Text.Contains('%'))
                {
                    saleDiscount.Text =  saleDiscount.Text.Replace("%",string.Empty);
                    //MessageBox.Show(saleDiscount.Text);
                    saleDiscount.Text = (Convert.ToInt32(salePairs.Text)*(Convert.ToDouble(salePrice.Text) * (Convert.ToDouble(saleDiscount.Text)/100.00))).ToString();
                }
                string[] row = { saleArticle.Text,dailyCompanyTextBox.Text,saleSize.Text,salePairs.Text,salePrice.Text,saleDiscount.Text,
                (((Convert.ToDouble(salePrice.Text) * (Convert.ToDouble(salePairs.Text)))-Convert.ToDouble(saleDiscount.Text))).ToString()};
                double tm = (((Convert.ToDouble(salePrice.Text) * Convert.ToDouble(salePairs.Text)) - Convert.ToDouble(saleDiscount.Text)));
                tm += Convert.ToDouble(totalAmount.Text);
                totalAmount.Text = tm.ToString();
                orderGridView.Rows.Add(row);
                string t = saleStaff.Text;
                clearTextBoxes(groupAddPair);
                saleStaff.Text = t;
                saleDiscount.Text = "0";
                salePairs.Text = "1";
                saleSize.Text = "0";
                saleArticle.Focus();
            }
       //     catch (Exception ex)
            {
         //       MessageBox.Show(ex.Message.ToString());
            }
        }
        void addEmployeeToGridView(object sender, EventArgs e)
        {
            try
            {
                Employee emp = new Employee(Convert.ToInt32(staffCodeTextBox.Text), nameTextBox.Text, cnicTextBox.Text, contactMaskBox.Text, Convert.ToDouble(slarayTextBox.Text), changeDateForamt(empDateMaskedBox.Text));
                if(emp.InsertIntoDB())
                MessageBox.Show("Employee Added!");
                clearTextBoxes(addEmployeeGroupBox);
                clearMaskTextBoxes(addEmployeeGroupBox);
                this.empDateMaskedBox.Text = string.Format("{0:dd/MM/yyyy}", DateTime.Now);

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        void addStockToGridView()
        {
            try
            {
                SizeInformation[] sizes = new SizeInformation[sizeGrid.ColumnCount];
                int tp = 0;
                for (int i = 0; i < sizeGrid.ColumnCount; i++)
                {
                    sizes[i].size = Convert.ToInt32(sizeGrid.Columns[i].HeaderText);
                }
                for (int i = 0; i < sizeGrid.Rows[0].Cells.Count; i++)
                {
                    sizes[i].pairs = Convert.ToInt32(sizeGrid.Rows[0].Cells[i].Value.ToString());
                    tp += sizes[i].pairs;
                }
                string sizeRangeS = sizes[0].size.ToString() + "-" + sizes[sizeGrid.ColumnCount - 1].size.ToString();

                Stock stock = new Stock(addStockArticle.Text, addStockCompany.Text, Convert.ToDouble(addStockCostPrice.Text), Convert.ToDouble(addStockRetailPrice.Text), sizeRangeS, tp, (tp * Convert.ToDouble(addStockCostPrice.Text)), (tp * Convert.ToDouble(addStockRetailPrice.Text)), sizes);

                addingStock.Add(stock); //Adding Stock To LIST
                MessageBox.Show("Data Added");
                clearTextBoxes(groupArticleInfo);
                clearRadioButtons(groupSizeRange);
                sizeGrid.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        void addStock(object sender, EventArgs e)
        {
         //   try
           // {
           //     SizeInformation[] sizes = new SizeInformation[sizeGrid.ColumnCount];
           //     for (int i = 0; i < sizeGrid.ColumnCount; i++)
           //     {
           //         sizes[i].size = Convert.ToInt32(sizeGrid.Columns[i].HeaderText);
           //     }
           //     for (int i = 0; i < sizeGrid.Rows[0].Cells.Count; i++)
           //     {
           //         sizes[i].pairs = Convert.ToInt32(sizeGrid.Rows[0].Cells[i].Value.ToString());
           //     }
           //     string sizeRangeS = sizes[0].size.ToString() + "-" + sizes[sizeGrid.ColumnCount - 1].size.ToString();
           //     Stock stock = new Stock(addStockArticle.Text, addStockCompany.Text, Convert.ToDouble(addStockCostPrice.Text), Convert.ToDouble(addStockRetailPrice.Text),sizeRangeS, sizes);
           //     if (!stock.isAlreadyInDB())
           //     {
           //         if (stock.InsertIntoDB())
           //         {
           //             MessageBox.Show("Data Inserted SuccessFully");
           //             clearTextBoxes(groupArticleInfo);
           //             clearRadioButtons(groupSizeRange);
           //             sizeGrid.Visible = false;
           //         }
           //         else
           //         {
           //             MessageBox.Show("Some Error Occured!");
           //         }
           //     }
           //     else
           //     {
           //         if (stock.UpdateIntoDB())
           //         {
           //             MessageBox.Show("Data Inserted SuccessFully");
           //             clearTextBoxes(groupArticleInfo);
           //             clearRadioButtons(groupSizeRange);
           //             sizeGrid.Visible = false;
           //         }
           //         else
           //         {
           //             MessageBox.Show("Some Error Occured!");
           //         }
           //     }
           // }
           //// catch (Exception ex)
           // {
           //  //   MessageBox.Show(ex.Message.ToString());
           // }
        }
        void viewStockInGridView(string type)
        {
            try
            {
                viewStockGridView.ReadOnly = true;
                viewStockGridView.AllowUserToAddRows = false;
                viewStockGridView.DataSource = (new Stock()).getGridViewOfStock(type).Tables[0];
                viewStockGridView.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        void viewArticleInfoInGridView(string article)
        {
            try
            {
                viewArticleInfoGridView.ReadOnly = true;
                viewArticleInfoGridView.AllowUserToAddRows = false;
                viewArticleInfoGridView.DataSource = (new Stock()).getGridViewOfArticle(article).Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        double[] getValuesOfStockHistory(string type)
        {
            double[] values = { 0.0, 0.0 };
            try
            {
                DataSet Data = (new StockHistory()).getGridViewOfStockHistoryDetails(type);
                foreach (DataRow dr in Data.Tables[0].Rows)
                {
                    values[0] += Convert.ToDouble(dr["ArticlePairs"].ToString());
                    values[1] += Convert.ToDouble(dr["ArticleRetailAmount"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return values;
        }
        double[] getValuesOfStock(string type)
        {
            double[] values = { 0.0, 0.0 };
            try
            {
                DataSet Data = (new Stock()).getGridViewOfStock(type);
                foreach (DataRow dr in Data.Tables[0].Rows)
                {
                    values[0] += Convert.ToDouble(dr["Pairs"].ToString());
                    values[1] += Convert.ToDouble(dr["Amount"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return values;
        }
        void viewStockHistoryFillVlaues()
        {
            try
            {
                double[] Values = getValuesOfStockHistory("All");
                this.stockHistoryAllPairsTextBox.Text = Values[0].ToString();
                this.stockHistoryAllAmountTextBox.Text = Values[1].ToString();
                Values = getValuesOfStockHistory("Bata");
                this.stockHistoryBataPairsTextBox.Text = Values[0].ToString();
                this.stockHistoryBataAmountTextBox.Text = Values[1].ToString();
                Values = getValuesOfStockHistory("Servis");
                this.stockHistoryServisPairsTextBox.Text = Values[0].ToString();
                this.stockHistoryServisAmountTextBox.Text = Values[1].ToString();
                Values = getValuesOfStockHistory("Xara");
                this.stockHistoryXaraPairsTextBox.Text = Values[0].ToString();
                this.stockHistoryXaraAmountTextBox.Text = Values[1].ToString();
                Values = getValuesOfStockHistory("Local");
                this.stockHistoryLocalPairsTextBox.Text = Values[0].ToString();
                this.stockHistoryLocalAmountTextBox.Text = Values[1].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        void viewStockFillVlaues()
        {
            try
            {
                double[] Values = getValuesOfStock("All");
                this.allPairs.Text = Values[0].ToString();
                this.allAmount.Text = Values[1].ToString();
                Values = getValuesOfStock("Bata");
                this.bataPairs.Text = Values[0].ToString();
                this.bataAmount.Text = Values[1].ToString();
                Values = getValuesOfStock("Servis");
                this.servisPairs.Text = Values[0].ToString();
                this.servisAmount.Text = Values[1].ToString();
                Values = getValuesOfStock("Xara");
                this.xaraPairs.Text = Values[0].ToString();
                this.xaraAmount.Text = Values[1].ToString();
                Values = getValuesOfStock("Local");
                this.localPairs.Text = Values[0].ToString();
                this.localAmount.Text = Values[1].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        void viewStockFillCharts()
        {
            try
            {
                double[] Values = getValuesOfStock("Bata");
                this.chart1.Series["Pairs"].Points.AddXY("Bata", Values[0]);
                this.chart2.Series["Amount"].Points.AddXY("Bata", Values[1]);
                Values = getValuesOfStock("Servis");
                this.chart1.Series["Pairs"].Points.AddXY("Servis", Values[0]);
                this.chart2.Series["Amount"].Points.AddXY("Servis", Values[1]);
                Values = getValuesOfStock("Xara");
                this.chart1.Series["Pairs"].Points.AddXY("Xara", Values[0]);
                this.chart2.Series["Amount"].Points.AddXY("Xara", Values[1]);
                Values = getValuesOfStock("Local");
                this.chart1.Series["Pairs"].Points.AddXY("Local", Values[0]);
                this.chart2.Series["Amount"].Points.AddXY("Local", Values[1]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        double[] getValuesOfSale(string type,string fromDate,string toDate)
        {
            double[] values = { 0.0, 0.0 };
            DataSet Data = (new Sale()).getGridViewOfSale(type,fromDate,toDate);
            foreach (DataRow dr in Data.Tables[0].Rows)
            {
                try
                {
                    values[0] += Convert.ToDouble(dr["Pairs"].ToString());
                    values[1] += Convert.ToDouble(dr["Amount"].ToString());
                }
                catch(Exception ex)
                {
                    //Igonre If Data Cell Is Emty
                }
            }
            return values;
        }
        void viewSaleFillVlaues(string fromDate, string toDate)
        {
            try
            {
                double[] Values = getValuesOfSale("All", fromDate, toDate);
                this.allSalePairs.Text = Values[0].ToString();
                this.allSaleAmount.Text = Values[1].ToString();
                Values = getValuesOfSale("Bata", fromDate, toDate);
                this.bataSalePairs.Text = Values[0].ToString();
                this.bataSaleAmount.Text = Values[1].ToString();
                Values = getValuesOfSale("Servis", fromDate, toDate);
                this.servisSalePairs.Text = Values[0].ToString();
                this.servisSaleAmount.Text = Values[1].ToString();
                Values = getValuesOfSale("Xara", fromDate, toDate);
                this.xaraSalePairs.Text = Values[0].ToString();
                this.xaraSaleAmount.Text = Values[1].ToString();
                Values = getValuesOfSale("Local", fromDate, toDate);
                this.localSalePairs.Text = Values[0].ToString();
                this.localSaleAmount.Text = Values[1].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        void addSale()
        {
            try
            {
                double tm = 0.0;
                SaleInfo[] saleInfo = new SaleInfo[orderGridView.RowCount];
                for (int i = 0; i < orderGridView.RowCount; i++)
                {
                    //MessageBox.Show(orderGridView.Rows[i].Cells[0].Value.ToString());
                    saleInfo[i].Article = orderGridView.Rows[i].Cells[0].Value.ToString();
                    saleInfo[i].Company = orderGridView.Rows[i].Cells[1].Value.ToString();
                    saleInfo[i].Size = Convert.ToInt32(orderGridView.Rows[i].Cells[2].Value.ToString());
                    saleInfo[i].Pairs = Convert.ToInt32(orderGridView.Rows[i].Cells[3].Value.ToString());
                    saleInfo[i].Price = Convert.ToDouble(orderGridView.Rows[i].Cells[4].Value.ToString());
                    saleInfo[i].Discount = Convert.ToDouble(orderGridView.Rows[i].Cells[5].Value.ToString());
                    saleInfo[i].Amount = Convert.ToDouble(orderGridView.Rows[i].Cells[6].Value.ToString());
                    tm += Convert.ToDouble(orderGridView.Rows[i].Cells[6].Value.ToString());
                    //string line = this.orderno.Text +" | " + saleInfo[i].Article + " | "+ saleInfo[i].Size.ToString() + " | " + saleInfo[i].Pairs.ToString() +
                    //    " | " + saleInfo[i].Price.ToString() + " | " + saleInfo[i].Discount.ToString() + " | " + saleInfo[i].Amount.ToString() + "\n";
                    //File.WriteAllText("Temp.txt",line);
                }
                Sale sale = new Sale(this.orderno.Text, changeDateForamt(saleDate.Text), tm, Convert.ToInt32(saleStaff.Text), saleInfo);
                sale.InsertIntoDB();
                makeRecipt();
                orderGridView.Rows.Clear();
                labledaily.Text = "B A L A N C E  :";
                totalAmount.Text = (Convert.ToDouble(amountTextBox.Text) - Convert.ToDouble(totalAmount.Text)).ToString();
                MessageBox.Show("Transaction Complete!");
                labledaily.Text = "Total Amount : ";
                amountTextBox.Text = "0.0";
                amountTextBox.Enabled = false;
                saleArticle.Focus();
                totalAmount.Text = "0.0";
                orderno.Text = sale.getNextOrder();
                saleDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        void makeSale()
        {
            addSale();
        }

        private void makeRecipt()
        {
            print();
        }
        PrintDocument pdoc = null;
        private Font printFont;

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

        public bool Logedtype
        {
            get
            {
                return logedtype;
            }

            set
            {
                logedtype = value;
            }
        }

        public void print()
        {
            try
            {

                printFont = new Font("Arial", 10);
                PrintDocument pd = new PrintDocument();
                PrintController printController = new StandardPrintController();
                pd.PrintController = printController;
               pd.PrinterSettings.PrinterName = "POS Printer";
                pd.PrintPage += new PrintPageEventHandler
                   (this.pd_PrintPage);
                pd.Print();
                //PrintDialog pd = new PrintDialog();
                //pdoc = new PrintDocument();
                //PrinterSettings ps = new PrinterSettings();
                //Font font = new Font("Courier New", 15);


                //PaperSize psize = new PaperSize("Custom", 100, 200);
                ////ps.DefaultPageSettings.PaperSize = psize;

                //pd.Document = pdoc;
                //pd.Document.DefaultPageSettings.PaperSize = psize;
                ////pdoc.DefaultPageSettings.PaperSize.Height =320;
                //pdoc.DefaultPageSettings.PaperSize.Height = 820;

                //pdoc.DefaultPageSettings.PaperSize.Width = 520;

                //pdoc.PrintPage += new PrintPageEventHandler(pdoc_PrintPage);

                //DialogResult result = pd.ShowDialog();
                //if (result == DialogResult.OK)
                //{
                //    PrintDocument pdq = new PrintDocument();
                //    pdq.PrintPage += new PrintPageEventHandler
                //       (this.pdq_PrintPage);
                //    pdq.Print();
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }
        void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                Graphics graphics = e.Graphics;
                Font font = new Font("Courier New", 10);
                float fontHeight = font.GetHeight();
                int startX = 30;
                int startY = 10;
                int Offset = 40;
                graphics.DrawString("Bata Comfort Shoes", new Font("Arial", 14,FontStyle.Bold),
                                    new SolidBrush(Color.Black), startX + 30, startY);
                Offset = Offset + 10;
                graphics.DrawString("Adam Chowk, GM Abad,\n     Faisalabad",
             new Font("Courier New", 10, FontStyle.Bold),
             new SolidBrush(Color.Black), startX+Offset -5, startY + Offset - 20);
                Offset = Offset + 20;
                graphics.DrawString("0345-2276206",
             new Font("Courier New", 10, FontStyle.Bold),
             new SolidBrush(Color.Black), startX + Offset + 10, startY + Offset - 10);
                Offset = Offset + 10;

                graphics.DrawString("Reciept# : " + orderno.Text,
                         new Font("Courier New", 10, FontStyle.Bold),
                         new SolidBrush(Color.Black), startX, startY + Offset);
                Offset = Offset + 25;
                graphics.DrawString("Date : " + saleDate.Text + "\t"  + DateTime.Now.ToString("hh:mm:ss"),
                         new Font("Courier New", 10, FontStyle.Bold),
                         new SolidBrush(Color.Black), startX, startY + Offset);
                Offset = Offset + 25;
                String underLine = "-------------------------------------------------";
                graphics.DrawString(underLine, new Font("Courier New", 8, FontStyle.Bold),
                         new SolidBrush(Color.Black), startX, startY + Offset);

                Offset = Offset + 20;
                //String Source = this.source;
                graphics.DrawString("Article\tPrice\tSz\tPr\tAmount", new Font("Courier New", 7, FontStyle.Bold),
                         new SolidBrush(Color.Black), startX, startY + Offset);
                int tpairs = 0;
                double tdisc = 0.0;
                double tamt = 0.0;
                for (int rows = 0; rows < orderGridView.Rows.Count; rows++)
                {
                    string art = orderGridView.Rows[rows].Cells[0].Value.ToString();
                    string price = orderGridView.Rows[rows].Cells[4].Value.ToString();
                    string size = orderGridView.Rows[rows].Cells[2].Value.ToString();
                    string pairs = orderGridView.Rows[rows].Cells[3].Value.ToString();
                    string disc = orderGridView.Rows[rows].Cells[5].Value.ToString();
                    string amt = ((Convert.ToDouble(price) * Convert.ToDouble(pairs))).ToString();
                    if (art.Length < 7)
                        art += "\t";
                    String Grosstotal = art + "\t" + string.Format("{0:0.00}", price) + "\t" + size + "\t" + pairs + "\t" + string.Format("{0:0.00}", amt);
                    Offset = Offset + 20;
                    graphics.DrawString(Grosstotal, new Font("Courier New", 7, FontStyle.Bold),
                             new SolidBrush(Color.Black), startX, startY + Offset);
                    tpairs += Convert.ToInt32(pairs);
                    tdisc += Convert.ToDouble(disc);
                    tamt += Convert.ToDouble(amt);
                }

                Offset = Offset + 20;
                underLine = "-------------------------------------------------";
                graphics.DrawString(underLine, new Font("Courier New", 8, FontStyle.Bold),
                         new SolidBrush(Color.Black), startX, startY + Offset);
                //  Offset = Offset + 20;
                Offset = Offset + 20;
                //String DrawnBy = this.drawnBy;
                graphics.DrawString("Pairs\t: " + tpairs.ToString() + "\t\tAmount\t: " + string.Format("{0:0.00}", tamt) + "\n\t\t\tDisc\t: " + string.Format("{0:0.00}", tdisc) + "\n\t\t\tTotal\t: " + string.Format("{0:0.00}", (tamt - tdisc)) + "\n", 
                    new Font("Courier New", 8, FontStyle.Bold),
                         new SolidBrush(Color.Black), startX, startY + Offset);
                //graphics.DrawString("\t\t\t\tAmount\t: " + string.Format("{0:0.00}", tamt) + "\n\t\t\t\t\tDisc\t: " + string.Format("{0:0.00}", tdisc) + "\n\t\t\t\t\tTotal\t: " + string.Format("{0:0.00}", (tamt - tdisc)) + "\n", new Font("Courier New", 10),
                //       new SolidBrush(Color.Black), startX+100, startY + Offset);
                Offset = Offset + 40;
                underLine = "-------------------------------------------------";
                graphics.DrawString(underLine, new Font("Courier New", 8, FontStyle.Bold),
                         new SolidBrush(Color.Black), startX, startY + Offset);
                Offset = Offset + 20;
                underLine = "Amount Recieved : " + string.Format("{0:0.00}", Convert.ToDouble(amountTextBox.Text));
                graphics.DrawString(underLine, new Font("Arial", 9,FontStyle.Bold),
                         new SolidBrush(Color.Black), startX+100, startY + Offset);
                Offset = Offset + 20;
                underLine = "Customer Balance: " + string.Format("{0:0.00}", Convert.ToDouble((Convert.ToDouble(amountTextBox.Text) - (tamt - tdisc)).ToString()));
                graphics.DrawString(underLine, new Font("Arial", 9, FontStyle.Bold),
                         new SolidBrush(Color.Black), startX+100, startY + Offset);
                Offset = Offset + 20;
                underLine = "-------------------------------------------------";
                graphics.DrawString(underLine, new Font("Courier New", 8),
                         new SolidBrush(Color.Black), startX, startY + Offset);
                Offset = Offset + 20;
                graphics.DrawString("----You Were Served By : " + staffMemberLable.Text + "-------", new Font("Courier New", 8, FontStyle.Bold),
                         new SolidBrush(Color.Black), startX, startY + Offset);
                Offset = Offset + 20;
                graphics.DrawString("~~~~~THANKYOU FOR SHOPPING HERE~~~~~~~~~", new Font("Courier New", 8, FontStyle.Bold),
                         new SolidBrush(Color.Black), startX, startY + Offset);
                Offset = Offset + 20;
                graphics.DrawString(underLine, new Font("Courier New", 8),
                         new SolidBrush(Color.Black), startX, startY + Offset);
                Offset = Offset + 20;
                graphics.DrawString("*****SOFTWARE MADE BY INFINITY DEVS*****", new Font("Arial", 8, FontStyle.Bold),
                         new SolidBrush(Color.Black), startX, startY + Offset);
                Offset = Offset + 20;
                graphics.DrawString("----For Software Contact : 0334-9847374----", new Font("Arial",9, FontStyle.Bold),
                         new SolidBrush(Color.Black), startX, startY + Offset);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        /// <summary>
        /// Form Functions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            Init();
        }
        private void addStockCostPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
           // singlePointInput(sender, e);
        }

        private void addStockRetailPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            singlePointInput(sender, e);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                makeSizeGrid(sender, e, radioButton1.Text);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
                makeSizeGrid(sender, e, radioButton2.Text);
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
                makeSizeGrid(sender, e, radioButton4.Text);
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked)
                makeSizeGrid(sender, e, radioButton5.Text);
        }

        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton11.Checked)
                makeSizeGrid(sender, e, radioButton11.Text);
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton6.Checked)
                makeSizeGrid(sender, e, radioButton6.Text);
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton7.Checked)
                makeSizeGrid(sender, e, radioButton7.Text);
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton8.Checked)
                makeSizeGrid(sender, e, radioButton8.Text);
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton9.Checked)
                makeSizeGrid(sender, e, radioButton9.Text);
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton10.Checked)
                makeSizeGrid(sender, e, radioButton10.Text);
        }

        private void radioButton12_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton12.Checked)
                makeSizeGrid(sender, e, radioButton12.Text);
        }

        private void radioButton13_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton13.Checked)
                makeSizeGrid(sender, e, radioButton13.Text);
        }

        private void radioButton14_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton14.Checked)
                makeSizeGrid(sender, e, radioButton14.Text);
        }

        private void radioButton15_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton15.Checked)
                makeSizeGrid(sender, e, radioButton15.Text);
        }

        private void addStockArticle_TextChanged(object sender, EventArgs e)
        {
            this.addStockArticleLable.Text = addStockArticle.Text;
            string art = this.addStockArticle.Text;
            addStockCompany.Text = getCompany(art, addStockCompany);
        }

        private void ViewStock_Click(object sender, EventArgs e)
        {
            this.viewStockDownPanel.Visible = true;
            this.viewStockUpPanel.Visible = true;
            hidePanels(splitContainer2.Panel1,viewStockUpPanel);
            hidePanels(splitContainer2.Panel2,viewStockDownPanel);
            viewStockInGridView("All");
            viewStockFillVlaues();
        }

        private void AddStock_Click(object sender, EventArgs e)
        {
            this.addStockUpPanel.Visible = true;
            this.addStockDownPanel.Visible = true;
            hidePanels(splitContainer2.Panel1, addStockUpPanel);
            hidePanels(splitContainer2.Panel2, addStockDownPanel);
        }

        private void Sale_Click(object sender, EventArgs e)
        {
            this.viewSaleDownPanel.Visible = true;
            this.viewSaleUpPanel.Visible = true;
            allSaleViewBtn_Click(sender, e);
            hidePanels(splitContainer2.Panel1, viewSaleUpPanel);
            hidePanels(splitContainer2.Panel2, viewSaleDownPanel);
            
        }

        private void Daily_Click(object sender, EventArgs e)
        {
            //this.orderno.Text = string.Format("{0:yyMMdd}", DateTime.Now);
            Sale s = new Sale();
            this.orderno.Text = s.getNextOrder();
            saleDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            this.dailyDownPanel.Visible = true;
            this.dailyUpPanel.Visible = true;
            hidePanels(splitContainer2.Panel1, dailyUpPanel);
            hidePanels(splitContainer2.Panel2, dailyDownPanel);

            this.saleArticle.Focus();
        }
        private void Staff_Click(object sender, EventArgs e)
        {
            viewEmployeeGridView();
            this.nameTextBox.Focus();
            this.staffDownPanel.Visible = true;
            this.staffUpPanel.Visible = true;
            hidePanels(splitContainer2.Panel1, staffUpPanel);
            hidePanels(splitContainer2.Panel2, staffDownPanel);
        }

        private void viewEmployeeGridView()
        {
            try
            {
                empGridView.ReadOnly = true;
                empGridView.AllowUserToAddRows = false;
                empGridView.DataSource = (new Employee()).getGridViewOfEmployee().Tables[0];
                empGridView.Focus();
                staffCodeTextBox.Text = (new Employee()).getLatestID().ToString();
                staffCodeTextBox.ReadOnly = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void saleAddButton_Click(object sender, EventArgs e)
        {
            availableSizesOfCurrentArticleGirdView.Visible = false;
            addSaleToGridView(sender, e);
            saleDiscount.Enabled = false;
        }

        private void saleDiscount_KeyPress(object sender, KeyPressEventArgs e)
        {
            //singlePointInput(sender, e);
        }

        private void empAddButton_Click(object sender, EventArgs e)
        {
            this.addEmployeeToGridView(sender, e);
            this.Staff_Click(sender,e);
        }

        private void Home_Click(object sender, EventArgs e)
        {
            this.homeDownPanel.Visible = true;
            this.homeUpPanel.Visible = true;
            hidePanels(splitContainer2.Panel1, homeUpPanel);
            hidePanels(splitContainer2.Panel2, homeDownPanel);
        }

        private void addStockButton_Click(object sender, EventArgs e)
        {
            addStockToGridView();
            //addStock(sender, e);
            this.addStockArticle.Focus();
        }

        private void addStockCostPrice_Enter(object sender, EventArgs e)
        {
            try
            {
                Stock stock = new Stock();
                stock.Article = (addStockArticle.Text);
                if (stock.isAlreadyInDB())
                {
                    this.addStockCostPrice.Text = stock.CostPrice.ToString();
                    this.addStockRetailPrice.Text = stock.RetailPrice.ToString();
                    this.groupSizeRange.Enabled = false;
                    makeSizeGrid(sender, e, stock.SizeRange);
                    this.sizeGrid.Focus();
                }
                else
                {
                    this.groupSizeRange.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void addStockRetailPrice_Leave_1(object sender, EventArgs e)
        {
            if (addStockCostPrice.Text.Contains('%'))
            {
                addStockCostPrice.Text = addStockCostPrice.Text.Replace("%", string.Empty);
                //MessageBox.Show(saleDiscount.Text);
                addStockCostPrice.Text = (Convert.ToInt32(addStockRetailPrice.Text) - (Convert.ToDouble(addStockRetailPrice.Text) * (Convert.ToDouble(addStockCostPrice.Text) / 100.00))).ToString();
            }

            if (groupSizeRange.Enabled==true)
            {
                radioButton1.Focus();
                groupBox20.Enabled = false;
            }
            else
                sizeGrid.Focus();
        }

        private void viewStockGridView_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (viewStockGridView.SelectedCells.Count > 0)
                {
                    int selectedrowindex = viewStockGridView.SelectedCells[0].RowIndex;
                    DataGridViewRow selectedRow = viewStockGridView.Rows[selectedrowindex];
                    string a = Convert.ToString(selectedRow.Cells["Article"].Value);
                    this.selectedArticle.Text = a;
                    viewArticleInfoInGridView(a);
                    this.viewArticleInfoGridView.Columns[0].Width = 91;
                    this.viewArticleInfoGridView.Columns[1].Width = 100;
                    this.viewArticleInfoGridView.RowHeadersVisible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        //viewStockAllButton
        private void button5_Click(object sender, EventArgs e)
        {
            viewStockInGridView(button5.Text);
            this.chartPanel.Visible = false;
            this.viewStockDownPanel.Visible = true;
        }
        //viewStockBataButton
        private void button6_Click(object sender, EventArgs e)
        {
            viewStockInGridView(button6.Text);
            this.chartPanel.Visible = false;
            this.viewStockDownPanel.Visible = true;
        }
        //viewStockServisButton
        private void button7_Click(object sender, EventArgs e)
        {
            viewStockInGridView(button7.Text);
            this.chartPanel.Visible = false;
            this.viewStockDownPanel.Visible = true;
        }
        //viewStockXaraButton
        private void button8_Click(object sender, EventArgs e)
        {
            viewStockInGridView(button8.Text);
            this.chartPanel.Visible = false;
            this.viewStockDownPanel.Visible = true;
        }
        //viewStockLocalButton
        private void button9_Click(object sender, EventArgs e)
        {
            viewStockInGridView(button9.Text);
            this.chartPanel.Visible = false;
            this.viewStockDownPanel.Visible = true;
        }
        //viewStockChartButton
        private void button1_Click(object sender, EventArgs e)
        {
            chart1.Series["Pairs"].Points.Clear();
            chart2.Series["Amount"].Points.Clear();
            viewStockFillCharts();
            this.chartPanel.Visible = true;
            this.viewStockDownPanel.Visible = false;
        }

        private void amountTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            singlePointInput(sender, e);
        }

        private void saleSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            singlePointInput(sender, e);
        }

        private void salePairs_KeyPress(object sender, KeyPressEventArgs e)
        {
            singlePointInput(sender, e);
        }

        private void salePrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            singlePointInput(sender, e);
        }

        private void saleStaff_KeyPress(object sender, KeyPressEventArgs e)
        {
            singlePointInput(sender, e);
        }

        private void saleSize_Enter(object sender, EventArgs e)
        {
            try
            {
                //checkIfArticleExist
                Stock S = new Stock();
                S.Article = (saleArticle.Text);
                if (S.isAlreadyInDB())
                {
                    salePrice.Text = S.RetailPrice.ToString();
                    leaveSaleTextFucntion();
                }
                else
                {
                    saleArticle.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void leaveSaleTextFucntion()
        {
            try
            {

                availableSizesOfCurrentArticleGirdView.Rows.Clear();
                //temp.Rows.Clear();
                availableSizesOfCurrentArticleGirdView.RowHeadersVisible = false;
                temp.DataSource = (new Stock()).getGridViewOfArticle(saleArticle.Text).Tables[0];

                //MessageBox.Show(temp.Rows[0].Cells[0].Value.ToString());
                int min = Convert.ToInt32(temp.Rows[0].Cells[0].Value.ToString());
                int max = Convert.ToInt32(temp.Rows[temp.RowCount - 2].Cells[0].Value.ToString());
                int size = max - min + 1;
                availableSizesOfCurrentArticleGirdView.ColumnCount = size;
                for (int i = min, j = 0; i <= max; i++, j++)
                {
                    availableSizesOfCurrentArticleGirdView.Columns[j].Name = i.ToString();
                }
                string[] row = new string[size];
                int sum = 0;
                for (int i = 0; i < size; i++)
                {
                    //row[i] -= getPairsInSaleGrid(saleArticle.Text);

                    row[i] = temp.Rows[i].Cells[1].Value.ToString();
                    row[i] = (Convert.ToInt32(row[i]) - getPairsInSaleGrid(saleArticle.Text, availableSizesOfCurrentArticleGirdView.Columns[i].Name.ToString())).ToString();
                    sum += Convert.ToInt32(row[i]);
                    //sum -= getPairsInSaleGrid(saleArticle.Text, row[i]);
                }
                availableStockTextBox.Text = sum.ToString();
                availableSizesOfCurrentArticleGirdView.Rows.Add(row);
                availableSizesOfCurrentArticleGirdView.Visible = true;
            }
            catch (Exception ex)
            {
                //    MessageBox.Show(ex.Message.ToString());
             //   saleArticle.Focus();
                availableSizesOfCurrentArticleGirdView.Visible = false;
            }
        }

        private void amountTextBox_Enter(object sender, EventArgs e)
        {
            if (orderGridView.Rows.Count < 1)
            {
                amountTextBox.Enabled = false;
                saleArticle.Focus();
            }
        }
        private void saleArticle_TextChanged(object sender, EventArgs e)
        {
            string art = this.saleArticle.Text;
            Stock s = new Stock();
            s.Article = art;
            if(s.isAlreadyInDB())
            {
                dailyCompanyTextBox.Text = s.Company;
            }
        }

        private void allSaleViewBtn_Click(object sender, EventArgs e)
        {
            viewSaleInGridView(allSaleViewBtn.Text,changeDateForamt(fromDateBox.Text),changeDateForamt(toDateBox.Text));
            viewSaleFillVlaues(changeDateForamt(fromDateBox.Text), changeDateForamt(toDateBox.Text));
        }

        private void viewSaleInGridView(string type,string fromDate,string toDate)
        {
            viewSaleGridView.ReadOnly = true;
            viewSaleGridView.AllowUserToAddRows = false;
            if(type=="All")
            {
                specificSaleViewGroupBox.Visible = false;
                viewSaleGridView.DataSource = (new Sale()).getGridViewOfSale(type, fromDate, toDate).Tables[0];
                viewSaleGridView.Focus();
            }
            else
            {
                specificSaleViewGroupBox.Visible = true;
                specificSaleViewGrid.DataSource = (new Sale()).getGridViewOfSale(type, fromDate, toDate).Tables[0];
                specificSaleViewGrid.Focus();
            }
        }
        void viewOrderInfoInGridView(string orderID)
        {
            viewOrderInfoGridView.ReadOnly = true;
            viewOrderInfoGridView.AllowUserToAddRows = false;
            viewOrderInfoGridView.DataSource = (new Sale()).getGridViewOfOrder(orderID).Tables[0];
        }

        private void viewSaleGridView_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (viewSaleGridView.SelectedCells.Count > 0)
                {
                    int selectedrowindex = viewSaleGridView.SelectedCells[0].RowIndex;
                    DataGridViewRow selectedRow = viewSaleGridView.Rows[selectedrowindex];
                    string a = Convert.ToString(selectedRow.Cells["Order#"].Value);
                    this.selectedOrder.Text = a;
                    viewOrderInfoInGridView(a);
                    //this.viewArticleInfoGridView.Columns[0].Width = 91;
                    //this.viewArticleInfoGridView.Columns[1].Width = 100;
                    this.viewOrderInfoGridView.RowHeadersVisible = false;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        private void bataSaleViewBtn_Click(object sender, EventArgs e)
        {
            viewSaleInGridView(bataSaleViewBtn.Text, changeDateForamt(fromDateBox.Text), changeDateForamt(toDateBox.Text));
            viewSaleFillVlaues(changeDateForamt(fromDateBox.Text), changeDateForamt(toDateBox.Text));
        }

        private void servisSaleViewBtn_Click(object sender, EventArgs e)
        {
            viewSaleInGridView(servisSaleViewBtn.Text, changeDateForamt(fromDateBox.Text), changeDateForamt(toDateBox.Text));
            viewSaleFillVlaues(changeDateForamt(fromDateBox.Text), changeDateForamt(toDateBox.Text));
        }

        private void xaraSaleViewBtn_Click(object sender, EventArgs e)
        {
            viewSaleInGridView(xaraSaleViewBtn.Text, changeDateForamt(fromDateBox.Text), changeDateForamt(toDateBox.Text));
            viewSaleFillVlaues(changeDateForamt(fromDateBox.Text), changeDateForamt(toDateBox.Text));
        }

        private void localSaleViewBtn_Click(object sender, EventArgs e)
        {
            viewSaleInGridView(localSaleViewBtn.Text, changeDateForamt(fromDateBox.Text), changeDateForamt(toDateBox.Text));
            viewSaleFillVlaues(changeDateForamt(fromDateBox.Text), changeDateForamt(toDateBox.Text));
        }

        private void empGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (viewSaleGridView.SelectedCells.Count > 0)
            {
                //int selectedrowindex = viewSaleGridView.SelectedCells[0].RowIndex;
                //DataGridViewRow selectedRow = viewSaleGridView.Rows[selectedrowindex];
                //string a = Convert.ToString(selectedRow.Cells["ID"].Value);
                //this.selectedOrder.Text = a;
                //viewOrderInfoInGridView(a);
                //this.viewArticleInfoGridView.Columns[0].Width = 91;
                //this.viewArticleInfoGridView.Columns[1].Width = 100;
                //this.viewOrderInfoGridView.RowHeadersVisible = false;

            }
        }
        
        private void addStockArticle_Enter(object sender, EventArgs e)
        {
            this.groupSizeRange.Enabled = false;
        }

        private void addStockParty_Leave(object sender, EventArgs e)
        {
            if(addStockInvoiceNo.Text==null || addStockInvoiceNo.Text.Length <4)
            {
                MessageBox.Show("Enter a valid Invoice NO.");
                this.addStockInvoiceNo.Focus();
            }
            else
            {
                if (addStockParty.Text == null || addStockParty.Text.Length < 3)
                {
                    MessageBox.Show("Enter a valid Party Name");
                    this.addStockParty.Focus();
                }
                else
                {
                    groupArticleInfo.Enabled = true;
                    this.addStockArticle.Focus();
                    groupSizeRange.Enabled = false;
                    groupStockInfo.Enabled = false;
                    addingStock = new List<Stock>();
                }
            }
        }

        private void StockHistory_Click(object sender, EventArgs e)
        {
            this.viewStockHistoryDownPanel.Visible = true;
            this.viewStockHistoryUpPanel.Visible = true;
            hidePanels(splitContainer2.Panel1, viewStockHistoryUpPanel);
            hidePanels(splitContainer2.Panel2, viewStockHistoryDownPanel);
            viewStockHistory();
        }

        private void viewStockHistory()
        {
            stockHistoryGridView.DataSource = (new StockHistory()).getGridViewOfStockHistory().Tables[0];
            viewStockHistoryFillVlaues();
        }

        private void byCompanyStockHistoryButton_Click(object sender, EventArgs e)
        {
            stockHistoryGridView.DataSource = (new StockHistory()).getGridViewOfStockHistoryDetailsBy("Company").Tables[0];
        }

        private void byPartyStockHistoryButton_Click(object sender, EventArgs e)
        {
            stockHistoryGridView.DataSource = (new StockHistory()).getGridViewOfStockHistoryBy("Party").Tables[0];
        }

        private void byArticleStockHistoryButton_Click(object sender, EventArgs e)
        {
            stockHistoryGridView.DataSource = (new StockHistory()).getGridViewOfStockHistoryDetailsBy("Article").Tables[0];
        }

        private void allStockHistoryButton_Click(object sender, EventArgs e)
        {
            stockHistoryGridView.DataSource = (new StockHistory()).getGridViewOfStockHistory().Tables[0];
            viewStockHistoryFillVlaues();
        }

        private void saleArticle_Leave(object sender, EventArgs e)
        {
            
        }
        private int getPairsInSaleGrid(string article)
        {
            int t = 0;
            for (int rows = 0; rows < orderGridView.Rows.Count; rows++)
            {
                if (orderGridView.Rows[rows].Cells[0].Value.ToString() == article)
                {
                    t+= Convert.ToInt32(orderGridView.Rows[rows].Cells[3].Value.ToString());
                }
            }
            return t;
        }
        private int getPairsInSaleGrid(string article,string size)
        {
            int t = 0;
            for (int rows = 0; rows < orderGridView.Rows.Count; rows++)
            {
                    if(orderGridView.Rows[rows].Cells[0].Value.ToString()==article && orderGridView.Rows[rows].Cells[2].Value.ToString() == size)
                {
                    t+= Convert.ToInt32(orderGridView.Rows[rows].Cells[3].Value.ToString());
                }
            }
            return t;
        }

        private void saleSize_Leave(object sender, EventArgs e)
        {
            //salePrice.Enabled = true;
            //salePrice.ReadOnly = false;
            //salePrice.Focus();
            try
            {
                if (saleSize.Text.Length < 1)
                {
                    MessageBox.Show("Enter A Valid Size");
                }
                else
                {
                    Stock s = new Stock();
                    s.Article = saleArticle.Text;
                    s.isAlreadyInDB();
                    string[] r = s.SizeRange.Split('-');
                    if (Convert.ToInt32(saleSize.Text) >= Convert.ToInt32(r[0]) && Convert.ToInt32(saleSize.Text) <= Convert.ToInt32(r[1]))
                    {
                        availableStockTextBox.Text = s.getPairsOfArtileBySize(Convert.ToInt32(saleSize.Text)).ToString();
                    }
                    else
                    {
                        MessageBox.Show("Invalid Size For Article : " + saleArticle.Text);
                        saleSize.Focus();
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void saleArticle_Enter(object sender, EventArgs e)
        {
            if(saleArticle.Text.Contains(" "))
            saleArticle.Text = "";
        }

        private void saleStaff_DragLeave(object sender, EventArgs e)
        {
            try
            {
                staffMemberLable.Text = new Employee().getNameBy(saleStaff.Text);
            }
            catch(Exception ex)
            {
                saleStaff.Focus();
            }
        }

        private void saleStaff_TextChanged(object sender, EventArgs e)
        {
            try
            {
                staffMemberLable.Text = new Employee().getNameBy(saleStaff.Text);
            }
            catch (Exception ex)
            {
                saleStaff.Focus();
            }
        }

        private void Backup_Click(object sender, EventArgs e)
        {
            try
            {
                string path = null;
                if(File.Exists("BackupPath.txt"))
                {
                    StreamReader file =
                     new StreamReader("BackupPath.txt");
                    path = file.ReadLine();
                    if (!Directory.Exists(path))
                        path = null;
                    file.Close();
                }
                if(path==null)
                {
                    using (var fbd = new FolderBrowserDialog())
                    {
                        DialogResult result = fbd.ShowDialog();
                        if (result == DialogResult.OK)
                        {
                            using (StreamWriter writetext = new StreamWriter("BackupPath.txt"))
                            {
                                writetext.WriteLine(fbd.SelectedPath);
                                path = fbd.SelectedPath;
                            }
                        }
                    }
                }
                if(path!=null)
                {
                    Connection backup = new Connection();
                    var databaseName = "POS";
                    //MessageBox.Show(path);
                    path = @"E:\POS Backup";
                    string saveFileName = DateTime.Now.ToString("dd-MM-yyyy") + "-POS-BACKUP";
                    backup.ExecuteQuery("backup database " + databaseName + " to disk = '" + path + "\\" + saveFileName + ".Bak' WITH INIT");
                    MessageBox.Show("Backup Done Successfully!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Backup Failed Due To : " + ex.Message.ToString());
            }
        }
        private void companySaleViewBtn_Click(object sender, EventArgs e)
        {
            viewSaleInSpecificGridViewBy(companySaleViewBtn.Text, changeDateForamt(fromDateBox.Text), changeDateForamt(toDateBox.Text));
        }

        private void viewSaleInSpecificGridViewBy(string type,string fromDate,string toDate)
        {
            specificSaleViewGroupBox.Visible = true;
            specificSaleViewGrid.DataSource = (new Sale()).getGridViewOfSaleBy(type, fromDate, toDate).Tables[0];
            specificSaleViewGrid.Focus();
        }

        private void articleSaleViewBtn_Click(object sender, EventArgs e)
        {
            viewSaleInSpecificGridViewBy("Article", changeDateForamt(fromDateBox.Text), changeDateForamt(toDateBox.Text));
        }

        private void staffSaleViewBtn_Click(object sender, EventArgs e)
        {
            viewSaleInSpecificGridViewBy("Staff", changeDateForamt(fromDateBox.Text), changeDateForamt(toDateBox.Text));
        }

        private void allSaleAmount_TextChanged(object sender, EventArgs e)
        {

        }

        private void MonthSale_Click(object sender, EventArgs e)
        {
            this.monthSaleDownPanel.Visible = true;
            this.monthSaleUpPanel.Visible = true;
            hidePanels(splitContainer2.Panel1, monthSaleUpPanel);
            hidePanels(splitContainer2.Panel2, monthSaleDownPanel);
            viewMonthSale();
        }

        private void viewMonthSale()
        {
            try
            {
                monthSaleGridView.DataSource = (new Sale()).getGridViewOfSaleBy((DateTime.Now.ToString("yyyy-MM-dd"))).Tables[0];
                int tpairs = 0;
                double tamt = 0.0;
                double tdisc = 0.0;
                double tamnt = 0.0;
                for (int rows = 0; rows < monthSaleGridView.Rows.Count; rows++)
                {
                    tpairs += Convert.ToInt32(monthSaleGridView.Rows[rows].Cells[1].Value.ToString());
                    tamt += Convert.ToDouble(monthSaleGridView.Rows[rows].Cells[2].Value.ToString());
                    tdisc += Convert.ToDouble(monthSaleGridView.Rows[rows].Cells[3].Value.ToString());
                    tamnt += Convert.ToDouble(monthSaleGridView.Rows[rows].Cells[4].Value.ToString());
                }
                string[] t = { DateTime.Now.ToString(), tpairs.ToString(), string.Format("{0:0.00}", tamt), string.Format("{0:0.00}", tdisc), string.Format("{0:0.00}", tamnt) };
                DataTable dt = monthSaleGridView.DataSource as DataTable;
                dt.Rows.Add(t);
                monthSaleGridView.DataSource = dt;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void specificArticleTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Stock temp1 = new Stock();
                temp1.Article = specificArticleTextBox.Text;
                if (temp1.isAlreadyInDB())
                {
                    updateArticleInfo.Enabled = true;
                    editArticleInfo.Enabled = true;
                    currentPairsInStockTextBox.Text = temp1.TotalPairs.ToString();
                    currentCostPriceTextBox.Text = temp1.CostPrice.ToString();
                    currentRetailPriceTextBox.Text = temp1.RetailPrice.ToString();
                    pairsSoldInMonthTextBox.Text = (new Sale()).getArticleSoldInMonth(temp1.Article, changeDateForamt(DateTime.Now.ToString("dd/MM/yyyy"))).ToString();
                    pairsRefundedInMonthsTextBox.Text = (new Sale()).getArtilceRefundInMonth(temp1.Article, changeDateForamt(DateTime.Now.ToString("dd/MM/yyyy"))).ToString();
                    availableSizesOfCurrentArticleGirdView.Rows.Clear();
                    //  temp.Rows.Clear();
                    articleSizeInfoGridView.RowHeadersVisible = false;
                    temp.DataSource = (new Stock()).getGridViewOfArticle(temp1.Article).Tables[0];

                    //MessageBox.Show(temp.Rows[0].Cells[0].Value.ToString());
                    int min = Convert.ToInt32(temp.Rows[0].Cells[0].Value.ToString());
                    int max = Convert.ToInt32(temp.Rows[temp.RowCount - 2].Cells[0].Value.ToString());
                    int size = max - min + 1;
                    articleSizeInfoGridView.ColumnCount = size;
                    for (int i = min, j = 0; i <= max; i++, j++)
                    {
                        articleSizeInfoGridView.Columns[j].Name = i.ToString();
                    }
                    string[] row = new string[size];
                    int sum = 0;
                    for (int i = 0; i < size; i++)
                    {
                        //row[i] -= getPairsInSaleGrid(saleArticle.Text);

                        row[i] = temp.Rows[i].Cells[1].Value.ToString();
                        row[i] = (Convert.ToInt32(row[i]) - getPairsInSaleGrid(saleArticle.Text, articleSizeInfoGridView.Columns[i].Name.ToString())).ToString();
                        sum += Convert.ToInt32(row[i]);
                        //sum -= getPairsInSaleGrid(saleArticle.Text, row[i]);
                    }
                    articleSizeInfoGridView.Rows.Add(row);
                    articleSizeInfoGridView.Visible = true;
                }
                else
                {
                    currentPairsInStockTextBox.Text = "0";
                    currentCostPriceTextBox.Text = "0";
                    currentRetailPriceTextBox.Text = "0";
                    pairsSoldInMonthTextBox.Text = "0";
                    pairsRefundedInMonthsTextBox.Text = "0";
                    articleSizeInfoGridView.Rows.Clear();
                    articleSizeInfoGridView.Visible = false;
                    updateArticleInfo.Enabled = false;
                    editArticleInfo.Enabled = false;
                    currentCostPriceTextBox.ReadOnly = true;
                    currentRetailPriceTextBox.ReadOnly = true;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void CheckArticle_Click(object sender, EventArgs e)
        {
            this.articleDownPanel.Visible = true;
            this.articleUpPanel.Visible = true;
            hidePanels(splitContainer2.Panel1, articleUpPanel);
            hidePanels(splitContainer2.Panel2, articleDownPanel);
            specificArticleTextBox.Focus();
        }

        private void ArticleInfo_Click(object sender, EventArgs e)
        {
            bestSlowDownPanel.Visible = true;
            bestSlowUpPanel.Visible = true;
            hidePanels(splitContainer2.Panel1, bestSlowUpPanel);
            hidePanels(splitContainer2.Panel2, bestSlowDownPanel);
            fillBestSlowGridViews();
        }

        private void fillBestSlowGridViews()
        {
            try
            {
                bestSellersGridView.DataSource = (new Sale()).getGridViewOfArticleSaleBy("best").Tables[0];
                slowMoverGridView.DataSource = (new Sale()).getGridViewOfArticleSaleBy("slow").Tables[0];
                deadArticleGridView.DataSource = (new Sale()).getGridViewOfArticleSaleBy("dead").Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                Daily_Click(sender, e);
            }
            if (e.KeyCode == Keys.F3)
            {
                Sale_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F4)
            {
                MonthSale_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F5)
            {
                ViewStock_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F11)
            {
                Backup_Click(sender, e);
            }
            if (!logedtype)
                return;

            if (e.KeyCode == Keys.F1)
            {
                Home_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F2)
            {
                Daily_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F3)
            {
                Sale_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F4)
            {
                MonthSale_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F5)
            {
                ViewStock_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F6)
            {
                AddStock_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F7)
            {
                Staff_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F8)
            {
                StockHistory_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F9)
            {
                CheckArticle_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F10)
            {
           //     Export.Stock();
                ArticleInfo_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F11)
            {
                Backup_Click(sender, e);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{F2}");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{F3}");
        }

        private void button12_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{F4}");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{F5}");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{F6}");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{F7}");
        }

        private void button13_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{F8}");
        }

        private void button17_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{F9}");
        }

        private void button16_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{F10}");
        }

        private void button15_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{F11}");
        }

        private void button14_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{Esc}");
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
                makeSizeGrid(sender, e, radioButton3.Text);
        }

        private void radioButton16_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton16.Checked)
                makeSizeGrid(sender, e, radioButton16.Text);
        }

        private void button18_Click(object sender, EventArgs e)
        {
            try
            {
                int min = 0;
                int max = 0;
                if(Int32.TryParse(textBox1.Text,out min) && (Int32.TryParse(textBox2.Text, out max)))
                {
                    if(min<=max) 
                    {
                        makeSizeGrid(sender, e, textBox1.Text + "-" + textBox2.Text);
                    }
                    else
                    {
                        MessageBox.Show("Minimum Must Be Less Than Maxmimum");
                    }
                }
                else
                {
                    MessageBox.Show("Enter Corrent Size Range");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void editArticleInfo_Click(object sender, EventArgs e)
        {
            currentCostPriceTextBox.ReadOnly = false;
            currentRetailPriceTextBox.ReadOnly = false;
            articleSizeInfoGridView.ReadOnly = false;
            
            currentCostPriceTextBox.Focus();
        }

        private void updateArticleInfo_Click(object sender, EventArgs e)
        {
            try
            {
                Stock S = new Stock();
                S.Article = specificArticleTextBox.Text;
                if(S.isAlreadyInDB())
                {
                    S.CostPrice = Convert.ToDouble(currentCostPriceTextBox.Text);
                    S.RetailPrice = Convert.ToDouble(currentRetailPriceTextBox.Text);
                    
                    int min = Convert.ToInt32(articleSizeInfoGridView.Columns[0].Name);
                    int max = Convert.ToInt32(articleSizeInfoGridView.Columns[articleSizeInfoGridView.ColumnCount - 1].Name);
                    SizeInformation[] sz = new SizeInformation[max - min + 1];
                    for (int i = min, j = 0; i <= max; i++, j++)
                    {
                        sz[j].size = Convert.ToInt32(articleSizeInfoGridView.Columns[j].Name.ToString());
                        sz[j].pairs = Convert.ToInt32(articleSizeInfoGridView.Rows[0].Cells[j].Value.ToString());

                    }
                    S.Sizes = sz;
                    if (S.UpdateArticleInfo())
                    {
                        MessageBox.Show("Updated Successfully");
                        currentCostPriceTextBox.ReadOnly = true;
                        currentRetailPriceTextBox.ReadOnly = true;
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void PriceChange_Click(object sender, EventArgs e)
        {
            priceDetailsDownPanel.Visible = true;
            priceDetailsUpPanel.Visible = true;
            hidePanels(splitContainer2.Panel1, priceDetailsUpPanel);
            hidePanels(splitContainer2.Panel2, priceDetailsDownPanel);
            fillPriceDetails();
        }

        private void fillPriceDetails()
        {
            try
            {
                priceChangeGridView.DataSource = (new Stock()).getGridViewOfStockPriceDetails().Tables[0];
                int tcpairs = 0;
                int invpairs = 0;
                double currcost = 0.0;
                double invcost = 0.0;
                double currret = 0.0;
                double invret = 0.0;
                double tcurrcost = 0.0;
                double tinvcost = 0.0;
                double tcurrret = 0.0;
                double tinvret = 0.0;
                for (int rows = 0; rows < priceChangeGridView.Rows.Count; rows++)
                {
                    tcpairs += Convert.ToInt32(priceChangeGridView.Rows[rows].Cells[1].Value.ToString());
                    invpairs += Convert.ToInt32(priceChangeGridView.Rows[rows].Cells[2].Value.ToString());
                    currcost += Convert.ToDouble(priceChangeGridView.Rows[rows].Cells[3].Value.ToString());
                    invcost += Convert.ToDouble(priceChangeGridView.Rows[rows].Cells[4].Value.ToString());
                    currret += Convert.ToDouble(priceChangeGridView.Rows[rows].Cells[5].Value.ToString());
                    invret += Convert.ToDouble(priceChangeGridView.Rows[rows].Cells[6].Value.ToString());
                    tcurrcost += Convert.ToDouble(priceChangeGridView.Rows[rows].Cells[7].Value.ToString());
                    tinvcost += Convert.ToDouble(priceChangeGridView.Rows[rows].Cells[8].Value.ToString());
                    tcurrret += Convert.ToDouble(priceChangeGridView.Rows[rows].Cells[9].Value.ToString());
                    tinvret += Convert.ToDouble(priceChangeGridView.Rows[rows].Cells[10].Value.ToString());
                }
                string[] t = { "Total", tcpairs.ToString(), invpairs.ToString(), string.Format("{0:0.00}", currcost),
                              string.Format("{0:0.00}", invcost),string.Format("{0:0.00}", currret),string.Format("{0:0.00}", invret),
                              string.Format("{0:0.00}", tcurrcost),string.Format("{0:0.00}", tinvcost),
                              string.Format("{0:0.00}", tcurrret),string.Format("{0:0.00}", tinvret)};
                DataTable dt = priceChangeGridView.DataSource as DataTable;
                dt.Rows.Add(t);
                priceChangeGridView.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            try
            {
                //Export.Stock();
                Export.Sale();
                MessageBox.Show("Exported!");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            try
            {
                Export.Stock();
                MessageBox.Show("Exported!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void saleStaff_Leave(object sender, EventArgs e)
        {
            //saleAddButton_Click(sender, e);
        }

        private void button24_Click(object sender, EventArgs e)
        {
            homeUpPanel.Visible = true;
            passwordsPanel.Visible = true;
            hidePanels(splitContainer2.Panel1, homeUpPanel);
            hidePanels(splitContainer2.Panel2, passwordsPanel);
        }

        private void button29_Click(object sender, EventArgs e)
        {
            if(new login().check("MGR",mgrOldPasswordTextBox.Text))
            {
                if(mgrNewPasswordTextBox.Text==mgrConfirmPasswordTextBox.Text)
                {
                    if(new login().UpdatePassword("MGR",mgrNewPasswordTextBox.Text))
                    {
                        MessageBox.Show("Password Updated!");
                        clearTextBoxes(groupBox22);
                    }
                    else
                    {
                        MessageBox.Show("Error Occured!");
                    }
                }
                else
                {
                    MessageBox.Show("Password Doesn't Match!");
                }
            }
            else
            {
                MessageBox.Show("Wrong Old Password");
            }
        }

        private void button30_Click(object sender, EventArgs e)
        {
            if (new login().check("EMP", empOldPasswordTextBox.Text))
            {
                if (empNewPasswordTextBox.Text == empConfirmPasswordTextBox.Text)
                {
                    if (new login().UpdatePassword("EMP", empNewPasswordTextBox.Text))
                    {
                        MessageBox.Show("Password Updated!");
                        clearTextBoxes(groupBox23);
                    }
                    else
                    {
                        MessageBox.Show("Error Occured!");
                    }
                }
                else
                {
                    MessageBox.Show("Password Doesn't Match!");
                }
            }
            else
            {
                MessageBox.Show("Wrong Old Password");
            }
        }

        private void EDaily_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{F2}");
        }

        private void ESale_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{F3}");
        }

        private void EStock_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{F5}");
        }

        private void EMonth_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{F4}");
        }

        private void EBackUP_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{F11}");
        }

        private void label84_Click(object sender, EventArgs e)
        {

        }
    }
}