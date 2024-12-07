using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;
using static System.Collections.Specialized.BitVector32;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace InventoryManagementSystem_GloriousSole
{
    public partial class updateInventoryForm : Form
    {

        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\reyes\Downloads\Ims + Desing\IMSDesign-SISON\InventoryManagementSystem-GloriousSole\SQL InventoryManagementSystem-GloriousSole\GS_IMS.mdf"";Integrated Security=True;Connect Timeout=30";

        public updateInventoryForm()
        {
            InitializeComponent();
            LoadItems();
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

        private void btnInvoiceLogs_Click(object sender, EventArgs e)
        {
            invoiceLogsForm il = new invoiceLogsForm();
            this.Hide();
            il.ShowDialog();
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

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedItem = cbItemsRemove.Text;
                string quantityText = txtQuantityRemove.Text;

                // Validate the inputs for removal
                if (!ValidateRemovalInputs(selectedItem, quantityText))
                {
                    return;
                }
                string[] itemDetails = selectedItem.Split(new[] { " - " }, StringSplitOptions.None);
                string brand = itemDetails[0];
                string model = itemDetails[1];
                string item = $"{brand} - {model}";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = @"UPDATE Inventory SET Quantity = Quantity - @Quantity 
                                     WHERE Brand = @Brand AND Model = @Model AND Quantity >= @Quantity";
                    using (SqlCommand cm = new SqlCommand(query, con))
                    {
                        cm.Parameters.AddWithValue("@Brand", brand);
                        cm.Parameters.AddWithValue("@Model", model);
                        cm.Parameters.AddWithValue("@Quantity", int.Parse(quantityText));

                        int result = cm.ExecuteNonQuery();

                        if (result > 0)
                        {
                            LogUserAction("Removed item", item, "Approved");
                            ConfirmChanges();
                            LoadItems();

                            txtQuantityRemove.Clear();
                            cbItemsRemove.SelectedIndex = 0;
                        }
                        else
                        {
                            MessageBox.Show("Not enough items in stock or item not found.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbItemUpdate.SelectedItem == null)
                {
                    MessageBox.Show("Please select an item to update.");
                    return;
                }

                string selectedItem = cbItemUpdate.SelectedItem.ToString();
                string[] parts = selectedItem.Split(new[] { " - " }, StringSplitOptions.None);
                string brand = parts[0];
                string model = parts[1];
                string item = $"{brand} - {model}";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "UPDATE Inventory SET Size = @Size, Quantity = @Quantity, PricePerPiece = @PricePerPiece WHERE Brand = @Brand AND Model = @Model";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Size", string.IsNullOrWhiteSpace(txtSizeUpdate.Text) ? (object)DBNull.Value : decimal.Parse(txtSizeUpdate.Text));
                        cmd.Parameters.AddWithValue("@Quantity", string.IsNullOrWhiteSpace(txtQuantityUpdate.Text) ? (object)DBNull.Value : int.Parse(txtQuantityUpdate.Text));
                        cmd.Parameters.AddWithValue("@PricePerPiece", string.IsNullOrWhiteSpace(txtPriceUpdate.Text) ? (object)DBNull.Value : decimal.Parse(txtPriceUpdate.Text));
                        cmd.Parameters.AddWithValue("@Brand", brand);
                        cmd.Parameters.AddWithValue("@Model", model);

                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            LogUserAction($"Updated item", item , "Approved");
                            ConfirmChanges();
                            LoadItems();

                            cbItemUpdate.SelectedIndex = -1;
                            txtBrandUpdate.Clear();
                            txtModelUpdate.Clear();
                            txtSizeUpdate.Clear();
                            txtQuantityUpdate.Clear();
                            txtPriceUpdate.Clear();
                        }
                        else
                        {
                            MessageBox.Show("Failed to update item. Please check your input.");
                        }
                    }
                }    
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateItemInputs(txtBrandAdd.Text, txtModelAdd.Text, txtSizeAdd.Text, txtQuantityAdd.Text, txtPriceAdd.Text))
                {
                    return;
                }

                string item = $"{txtBrandAdd.Text} - {txtModelAdd.Text}";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = @"INSERT INTO Inventory (Brand, Model, Size, Quantity, PricePerPiece) 
                             VALUES (@Brand, @Model, @Size, @Quantity, @PricePerPiece)";

                    using (SqlCommand cm = new SqlCommand(query, con))
                    {
                        cm.Parameters.AddWithValue("@Brand", txtBrandAdd.Text);
                        cm.Parameters.AddWithValue("@Model", txtModelAdd.Text);
                        cm.Parameters.AddWithValue("@Size", decimal.Parse(txtSizeAdd.Text));
                        cm.Parameters.AddWithValue("@Quantity", int.Parse(txtQuantityAdd.Text));
                        cm.Parameters.AddWithValue("@PricePerPiece", decimal.Parse(txtPriceAdd.Text));

                        int result = cm.ExecuteNonQuery();

                        if (result > 0)
                        {
                            LogUserAction("Added item", item, "Approved");
                            ConfirmChanges();

                            txtBrandAdd.Clear();
                            txtModelAdd.Clear();
                            txtSizeAdd.Clear();
                            txtQuantityAdd.Clear();
                            txtPriceAdd.Clear();
                        }
                        else
                        {
                            MessageBox.Show("Failed to add item. Please check your input.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void LogUserAction(string action, string item, string status)
        {
           try
    {
        string username = UserSession.Username;
        string fullname = UserSession.FullName;
        string role = UserSession.Role;

        string query = @"INSERT INTO Logs (FullName, Username, Role, Action, LogDate, Item, Status) 
                         VALUES (@FullName, @Username, @Role, @Action, @LogDate, @Item, @Status)";

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            using (SqlCommand cm = new SqlCommand(query, con))
            {
                cm.Parameters.AddWithValue("@Item", item);
                cm.Parameters.AddWithValue("@Username", username);
                cm.Parameters.AddWithValue("@FullName", fullname);
                cm.Parameters.AddWithValue("@Role", role);
                cm.Parameters.AddWithValue("@Action", action);
                cm.Parameters.AddWithValue("@LogDate", DateTime.Now);
                cm.Parameters.AddWithValue("@Status", status);

                con.Open();
                cm.ExecuteNonQuery();
            }
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show("Error logging user action: " + ex.Message);
    }
        }

        private bool ValidateItemInputs(string brand, string model, string size, string quantity, string price)
        {
            string lettersOnlyPattern = @"^[A-Za-z\s]+$";
            string alphanumericWithSpacesPattern = @"^[A-Za-z0-9\s]+$"; // Updated pattern for model
            string numbersOnlyPattern = @"^\d+(\.\d+)?$";

            // Check for empty fields
            if (string.IsNullOrWhiteSpace(brand) ||
                string.IsNullOrWhiteSpace(model) ||
                string.IsNullOrWhiteSpace(size) ||
                string.IsNullOrWhiteSpace(quantity) ||
                string.IsNullOrWhiteSpace(price))
            {
                MessageBox.Show("Please fill out all fields before proceeding.");
                return false;
            }

            // Validate brand
            if (!System.Text.RegularExpressions.Regex.IsMatch(brand, alphanumericWithSpacesPattern))
            {
                MessageBox.Show("Brand must contain only letters, numbers, and spaces.");
                return false;
            }

            // Validate model
            if (!System.Text.RegularExpressions.Regex.IsMatch(model, alphanumericWithSpacesPattern))
            {
                MessageBox.Show("Model must contain only letters, numbers, and spaces.");
                return false;
            }

            // Validate size
            if (!System.Text.RegularExpressions.Regex.IsMatch(size, numbersOnlyPattern))
            {
                MessageBox.Show("Size must contain only numbers or decimals.");
                return false;
            }

            // Validate quantity
            if (!System.Text.RegularExpressions.Regex.IsMatch(quantity, numbersOnlyPattern))
            {
                MessageBox.Show("Quantity must contain only numbers.");
                return false;
            }

            // Validate price
            if (!System.Text.RegularExpressions.Regex.IsMatch(price, numbersOnlyPattern))
            {
                MessageBox.Show("Price must contain only numbers or decimals.");
                return false;
            }

            return true; // All validations passed
        }

        private bool ValidateRemovalInputs(string selectedItem, string quantityText)
        {
            // Check if an item is selected
            if (string.IsNullOrEmpty(selectedItem))
            {
                MessageBox.Show("Please select an item to remove.");
                return false;
            }

            // Check if the quantity textbox is empty
            if (string.IsNullOrWhiteSpace(quantityText))
            {
                MessageBox.Show("Quantity is empty. Please fill it.");
                return false;
            }

            // Validate that the quantity is a valid number
            if (!int.TryParse(quantityText, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Please enter a valid quantity greater than zero.");
                return false;
            }

            return true; // All validations passed
        }

        private void LoadItems()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "SELECT DISTINCT Brand, Model FROM Inventory";
                    using (SqlCommand cm = new SqlCommand(query, con))
                    {
                        using (SqlDataReader dr = cm.ExecuteReader())
                        {
                            cbItemsRemove.Items.Clear();
                            cbItemUpdate.Items.Clear();

                            while (dr.Read())
                            {
                                string brand = dr["Brand"].ToString();
                                string model = dr["Model"].ToString();

                                if (!string.IsNullOrEmpty(brand) && !string.IsNullOrEmpty(model))
                                {
                                    cbItemsRemove.Items.Add($"{brand} - {model}");
                                    cbItemUpdate.Items.Add($"{brand} - {model}");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading items: " + ex.Message);
            }
        }

        private void cbItemUpdate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbItemUpdate.SelectedItem != null)
            {
                string selectedItem = cbItemUpdate.SelectedItem.ToString();
                string[] parts = selectedItem.Split(new[] { " - " }, StringSplitOptions.None);
                if (parts.Length == 2)
                {
                    LoadItemDetails(parts[0], parts[1]);
                }
            }
        }
            private void LoadItemDetails(string brand, string model)
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "SELECT Size, Quantity, Price FROM Inventory WHERE Brand = @Brand AND Model = @Model";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Brand", brand);
                        cmd.Parameters.AddWithValue("@Model", model);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtBrandUpdate.Text = brand;
                                txtModelUpdate.Text = model;
                                txtSizeUpdate.Text = reader["Size"].ToString();
                                txtQuantityUpdate.Text = reader["Quantity"].ToString();
                                txtPriceUpdate.Text = reader["Price"].ToString();
                            }
                        }
                    }
                }
            }
        private void ConfirmChanges()
        {
            if (UserSession.Role == "Admin")
    {
                MessageBox.Show("Changes saved successfully.");
            }
    else
            {
                actioncommitConfirmation aCC = new actioncommitConfirmation();
                this.Hide();
                aCC.ShowDialog();
            }
        }
    }
    }