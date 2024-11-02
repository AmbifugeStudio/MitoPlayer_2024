using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NAudio.Wave;
using NAudio.Dsp;
using MitoPlayer_2024.Model;
using MitoPlayer_2024.Dao;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using MitoPlayer_2024.Views;
using System.Windows.Forms;
using System.Collections.Concurrent;

namespace MitoPlayer_2024.Helpers
{
    public class KeyTrainingDataGenerator
    {
        public static KeyTrainingDataGenerator instance;
        public static KeyTrainingDataGenerator GetInstance()
        {
            if (instance == null)
            {
                instance = new KeyTrainingDataGenerator();
            }

            return instance;
        }

        public KeyTrainingDataGenerator()
        {

        }

        public ConcurrentBag<Track> tracks = new ConcurrentBag<Track>();
        public void AddTrack(string filePath, string keycode)
        {
            var key = TransformKeycodeToKey(keycode);
            var features = ExtractFeatures(filePath);
            tracks.Add(new Track { Path = filePath, Key = key, Features = features });
        }

        private string TransformKeycodeToKey(string keycode)
        {
            var keyMap = new Dictionary<string, string>
            {
            { "01A", "A Minor" }, { "02A", "E Minor" }, { "03A", "B Minor" }, { "04A", "F# Minor" },
            { "05A", "C# Minor" }, { "06A", "G# Minor" }, { "07A", "D# Minor" }, { "08A", "A# Minor" },
            { "09A", "F Minor" }, { "10A", "C Minor" }, { "11A", "G Minor" }, { "12A", "D Minor" },
            { "01B", "C Major" }, { "02B", "G Major" }, { "03B", "D Major" }, { "04B", "A Major" },
            { "05B", "E Major" }, { "06B", "B Major" }, { "07B", "F# Major" }, { "08B", "C# Major" },
            { "09B", "G# Major" }, { "10B", "D# Major" }, { "11B", "A# Major" }, { "12B", "F Major" }
            }; 
            return keyMap.ContainsKey(keycode) ? keyMap[keycode] : "Unknown";
        }

        public void WriteToCsv(string fileName)
        {
            string debugDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string fullPath = Path.Combine(debugDirectory, fileName);

            // Ensure the file is created if it doesn't exist
            bool fileExists = File.Exists(fullPath);
            if (!fileExists)
            {
                using (File.Create(fullPath)) { }
            }

            using (var writer = new StreamWriter(fullPath, append: true))
            {
                // Write the header if the file is newly created
                if (!fileExists)
                {
                    writer.WriteLine("Path,Key,Feature1,Feature2,Feature3,...,FeatureN");
                }

                // Write the data
                foreach (var track in tracks)
                {
                    var line = $"{track.Path},{track.Key},{string.Join(",", track.Features)}";
                    writer.WriteLine(line);
                }
            }
        }

        public void GenerateCsv(string fileName)
        {
            string debugDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string fullPath = Path.Combine(debugDirectory, fileName);

            // Ensure the file is created if it doesn't exist
            if (!File.Exists(fullPath))
            {
                using (File.Create(fullPath)) { }
            }

            using (var writer = new StreamWriter(fullPath))
            {
                // Write the header
                writer.WriteLine("Path,Key,Feature1,Feature2,Feature3,...,FeatureN");

                // Write the data
                foreach (var track in tracks)
                {
                    var line = $"{track.Path},{track.Key},{string.Join(",", track.Features)}";
                    writer.WriteLine(line);
                }
            }
        }

        bool enableExtractChromaFeaturesLog = false;
        bool enableExtractMFCCsLog = false;
        bool enableExtractSpectralContrastLog = false;
        bool enableExtractHPCPLog = false;
        bool enableExtractSpectralCentroidLog = false;
        bool enableExtractFeaturesLog = false;

        private float[] ExtractFeatures(string filePath)
        {
            using (var reader = new AudioFileReader(filePath))
            {
                var sampleProvider = reader.ToSampleProvider();
                float[] buffer = new float[reader.WaveFormat.SampleRate * 2];
                int samplesRead;
                List<float> features = new List<float>();
                int totalSamples = (int)Math.Ceiling((double)reader.Length / (buffer.Length * 2 * 2)); // Adjust for bytes per sample and channels
                int currentSample = 0;

                if(enableExtractFeaturesLog)
                    Console.WriteLine($"Total samples: {totalSamples}");

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
                    float[] chromaFeatures = ExtractChromaFeatures(fftBuffer);
                    features.AddRange(chromaFeatures);

                    // Extract MFCCs
                    float[] mfccFeatures = ExtractMFCCs(buffer, reader.WaveFormat.SampleRate);
                    features.AddRange(mfccFeatures);

                    // Extract spectral contrast
                    float[] spectralContrastFeatures = ExtractSpectralContrast(fftBuffer, reader.WaveFormat.SampleRate);
                    features.AddRange(spectralContrastFeatures);

                    // Extract HPCP
                    /* float[] hpcpFeatures = ExtractHPCP(fftBuffer, reader.WaveFormat.SampleRate);
                     features.AddRange(hpcpFeatures);*/

                    // Harmonic/Percussive Source Separation
                    var (harmonic, percussive) = HarmonicPercussiveSeparation(buffer, reader.WaveFormat.SampleRate);
                    features.AddRange(harmonic);
                    features.AddRange(percussive);

                    // Extract spectral centroid
                    float spectralCentroid = ExtractSpectralCentroid(fftBuffer, reader.WaveFormat.SampleRate);
                    features.Add(spectralCentroid);

                    // Extract Tonnetz features
                    float[] tonnetzFeatures = ExtractTonnetzFeatures(chromaFeatures);
                    features.AddRange(tonnetzFeatures);

                    currentSample++;
                    int progressValue = currentSample * 100 / totalSamples;

                    if (enableExtractFeaturesLog)
                        Console.WriteLine($"Processing {filePath}: {progressValue}% complete (Sample {currentSample} of {totalSamples})");

                }

                return features.ToArray();
            }
        }

        private float[] ExtractChromaFeatures(Complex[] fftBuffer)
        {
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

                // Log progress
                if (enableExtractChromaFeaturesLog)
                {
                    if (i % (totalBins / 10) == 0) // Log every 10% of progress
                    {
                        Console.WriteLine($"ExtractChromaFeatures progress: {i * 100 / totalBins}% complete");
                    }
                }
                
            }
            if (enableExtractChromaFeaturesLog)
                Console.WriteLine($"ExtractChromaFeatures progress: {100}% complete");

            return chromaFeatures;
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
            if(enableExtractMFCCsLog)
                Console.WriteLine("Step 1: Pre-emphasis completed.");

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
            if (enableExtractMFCCsLog)
                Console.WriteLine("Step 2: Framing completed.");


            // Step 3: Windowing
            for (int i = 0; i < numFrames; i++)
            {
                for (int j = 0; j < frameSize; j++)
                {
                    frames[i][j] *= (float)(0.54 - 0.46 * Math.Cos(2 * Math.PI * j / (frameSize - 1))); // Hamming window
                }
            }
            if (enableExtractMFCCsLog)
                Console.WriteLine("Step 3: Windowing completed.");

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
            if (enableExtractMFCCsLog)
                Console.WriteLine("Step 4: FFT and Power Spectrum completed.");

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
            if (enableExtractMFCCsLog)
                Console.WriteLine("Step 5: Mel Filter Banks completed.");


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
            if (enableExtractMFCCsLog)
                Console.WriteLine("Step 6: Mel Spectrum completed.");

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
            if (enableExtractMFCCsLog)
                Console.WriteLine("Step 7: Discrete Cosine Transform (DCT) completed.");

            // Flatten the MFCCs into a single array
            List<float> mfccFeatures = new List<float>();
            foreach (var frame in mfcc)
            {
                mfccFeatures.AddRange(frame);
            }
            if (enableExtractMFCCsLog)
                Console.WriteLine("MFCC extraction completed.");

            return mfccFeatures.ToArray();
        }
        private float MelScale(float freq, int sampleRate)
        {
            return 2595 * (float)Math.Log10(1 + freq / 700);
        }
        private float[] ExtractSpectralContrast(Complex[] fftBuffer, int sampleRate, int numBands = 6)
        {
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
            if(enableExtractSpectralContrastLog)
                Console.WriteLine("Frequency bands defined.");

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

                // Log progress for each band
                if (enableExtractSpectralContrastLog)
                    Console.WriteLine($"Processed band {band + 1}/{numBands}: start={start}, end={end}, min={min}, max={max}, median={median}");
            }

            if (enableExtractSpectralContrastLog)
                Console.WriteLine("Spectral contrast extraction completed.");
            return spectralContrast;
        }
        private float[] ExtractHPCP(Complex[] fftBuffer, int sampleRate)
        {
            float[] hpcp = new float[12];
            int numBins = fftBuffer.Length / 2;
            double tuningFrequency = 440.0; // A4 tuning frequency

            for (int i = 0; i < numBins; i++)
            {
                double magnitude = Math.Sqrt(fftBuffer[i].X * fftBuffer[i].X + fftBuffer[i].Y * fftBuffer[i].Y);
                if (magnitude > 0)
                {
                    double frequency = i * sampleRate / (double)fftBuffer.Length;
                    double pitchClass = 12 * Log2(frequency / tuningFrequency);
                    int pitchClassIndex = (int)Math.Round(pitchClass) % 12;
                    if (pitchClassIndex < 0)
                    {
                        pitchClassIndex += 12; // Ensure pitchClassIndex is non-negative
                    }
                    hpcp[pitchClassIndex] += (float)magnitude;
                }
                if (enableExtractHPCPLog)
                {
                    // Log progress every 10% of the bins processed
                    if (i % (numBins / 10) == 0)
                    {
                        int progress = (i * 100) / numBins;
                        Console.WriteLine($"HPCP extraction progress: {progress}% complete");
                    }
                }
            }

            return hpcp;
        }
        private float ExtractSpectralCentroid(Complex[] fftBuffer, int sampleRate)
        {
            double weightedSum = 0;
            double totalMagnitude = 0;
            int numBins = fftBuffer.Length / 2;

            if(enableExtractSpectralCentroidLog)
                Console.WriteLine("Starting spectral centroid calculation...");

            for (int i = 0; i < numBins; i++)
            {
                double magnitude = Math.Sqrt(fftBuffer[i].X * fftBuffer[i].X + fftBuffer[i].Y * fftBuffer[i].Y);
                weightedSum += i * magnitude;
                totalMagnitude += magnitude;

                if (enableExtractSpectralCentroidLog)
                {
                    // Log progress every 10% of the bins processed
                    if (i % (numBins / 10) == 0)
                    {
                        int progress = (i * 100) / numBins;
                        Console.WriteLine($"Spectral centroid calculation progress: {progress}% complete");
                    }
                }
                    
            }

            float spectralCentroid = (float)(weightedSum / totalMagnitude);
            if (enableExtractSpectralCentroidLog)
                Console.WriteLine($"Spectral centroid calculation completed. Result: {spectralCentroid}");

            return spectralCentroid;
        }
        private (float[] harmonic, float[] percussive) HarmonicPercussiveSeparation(float[] buffer, int sampleRate)
        {
            // Convert buffer to spectrogram
            var spectrogram = ComputeSpectrogram(buffer, sampleRate);

            // Apply horizontal median filter for harmonic components
            var harmonicSpectrogram = ApplyMedianFilter(spectrogram, filterDirection: "horizontal");

            // Apply vertical median filter for percussive components
            var percussiveSpectrogram = ApplyMedianFilter(spectrogram, filterDirection: "vertical");

            // Convert spectrograms back to time domain
            var harmonic = InverseSpectrogram(harmonicSpectrogram, sampleRate);
            var percussive = InverseSpectrogram(percussiveSpectrogram, sampleRate);

            return (harmonic, percussive);
        }

        private float[][] ComputeSpectrogram(float[] buffer, int sampleRate, int fftSize = 512, int hopSize = 256)
        {
            int numFrames = (buffer.Length - fftSize) / hopSize + 1;
            float[][] spectrogram = new float[numFrames][];

            for (int i = 0; i < numFrames; i++)
            {
                float[] frame = new float[fftSize];
                Array.Copy(buffer, i * hopSize, frame, 0, fftSize);

                // Apply a window function (e.g., Hamming window)
                for (int j = 0; j < fftSize; j++)
                {
                    frame[j] *= (float)(0.54 - 0.46 * Math.Cos(2 * Math.PI * j / (fftSize - 1)));
                }

                // Perform FFT
                Complex[] fftBuffer = new Complex[fftSize];
                for (int j = 0; j < fftSize; j++)
                {
                    fftBuffer[j] = new Complex { X = frame[j], Y = 0 };
                }

                FastFourierTransform.FFT(true, (int)Math.Log(fftSize, 2.0), fftBuffer);

                // Compute magnitude spectrum
                float[] magnitudeSpectrum = new float[fftSize / 2 + 1];
                for (int j = 0; j < fftSize / 2 + 1; j++)
                {
                    magnitudeSpectrum[j] = (float)Math.Sqrt(fftBuffer[j].X * fftBuffer[j].X + fftBuffer[j].Y * fftBuffer[j].Y);
                }

                spectrogram[i] = magnitudeSpectrum;
            }

            return spectrogram;
        }



        private float[][] ApplyMedianFilter(float[][] spectrogram, string filterDirection)
        {
            int numRows = spectrogram.Length;
            int numCols = spectrogram[0].Length;
            float[][] filteredSpectrogram = new float[numRows][];

            for (int i = 0; i < numRows; i++)
            {
                filteredSpectrogram[i] = new float[numCols];
            }

            int filterSize = 3; // You can adjust the filter size as needed
            int halfFilterSize = filterSize / 2;

            if (filterDirection == "horizontal")
            {
                for (int i = 0; i < numRows; i++)
                {
                    for (int j = 0; j < numCols; j++)
                    {
                        List<float> window = new List<float>();

                        for (int k = -halfFilterSize; k <= halfFilterSize; k++)
                        {
                            int colIndex = j + k;
                            if (colIndex >= 0 && colIndex < numCols)
                            {
                                window.Add(spectrogram[i][colIndex]);
                            }
                        }

                        window.Sort();
                        filteredSpectrogram[i][j] = window[window.Count / 2];
                    }
                }
            }
            else if (filterDirection == "vertical")
            {
                for (int j = 0; j < numCols; j++)
                {
                    for (int i = 0; i < numRows; i++)
                    {
                        List<float> window = new List<float>();

                        for (int k = -halfFilterSize; k <= halfFilterSize; k++)
                        {
                            int rowIndex = i + k;
                            if (rowIndex >= 0 && rowIndex < numRows)
                            {
                                window.Add(spectrogram[rowIndex][j]);
                            }
                        }

                        window.Sort();
                        filteredSpectrogram[i][j] = window[window.Count / 2];
                    }
                }
            }
            else
            {
                throw new ArgumentException("Invalid filter direction. Use 'horizontal' or 'vertical'.");
            }

            return filteredSpectrogram;
        }

        private float[] InverseSpectrogram(float[][] spectrogram, int sampleRate, int fftSize = 512, int hopSize = 256)
        {
            int numFrames = spectrogram.Length;
            int frameSize = fftSize;
            float[] reconstructedSignal = new float[numFrames * hopSize + frameSize];

            for (int i = 0; i < numFrames; i++)
            {
                // Create a complex array for the inverse FFT
                Complex[] ifftBuffer = new Complex[fftSize];
                for (int j = 0; j < fftSize / 2 + 1; j++)
                {
                    ifftBuffer[j] = new Complex { X = spectrogram[i][j], Y = 0 };
                    if (j > 0 && j < fftSize / 2)
                    {
                        ifftBuffer[fftSize - j] = new Complex { X = spectrogram[i][j], Y = 0 };
                    }
                }

                // Perform inverse FFT
                FastFourierTransform.FFT(false, (int)Math.Log(fftSize, 2.0), ifftBuffer);

                // Apply the window function (e.g., Hamming window)
                for (int j = 0; j < frameSize; j++)
                {
                    ifftBuffer[j].X *= (float)(0.54 - 0.46 * Math.Cos(2 * Math.PI * j / (frameSize - 1)));
                }

                // Overlap-add the reconstructed frame to the output signal
                for (int j = 0; j < frameSize; j++)
                {
                    reconstructedSignal[i * hopSize + j] += ifftBuffer[j].X;
                }
            }

            return reconstructedSignal;
        }


       /* private float[] ExtractTonnetzFeatures(float[] chromaFeatures)
        {
            // Implement Tonnetz feature extraction here
            // This is a placeholder example
            float[] tonnetzFeatures = new float[6];

            // Example calculation (this is simplified)
            for (int i = 0; i < chromaFeatures.Length; i++)
            {
                tonnetzFeatures[i % 6] += chromaFeatures[i];
            }

            return tonnetzFeatures;
        }*/

        private float[] ExtractTonnetzFeatures(float[] chromaFeatures)
        {
            // Ensure the chroma features are in the correct format
            if (chromaFeatures.Length % 12 != 0)
            {
                throw new ArgumentException("Chroma features length must be a multiple of 12.");
            }

            int numFrames = chromaFeatures.Length / 12;
            float[] tonnetzFeatures = new float[numFrames * 6];

            // Tonnetz transformation matrix
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

            for (int frame = 0; frame < numFrames; frame++)
            {
                float[] chromaFrame = new float[12];
                Array.Copy(chromaFeatures, frame * 12, chromaFrame, 0, 12);

                for (int i = 0; i < 6; i++)
                {
                    float tonnetzValue = 0;
                    for (int j = 0; j < 12; j++)
                    {
                        tonnetzValue += tonnetzMatrix[i, j] * chromaFrame[j];
                    }
                    tonnetzFeatures[frame * 6 + i] = tonnetzValue;
                }
            }

            return tonnetzFeatures;
        }

        private static double Log2(double x)
        {
            return Math.Log(x) / Math.Log(2);
        }

    }

}
