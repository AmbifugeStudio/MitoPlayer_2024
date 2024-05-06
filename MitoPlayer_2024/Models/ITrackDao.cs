using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Model
{
    public interface ITrackDao
    {
        TrackModel GetTrackByPath(String path);
        void AddTrackToDatabase(TrackModel trackModel);
        void AddTrackToPlaylist(int id, int playlistId, int trackId, int sortingId, int trackIdInPlaylist);
      
    }
}
