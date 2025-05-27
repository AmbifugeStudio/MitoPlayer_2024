using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MitoPlayer_2024.Helpers
{
    public class DataTableModel
    {
        public BindingSource BindingSource {  get; set; }
        public bool[] ColumnVisibilityArray {  get; set; }
        public int[] ColumnDisplayIndexArray {  get; set; }
        public int CurrentObjectId {  get; set; }
        public int CurrentTrackIdInPlaylist {  get; set; }
        public String CurrentPlaylistName {  get; set; }

        public DataTableModel()
        {

        }
    }
}
