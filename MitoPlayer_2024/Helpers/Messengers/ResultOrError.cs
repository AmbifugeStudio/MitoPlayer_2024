using Mysqlx;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Helpers
{
    public class ResultOrError 
    {
        public bool Success { get; set; }
        public String ErrorMessage { get; set; }

        public ResultOrError() {
            this.Success = true;
        }
        public void AddError(String errorMessage)
        {
            this.Success = false;
            if (String.IsNullOrEmpty(this.ErrorMessage))
            {
                this.ErrorMessage = errorMessage;
            }
            else
            {
                this.ErrorMessage = this.ErrorMessage + "\n" + errorMessage;
            }
            
        }
    }
}
