using MitoPlayer_2024.Model;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace MitoPlayer_2024.Dao
{
    public class TrackDao: BaseDao, ITrackDao
    {
        private int profileId = -1;
        public TrackDao(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public void SetProfileId(int profileId)
        {
            this.profileId = profileId;
        }

        #region OPEN FILES

        /*
         * szám lekérése a path alapján
         */
        public Track GetTrackByPath(String path)
        {
            Track track = null;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT * FROM Track WHERE Path = @Path AND ProfileId = @ProfileId ";
                command.Parameters.Add("@Path", MySqlDbType.VarChar).Value = path;
                command.Parameters.Add("@ProfileId", MySqlDbType.VarChar).Value = this.profileId;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        track = new Track();
                        track.Id = (int)reader[0];
                        track.Path = reader[1].ToString();
                        track.FileName = reader[2].ToString();
                        track.Artist = reader[3].ToString();
                        track.Title = reader[4].ToString();
                        track.Album = reader[5].ToString();
                        track.Year = (int)reader[6];
                        track.Length = (int)reader[7];
                    }
                }
            }
            return track;
        }
        /*
         * új szám hozzáadása az adatbázishoz
         */
        public void AddTrackToDatabase(Track track)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "INSERT INTO Track values (@Id, @Path, @FileName, @Artist, @Title, @Album, @Year, @Length, @ProfileId)";
                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = track.Id;
                command.Parameters.Add("@Path", MySqlDbType.VarChar).Value = track.Path;
                command.Parameters.Add("@FileName", MySqlDbType.VarChar).Value = track.FileName;
                command.Parameters.Add("@Artist", MySqlDbType.VarChar).Value = track.Artist;
                command.Parameters.Add("@Title", MySqlDbType.VarChar).Value = track.Title;
                command.Parameters.Add("@Album", MySqlDbType.VarChar).Value = track.Album;
                command.Parameters.Add("@Year", MySqlDbType.Int32).Value = track.Year;
                command.Parameters.Add("@Length", MySqlDbType.Int32).Value = track.Length;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                command.ExecuteNonQuery();
            }
        }
        /*
         * szám hozzáadása playlist-hez
         */
        public void AddTrackToPlaylist(int id, int playlistId, int trackId, int orderInList, int trackIdInPlaylist)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "INSERT INTO PlaylistContent values (@Id, @PlaylistId, @TrackId, @OrderInList, @TrackIdInPlaylist, @ProfileId)";
                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = id;
                command.Parameters.Add("@PlaylistId", MySqlDbType.Int32).Value = playlistId;
                command.Parameters.Add("@TrackId", MySqlDbType.Int32).Value = trackId;
                command.Parameters.Add("@OrderInList", MySqlDbType.Int32).Value = orderInList;
                command.Parameters.Add("@TrackIdInPlaylist", MySqlDbType.Int32).Value = trackIdInPlaylist;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                command.ExecuteNonQuery();
            }
        }

        #endregion

        public void ClearTrackTable()
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "DELETE FROM Track ";
                try
                {
                    command.ExecuteNonQuery();
                    //MessageBox.Show("Track deleted succesfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Track is not deleted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }

        public void DeleteAllTrackFromProfile(int profileId)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "DELETE FROM Track WHERE ProfileId = @ProfileId ";
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = profileId;
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
