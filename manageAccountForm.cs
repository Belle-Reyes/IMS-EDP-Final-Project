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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace InventoryManagementSystem_GloriousSole
{
    public partial class manageAccountForm : Form
    {
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\reyes\Downloads\Ims + Desing\IMSDesign-SISON\InventoryManagementSystem-GloriousSole\SQL InventoryManagementSystem-GloriousSole\GS_IMS.mdf"";Integrated Security=True;Connect Timeout=30";
        public manageAccountForm()
        {
            InitializeComponent();
            LoadAccountDetails();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            dashboardForm d = new dashboardForm();
            this.Hide();
            d.ShowDialog();
        }

        private void btnViewInventory_Click(object sender, EventArgs e)
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

        private void btnInvoiceLogs_Click(object sender, EventArgs e)
        {
            invoiceLogsForm il = new invoiceLogsForm();
            this.Hide();
            il.ShowDialog();
        }
         private void LoadAccountDetails()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    // Update query using UserId from UserSession
                    SqlCommand cmd = new SqlCommand("SELECT LastName, FirstName, Username FROM Users WHERE UserID = @UserID", con);
                    cmd.Parameters.AddWithValue("@UserID", UserSession.UserId); // Use UserId from UserSession

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            UserSession.FullName = $"{reader["FirstName"]} {reader["LastName"]}";
                            UserSession.Username = reader["Username"].ToString();

                            // Display account details in labels
                            txtLastName.Text = reader["LastName"].ToString();
                            txtFirstName.Text = reader["FirstName"].ToString();
                            txtUsername.Text = reader["Username"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("Account not found.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
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

        private void btnEdit_Click(object sender, EventArgs e)
        {
            editAccountDetailsForm eadForm = new editAccountDetailsForm();
            this.Hide();
            eadForm.ShowDialog();
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            changePasswordForm cpForm = new changePasswordForm();
            this.Hide();
            cpForm.ShowDialog();
        }
    }
}
