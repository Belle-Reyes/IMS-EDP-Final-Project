using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryManagementSystem_GloriousSole
{
    public partial class invoiceLogsForm : Form
    {
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\reyes\Downloads\Ims + Desing\IMSDesign-SISON\InventoryManagementSystem-GloriousSole\SQL InventoryManagementSystem-GloriousSole\GS_IMS.mdf"";Integrated Security=True;Connect Timeout=30";
        SqlConnection con;
        SqlCommand cm;
        SqlDataReader dr;
        public invoiceLogsForm()
        {
            InitializeComponent();
             LoadData();
        }
        private void LoadData()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT DISTINCT FullName, Action, Item, Status, LogDate FROM Logs;", con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvInvoiceLogs.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        private void btnDashboard_Click(object sender, EventArgs e)
        {
            dashboardForm d = new dashboardForm();
            this.Hide();
            d.ShowDialog();
        }

        private void btnViewInventory_Click_1(object sender, EventArgs e)
        {
            viewInventoryForm vi = new viewInventoryForm();
            this.Hide();
            vi.ShowDialog();
        }

        private void btnUpdateInventory_Click(object sender, EventArgs e)
        {
            updateInventoryForm ui = new updateInventoryForm();
            this.Hide();
            ui.ShowDialog();
        }

        private void btnManageAccount_Click(object sender, EventArgs e)
        {
            manageAccountForm ma = new manageAccountForm();
            this.Hide();
            ma.ShowDialog();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {

            DialogResult result = MessageBox.Show(
                "Are you sure you want to log out?",
                "Logout Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit(); // This will close the entire application
            }
        }
    }
}
