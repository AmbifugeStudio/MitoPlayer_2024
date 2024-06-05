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
    public class TagDao : BaseDao, ITagDao
    {
        private int profileId = -1;
        public TagDao(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public void SetProfileId(int profileId)
        {
            this.profileId = profileId;
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
        public void CreateTag(Tag tag)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"INSERT INTO Tag values ( 
                                        @Id, 
                                        @Name, 
                                        @ProfileId)";

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

        public Tag GetTag(int id)
        {
            Tag tag = null;
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT * FROM Tag 
                                        WHERE Id = @Id 
                                        AND ProfileId = @ProfileId ";

                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = id;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tag = new Tag();
                        tag.Id = (int)reader[0];
                        tag.Name = (string)reader[1];
                        tag.ProfileId = (int)reader[2];
                        break;
                    }
                }
                connection.Close();
            }
            return tag;
        }
        public Tag GetTagByName(String name)
        {
            Tag tag = null;
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT * FROM Tag 
                                        WHERE Name = @Name 
                                        AND ProfileId = @ProfileId ";
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tag = new Tag();
                        tag.Id = (int)reader[0];
                        tag.Name = reader[1].ToString();
                        tag.ProfileId = (int)reader[2];
                        break;
                    }
                }
                connection.Close();
            }
            return tag;
        }
        public List<Tag> GetAllTag()
        {
            List<Tag> tagList = null;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT * FROM Tag 
                                        WHERE ProfileId = @ProfileId ";

                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

                using (var reader = command.ExecuteReader())
                {
                    tagList = new List<Tag>();
                    while (reader.Read())
                    {
                        Tag tag = new Tag();
                        tag.Id = (int)reader[0];
                        tag.Name = (string)reader[1];
                        tag.ProfileId = (int)reader[2];
                        tagList.Add(tag);
                    }
                }
                connection.Close();
            }
            return tagList;
        }

        public void UpdateTag(Tag tag)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"UPDATE Tag 
                                        SET Name = @Name 
                                        WHERE Id = @Id 
                                        AND ProfileId = @ProfileId";
                
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

        public void DeleteTag(int id)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"DELETE FROM Tag 
                                        WHERE Id = @Id 
                                        AND ProfileId = @ProfileId";

                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = id;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Tag is not deleted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        public void DeleteAllTag()
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"DELETE FROM Tag 
                                        WHERE ProfileId = @ProfileId";
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Tag is not deleted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }

            this.DeleteAllTagValue();
        }
        public void ClearTagTable()
        {
            this.ClearTagValueTable();

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "DELETE FROM Tag ";

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Tag is not deleted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }

        public void CreateTagValue(TagValue tagValue)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"INSERT INTO TagValue VALUES ( 
                                        @Id, 
                                        @Name, 
                                        @TagId, 
                                        @ProfileId)";

                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = tagValue.Id;
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = tagValue.Name;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                command.Parameters.Add("@TagId", MySqlDbType.Int32).Value = tagValue.TagId;
                
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
        public List<TagValue> GetTagValuesByTagId(int tagId)
        {
            List<TagValue> tagValueList = null;
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT * FROM TagValue
                                        WHERE TagId = @TagId 
                                        AND ProfileId = @ProfileId ";

                command.Parameters.Add("@TagId", MySqlDbType.Int32).Value = tagId;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

                using (var reader = command.ExecuteReader())
                {
                    tagValueList = new List<TagValue>();
                    while (reader.Read())
                    {
                        TagValue tagValue = new TagValue();
                        tagValue.Id = (int)reader[0];
                        tagValue.Name = (string)reader[1];
                        tagValue.ProfileId = (int)reader[2];
                        tagValueList.Add(tagValue);
                    }
                }
                connection.Close();
            }
            return tagValueList;
        }
        public TagValue GetTagValue(int id)
        {
            TagValue tagValue = null;
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT * FROM TagValue 
                                        WHERE Id = @Id 
                                        AND ProfileId = @ProfileId ";

                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = id;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tagValue = new TagValue();
                        tagValue.Id = (int)reader[0];
                        tagValue.Name = reader[1].ToString();
                        tagValue.ProfileId = (int)reader[2];
                        break;
                    }
                }
                connection.Close();
            }
            return tagValue;
        }
        public TagValue GetTagValueByTagId(int id, int tagId)
        {
            TagValue tagValue = null;
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT * FROM TagValue 
                                        WHERE Id = @Id 
                                        AND TagId = @TagId 
                                        AND ProfileId = @ProfileId ";

                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = id;
                command.Parameters.Add("@TagId", MySqlDbType.Int32).Value = tagId;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tagValue = new TagValue();
                        tagValue.Id = (int)reader[0];
                        tagValue.Name = reader[1].ToString();
                        tagValue.ProfileId = (int)reader[2];
                        break;
                    }
                }
                connection.Close();
            }
            return tagValue;
        }
        public TagValue GetTagValueByName(int tagId, String name)
        {
            TagValue tagValue = null;
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT * FROM TagValue 
                                        WHERE Name = @Name 
                                        AND TagId = @TagId 
                                        AND ProfileId = @ProfileId ";

                command.Parameters.Add("@TagId", MySqlDbType.Int32).Value = tagId;
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tagValue = new TagValue();
                        tagValue.Id = (int)reader[0];
                        tagValue.Name = reader[1].ToString();
                        tagValue.ProfileId = (int)reader[2];
                        break;
                    }
                }
                connection.Close();
            }
            return tagValue;
        }

        public void UpdateTagValue(TagValue tagValue)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"UPDATE TagValue 
                                        SET Name = @Name 
                                        WHERE Id = @Id 
                                        AND ProfileId = @ProfileId";

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
                command.CommandText = @"DELETE FROM TagValue 
                                        WHERE Id = @Id 
                                        AND ProfileId = @ProfileId";

                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = id;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("TagValue is not deleted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        public void DeleteTagValuesByTagId(int tagId)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"DELETE FROM TagValue 
                                        WHERE TagId = @TagId 
                                        AND ProfileId = @ProfileId";

                command.Parameters.Add("@TagId", MySqlDbType.Int32).Value = tagId;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("TagValue is not deleted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        public void DeleteAllTagValue()
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"DELETE FROM TagValue 
                                        WHERE ProfileId = @ProfileId";
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("TagValue is not deleted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        public void ClearTagValueTable()
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "DELETE FROM TagValue ";

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("TagValue is not deleted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
    }
}
