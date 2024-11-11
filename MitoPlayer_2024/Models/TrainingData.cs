using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Models
{
    public class TrainingData
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public DateTime CreateDate { get; set; }
        public int SampleCount { get; set; }
        public Decimal Balance { get; set; }
        public bool ExtractChromaFeatures { get; set; }
        public bool ExtractMFCCs { get; set; }
        public bool ExtractSpectralContrast { get; set; }
        public bool ExtractHPCP { get; set; }
        public bool ExtractSpectralCentroid { get; set; }
        public bool ExtractSpectralBandwidth { get; set; }
        public bool HarmonicPercussiveSeparation { get; set; }
        public bool ExtractTonnetzFeatures { get; set; }
        public bool ExtractZeroCrossingRate { get; set; }
        public bool ExtractRmsEnergy { get; set; }
        public bool ExtractPitch { get; set; }
        public bool IsTemplate { get; set; }
        public int ProfileId { get; set; }
        public String FilePath { get; set; }
        public int TagId { get; set; }

        public TrainingData()
        {
            this.ExtractChromaFeatures = false;
            this.ExtractMFCCs = false;
            this.ExtractSpectralContrast = false;
            this.ExtractHPCP = false;

            this.ExtractSpectralCentroid = false;
            this.ExtractSpectralBandwidth = false;

            this.HarmonicPercussiveSeparation = false;
            this.ExtractTonnetzFeatures = false;

            this.ExtractZeroCrossingRate = false;
            this.ExtractRmsEnergy = false;
            this.ExtractPitch = false;
            this.IsTemplate = false;

        }
    }
}
