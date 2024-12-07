using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace InventoryManagementSystem_GloriousSole
{
    public partial class changePasswordForm : Form
    {
        // Connection string to connect to the local database
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\reyes\Downloads\Ims + Desing\IMSDesign-SISON\InventoryManagementSystem-GloriousSole\SQL InventoryManagementSystem-GloriousSole\GS_IMS.mdf"";Integrated Security=True;Connect Timeout=30";
        SqlConnection con; // SQL connection object
        SqlCommand cm; // SQL command object
        SqlDataReader dr; // SQL data reader object

        public changePasswordForm()
        {
            InitializeComponent(); // Initialize form components
            con = new SqlConnection(connectionString); // Create a new SQL connection
            txtNewPassword.PasswordChar = '*'; //Allows the password to be hidden when typing
            txtOldPassword.PasswordChar = '*';
        }

        // Event handler for the Cancel button
        private void btnCancel_Click(object sender, EventArgs e)
        {
            manageAccountForm ma = new manageAccountForm(); // Create instance of manageAccountForm
            this.Hide(); // Hide current form
            ma.ShowDialog(); // Show the manage account form
        }

        // Event handler for the Change Password button
        private void btnChangePassword_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Check if the old password is correct
                cm = new SqlCommand("SELECT * FROM Users WHERE PasswordHash = @oldpassword", con);
                cm.Parameters.AddWithValue("@oldpassword", txtOldPassword.Text);
                con.Open(); // Open the database connection
                dr = cm.ExecuteReader(); // Execute the command and read the result

                if (dr.Read()) // If the old password is found
                {
                    dr.Close(); // Close the data reader

                    // Update the password in the database
                    cm = new SqlCommand("UPDATE Users SET PasswordHash = @newPassword WHERE PasswordHash = @oldPassword", con);
                    cm.Parameters.AddWithValue("@oldPassword", txtOldPassword.Text);
                    cm.Parameters.AddWithValue("@newPassword", txtNewPassword.Text);
                    cm.ExecuteNonQuery(); // Execute the update command

                    MessageBox.Show("Password changed successfully."); // Notify user of success
                    manageAccountForm main = new manageAccountForm(); // Create instance of manageAccountForm
                    this.Hide(); // Hide current form
                    main.ShowDialog(); // Show the manage account form
                }
                else
                {
                    MessageBox.Show("Invalid old password."); // Notify user of invalid old password
                    txtNewPassword.Clear(); // Clear the new password field
                    txtOldPassword.Clear(); // Clear the old password field
                }
                con.Close(); // Close the database connection
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); // Show any errors that occur
            }
        }
    }
}