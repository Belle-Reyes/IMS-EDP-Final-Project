(manageAccountForm_Load)

private void manageAccountForm_Load(object sender, EventArgs e)
{
    int userId = 1; // pakireplace with the logged-in user's ID.
    using (SqlConnection conn = new SqlConnection(connectionString))
    {
        string query = "SELECT FirstName, LastName, Username FROM Users WHERE UserID = @UserID";
        SqlCommand cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@UserID", userId);

        try
        {
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                txtFirstName.Text = reader["FirstName"].ToString();
                txtLastName.Text = reader["LastName"].ToString();
                txtUsername.Text = reader["Username"].ToString();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error loading account details: " + ex.Message);
        }
    }
}
---------------------------------------------------------------------------------------------------------------------------------------------
(editing button)

private void btnEdit_Click(object sender, EventArgs e)
{
    {

        // para sa confirmation to save changes
        DialogResult result = MessageBox.Show("Do you want to save the changes?", "Confirm Save", MessageBoxButtons.YesNo);
        if (result == DialogResult.Yes)
        {
            int userId = 1; // pakireplace with the logged-in user's ID.
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE Users SET FirstName = @FirstName, LastName = @LastName, Username = @Username WHERE UserID = @UserID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                cmd.Parameters.AddWithValue("@LastName", txtLastName.Text);
                cmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                cmd.Parameters.AddWithValue("@UserID", userId);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Account details updated successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating account details: " + ex.Message);
                }

            }
        }

    }
}
---------------------------------------------------------------------------------------------------------------------------------------------
(Change Password button)

changePasswordForm cpForm = new changePasswordForm();
    cpForm.ShowDialog();
