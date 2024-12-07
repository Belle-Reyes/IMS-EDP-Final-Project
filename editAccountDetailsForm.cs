using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace InventoryManagementSystem_GloriousSole
{
    public partial class editAccountDetailsForm : Form
    {
        // Connection string to connect to the local database
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\reyes\Downloads\Ims + Desing\IMSDesign-SISON\InventoryManagementSystem-GloriousSole\SQL InventoryManagementSystem-GloriousSole\GS_IMS.mdf"";Integrated Security=True;Connect Timeout=30";

        public editAccountDetailsForm()
        {
            InitializeComponent(); // Initialize form components
        }

        // Event handler for the Apply Changes button
        private void btnApplyChanges_Click(object sender, EventArgs e)
        {
            // Confirm with the user if they want to save changes
            DialogResult result = MessageBox.Show("Do you want to save the changes?", "Confirm Save", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                // Input validation to ensure all fields are filled
                if (string.IsNullOrWhiteSpace(txtNewFirstName.Text) ||
                    string.IsNullOrWhiteSpace(txtNewLastName.Text) ||
                    string.IsNullOrWhiteSpace(txtNewUsername.Text))
                {
                    MessageBox.Show("Please fill in all fields.");
                    return; // Exit if validation fails
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // SQL query to update user account details
                    string updateQuery = "UPDATE Users SET FirstName = @FirstName, LastName = @LastName, Username = @Username WHERE UserID = @UserID";
                    using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                    {
                        // Add parameters to the SQL command to prevent SQL injection
                        cmd.Parameters.AddWithValue("@FirstName", txtNewFirstName.Text);
                        cmd.Parameters.AddWithValue("@LastName", txtNewLastName.Text);
                        cmd.Parameters.AddWithValue("@Username", txtNewUsername.Text);
                        cmd.Parameters.AddWithValue("@UserID", UserSession.UserId); // Get the current user's ID

                        try
                        {
                            conn.Open(); // Open the database connection
                            int rowsAffected = cmd.ExecuteNonQuery(); // Execute the update command
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Account details updated successfully!"); // Success message
                                manageAccountForm ma = new manageAccountForm();
                                this.Hide(); // Hide current form
                                ma.ShowDialog(); // Show the manage account form
                            }
                            else
                            {
                                MessageBox.Show("No accounts were updated. Please check your input."); // Error message if no rows affected
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error updating account details: " + ex.Message); // Show any errors
                        }
                    }
                }
            }
        }

        // Event handler for the Cancel button
        private void btnCancel_Click(object sender, EventArgs e)
        {
            manageAccountForm ma = new manageAccountForm();
            this.Hide(); // Hide current form
            ma.ShowDialog(); // Show the manage account form
            txtNewFirstName.Clear(); // Clear the first name field
            txtNewLastName.Clear(); // Clear the last name field
        }
    }
}