using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Helpers
{
    public enum FeatureType
    {
        Empty,
        Chroma,
        MFCCs,
        HPCP,
        HPSS,
        SpectralContrast,
        SpectralCentroid,
        SpectralBandwidth,
        Tonnetz,
        ZCR,
        RMS,
        Pitch
    }
}
