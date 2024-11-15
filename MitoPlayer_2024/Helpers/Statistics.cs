using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Helpers
{
    public static class Statistics
    {

        public static float Mean(float[] values)
        {
            return values.Average();
        }

        public static float Variance(float[] values)
        {
            float mean = Mean(values);
            return values.Select(val => (val - mean) * (val - mean)).Average();
        }

        public static float StandardDeviation(float[] values)
        {
            return (float)Math.Sqrt(Variance(values));
        }

        public static float Sum(float[] values)
        {
            float sum = 0;
            foreach (var value in values)
            {
                sum += value;
            }
            return sum;
        }
    

    public static float Min(float[] values)
        {
            return values.Min();
        }

        public static float Max(float[] values)
        {
            return values.Max();
        }
        public static float Median(float[] values)
        {
            Array.Sort(values);
            int n = values.Length;
            if (n % 2 == 0)
                return (values[n / 2 - 1] + values[n / 2]) / 2.0f;
            else
                return values[n / 2];
        }

        public static float Percentile(float[] values, float percentile)
        {
            Array.Sort(values);
            int N = values.Length;
            float n = (N - 1) * percentile + 1;
            if (n == 1f) return values[0];
            else if (n == N) return values[N - 1];
            else
            {
                int k = (int)n;
                float d = n - k;
                return values[k - 1] + d * (values[k] - values[k - 1]);
            }
        }
        public static float Skewness(float[] values)
        {
            float mean = values.Average();
            float stdDev = (float)Math.Sqrt(values.Select(val => Math.Pow(val - mean, 2)).Average());
            int n = values.Length;
            return (float)(n * values.Select(val => Math.Pow(val - mean, 3)).Sum() / ((n - 1) * (n - 2) * Math.Pow(stdDev, 3)));
        }
        public static float Kurtosis(float[] values)
        {
            float mean = values.Average();
            float stdDev = (float)Math.Sqrt(values.Select(val => Math.Pow(val - mean, 2)).Average());
            int n = values.Length;
            return (float)(n * (n + 1) * values.Select(val => Math.Pow(val - mean, 4)).Sum() / ((n - 1) * (n - 2) * (n - 3) * Math.Pow(stdDev, 4)) - 3 * Math.Pow(n - 1, 2) / ((n - 2) * (n - 3)));
        }
        public static float Entropy(float[] values)
        {
            var valueCounts = values.GroupBy(v => v).Select(g => g.Count()).ToArray();
            float entropy = 0;
            foreach (var count in valueCounts)
            {
                float p = (float)count / values.Length;
                entropy -= p * (float)Math.Log(p, 2);
            }
            return entropy;
        }
        public static float Range(float[] values)
        {
            return values.Max() - values.Min();
        }

        public static float InterquartileRange(float[] values)
        {
            return Percentile(values, 0.75f) - Percentile(values, 0.25f);
        }
        public static float[] FirstDerivative(float[] values)
        {
            float[] delta = new float[values.Length - 1];
            for (int i = 0; i < values.Length - 1; i++)
            {
                delta[i] = values[i + 1] - values[i];
            }
            return delta;
        }

        public static float[] SecondDerivative(float[] values)
        {
            float[] delta = FirstDerivative(values);
            return FirstDerivative(delta);
        }
        public static float RMSEnergy(float[] values)
        {
            return (float)Math.Sqrt(values.Select(val => val * val).Average());
        }
        public static float ZeroCrossingRate(float[] values)
        {
            int zeroCrossings = 0;
            for (int i = 1; i < values.Length; i++)
            {
                if ((values[i - 1] >= 0 && values[i] < 0) || (values[i - 1] < 0 && values[i] >= 0))
                {
                    zeroCrossings++;
                }
            }
            return (float)zeroCrossings / values.Length;
        }
        public static float SpectralCentroid(float[] magnitudes, float[] frequencies)
        {
            float weightedSum = 0;
            float sum = 0;
            for (int i = 0; i < magnitudes.Length; i++)
            {
                weightedSum += magnitudes[i] * frequencies[i];
                sum += magnitudes[i];
            }
            return weightedSum / sum;
        }
        public static float SpectralBandwidth(float[] magnitudes, float[] frequencies, float spectralCentroid)
        {
            float sum = 0;
            float totalMagnitude = magnitudes.Sum();
            for (int i = 0; i < magnitudes.Length; i++)
            {
                sum += magnitudes[i] * (float)Math.Pow(frequencies[i] - spectralCentroid, 2);
            }
            return (float)Math.Sqrt(sum / totalMagnitude);
        }
        public static float[] SpectralContrast(float[] magnitudes, int numBands)
        {
            float[] contrast = new float[numBands];
            int bandSize = magnitudes.Length / numBands;
            for (int i = 0; i < numBands; i++)
            {
                float[] band = magnitudes.Skip(i * bandSize).Take(bandSize).ToArray();
                contrast[i] = band.Max() - band.Min();
            }
            return contrast;
        }
        public static float SpectralFlatness(float[] magnitudes)
        {
            float geometricMean = (float)Math.Exp(magnitudes.Select(val => Math.Log(val)).Average());
            float arithmeticMean = magnitudes.Average();
            return geometricMean / arithmeticMean;
        }
        public static float[] Envelope(float[] values)
        {
            float[] envelope = new float[values.Length];
            envelope[0] = values[0];
            for (int i = 1; i < values.Length; i++)
            {
                envelope[i] = Math.Max(values[i], envelope[i - 1] * 0.99f);
            }
            return envelope;
        }
        public static float[] Autocorrelation(float[] values)
        {
            int n = values.Length;
            float[] result = new float[n];
            for (int lag = 0; lag < n; lag++)
            {
                float sum = 0;
                for (int i = 0; i < n - lag; i++)
                {
                    sum += values[i] * values[i + lag];
                }
                result[lag] = sum / n;
            }
            return result;
        }
        public static float HarmonicToNoiseRatio(float[] values)
        {
            float harmonicEnergy = values.Where(val => val > 0).Sum(val => val * val);
            float noiseEnergy = values.Where(val => val <= 0).Sum(val => val * val);
            return 10 * (float)Math.Log10(harmonicEnergy / noiseEnergy);
        }
        public static float Mode(float[] values)
        {
            return values.GroupBy(v => v)
            .OrderByDescending(g => g.Count())
            .First()
            .Key;
        }
        public static float RootMeanSquare(float[] values)
        {
            return (float)Math.Sqrt(values.Select(val => val * val).Average());
        }
        public static float HarmonicMean(float[] values)
        {
            return values.Length / values.Select(val => 1.0f / val).Sum();
        }
        public static float GeometricMean(float[] values)
        {
            return (float)Math.Pow(values.Aggregate(1.0f, (acc, val) => acc * val), 1.0f / values.Length);
        }
        public static float Energy(float[] values)
        {
            return values.Select(val => val * val).Sum();
        }
        public static float PeakToPeakAmplitude(float[] values)
        {
            return values.Max() - values.Min();
        }

        public static float SpectralRollOff(float[] magnitudes, float rollOffPercent = 0.85f)
        {
            float totalEnergy = magnitudes.Sum();
            float threshold = rollOffPercent * totalEnergy;
            float cumulativeEnergy = 0;
            for (int i = 0; i < magnitudes.Length; i++)
            {
                cumulativeEnergy += magnitudes[i];
                if (cumulativeEnergy >= threshold)
                {
                    return i;
                }
            }
            return magnitudes.Length - 1;
        }
        public static float SpectralEntropy(float[] magnitudes)
        {
            float totalEnergy = magnitudes.Sum();
            float[] probabilities = magnitudes.Select(val => val / totalEnergy).ToArray();
            float entropy = 0;
            foreach (var p in probabilities)
            {
                if (p > 0)
                {
                    entropy -= p * (float)Math.Log(p, 2);
                }
            }
            return entropy;
        }
    }
}
