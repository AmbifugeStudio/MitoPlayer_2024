using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using NAudio.Wave;
using NAudio.Dsp;
using MitoPlayer_2024.Model;
using MitoPlayer_2024.Dao;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace MitoPlayer_2024.Helpers
{
    public static class Class1
    {
        public static List<Track> tracks = new List<Track>();

        public static void AddTrack(string filePath, string keycode)
        {
            var key = TransformKeycodeToKey(keycode);
            var features = ExtractChromaFeatures(filePath);
            tracks.Add(new Track { Path = filePath, Key = key, Features = features });
        }

        private static string TransformKeycodeToKey(string keycode)
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

        public static void GenerateCsv(String fileName)
        {
            string debugDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string fullPath = Path.Combine(debugDirectory, fileName);

            if (!File.Exists(fullPath))
            {
                using (File.Create(fullPath)) { }
            }

            using (var writer = new StreamWriter(fullPath))
            {
                writer.WriteLine("Label,Feature1,Feature2,Feature3,...,Feature12");

                foreach (var track in tracks)
                {
                    var line = $"{track.Key},{string.Join(",", track.Features)}";
                    writer.WriteLine(line);
                }
            }
        }
        private static float[] ExtractChromaFeatures(string filePath)
        {
            using (var reader = new AudioFileReader(filePath))
            {
                var sampleProvider = reader.ToSampleProvider();
                float[] buffer = new float[reader.WaveFormat.SampleRate];
                int samplesRead;
                List<float> chromaFeatures = new List<float>(new float[12]);
                int totalSamples = (int)reader.Length / buffer.Length;
                int currentSample = 0;

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
                    for (int i = 0; i < fftBuffer.Length / 2; i++)
                    {
                        /*double magnitude = Math.Sqrt(fftBuffer[i].X * fftBuffer[i].X + fftBuffer[i].Y * fftBuffer[i].Y);
                        double frequency = i * reader.WaveFormat.SampleRate / buffer.Length;
                        int pitchClass = (int)Math.Round(12 * Log2(frequency / 440.0) + 69) % 12;
                        if (pitchClass < 0) pitchClass += 12; // Ensure pitchClass is within 0-11
                        chromaFeatures[pitchClass] += (float)magnitude;*/
                        double magnitude = Math.Sqrt(fftBuffer[i].X * fftBuffer[i].X + fftBuffer[i].Y * fftBuffer[i].Y);
                        int pitchClass = (int)Math.Round(12 * Log2(magnitude)) % 12;
                        chromaFeatures[pitchClass] += (float)magnitude;
                    }

                    currentSample++;
                    int progressValue = currentSample * 100 / totalSamples;
                    Console.WriteLine($"Processing {filePath}: {progressValue}% complete (Sample {currentSample} of {totalSamples})");
                }

                return NormalizeFeatures(chromaFeatures.ToArray());
            }

            /* using (var reader = new AudioFileReader(filePath))
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
                                    double frequency = i * reader.WaveFormat.SampleRate / buffer.Length;
                                    int pitchClass = (int)Math.Round(12 * Log2(frequency / 440.0) + 69) % 12;
                                    if (pitchClass < 0) pitchClass += 12; // Ensure pitchClass is within 0-11
                                    chromaFeatures[pitchClass] += (float)magnitude;
                                }

                                currentSample++;
                                int progressValue = currentSample * 100 / totalSamples;
                                progress.Report(progressValue);
                                Console.WriteLine($"Processing {filePath}: {progressValue}% complete");
                            }

                            return NormalizeFeatures(chromaFeatures.ToArray());
                        }*/
        }
        private static float[] NormalizeFeatures(float[] features)
        {
            float max = features.Max();
            if (max == 0) return features; // Avoid division by zero
            for (int i = 0; i < features.Length; i++)
            {
                features[i] /= max;
            }
            return features;
        }
        /* private static float[] ExtractChromaFeatures(string filePath)
         {
             using (var reader = new AudioFileReader(filePath))
             {
                 var sampleProvider = reader.ToSampleProvider();
                 float[] buffer = new float[reader.WaveFormat.SampleRate];
                 int samplesRead;
                 List<float> chromaFeatures = new List<float>(new float[12]);

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
                         double frequency = i * reader.WaveFormat.SampleRate / buffer.Length;
                         int pitchClass = (int)Math.Round(12 * Log2(frequency / 440.0) + 69) % 12;
                         if (pitchClass < 0) pitchClass += 12; // Ensure pitchClass is within 0-11
                         chromaFeatures[pitchClass] += (float)magnitude;
                     }
                 }

                 return chromaFeatures.ToArray();
             }
         }*/

        private static double Log2(double x)
        {
            return Math.Log(x) / Math.Log(2);
        }

        /* public static void Main(string[] args)
         {
             var generator = new KeyTrainingDataGenerator();

             while (true)
             {
                 Console.WriteLine("Enter the file path of the track (or 'exit' to finish):");
                 var filePath = Console.ReadLine();
                 if (filePath.ToLower() == "exit") break;

                 Console.WriteLine("Enter the key of the track (e.g., C Major, A Minor):");
                 var key = Console.ReadLine();

                 generator.AddTrack(filePath, key);
             }

             Console.WriteLine("Enter the output path for the CSV file:");
             var outputPath = Console.ReadLine();
             generator.GenerateCsv(outputPath);

             Console.WriteLine("CSV file generated successfully!");
         }*/
    }


}
