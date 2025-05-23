﻿
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers.LightGbm;
using NAudio.Dsp;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using MathNet.Numerics.IntegralTransforms;
using MathNet.Numerics;
using System.Threading;
using MathNet.Numerics.Statistics;
using NWaves.Transforms;

namespace MitoPlayer_2024.Helpers
{
    public class KeyDetector
    {

        private MLContext _mlContext;
        private ITransformer _model;

        public static KeyDetector instance;
        public static KeyDetector GetInstance()
        {
            if (instance == null)
            {
                instance = new KeyDetector();
            }

            return instance;
        }

        public KeyDetector()
        {
            
        }



            public float[] ExtractFeatures(string filePath, int segmentSize, int sampleRate, CancellationToken cancellationToken)
            {
            List<float[]> chromaFeatureList = new List<float[]>();
            List<float> pitchFeatureList = new List<float>();
            List<float> spectralCentroidFeatureList = new List<float>();
            List<float> spectralBandwidthFeatureList = new List<float>();
            List<float[]> mfccFeatureList = new List<float[]>();
            List<float> zeroCrossingRateFeatureList = new List<float>();
            List<float> rmsEnergyFeatureList = new List<float>();

            using (var reader = new AudioFileReader(filePath))
                {
                    var sampleProvider = reader.ToSampleProvider();
                    float[] buffer = new float[segmentSize];
                    int samplesRead;

                    while ((samplesRead = sampleProvider.Read(buffer, 0, segmentSize)) > 0)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            break;
                        }

                    // Perform FFT
                    Complex[] fftBuffer = new Complex[buffer.Length];
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        fftBuffer[i].X = buffer[i];
                        fftBuffer[i].Y = 0;
                    }
                    FastFourierTransform.FFT(true, (int)Math.Log(buffer.Length, 2.0), fftBuffer);

                    // Extract features
                    chromaFeatureList.Add(CalculateChromaFeatures(fftBuffer, buffer, sampleRate));
                    pitchFeatureList.Add(CalculatePitchFeatures(buffer, sampleRate));
                    spectralCentroidFeatureList.Add(CalculateSpectralCentroidFeatures(fftBuffer));
                    spectralBandwidthFeatureList.Add(CalculateSpectralBandwidthFeatures(buffer));
                    mfccFeatureList.Add(CalculateMfccFeatures(buffer, sampleRate));
                    zeroCrossingRateFeatureList.Add(CalculateZeroCrossingRateFeatures(buffer, sampleRate));
                    rmsEnergyFeatureList.Add(CalculateRmsEnergyFeatures(buffer, sampleRate));
                }
                }
            // Aggregate features
            float[] trainingData = AggregateFeatures(chromaFeatureList, pitchFeatureList, spectralCentroidFeatureList, spectralBandwidthFeatureList, mfccFeatureList, zeroCrossingRateFeatureList, rmsEnergyFeatureList);
            return trainingData;
            }

        private const int chromaFeaturesLength = 12;
        private const int tonnetzFeaturesLength = 6;
        private const int mFCCFeaturesLength = 13;
        private const int hpcpFeaturesLength = 13;

        private const int harmonicPercussiveSourceSeparationFeaturesLength = 12;
        private const int harmonicFeaturesLength = 10;
        private const int percussiveFeaturesLength = 10;

        private const int spectralContrastFeaturesLength = 6;
        private const int spectralCentroidFeaturesLength = 1;
        private const int spectralBandwidthFeaturesLength = 1;

        private const int zeroCrossingRateFeaturesLength = 1;
        private const int rMSEnergyFeaturesLength = 1;
        private const int pitchFeaturesLength = 1;

        private float[] CalculateChromaFeatures(Complex[] fftBuffer, float[] buffer, int sampleRate)
        {
            float[] chromaFeatures = new float[chromaFeaturesLength];
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

        private float CalculatePitchFeatures(float[] buffer, int sampleRate)
        {
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
        private float CalculateSpectralCentroidFeatures(Complex[] fftBuffer)
        {
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
        private float CalculateSpectralBandwidthFeatures(float[] audioSamples)
        {
            // Perform Fourier Transform
            int fftSize = (int)Math.Pow(2, Math.Ceiling(Math.Log(audioSamples.Length, 2)));
            Complex[] fftBuffer = new Complex[fftSize];
            for (int i = 0; i < audioSamples.Length; i++)
            {
                fftBuffer[i].X = audioSamples[i];
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
        private float[] CalculateMfccFeatures(float[] buffer, int sampleRate, int fftSize = 512, int numFilters = 26, int numCoefficients = mFCCFeaturesLength)
        {
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
                mfcc[i] = new float[numCoefficients];
                for (int j = 0; j < numCoefficients; j++)
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
        private float MelScale(float freq)
        {
            return 2595 * (float)Math.Log10(1 + freq / 700);
        }
        private float CalculateZeroCrossingRateFeatures(float[] buffer, int sampleRate)
        {
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
        private float CalculateRmsEnergyFeatures(float[] buffer, int sampleRate)
        {
            double sumOfSquares = buffer.Select(sample => sample * sample).Sum();
            return (float)Math.Sqrt(sumOfSquares / buffer.Length);
        }

        private float[] AggregateFeatures(List<float[]> chromaFeaturesList, List<float> pitchFeatureList, List<float> spectralCentroidFeatureList, List<float> spectralBandwidthFeatureList, List<float[]> mfccFeatureList, List<float> zeroCrossingRateFeatureList, List<float> rmsEnergyFeatureList)
        {
            List<float> trainingData = new List<float>();

            // Aggregate chroma features
           /* if (chromaFeaturesList.Count > 0)
            {
                int numFeatures = chromaFeaturesList[0].Length;

                for (int i = 0; i < numFeatures; i++)
                {
                    var featureValues = chromaFeaturesList.Select(f => f[i]).ToArray();
                    trainingData.Add(Statistics.Mean(featureValues));
                    trainingData.Add(Statistics.Variance(featureValues));
                    trainingData.Add(Statistics.StandardDeviation(featureValues));
                    trainingData.Add(Statistics.Min(featureValues));
                    trainingData.Add(Statistics.Max(featureValues));
                    trainingData.Add(Statistics.Median(featureValues));
                    trainingData.Add(Statistics.Entropy(featureValues));
                    trainingData.Add(Statistics.Mode(featureValues));
                }
            }

            // Aggregate pitch features
            if (pitchFeatureList.Count > 0)
            {
                trainingData.Add(Statistics.Mean(pitchFeatureList));
                trainingData.Add(Statistics.Variance(pitchFeatureList));
                trainingData.Add(Statistics.StandardDeviation(pitchFeatureList));
                trainingData.Add(Statistics.Min(pitchFeatureList));
                trainingData.Add(Statistics.Max(pitchFeatureList));
                trainingData.Add(Statistics.Median(pitchFeatureList));
                trainingData.Add(Statistics.Entropy(pitchFeatureList));
                trainingData.Add(Statistics.Mode(pitchFeatureList));
            }
            // Aggregate spectral centroid features
            if (spectralCentroidFeatureList.Count > 0)
            {
                trainingData.Add(Statistics.Mean(spectralCentroidFeatureList));
                trainingData.Add(Statistics.Variance(spectralCentroidFeatureList));
                trainingData.Add(Statistics.StandardDeviation(spectralCentroidFeatureList));
                trainingData.Add(Statistics.Min(spectralCentroidFeatureList));
                trainingData.Add(Statistics.Max(spectralCentroidFeatureList));
                trainingData.Add(Statistics.Median(spectralCentroidFeatureList));
                trainingData.Add(Statistics.Entropy(spectralCentroidFeatureList));
                trainingData.Add(Statistics.Mode(spectralCentroidFeatureList));
            }

            // Aggregate spectral bandwidth features
            if (spectralBandwidthFeatureList.Count > 0)
            {
                trainingData.Add(Statistics.Mean(spectralBandwidthFeatureList));
                trainingData.Add(Statistics.Variance(spectralBandwidthFeatureList));
                trainingData.Add(Statistics.StandardDeviation(spectralBandwidthFeatureList));
                trainingData.Add(Statistics.Minimum(spectralBandwidthFeatureList));
                trainingData.Add(Statistics.Maximum(spectralBandwidthFeatureList));
                trainingData.Add(Statistics.Median(spectralBandwidthFeatureList));
                trainingData.Add(Statistics.Entropy(spectralBandwidthFeatureList));
                trainingData.Add(Statistics.Mode(spectralBandwidthFeatureList));
            }

            // Aggregate MFCC features
            if (mfccFeatureList.Count > 0)
            {
                int numFeatures = mfccFeatureList[0].Length;

                for (int i = 0; i < numFeatures; i++)
                {
                    var featureValues = mfccFeatureList.Select(f => f[i]).ToArray();
                    trainingData.Add(Statistics.Mean(featureValues));
                    trainingData.Add(Statistics.Variance(featureValues));
                    trainingData.Add(Statistics.StandardDeviation(featureValues));
                    trainingData.Add(Statistics.Minimum(featureValues));
                    trainingData.Add(Statistics.Maximum(featureValues));
                    trainingData.Add(Statistics.Median(featureValues));
                    trainingData.Add(Statistics.Entropy(featureValues));
                    trainingData.Add(Statistics.Mode(featureValues));
                }
            }

            // Aggregate zero crossing rate features
            if (zeroCrossingRateFeatureList.Count > 0)
            {
                trainingData.Add(Statistics.Mean(zeroCrossingRateFeatureList));
                trainingData.Add(Statistics.Variance(zeroCrossingRateFeatureList));
                trainingData.Add(Statistics.StandardDeviation(zeroCrossingRateFeatureList));
                trainingData.Add(Statistics.Minimum(zeroCrossingRateFeatureList));
                trainingData.Add(Statistics.Maximum(zeroCrossingRateFeatureList));
                trainingData.Add(Statistics.Median(zeroCrossingRateFeatureList));
                trainingData.Add(Statistics.Entropy(zeroCrossingRateFeatureList));
                trainingData.Add(Statistics.Mode(zeroCrossingRateFeatureList));
            }

            // Aggregate RMS energy features
            if (rmsEnergyFeatureList.Count > 0)
            {
                trainingData.Add(Statistics.Mean(rmsEnergyFeatureList));
                trainingData.Add(Statistics.Variance(rmsEnergyFeatureList));
                trainingData.Add(Statistics.StandardDeviation(rmsEnergyFeatureList));
                trainingData.Add(Statistics.Minimum(rmsEnergyFeatureList));
                trainingData.Add(Statistics.Maximum(rmsEnergyFeatureList));
                trainingData.Add(Statistics.Median(rmsEnergyFeatureList));
                trainingData.Add(Statistics.Entropy(rmsEnergyFeatureList));
                trainingData.Add(Statistics.Mode(rmsEnergyFeatureList));
            }
            */
            return trainingData.ToArray();
        }






























        public void TrainKeyDetector(String keyTrainingDataFileName)
        {
            _mlContext = new MLContext();
            _model = TrainModel(keyTrainingDataFileName);
            
        }

        public string DetectKey(string filePath)
        {            
             var features = ExtractFeatures(filePath);
             var predictionEngine = _mlContext.Model.CreatePredictionEngine<AudioData, KeyPrediction>(_model);
             var prediction = predictionEngine.Predict(new AudioData { Features = features });
             return TransformKeyToKeyCode(prediction.PredictedKey);
        }


        private float[] ExtractChromaFeatures(string filePath)
        {
            using (var reader = new AudioFileReader(filePath))
            {
                var sampleProvider = reader.ToSampleProvider();
                float[] buffer = new float[reader.WaveFormat.SampleRate];
                int samplesRead;
                List<float> chromaFeatures = new List<float>(new float[12]);
                int totalSamples = (int)reader.Length / buffer.Length;
                int currentSample = 0;

                while ((samplesRead = sampleProvider.Read(buffer, 0, buffer.Length)) > 0)
                {
                    // Perform FFT
                    Complex[] fftBuffer = new Complex[buffer.Length];
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        fftBuffer[i] = new Complex { X = buffer[i], Y = 0 };
                    }

                    FastFourierTransform.FFT(true, (int)Math.Log(buffer.Length, 2.0), fftBuffer);

                    // Extract chroma features from FFT result
                    for (int i = 0; i < fftBuffer.Length / 2; i++)
                    {
                        double magnitude = Math.Sqrt(fftBuffer[i].X * fftBuffer[i].X + fftBuffer[i].Y * fftBuffer[i].Y);
                        if (magnitude > 0)
                        {
                            int pitchClass = (int)Math.Round(12 * Log2(magnitude)) % 12;
                            if (pitchClass < 0)
                            {
                                pitchClass += 12; // Ensure pitchClass is non-negative
                            }
                            chromaFeatures[pitchClass] += (float)magnitude;
                        }
                    }
                    currentSample++;
                    int progressValue = currentSample * 100 / totalSamples;
                    Console.Write($"\rProcessing {filePath}: {progressValue}% complete (Sample {currentSample} of {totalSamples})");
                }

                return chromaFeatures.ToArray();
            }
        }

        private float[] ExtractFeatures(string filePath)
        {
            using (var reader = new AudioFileReader(filePath))
            {
                var sampleProvider = reader.ToSampleProvider();
                float[] buffer = new float[reader.WaveFormat.SampleRate];
                int samplesRead;
                List<float> features = new List<float>();

                while ((samplesRead = sampleProvider.Read(buffer, 0, buffer.Length)) > 0)
                {
                    // Perform FFT and extract chroma features
                    Complex[] fftBuffer = new Complex[buffer.Length];
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        fftBuffer[i] = new Complex { X = buffer[i], Y = 0 };
                    }

                    FastFourierTransform.FFT(true, (int)Math.Log(buffer.Length, 2.0), fftBuffer);

                    // Extract chroma features
                    float[] chromaFeatures = ExtractChromaFeatures(fftBuffer);
                    features.AddRange(chromaFeatures);

                    // Extract MFCCs
                    float[] mfccFeatures = ExtractMFCCs(buffer, reader.WaveFormat.SampleRate);
                    features.AddRange(mfccFeatures);

                    // Extract spectral contrast
                    float[] spectralContrastFeatures = ExtractSpectralContrast(fftBuffer, reader.WaveFormat.SampleRate);
                    features.AddRange(spectralContrastFeatures);
                }

                return features.ToArray();
            }
        }

        private float[] chromaFeatures = new float[12];

        private float[] ExtractChromaFeatures(Complex[] fftBuffer)
        {
            this.chromaFeatures = new float[12];
            for (int i = 0; i < fftBuffer.Length / 2; i++)
            {
                double magnitude = Math.Sqrt(fftBuffer[i].X * fftBuffer[i].X + fftBuffer[i].Y * fftBuffer[i].Y);
                if (magnitude > 0)
                {
                    int pitchClass = (int)Math.Round(12 * Log2(magnitude)) % 12;
                    if (pitchClass < 0)
                    {
                        pitchClass += 12; // Ensure pitchClass is non-negative
                    }
                    this.chromaFeatures[pitchClass] += (float)magnitude;
                }
            }
            return this.chromaFeatures;
        }

        private float[] ExtractMFCCs(float[] buffer, int sampleRate, int numCoefficients = 13, int numFilters = 26, int fftSize = 512)
        {
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
                melPoints[i] = MelScale(i * (sampleRate / 2) / (numFilters + 1), sampleRate);
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
                mfcc[i] = new float[numCoefficients];
                for (int j = 0; j < numCoefficients; j++)
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

        private float MelScale(float freq, int sampleRate)
        {
            return 2595 * (float)Math.Log10(1 + freq / 700);
        }

        private float[] ExtractSpectralContrast(Complex[] fftBuffer, int sampleRate, int numBands = 6)
        {
            // Ensure the FFT buffer has been computed
            if (fftBuffer == null || fftBuffer.Length == 0)
            {
                throw new ArgumentException("FFT buffer must not be null or empty.");
            }

            // Initialize the spectral contrast array
            float[] spectralContrast = new float[numBands];

            // Define frequency bands
            int[] bandEdges = new int[numBands + 1];
            for (int i = 0; i <= numBands; i++)
            {
                bandEdges[i] = (int)Math.Round(Math.Pow(2, i) * sampleRate / (2 * fftBuffer.Length));
            }

            // Compute spectral contrast for each band
            for (int band = 0; band < numBands; band++)
            {
                int start = bandEdges[band];
                int end = bandEdges[band + 1];

                // Extract magnitudes for the current band
                List<double> magnitudes = new List<double>();
                for (int i = start; i < end; i++)
                {
                    double magnitude = Math.Sqrt(fftBuffer[i].X * fftBuffer[i].X + fftBuffer[i].Y * fftBuffer[i].Y);
                    magnitudes.Add(magnitude);
                }

                // Compute the spectral contrast for the current band
                magnitudes.Sort();
                double median = magnitudes[magnitudes.Count / 2];
                double min = magnitudes[0];
                double max = magnitudes[magnitudes.Count - 1];

                spectralContrast[band] = (float)(max - min) / (float)(median + 1e-10); // Add a small value to avoid division by zero
            }

            return spectralContrast;
        }

        private float[] ExtractTonnetzFeatures(Complex[] fftBuffer)
        {
            // Ensure the chroma features array has 12 elements
            if (this.chromaFeatures.Length != 12)
            {
                throw new ArgumentException("Chroma features array must have 12 elements.");
            }

            // Define the Tonnetz transformation matrix
            float[,] tonnetzMatrix = new float[,]
            {
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }
            };

            // Initialize the Tonnetz features array
            float[] tonnetzFeatures = new float[6];

            // Compute the Tonnetz features
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    tonnetzFeatures[i] += tonnetzMatrix[i, j] * this.chromaFeatures[j];
                }
            }

            return tonnetzFeatures;
        }




        private string TransformKeyToKeyCode(string key)
        {
            var keyCodeMap = new Dictionary<string, string>
            {
            { "A Minor", "01A" }, { "E Minor", "02A" }, { "B Minor", "03A" }, { "F# Minor", "04A" },
            { "C# Minor", "05A" }, { "G# Minor", "06A" }, { "D# Minor", "07A" }, { "A# Minor", "08A" },
            { "F Minor", "09A" }, { "C Minor", "10A" }, { "G Minor", "11A" }, { "D Minor", "12A" },
            { "C Major", "01B" }, { "G Major", "02B" }, { "D Major", "03B" }, { "A Major", "04B" },
            { "E Major", "05B" }, { "B Major", "06B" }, { "F# Major", "07B" }, { "C# Major", "08B" },
            { "G# Major", "09B" }, { "D# Major", "10B" }, { "A# Major", "11B" }, { "F Major", "12B" }
            };

            var uintToKeyMap = new Dictionary<uint, string>
            {
            { 0, "A Minor" }, { 1, "E Minor" }, { 2, "B Minor" }, { 3, "F# Minor" },
            { 4, "C# Minor" }, { 5, "G# Minor" }, { 6, "D# Minor" }, { 7, "A# Minor" },
            { 8, "F Minor" }, { 9, "C Minor" }, { 10, "G Minor" }, { 11, "D Minor" },
            { 12, "C Major" }, { 13, "G Major" }, { 14, "D Major" }, { 15, "A Major" },
            { 16, "E Major" }, { 17, "B Major" }, { 18, "F# Major" }, { 19, "C# Major" },
            { 20, "G# Major" }, { 21, "D# Major" }, { 22, "A# Major" }, { 23, "F Major" }
            };

            if (uint.TryParse(key, out uint keyInt) && uintToKeyMap.TryGetValue(keyInt, out string keyName) && keyCodeMap.TryGetValue(keyName, out string keyCode))
            {
                return keyCode;
            }
            return "Unknown";
        }

        private static readonly Dictionary<string, uint> KeyToUintMap = new Dictionary<string, uint>
        {
        { "A Minor", 0 }, { "E Minor", 1 }, { "B Minor", 2 }, { "F# Minor", 3 },
        { "C# Minor", 4 }, { "G# Minor", 5 }, { "D# Minor", 6 }, { "A# Minor", 7 },
        { "F Minor", 8 }, { "C Minor", 9 }, { "G Minor", 10 }, { "D Minor", 11 },
        { "C Major", 12 }, { "G Major", 13 }, { "D Major", 14 }, { "A Major", 15 },
        { "E Major", 16 }, { "B Major", 17 }, { "F# Major", 18 }, { "C# Major", 19 },
        { "G# Major", 20 }, { "D# Major", 21 }, { "A# Major", 22 }, { "F Major", 23 }
        };

        private double Log2(double x)
        {
            return Math.Log(x) / Math.Log(2);
        }
        public static class FeatureConstants
        {
            //public int FeatureCount = 44;
        }
        private class AudioData
        {
            [LoadColumn(0)]
            public string Path { get; set; }

            [LoadColumn(1)]
            public string Label { get; set; }

            [LoadColumn(2, 2 + 44- 1)]
            [VectorType(44)]
            public float[] Features { get; set; }
        }



        private ITransformer TrainModel(string fileName)
        {

            string debugDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string fullPath = Path.Combine(debugDirectory, fileName);

            try
            {
                Console.WriteLine("Starting model training...");

                // Step 1: Load training data
                Console.WriteLine("Step 1/6: Loading training data...");
                var data = _mlContext.Data.LoadFromTextFile<AudioData>(fullPath, separatorChar: ',', hasHeader: true);
                Console.WriteLine("Training data loaded.");

                // Step 2: Map string labels to uint values
                Console.WriteLine("Step 2/6: Mapping string labels to uint values...");
                var mappedData = _mlContext.Data.CreateEnumerable<AudioData>(data, reuseRowObject: false)
                .Select(row => new AudioData
                {
                    Label = KeyToUintMap.ContainsKey(row.Label) ? KeyToUintMap[row.Label].ToString() : "Unknown",
                    Features = row.Features
                });
                var mappedDataView = _mlContext.Data.LoadFromEnumerable(mappedData);
                Console.WriteLine("Labels mapped.");

                // Step 3: Shuffle data
                Console.WriteLine("Step 3/6: Shuffling data...");
                var shuffledData = _mlContext.Data.ShuffleRows(mappedDataView);
                Console.WriteLine("Data shuffled.");

                // Step 4: Define data preparation and training pipeline
                Console.WriteLine("Step 4/6: Defining data preparation and training pipeline...");
                var options = new LightGbmMulticlassTrainer.Options
                {
                    LabelColumnName = "Label",
                    FeatureColumnName = "Features",
                    NumberOfLeaves = 31, // Example hyperparameter
                    LearningRate = 0.05,  // Example hyperparameter
                    NumberOfIterations = 200, // Example hyperparameter
                    MinimumExampleCountPerLeaf = 20 // Example hyperparameter
                };

                var pipeline = _mlContext.Transforms.Concatenate("Features", nameof(AudioData.Features))
                .Append(_mlContext.Transforms.NormalizeMinMax("Features"))
                .Append(_mlContext.Transforms.Conversion.MapValueToKey("Label"))
                .Append(_mlContext.MulticlassClassification.Trainers.LightGbm(options))
                .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));
                Console.WriteLine("Pipeline defined.");

                /*
                 •  Number of Leaves: Start with 31 and adjust based on performance.
                •  Learning Rate: Common values range from 0.01 to 0.1.
                •  Number of Iterations: Start with 100 and increase if the model is underfitting.
                •  Max Depth: A value between 3 and 10 is usually effective.
                •  Min Data in Leaf: Start with 20 and adjust based on overfitting/underfitting.
                 */

                // Step 5: Perform cross-validation
                Console.WriteLine("Step 5/6: Performing cross-validation...");
                var crossValidationResults = _mlContext.MulticlassClassification.CrossValidate(shuffledData, pipeline, numberOfFolds: 5);
                Console.WriteLine("Cross-validation completed.");

                // Print cross-validation results
                foreach (var result in crossValidationResults)
                {
                    var metrics = result.Metrics;
                    Console.WriteLine($"Fold: {result.Fold}, MicroAccuracy: {metrics.MicroAccuracy}, MacroAccuracy: {metrics.MacroAccuracy}, LogLoss: {metrics.LogLoss}, LogLossReduction: {metrics.LogLossReduction}, TopKAccuracy: {metrics.TopKAccuracy}");

                    // Calculate precision, recall, and F1-score for each class
                    var confusionMatrix = metrics.ConfusionMatrix;
                    for (int i = 0; i < confusionMatrix.NumberOfClasses; i++)
                    {
                        var precision = confusionMatrix.PerClassPrecision[i];
                        var recall = confusionMatrix.PerClassRecall[i];
                        var f1Score = 2 * (precision * recall) / (precision + recall);
                        Console.WriteLine($"Class {i}: Precision: {precision}, Recall: {recall}, F1-Score: {f1Score}");
                    }
                }

                // Step 6: Train the model on the full dataset
                Console.WriteLine("Step 6/6: Training the model on the full dataset...");
                var model = pipeline.Fit(shuffledData);
                Console.WriteLine("Model training completed.");

                // Save the model
                string modelPath = Path.Combine(debugDirectory, "keyDetectionModel.zip");
                _mlContext.Model.Save(model, shuffledData.Schema, modelPath);
                Console.WriteLine($"Model saved to {modelPath}");

                return model;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }

        public void LoadModel()
        {
            ITransformer loadedModel;
            DataViewSchema modelSchema;
            using (var stream = new FileStream("keyDetectionModel.zip", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                loadedModel = _mlContext.Model.Load(stream, out modelSchema);
            }
        }


    }
}
