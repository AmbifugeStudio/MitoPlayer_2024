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

    public interface ILiveStreamAnimationSettingView
    {
        event EventHandler BrowseDirectoryEvent;
        event EventHandler<Messenger> SetImagePathEvent;

        event EventHandler CloseViewWithOkEvent;
        event EventHandler CloseViewWithCancelEvent;

        void InitializeView(Messenger messenger);
    }
}
