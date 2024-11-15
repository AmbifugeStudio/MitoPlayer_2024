using MitoPlayer_2024.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MitoPlayer_2024.Views
{
    public interface IProfileView
    {

        event EventHandler CreateProfileEvent;
        event EventHandler<Messenger> SetProfileAsActiveEvent;
        event EventHandler<Messenger> RenameProfileEvent;
        event EventHandler<Messenger> DeleteProfileEvent;
        event EventHandler CloseProfileViewEvent;
        void Show();
        void SetProfileListBindingSource(BindingSource profileList);
    }
}
