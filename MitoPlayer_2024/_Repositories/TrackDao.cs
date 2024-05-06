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

namespace MitoPlayer_2024._Repositories
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
        public TrackModel GetTrackByPath(String path)
        {
            TrackModel track = null;

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
                        track = new TrackModel();
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
        public void AddTrackToDatabase(TrackModel trackModel)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "INSERT INTO Track values (@Id, @Path, @FileName, @Artist, @Title, @Album, @Year, @Length)";
                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = trackModel.Id;
                command.Parameters.Add("@Path", MySqlDbType.VarChar).Value = trackModel.Path;
                command.Parameters.Add("@FileName", MySqlDbType.VarChar).Value = trackModel.FileName;
                command.Parameters.Add("@Artist", MySqlDbType.VarChar).Value = trackModel.Artist;
                command.Parameters.Add("@Title", MySqlDbType.VarChar).Value = trackModel.Title;
                command.Parameters.Add("@Album", MySqlDbType.VarChar).Value = trackModel.Album;
                command.Parameters.Add("@Year", MySqlDbType.Int32).Value = trackModel.Year;
                command.Parameters.Add("@Length", MySqlDbType.Int32).Value = trackModel.Length;
                command.ExecuteNonQuery();
            }
        }
        /*
         * szám hozzáadása playlist-hez
         */
        public void AddTrackToPlaylist(int id, int playlistId, int trackId, int sortingId, int trackIdInPlaylist)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "INSERT INTO PlaylistContent values (@Id, @PlaylistId, @TrackId, @SortingId, @TrackIdInPlaylist)";
                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = id;
                command.Parameters.Add("@PlaylistId", MySqlDbType.Int32).Value = playlistId;
                command.Parameters.Add("@TrackId", MySqlDbType.Int32).Value = trackId;
                command.Parameters.Add("@SortingId", MySqlDbType.Int32).Value = sortingId;
                command.Parameters.Add("@TrackIdInPlaylist", MySqlDbType.Int32).Value = trackIdInPlaylist;
                command.ExecuteNonQuery();
            }
        }

        #endregion


/*
        public int GetLastTrackId()
        {
            int lastId = 0;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT Id FROM Track ORDER BY Id desc LIMIT 1";
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


        public bool TrackIsExists(String path)
        {
            int existingTrackId = -1;
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT Id FROM Track WHERE Path = @Path";
                command.Parameters.Add("@Path", MySqlDbType.Int32).Value = path;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        existingTrackId = (int)reader[0];
                    }
                }
            }
            if (existingTrackId != -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        
        
        


        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Edit(TrackModel trackModel)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TrackModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TrackModel> GetByValue(string value)
        {
            throw new NotImplementedException();
        }*/
    }
}
