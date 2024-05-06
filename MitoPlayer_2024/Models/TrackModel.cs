using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Model
{

    public class TrackModel
    {
        //Fields
        private int id;
        private string path;
        private string fileName;
        private string artist;
        private string title;
        private string album;
        private int year;
        private double length;
        private bool isMissing;
        private int idInPlaylist;

        public TrackModel()
        {
            id = -1;
            isMissing = false;
        }

        //Properties - Validations
        [DisplayName("Track Id")]
        public int Id { get => id; set => id = value; }
        [DisplayName("Track Path")]
        public string Path { get => path; set => path = value; }
        [DisplayName("Track FileName")]
        public string FileName { get => fileName; set => fileName = value; }
        [DisplayName("Track Artist")]
        public string Artist { get => artist; set => artist = value; }
        [DisplayName("Track Title")]
        public string Title { get => title; set => title = value; }
        [DisplayName("Track Album")]
        public string Album { get => album; set => album = value; }
        [DisplayName("Track Year")]
        public int Year { get => year; set => year = value; }
        [DisplayName("Track Length")]
        public double Length { get => length; set => length = value; }
        [DisplayName("Track Is Missing")]
        public bool IsMissing { get => isMissing; set => isMissing = value; }
        [DisplayName("Track Id In Playlist")]
        public int IdInPlaylist { get => idInPlaylist; set => idInPlaylist = value; }

    }
}
