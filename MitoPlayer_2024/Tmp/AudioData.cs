using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Helpers
{

    public class AudioData
    {
        [LoadColumn(0)]
        public string Label { get; set; }

        [LoadColumn(1)]
        public float[] Features { get; set; }
    }

}
