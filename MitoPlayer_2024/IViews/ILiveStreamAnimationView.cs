using MitoPlayer_2024.Helpers;
using OxyPlot.WindowsForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.IViews
{

    public interface ILiveStreamAnimationView
    {
        event EventHandler TrackChangeCheckEvent;
        event EventHandler CloseViewEvent;
        void FirstInitialization(Messenger msg);
        void InitializeView(Messenger msg);
       

    }
}
