
using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace MitoPlayer_2024.Dao
{
    public class ProfileDao : BaseDao, IProfileDao
    {
        public ProfileDao(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public override int GetNextId(String tableName)
        {
            int lastId = -1;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT Id 
                                        FROM " + tableName + " " +
                                        "ORDER BY Id " +
                                        "desc LIMIT 1";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lastId = (int)reader[0];
                    }
                }
                connection.Close();
            }
            return lastId + 1;
        }
        public ResultOrError CreateProfile(Profile profile)
        {
            ResultOrError result = new ResultOrError();
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"INSERT INTO Profile 
                                        values ( 
                                        @Id, 
                                        @Name, 
                                        @IsActive)";

                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = profile.Id;
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = profile.Name;
                command.Parameters.Add("@IsActive", MySqlDbType.Bit).Value = profile.IsActive;
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    result.AddError("Profile [" + profile.Name + "] is not inserted. \n" + ex.Message);
                }
                connection.Close();
            }
            return result;
        }

        public Profile GetActiveProfile()
        {
            Profile profile = null;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT * 
                                        FROM Profile 
                                        WHERE IsActive = true ";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        profile = new Profile();
                        profile.Id = (int)reader[0];
                        profile.Name = (string)reader[1];
                        profile.IsActive = true;
                        break;
                    }
                }
                connection.Close();
            }
            return profile;
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
                command.CommandText = @"SELECT * 
                                        FROM Profile 
                                        WHERE Id = @Id ";

                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = id;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        profile = new Profile();
                        profile.Id = (int)reader[0];
                        profile.Name = (string)reader[1];
                        profile.IsActive = Convert.ToBoolean(reader[2]);
                    }
                }
                connection.Close();
            }
            return profile;
        }
        public Profile GetProfileByName(String name)
        {
            Profile profile = null;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT * 
                                        FROM Profile 
                                        WHERE Name = @Name ";

                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        profile = new Profile();
                        profile.Id = (int)reader[0];
                        profile.Name = (string)reader[1];
                        profile.IsActive = Convert.ToBoolean(reader[2]);
                    }
                }
                connection.Close();
            }
            return profile;
        }
        public List<Profile> GetAllProfile()
        {
            List<Profile> profileList = null;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT * 
                                        FROM Profile ";

                using (var reader = command.ExecuteReader())
                {
                    profileList = new List<Profile>();
                    while (reader.Read())
                    {
                        Profile profile = new Profile();
                        profile.Id = (int)reader[0];
                        profile.Name = (string)reader[1];
                        profile.IsActive = Convert.ToBoolean(reader[2]);
                        profileList.Add(profile);
                    }
                }
                connection.Close();
            }
            return profileList;
        }

        public void UpdateProfile(Profile profile)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"UPDATE Profile 
                                        SET 
                                        Name = @Name, 
                                        IsActive = @IsActive 
                                        WHERE Id = @Id ";

                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = profile.Id;
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = profile.Name;
                command.Parameters.Add("@IsActive", MySqlDbType.Bit).Value = profile.IsActive;
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Profile is not updated. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }


        public void DeleteProfile(int id)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"DELETE FROM Profile 
                                        WHERE Id = @Id ";

                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = id;
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Profile is not deleted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        public void ClearProfileTable()
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "DELETE FROM Profile ";

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Profile is not deleted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }


    }
}
