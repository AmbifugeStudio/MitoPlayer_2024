using Accord.Audio;
using Accord.Audio.Formats;
using Accord.Math;
using Accord.Statistics;
using NAudio.Dsp;
using NAudio.Wave;
using NWaves.FeatureExtractors;
using NWaves.Signals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;


namespace MitoPlayer_2024.Trainer
{
    public class FeatureExtractor
    {
        public float[] ExtractFeatures(string filePath, CancellationToken cancellationToken)
        {
            List<float> featureList = new List<float>();

            using (var reader = new AudioFileReader(filePath))
            {
                int sampleRate = reader.WaveFormat.SampleRate;
                int channels = reader.WaveFormat.Channels;
                int durationInSeconds = 90; // 1.5 minutes
                int sampleCount = durationInSeconds * sampleRate * channels;

                float[] buffer = new float[sampleCount];
                int samplesRead = reader.Read(buffer, 0, sampleCount);

                // Convert to mono if necessary
                if (channels > 1)
                {
                    buffer = ConvertToMono(buffer, channels);
                }

                // Perform FFT and extract other features
                int intervalSeconds = 1;
                int intervalSampleCount = sampleRate * intervalSeconds;
                for (int i = 0; i < buffer.Length; i += intervalSampleCount)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    float[] intervalBuffer = buffer.Skip(i).Take(intervalSampleCount).ToArray();
                    Complex[] fftBuffer = new Complex[intervalBuffer.Length];
                    for (int j = 0; j < intervalBuffer.Length; j++)
                    {
                        fftBuffer[j] = new Complex { X = intervalBuffer[j], Y = 0 };
                    }
                    FastFourierTransform.FFT(true, (int)Math.Log(intervalBuffer.Length, 2.0), fftBuffer);

                    // Extract features
                    float[] features = ExtractFeaturesFromBuffer(fftBuffer, intervalBuffer, sampleRate);
                    featureList.AddRange(features);
                }
            }

            return featureList.ToArray();
        }
        private static float[] ConvertToMono(float[] samples, int channels)
        {
            int monoLength = samples.Length / channels;
            float[] monoSamples = new float[monoLength];

            for (int i = 0; i < monoLength; i++)
            {
                for (int j = 0; j < channels; j++)
                {
                    monoSamples[i] += samples[i * channels + j];
                }
                monoSamples[i] /= channels;
            }

            return monoSamples;
        }
        private float[] ExtractFeaturesFromBuffer(Complex[] fftBuffer, float[] buffer, int sampleRate)
        {
            List<float> features = new List<float>();

            // Extract Chroma features
            var chromaFeatures = ExtractChromaFeatures(fftBuffer);
            features.AddRange(chromaFeatures);

            // Extract MFCCs
            var mfccFeatures = ExtractMFCCsFeatures(buffer, sampleRate);
            features.AddRange(mfccFeatures);

            // Extract Spectral Contrast
            var spectralContrastFeatures = ExtractSpectralContrastFeatures(fftBuffer);
            features.AddRange(spectralContrastFeatures);

            // Extract Spectral Centroid
            var spectralCentroid = ExtractSpectralCentroidFeatures(fftBuffer);
            features.Add(spectralCentroid);

            // Extract Spectral Bandwidth
            var spectralBandwidth = ExtractSpectralBandwidthFeatures(buffer);
            features.Add(spectralBandwidth);

            // Extract Zero Crossing Rate
            var zcr = ExtractZeroCrossingRateFeatures(buffer);
            features.Add(zcr);

            // Extract RMS Energy
            var rmsEnergy = ExtractRmsEnergyFeatures(buffer);
            features.Add(rmsEnergy);

            // Extract Pitch
            var pitch = ExtractPitchFeatures(buffer, sampleRate);
            features.Add(pitch);

            return features.ToArray();
        }
        private static double Log2(double x)
        {
            return Math.Log(x) / Math.Log(2);
        }
        // Placeholder methods for feature extraction
        private float[] ExtractChromaFeatures(Complex[] fftBuffer) {
            float[] chromaFeatures = new float[12];
            int totalBins = fftBuffer.Length / 2;

            for (int i = 0; i < totalBins; i++)
            {
                double magnitude = Math.Sqrt(fftBuffer[i].X * fftBuffer[i].X + fftBuffer[i].Y * fftBuffer[i].Y);
                if (magnitude > 0)
                {
                    int pitchClass = (int)Math.Round(12 * Log2(i + 1)) % 12; // Use i + 1 to avoid log(0)
                    if (pitchClass < 0)
                    {
                        pitchClass += 12; // Ensure pitchClass is non-negative
                    }
                    chromaFeatures[pitchClass] += (float)magnitude;
                }
            }

            return chromaFeatures;
        }
        private float MelScale(float freq)
        {
            return 2595 * (float)Math.Log10(1 + freq / 700);
        }
        private float[] ExtractMFCCsFeatures(float[] buffer, int sampleRate, int numFilters = 26, int fftSize = 512) {
            // Step 1: Pre-emphasis
            float[] preEmphasized = new float[buffer.Length];
            float preEmphasis = 0.97f;
            for (int i = 1; i < buffer.Length; i++)
            {
                preEmphasized[i] = buffer[i] - preEmphasis * buffer[i - 1];
            }

            // Step 2: Framing
            int frameSize = fftSize;
            int frameStep = frameSize / 2;
            int numFrames = (buffer.Length - frameSize) / frameStep + 1;
            float[][] frames = new float[numFrames][];
            for (int i = 0; i < numFrames; i++)
            {
                frames[i] = new float[frameSize];
                Array.Copy(preEmphasized, i * frameStep, frames[i], 0, frameSize);
            }


            // Step 3: Windowing
            for (int i = 0; i < numFrames; i++)
            {
                for (int j = 0; j < frameSize; j++)
                {
                    frames[i][j] *= (float)(0.54 - 0.46 * Math.Cos(2 * Math.PI * j / (frameSize - 1))); // Hamming window
                }
            }

            // Step 4: FFT and Power Spectrum
            float[][] powerSpectrum = new float[numFrames][];
            for (int i = 0; i < numFrames; i++)
            {
                Complex[] fftBuffer = new Complex[fftSize];
                for (int j = 0; j < frameSize; j++)
                {
                    fftBuffer[j] = new Complex { X = frames[i][j], Y = 0 };
                }
                FastFourierTransform.FFT(true, (int)Math.Log(fftSize, 2.0), fftBuffer);
                powerSpectrum[i] = new float[fftSize / 2 + 1];
                for (int j = 0; j < fftSize / 2 + 1; j++)
                {
                    powerSpectrum[i][j] = (float)(fftBuffer[j].X * fftBuffer[j].X + fftBuffer[j].Y * fftBuffer[j].Y) / fftSize;
                }
            }

            // Step 5: Mel Filter Banks
            float[][] melFilterBanks = new float[numFilters][];
            for (int i = 0; i < numFilters; i++)
            {
                melFilterBanks[i] = new float[fftSize / 2 + 1];
            }
            float[] melPoints = new float[numFilters + 2];
            for (int i = 0; i < melPoints.Length; i++)
            {
                melPoints[i] = MelScale(i * (sampleRate / 2) / (numFilters + 1));
            }
            for (int i = 1; i <= numFilters; i++)
            {
                for (int j = 0; j < fftSize / 2 + 1; j++)
                {
                    float freq = j * sampleRate / fftSize;
                    if (freq >= melPoints[i - 1] && freq <= melPoints[i])
                    {
                        melFilterBanks[i - 1][j] = (freq - melPoints[i - 1]) / (melPoints[i] - melPoints[i - 1]);
                    }
                    else if (freq >= melPoints[i] && freq <= melPoints[i + 1])
                    {
                        melFilterBanks[i - 1][j] = (melPoints[i + 1] - freq) / (melPoints[i + 1] - melPoints[i]);
                    }
                }
            }


            // Step 6: Mel Spectrum
            float[][] melSpectrum = new float[numFrames][];
            for (int i = 0; i < numFrames; i++)
            {
                melSpectrum[i] = new float[numFilters];
                for (int j = 0; j < numFilters; j++)
                {
                    for (int k = 0; k < fftSize / 2 + 1; k++)
                    {
                        melSpectrum[i][j] += powerSpectrum[i][k] * melFilterBanks[j][k];
                    }
                    melSpectrum[i][j] = (float)Math.Log(melSpectrum[i][j] + 1e-10); // Logarithm of Mel spectrum
                }
            }

            // Step 7: Discrete Cosine Transform (DCT)
            float[][] mfcc = new float[numFrames][];
            for (int i = 0; i < numFrames; i++)
            {
                mfcc[i] = new float[13];
                for (int j = 0; j < 13; j++)
                {
                    for (int k = 0; k < numFilters; k++)
                    {
                        mfcc[i][j] += melSpectrum[i][k] * (float)Math.Cos(Math.PI * j * (k + 0.5) / numFilters);
                    }
                }
            }

            // Flatten the MFCCs into a single array
            List<float> mfccFeatures = new List<float>();
            foreach (var frame in mfcc)
            {
                mfccFeatures.AddRange(frame);
            }

            return mfccFeatures.ToArray();
        }
        private float[] ExtractSpectralContrastFeatures(Complex[] fftBuffer, int numBands = 6) {
            if (fftBuffer == null || fftBuffer.Length == 0)
            {
                throw new ArgumentException("FFT buffer must not be null or empty.");
            }

            float[] spectralContrast = new float[numBands];
            int[] bandEdges = new int[numBands + 1];

            // Define frequency bands more evenly
            for (int i = 0; i <= numBands; i++)
            {
                bandEdges[i] = (int)Math.Round(i * (fftBuffer.Length / (double)numBands));
            }

            for (int band = 0; band < numBands; band++)
            {
                int start = bandEdges[band];
                int end = bandEdges[band + 1];

                List<double> magnitudes = new List<double>();
                for (int i = start; i < end; i++)
                {
                    double magnitude = Math.Sqrt(fftBuffer[i].X * fftBuffer[i].X + fftBuffer[i].Y * fftBuffer[i].Y);
                    magnitudes.Add(magnitude);
                }

                magnitudes.Sort();
                double median = magnitudes[magnitudes.Count / 2];
                double min = magnitudes[0];
                double max = magnitudes[magnitudes.Count - 1];

                spectralContrast[band] = (float)(max - min) / (float)(median + 1e-10);

            }

            return spectralContrast;
        }
        private float ExtractSpectralCentroidFeatures(Complex[] fftBuffer) {
            double weightedSum = 0;
            double totalMagnitude = 0;
            int numBins = fftBuffer.Length / 2;


            for (int i = 0; i < numBins; i++)
            {
                double magnitude = Math.Sqrt(fftBuffer[i].X * fftBuffer[i].X + fftBuffer[i].Y * fftBuffer[i].Y);
                weightedSum += i * magnitude;
                totalMagnitude += magnitude;


            }

            float spectralCentroid = (float)(weightedSum / totalMagnitude);

            return spectralCentroid;
        }
        private float ExtractSpectralBandwidthFeatures(float[] buffer) {
            // Perform Fourier Transform
            int fftSize = (int)Math.Pow(2, Math.Ceiling(Math.Log(buffer.Length, 2)));
            Complex[] fftBuffer = new Complex[fftSize];
            for (int i = 0; i < buffer.Length; i++)
            {
                fftBuffer[i].X = buffer[i];
                fftBuffer[i].Y = 0;
            }

            FastFourierTransform.FFT(true, (int)Math.Log(fftSize, 2.0), fftBuffer);

            // Calculate Magnitudes
            double[] magnitudes = fftBuffer.Select(c => Math.Sqrt(c.X * c.X + c.Y * c.Y)).ToArray();

            // Calculate Spectral Bandwidth
            double mean = magnitudes.Average();
            double sum = magnitudes.Sum(m => Math.Pow(m - mean, 2));
            double spectralBandwidth = Math.Sqrt(sum / magnitudes.Length);

            return (float)spectralBandwidth;
        }
        private float ExtractZeroCrossingRateFeatures(float[] buffer) {
            int zeroCrossings = 0;
            for (int i = 1; i < buffer.Length; i++)
            {
                if ((buffer[i - 1] >= 0 && buffer[i] < 0) || (buffer[i - 1] < 0 && buffer[i] >= 0))
                {
                    zeroCrossings++;
                }
            }
            return (float)zeroCrossings / buffer.Length;
        }
        private float ExtractRmsEnergyFeatures(float[] buffer) {
            double sumOfSquares = buffer.Select(sample => sample * sample).Sum();
            return (float)Math.Sqrt(sumOfSquares / buffer.Length);
        }
        private float ExtractPitchFeatures(float[] buffer, int sampleRate) {
            int maxLag = sampleRate / 50; // Minimum frequency of 50 Hz
            int minLag = sampleRate / 500; // Maximum frequency of 500 Hz
            float[] autoCorrelation = new float[maxLag];

            // Calculate auto-correlation
            for (int lag = minLag; lag < maxLag; lag++)
            {
                for (int i = 0; i < buffer.Length - lag; i++)
                {
                    autoCorrelation[lag] += buffer[i] * buffer[i + lag];
                }
            }

            // Find the lag with the maximum auto-correlation value
            int bestLag = minLag;
            for (int lag = minLag + 1; lag < maxLag; lag++)
            {
                if (autoCorrelation[lag] > autoCorrelation[bestLag])
                {
                    bestLag = lag;
                }
            }

            // Calculate the pitch
            float pitch = sampleRate / (float)bestLag;
            return pitch;
        }
    }
}