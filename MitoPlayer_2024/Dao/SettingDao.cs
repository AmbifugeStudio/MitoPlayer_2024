using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace MitoPlayer_2024.Dao
{
    public class SettingDao : BaseDao, ISettingDao
    {
        public int ProfileId = -1;
        public SettingDao(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public void CreateStringSetting(String name, String value)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "INSERT INTO Setting (Name, StringValue) VALUES (@Name, @StringValue, @ProfileId)";
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                command.Parameters.Add("@StringValue", MySqlDbType.VarChar).Value = value;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.ProfileId;
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Object is not inserted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        public void CreateIntegerSetting(String name, Int32 value, bool notRelatedToProfile = false)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                
                if (notRelatedToProfile)
                {
                    command.CommandText = "INSERT INTO Setting (Name, IntegerValue, ProfileId) VALUES (@Name, @IntegerValue, @ProfileId)";
                    command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                    command.Parameters.Add("@IntegerValue", MySqlDbType.Int32).Value = value;
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = -1;
                }
                else
                {
                    command.CommandText = "INSERT INTO Setting (Name, IntegerValue, ProfileId) VALUES (@Name, @IntegerValue, @ProfileId)";
                    command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                    command.Parameters.Add("@IntegerValue", MySqlDbType.Int32).Value = value;
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.ProfileId;
                }

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Object is not inserted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        public void CreateDecimalSetting(String name, Decimal value)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "INSERT INTO Setting (Name, DecimalValue) VALUES (@Name, @DecimalValue, @ProfileId)";
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                command.Parameters.Add("@DecimalValue", MySqlDbType.Decimal).Value = value;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.ProfileId;
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Object is not inserted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        public void CreateBooleanSetting(String name, Boolean value)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "INSERT INTO Setting (Name, BooleanValue) VALUES (@Name, @BooleanValue, @ProfileId)";
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                command.Parameters.Add("@BooleanValue", MySqlDbType.Bit).Value = value;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.ProfileId;
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Object is not inserted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        public String GetStringSettingByName(string name, bool external)
        {
            String result = null;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT StringValue FROM Setting WHERE Name = @Name AND ProfileId = @ProfileId";
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.ProfileId;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = reader[0].ToString();
                    }
                }
            }

            if (String.IsNullOrEmpty(result) && external)
            {
                result = System.Configuration.ConfigurationManager.AppSettings[name];
            }

            return result;
        }
        public int GetIntegerSettingByName(string name, bool external, bool notRelatedToProfile = false)
        {
            int result = -1;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;

                if (notRelatedToProfile)
                {
                    command.CommandText = "SELECT IntegerValue FROM Setting WHERE Name = @Name AND ProfileId = @ProfileId ";
                    command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = -1;
                }
                else
                {
                    command.CommandText = "SELECT IntegerValue FROM Setting WHERE Name = @Name AND ProfileId = @ProfileId ";
                    command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.ProfileId;
                }
                
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = (int)reader[0];
                    }
                }
            }

            if (result == -1 && external)
            {
                result = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings[name]);
            }

            return result;
        }
        public decimal GetDecimalSettingByName(string name, bool external)
        {
            decimal result = -1;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT DecimalValue FROM Setting WHERE Name = @Name AND ProfileId = @ProfileId ";
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.ProfileId;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = (decimal)reader[0];
                    }
                }
            }

            if (result == -1 && external)
            {
                result = Convert.ToDecimal(System.Configuration.ConfigurationManager.AppSettings[name]);
            }

            return result;
        }
        public bool? GetBooleanSettingByName(string name, bool external)
        {
            bool? result = null;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT BooleanValue FROM Setting WHERE Name = @Name AND ProfileId = @ProfileId ";
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.ProfileId;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = (bool)reader[0];
                    }
                }
            }

            if (result == null && external)
            {
                result = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings[name]);
            }

            return result;
        }
        public void SetStringSetting(String name, String value)
        {
            String result = GetStringSettingByName(name, false);
            if (String.IsNullOrEmpty(result))
            {
                this.CreateStringSetting(name, value);
            }
            else
            {
                using (var connection = new MySqlConnection(connectionString))
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"UPDATE Setting 
                                            SET StringValue = @StringValue
                                            WHERE Name = @Name AND ProfileId = @ProfileId ";
                    command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                    command.Parameters.Add("@StringValue", MySqlDbType.VarChar).Value = value;
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.ProfileId;

                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show("Object is not updated. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    connection.Close();
                }
            }

            
        }
        public void SetIntegerSetting(String name, Int32 value)
        {
            int result = GetIntegerSettingByName(name, false);
            if (result == -1)
            {
                this.CreateIntegerSetting(name, value);
            }
            else
            {
                using (var connection = new MySqlConnection(connectionString))
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"UPDATE Setting 
                                            SET IntegerValue = @IntegerValue
                                            WHERE Name = @Name AND ProfileId = @ProfileId ";
                    command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                    command.Parameters.Add("@IntegerValue", MySqlDbType.Int32).Value = value;
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.ProfileId;

                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show("Object is not updated. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    connection.Close();
                }
            }
            
        }
        public void SetDecimalSetting(String name, Decimal value)
        {
            decimal result = GetDecimalSettingByName(name, false);
            if (result == -1)
            {
                this.CreateDecimalSetting(name, value);
            }
            else
            {
                using (var connection = new MySqlConnection(connectionString))
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"UPDATE Setting 
                                            SET DecimalValue = @DecimalValue
                                            WHERE Name = @Name AND ProfileId = @ProfileId ";
                    command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                    command.Parameters.Add("@DecimalValue", MySqlDbType.Decimal).Value = value;
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.ProfileId;

                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show("Object is not updated. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    connection.Close();
                }
            }
            
        }
        public void SetBooleanSetting(String name, Boolean value)
        {
            bool? result = GetBooleanSettingByName(name, false);
            if (result == null)
            {
                this.CreateBooleanSetting(name, value);
            }
            else
            {
                using (var connection = new MySqlConnection(connectionString))
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"UPDATE Setting 
                                            SET BooleanValue = @BooleanValue
                                            WHERE Name = @Name AND ProfileId = @ProfileId ";
                    command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                    command.Parameters.Add("@BooleanValue", MySqlDbType.Bit).Value = value;
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.ProfileId;

                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show("Object is not updated. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    connection.Close();
                }
            }
            
        }
    }
}
