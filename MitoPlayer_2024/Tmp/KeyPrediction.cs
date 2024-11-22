using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Helpers
{
    public class KeyPrediction
    {
        [ColumnName("PredictedLabel")]
        public string PredictedKey { get; set; }
    }
}
