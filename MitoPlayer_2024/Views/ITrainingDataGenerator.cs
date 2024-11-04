using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MitoPlayer_2024.Views
{
    internal interface ITrainingDataGenerator
    {
        event EventHandler CloseViewWithOk;
        event EventHandler CloseViewWithCancel;
        void InitializeView();
        void SetTrackListBindingSource(BindingSource trackList);
    }
}
