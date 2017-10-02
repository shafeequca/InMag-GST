using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InMag_V._16
{
    public partial class frmPartyReport : Form
    {
        public frmPartyReport()
        {
            InitializeComponent();
            this.txtCustomer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCustomer_KeyDown);
            this.CustomerGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CustomerGrid_KeyDown);
        }

        private void Both_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Party_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Customer_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void frmPartyReport_Load(object sender, EventArgs e)
        {
            try
            {
                string query = "select areaId,Area from tblArea order By Area";
                cboArea.DataSource = Connections.Instance.ShowDataInGridView(query);
                cboArea.DisplayMember = "Area";
                cboArea.ValueMember = "areaId";
                cboArea.SelectedIndex = -1;
                cboArea.Text = "";

              
            }
            catch { }
        }
        private void txtCustomer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Down)
            {
                txtCustomer_TextChanged(null, null);
                CustomerGrid.Focus();
            }
            else if (e.KeyData == Keys.Enter)
            {
                CustomerGrid.Visible = false;
                btnShow_Click(null, null);
            }

        }
        private void txtCustomer_TextChanged(object sender, EventArgs e)
        {
            string Query="";
            if (cboArea.SelectedIndex == -1)
            {
                CustomerGrid.Visible = false;            
            }
            else
            {
                CustomerGrid.Visible = true;
                if (Customer.Checked)
                    Query = "select * from tblCustomer where Customer like '" + txtCustomer.Text + "%' and areaid='" + cboArea.SelectedValue + "'  and CType IN('Customer')";
                else if (Party.Checked)
                    Query = "select * from tblCustomer where Customer like '" + txtCustomer.Text + "%' and areaid='" + cboArea.SelectedValue + "'  and CType IN('Party')";
                else
                    Query = "select * from tblCustomer where Customer like '" + txtCustomer.Text + "%' and areaid='" + cboArea.SelectedValue + "'  and CType IN('Both')";

                CustomerGrid.DataSource = Connections.Instance.ShowDataInGridView(Query);
                CustomerGrid.ColumnHeadersVisible = false;
                CustomerGrid.Columns[0].Visible = false;
                CustomerGrid.Columns[2].Visible = false;
                CustomerGrid.Columns[3].Visible = false;
                CustomerGrid.Columns[4].Visible = false;
                CustomerGrid.Columns[5].Visible = false;
                CustomerGrid.Columns[6].Visible = false;
                CustomerGrid.Columns[7].Visible = false;
                CustomerGrid.Columns[8].Visible = false;
                CustomerGrid.Columns[9].Visible = false;
                CustomerGrid.Columns[10].Visible = false;
                CustomerGrid.Columns[11].Visible = false;
                CustomerGrid.Columns[12].Visible = false;
                CustomerGrid.Columns[13].Visible = false;
                CustomerGrid.Columns[14].Visible = false;
                CustomerGrid.Columns[15].Visible = false;

                
            }
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            string Query = "select BillNo,BillDate,CGST,SGST,IGST,Discount,GrandTotal As [Bill Total],CBalance AS [Prev. Balance],Cash,Balance,BillType from tblSales where custId ='" + txtCustomer.Tag + "' and areaid='" + cboArea.SelectedValue + "'  and BillDate>='" + DtFrom.Value.ToString("yyyy-MM-dd") + "' and BillDate<='" + DtTo.Value.ToString("yyyy-MM-dd") + "'";
            ItemGrid.DataSource = Connections.Instance.ShowDataInGridView(Query);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        private void CustomerGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int r = CustomerGrid.CurrentRow.Index;
                txtCustomer.Tag = CustomerGrid.Rows[r].Cells[0].Value.ToString();
                txtCustomer.Text = CustomerGrid.Rows[r].Cells[1].Value.ToString();
                CustomerGrid.Visible = false;
                
            }
        }

        private void ItemGrid_CellContentClick1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void CustomerGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void CustomerGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter && CustomerGrid.Rows.Count > 0)
            {
                int r = CustomerGrid.CurrentRow.Index;
                txtCustomer.Tag = CustomerGrid.Rows[r].Cells[0].Value.ToString();
                
                txtCustomer.Text = CustomerGrid.Rows[r].Cells[1].Value.ToString();
                CustomerGrid.Visible = false;
            }
            else if (e.KeyData == Keys.Escape)
            {
                CustomerGrid.Visible = false;
                txtCustomer.Focus();
            }
        }
    }
}
