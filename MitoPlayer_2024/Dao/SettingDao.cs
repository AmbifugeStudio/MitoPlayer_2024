using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Models;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
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
        public void CreateStringSetting(String name, String value, bool withoutProfile = false)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;

                if (!withoutProfile)
                {
                    command.CommandText = "INSERT INTO Setting (Name, StringValue, ProfileId) VALUES (@Name, @StringValue, @ProfileId)";
                    command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                    command.Parameters.Add("@StringValue", MySqlDbType.VarChar).Value = value;
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.ProfileId;
                }
                else
                {
                    command.CommandText = "INSERT INTO Setting (Name, StringValue) VALUES (@Name, @StringValue)";
                    command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                    command.Parameters.Add("@StringValue", MySqlDbType.VarChar).Value = value;
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
        public void CreateIntegerSetting(String name, Int32 value, bool withoutProfile = false)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                
                if (!withoutProfile)
                {
                    command.CommandText = "INSERT INTO Setting (Name, IntegerValue, ProfileId) VALUES (@Name, @IntegerValue, @ProfileId)";
                    command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                    command.Parameters.Add("@IntegerValue", MySqlDbType.Int32).Value = value;
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.ProfileId;
                }
                else
                {
                    command.CommandText = "INSERT INTO Setting (Name, IntegerValue) VALUES (@Name, @IntegerValue)";
                    command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                    command.Parameters.Add("@IntegerValue", MySqlDbType.Int32).Value = value;
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
        public void CreateDecimalSetting(String name, Decimal value, bool withoutProfile = false)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;

                if (!withoutProfile)
                {
                    command.CommandText = "INSERT INTO Setting (Name, DecimalValue, ProfileId) VALUES (@Name, @DecimalValue, @ProfileId)";
                    command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                    command.Parameters.Add("@DecimalValue", MySqlDbType.Decimal).Value = value;
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.ProfileId;
                }
                else
                {
                    command.CommandText = "INSERT INTO Setting (Name, DecimalValue) VALUES (@Name, @DecimalValue)";
                    command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                    command.Parameters.Add("@DecimalValue", MySqlDbType.Decimal).Value = value;
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
        public void CreateBooleanSetting(String name, Boolean value, bool withoutProfile = false)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;

                if (!withoutProfile)
                {
                    command.CommandText = "INSERT INTO Setting (Name, BooleanValue, ProfileId) VALUES (@Name, @BooleanValue, @ProfileId)";
                    command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                    command.Parameters.Add("@BooleanValue", MySqlDbType.Bit).Value = value;
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.ProfileId;
                }
                else
                {
                    command.CommandText = "INSERT INTO Setting (Name, BooleanValue) VALUES (@Name, @BooleanValue)";
                    command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                    command.Parameters.Add("@BooleanValue", MySqlDbType.Bit).Value = value;
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
        
        public String GetStringSetting(string name, bool withoutProfile = false, bool intern = true)
        {
            String result = null;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;

                if (!withoutProfile)
                {
                    command.CommandText = "SELECT StringValue FROM Setting WHERE Name = @Name AND ProfileId = @ProfileId";
                    command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.ProfileId;
                }
                else
                {
                    command.CommandText = "SELECT StringValue FROM Setting WHERE Name = @Name ";
                    command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                }
               
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = reader[0].ToString();
                    }
                }
            }

            if (String.IsNullOrEmpty(result) && intern)
            {
                result = System.Configuration.ConfigurationManager.AppSettings[name];
            }

            return result;
        }
        public int GetIntegerSetting(string name, bool withoutProfile = false, bool intern = true)
        {
            int result = -1;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;

                if (!withoutProfile)
                {
                    command.CommandText = "SELECT IntegerValue FROM Setting WHERE Name = @Name AND ProfileId = @ProfileId ";
                    command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.ProfileId;
                }
                else
                {
                    command.CommandText = "SELECT IntegerValue FROM Setting WHERE Name = @Name ";
                    command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                }
                
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = (int)reader[0];
                    }
                }
            }

            if (result == -1 && intern)
            {
                if(name != "CurrentPlaylistId")
                    result = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings[name]);
            }

            return result;
        }
        public decimal GetDecimalSetting(string name, bool withoutProfile = false, bool intern = true)
        {
            decimal result = -1;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;

                if(!withoutProfile)
                {
                    command.CommandText = "SELECT DecimalValue FROM Setting WHERE Name = @Name AND ProfileId = @ProfileId ";
                    command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.ProfileId;
                }
                else
                {
                    command.CommandText = "SELECT DecimalValue FROM Setting WHERE Name = @Name ";
                    command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                }
                
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = (decimal)reader[0];
                    }
                }
            }

            if (result == -1 && intern)
            {
                result = Convert.ToDecimal(System.Configuration.ConfigurationManager.AppSettings[name]);
            }

            return result;
        }
        public bool? GetBooleanSetting(string name, bool withoutProfile = false, bool intern = true)
        {
            bool? result = null;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;

                if (!withoutProfile)
                {
                    command.CommandText = "SELECT BooleanValue FROM Setting WHERE Name = @Name AND ProfileId = @ProfileId ";
                    command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.ProfileId;
                }
                else
                {
                    command.CommandText = "SELECT BooleanValue FROM Setting WHERE Name = @Name ";
                    command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                }

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = (bool)reader[0];
                    }
                }
            }

            if (result == null && intern)
            {
                result = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings[name]);
            }

            return result;
        }
       
        public void SetStringSetting(String name, String value, bool withoutProfile = false)
        {
            String result = GetStringSetting(name, withoutProfile, true);
            if (String.IsNullOrEmpty(result))
            {
                this.CreateStringSetting(name, value, withoutProfile);
            }
            else
            {
                using (var connection = new MySqlConnection(connectionString))
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;

                    if (!withoutProfile)
                    {
                        command.CommandText = @"UPDATE Setting SET StringValue = @StringValue
                                            WHERE Name = @Name AND ProfileId = @ProfileId ";
                        command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                        command.Parameters.Add("@StringValue", MySqlDbType.VarChar).Value = value;
                        command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.ProfileId;
                    }
                    else
                    {
                        command.CommandText = @"UPDATE Setting SET StringValue = @StringValue
                                            WHERE Name = @Name ";
                        command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                        command.Parameters.Add("@StringValue", MySqlDbType.VarChar).Value = value;
                    }

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
        public void SetIntegerSetting(String name, Int32 value, bool withoutProfile = false)
        {
            int result = GetIntegerSetting(name, withoutProfile, true);
            if (result == -1)
            {
                this.CreateIntegerSetting(name, value, withoutProfile);
            }
            else
            {
                using (var connection = new MySqlConnection(connectionString))
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;

                    if (!withoutProfile)
                    {
                        command.CommandText = @"UPDATE Setting SET IntegerValue = @IntegerValue
                                            WHERE Name = @Name AND ProfileId = @ProfileId ";
                        command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                        command.Parameters.Add("@IntegerValue", MySqlDbType.Int32).Value = value;
                        command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.ProfileId;
                    }
                    else
                    {
                        command.CommandText = @"UPDATE Setting SET IntegerValue = @IntegerValue
                                            WHERE Name = @Name ";
                        command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                        command.Parameters.Add("@IntegerValue", MySqlDbType.Int32).Value = value;
                    }

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
        public void SetDecimalSetting(String name, Decimal value, bool withoutProfile = false)
        {
            decimal result = GetDecimalSetting(name, withoutProfile, true);
            if (result == -1)
            {
                this.CreateDecimalSetting(name, value, withoutProfile);
            }
            else
            {
                using (var connection = new MySqlConnection(connectionString))
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;

                    if (!withoutProfile)
                    {
                        command.CommandText = @"UPDATE Setting SET DecimalValue = @DecimalValue
                                            WHERE Name = @Name AND ProfileId = @ProfileId ";
                        command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                        command.Parameters.Add("@DecimalValue", MySqlDbType.Decimal).Value = value;
                        command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.ProfileId;
                    }
                    else
                    {
                        command.CommandText = @"UPDATE Setting SET DecimalValue = @DecimalValue
                                            WHERE Name = @Name ";
                        command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                        command.Parameters.Add("@DecimalValue", MySqlDbType.Decimal).Value = value;
                    }

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
        public void SetBooleanSetting(String name, Boolean value, bool withoutProfile = false)
        {
            bool? result = GetBooleanSetting(name, withoutProfile, true);
            if (result == null)
            {
                this.CreateBooleanSetting(name, value, withoutProfile);
            }
            else
            {
                using (var connection = new MySqlConnection(connectionString))
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;

                    if (!withoutProfile)
                    {
                        command.CommandText = @"UPDATE Setting SET BooleanValue = @BooleanValue
                                            WHERE Name = @Name AND ProfileId = @ProfileId ";
                        command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                        command.Parameters.Add("@BooleanValue", MySqlDbType.Bit).Value = value;
                        command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.ProfileId;
                    }
                    else
                    {
                        command.CommandText = @"UPDATE Setting SET BooleanValue = @BooleanValue
                                            WHERE Name = @Name ";
                        command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                        command.Parameters.Add("@BooleanValue", MySqlDbType.Bit).Value = value;
                    }

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
