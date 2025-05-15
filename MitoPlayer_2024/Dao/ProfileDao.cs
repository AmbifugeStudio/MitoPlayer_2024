using Microsoft.Data.Sqlite;
using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Helpers.ErrorHandling;
using MitoPlayer_2024.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace MitoPlayer_2024.Dao
{
    public class ProfileDao : BaseDao, IProfileDao
    {
        public ProfileDao(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public ResultOrError CreateProfile(Profile profile)
        {
            ResultOrError result = new ResultOrError();

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"INSERT INTO Profile (Name, IsActive)
                                            VALUES (@Name, @IsActive)";

                    command.Parameters.AddWithValue("@Name", profile.Name ?? "");
                    command.Parameters.AddWithValue("@IsActive", profile.IsActive ? 1 : 0);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SqliteException ex)
            {
                result.AddError($"Profile [{profile.Name}] is not inserted. \n{ex.Message}");
                Logger.Error($"Error occurred while inserting profile [{profile.Name}].", ex);
            }

            return result;
        }
        public ResultOrError<Profile> GetActiveProfile()
        {
            ResultOrError<Profile> result = new ResultOrError<Profile>();
            Profile profile = null;

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                using (var command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"SELECT * FROM Profile WHERE IsActive = 1";

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            profile = new Profile
                            {
                                Id = reader.ReadInt("Id"),
                                Name = reader.ReadString("Name"),
                                IsActive = reader.ReadBool("IsActive")
                            };
                            result.Value = profile;
                        }
                        else
                        {
                            //result.AddError("No active profile found.");
                        }
                    }
                }
            }
            catch (SqliteException ex)
            {
                result.AddError($"Error occurred while fetching active profile.\n{ex.Message}");
                Logger.Error("Error occurred while fetching active profile.", ex);
            }

            return result;
        }
        public ResultOrError<Profile> GetProfile(int id)
        {
            ResultOrError<Profile> result = new ResultOrError<Profile>();
            Profile profile = null;

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                using (var command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"SELECT * FROM Profile WHERE Id = @Id";

                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            profile = new Profile
                            {
                                Id = reader.ReadInt("Id"),
                                Name = reader.ReadString("Name"),
                                IsActive = reader.ReadBool("IsActive")
                            };
                            result.Value = profile;
                        }
                        else
                        {
                            result.AddError($"Profile with ID [{id}] not found.");
                        }
                    }
                }
            }
            catch (SqliteException ex)
            {
                result.AddError($"Error occurred while fetching profile with ID [{id}].\n{ex.Message}");
                Logger.Error($"Error occurred while fetching profile with ID [{id}].", ex);
            }

            return result;
        }
        public ResultOrError<Profile> GetProfileByName(string name)
        {
            ResultOrError<Profile> result = new ResultOrError<Profile>();
            Profile profile = null;

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                using (var command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"SELECT * FROM Profile WHERE Name = @Name";

                    command.Parameters.AddWithValue("@Name", name);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            profile = new Profile
                            {
                                Id = reader.ReadInt("Id"),
                                Name = reader.ReadString("Name"),
                                IsActive = reader.ReadBool("IsActive")
                            };
                            result.Value = profile;
                        }
                        else
                        {
                            result.AddError($"Profile with Name [{name}] not found.");
                        }
                    }
                }
            }
            catch (SqliteException ex)
            {
                result.AddError($"Error occurred while fetching profile with Name [{name}].\n{ex.Message}");
                Logger.Error($"Error occurred while fetching profile with Name [{name}].", ex);
            }

            return result;
        }
        public ResultOrError<List<Profile>> GetAllProfile()
        {
            ResultOrError<List<Profile>> result = new ResultOrError<List<Profile>> { Value = new List<Profile>() };

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                using (var command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"SELECT * FROM Profile";

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Profile profile = new Profile
                            {
                                Id = reader.ReadInt("Id"),
                                Name = reader.ReadString("Name"),
                                IsActive = reader.ReadBool("IsActive")
                            };
                            result.Value.Add(profile);
                        }
                    }
                }
            }
            catch (SqliteException ex)
            {
                result.AddError($"Error occurred while fetching all profiles.\n{ex.Message}");
                Logger.Error("Error occurred while fetching all profiles.", ex);
            }

            return result;
        }
        public ResultOrError UpdateProfile(Profile profile)
        {
            ResultOrError result = new ResultOrError();

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                using (var command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"UPDATE Profile 
                                            SET Name = @Name, IsActive = @IsActive 
                                            WHERE Id = @Id";

                    command.Parameters.AddWithValue("@Id", profile.Id);
                    command.Parameters.AddWithValue("@Name", profile.Name);
                    command.Parameters.AddWithValue("@IsActive", profile.IsActive);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SqliteException ex)
            {
                result.AddError($"Error occurred while updating profile [{profile.Name}].\n{ex.Message}");
                Logger.Error($"Error occurred while updating profile [{profile.Name}].", ex);
            }

            return result;
        }

        public ResultOrError DeleteProfile(int id)
        {
            ResultOrError result = new ResultOrError();

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                using (var command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"DELETE FROM Profile WHERE Id = @Id";

                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SqliteException ex)
            {
                result.AddError($"Error occurred while deleting profile with ID [{id}].\n{ex.Message}");
                Logger.Error($"Error occurred while deleting profile with ID [{id}].", ex);
            }

            return result;
        }
        public ResultOrError ClearProfileTable()
        {
            ResultOrError result = new ResultOrError();

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                using (var command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"DELETE FROM Profile";

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SqliteException ex)
            {
                result.AddError($"Error occurred while clearing the profile table.\n{ex.Message}");
                Logger.Error("Error occurred while clearing the profile table.", ex);
            }

            return result;
        }


    }
}
