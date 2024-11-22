using MitoPlayer_2024.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Trainer
{
    public class FeatureAggregator
    {
        public List<float[]> AggregateFeaturesWithSlidingWindow(float[] features, int windowSize, int stepSize)
        {
            List<float[]> aggregatedFeatures = new List<float[]>();

            for (int i = 0; i <= features.Length - windowSize; i += stepSize)
            {
                float[] windowFeatures = new float[windowSize];
                Array.Copy(features, i, windowFeatures, 0, windowSize);

                float[] aggregatedWindowFeatures = AggregateWindowFeatures(windowFeatures);
                aggregatedFeatures.Add(aggregatedWindowFeatures);
            }

            return aggregatedFeatures;
        }

        private float[] AggregateWindowFeatures(float[] windowFeatures)
        {
            float[] aggregatedFeatures = new float[8];
            aggregatedFeatures[0] = Statistics.Mean(windowFeatures);
            aggregatedFeatures[1] = Statistics.Variance(windowFeatures);
            aggregatedFeatures[2] = Statistics.StandardDeviation(windowFeatures);
            aggregatedFeatures[3] = Statistics.Min(windowFeatures);
            aggregatedFeatures[4] = Statistics.Max(windowFeatures);
            aggregatedFeatures[5] = Statistics.Median(windowFeatures);
            aggregatedFeatures[6] = Statistics.Entropy(windowFeatures);
            aggregatedFeatures[7] = Statistics.Mode(windowFeatures);

            return aggregatedFeatures;
        }
    }

}
