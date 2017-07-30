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
    public partial class frmMenu : Form
    {
        public string user;
        public frmMenu()
        {
            InitializeComponent();
        }

        private void areaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Form> openForms = new List<Form>();
            foreach (Form f in Application.OpenForms)
                openForms.Add(f);
            foreach (Form f in openForms)
            {
                if (f.Name == "frmArea")
                {
                    f.Activate();
                    return;
                }
            }
            frmArea frm = new frmArea();
            frm.Show(this);
        }

        private void frmMenu_Load(object sender, EventArgs e)
        {
            frmLogin frm  = new frmLogin();
            frm.ShowDialog();
        }

        private void staffRegistrationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Form> openForms = new List<Form>();
            foreach (Form f in Application.OpenForms)
                openForms.Add(f);
            foreach (Form f in openForms)
            {
                if (f.Name == "frmStaffRegistration")
                {
                    f.Activate();
                    return;
                }
            }
            frmStaffRegistration frm = new frmStaffRegistration();
            frm.Show(this);
        }

        private void customerMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Form> openForms = new List<Form>();
            foreach (Form f in Application.OpenForms)
                openForms.Add(f);
            foreach (Form f in openForms)
            {
                if (f.Name == "frmCustomerMaster")
                {
                    f.Activate();
                    return;
                }
            }
            frmCustomerMaster frm = new frmCustomerMaster();
            frm.Show(this);
        }

        private void itemMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Form> openForms = new List<Form>();
            foreach (Form f in Application.OpenForms)
                openForms.Add(f);
            foreach (Form f in openForms)
            {
                if (f.Name == "frmItemMaster")
                {
                    f.Activate();
                    return;
                }
            }
            frmItemMaster frm = new frmItemMaster();
            frm.Show(this);
        }

        private void salesVoucherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Form> openForms = new List<Form>();
            foreach (Form f in Application.OpenForms)
                openForms.Add(f);
            foreach (Form f in openForms)
            {
                if (f.Name == "frmSales")
                {
                    f.Activate();
                    return;
                }
            }
            frmSalesGST frm = new frmSalesGST();
            frm.Show(this);
        }

        private void saleSheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Form> openForms = new List<Form>();
            foreach (Form f in Application.OpenForms)
                openForms.Add(f);
            foreach (Form f in openForms)
            {
                if (f.Name == "frmSaleSheet")
                {
                    f.Activate();
                    return;
                }
            }
            frmSaleSheet frm = new frmSaleSheet();
            frm.Show(this);
        }

        private void addStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Form> openForms = new List<Form>();
            foreach (Form f in Application.OpenForms)
                openForms.Add(f);
            foreach (Form f in openForms)
            {
                if (f.Name == "frmAddStock")
                {
                    f.Activate();
                    return;
                }
            }
            frmAddStock frm = new frmAddStock();
            frm.Show(this);
        }

        private void orderedItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Form> openForms = new List<Form>();
            foreach (Form f in Application.OpenForms)
                openForms.Add(f);
            foreach (Form f in openForms)
            {
                if (f.Name == "frmOrderedItems")
                {
                    f.Activate();
                    return;
                }
            }
            frmOrderedItems frm = new frmOrderedItems();
            frm.Show(this);
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Form> openForms = new List<Form>();
            foreach (Form f in Application.OpenForms)
                openForms.Add(f);
            foreach (Form f in openForms)
            {
                if (f.Name != "frmMenu")
                    f.Close();
            }
            frmLogin frm = new frmLogin();
            frm.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            List<Form> openForms = new List<Form>();
            foreach (Form f in Application.OpenForms)
                openForms.Add(f);
            foreach (Form f in openForms)
            {
                if (f.Name == "frmSales")
                {
                    f.Activate();
                    return;
                }
            }
            frmSalesGST frm = new frmSalesGST();
            frm.Show(this);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            List<Form> openForms = new List<Form>();
            foreach (Form f in Application.OpenForms)
                openForms.Add(f);
            foreach (Form f in openForms)
            {
                if (f.Name == "frmAddStock")
                {
                    f.Activate();
                    return;
                }
            }
            frmAddStock frm = new frmAddStock();
            frm.Show(this);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            List<Form> openForms = new List<Form>();
            foreach (Form f in Application.OpenForms)
                openForms.Add(f);
            foreach (Form f in openForms)
            {
                if (f.Name == "frmSaleSheet")
                {
                    f.Activate();
                    return;
                }
            }
            frmSaleSheet frm = new frmSaleSheet();
            frm.Show(this);

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            List<Form> openForms = new List<Form>();
            foreach (Form f in Application.OpenForms)
                openForms.Add(f);
            foreach (Form f in openForms)
            {
                if (f.Name == "frmOrderedItems")
                {
                    f.Activate();
                    return;
                }
            }
            frmOrderedItems frm = new frmOrderedItems();
            frm.Show(this);
        }

        private void stockReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Form> openForms = new List<Form>();
            foreach (Form f in Application.OpenForms)
                openForms.Add(f);
            foreach (Form f in openForms)
            {
                if (f.Name == "frmStockView")
                {
                    f.Activate();
                    return;
                }
            }
            frmStockView frm = new frmStockView();
            frm.Show(this);
        }

        private void cashEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Form> openForms = new List<Form>();
            foreach (Form f in Application.OpenForms)
                openForms.Add(f);
            foreach (Form f in openForms)
            {
                if (f.Name == "frmCashEntry")
                {
                    f.Activate();
                    return;
                }
            }
            frmCashEntry frm = new frmCashEntry();
            frm.Show(this);
        }
    }
}
