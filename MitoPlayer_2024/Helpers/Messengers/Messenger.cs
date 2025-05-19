using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MitoPlayer_2024.Helpers
{
    public class Messenger : EventArgs
    {
        public string[] DragAndDropFilePathArray { get; set; }
        public List<int> DragAndDropTrackIds { get; set; }
        public DataGridViewRowCollection Rows { get; set; }
        public DataGridViewSelectedRowCollection SelectedRows { get; set; }
        public List<int> SelectedIndices { get; internal set; }
        public String StringField1 { get; set; }
        public String StringField2 { get; set; }
        public String StringField3 { get; set; }
        public String StringField4 { get; set; }
        public String StringField5 { get; set; }
        public Int32 IntegerField1 { get; set; }
        public Int32 IntegerField2 { get; set; }
        public Int32 IntegerField3 { get; set; }
        public Int32 IntegerField4 { get; set; }
        public Int32 IntegerField5 { get; set; }
        public Decimal DecimalField1 { get; set; }
        public Decimal DecimalField2 { get; set; }
        public Decimal DecimalField3 { get; set; }
        public Decimal DecimalField4 { get; set; }
        public Decimal DecimalField5 { get; set; }
        public DateTime DateTimeField1 { get; set; }
        public DateTime DateTimeField2 { get; set; }
        public DateTime DateTimeField3 { get; set; }
        public DateTime DateTimeField4 { get; set; }
        public DateTime DateTimeField5 { get; set; }
        public Boolean BooleanField1 { get; set; }
        public Boolean BooleanField2 { get; set; }
        public Boolean BooleanField3 { get; set; }
        public Boolean BooleanField4 { get; set; }
        public Boolean BooleanField5 { get; set; }
        public Boolean BooleanField6 { get; set; }
        public Boolean BooleanField7 { get; set; }
        public Image Image { get; set; }
        public List<String> StringList { get;set; }

        public Messenger()
        {
            this.Image = null;
            this.StringList = new List<String>();
            this.StringField1 = String.Empty;
            this.StringField2 = String.Empty;
            this.StringField3 = String.Empty;
            this.StringField4 = String.Empty;
            this.StringField5 = String.Empty;
            this.IntegerField1 = 0;
            this.IntegerField2 = 0;
            this.IntegerField3 = 0;
            this.IntegerField4 = 0;
            this.IntegerField5 = 0;
            this.DecimalField1 = 0;
            this.DecimalField2 = 0;
            this.DecimalField3 = 0;
            this.DecimalField4 = 0;
            this.DecimalField5 = 0;
            this.DateTimeField1 = DateTime.MinValue;
            this.DateTimeField2 = DateTime.MinValue;
            this.DateTimeField3 = DateTime.MinValue;
            this.DateTimeField4 = DateTime.MinValue;
            this.DateTimeField5 = DateTime.MinValue;
            this.BooleanField1 = false;
            this.BooleanField2 = false;
            this.BooleanField3 = false;
            this.BooleanField4 = false;
            this.BooleanField5 = false;
            this.BooleanField6 = false;
            this.BooleanField7 = false;
        }
    }
   
}
