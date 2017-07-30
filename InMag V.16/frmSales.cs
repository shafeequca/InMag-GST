using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InMag_V._16.DataSet;
using CrystalDecisions.CrystalReports.Engine;

namespace InMag_V._16
{
    public partial class frmSalesGST : Form
    {
        DataSet1 ds;
        public frmSalesGST()
        {
            InitializeComponent();
            this.cboArea.SelectionChangeCommitted += new System.EventHandler(this.cboArea_SelectionChangeCommitted);
            this.cboAreaSearch.SelectionChangeCommitted += new System.EventHandler(this.cboAreaSearch_SelectionChangeCommitted);

            this.ItemDisplayGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ItemDisplayGrid_KeyDown);
            this.cboCustomer.SelectionChangeCommitted += new System.EventHandler(this.cboCustomer_SelectionChangeCommitted);
            this.cboArea.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboArea_KeyDown);
            this.cboAreaSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboAreaSearch_KeyDown);
            this.cboCustomer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboCustomer_KeyDown);
            this.txtItemcode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtItemcode_KeyDown);
            this.txtQuantity.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtQuantity_KeyDown);
            this.txtItems.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtItems_KeyDown);
            this.txtRate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtRate_KeyDown);
            this.txtRate.KeyPress += new KeyPressEventHandler(NumberOnly_KeyPress);
            this.txtItems.KeyPress += new KeyPressEventHandler(txtItems_KeyPress);
            this.txtCash.KeyPress += new KeyPressEventHandler(NumberOnly_KeyPress);
            this.txtQuantity.KeyPress += new KeyPressEventHandler(NumberOnly_KeyPress);
            this.txtDiscount.KeyPress += new KeyPressEventHandler(NumberOnly_KeyPress);
            this.txtCBalance.KeyPress += new KeyPressEventHandler(NumberOnly_KeyPress);
            this.contextMenuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.Edit_Click);

            this.cboAreaSearch.KeyPress += new KeyPressEventHandler(Search_KeyPress);
            this.txtBillNoSearch.KeyPress += new KeyPressEventHandler(Search_KeyPress);
            this.cboCustomerSearch.KeyPress += new KeyPressEventHandler(Search_KeyPress);
            this.DtFrom.KeyPress += new KeyPressEventHandler(Search_KeyPress);
            this.DtTo.KeyPress += new KeyPressEventHandler(Search_KeyPress);

        }

        private void Search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SearchGridLoad();
            }
        }
        private void NumberOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (!(e.KeyChar == 8 || e.KeyChar == 46 || e.KeyChar == 13 || (e.KeyChar >= 48 && e.KeyChar <= 57)))
                e.Handled = true;
            else
            {
                if (e.KeyChar == 46 && tb.Text.Contains("."))
                {
                    e.Handled = true;
                }
            }
            if (e.KeyChar == 13)
            {
                if (tb.Name == "txtCash")
                {
                    txtDiscount.SelectionStart = 0;
                    txtDiscount.SelectionLength = txtDiscount.Text.Length;
                    txtDiscount.Focus();
                }
                else if (tb.Name == "txtDiscount")
                    btnSave_Click(null, null);
                else if (tb.Name == "txtItems")
                {
                    txtQuantity.SelectionStart = 0;
                    txtQuantity.SelectionLength = txtQuantity.Text.Length;
                    txtQuantity.Focus();
                }
            }
            else if (e.KeyChar == 27)
            {
                if (tb.Name == "txtDiscount")
                {
                    txtCash.SelectionStart = 0;
                    txtCash.SelectionLength = txtCash.Text.Length;
                }
            }
        }
        private void frmSales_Load(object sender, EventArgs e)
        {
            ds = new DataSet1();
            comboLoad();
            chkWholeSale.Checked = false;
            txtItemcode.Tag = null;
            SetBillNo();
            SearchGridLoad();
            btnClear_Click(null, null);
            if (cboArea.Items.Count > 0)
            {
                cboArea.SelectedValue = 1;
                //cboAreaSearch.Text = "General";
            }
            

        }
        private void SetBillNo()
        {
            string query = "Truncate table tblTemp";
            Connections.Instance.ExecuteQueries(query);
            query = "select billno from tblSettings";
            DataTable dt = (DataTable)Connections.Instance.ShowDataInGridView(query);
            txtBillno.Text = (Convert.ToInt32(dt.Rows[0][0].ToString()) + 1).ToString();
        
        }
        private void SearchGridLoad()
        {
            try
            {
                string chCondition="";
                if (chkCashEntry.Checked)
                    chCondition = "and s.Cash=0 order by c.Customer";
                else
                    chCondition = "order by s.BillNo DESC";

                string condition = "(s.BillDate>='" + DtFrom.Value.ToString("dd-MMM-yyyy") + "' and s.BillDate<='" + DtTo.Value.ToString("dd-MMM-yyyy") + "') " + chCondition;
                if (txtBillNoSearch.Text.Trim() != "")
                    condition = "s.BillNo='" + txtBillNoSearch.Text + "'";
                else if (cboAreaSearch.SelectedIndex != -1 && cboAreaSearch.Text != "" && cboCustomerSearch.SelectedIndex != -1 && cboCustomerSearch.Text !="")
                    condition = "(s.BillDate>='" + DtFrom.Value.ToString("dd-MMM-yyyy") + "' and s.BillDate<='" + DtTo.Value.ToString("dd-MMM-yyyy") + "' and s.areaId='" + cboAreaSearch.SelectedValue + "' and s.custId='" + cboCustomerSearch.SelectedValue + "') " + chCondition;
                else if (cboAreaSearch.SelectedIndex != -1 && cboAreaSearch.Text != "")
                    condition = "(s.BillDate>='" + DtFrom.Value.ToString("dd-MMM-yyyy") + "' and s.BillDate<='" + DtTo.Value.ToString("dd-MMM-yyyy") + "' and s.areaId='" + cboAreaSearch.SelectedValue + "') " + chCondition;
                string query = "select s.saleId,s.BillNo as Bill_No,CONVERT(VARCHAR(11),s.BillDate,106) as Bill_Date,c.Customer,s.areaId,s.custId,s.CBalance,s.GrandTotal as Bill_Amount,s.Cash,s.Discount,s.Balance from tblSales s,tblCustomer c where s.custId=c.Custid  and " + condition;
                SearchGrid.DataSource = Connections.Instance.ShowDataInGridView(query);
                SearchGrid.Columns[0].Visible = false;
                SearchGrid.Columns[1].Width = 60;
                SearchGrid.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                SearchGrid.Columns[2].Width = 90;
                SearchGrid.Columns[3].Width = 200;
                SearchGrid.Columns[4].Visible = false;
                SearchGrid.Columns[5].Visible = false;
                SearchGrid.Columns[6].Visible = false;
                SearchGrid.Columns[7].Width = 88;
                SearchGrid.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                SearchGrid.Columns[8].Visible = false;
                SearchGrid.Columns[9].Visible = false;
                SearchGrid.Columns[10].Visible = false;
            }
            catch { }
        }
        private void comboLoad()
        {
            try
            {
                itemView.Visible = false;
                string query = "select areaId,Area from tblArea order By Area";
                cboArea.DataSource = Connections.Instance.ShowDataInGridView(query);
                cboArea.DisplayMember = "Area";
                cboArea.ValueMember = "areaId";
                cboArea.SelectedIndex = -1;
                cboArea.Text = "";

                cboAreaSearch.DataSource = Connections.Instance.ShowDataInGridView(query);
                cboAreaSearch.DisplayMember = "Area";
                cboAreaSearch.ValueMember = "areaId";
                cboAreaSearch.SelectedIndex = -1;
                cboAreaSearch.Text = "";

                

                //query = "select itemId,Item_Name from tblItem order By Item_Name";
                //cboItems.DataSource = Connections.Instance.ShowDataInGridView(query);
                //cboItems.DisplayMember = "Item_Name";
                //cboItems.ValueMember = "itemId";
                //cboItems.SelectedIndex = -1;
                //cboItems.Text = "";
            }
            catch { }
        }

        private void cboAreaSearch_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cboAreaSearch.SelectedValue != null && cboAreaSearch.Text !="")
            {
                System.Data.DataRowView DR = (System.Data.DataRowView)cboArea.Items[cboAreaSearch.SelectedIndex];
                string query = "select custId,Customer from tblCustomer where areaId='" + DR.Row[0].ToString() + "' order by Customer";
                cboCustomerSearch.DataSource = Connections.Instance.ShowDataInGridView(query);
                cboCustomerSearch.DisplayMember = "Customer";
                cboCustomerSearch.ValueMember = "custId";
                cboCustomerSearch.Text = "";
                cboCustomerSearch.SelectedIndex = -1;
               
            }
        }
        private void cboArea_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cboArea.SelectedValue != null && cboArea.Text !="")
            {
                System.Data.DataRowView DR = (System.Data.DataRowView)cboArea.Items[cboArea.SelectedIndex];
                string query = "select custId,Customer from tblCustomer where areaId='" + DR.Row[0].ToString() + "' order by Customer";
                cboCustomer.DataSource = Connections.Instance.ShowDataInGridView(query);
                cboCustomer.DisplayMember = "Customer";
                cboCustomer.ValueMember = "custId";
                cboCustomer.SelectedIndex = -1;
                cboCustomer.Text = "";
                txtPlace.Text = "";
                txtCBalance.Text = "";
            }
        }
        private void cboCustomer_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cboCustomer.SelectedValue != null && cboCustomer.Text !="")
            {
                System.Data.DataRowView DR = (System.Data.DataRowView)cboCustomer.Items[cboCustomer.SelectedIndex];
                string query = "select Place,creditBal from tblCustomer where custId='" + DR.Row[0].ToString() + "'";
                DataTable dt = (DataTable)Connections.Instance.ShowDataInGridView(query);
                if (dt.Rows.Count > 0)
                {
                    txtPlace.Text = dt.Rows[0][0].ToString();
                    txtCBalance.Text = dt.Rows[0][1].ToString();
                }
            }
        }
        
        
        private void cboAreaSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                cboAreaSearch_SelectionChangeCommitted(null, null);
            }
        }
        
        
        private void cboArea_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            
            {
                cboArea_SelectionChangeCommitted(null, null);
                cboCustomer.Focus();

            }
        }

        private void cboCustomer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                cboCustomer_SelectionChangeCommitted(null, null);
                txtItems.Focus();
            }
        }
        private void txtItemcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                if (txtItemcode.Text.Trim() != "")
                {
                    string query = "select itemId,Rate,PRate from tblItem where item_Code='" + txtItemcode.Text.Trim() + "'";
                    if (chkWholeSale.Checked)
                    {
                        query = "select itemId,WRate,PRate from tblItem where item_Code='" + txtItemcode.Text.Trim() + "'";
                    }
                    DataTable dt = (DataTable)Connections.Instance.ShowDataInGridView(query);
                    if (dt.Rows.Count > 0)
                    {
                        txtItems.Tag = dt.Rows[0][0].ToString();
                        txtRate.Text = dt.Rows[0][1].ToString();
                        lblPRate.Text = dt.Rows[0][2].ToString();
                        txtQuantity.SelectionStart = 0;
                        txtQuantity.SelectionLength = txtQuantity.Text.Length;
                        txtQuantity.Focus();
                    }
                    dt.Dispose();
                }
                else
                    txtItems.Focus();
            }
        }

        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            if (txtQuantity.Text.Trim() == "")
                txtQuantity.Text = "0";
            if (txtRate.Text.Trim() == "")
                txtRate.Text = "0";
            try
            {
                txtTotal.Text = (Convert.ToDouble(txtQuantity.Text) * Convert.ToDouble(txtRate.Text)).ToString();
            }

            catch
            {
                txtTotal.Text = "0";
            }
        }

        private void txtRate_TextChanged(object sender, EventArgs e)
        {
            if (txtRate.Text.Trim() == "")
                txtRate.Text = "0";
            if (txtQuantity.Text.Trim() == "")
                txtQuantity.Text = "0";
            try
            {
                txtTotal.Text = (Convert.ToDouble(txtQuantity.Text) * Convert.ToDouble(txtRate.Text)).ToString();
            }
            catch
            { }
        }
        private void txtQuantity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                txtRate.SelectionStart = 0;
                txtRate.SelectionLength = txtRate.Text.Length;
                txtRate.Focus();
            }
        }
        private void GridShow()
        {
            try
            {
                Type t = typeof(System.Windows.Forms.SystemInformation);
                
                string query = "select ROW_NUMBER() OVER(ORDER BY id) AS Row,itemcode,itemid,itemname,qty,rate,total from tblTemp;";
                ItemGrid.DataSource = Connections.Instance.ShowDataInGridView(query);
                ItemGrid.Columns[0].Width = 35;
                ItemGrid.Columns[1].Width = txtItemcode.Width;//86;
                ItemGrid.Columns[2].Visible = false;
                ItemGrid.Columns[3].Width = txtItems.Width - 1;//203;
                ItemGrid.Columns[4].Width = txtQuantity.Width;//128;
                ItemGrid.Columns[5].Width = txtRate.Width-1;//124;
                ItemGrid.Columns[6].Width = txtTotal.Width -5-  (ItemGrid.Controls.OfType<VScrollBar>().First().Visible? SystemInformation.VerticalScrollBarWidth:0); //145;
                if (ItemGrid.Rows.Count > 0)
                {
                    ItemGrid.FirstDisplayedScrollingRowIndex = ItemGrid.RowCount - 1;
                    ItemGrid.Rows[ItemGrid.RowCount - 1].Selected = true;
                }
                query = "select sum(total) from tblTemp";
                DataTable dt = (DataTable)Connections.Instance.ShowDataInGridView(query);
                if (dt.Rows.Count > 0)
                    txtGrand.Text = dt.Rows[0][0].ToString();
            }
            catch { }
        }
        private void txtRate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                if (txtItems.Text.Trim() == "" || txtItems.Tag == null || txtQuantity.Text.Trim() == "" || txtQuantity.Text.Trim() == "0" || txtRate.Text.Trim() == "" || txtRate.Text.Trim() == "0")
                {
                    if (txtItems.Text.Trim() == "" && txtItems.Tag == null && (txtQuantity.Text.Trim() == "" || txtQuantity.Text.Trim() == "0") && (txtRate.Text.Trim() == "" || txtRate.Text.Trim() == "0"))
                    {
                        txtCash.SelectionStart = 0;
                        txtCash.SelectionLength = txtCash.Text.Length;
                        txtCash.Focus();
                    }
                    else
                    {
                        MessageBox.Show("Please enter the details properly");
                        txtItems.Focus();
                    }

                    
                }
                else
                {
                    string query = "select Current_Stock,minRate from tblItem where itemId='" + txtItems.Tag.ToString() + "'";
                    DataTable dt = (DataTable)Connections.Instance.ShowDataInGridView(query);
                    query = "select qty from tblTemp where itemId='" + txtItems.Tag.ToString() + "'";
                    DataTable dt1 = (DataTable)Connections.Instance.ShowDataInGridView(query);
                    if (Convert.ToDouble(dt.Rows[0][1].ToString()) > Convert.ToDouble(txtRate.Text))
                    {
                        MessageBox.Show("Minimum rate level reached" + Environment.NewLine + "Please enter the rate >= " + dt.Rows[0][1].ToString());
                        txtRate.Text = dt.Rows[0][1].ToString();
                        txtRate.SelectionStart = 0;
                        txtRate.SelectionLength = txtRate.Text.Length;
                        txtRate.Focus();
                        return;
                    
                    }
                    
                    if (dt1.Rows.Count > 0)
                    {
                        if (txtBillno.Tag == null)
                        {
                            if (txtItemcode.Tag == null)
                            {
                                double newQty = Convert.ToDouble(txtQuantity.Text) + Convert.ToDouble(dt1.Rows[0][0].ToString());
                                double newTotal = newQty * Convert.ToDouble(txtRate.Text);
                                if (Convert.ToDouble(dt.Rows[0][0].ToString()) < newQty)
                                {
                                    DialogResult dialogResult = MessageBox.Show("No sufficient stock." + Environment.NewLine + "Do you want to continue with negative stock?", "Sale Voucher", MessageBoxButtons.YesNo);
                                    if (dialogResult == DialogResult.Yes)
                                        query = "update tblTemp set Qty='" + newQty + "',rate='" + Convert.ToDouble(txtRate.Text.Trim()) + "',total='" + newTotal + "' where itemId='" + txtItems.Tag.ToString() + "'";
                                }
                                else
                                    query = "update tblTemp set Qty='" + newQty + "',rate='" + Convert.ToDouble(txtRate.Text.Trim()) + "',total='" + newTotal + "' where itemId='" + txtItems.Tag.ToString() + "'";
                            }
                            else
                            {
                                if (Convert.ToDouble(dt.Rows[0][0].ToString()) < Convert.ToDouble(txtQuantity.Text))
                                {
                                    DialogResult dialogResult = MessageBox.Show("No sufficient stock." + Environment.NewLine + "Do you want to continue with negative stock?", "Sale Voucher", MessageBoxButtons.YesNo);
                                    if (dialogResult == DialogResult.Yes)
                                        query = "update tblTemp set Qty='" + Convert.ToDouble(txtQuantity.Text.Trim()) + "',rate='" + Convert.ToDouble(txtRate.Text.Trim()) + "',total='" + Convert.ToDouble(txtTotal.Text.Trim()) + "' where itemId='" + Convert.ToDouble(txtItemcode.Tag.ToString()) + "'";
                                }
                                else
                                    query = "update tblTemp set Qty='" + Convert.ToDouble(txtQuantity.Text.Trim()) + "',rate='" + Convert.ToDouble(txtRate.Text.Trim()) + "',total='" + Convert.ToDouble(txtTotal.Text.Trim()) + "' where itemId='" + Convert.ToDouble(txtItemcode.Tag.ToString()) + "'";
                            }
                        }
                        else
                        {
                            if (txtItemcode.Tag == null)
                            {
                                double newQty = Convert.ToDouble(txtQuantity.Text) + Convert.ToDouble(dt1.Rows[0][0].ToString());
                                double newTotal = newQty * Convert.ToDouble(txtRate.Text);
                                query = "select qty from tblSaleTrans where itemId='" + txtItems.Tag.ToString() + "' and saleId='" + txtBillno.Tag.ToString() + "'";
                                DataTable dt2 = (DataTable)Connections.Instance.ShowDataInGridView(query);
                                if (Convert.ToDouble(dt.Rows[0][0].ToString()) < (newQty-Convert.ToDouble(dt2.Rows[0][0].ToString())))
                                {
                                    DialogResult dialogResult = MessageBox.Show("No sufficient stock." + Environment.NewLine + "Do you want to continue with negative stock?", "Sale Voucher", MessageBoxButtons.YesNo);
                                    if (dialogResult == DialogResult.Yes)
                                        query = "update tblTemp set Qty='" + newQty + "',rate='" + Convert.ToDouble(txtRate.Text.Trim()) + "',total='" + newTotal + "' where itemId='" + txtItems.Tag.ToString() + "'";
                                }
                                else
                                    query = "update tblTemp set Qty='" + newQty + "',rate='" + Convert.ToDouble(txtRate.Text.Trim()) + "',total='" + newTotal + "' where itemId='" + txtItems.Tag.ToString() + "'";
                            }
                            else
                            {
                                query = "select qty from tblSaleTrans where itemId='" + txtItems.Tag.ToString() + "' and saleId='" + txtBillno.Tag.ToString() + "'";
                                DataTable dt2 = (DataTable)Connections.Instance.ShowDataInGridView(query);
                                if (Convert.ToDouble(dt.Rows[0][0].ToString()) < (Convert.ToDouble(txtQuantity.Text) - Convert.ToDouble(dt2.Rows[0][0].ToString())))
                                {
                                    DialogResult dialogResult = MessageBox.Show("No sufficient stock." + Environment.NewLine + "Do you want to continue with negative stock?", "Sale Voucher", MessageBoxButtons.YesNo);
                                    if (dialogResult == DialogResult.Yes)
                                        query = "update tblTemp set Qty='" + Convert.ToDouble(txtQuantity.Text.Trim()) + "',rate='" + Convert.ToDouble(txtRate.Text.Trim()) + "',total='" + Convert.ToDouble(txtTotal.Text.Trim()) + "' where itemId='" + Convert.ToDouble(txtItemcode.Tag.ToString()) + "'";
                                }
                                else
                                    query = "update tblTemp set Qty='" + Convert.ToDouble(txtQuantity.Text.Trim()) + "',rate='" + Convert.ToDouble(txtRate.Text.Trim()) + "',total='" + Convert.ToDouble(txtTotal.Text.Trim()) + "' where itemId='" + Convert.ToDouble(txtItemcode.Tag.ToString()) + "'";
                            }
                        }
                    }
                    else
                    {
                        double exQty = 0;
                        if (txtBillno.Tag != null)
                        {
                            query = "select qty from tblSaleTrans where itemId='" + txtItems.Tag.ToString() + "' and saleId='" + txtBillno.Tag.ToString() + "'";
                            DataTable dt3 = (DataTable)Connections.Instance.ShowDataInGridView(query);
                            exQty = Convert.ToDouble(dt.Rows[0][0].ToString());
                        }
                        if (Convert.ToDouble(dt.Rows[0][0].ToString()) < (Convert.ToDouble(txtQuantity.Text)-exQty))
                        {
                            DialogResult dialogResult = MessageBox.Show("No sufficient stock." + Environment.NewLine + "Do you want to continue with negative stock?", "Sale Voucher", MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                                query = "insert into tblTemp values('" + txtItemcode.Text.Trim() + "','" + txtItems.Tag.ToString() + "','" + txtItems.Text.Trim() + "','" + Convert.ToDouble(txtQuantity.Text.Trim()) + "','" + Convert.ToDouble(txtRate.Text.Trim()) + "','" + Convert.ToDouble(txtTotal.Text.Trim()) + "')";
                        }
                        else
                            query = "insert into tblTemp values('" + txtItemcode.Text.Trim() + "','" + txtItems.Tag.ToString() + "','" + txtItems.Text.Trim() + "','" + Convert.ToDouble(txtQuantity.Text.Trim()) + "','" + Convert.ToDouble(txtRate.Text.Trim()) + "','" + Convert.ToDouble(txtTotal.Text.Trim()) + "')";
                    }
                    Connections.Instance.ExecuteQueries(query);
                    dt.Dispose();
                    dt1.Dispose();
                    GridShow();
                    cmdItemClear_Click(null, null);
                }
            }
        }
        private void cmdItemClear_Click(object sender, EventArgs e)
        {
            txtItemcode.Tag = null;
            txtItemcode.Enabled = true;
            txtItemcode.Text ="";
            txtItems.Enabled = true;
            txtItems.Tag = null;
            txtItems.Text = "";
            txtQuantity.Text = "0";
            txtRate.Text = "0";
            lblPRate.Text = "0";
            txtItems.Focus();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }
        private void Edit_Click(object sender, ToolStripItemClickedEventArgs e)
        {
            if (ItemGrid.SelectedRows.Count > 0)
            {
                int rowno = ItemGrid.SelectedRows[0].Index;
                ToolStripItem item = e.ClickedItem;
                if (item.Name == "Edit")
                {
                    txtItemcode.Enabled = false;
                    txtItems.Enabled = false;
                    txtItems.Text = ItemGrid.Rows[rowno].Cells[3].Value.ToString();
                    txtItemcode.Text = ItemGrid.Rows[rowno].Cells[1].Value.ToString();
                    txtItems.Tag = ItemGrid.Rows[rowno].Cells[2].Value.ToString();
                    txtQuantity.Text  = ItemGrid.Rows[rowno].Cells[4].Value.ToString();
                    txtRate.Text = ItemGrid.Rows[rowno].Cells[5].Value.ToString();
                    txtItemcode.Tag = ItemGrid.Rows[rowno].Cells[2].Value.ToString();
                    txtQuantity.SelectionStart = 0;
                    txtQuantity.SelectionLength = txtQuantity.Text.Length;
                    txtQuantity.Focus();
                }
                else if (item.Name == "Delete")
                {
                    string query = "delete from tblTemp where itemId='" + ItemGrid.Rows[rowno].Cells[2].Value.ToString() + "'";
                    Connections.Instance.ExecuteQueries(query);
                    GridShow();
                }
            }
        }

        private void txtCash_TextChanged(object sender, EventArgs e)
        {
            Calculation();
        }
        private void Calculation()
        {
            if (txtCBalance.Text == "")
                txtCBalance.Text = "0";
            if (txtGrand.Text == "")
                txtGrand.Text = "0";
            if (txtCash.Text == "")
                txtCash.Text = "0";
            if (txtDiscount.Text == "")
                txtDiscount.Text = "0";
            txtBalance.Text = ((Convert.ToDouble(txtCBalance.Text) + Convert.ToDouble(txtGrand.Text)) - Convert.ToDouble(txtCash.Text) - Convert.ToDouble(txtDiscount.Text)).ToString();
        }

        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            Calculation();

        }

        private void txtCBalance_TextChanged(object sender, EventArgs e)
        {
            Calculation();
        }

        private void txtGrand_TextChanged(object sender, EventArgs e)
        {
            Calculation();
        }

        private void cboRategroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ItemGrid.Rows.Count > 0)
            {
                for (int i = 0; i < ItemGrid.Rows.Count; i++)
                {
                    string query = "";
                    if (!chkWholeSale.Checked)
                        query = "select rate from tblItem where itemId='" + ItemGrid.Rows[i].Cells[2].Value + "'";
                    else if (chkWholeSale.Checked)
                        query = "select wrate from tblItem where itemId='" + ItemGrid.Rows[i].Cells[2].Value + "'";
                    DataTable dt = (DataTable)Connections.Instance.ShowDataInGridView(query);
                    query = "update tblTemp set rate='" + Convert.ToDouble(dt.Rows[0][0].ToString()) + "',Total='" + Convert.ToDouble(dt.Rows[0][0].ToString()) * Convert.ToDouble(ItemGrid.Rows[i].Cells[4].Value) + "' where itemId='" + ItemGrid.Rows[i].Cells[2].Value + "'";
                    Connections.Instance.ExecuteQueries(query);
                }
                GridShow();
            }

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtBillno.Tag = null;
            cboArea.Enabled = true;
            cboCustomer.Enabled = true;
            txtPlace.Text = "";
            txtCBalance.Text = "";
            cmdItemClear_Click(null, null);
            ItemGrid.DataSource = null;
            txtGrand.Text = "";
            txtCash.Text = "";
            txtDiscount.Text ="";
            txtBalance.Text = "0";
            SetBillNo();
            cboCustomer.SelectedIndex = -1;
            cboCustomer.Text = "";
            //SearchGridLoad();
            //if(!(cboAreaSearch.SelectedIndex>=0 || txtBillNoSearch.Text.Trim()!="" || cboCustomerSearch.SelectedIndex>=0))
            //    btnReset_Click(null, null);
            //else if(cboAreaSearch.SelectedIndex>=0 && cboAreaSearch.SelectedIndex==cboArea.SelectedIndex)
                btnReset_Click(null, null);
            cboCustomer.Focus();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchGridLoad();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            cboAreaSearch.SelectedValue = -1;
            cboCustomerSearch.SelectedValue = -1;
            DtFrom.Value = DateTime.Today;
            DtTo.Value = DateTime.Today;
            txtBillNoSearch.Text = "";
            SearchGridLoad();
            cboAreaSearch.Focus();
        }

        private void SearchGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            { 
                txtBillno.Tag = SearchGrid.Rows[e.RowIndex].Cells[0].Value.ToString();
                txtBillno.Text = SearchGrid.Rows[e.RowIndex].Cells[1].Value.ToString();
                DatePicker.Value = Convert.ToDateTime(SearchGrid.Rows[e.RowIndex].Cells[2].Value);
                cboArea.SelectedValue = SearchGrid.Rows[e.RowIndex].Cells[4].Value.ToString();
                cboArea_SelectionChangeCommitted(null, null);
                cboCustomer.SelectedValue = SearchGrid.Rows[e.RowIndex].Cells[5].Value.ToString();
                cboCustomer_SelectionChangeCommitted(null, null);
                cboCustomer.Text = SearchGrid.Rows[e.RowIndex].Cells[3].Value.ToString();
                txtCBalance.Text = SearchGrid.Rows[e.RowIndex].Cells[6].Value.ToString();
                txtGrand.Text = SearchGrid.Rows[e.RowIndex].Cells[7].Value.ToString();
                txtCash.Text = SearchGrid.Rows[e.RowIndex].Cells[8].Value.ToString();
                txtDiscount.Text = SearchGrid.Rows[e.RowIndex].Cells[9].Value.ToString();
                txtBalance.Text = SearchGrid.Rows[e.RowIndex].Cells[10].Value.ToString();
                cboArea.Enabled = false;
                cboCustomer.Enabled = false;
                string query = "truncate table tblTemp";
                Connections.Instance.ExecuteQueries(query);
                query = "insert into tblTemp select i.Item_Code,i.itemId,i.Item_Name,s.qty,s.rate,s.total from tblSaleTrans s,tblItem i where s.itemId=i.itemId and saleId='" + txtBillno.Tag.ToString() + "'";
                Connections.Instance.ExecuteQueries(query);
                GridShow();
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
                 //if (ItemGrid.Rows.Count == 0)
                //    MessageBox.Show("Please add items");
                //else
                //{

                    DialogResult dialogResult = MessageBox.Show("Do you want to save this bill?", "Sale Voucher", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        string query = "";
                        if (txtBillno.Tag == null)
                        {
                            string custId = ((cboCustomer.SelectedValue == null) ? 1 : cboCustomer.SelectedValue).ToString();

                            query = "insert into tblSales values('" + txtBillno.Text + "','" + DatePicker.Value.ToString("dd-MMM-yyyy") + "','" + cboArea.SelectedValue + "','" + custId + "','" + Convert.ToDouble(txtCBalance.Text) + "','" + Convert.ToDouble(txtGrand.Text) + "','" + Convert.ToDouble(txtCash.Text) + "','" + Convert.ToDouble(txtDiscount.Text) + "','" + Convert.ToDouble(txtBalance.Text) + "','false')";
                            Connections.Instance.ExecuteQueries(query);
                            query = "select ident_current('tblSales')";
                            DataTable dt = (DataTable)Connections.Instance.ShowDataInGridView(query);
                            int id = Convert.ToInt32(dt.Rows[0][0].ToString());
                            if (custId != "1")
                            {
                                query = "update tblCustomer set creditBal='" + Convert.ToDouble(txtBalance.Text) + "' where custId='" + cboCustomer.SelectedValue + "'";
                                Connections.Instance.ExecuteQueries(query);
                            }
                            for (int i = 0; i < ItemGrid.Rows.Count; i++)
                            {
                                query = "insert into tblSaleTrans values('" + id + "','" + ItemGrid.Rows[i].Cells[2].Value + "','" + ItemGrid.Rows[i].Cells[4].Value + "','" + ItemGrid.Rows[i].Cells[5].Value + "','" + ItemGrid.Rows[i].Cells[6].Value + "','false')";
                                Connections.Instance.ExecuteQueries(query);
                                query = "update tblItem set Current_Stock=Current_Stock-'" + ItemGrid.Rows[i].Cells[4].Value + "' where itemId='" + ItemGrid.Rows[i].Cells[2].Value + "'";
                                Connections.Instance.ExecuteQueries(query);
                            }
                            query = "update tblSettings set BillNo='" + id + "'";
                            Connections.Instance.ExecuteQueries(query);

                        }
                        else
                        {
                            //update
                            query = "select custId,Balance from tblSales where saleId='" + txtBillno.Tag.ToString() + "'";
                            DataTable dt = (DataTable)Connections.Instance.ShowDataInGridView(query);
                            double newBal = Convert.ToDouble(dt.Rows[0][1].ToString()) - Convert.ToDouble(txtBalance.Text);//exbalance-balance
                            query = "update tblCustomer set creditBal=creditBal-'" + newBal + "' where custId='" + dt.Rows[0][0].ToString() + "'";
                            Connections.Instance.ExecuteQueries(query);
                            query = "update tblSales set BillDate='" + DatePicker.Value.ToString("dd-MMM-yyyy") + "',GrandTotal='" + txtGrand.Text + "',Cash='" + Convert.ToDouble(txtCash.Text) + "',Discount='" + Convert.ToDouble(txtDiscount.Text) + "',Balance='" + Convert.ToDouble(txtBalance.Text) + "'  where saleId='" + txtBillno.Tag.ToString() + "'";
                            Connections.Instance.ExecuteQueries(query);
                            query = "select itemId,qty from tblSaletrans where saleId='" + txtBillno.Tag.ToString() + "'";
                            dt.Rows.Clear();
                            dt = (DataTable)Connections.Instance.ShowDataInGridView(query);
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                query = "update tblItem set Current_Stock=Current_Stock+'" + Convert.ToDouble(dt.Rows[i][1].ToString()) + "' where itemId='" + dt.Rows[i][0].ToString() + "'";
                                Connections.Instance.ExecuteQueries(query);
                            }
                            query = "delete from tblSaletrans where saleId='" + txtBillno.Tag.ToString() + "'";
                            Connections.Instance.ExecuteQueries(query);
                            for (int i = 0; i < ItemGrid.Rows.Count; i++)
                            {
                                query = "insert into tblSaleTrans values('" + txtBillno.Tag.ToString() + "','" + ItemGrid.Rows[i].Cells[2].Value + "','" + ItemGrid.Rows[i].Cells[4].Value + "','" + ItemGrid.Rows[i].Cells[5].Value + "','" + ItemGrid.Rows[i].Cells[6].Value + "','false')";
                                Connections.Instance.ExecuteQueries(query);
                                query = "update tblItem set Current_Stock=Current_Stock-'" + ItemGrid.Rows[i].Cells[4].Value + "' where itemId='" + ItemGrid.Rows[i].Cells[2].Value + "'";
                                Connections.Instance.ExecuteQueries(query);
                            }

                        }
                        dialogResult = MessageBox.Show("Do you want to print this bill?", "Sale Voucher", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            string query1 = "select i.inMalayalam as ItemsInMalayalam,t.ItemName as Items,t.Qty as Qty,t.Rate as Rate,t.Total as Total from tblTemp t,tblItem i where t.ItemId=i.ItemId";
                            System.Data.DataColumn BillNo = new System.Data.DataColumn("BillNo", typeof(System.String));
                            BillNo.DefaultValue = txtBillno.Text;
                            System.Data.DataColumn BillDate = new System.Data.DataColumn("BillDate", typeof(System.String));
                            BillDate.DefaultValue = DatePicker.Value.ToString("dd-MMM-yyyy");
                            System.Data.DataColumn Customer = new System.Data.DataColumn("Customer", typeof(System.String));
                            Customer.DefaultValue = cboCustomer.Text.ToString();
                            System.Data.DataColumn GrandTotal = new System.Data.DataColumn("GrandTotal", typeof(System.Decimal));
                            GrandTotal.DefaultValue = txtGrand.Text;
                            System.Data.DataColumn Cash = new System.Data.DataColumn("Cash", typeof(System.Decimal));
                            Cash.DefaultValue = txtCash.Text;
                            System.Data.DataColumn Discount = new System.Data.DataColumn("Discount", typeof(System.Decimal));
                            Discount.DefaultValue = txtDiscount.Text;
                            System.Data.DataColumn Balance = new System.Data.DataColumn("Balance", typeof(System.Decimal));
                            Balance.DefaultValue = txtBalance.Text;
                            System.Data.DataColumn PrevBalance = new System.Data.DataColumn("PrevBalance", typeof(System.Decimal));
                            PrevBalance.DefaultValue = txtCBalance.Text;

                            

                            DataTable dt = (DataTable)Connections.Instance.ShowDataInGridView(query1);

                            dt.Columns.Add(PrevBalance);
                            dt.Columns.Add(Balance);
                            dt.Columns.Add(Discount);

                            dt.Columns.Add(Cash);
                            dt.Columns.Add(GrandTotal);
                            dt.Columns.Add(Customer);
                            dt.Columns.Add(BillDate);
                            dt.Columns.Add(BillNo);
                            ds.Tables["Bill"].Clear();
                            ds.Tables["Bill"].Merge(dt);

                            ReportDocument cryRpt = new ReportDocument();
                            cryRpt.Load(System.IO.Path.GetDirectoryName(Application.ExecutablePath).ToString() + @"\Reports\rptBill.rpt");
                            cryRpt.SetDataSource(ds);
                            cryRpt.Refresh();
                            cryRpt.PrintToPrinter(1, true, 0, 0);
                        }

                            query = "truncate table tblTemp";
                            Connections.Instance.ExecuteQueries(query);
                        
                        btnClear_Click(null, null);
                    //}
                
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtBillno.Tag == null)
                MessageBox.Show("Please select a bill to delete");
            else
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to delete the bill?", "Sale Voucher", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    string query = "select custId,CBalance,Balance from tblSales where saleId='" + txtBillno.Tag.ToString() + "'";
                    DataTable dt = (DataTable)Connections.Instance.ShowDataInGridView(query);
                    double newBal = Convert.ToDouble(dt.Rows[0][1].ToString()) - Convert.ToDouble(dt.Rows[0][2].ToString());//creditbalance-balance
                    query = "update tblCustomer set creditBal=creditBal+'" + newBal + "' where custId='" + dt.Rows[0][0].ToString() + "'";
                    Connections.Instance.ExecuteQueries(query);
                    query = "delete from tblSales where saleId='" + txtBillno.Tag.ToString() + "'";
                    Connections.Instance.ExecuteQueries(query);
                    query = "select itemid,qty from tblSaletrans where saleId='" + txtBillno.Tag.ToString() + "'";
                    dt.Rows.Clear();
                    dt = (DataTable)Connections.Instance.ShowDataInGridView(query);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        query = "update tblItem set Current_Stock=Current_Stock+'" + Convert.ToDouble(dt.Rows[i][1].ToString()) + "' where itemId='" + dt.Rows[i][0].ToString() + "'";
                        Connections.Instance.ExecuteQueries(query);
                    }
                    query = "delete from tblSaletrans where saleId='" + txtBillno.Tag.ToString() + "'";
                    Connections.Instance.ExecuteQueries(query);
                    SearchGridLoad();
                    btnClear_Click(null, null);
                }
            }
               
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            btnSave_Click(null, null);
        }

        private void cboArea_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cboCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void cboCustomer_TextChange(object sender, EventArgs e)
        {
            if (cboCustomer.Text == "")
            {
                txtCBalance.Text = "";
                cboCustomer.SelectedIndex = -1;
            }
        }
        

        private void cboAreaSearch_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            btnSearch_Click(null, null);
        }

        private void txtItemcode_TextChanged(object sender, EventArgs e)
        {

        }
        private void txtItems_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            if (e.KeyChar == 13)
            {
                txtQuantity.SelectionStart = 0;
                txtQuantity.SelectionLength = txtQuantity.Text.Length;
                    txtQuantity.Focus();
            }
        }
        private void txtItems_TextChanged(object sender, EventArgs e)
        {
            string query = "select itemId,Item_Code,Item_Name,Rate,WRate,PRate from tblitem where Item_Name like '" + txtItems.Text + "%';";
            ItemDisplayGrid.DataSource = Connections.Instance.ShowDataInGridView(query);
            ItemDisplayGrid.Columns[2].Width = 275;

            if (txtItems.Text.Trim() != "")
            {
                itemView.Visible = false;
                if (ItemDisplayGrid.Rows.Count == 1 && txtItems.Text != ItemDisplayGrid.Rows[0].Cells[2].Value.ToString())
                {

                    txtItems.Tag = ItemDisplayGrid.Rows[0].Cells[0].Value.ToString();
                    txtItemcode.Text = ItemDisplayGrid.Rows[0].Cells[1].Value.ToString();
                    txtRate.Text = ItemDisplayGrid.Rows[0].Cells[3].Value.ToString();
                    lblPRate.Text = ItemDisplayGrid.Rows[0].Cells[5].Value.ToString();

                    if (chkWholeSale.Checked == true)
                        txtRate.Text = ItemDisplayGrid.Rows[0].Cells[4].Value.ToString();
                    itemView.Visible = false;
                    txtQuantity.SelectionStart = 0;
                    txtQuantity.SelectionLength = txtQuantity.Text.Length;
                    txtQuantity.Focus();
                    txtItems.Text = ItemDisplayGrid.Rows[0].Cells[2].Value.ToString();
                }
                else if (ItemDisplayGrid.Rows.Count == 1 && txtItems.Text == ItemDisplayGrid.Rows[0].Cells[2].Value.ToString())
                {
                    itemView.Visible = false;
                }
                else
                {
                    itemView.Visible = true;
                    ItemDisplayGrid.ColumnHeadersVisible = true;
                    ItemDisplayGrid.Columns[0].Visible = false;
                    ItemDisplayGrid.Columns[1].Visible = false;
                    //ItemDisplayGrid.Columns[3].Visible = false;
                    //ItemDisplayGrid.Columns[4].Visible = false;
                    //ItemDisplayGrid.Columns[5].Visible = false;

                    ItemDisplayGrid.ClearSelection();
                }
            }
        }

        private void txtItems_KeyDown(object sender, KeyEventArgs e)
        {
           
            if (ItemDisplayGrid.Rows.Count > 1 && e.KeyCode == Keys.Down)
            {
                if (txtItems.Text.Trim() == "")
                {
                    string query = "select itemId,Item_Code,Item_Name,Rate,WRate,PRate from tblitem order by Item_Name;";
                    ItemDisplayGrid.DataSource = Connections.Instance.ShowDataInGridView(query);
                    ItemDisplayGrid.Columns[2].Width = 275;
                }
                itemView.Visible = true;
                ItemDisplayGrid.Focus();
                ItemDisplayGrid.Rows[0].Selected = true;

            }
            else if (e.KeyCode == Keys.Escape)
            {
                itemView.Visible = false;
            }
            else if(e.KeyCode == Keys.Down)
            {
                itemView.Visible = true;

                if (txtItems.Text.Trim() == "")
                {
                    itemView.Visible = false;
                    string query = "select itemId,Item_Code,Item_Name,Rate,WRate,PRate from tblitem order by Item_Name;";
                    ItemDisplayGrid.DataSource = Connections.Instance.ShowDataInGridView(query);
                    ItemDisplayGrid.Columns[2].Width = 275;
                    itemView.Visible = true;
                    ItemDisplayGrid.ColumnHeadersVisible = true;
                    ItemDisplayGrid.Columns[0].Visible = false;
                    ItemDisplayGrid.Columns[1].Visible = false;
                    //ItemDisplayGrid.Columns[3].Visible = false;
                    //ItemDisplayGrid.Columns[4].Visible = false;
                    //ItemDisplayGrid.Columns[5].Visible = false;

                    ItemDisplayGrid.ClearSelection();
                    ItemDisplayGrid.Focus();
                    ItemDisplayGrid.Rows[0].Selected = true;
                }
            }
        }
        private void ItemDisplayGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter && ItemDisplayGrid.Rows.Count > 0)
            {
                int r = ItemDisplayGrid.CurrentRow.Index;
                txtItems.Tag = ItemDisplayGrid.Rows[r].Cells[0].Value.ToString();
                txtItemcode.Text = ItemDisplayGrid.Rows[r].Cells[1].Value.ToString();
                txtRate.Text = ItemDisplayGrid.Rows[r].Cells[3].Value.ToString();
                if (chkWholeSale.Checked == true)
                    txtRate.Text = ItemDisplayGrid.Rows[r].Cells[4].Value.ToString();
                txtQuantity.SelectionStart = 0;
                txtQuantity.SelectionLength = txtQuantity.Text.Length;
                txtQuantity.Focus();
                lblPRate.Text = ItemDisplayGrid.Rows[r].Cells[5].Value.ToString();
                txtItems.Text = ItemDisplayGrid.Rows[r].Cells[2].Value.ToString();
                

                itemView.Visible = false;
            }
            else if (e.KeyData == Keys.Escape)
            {
                itemView.Visible = false;
                txtItems.Focus();
            }
        }
        private void ItemDisplayGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int r = ItemDisplayGrid.CurrentRow.Index;
                txtItems.Tag = ItemDisplayGrid.Rows[r].Cells[0].Value.ToString();
                txtItemcode.Text = ItemDisplayGrid.Rows[r].Cells[1].Value.ToString();
                txtRate.Text = ItemDisplayGrid.Rows[r].Cells[3].Value.ToString();
                if (chkWholeSale.Checked)
                    txtRate.Text = ItemDisplayGrid.Rows[r].Cells[4].Value.ToString();
                txtQuantity.SelectionStart = 0;
                txtQuantity.SelectionLength = txtQuantity.Text.Length;
                txtQuantity.Focus();
                lblPRate.Text = ItemDisplayGrid.Rows[r].Cells[5].Value.ToString();
                txtItems.Text = ItemDisplayGrid.Rows[r].Cells[2].Value.ToString();
                if (ItemDisplayGrid.Rows.Count>0) 
                    ItemDisplayGrid.Rows.Clear();
                itemView.Visible = false;
            }
        }

        private void chkWholeSale_CheckedChanged(object sender, EventArgs e)
        {

            if (txtItems.Tag != "" && txtItems.Tag != null)
            {
                string query = "";
                if (!chkWholeSale.Checked)
                    query = "select rate from tblItem where itemId='" + txtItems.Tag + "'";
                else if (chkWholeSale.Checked)
                    query = "select wrate from tblItem where itemId='" + txtItems.Tag + "'";
                DataTable dt = (DataTable)Connections.Instance.ShowDataInGridView(query);
                txtRate.Text = dt.Rows[0][0].ToString();
            }
        }

        private void SearchGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Edit_Click(object sender, EventArgs e)
        {

        }

        private void ItemDisplayGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


    }
}
