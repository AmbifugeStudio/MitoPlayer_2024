using MitoPlayer_2024.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.IO;
using MitoPlayer_2024.Models;
using System.Windows.Forms;

namespace MitoPlayer_2024.Dao
{
    public class TrackDao: BaseDao, ITrackDao
    {
        //Constructor
        public TrackDao(string connectionString)
        {
            this.connectionString = connectionString;
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
                command.CommandText = "SELECT * FROM Track WHERE Path = @Path";
                command.Parameters.Add("@Path", MySqlDbType.VarChar).Value = path;
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
                command.CommandText = "INSERT INTO Track values (@Id, @Path, @FileName, @Artist, @Title, @Album, @Year, @Length)";
                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = track.Id;
                command.Parameters.Add("@Path", MySqlDbType.VarChar).Value = track.Path;
                command.Parameters.Add("@FileName", MySqlDbType.VarChar).Value = track.FileName;
                command.Parameters.Add("@Artist", MySqlDbType.VarChar).Value = track.Artist;
                command.Parameters.Add("@Title", MySqlDbType.VarChar).Value = track.Title;
                command.Parameters.Add("@Album", MySqlDbType.VarChar).Value = track.Album;
                command.Parameters.Add("@Year", MySqlDbType.Int32).Value = track.Year;
                command.Parameters.Add("@Length", MySqlDbType.Int32).Value = track.Length;
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
                command.CommandText = "INSERT INTO PlaylistContent values (@Id, @PlaylistId, @TrackId, @OrderInList, @TrackIdInPlaylist)";
                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = id;
                command.Parameters.Add("@PlaylistId", MySqlDbType.Int32).Value = playlistId;
                command.Parameters.Add("@TrackId", MySqlDbType.Int32).Value = trackId;
                command.Parameters.Add("@OrderInList", MySqlDbType.Int32).Value = orderInList;
                command.Parameters.Add("@TrackIdInPlaylist", MySqlDbType.Int32).Value = trackIdInPlaylist;
                command.ExecuteNonQuery();
            }
        }

        #endregion

    }
}
