
using MitoPlayer_2024.Models;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace MitoPlayer_2024.Dao
{
    public class ProfileDao : BaseDao, IProfileDao
    {
        public int ProfileId = -1;
        public ProfileDao(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public void CreateProfile(Profile profile)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "INSERT INTO Profile values (@Id, @Name)";
                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = profile.Id;
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = profile.Name;
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Profile is not inserted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }

        public Profile GetProfile(int id)
        {
            Profile profile = null;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT * FROM Profile WHERE Id = @Id ";
                command.Parameters.Add("@Id", MySqlDbType.VarChar).Value = id;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        profile = new Profile();
                        profile.Id = (int)reader[0];
                        profile.Name = reader[1].ToString();
                    }
                }
            }
            return profile;
        }
       

    }
}
