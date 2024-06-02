
using MitoPlayer_2024.Model;
using MitoPlayer_2024.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MitoPlayer_2024.Helpers
{
    public class ListEventArgs : EventArgs
    {
        public string[] DragAndDropFiles { get; set; }
        public DataGridViewRowCollection Rows { get; set; }
        public DataGridViewSelectedRowCollection SelectedRows { get; set; }
        public String StringField1 { get; set; }
        public String StringField2 { get; set; }
        public int IntegerField1 { get; set; }
        public int IntegerField2 { get; set; }
    }
   
}
