using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Model
{
    public interface ITrackDao
    {
        Track GetTrackByPath(String path);
        void AddTrackToDatabase(Track track);
        void AddTrackToPlaylist(int id, int playlistId, int trackId, int orderInList);
    }
}
