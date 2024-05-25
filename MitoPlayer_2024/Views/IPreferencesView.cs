using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Views
{
    public interface IPreferencesView
    {
        event EventHandler CloseViewWithOkEvent;
        event EventHandler CloseViewWithCancelEvent;
        event EventHandler ClearDatabaseEvent;
    }
}
