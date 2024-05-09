using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MitoPlayer_2024.Views
{
    public partial class RuleEditorView : Form
    {
        public RuleEditorView()
        {
            InitializeComponent();
        }

        internal static IRuleEditorView GetInstance(MainView mainView)
        {
            throw new NotImplementedException();
        }
    }
}
