using MitoPlayer_2024.Model;
using MitoPlayer_2024.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace MitoPlayer_2024.Dao
{
    public class TagValueDao : BaseDao, ITagValueDao
    {
        private int profileId = -1;
        public TagValueDao(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public void SetProfileId(int profileId)
        {
            this.profileId = profileId;
        }
        public List<Tag> GetAllTag()
        {
            List<Tag> result = null;
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT * FROM Tag WHERE ProfileId = @ProfileId ";
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                using (var reader = command.ExecuteReader())
                {
                    result = new List<Tag>();
                    while (reader.Read())
                    {
                        Tag tag = new Tag();
                        tag.Id = (int)reader[0];
                        tag.Name = (string)reader[1];
                        tag.ProfileId = (int)reader[2];
                        result.Add(tag);
                    }
                }
            }
            return result;
        }
        public Tag GetTag(int id)
        {
            Tag result = null;
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT * FROM Tag WHERE Id = @Id AND ProfileId = @ProfileId ";
                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = id;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = new Tag();
                        result.Id = (int)reader[0];
                        result.Name = (string)reader[1];
                        result.ProfileId = (int)reader[2];
                        break;
                    }
                }
            }
            return result;
        }
        public List<TagValue> GetTagValuesByTagId(int tagId)
        {
            List<TagValue> result = null;
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT * FROM TagValue WHERE TagId = @TagId AND ProfileId = @ProfileId ";
                command.Parameters.Add("@TagId", MySqlDbType.Int32).Value = tagId;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                using (var reader = command.ExecuteReader())
                {
                    result = new List<TagValue>();
                    while (reader.Read())
                    {
                        TagValue tagValue = new TagValue();
                        tagValue.Id = (int)reader[0];
                        tagValue.Name = (string)reader[1];
                        tagValue.ProfileId = (int)reader[2];
                        result.Add(tagValue);
                    }
                }
            }
            return result;
        }
        public TagValue GetTagValueByTagIdAndTagValueId(int tagId, int id)
        {
            TagValue result = null;
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT * FROM TagValue WHERE Id = @Id AND TagId = @TagId AND ProfileId = @ProfileId ";
                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = id;
                command.Parameters.Add("@TagId", MySqlDbType.Int32).Value = tagId;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = new TagValue();
                        result.Id = (int)reader[0];
                        result.Name = reader[1].ToString();
                        result.ProfileId = (int)reader[2];
                        break;
                    }
                }
            }
            return result;
        }
        
        public void CreateTag(Tag tag)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "INSERT INTO Tag values (@Id, @Name, @ProfileId)";
                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = tag.Id;
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = tag.Name;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Tag is not inserted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        public void UpdateTag(Tag tag)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"UPDATE Tag SET Name = @Name WHERE Id = @Id AND ProfileId = @ProfileId";
                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = tag.Id;
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = tag.Name;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Tag is not updated. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        public Tag GetTagByName(String name)
        {
            Tag result = null;
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT * FROM Tag WHERE Name = @Name AND ProfileId = @ProfileId ";
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = new Tag();
                        result.Id = (int)reader[0];
                        result.Name = reader[1].ToString();
                        result.ProfileId = (int)reader[2];
                        break;
                    }
                }
            }
            return result;
        }
        public void DeleteTag(int id)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "DELETE FROM Tag WHERE Id = @Id AND ProfileId = @ProfileId";
                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = id;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Object is not deleted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "DELETE FROM TagValue WHERE TagId = @TagId AND ProfileId = @ProfileId";
                command.Parameters.Add("@TagId", MySqlDbType.Int32).Value = id;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Object is not deleted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        public int GetLastObjectId(String tableName)
        {
            int lastId = -1;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT Id FROM " + tableName + " ORDER BY Id desc LIMIT 1";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lastId = (int)reader[0];
                    }
                }
            }
            return lastId;
        }
        public void CreateTagValue(int tagId, TagValue tagValue)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "INSERT INTO TagValue values (@Id, @Name, @TagId, @ProfileId)";
                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = tagValue.Id;
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = tagValue.Name;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                command.Parameters.Add("@TagId", MySqlDbType.Int32).Value = tagId;
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("TagValue is not inserted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        public TagValue GetTagValueByNameAndTagId(int tagId, string stringField1)
        {
            TagValue result = null;
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT * FROM TagValue WHERE TagId = @TagId AND Name = @Name AND ProfileId = @ProfileId ";
                command.Parameters.Add("@TagId", MySqlDbType.Int32).Value = tagId;
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = stringField1;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = new TagValue();
                        result.Id = (int)reader[0];
                        result.Name = reader[1].ToString();
                        result.ProfileId = (int)reader[2];
                        break;
                    }
                }
            }
            return result;
        }
        public void UpdateTagValue(TagValue tagValue)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"UPDATE TagValue SET Name = @Name WHERE Id = @Id AND ProfileId = @ProfileId";
                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = tagValue.Id;
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = tagValue.Name;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("TagValue is not updated. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        public void DeleteTagValue(int id)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "DELETE FROM TagValue WHERE Id = @Id AND ProfileId = @ProfileId";
                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = id;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Object is not deleted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }


    }
}
