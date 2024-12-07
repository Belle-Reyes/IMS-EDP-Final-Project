using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System;

namespace InventoryManagementSystem_GloriousSole
{
    public partial class viewInventoryForm : Form
    {
        // Connection string to connect to the local database
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\reyes\Downloads\Ims + Desing\IMSDesign-SISON\InventoryManagementSystem-GloriousSole\SQL InventoryManagementSystem-GloriousSole\GS_IMS.mdf"";Integrated Security=True;Connect Timeout=30";

        public viewInventoryForm()
        {
            InitializeComponent(); // Initialize form components
            InitializeSortComboBox(); // Set up sorting options in the combo box
            LoadData(); // Load inventory data into the DataGridView
        }

        // Method to populate the sorting options in the combo box
        private void InitializeSortComboBox()
        {
            cbSortBy.Items.Add("Recently Updated");
            cbSortBy.Items.Add("A-Z");
            cbSortBy.Items.Add("Z-A");
            cbSortBy.Items.Add("Price (Low to High)");
            cbSortBy.Items.Add("Price (High to Low)");
            cbSortBy.SelectedIndex = 1; // Default sort order is A-Z
        }

        // Method to load data from the database and apply sorting
        private void LoadData(string sortOrder = "A-Z")
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cm = new SqlCommand())
                {
                    cm.Connection = con;

                    try
                    {
                        con.Open(); // Open the database connection
                        string query = @"SELECT Brand, Model, Size, Quantity, PricePerPiece, (Quantity * PricePerPiece) AS TotalPrice FROM Inventory";

                        // Determine the sorting order based on user selection
                        switch (sortOrder)
                        {
                            case "A-Z":
                                query += " ORDER BY Brand ASC"; // Sort by Brand ascending
                                break;
                            case "Z-A":
                                query += " ORDER BY Brand DESC"; // Sort by Brand descending
                                break;
                            case "Price (Low to High)":
                                query += " ORDER BY PricePerPiece ASC"; // Sort by Price ascending
                                break;
                            case "Price (High to Low)":
                                query += " ORDER BY PricePerPiece DESC"; // Sort by Price descending
                                break;
                            case "Recently Updated":
                                query += " ORDER BY UpdatedAt DESC"; // Sort by most recently updated
                                break;
                            default:
                                break;
                        }

                        cm.CommandText = query; // Set the SQL command text

                        SqlDataAdapter da = new SqlDataAdapter(cm);
                        DataTable dt = new DataTable();
                        da.Fill(dt); // Fill the DataTable with data from the database
                        dgvInventoryView.DataSource = dt; // Bind the DataTable to the DataGridView
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message); // Show any errors that occur
                    }
                }
            }
        }

        // Event handler for the Dashboard button
        private void btnDashboard_Click(object sender, EventArgs e)
        {
            dashboardForm d = new dashboardForm();
            this.Hide(); // Hide current form
            d.ShowDialog(); // Show the dashboard form
        }

        // Event handler for the Update Inventory button
        private void btnUpdateInventory_Click(object sender, EventArgs e)
        {
            updateInventoryForm ui = new updateInventoryForm();
            this.Hide(); // Hide current form
            ui.ShowDialog(); // Show the update inventory form
        }

        // Event handler for the Invoice Logs button
        private void btnInvoiceLogs_Click(object sender, EventArgs e)
        {
            invoiceLogsForm il = new invoiceLogsForm();
            this.Hide(); // Hide current form
            il.ShowDialog(); // Show the invoice logs form
        }

        // Event handler for the Manage Account button
        private void btnManageAccount_Click(object sender, EventArgs e)
        {
            manageAccountForm ma = new manageAccountForm();
            this.Hide(); // Hide current form
            ma.ShowDialog(); // Show the manage account form
        }

        // Event handler for when the sorting option is changed
        private void cbSortBy_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            string selectedSortOrder = cbSortBy.SelectedItem.ToString(); // Get selected sort order
            LoadData(selectedSortOrder); // Reload data with the new sort order
        }

        // Event handler for the Logout button
        private void btnLogout_Click(object sender, EventArgs e)
        {
            // Confirm logout action with the user
            DialogResult result = MessageBox.Show(
                "Are you sure you want to log out?",
                "Logout Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit(); // Close the entire application
            }
        }
    }
}