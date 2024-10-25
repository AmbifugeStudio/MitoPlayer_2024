using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MitoPlayer_2024.Helpers
{
    public  class TagValueButton : Button
    {
        public String TagName { get; set; }
        public String TagValueName { get; set; }
        public int TagValueId { get; set; }

        public TextBox TextBox {  get; set; }
        public bool IsPressed { get; set; }

        public TagValueButton()
        {
            this.IsPressed = false;
        }
    }
}
