using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Helpers.ErrorHandling;
using MitoPlayer_2024.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;

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

        #region TAG
        public ResultOrError CreateTag(Tag tag)
        {
            ResultOrError result = new ResultOrError();

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"INSERT INTO Tag (Name, TextColoring, HasMultipleValues, IsIntegrated, OrderInList, ProfileId) 
                                            VALUES (@Name, @TextColoring, @HasMultipleValues, @IsIntegrated, @OrderInList, @ProfileId)";

                    command.Parameters.AddWithValue("@Name", tag.Name ?? "");
                    command.Parameters.AddWithValue("@TextColoring", tag.TextColoring ? 1 : 0);
                    command.Parameters.AddWithValue("@HasMultipleValues", tag.HasMultipleValues ? 1 : 0);
                    command.Parameters.AddWithValue("@IsIntegrated", tag.IsIntegrated ? 1 : 0);
                    command.Parameters.AddWithValue("@OrderInList", tag.OrderInList);
                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex)
            {
                String errorMessage = $"Tag [{tag.Name}] is not inserted. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError<Tag> GetTag(int id)
        {
            ResultOrError<Tag> result = new ResultOrError<Tag>();
            Tag tag = null;

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"SELECT * FROM Tag 
                                        WHERE Id = @Id 
                                        AND ProfileId = @ProfileId ";

                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tag = new Tag
                            {
                                Id = reader.ReadInt("Id"),
                                Name = reader.ReadString("Name"),
                                TextColoring = reader.ReadBool("TextColoring"),
                                HasMultipleValues = reader.ReadBool("HasMultipleValues"),
                                IsIntegrated = reader.ReadBool("IsIntegrated"),
                                OrderInList = reader.ReadInt("OrderInList"),
                                ProfileId = reader.ReadInt("ProfileId")
                            };
                            break;
                        }
                    }
                    connection.Close();
                }
            }
            catch (MySqlException ex)
            {
                String errorMessage = $"Tag [{id}] read error. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            if (tag == null)
                result.AddError($"Tag with ID [{id}] not found.");
            else
                result.Value = tag;

            return result;
        }
        public ResultOrError<Tag> GetTagByName(String name)
        {
            ResultOrError<Tag> result = new ResultOrError<Tag>();
            Tag tag = null;

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"SELECT * FROM Tag 
                                        WHERE Name = @Name 
                                        AND ProfileId = @ProfileId ";

                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tag = new Tag
                            {
                                Id = reader.ReadInt("Id"),
                                Name = reader.ReadString("Name"),
                                TextColoring = reader.ReadBool("TextColoring"),
                                HasMultipleValues = reader.ReadBool("HasMultipleValues"),
                                IsIntegrated = reader.ReadBool("IsIntegrated"),
                                OrderInList = reader.ReadInt("OrderInList"),
                                ProfileId = reader.ReadInt("ProfileId")
                            };
                            break;
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                String errorMessage = $"Tag [{name}] read error. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            if (tag == null)
                result.AddError($"Tag with ID [{name}] not found.");
            else
                result.Value = tag;

            
            return result;
        }
        public ResultOrError<List<Tag>> GetAllTag()
        {
            ResultOrError<List<Tag>> result = new ResultOrError<List<Tag>> { Value = new List<Tag>() };

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"SELECT * FROM Tag WHERE ProfileId = @ProfileId ";

                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Tag tag = new Tag
                            {
                                Id = reader.ReadInt("Id"),
                                Name = reader.ReadString("Name"),
                                TextColoring = reader.ReadBool("TextColoring"),
                                HasMultipleValues = reader.ReadBool("HasMultipleValues"),
                                IsIntegrated = reader.ReadBool("IsIntegrated"),
                                OrderInList = reader.ReadInt("OrderInList"),
                                ProfileId = reader.ReadInt("ProfileId")
                            };
                            result.Value.Add(tag);
                        }
                    }
                    connection.Close();
                }
            }
            catch (MySqlException ex)
            {
                String errorMessage = $"Tag read error. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError UpdateTag(Tag tag)
        {
            ResultOrError result = new ResultOrError();

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"
                        UPDATE Tag 
                        SET 
                            Name = @Name, 
                            TextColoring = @TextColoring, 
                            HasMultipleValues = @HasMultipleValues, 
                            IsIntegrated = @IsIntegrated, 
                            OrderInList = @OrderInList 
                        WHERE 
                            Id = @Id 
                            AND ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@Id", tag.Id);
                    command.Parameters.AddWithValue("@Name", tag.Name ?? "");
                    command.Parameters.AddWithValue("@TextColoring", tag.TextColoring ? 1 : 0);
                    command.Parameters.AddWithValue("@HasMultipleValues", tag.HasMultipleValues ? 1 : 0);
                    command.Parameters.AddWithValue("@IsIntegrated", tag.IsIntegrated ? 1 : 0);
                    command.Parameters.AddWithValue("@OrderInList", tag.OrderInList);
                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex)
            {
                string errorMessage = $"Error occurred while updating tag [{tag.Name}]. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError DeleteTag(int id)
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"
                        DELETE FROM Tag 
                        WHERE Id = @Id 
                        AND ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                string errorMessage = $"Error occurred while deleting tag with ID {id}. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError DeleteAllTag()
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"DELETE FROM Tag 
                                    WHERE ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                // Call DeleteAllTagValue after successful deletion
                this.DeleteAllTagValue();
            }
            catch (SQLiteException ex)
            {
                string errorMessage = $"Error occurred while deleting all tags. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError ClearTagTable()
        {
            var result = new ResultOrError();

            try
            {
                // Clear related table first
                this.ClearTagValueTable();

                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "DELETE FROM Tag";

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                string errorMessage = $"Error occurred while clearing the tag table. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        #endregion

        #region TAGVALUE
        public ResultOrError CreateTagValue(TagValue tagValue)
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"INSERT INTO TagValue (Name, TagId, Color, Hotkey, ProfileId) 
                                            VALUES (@Name, @TagId, @Color, @Hotkey, @ProfileId)";

                    command.Parameters.AddWithValue("@Name", tagValue.Name);
                    command.Parameters.AddWithValue("@TagId", tagValue.TagId);
                    command.Parameters.AddWithValue("@Color", ColorToHex(tagValue.Color));
                    command.Parameters.AddWithValue("@Hotkey", tagValue.Hotkey);
                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                result.AddError($"TagValue [{tagValue.Name}] is not inserted. \n{ex.Message}");
                Logger.Error($"Error occurred while inserting TagValue [{tagValue.Name}].", ex);
            }

            return result;
        }
        public string ColorToHex(Color color)
        {
            return ColorTranslator.ToHtml(color);
        }
        public ResultOrError<List<TagValue>> GetTagValuesByTagId(int tagId)
        {
            var result = new ResultOrError<List<TagValue>> { Value = new List<TagValue>() };

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"SELECT
                                    tv.Id,
                                    tv.Name,
                                    tv.TagId,
                                    t.Name,
                                    tv.Color,
                                    tv.Hotkey,
                                    tv.ProfileId 
                                    FROM TagValue tv, Tag t 
                                    WHERE tv.TagId = t.Id 
                                    AND tv.TagId = @TagId 
                                    AND tv.ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@TagId", tagId);
                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var tagValue = new TagValue
                            {
                                Id = reader.ReadInt("Id"),
                                Name = reader.ReadString("Name"),
                                TagId = reader.ReadInt("TagId"),
                                TagName = reader.ReadString("TagName"),
                                Color = HexToColor(reader.ReadString("Color")),
                                Hotkey = reader.ReadInt("Hotkey"),
                                ProfileId = reader.ReadInt("ProfileId")
                            };

                            result.Value.Add(tagValue);
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                result.AddError($"Error occurred while fetching tag values for TagId [{tagId}]. \n{ex.Message}");
                Logger.Error($"Error occurred while fetching tag values for TagId [{tagId}].", ex);
            }

            return result;
        }
        public ResultOrError<TagValue> GetTagValue(int id)
        {
            var result = new ResultOrError<TagValue>();

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"SELECT * FROM TagValue 
                                    WHERE Id = @Id 
                                    AND ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result.Value = new TagValue
                            {
                                Id = reader.ReadInt("Id"),
                                Name = reader.ReadString("Name"),
                                TagId = reader.ReadInt("TagId"),
                                Color = HexToColor(reader.ReadString("Color")),
                                Hotkey = reader.ReadInt("Hotkey"),
                                ProfileId = reader.ReadInt("ProfileId")
                            };
                        }
                        else
                        {
                            result.AddError($"TagValue with ID [{id}] not found.");
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                result.AddError($"Error occurred while fetching TagValue with ID [{id}]. \n{ex.Message}");
                Logger.Error($"Error occurred while fetching TagValue with ID [{id}].", ex);
            }

            return result;
        }
        private Color HexToColor(string hexValue)
        {
            return System.Drawing.ColorTranslator.FromHtml(hexValue);
        }
        public ResultOrError<TagValue> GetTagValueByTagId(int id, int tagId)
        {
            var result = new ResultOrError<TagValue>();

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                   
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"SELECT * FROM TagValue 
                                    WHERE Id = @Id 
                                    AND TagId = @TagId 
                                    AND ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@TagId", tagId);
                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result.Value = new TagValue
                            {
                                Id = reader.ReadInt("Id"),
                                Name = reader.ReadString("Name"),
                                TagId = reader.ReadInt("TagId"),
                                Color = HexToColor(reader.ReadString("Color")),
                                Hotkey = reader.ReadInt("Hotkey"),
                                ProfileId = reader.ReadInt("ProfileId")
                            };
                        }
                        else
                        {
                            result.AddError($"TagValue with Id [{id}] and TagId [{tagId}] not found.");
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                result.AddError($"Error occurred while fetching TagValue with Id [{id}] and TagId [{tagId}]. \n{ex.Message}");
                Logger.Error($"Error occurred while fetching TagValue with Id [{id}] and TagId [{tagId}].", ex);
            }

            return result;
        }
        public ResultOrError<TagValue> GetTagValueByName(int tagId, string name)
        {
            var result = new ResultOrError<TagValue>();

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {

                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"SELECT * FROM TagValue 
                                    WHERE TagId = @TagId 
                                    AND Name = @Name 
                                    AND ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@TagId", tagId);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result.Value = new TagValue
                            {
                                Id = reader.ReadInt("Id"),
                                Name = reader.ReadString("Name"),
                                TagId = reader.ReadInt("TagId"),
                                Color = HexToColor(reader.ReadString("Color")),
                                Hotkey = reader.ReadInt("Hotkey"),
                                ProfileId = reader.ReadInt("ProfileId")
                            };
                        }
                        else
                        {
                            result.AddError($"TagValue with Name [{name}] and TagId [{tagId}] not found.");
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                result.AddError($"Error occurred while fetching TagValue with Name [{name}] and TagId [{tagId}]. \n{ex.Message}");
                Logger.Error($"Error occurred while fetching TagValue with Name [{name}] and TagId [{tagId}].", ex);
            }

            return result;
        }
        public ResultOrError UpdateTagValue(TagValue tagValue)
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"UPDATE TagValue 
                                    SET Name = @Name,
                                        Color = @Color, 
                                        Hotkey = @Hotkey
                                    WHERE Id = @Id 
                                    AND ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@Id", tagValue.Id);
                    command.Parameters.AddWithValue("@Name", tagValue.Name ?? "");
                    command.Parameters.AddWithValue("@Color", ColorToHex(tagValue.Color) ?? "");
                    command.Parameters.AddWithValue("@Hotkey", tagValue.Hotkey);
                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                result.AddError($"Error occurred while updating TagValue [{tagValue.Name}]. \n{ex.Message}");
                Logger.Error($"Error occurred while updating TagValue [{tagValue.Name}].", ex);
            }

            return result;
        }
        public ResultOrError DeleteTagValue(int id)
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"DELETE FROM TagValue 
                                    WHERE Id = @Id 
                                    AND ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                result.AddError($"Error occurred while deleting TagValue with ID [{id}]. \n{ex.Message}");
                Logger.Error($"Error occurred while deleting TagValue with ID [{id}].", ex);
            }

            return result;
        }
        public ResultOrError DeleteTagValuesByTagId(int tagId)
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"DELETE FROM TagValue 
                                    WHERE TagId = @TagId 
                                    AND ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@TagId", tagId);
                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                result.AddError($"Error occurred while deleting TagValues for TagId [{tagId}]. \n{ex.Message}");
                Logger.Error($"Error occurred while deleting TagValues for TagId [{tagId}].", ex);
            }

            return result;
        }
        public ResultOrError DeleteAllTagValue()
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"DELETE FROM TagValue 
                                    WHERE ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                result.AddError($"Error occurred while deleting all TagValues. \n{ex.Message}");
                Logger.Error($"Error occurred while deleting all TagValues.", ex);
            }

            return result;
        }
        public ResultOrError ClearTagValueTable()
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "DELETE FROM TagValue";

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                result.AddError($"Error occurred while clearing the TagValue table. \n{ex.Message}");
                Logger.Error($"Error occurred while clearing the TagValue table.", ex);
            }

            return result;
        }
        #endregion
    }
}
