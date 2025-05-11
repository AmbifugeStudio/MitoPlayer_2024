using Accord.Audio;
using Accord.Math;
using Accord.Statistics.Models.Markov;
using Accord.Statistics.Models.Markov.Learning;
using Accord.Statistics.Models.Markov.Topology;
using Accord.Statistics.Distributions.Multivariate;
using HDF.PInvoke;
using MathNet.Numerics.LinearAlgebra;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Model;
using MitoPlayer_2024.Models;
using MitoPlayer_2024.Trainer;
using MitoPlayer_2024.Views;
using NAudio.Dsp;
using NAudio.Wave;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathNet.Numerics.Distributions;
using static MitoPlayer_2024.Presenters.ModelTrainerPresenter;
using Accord.Statistics.Distributions.Fitting;


namespace MitoPlayer_2024.Presenters
{
    public class ModelTrainerPresenter
    {
        private IModelTrainerView view;
        private ITagDao tagDao { get; set; }
        private ITrackDao trackDao { get; set; }
        private ISettingDao settingDao { get; set; }
        private List<Model.Track> trackList { get; set; }
        private DataTable inputTrackTable { get; set; }
        private BindingSource inputTrackListBindingSource { get; set; }
        private DataTable resultTrackTable { get; set; }
        private BindingSource resultTrackListBindingSource { get; set; }
        private DataTable trainingDataTable { get; set; }
        private BindingSource traningDataListBindingSource { get; set; }

        private List<Tag> TagList { get; set; }
        private List<Playlist> PlaylistList { get; set; }
        private List<TrainingData> TemplateList { get; set; }
        private Tag CurrentTag { get; set; }
        private TrackProperty CurrentTrackProperty { get; set; }
        private Playlist CurrentPlaylist { get; set; }
        private TrainingData CurrentTemplate { get; set; }
        private TrainingData CurrentTrainingData { get; set; }
        private Track currentTrack { get; set; }
       
        private bool IsTracklistDetailsDisplayed { get; set; }
        private int BatchSize { get; set; }

       
        public ModelTrainerPresenter(IModelTrainerView modelTrainerView, ITagDao tagDao,ITrackDao trackDao, ISettingDao settingDao)
        {
            this.view = modelTrainerView;
            this.tagDao = tagDao;
            this.trackDao = trackDao;
            this.settingDao = settingDao;

            this.view.SelectTag += SelectTag;
            this.view.SelectPlaylist += SelectPlaylist;
            this.view.SelectTemplate += SelectTemplate;

            this.view.CloseViewWithOk += CloseViewWithOk;

            this.view.IsChromaFeaturesEnabled += IsChromaFeaturesEnabledEvent;
            this.view.IsMFCCsEnabled += IsMFCCsEnabledEvent;
            this.view.IsSpectralContrastEnabled += IsSpectralContrastEnabledEvent;
            this.view.IsHPCPEnabled += IsHPCPEnabledEvent;
            this.view.IsHPSEnabled += IsHPSEnabledEvent;
            this.view.IsSpectralCentroidEnabled += IsSpectralCentroidEnabledEvent;
            this.view.IsTonnetzFeaturesEnabled += IsTonnetzFeaturesEnabledEvent;
            this.view.IsSpectralBandwidthEnabled += IsSpectralBandwidthEnabledEvent;
            this.view.IsZCREnabled += IsZCREnabledEvent;
            this.view.IsRMSEnabled += IsRMSEnabledEvent;
            this.view.IsPitchEnabled += IsPitchEnabledEvent;
            this.view.BatchProcessChanged += BatchProcessChangedEvent;
            this.view.CancelGenerationEvent += CancelGenerationEvent;
            this.view.LoadTrainingDataEvent += LoadTrainingDataEvent;
            this.view.TrainModelEvent += TrainModelEvent;
            this.view.DeleteTrainingDataEvent += DeleteTrainingDataEvent;
            this.view.SetIsTracklistDetailsDisplayedEvent += SetIsTracklistDetailsDisplayedEvent;
            this.view.CalculateDataSetQualityEvent += CalculateDataSetQualityEvent;

            this.view.AnalyseTrackEvent += AnalyseTrackEvent;
            this.view.SetCurrentFeatureTypeEvent += SetCurrentFeatureTypeEvent;


            this.view.GenerateTrainingData += GenerateTrainingDataEvent;

            this.TagList = this.tagDao.GetAllTag().Value.Where(x => !x.HasMultipleValues).ToList();
            this.PlaylistList = this.trackDao.GetAllPlaylist().Value.FindAll(x=>x.IsModelTrainer);
            this.TemplateList = this.trackDao.GetAllTrainingData().FindAll(x => x.IsTemplate);
            this.BatchSize = this.settingDao.GetIntegerSetting(Settings.TrainingModelBatchCount.ToString()).Value;
            this.IsTracklistDetailsDisplayed = this.settingDao.GetBooleanSetting(Settings.IsTracklistDetailsDisplayed.ToString()).Value.Value;

            this.InitializeTagsAndTagValues();

            this.view.InitializeView(this.TagList,this.PlaylistList, this.TemplateList, this.BatchSize, this.IsTracklistDetailsDisplayed);

            this.InitializeTrainingDataListStructure();
            this.InitializeTrainingDataListContent();

            this.ConcBag = new ObservableConcurrentBag<MessageTest>();
            this.ConcBag.ItemAdded += ConcBag_ItemAdded;

            this.currentFeatureType = FeatureType.Empty;


            this.InitializeTrainer();
            
        }

        

        private FeatureType currentFeatureType { get; set; }
        private void SetCurrentFeatureTypeEvent(object sender, Messenger e)
        {
            switch (e.StringField1)
            {
                case "Chroma": currentFeatureType = FeatureType.Chroma; break;
                case "MFCCs": currentFeatureType = FeatureType.MFCCs; break;
                case "HPCP": currentFeatureType = FeatureType.HPCP; break;
                case "HPSS": currentFeatureType = FeatureType.HPSS; break;
                case "SpectralContrast": currentFeatureType = FeatureType.SpectralContrast; break;
                case "SpectralCentroid": currentFeatureType = FeatureType.SpectralCentroid; break;
                case "SpectralBandwidth": currentFeatureType = FeatureType.SpectralBandwidth; break;
                case "Tonnetz": currentFeatureType = FeatureType.Tonnetz; break;
                case "ZCR": currentFeatureType = FeatureType.ZCR; break;
                case "RMS": currentFeatureType = FeatureType.RMS; break;
                case "Pitch": currentFeatureType = FeatureType.Pitch; break;
            }
        }
        // Step 1: Extract Chroma Features
        /* var extractor = new ChromaFeatureExtractor();
         float[][] chromaFeatures = extractor.ExtractChromaFeatures("path/to/your/audiofile.wav");

         // Step 2: Train the HMM (with example training data)
         var hmmKeyDetector = new HMMKeyDetector();
         float[][] trainingData = ...; // Your training chroma features
         int[] labels = ...; // Corresponding key labels as integers
         hmmKeyDetector.Train(trainingData, labels);

         // Step 3: Predict the Key
         string detectedKey = hmmKeyDetector.Predict(chromaFeatures[0]); // Use the first frame as an example
         Console.WriteLine($"Detected Key: {detectedKey}");
         */


        /*

        // Step 2: Detect Key Using KeyDetector
        var keyDetector = new KeyDetector();
        foreach (var frame in chromaFeatures)
        {
            string detectedKey = keyDetector.DetectKey(frame);
            Console.WriteLine($"Detected Key: {detectedKey}");
        }

        // Step 3: Train and Predict Using HMMKeyDetector
        var hmmKeyDetector = new HMMKeyDetector();

        // Example training data (you need to provide actual data)
        float[][] trainingData = chromaFeatures; // Use extracted chroma features as training data

        hmmKeyDetector.Train(trainingData);

        // Predict the key of the chroma feature sequence
        string predictedKey = hmmKeyDetector.Predict(chromaFeatures);
        Console.WriteLine($"Predicted Key: {predictedKey}");*/


        private ChromaFeatureExtractor chromaFeatureExtractor;
        private KeyDetector keyDetector;
        private HMMKeyDetector hmmKeyDetector;
        private void InitializeTrainer()
        {
            chromaFeatureExtractor = new ChromaFeatureExtractor();
            keyDetector = new KeyDetector();
            hmmKeyDetector = new HMMKeyDetector();

            //TODO
            // Train the HMM with a diverse dataset
           /* Dictionary<string, string> trainingData = new Dictionary<string, string>
            {
            { "path/to/song1.wav", "C Major" },
            { "path/to/song2.wav", "G Minor" },
            { "path/to/song3.wav", "A Major" }
            };
            TrainHMM(trainingData);

            // Validate the model
            Dictionary<string, string> validationData = new Dictionary<string, string>
            {
            { "path/to/song4.wav", "D Major" },
            { "path/to/song5.wav", "E Minor" }
            };
            ValidateModel(validationData);*/
        }

        public void TrainHMM(Dictionary<string, string> trainingData)
        {
            List<float[][]> trainingChromaFeatures = new List<float[][]>();
            List<int> labels = new List<int>();

            foreach (var entry in trainingData)
            {
                var chromaFeatures = chromaFeatureExtractor.ExtractChromaFeatures(entry.Key);
                trainingChromaFeatures.Add(chromaFeatures);

                // Convert key label to state index
                int label = KeyToStateIndex(entry.Value);
                labels.Add(label);
            }

            // Flatten the list of chroma features for training
            float[][] allChromaFeatures = trainingChromaFeatures.SelectMany(x => x).ToArray();
            int[] allLabels = labels.ToArray();

            hmmKeyDetector.Train(allChromaFeatures, allLabels);
        }

        public void ValidateModel(Dictionary<string, string> validationData)
        {
            int correctPredictions = 0;
            int totalPredictions = validationData.Count;

            foreach (var entry in validationData)
            {
                var chromaFeatures = chromaFeatureExtractor.ExtractChromaFeatures(entry.Key);
                string predictedKey = hmmKeyDetector.DetectKey(chromaFeatures);
                if (predictedKey == entry.Value)
                {
                    correctPredictions++;
                }
            }

            double accuracy = (double)correctPredictions / totalPredictions;
            Console.WriteLine($"Validation Accuracy: {accuracy * 100}%");
        }

        private int KeyToStateIndex(string key)
        {
            string[] majorKeys = { "C Major", "C# Major", "D Major", "D# Major", "E Major", "F Major", "F# Major", "G Major", "G# Major", "A Major", "A# Major", "B Major" };
            string[] minorKeys = { "C Minor", "C# Minor", "D Minor", "D# Minor", "E Minor", "F Minor", "F# Minor", "G Minor", "G# Minor", "A Minor", "A# Minor", "B Minor" };

            if (majorKeys.Contains(key))
            {
                return Array.IndexOf(majorKeys, key);
            }
            else
            {
                return Array.IndexOf(minorKeys, key) + 12;
            }
        }

        public string DetectKey(string filePath)
        {
            var chromaFeatures = chromaFeatureExtractor.ExtractChromaFeatures(filePath);

            // Use K-S algorithm
            float[] averageChroma = new float[12];
            foreach (var chroma in chromaFeatures)
            {
                for (int i = 0; i < chroma.Length; i++)
                {
                    averageChroma[i] += chroma[i];
                }
            }
            for (int i = 0; i < averageChroma.Length; i++)
            {
                averageChroma[i] /= chromaFeatures.Length;
            }

            string ksKey = keyDetector.DetectKey(averageChroma);

            // Use HMM
            string hmmKey = hmmKeyDetector.DetectKey(chromaFeatures);

            // Combine results or choose one based on your preference
            return $"K-S Detected Key: {ksKey}, HMM Detected Key: {hmmKey}";
        }
    

    public void AddTrack(string filePath, string tagValue, CancellationToken cancellationToken)
        {
            string detectedKey = DetectKey("path/to/new_song.wav");
            Console.WriteLine(detectedKey);
        }
        public class ChromaFeatureExtractor
        {
            public float[][] ExtractChromaFeatures(string filePath)
            {
                using (var reader = new AudioFileReader(filePath))
                {
                    int sampleRate = reader.WaveFormat.SampleRate;
                    int channels = reader.WaveFormat.Channels;
                    var buffer = new float[sampleRate * channels];
                    int samplesRead = reader.Read(buffer, 0, buffer.Length);

                    // Convert to mono
                    float[] monoBuffer = new float[samplesRead / channels];
                    for (int i = 0; i < samplesRead; i += channels)
                    {
                        monoBuffer[i / channels] = buffer[i];
                    }

                    // Frame the signal
                    int frameSize = 2048;
                    int hopSize = frameSize / 2;
                    int numFrames = (monoBuffer.Length - frameSize) / hopSize + 1;
                    float[][] chromaFeatures = new float[numFrames][];

                    for (int frame = 0; frame < numFrames; frame++)
                    {
                        float[] windowedFrame = new float[frameSize];
                        Array.Copy(monoBuffer, frame * hopSize, windowedFrame, 0, frameSize);

                        // Apply Hann window
                        for (int i = 0; i < frameSize; i++)
                        {
                            windowedFrame[i] = (float)(0.5 - 0.5 * Math.Cos(2 * Math.PI * i / (frameSize - 1)));
                        }

                        // Compute STFT
                        Complex[] complexSpectrum = windowedFrame.Select(v => new Complex { X = v, Y = 0 }).ToArray();
                        FastFourierTransform.FFT(true, (int)Math.Log(frameSize, 2.0), complexSpectrum);

                        // Calculate chroma vector
                        float[] chromaVector = new float[12];
                        for (int i = 0; i < complexSpectrum.Length / 2; i++)
                        {
                            double frequency = i * sampleRate / frameSize;
                            if (frequency > 0)
                            {
                                int pitchClass = (int)Math.Round(12 * Math.Log(frequency / 440.0, 2)) % 12;
                                if (pitchClass < 0) pitchClass += 12; // Ensure pitch class is between 0 and 11
                                chromaVector[pitchClass] += (float)Math.Sqrt(complexSpectrum[i].X * complexSpectrum[i].X + complexSpectrum[i].Y * complexSpectrum[i].Y);
                            }
                        }

                        // Normalize chroma vector
                        float max = chromaVector.Max();
                        if (max > 0)
                        {
                            for (int i = 0; i < chromaVector.Length; i++)
                            {
                                chromaVector[i] /= max;
                            }
                        }

                        chromaFeatures[frame] = chromaVector;
                    }

                    return chromaFeatures;
                }
            }
        }
        public class KeyDetector
        {
            private readonly float[][] majorTemplates;
            private readonly float[][] minorTemplates;

            public KeyDetector()
            {
                // Initialize K-S templates for major and minor keys
                majorTemplates = new float[12][];
                minorTemplates = new float[12][];

                // K-S profile values for major keys (C, C#, D, D#, E, F, F#, G, G#, A, A#, B)
                float[] majorProfile = { 6.35f, 2.23f, 3.48f, 2.33f, 4.38f, 4.09f, 2.52f, 5.19f, 2.39f, 3.66f, 2.29f, 2.88f };

                // K-S profile values for minor keys (C, C#, D, D#, E, F, F#, G, G#, A, A#, B)
                float[] minorProfile = { 6.33f, 2.68f, 3.52f, 5.38f, 2.60f, 3.53f, 2.54f, 4.75f, 3.98f, 2.69f, 3.34f, 3.17f };

                for (int i = 0; i < 12; i++)
                {
                    majorTemplates[i] = RotateArray(majorProfile, i);
                    minorTemplates[i] = RotateArray(minorProfile, i);
                }
            }
            private float[] RotateArray(float[] array, int shift)
            {
                float[] rotated = new float[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    rotated[i] = array[(i + shift) % array.Length];
                }
                return rotated;
            }

            public string DetectKey(float[] chromaFeatures)
            {
                float maxCorrelation = float.MinValue;
                string detectedKey = "Unknown";

                for (int i = 0; i < 12; i++)
                {
                    float majorCorrelation = Correlate(chromaFeatures, majorTemplates[i]);
                    float minorCorrelation = Correlate(chromaFeatures, minorTemplates[i]);

                    if (majorCorrelation > maxCorrelation)
                    {
                        maxCorrelation = majorCorrelation;
                        detectedKey = $"{GetNoteName(i)} Major";
                    }

                    if (minorCorrelation > maxCorrelation)
                    {
                        maxCorrelation = minorCorrelation;
                        detectedKey = $"{GetNoteName(i)} Minor";
                    }
                }

                return detectedKey;
            }
            private float Correlate(float[] chroma, float[] template)
            {
                float sum = 0;
                for (int i = 0; i < chroma.Length; i++)
                {
                    sum += chroma[i] * template[i];
                }
                return sum + 1e-10f; // Add a small epsilon value
            }


            private string GetNoteName(int index)
            {
                string[] noteNames = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
                return noteNames[index];
            }
        }
        public class HMMKeyDetector
        {
            private HiddenMarkovModel<MultivariateNormalDistribution, double[]> hmm;

            public HMMKeyDetector()
            {
                // Define the number of states (keys) and the dimensionality of the observations (chroma features)
                int numberOfStates = 24; // 12 major + 12 minor keys
                int observationDimension = 12; // Chroma features

                // Initialize the HMM with multivariate normal distributions
                hmm = new HiddenMarkovModel<MultivariateNormalDistribution, double[]>(numberOfStates, new MultivariateNormalDistribution(observationDimension));
            }

            public void Train(float[][] chromaFeatures, int[] labels)
            {
                // Convert float[][] to double[][]
                double[][] observations = chromaFeatures.Select(f => f.Select(v => (double)v).ToArray()).ToArray();

                // Create a sequence of observations for training
                double[][][] sequences = new double[1][][];
                sequences[0] = observations;

                // Set up the fitting options with regularization
                var fittingOptions = new NormalOptions
                {
                    Regularization = 1e-5 // Add a small regularization constant
                };

                // Train the HMM using the Baum-Welch algorithm with fitting options
                var teacher = new BaumWelchLearning<MultivariateNormalDistribution, double[]>(hmm)
                {
                    Tolerance = 0.01,
                    MaxIterations = 100, // Set a reasonable number of iterations
                    FittingOptions = fittingOptions // Apply the fitting options
                };

                // Train the model
                hmm = teacher.Learn(sequences);

                // Log the likelihood of the sequences
                double logLikelihood = hmm.LogLikelihood(sequences[0]);
                Console.WriteLine($"Log Likelihood: {logLikelihood}");
            }


            public string DetectKey(float[][] chromaFeatures)
            {
                // Convert float[][] to double[][]
                double[][] observations = chromaFeatures.Select(f => f.Select(v => (double)v).ToArray()).ToArray();

                // Use the Viterbi algorithm to find the most likely sequence of states
                int[] path = hmm.Decode(observations);

                // Determine the most frequent state key in the path
                int mostLikelyState = path.GroupBy(p => p).OrderByDescending(g => g.Count()).First().Key;

                // Map the state to a key name
                string detectedKey = GetKeyName(mostLikelyState);

                return detectedKey;
            }

            private string GetKeyName(int state)
            {
                string[] majorKeys = { "C Major", "C# Major", "D Major", "D# Major", "E Major", "F Major", "F# Major", "G Major", "G# Major", "A Major", "A# Major", "B Major" };
                string[] minorKeys = { "C Minor", "C# Minor", "D Minor", "D# Minor", "E Minor", "F Minor", "F# Minor", "G Minor", "G# Minor", "A Minor", "A# Minor", "B Minor" };

                if (state < 12)
                {
                    return majorKeys[state];
                }
                else
                {
                    return minorKeys[state - 12];
                }
            }
        }


      /*  public class HMMKeyDetector
        {
            private HiddenMarkovModel<MultivariateNormalDistribution, double[]> hmm;

            public HMMKeyDetector()
            {
                // Initialize HMM with appropriate states and topology
                hmm = new HiddenMarkovModel<MultivariateNormalDistribution, double[]>(new Ergodic(24), new MultivariateNormalDistribution(12));
            }

            public void Train(float[][] trainingData, int[] labels)
            {
                var teacher = new BaumWelchLearning<MultivariateNormalDistribution, double[]>(hmm)
                {
                    Tolerance = 0.01,
                    MaxIterations = 100 // Set a reasonable number of iterations
                };

                // Convert trainingData to sequences of observations
                double[][][] sequences = trainingData.Select(data => data.Select(d => new double[] { d }).ToArray()).ToArray();

                // Flatten state sequences
                double[] stateSequences = labels.SelectMany(label => Enumerable.Repeat((double)label, sequences[0].Length)).ToArray();

                // Train the HMM
                teacher.Learn(sequences, stateSequences);
            }

            public string Predict(float[] chromaFeatures)
            {
                // Convert chromaFeatures to sequence of observations
                double[][] observations = chromaFeatures.Select(f => new double[] { f }).ToArray();

                // Use the Decide method
                int[] predictedStates = hmm.Decide(observations);

                // Get the most frequent state
                int predictedState = predictedStates.GroupBy(x => x).OrderByDescending(g => g.Count()).First().Key;
                return GetNoteName(predictedState);
            }

            private string GetNoteName(int index)
            {
                string[] noteNames = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
                if (index < 12)
                {
                    return noteNames[index] + " Major";
                }
                else
                {
                    return noteNames[index - 12] + " Minor";
                }
            }
        }

        */
        /*  public class HMMKeyDetector
          {
              private HiddenMarkovModel hmm;

              public HMMKeyDetector()
              {
                  // Initialize HMM with appropriate states and topology
                  hmm = new HiddenMarkovModel(new Forward(12), 12);
              }

              public void Train(float[][] trainingData, int[] labels)
              {
                  // Convert float[][] to int[][]
                  int[][] intTrainingData = trainingData.Select(seq => seq.Select(f => (int)f).ToArray()).ToArray();

                  var teacher = new BaumWelchLearning(hmm)
                  {
                      Tolerance = 0.01,
                      MaxIterations = 0
                  };
                  teacher.Learn(intTrainingData);
              }

              public string Predict(float[][] chromaFeatures)
              {
                  // Convert float[][] to int[][]
                  int[][] intChromaFeatures = chromaFeatures.Select(seq => seq.Select(f => (int)f).ToArray()).ToArray();

                  int[] predictedStates = hmm.Decide(intChromaFeatures);
                  int predictedState = predictedStates.Last(); // Get the last predicted state
                  return GetNoteName(predictedState);
              }

              private string GetNoteName(int index)
              {
                  string[] noteNames = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
                  return noteNames[index];
              }
          }
          */

















        private void AnalyseTrackEvent(object sender, Messenger e)
        {
            this.currentTrack = this.trackDao.GetTrackWithTags(e.IntegerField1, this.tagList).Value;
            if (this.currentTrack != null)
            {
                if (this.currentFeatureType != FeatureType.Empty)
                {
                    this.AnalyseTrack(this.currentTrack);
                }
            }
           
        }

        private void AnalyseTrack(Track track)
        {
            double songLengthInSeconds = track.Length;

            using (var reader = new AudioFileReader(track.Path))
            {
                if (currentFeatureType == FeatureType.Chroma)
                {
                    List<float[]> chromaFeatures = ExtractChromaFeaturesForAnalysing(track.Path);
                    PlotChromaFeatures(chromaFeatures, songLengthInSeconds);
                }
            }
        }

        public List<float[]> ExtractChromaFeaturesForAnalysingOld(string filePath)
        {
            using (var reader = new AudioFileReader(filePath))
            {
                var sampleProvider = reader.ToSampleProvider();
                int sampleRate = reader.WaveFormat.SampleRate;
                // int bufferSize = 8192; // Increased FFT size for better resolution
                int bufferSize = 2048; // Increased FFT size for better resolution
                float[] buffer = new float[bufferSize];
                List<float[]> chromaFeaturesList = new List<float[]>();

                int bytesRead;
                float[] pitchHistogram = new float[12]; // New line to initialize pitch histogram

                while ((bytesRead = sampleProvider.Read(buffer, 0, bufferSize)) > 0)
                {
                    // Apply windowing function
                    for (int i = 0; i < bufferSize; i++)
                    {
                        buffer[i] *= (float)(0.5 * (1 - Math.Cos(2 * Math.PI * i / (bufferSize - 1))));
                    }

                    // Apply FFT
                    Complex[] complexBuffer = new Complex[bufferSize];
                    for (int i = 0; i < bufferSize; i++)
                    {
                        complexBuffer[i].X = buffer[i];
                        complexBuffer[i].Y = 0;
                    }
                    FastFourierTransform.FFT(true, (int)Math.Log(bufferSize, 2.0), complexBuffer);

                    // LogFrequenciesAndKeys(complexBuffer, sampleRate, bufferSize);

                    // Calculate chroma features
                    float[] chroma = new float[12];
                    for (int i = 0; i < complexBuffer.Length / 2; i++)
                    {
                        double frequency = i * sampleRate / bufferSize;
                        if (frequency > 0) // Avoid log of zero or negative
                        {
                            int note = (int)Math.Round(12 * Log2(frequency / 440.0)) % 12;

                            if (note < 0) note += 12;
                            chroma[note] += (float)Math.Sqrt(complexBuffer[i].X * complexBuffer[i].X + complexBuffer[i].Y * complexBuffer[i].Y);
                            pitchHistogram[note] += chroma[note]; // New line to aggregate pitch histogram
                        }
                    }

                    chromaFeaturesList.Add(chroma);
                }

                // Normalize pitch histogram
                float maxPitchValue = pitchHistogram.Max();
                if (maxPitchValue > 0)
                {
                    for (int i = 0; i < pitchHistogram.Length; i++)
                    {
                        pitchHistogram[i] /= maxPitchValue;
                    }
                }

                // Weight chroma features by pitch histogram
                for (int i = 0; i < chromaFeaturesList.Count; i++)
                {
                    for (int j = 0; j < chromaFeaturesList[i].Length; j++)
                    {
                        chromaFeaturesList[i][j] *= pitchHistogram[j];
                    }
                }

                return chromaFeaturesList;
            }
        }


        public void DetectSongKey(string filePath)
        {
            List<float[]> chromaFeatures = this.ExtractChromaFeaturesForAnalysing(filePath);
            float[] aggregatedChroma = AggregateChromaFeatures(chromaFeatures);

            // Log aggregated chroma features for debugging
            Console.WriteLine("Aggregated Chroma Features:");
            for (int i = 0; i < aggregatedChroma.Length; i++)
            {
                Console.Write($"{aggregatedChroma[i]:F2} ");
            }
            Console.WriteLine();

            string key = DetectKey(aggregatedChroma);
            Console.WriteLine($"Song: {filePath} - Detected Key: {key}");
        }
        public List<float[]> ExtractChromaFeaturesForAnalysing(string filePath)
        {
            using (var reader = new AudioFileReader(filePath))
            {
                var sampleProvider = reader.ToSampleProvider();
                int sampleRate = reader.WaveFormat.SampleRate;
                int bufferSize = 2048; // Moderate buffer size for better time resolution
                int overlap = bufferSize / 2; // 50% overlap
                float[] buffer = new float[bufferSize];
                List<float[]> chromaFeaturesList = new List<float[]>();

                int bytesRead;
                float[] pitchHistogram = new float[12]; // Initialize pitch histogram

                long totalBytes = reader.Length;
                long processedBytes = 0;

                Console.WriteLine("Starting extraction process...");

                try
                {
                    while ((bytesRead = sampleProvider.Read(buffer, 0, bufferSize)) > 0)
                    {
                        Console.WriteLine($"Bytes read: {bytesRead}");

                        // Apply windowing function
                        Console.WriteLine("Applying windowing function...");
                        for (int i = 0; i < bufferSize; i++)
                        {
                            buffer[i] *= (float)(0.5 * (1 - Math.Cos(2 * Math.PI * i / (bufferSize - 1))));
                        }

                        // Apply FFT
                        Console.WriteLine("Applying FFT...");
                        Complex[] complexBuffer = new Complex[bufferSize];
                        for (int i = 0; i < bufferSize; i++)
                        {
                            complexBuffer[i].X = buffer[i];
                            complexBuffer[i].Y = 0;
                        }
                        FastFourierTransform.FFT(true, (int)Math.Log(bufferSize, 2.0), complexBuffer);

                        // Calculate chroma features
                        Console.WriteLine("Calculating chroma features...");
                        float[] chroma = new float[12];
                        for (int i = 0; i < complexBuffer.Length / 2; i++)
                        {
                            double frequency = i * sampleRate / bufferSize;
                            if (frequency > 0) // Avoid log of zero or negative
                            {
                                int note = (int)Math.Round(12 * Log2(frequency / 440.0)) % 12;
                                if (note < 0) note += 12;
                                chroma[note] += (float)Math.Sqrt(complexBuffer[i].X * complexBuffer[i].X + complexBuffer[i].Y * complexBuffer[i].Y);
                                pitchHistogram[note] += chroma[note]; // Aggregate pitch histogram
                                Console.WriteLine($"Frequency: {frequency}, Note: {note}, Chroma[{note}]: {chroma[note]}");
                            }
                        }

                        chromaFeaturesList.Add(chroma);

                        // Move back by the overlap amount to process the next window
                        long newPosition = reader.Position - overlap * sizeof(float);
                        Console.WriteLine($"Setting new position: {newPosition}");
                        if (newPosition < 0 || newPosition >= reader.Length)
                        {
                            Console.WriteLine("New position out of bounds, breaking loop.");
                            break; // Exit the loop if the new position is out of bounds
                        }
                        reader.Position = newPosition;
                        Console.WriteLine($"New reader position set: {reader.Position}");

                        // Update processed bytes and calculate progress
                        processedBytes = reader.Position;
                        double progress = (double)processedBytes / totalBytes * 100;
                        Console.WriteLine($"Processing: {progress:F2}%");

                        // Adjust or remove the sleep duration as needed
                        // System.Threading.Thread.Sleep(10); // Uncomment if necessary to prevent CPU overload
                    }

                    // Ensure the last segment is processed
                    if (processedBytes < totalBytes)
                    {
                        Console.WriteLine("Processing last segment...");
                        bytesRead = sampleProvider.Read(buffer, 0, bufferSize);
                        if (bytesRead > 0)
                        {
                            // Apply windowing function
                            for (int i = 0; i < bufferSize; i++)
                            {
                                buffer[i] *= (float)(0.5 * (1 - Math.Cos(2 * Math.PI * i / (bufferSize - 1))));
                            }

                            // Apply FFT
                            Complex[] complexBuffer = new Complex[bufferSize];
                            for (int i = 0; i < bufferSize; i++)
                            {
                                complexBuffer[i].X = buffer[i];
                                complexBuffer[i].Y = 0;
                            }
                            FastFourierTransform.FFT(true, (int)Math.Log(bufferSize, 2.0), complexBuffer);

                            // Calculate chroma features
                            float[] chroma = new float[12];
                            for (int i = 0; i < complexBuffer.Length / 2; i++)
                            {
                                double frequency = i * sampleRate / bufferSize;
                                if (frequency > 0) // Avoid log of zero or negative
                                {
                                    int note = (int)Math.Round(12 * Log2(frequency / 440.0)) % 12;
                                    if (note < 0) note += 12;
                                    chroma[note] += (float)Math.Sqrt(complexBuffer[i].X * complexBuffer[i].X + complexBuffer[i].Y * complexBuffer[i].Y);
                                    pitchHistogram[note] += chroma[note]; // Aggregate pitch histogram
                                    Console.WriteLine($"Frequency: {frequency}, Note: {note}, Chroma[{note}]: {chroma[note]}");
                                }
                            }

                            chromaFeaturesList.Add(chroma);
                        }
                    }

                    // Normalize pitch histogram
                    Console.WriteLine("Normalizing pitch histogram...");
                    float maxPitchValue = pitchHistogram.Max();
                    if (maxPitchValue > 0)
                    {
                        for (int i = 0; i < pitchHistogram.Length; i++)
                        {
                            pitchHistogram[i] /= maxPitchValue;
                        }
                    }

                    // Weight chroma features by pitch histogram
                    Console.WriteLine("Weighting chroma features by pitch histogram...");
                    for (int i = 0; i < chromaFeaturesList.Count; i++)
                    {
                        for (int j = 0; j < chromaFeaturesList[i].Length; j++)
                        {
                            chromaFeaturesList[i][j] *= pitchHistogram[j];
                        }
                    }

                    Console.WriteLine("Extraction process completed.");
                    return chromaFeaturesList;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return null;
                }
            }
        }
        public float[] AggregateChromaFeatures(List<float[]> chromaFeatures)
        {
            int numFrames = chromaFeatures.Count;
            int numPitchClasses = chromaFeatures[0].Length;
            float[] aggregatedChroma = new float[numPitchClasses];

            foreach (var frame in chromaFeatures)
            {
                for (int i = 0; i < numPitchClasses; i++)
                {
                    aggregatedChroma[i] += frame[i];
                }
            }

            // Normalize aggregated chroma features using dynamic thresholding
            float meanChromaValue = aggregatedChroma.Average();
            for (int i = 0; i < aggregatedChroma.Length; i++)
            {
                aggregatedChroma[i] /= meanChromaValue;
            }

            // Apply temporal smoothing
            aggregatedChroma = ApplyTemporalSmoothing(aggregatedChroma);

            return aggregatedChroma;
        }
        private float[] ApplyTemporalSmoothing(float[] chroma)
        {
            int windowSize = 5; // Adjust window size as needed
            float[] smoothedChroma = new float[chroma.Length];
            for (int i = 0; i < chroma.Length; i++)
            {
                float sum = 0;
                int count = 0;
                for (int j = Math.Max(0, i - windowSize / 2); j < Math.Min(chroma.Length, i + windowSize / 2 + 1); j++)
                {
                    sum += chroma[j];
                    count++;
                }
                smoothedChroma[i] = sum / count;
            }
            return smoothedChroma;
        }
        private float CalculateCosineSimilarity(float[] chroma, float[] profile)
        {
            float dotProduct = 0;
            float chromaMagnitude = 0;
            float profileMagnitude = 0;

            for (int i = 0; i < chroma.Length; i++)
            {
                dotProduct += chroma[i] * profile[i];
                chromaMagnitude += chroma[i] * chroma[i];
                profileMagnitude += profile[i] * profile[i];
            }

            return dotProduct / (float)(Math.Sqrt(chromaMagnitude) * Math.Sqrt(profileMagnitude));
        }
        public string DetectKey(float[] aggregatedChroma)
        {
            float maxSimilarity = float.MinValue;
            string detectedKey = "";

            for (int i = 0; i < 12; i++)
            {
                float similarity = CalculateCosineSimilarity(aggregatedChroma, majorProfiles[i]);
                if (similarity > maxSimilarity)
                {
                    maxSimilarity = similarity;
                    detectedKey = $"{i + 1}B"; // Camelot code for major keys
                }
            }

            for (int i = 0; i < 12; i++)
            {
                float similarity = CalculateCosineSimilarity(aggregatedChroma, minorProfiles[i]);
                if (similarity > maxSimilarity)
                {
                    maxSimilarity = similarity;
                    detectedKey = $"{i + 1}A"; // Camelot code for minor keys
                }
            }

            return detectedKey;
        }
        float[][] majorProfiles = new float[12][]
           {
            new float[] {6.35f, 2.23f, 3.48f, 2.33f, 4.38f, 4.09f, 2.52f, 5.19f, 2.39f, 3.66f, 2.29f, 2.88f}, // C major
            new float[] {2.88f, 6.35f, 2.23f, 3.48f, 2.33f, 4.38f, 4.09f, 2.52f, 5.19f, 2.39f, 3.66f, 2.29f}, // C# major
            new float[] {2.29f, 2.88f, 6.35f, 2.23f, 3.48f, 2.33f, 4.38f, 4.09f, 2.52f, 5.19f, 2.39f, 3.66f}, // D major
            new float[] {3.66f, 2.29f, 2.88f, 6.35f, 2.23f, 3.48f, 2.33f, 4.38f, 4.09f, 2.52f, 5.19f, 2.39f}, // D# major
            new float[] {2.39f, 3.66f, 2.29f, 2.88f, 6.35f, 2.23f, 3.48f, 2.33f, 4.38f, 4.09f, 2.52f, 5.19f}, // E major
            new float[] {5.19f, 2.39f, 3.66f, 2.29f, 2.88f, 6.35f, 2.23f, 3.48f, 2.33f, 4.38f, 4.09f, 2.52f}, // F major
            new float[] {2.52f, 5.19f, 2.39f, 3.66f, 2.29f, 2.88f, 6.35f, 2.23f, 3.48f, 2.33f, 4.38f, 4.09f}, // F# major
            new float[] {4.09f, 2.52f, 5.19f, 2.39f, 3.66f, 2.29f, 2.88f, 6.35f, 2.23f, 3.48f, 2.33f, 4.38f}, // G major
            new float[] {4.38f, 4.09f, 2.52f, 5.19f, 2.39f, 3.66f, 2.29f, 2.88f, 6.35f, 2.23f, 3.48f, 2.33f}, // G# major
            new float[] {2.33f, 4.38f, 4.09f, 2.52f, 5.19f, 2.39f, 3.66f, 2.29f, 2.88f, 6.35f, 2.23f, 3.48f}, // A major
            new float[] {3.48f, 2.33f, 4.38f, 4.09f, 2.52f, 5.19f, 2.39f, 3.66f, 2.29f, 2.88f, 6.35f, 2.23f}, // A# major
            new float[] {2.23f, 3.48f, 2.33f, 4.38f, 4.09f, 2.52f, 5.19f, 2.39f, 3.66f, 2.29f, 2.88f, 6.35f}  // B major
           };
        float[][] minorProfiles = new float[12][]
        {
            new float[] {6.33f, 2.68f, 3.52f, 5.38f, 2.60f, 3.53f, 2.54f, 4.75f, 3.98f, 2.69f, 3.34f, 3.17f}, // A minor
            new float[] {3.17f, 6.33f, 2.68f, 3.52f, 5.38f, 2.60f, 3.53f, 2.54f, 4.75f, 3.98f, 2.69f, 3.34f}, // A# minor
            new float[] {3.34f, 3.17f, 6.33f, 2.68f, 3.52f, 5.38f, 2.60f, 3.53f, 2.54f, 4.75f, 3.98f, 2.69f}, // B minor
            new float[] {2.69f, 3.34f, 3.17f, 6.33f, 2.68f, 3.52f, 5.38f, 2.60f, 3.53f, 2.54f, 4.75f, 3.98f}, // C minor
            new float[] {3.98f, 2.69f, 3.34f, 3.17f, 6.33f, 2.68f, 3.52f, 5.38f, 2.60f, 3.53f, 2.54f, 4.75f}, // C# minor
            new float[] {4.75f, 3.98f, 2.69f, 3.34f, 3.17f, 6.33f, 2.68f, 3.52f, 5.38f, 2.60f, 3.53f, 2.54f}, // D minor
            new float[] {2.54f, 4.75f, 3.98f, 2.69f, 3.34f, 3.17f, 6.33f, 2.68f, 3.52f, 5.38f, 2.60f, 3.53f}, // D# minor
            new float[] {3.53f, 2.54f, 4.75f, 3.98f, 2.69f, 3.34f, 3.17f, 6.33f, 2.68f, 3.52f, 5.38f, 2.60f}, // E minor
            new float[] {2.60f, 3.53f, 2.54f, 4.75f, 3.98f, 2.69f, 3.34f, 3.17f, 6.33f, 2.68f, 3.52f, 5.38f}, // F minor
            new float[] {5.38f, 2.60f, 3.53f, 2.54f, 4.75f, 3.98f, 2.69f, 3.34f, 3.17f, 6.33f, 2.68f, 3.52f}, // F# minor
            new float[] {3.52f, 5.38f, 2.60f, 3.53f, 2.54f, 4.75f, 3.98f, 2.69f, 3.34f, 3.17f, 6.33f, 2.68f}, // G minor
            new float[] {2.68f, 3.52f, 5.38f, 2.60f, 3.53f, 2.54f, 4.75f, 3.98f, 2.69f, 3.34f, 3.17f, 6.33f}  // G# minor
        };

        private void LogFrequenciesAndKeys(Complex[] complexBuffer, int sampleRate, int bufferSize)
        {
            // Calculate magnitudes and print prominent frequencies
            for (int i = 0; i < complexBuffer.Length / 2; i++)
            {
                double frequency = i * sampleRate / bufferSize;
                double magnitude = Math.Sqrt(complexBuffer[i].X * complexBuffer[i].X + complexBuffer[i].Y * complexBuffer[i].Y);

                // Print frequencies with significant magnitudes
                if (magnitude > 0.01) // Adjust threshold as needed
                {
                    string note = FrequencyToNoteName(frequency);
                    Console.WriteLine($"Frequency: {frequency} Hz, Magnitude: {magnitude}, Note: {note}");
                }
            }
        }

        public string FrequencyToNoteName(double frequency)
        {
            string[] noteNames = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
            int noteIndex = (int)Math.Round(12 * Log2(frequency / 440.0)) % 12;
            if (noteIndex < 0) noteIndex += 12;
            return noteNames[noteIndex];
        }

        public List<float[]> ExtractChromaFeaturesForAnalysing0(string filePath)
        {
            using (var reader = new AudioFileReader(filePath))
            {
                var sampleProvider = reader.ToSampleProvider();
                int sampleRate = reader.WaveFormat.SampleRate;
                int bufferSize = 4096; // Process in chunks of 4096 samples
                float[] buffer = new float[bufferSize];
                List<float[]> chromaFeaturesList = new List<float[]>();

                int bytesRead;
                while ((bytesRead = sampleProvider.Read(buffer, 0, bufferSize)) > 0)
                {
                    // Apply windowing function
                    for (int i = 0; i < bufferSize; i++)
                    {
                        buffer[i] *= (float)(0.5 * (1 - Math.Cos(2 * Math.PI * i / (bufferSize - 1))));
                    }

                    // Apply FFT
                    Complex[] complexBuffer = new Complex[bufferSize];
                    for (int i = 0; i < bufferSize; i++)
                    {
                        complexBuffer[i].X = buffer[i];
                        complexBuffer[i].Y = 0;
                    }
                    FastFourierTransform.FFT(true, (int)Math.Log(bufferSize, 2.0), complexBuffer);

                    // Calculate chroma features
                    float[] chroma = new float[12];
                    for (int i = 0; i < complexBuffer.Length / 2; i++)
                    {
                        double frequency = i * sampleRate / bufferSize;
                        if (frequency > 0) // Avoid log of zero or negative
                        {
                            int note = (int)Math.Round(12 * Math.Log(frequency / 440.0, 2)) % 12;
                            if (note < 0) note += 12;
                            chroma[note] += (float)Math.Sqrt(complexBuffer[i].X * complexBuffer[i].X + complexBuffer[i].Y * complexBuffer[i].Y);
                        }
                    }

                    // Normalize chroma features
                    float maxChromaValue = chroma.Max();
                    if (maxChromaValue > 0)
                    {
                        for (int i = 0; i < chroma.Length; i++)
                        {
                            chroma[i] /= maxChromaValue;
                        }
                    }


                    chromaFeaturesList.Add(chroma);
                }
                return chromaFeaturesList;
            }
        }
        public float[] AggregateChromaFeaturesOld(List<float[]> chromaFeatures)
        {
            int numFrames = chromaFeatures.Count;
            int numPitchClasses = chromaFeatures[0].Length;
            float[] aggregatedChroma = new float[numPitchClasses];

            foreach (var frame in chromaFeatures)
            {
                for (int i = 0; i < numPitchClasses; i++)
                {
                    aggregatedChroma[i] += frame[i];
                }
            }

            for (int i = 0; i < numPitchClasses; i++)
            {
                aggregatedChroma[i] /= numFrames;
            }

            return aggregatedChroma;
        }
        private float CalculateCorrelation(float[] chroma, float[] profile)
        {
            float sum = 0;
            for (int i = 0; i < chroma.Length; i++)
            {
                sum += chroma[i] * profile[i];
            }
            return sum;
        }
        public string DetectKeyOld(float[] aggregatedChroma)
        {
            float maxCorrelation = float.MinValue;
            string detectedKey = "";

            for (int i = 0; i < 12; i++)
            {
                float correlation = CalculateCorrelation(aggregatedChroma, majorProfiles[i]);
                if (correlation > maxCorrelation)
                {
                    maxCorrelation = correlation;
                    detectedKey = $"{i + 1}B"; // Camelot code for major keys
                }
            }

            for (int i = 0; i < 12; i++)
            {
                float correlation = CalculateCorrelation(aggregatedChroma, minorProfiles[i]);
                if (correlation > maxCorrelation)
                {
                    maxCorrelation = correlation;
                    detectedKey = $"{i + 1}A"; // Camelot code for minor keys
                }
            }

            return detectedKey;
        }
        /*
         public float[] AggregateAndSmoothChromaFeatures(List<float[]> chromaFeatures, int smoothingWindowSize = 5)
        {
            int numFrames = chromaFeatures.Count;
            int numPitchClasses = chromaFeatures[0].Length;
            float[] aggregatedChroma = new float[numPitchClasses];

            // Aggregate chroma features
            foreach (var frame in chromaFeatures)
            {
                for (int i = 0; i < numPitchClasses; i++)
                {
                    aggregatedChroma[i] += frame[i];
                }
            }

            for (int i = 0; i < numPitchClasses; i++)
            {
                aggregatedChroma[i] /= numFrames;
            }

            // Apply smoothing
            float[] smoothedChroma = new float[numPitchClasses];
            for (int i = 0; i < numPitchClasses; i++)
            {
                float sum = 0;
                int count = 0;
                for (int j = -smoothingWindowSize; j <= smoothingWindowSize; j++)
                {
                    int index = (i + j + numPitchClasses) % numPitchClasses;
                    sum += aggregatedChroma[index];
                    count++;
                }
                smoothedChroma[i] = sum / count;
            }

            return smoothedChroma;
        }

        private float CalculateWeightedCorrelation(float[] chroma, float[] profile, float[] weights)
        {
            float sum = 0;
            for (int i = 0; i < chroma.Length; i++)
            {
                sum += chroma[i] * profile[i] * weights[i];
            }
            return sum;
        }

          public string DetectKey(float[] aggregatedChroma)
           {
               float[] weights = new float[] { 1.0f, 0.9f, 0.8f, 0.7f, 0.6f, 0.5f, 0.4f, 0.3f, 0.2f, 0.1f, 0.05f, 0.01f }; // Example weights
               float maxCorrelation = float.MinValue;
               string detectedKey = "";

               // Compare with major profiles
               for (int i = 0; i < 12; i++)
               {
                   float correlation = CalculateWeightedCorrelation(aggregatedChroma, majorProfiles[i], weights);
                   if (correlation > maxCorrelation)
                   {
                       maxCorrelation = correlation;
                       detectedKey = $"{i + 1}B"; // Camelot code for major keys
                   }
               }

               // Compare with minor profiles
               for (int i = 0; i < 12; i++)
               {
                   float correlation = CalculateWeightedCorrelation(aggregatedChroma, minorProfiles[i], weights);
                   if (correlation > maxCorrelation)
                   {
                       maxCorrelation = correlation;
                       detectedKey = $"{i + 1}A"; // Camelot code for minor keys
                   }
               }

               return detectedKey;
           }*/




        private double FindReasonableMaxValue(List<float[]> chromaFeatures, double percentile = 0.99)
        {
            List<float> allValues = new List<float>();
            foreach (var chroma in chromaFeatures)
            {
                allValues.AddRange(chroma);
            }
            allValues.Sort();
            int index = (int)(allValues.Count * percentile);
            return allValues[index];
        }
        private void PlotChromaFeatures(List<float[]> chromaFeatures, double songLengthInSeconds)
        {
            var plotView = new PlotView
            {
                Dock = DockStyle.Fill
            };

            var plotModel = new PlotModel { Title = "Chroma Features Over Time" };

            // Find a reasonable maximum value in the chroma features
            double maxValue = FindReasonableMaxValue(chromaFeatures);

            // Add a color axis
            var colorAxis = new LinearColorAxis
            {
                Position = AxisPosition.Right,
                Palette = OxyPalettes.Jet(500),
                HighColor = OxyColors.Gray,
                LowColor = OxyColors.Black,
                Minimum = 0,
                Maximum = maxValue // Set the maximum based on a reasonable value
            };
            plotModel.Axes.Add(colorAxis);

            // Add X axis (Time)
            var xAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Time (seconds)",
                Minimum = 0,
                Maximum = songLengthInSeconds, // Set the maximum to the length of the song in seconds
                MajorStep = 15, // Major ticks every 15 seconds
                MinorStep = 5, // Minor ticks every 5 seconds
                LabelFormatter = value => TimeSpan.FromSeconds(value).ToString(@"mm\:ss") // Correct format for minutes:seconds
            };
            plotModel.Axes.Add(xAxis);

            // Add Y axis (Chroma Features)
            var yAxis = new CategoryAxis
            {
                Position = AxisPosition.Left,
                Title = "Chroma Features",
                Key = "ChromaAxis",
                ItemsSource = new[]
            {
"C (A01)", "C# (A02)", "D (A03)", "D# (A04)", "E (A05)", "F (A06)", "F# (A07)", "G (A08)", "G# (A09)", "A (A10)", "A# (A11)", "B (A12)"
}
            };
            plotModel.Axes.Add(yAxis);

            var heatMapSeries = new HeatMapSeries
            {
                X0 = 0,
                X1 = chromaFeatures.Count, // Set X1 to the number of chroma feature frames
                Y0 = 0,
                Y1 = 12,
                Interpolate = false,
                RenderMethod = HeatMapRenderMethod.Rectangles
            };

            double[,] data = new double[chromaFeatures.Count, 12];
            for (int x = 0; x < chromaFeatures.Count; x++)
            {
                for (int y = 0; y < 12; y++)
                {
                    data[x, y] = chromaFeatures[x][y];
                }
            }
            heatMapSeries.Data = data;
            plotModel.Series.Add(heatMapSeries);
            plotView.Model = plotModel;

            Form plotForm = new Form();
            plotForm.Controls.Add(plotView);
            plotForm.Text = "Chroma Features Visualization";
            plotForm.Width = 1200; // Adjust the width to make the plot less flat
            plotForm.Height = 600;
            plotForm.StartPosition = FormStartPosition.CenterScreen;
            plotForm.ShowDialog();
        }


        /*
        private void AnalyseTrack(Track track)
        {
            double songLengthInSeconds = track.Length;

            using (var reader = new AudioFileReader(track.Path))
            {
                if (currentFeatureType == FeatureType.Chroma)
                {
                    List<float[]> chromaFeatures = ExtractChromaFeaturesForAnalysing(track.Path);
                    PlotChromaFeatures(chromaFeatures, songLengthInSeconds);
                }                
            }
        }
        public List<float[]> ExtractChromaFeaturesForAnalysing(string filePath)
        {
            using (var reader = new AudioFileReader(filePath))
            {
                var sampleProvider = reader.ToSampleProvider();
                int sampleRate = reader.WaveFormat.SampleRate;
                int bufferSize = 4096; // Process in chunks of 4096 samples
                float[] buffer = new float[bufferSize];
                List<float[]> chromaFeaturesList = new List<float[]>();

                int bytesRead;
                while ((bytesRead = sampleProvider.Read(buffer, 0, bufferSize)) > 0)
                {
                    // Apply windowing function
                    for (int i = 0; i < bufferSize; i++)
                    {
                        buffer[i] *= (float)(0.5 * (1 - Math.Cos(2 * Math.PI * i / (bufferSize - 1))));
                    }

                    // Apply FFT
                    Complex[] complexBuffer = new Complex[bufferSize];
                    for (int i = 0; i < bufferSize; i++)
                    {
                        complexBuffer[i].X = buffer[i];
                        complexBuffer[i].Y = 0;
                    }
                    FastFourierTransform.FFT(true, (int)Math.Log(bufferSize, 2.0), complexBuffer);

                    // Calculate chroma features
                    float[] chroma = new float[12];
                    for (int i = 0; i < complexBuffer.Length / 2; i++)
                    {
                        double frequency = i * sampleRate / bufferSize;
                        if (frequency > 0) // Avoid log of zero or negative
                        {
                            int note = (int)Math.Round(12 * Math.Log(frequency / 440.0, 2)) % 12;
                            if (note < 0) note += 12;
                            chroma[note] += (float)Math.Sqrt(complexBuffer[i].X * complexBuffer[i].X + complexBuffer[i].Y * complexBuffer[i].Y);
                        }
                    }

                    // Normalize chroma features
                    float maxChromaValue = 0;
                    foreach (var value in chroma)
                    {
                        if (value > maxChromaValue)
                        {
                            maxChromaValue = value;
                        }
                    }
                    if (maxChromaValue > 0)
                    {
                        for (int i = 0; i < chroma.Length; i++)
                        {
                            chroma[i] /= maxChromaValue;
                        }
                    }

                    chromaFeaturesList.Add(chroma);
                }
                return chromaFeaturesList;
            }
        }
        private double FindReasonableMaxValue(List<float[]> chromaFeatures, double percentile = 0.99)
        {
            List<float> allValues = new List<float>();
            foreach (var chroma in chromaFeatures)
            {
                allValues.AddRange(chroma);
            }
            allValues.Sort();
            int index = (int)(allValues.Count * percentile);
            return allValues[index];
        }
        private void PlotChromaFeatures(List<float[]> chromaFeatures, double songLengthInSeconds)
        {
            var plotView = new PlotView
            {
                Dock = DockStyle.Fill
            };

            var plotModel = new PlotModel { Title = "Chroma Features Over Time" };

            // Find a reasonable maximum value in the chroma features
            double maxValue = FindReasonableMaxValue(chromaFeatures);

            // Add a color axis
            var colorAxis = new LinearColorAxis
            {
                Position = AxisPosition.Right,
                Palette = OxyPalettes.Jet(500),
                HighColor = OxyColors.Gray,
                LowColor = OxyColors.Black,
                Minimum = 0,
                Maximum = maxValue // Set the maximum based on a reasonable value
            };
            plotModel.Axes.Add(colorAxis);

            // Add X axis (Time)
            var xAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Time (seconds)",
                Minimum = 0,
                Maximum = songLengthInSeconds, // Set the maximum to the length of the song in seconds
                MajorStep = 60, // Major ticks every minute
                MinorStep = 10, // Minor ticks every 10 seconds
                StringFormat = "mm:ss" // Correct format for minutes:seconds
            };
            plotModel.Axes.Add(xAxis);

            // Add Y axis (Chroma Features)
            var yAxis = new CategoryAxis
            {
                Position = AxisPosition.Left,
                Title = "Chroma Features",
                Key = "ChromaAxis",
                ItemsSource = new[]
            {
"C (A01)", "C# (A02)", "D (A03)", "D# (A04)", "E (A05)", "F (A06)", "F# (A07)", "G (A08)", "G# (A09)", "A (A10)", "A# (A11)", "B (A12)"
}
            };
            plotModel.Axes.Add(yAxis);

            var heatMapSeries = new HeatMapSeries
            {
                X0 = 0,
                X1 = chromaFeatures.Count, // Set X1 to the number of chroma feature frames
                Y0 = 0,
                Y1 = 12,
                Interpolate = false,
                RenderMethod = HeatMapRenderMethod.Rectangles
            };

            double[,] data = new double[12, chromaFeatures.Count];
            for (int x = 0; x < chromaFeatures.Count; x++)
            {
                for (int y = 0; y < 12; y++)
                {
                    data[y, x] = chromaFeatures[x][y];
                }
            }
            heatMapSeries.Data = data;
            plotModel.Series.Add(heatMapSeries);
            plotView.Model = plotModel;

            Form plotForm = new Form();
            plotForm.Controls.Add(plotView);
            plotForm.Text = "Chroma Features Visualization";
            plotForm.Width = 800;
            plotForm.Height = 600;
            plotForm.ShowDialog();
        }

        */


        private Dictionary<String, Dictionary<String, Color>> tagValueDictionary { get; set; }
        private List<Tag> tagList { get;set; }
        private void InitializeTagsAndTagValues()
        {
            this.tagValueDictionary = new Dictionary<String, Dictionary<String, Color>>();

            List<Tag> tagList = this.tagDao.GetAllTag().Value;
            if (tagList != null && tagList.Count > 0)
            {
                this.tagList = tagList;

                List<TagValue> tagValueList = new List<TagValue>();

                foreach (Tag tag in this.tagList)
                {
                    Dictionary<String, Color> tvDic = new Dictionary<String, Color>();

                    tagValueList = this.tagDao.GetTagValuesByTagId(tag.Id).Value;
                    if (tagValueList != null && tagValueList.Count > 0)
                    {
                        foreach (TagValue tv in tagValueList)
                        {
                            tvDic.Add(tv.Name, tv.Color);
                        }
                        this.tagValueDictionary.Add(tag.Name, tvDic);
                    }
                }
            }
            this.view.InitializeTagsAndTagValues(this.tagList, this.tagValueDictionary);
        }
        private void SetIsTracklistDetailsDisplayedEvent(object sender, Messenger e)
        {
            this.IsTracklistDetailsDisplayed = e.BooleanField1;
            this.settingDao.SetBooleanSetting(Settings.IsTracklistDetailsDisplayed.ToString(), this.IsTracklistDetailsDisplayed);

            if(this.IsTracklistDetailsDisplayed && this.CurrentPlaylist != null && this.CurrentTag != null)
            {
                this.CalculateDataSetQuality();
            }
        }
        private void LoadTrainingDataEvent(object sender, Messenger e)
        {
            this.CurrentTrainingData = this.trackDao.GetTrainingData(e.IntegerField1);
        }
        private void SelectTag(object sender, Messenger e)
        {
            this.CurrentTag = this.tagDao.GetTagByName(e.StringField1).Value;
            this.CurrentTrackProperty = this.settingDao.GetTrackPropertyByNameAndGroup(this.CurrentTag.Name, ColumnGroup.TracklistColumns.ToString()).Value;
        
            if(this.CurrentPlaylist != null)
            {
                this.InitializeInputDataTableStructure();
                this.InitializeInputDataTableContent();

                this.InitializeOutputDataTableStructure();

                if(this.IsTracklistDetailsDisplayed)
                    this.CalculateDataSetQuality();
            }
        }
        private void SelectPlaylist(object sender, Messenger e)
        {
            this.CurrentPlaylist =this.trackDao.GetPlaylistByName(e.StringField1).Value;

            if (this.CurrentPlaylist != null)
            {
                this.trackList = this.trackDao.GetTracklistWithTagsByPlaylistId(this.CurrentPlaylist.Id, this.TagList).Value;
            }

            if (this.CurrentTag != null)
            {
                this.InitializeInputDataTableStructure();
                this.InitializeInputDataTableContent();

                this.InitializeOutputDataTableStructure();

                if (this.IsTracklistDetailsDisplayed)
                    this.CalculateDataSetQuality();
            }
        }
        private void SelectTemplate(object sender, Messenger e)
        {
            this.CurrentTemplate =this.trackDao.GetTrainingData(e.IntegerField1);

            if (this.CurrentTemplate != null)
            {
                this.view.InitializeFeatureSettings(
                    this.CurrentTemplate.ExtractChromaFeatures,
                    this.CurrentTemplate.HarmonicPercussiveSeparation,
                    this.CurrentTemplate.ExtractMFCCs,
                    this.CurrentTemplate.ExtractHPCP,
                    this.CurrentTemplate.ExtractSpectralContrast,
                    this.CurrentTemplate.ExtractSpectralCentroid,
                    this.CurrentTemplate.ExtractSpectralBandwidth,
                    this.CurrentTemplate.ExtractTonnetzFeatures,
                    this.CurrentTemplate.ExtractZeroCrossingRate,
                    this.CurrentTemplate.ExtractPitch,
                    this.CurrentTemplate.ExtractRmsEnergy
                );
            }
        }
        private void InitializeTrainingDataListStructure()
        {
            this.traningDataListBindingSource = new BindingSource();
            this.trainingDataTable = new DataTable();
            this.trainingDataTable.Columns.Add("Id", typeof(int));
            this.trainingDataTable.Columns.Add("Date", typeof(string));
            this.trainingDataTable.Columns.Add("Name", typeof(string));
            this.trainingDataTable.Columns.Add("TagName", typeof(string));
            this.trainingDataTable.Columns.Add("SampleCount", typeof(string));
            this.trainingDataTable.Columns.Add("Balance(%)", typeof(string));
            this.trainingDataTable.Columns.Add("Size", typeof(string));
        }
        private void InitializeInputDataTableStructure()
        {
            this.inputTrackListBindingSource = new BindingSource();
            this.inputTrackTable = new DataTable();
            this.inputTrackTable.Columns.Add("Artist", typeof(string));
            this.inputTrackTable.Columns.Add("Title", typeof(string));
            this.inputTrackTable.Columns.Add(this.CurrentTrackProperty.Name, Type.GetType(this.CurrentTrackProperty.Type));
        }
        private void InitializeOutputDataTableStructure()
        {
            this.inputTrackListBindingSource = new BindingSource();
            this.inputTrackTable = new DataTable();
            this.inputTrackTable.Columns.Add("Artist", typeof(string));
            this.inputTrackTable.Columns.Add("Title", typeof(string));
            this.inputTrackTable.Columns.Add(this.CurrentTrackProperty.Name, Type.GetType(this.CurrentTrackProperty.Type));
        }
        private void InitializeInputDataTableContent()
        {
            String artist = String.Empty;
            String title = String.Empty;
            String tag = String.Empty;

            this.inputTrackTable.Clear();

            List<Tag> tagList = this.tagDao.GetAllTag().Value;
            if(tagList != null && tagList.Count > 0)
            {
                this.trackList = this.trackDao.GetTracklistWithTagsByPlaylistId(this.CurrentPlaylist.Id, tagList).Value;

                if (this.trackList != null && this.trackList.Count > 0)
                {
                    for (int i = 0; i <= this.trackList.Count - 1; i++)
                    {
                        artist = this.trackList[i].Artist;
                        title = this.trackList[i].Title;

                        TrackTagValue ttv = this.trackList[i].TrackTagValues.Find(x => x.TagId == this.CurrentTag.Id);
                        if(ttv != null)
                        {
                            if (ttv.HasMultipleValues)
                            {
                                tag = ttv.Value;
                            }
                            else
                            {
                                tag = ttv.TagValueName;
                            }
                        }
                        else
                        {
                            tag = String.Empty;
                        }
                        this.inputTrackTable.Rows.Add(artist, title, tag);
                    }
                }

                this.inputTrackListBindingSource.DataSource = this.inputTrackTable;
                this.view.SetInputTrackListBindingSource(this.inputTrackListBindingSource);
            }
        }
        private Decimal balance { get; set; }
        private const int requiredMinimalSampleCount = 100;
        private void CalculateDataSetQualityEvent(object sender, EventArgs e)
        {
            this.CalculateDataSetQuality();
        }
        private void CalculateDataSetQuality()
        {
            String result = String.Empty;
            Dictionary<String, int> tagValueNumberDic = new Dictionary<String, int>();

            List<TagValue> tvList = this.tagDao.GetTagValuesByTagId(this.CurrentTag.Id).Value;
            if(tvList != null  && tvList.Count > 0)
            {
                for (int i = 0; i <= tvList.Count - 1; i++)
                {
                    if (!tagValueNumberDic.ContainsKey(tvList[i].Name))
                    {
                        tagValueNumberDic.Add(tvList[i].Name, 0);
                    }
                }  
            }

            if (this.trackList != null && this.trackList.Count > 0)
            {
                for (int i = 0; i <= this.trackList.Count - 1; i++)
                {
                    TrackTagValue ttv = this.trackList[i].TrackTagValues.Find(x => x.TagId == this.CurrentTag.Id);
                    if (ttv != null && !ttv.HasMultipleValues && !String.IsNullOrEmpty(ttv.TagValueName))
                    {
                        if (!tagValueNumberDic.ContainsKey(ttv.TagValueName))
                        {
                            tagValueNumberDic.Add(ttv.TagValueName, 1);
                        }
                        else
                        {
                            tagValueNumberDic[ttv.TagValueName]++;
                        }
                    }
                }

                tagValueNumberDic = tagValueNumberDic.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

                // Calculate the total number of keys
                int totalTagValues = tagValueNumberDic.Values.Sum();
                result += $"Total number of {this.CurrentTag.Name}: {totalTagValues}\n\n";
                result += $"Required minimal sample count: {requiredMinimalSampleCount} / {this.CurrentTag.Name} \n\n";

                bool sampleCountIsValid = true;

                // Calculate the proportion of each key
                foreach (var kvp in tagValueNumberDic)
                {
                    double proportion = (double)kvp.Value / totalTagValues * 100;
                    String passOrFail = kvp.Value >= requiredMinimalSampleCount ? "PASS" : "FAIL";

                    if (sampleCountIsValid)
                    {
                        if(kvp.Value < requiredMinimalSampleCount)
                        {
                            sampleCountIsValid = false;
                        }
                    }

                    result += $"{this.CurrentTag.Name}: {kvp.Key} - Proportion: {proportion:F2} % - Count: {kvp.Value}/{requiredMinimalSampleCount} - {passOrFail} \n";
                }

                // Check if the dataset is balanced
                double averageProportion = 100.0 / tagValueNumberDic.Count;
                double threshold = 10.0; // Define a threshold for balance, e.g., 10%

                bool isBalanced = tagValueNumberDic.Values.All(count =>
                Math.Abs((double)count / totalTagValues * 100 - averageProportion) <= threshold);

                result += $"\nThe sample count is enough? - {sampleCountIsValid}\n";

                result += $"\nIs the dataset balanced? - {isBalanced}\n\n";

                // Calculate the percentage value of the balance
                double balancePercentage = tagValueNumberDic.Values.Average(count => Math.Abs((double)count / totalTagValues * 100 - averageProportion));
                double normalizedBalancePercentage = 100 - balancePercentage; // Normalize to 0-100% scale

                this.balance = (Decimal)normalizedBalancePercentage;

                result += $"Balance percentage: {balancePercentage:F2}%\n";
                result += $"Normalized balance percentage: {normalizedBalancePercentage:F2}%\n";

                // Add explanation
                result += "\nA smaller balance percentage indicates that the distribution of keys is closer to the average proportion, suggesting a more balanced dataset.";

                // Display the result in a dialog window
                
                MessageBox.Show(result, this.CurrentTag.Name + " Balance Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void InitializeTrainingDataListContent()
        {
            int id = 0;
            String createDate = String.Empty;
            String name = String.Empty;
            int sampleCount = 0;
            Decimal balance = 0;
            String tagName = String.Empty;
            String size = String.Empty;

            if(this.trainingDataTable != null && this.trainingDataTable.Rows.Count > 0)
                this.trainingDataTable.Clear();

            List<TrainingData> trainingDataList = this.trackDao.GetAllTrainingData().FindAll(x => !x.IsTemplate);

            if (trainingDataList != null && trainingDataList.Count > 0)
            {
                for (int i = 0; i <= trainingDataList.Count - 1; i++)
                {
                    id = trainingDataList[i].Id;
                    createDate = trainingDataList[i].CreateDate.ToShortDateString() + " " + trainingDataList[i].CreateDate.ToShortTimeString();
                    name = trainingDataList[i].Name;
                    sampleCount = trainingDataList[i].SampleCount;
                    balance = trainingDataList[i].Balance;

                    Tag tag = this.tagDao.GetTag(trainingDataList[i].TagId).Value;
                    if(tag!= null)
                    {
                        tagName = tag.Name;
                    }

                    if (System.IO.File.Exists(trainingDataList[i].FilePath))
                    {
                        FileInfo fileInfo = new FileInfo(trainingDataList[i].FilePath);

                        this.trainingDataTable.Rows.Add(id, createDate, name, tagName, sampleCount, balance, (fileInfo.Length/1048576.0).ToString("N0") + " MB");
                    }
                    else
                    {
                        this.trackDao.DeleteTrainingData(trainingDataList[i].Id);
                    }
                }
            }
            this.traningDataListBindingSource.DataSource = this.trainingDataTable;
            this.view.SetTraningDataListBindingSource(this.traningDataListBindingSource);

        }
        private void IsChromaFeaturesEnabledEvent(object sender, Messenger e)
        {
            isChromaEnabled = e.BooleanField1;
        }
        private void IsMFCCsEnabledEvent(object sender, Messenger e)
        {
            isMFCCsEnabled = e.BooleanField1;
        }
        private void IsSpectralContrastEnabledEvent(object sender, Messenger e)
        {
            isSpectralContrastEnabled = e.BooleanField1;
        }
        private void IsHPCPEnabledEvent(object sender, Messenger e)
        {
            isHPCPEnabled = e.BooleanField1;
        }
        private void IsHPSEnabledEvent(object sender, Messenger e)
        {
            isHarmonicPercussiveSourceSeparationFeaturesEnabled = e.BooleanField1;
        }
        private void IsSpectralCentroidEnabledEvent(object sender, Messenger e)
        {
            isSpectralCentroidEnabled = e.BooleanField1;
        }
        private void IsTonnetzFeaturesEnabledEvent(object sender, Messenger e)
        {
            isTonnetzFeaturesEnabled = e.BooleanField1;
        }
        private void IsPitchEnabledEvent(object sender, Messenger e)
        {
            isPitchEnabled = e.BooleanField1;
        }
        private void IsRMSEnabledEvent(object sender, Messenger e)
        {
            isRmsEnergyEnabled = e.BooleanField1;
        }
        private void IsZCREnabledEvent(object sender, Messenger e)
        {
            isZeroCrossingRateEnabled = e.BooleanField1;
        }
        private void IsSpectralBandwidthEnabledEvent(object sender, Messenger e)
        {
            isSpectralBandwidthEnabled = e.BooleanField1;
        }
        private void BatchProcessChangedEvent(object sender, Messenger e)
        {
            this.BatchSize = e.IntegerField1;
        }
        private String hdf5FilePath { get; set; }
        private bool isGenerating { get; set; }
        private CancellationTokenSource cancellationTokenSourceForGenerating { get; set; }
        private void GenerateTrainingDataEvent(object sender, EventArgs e)
        {
            if (isGenerating)
            {
                MessageBox.Show("Generating is in progress, please wait!", "Training Model Validation", MessageBoxButtons.OK);
            }
            else
            {
                if (this.CurrentTag == null || this.CurrentPlaylist == null)
                {
                    MessageBox.Show("Tag and Playlist must be set!", "Training Model Validation", MessageBoxButtons.OK);
                }
                else if (this.trackList == null || this.trackList.Count == 0)
                {
                    MessageBox.Show("The model training tracklist is empty!", "Training Model Validation", MessageBoxButtons.OK);
                }
                else if (
                    !isChromaEnabled &&
                    !isHarmonicPercussiveSourceSeparationFeaturesEnabled &&
                    !isHPCPEnabled &&
                    !isMFCCsEnabled &&
                    !isTonnetzFeaturesEnabled &&
                    !isPitchEnabled &&
                    !isRmsEnergyEnabled &&
                    !isSpectralBandwidthEnabled &&
                    !isSpectralCentroidEnabled &&
                    !isSpectralContrastEnabled &&
                    !isZeroCrossingRateEnabled)
                {
                    MessageBox.Show("At least one feature must be set!", "Training Model Validation", MessageBoxButtons.OK);
                }
                else
                {
                    
                    hdf5FilePath = this.SaveTrainingDataSet();
                    if (!String.IsNullOrEmpty(hdf5FilePath))
                    {
                        isGenerating = true;
                        this.view.ChangeGeneratingStatus(true);
                        cancellationTokenSourceForGenerating = new CancellationTokenSource();
                        this.GenerateTrainingDataEvent(cancellationTokenSourceForGenerating.Token);
                    }
                }
            }
        }
        private String SaveTrainingDataSet()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "HDF5 files (*.hdf5)|*.hdf5";
            sfd.RestoreDirectory = true;
            sfd.FileName = "TrainingDataSet_" + DateTime.Now.ToString("yyyyMMddHHmm") + "_" + this.CurrentPlaylist.Name + "_" + this.CurrentTag.Name;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                return sfd.FileName;
            }
            else
            {
                return "";
            }
        }
        private async void GenerateTrainingDataEvent(CancellationToken cancellationToken)
        {
            try
            {
                await GenerateTrainingDataAsync(cancellationToken);
            }
            finally
            {
                isGenerating = false;
                this.view.ChangeGeneratingStatus(false);
            }
        }
        private async Task GenerateTrainingDataAsync(CancellationToken cancellationToken)
        {
            var trainingDataCreator = new TrainingDataCreator(sampleRate: 44100, intervalSeconds: 1, windowSize: 250, stepSize: 125);
            var cancellationTokenSource = new CancellationTokenSource();

            // Example usage
            //trainingDataCreator.AddTrack("path/to/song1.wav", "genreA", cancellationTokenSource.Token);
            //trainingDataCreator.AddTrack("path/to/song2.wav", "genreB", cancellationTokenSource.Token);

            // Create the training dataset
            

            await Task.Run(() =>
            {
                try
                {
                    int batchSize = this.BatchSize; // Adjust the batch size as needed
                    int totalTracks = this.trackList.Count;
                    int processedTracks = 0;

                    if (!String.IsNullOrEmpty(hdf5FilePath))
                    {
                        DateTime startTime = DateTime.Now;
                        DateTime lastUpdateTime = DateTime.Now;

                        this.ConcBag.Add(new MessageTest()
                        {
                            LogState = LogState.ParallelProcess,
                            Sum = processedTracks,
                            Total = totalTracks,
                        });

                        var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };

                        for (int i = 0; i < totalTracks; i += batchSize)
                        {
                            var batch = this.trackList.Skip(i).Take(batchSize).ToList();

                            ConcurrentBag<Track> concurrentTracks = new ConcurrentBag<Track>(batch);

                            //Parallel.ForEach(concurrentTracks, parallelOptions, track =>
                            foreach (var track in concurrentTracks)
                            {
                                if (cancellationToken.IsCancellationRequested)
                                {
                                    return;
                                }

                                if (track.TrackTagValues.Exists(x => x.TagName == this.CurrentTag.Name))
                                {
                                    String trainingTagValue = track.TrackTagValues.Find(x => x.TagName == this.CurrentTag.Name).TagValueName;
                                    if (!String.IsNullOrEmpty(trainingTagValue))
                                    {
                                        this.AddTrack(track.Path, trainingTagValue, cancellationToken);
                                        Interlocked.Increment(ref processedTracks);

                                        Console.WriteLine($"Add to model: {processedTracks}/{totalTracks} ready");

                                        TimeSpan elapsedTime = DateTime.Now - lastUpdateTime;
                                        lastUpdateTime = DateTime.Now;

                                        TimeSpan remaining = TimeSpan.Zero;
                                        if (processedTracks > 0)
                                        {
                                            double averageTimePerTrack = (DateTime.Now - startTime).TotalSeconds / processedTracks;
                                            double remainingTimeInSeconds = averageTimePerTrack * (totalTracks - processedTracks);
                                            remaining = TimeSpan.FromSeconds(remainingTimeInSeconds);
                                        }

                                        this.ConcBag.Add(new MessageTest()
                                        {
                                            LogState = LogState.ParallelProcess,
                                            Sum = processedTracks,
                                            Total = totalTracks,
                                            RemainingTime = remaining
                                        });
                                    }
                                }
                            }
                           // });

                            if (cancellationToken.IsCancellationRequested)
                            {
                                break;
                            }

                            try
                            {
                               // this.WriteToHDF5(hdf5FilePath);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            }

                            tracks.Clear();
                        }
                    }

                    /*  TrainingData trainingData = new TrainingData()
                      {
                          Id = this.trackDao.GetNextId(TableName.TrainingData.ToString()),
                          FilePath = this.hdf5FilePath,
                          TagId = this.CurrentTag.Id,
                          Name = this.CurrentPlaylist?.Name,
                          CreateDate = DateTime.Now,
                          SampleCount = this.trackList.Count,
                          Balance = this.balance
                      };

                      this.trackDao.CreateTrainingData(trainingData);*/

                    //trainingDataCreator.CreateTrainingDataset(sampleRate: 44100, intervalSeconds: 1, windowSize: 250, stepSize: 125);


                    this.ConcBag.Add(new MessageTest()
                    {
                        LogState = LogState.Finish,
                        Sum = processedTracks,
                        Total = totalTracks,
                        RemainingTime = new TimeSpan(),
                        EstimatedSize = 0,
                        Log = "COMPLETED SUCCESSFULLY!\n"
                    });

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            });

            this.InitializeTrainingDataListContent();
        }
        public ObservableConcurrentBag<MessageTest> ConcBag { get; set; }
        public class MessageTest
        {
            public int Sum { get; set; }
            public int Total { get; set; }
            public TimeSpan RemainingTime { get; set; }
            public double EstimatedSize { get; set; }
            public String Log { get; set; }
            public int ExtractionProgressValue { get; set; }
            public int CurrentSample { get; set; }
            public int TotalSamples { get; set; }
            public String FilePath { get; set; }
            public LogState LogState { get; set; }

            public MessageTest(int sum, int total, TimeSpan remainingTime)
            {
                this.Sum = sum;
                this.Total = total;
                this.RemainingTime = remainingTime;
            }

            public MessageTest()
            {
            }
            public MessageTest(double estimatedSize)
            {
                this.EstimatedSize = estimatedSize;
            }
        }
        private void ConcBag_ItemAdded(object sender, ItemAddedEventArgs<MessageTest> e)
        {
            //Console.WriteLine($"Item added: {e.Item}");
            this.view.UpdateProgressOnView(e.Item);

            if (this.ConcBag.Count > 100)
            {
                while (!this.ConcBag.IsEmpty)
                {
                    this.ConcBag.TryTake(out _);
                }
            }
        }
        public void CancelGenerationEvent(object sender, EventArgs e)
        {
            if (cancellationTokenSourceForGenerating != null)
            {
                this.ConcBag.Add(new MessageTest()
                {
                    LogState = LogState.Canceled,
                    RemainingTime = new TimeSpan(),
                    Sum = 0,
                    Total = 0,
                    EstimatedSize = 0,
                    Log = "GENERATING CANCELED!\n"
                }); 

                cancellationTokenSourceForGenerating.Cancel();
                isGenerating = false;
                this.view.ChangeGeneratingStatus(false);

                if (System.IO.File.Exists(hdf5FilePath))
                {
                    System.IO.File.Delete(hdf5FilePath);
                }
            }
        }

        List<TrackForTraining> tracks = new List<TrackForTraining>();
        public class TrackForTraining
        {
            public String Path { get; set; }
            public String Key { get; set; }
            public float[] Features { get; set; }
        }


        

        //public void AddTrack(string filePath, string tagValue, CancellationToken cancellationToken)
        //{
        //    //TODO VISSZARAKNI? EZ MOST CSAK TESZT
        //    // var features = ExtractFeatures(filePath, cancellationToken);
        //    // tracks.Add(new TrackForTraining { Path = filePath, Key = tagValue, Features = features });

        //    //this.DetectSongKey(filePath);

        //    // Step 1: Extract Chroma Features
        //    var chromaExtractor = new ChromaFeatureExtractor();
        //    float[][] chromaFeatures = chromaExtractor.ExtractChromaFeatures(filePath);

        //    // Step 2: Detect Key Using KeyDetector
        //    var keyDetector = new KeyDetector();
        //    foreach (var frame in chromaFeatures)
        //    {
        //        string detectedKey = keyDetector.DetectKey(frame);
        //        Console.WriteLine($"Detected Key: {detectedKey}");
        //    }

        //    // Step 3: Train and Predict Using HMMKeyDetector
        //    var hmmKeyDetector = new HMMKeyDetector();

        //    // Example training data (you need to provide actual data)
        //    float[][] trainingData = chromaFeatures; // Use extracted chroma features as training data

        //    hmmKeyDetector.Train(trainingData);

        //    // Predict the key of the chroma feature sequence
        //    string predictedKey = hmmKeyDetector.Predict(chromaFeatures);
        //    Console.WriteLine($"Predicted Key: {predictedKey}");

        //}




        private const int intervalSeconds = 2;
        /* private float[] ExtractFeatures(string filePath, CancellationToken cancellationToken)
         {
             using (var reader = new AudioFileReader(filePath))
             {
                 var sampleProvider = reader.ToSampleProvider();
                 float[] buffer = new float[reader.WaveFormat.SampleRate * intervalSeconds];
                 int samplesRead;
                 List<float[]> featuresList = new List<float[]>();
                 int totalSamples = (int)Math.Ceiling((double)reader.Length / (buffer.Length * 2 * reader.WaveFormat.Channels)); // Adjust for bytes per sample and channels
                 int currentSample = 0;

                 if (enableExtractFeaturesLog)
                     Console.WriteLine($"Total samples: {totalSamples}");

                 while ((samplesRead = sampleProvider.Read(buffer, 0, buffer.Length)) > 0)
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
                     ExtractAndAggregateFeatures(featuresList, fftBuffer, buffer, reader.WaveFormat.SampleRate);

                     currentSample++;
                     int progressValue = currentSample * 100 / totalSamples;

                     if (enableExtractFeaturesLog)
                         Console.WriteLine($"{progressValue}% complete (Sample {currentSample} of {totalSamples}) - File: {filePath}");

                     this.ConcBag.Add(new MessageTest()
                     {
                         LogState = LogState.Extraction,
                         ExtractionProgressValue = progressValue,
                         CurrentSample = currentSample,
                         TotalSamples = totalSamples,
                         FilePath = filePath
                     });
                 }

                 var allFeatures = featuresList.SelectMany(f => f).ToArray();
                 int originalFeatureCount = featuresList[0].Length; // Assuming all feature arrays have the same length
                 var reducedFeatures = ApplyPCA(featuresList, 100); // Adjust targetDimension as needed

                 return reducedFeatures;


             }
         }*/



        private List<float[]> ExtractFeatures(string filePath, int sampleRate, int intervalSeconds)
        {
            List<float[]> featureList = new List<float[]>();

            using (var reader = new AudioFileReader(filePath))
            {
                var sampleProvider = reader.ToSampleProvider();
                float[] buffer = new float[sampleRate * intervalSeconds];
                int samplesRead;

                while ((samplesRead = sampleProvider.Read(buffer, 0, buffer.Length)) > 0)
                {
                    // Perform FFT
                    Complex[] fftBuffer = new Complex[buffer.Length];
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        fftBuffer[i].X = buffer[i];
                        fftBuffer[i].Y = 0;
                    }
                    FastFourierTransform.FFT(true, (int)Math.Log(buffer.Length, 2.0), fftBuffer);

                    // Extract features
                    float[] features = ExtractFeaturesFromBuffer(fftBuffer, buffer, sampleRate);
                    featureList.Add(features);
                }
            }

            return featureList;
        }
        private float[] ExtractFeaturesFromBuffer(Complex[] fftBuffer, float[] buffer, int sampleRate)
        {
            List<float> features = new List<float>();

            // Extract Chroma features
            if (isChromaEnabled)
            {
                var chromaFeatures = ExtractChromaFeatures(fftBuffer);
                features.AddRange(chromaFeatures);
            }
            // Extract MFCCs
            if (isMFCCsEnabled)
            {
                var mfccFeatures = ExtractMFCCsFeatures(buffer, sampleRate);
                features.AddRange(mfccFeatures);
            }

            // Extract Spectral Contrast
            if (isSpectralContrastEnabled)
            {
                var spectralContrastFeatures = ExtractSpectralContrastFeatures(fftBuffer);
                features.AddRange(spectralContrastFeatures);
            }

            // Extract Spectral Centroid
            if (isSpectralCentroidEnabled)
            {
                var spectralCentroid = ExtractSpectralCentroidFeatures(fftBuffer);
                features.Add(spectralCentroid);
            }

            // Extract Spectral Bandwidth
            if (isSpectralBandwidthEnabled)
            {
                var spectralBandwidth = ExtractSpectralBandwidthFeatures(buffer);
                features.Add(spectralBandwidth);
            }

            // Extract Zero Crossing Rate
            if (isZeroCrossingRateEnabled)
            {
                var zcr = ExtractZeroCrossingRateFeatures(buffer);
                features.Add(zcr);
            }

            // Extract RMS Energy
            if (isRmsEnergyEnabled)
            {
                var rmsEnergy = ExtractRmsEnergyFeatures(buffer);
                features.Add(rmsEnergy);
            }

            // Extract Pitch
            if (isPitchEnabled)
            {
                var pitch = ExtractPitchFeatures(buffer, sampleRate);
                features.Add(pitch);
            }

            return features.ToArray();
        }

        private List<float[]> AggregateFeaturesWithSlidingWindow(List<float[]> featureList, int windowSize, int stepSize)
        {
            List<float[]> aggregatedFeatures = new List<float[]>();

            for (int i = 0; i <= featureList.Count - windowSize; i += stepSize)
            {
                List<float> windowFeatures = new List<float>();

                for (int j = 0; j < windowSize; j++)
                {
                    windowFeatures.AddRange(featureList[i + j]);
                }

                float[] aggregatedWindowFeatures = AggregateWindowFeatures(windowFeatures.ToArray());
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




      /*  private float[] SelectRelevantFeatures(float[] features)
        {
            // Example using Lasso for feature selection
            var lasso = new Lasso();
            lasso.Fit(features, target);
            var selectedFeatures = lasso.SelectFeatures(features);

            return selectedFeatures;
        }
        private float[] ReduceDimensionality(float[] features)
        {
            var pca = new PrincipalComponentAnalysis();
            var reducedFeatures = pca.Transform(features);

            return reducedFeatures;
        }
        private void CreateTrainingDataset(string[] filePaths, int sampleRate, int intervalSeconds, int windowSize, int stepSize)
        {
            List<float[]> trainingData = new List<float[]>();

            foreach (var filePath in filePaths)
            {
                var featureList = ExtractFeatures(filePath, sampleRate, intervalSeconds);
                var aggregatedFeatures = AggregateFeaturesWithSlidingWindow(featureList, windowSize, stepSize);

                foreach (var features in aggregatedFeatures)
                {
                    var selectedFeatures = SelectRelevantFeatures(features);
                    var reducedFeatures = ReduceDimensionality(selectedFeatures);
                    trainingData.Add(reducedFeatures);
                }
            }

            // Save or use the trainingData for model training
        }
        */








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

        private bool isChromaEnabled { get; set; }
        private bool isTonnetzFeaturesEnabled { get; set; }
        private bool isMFCCsEnabled { get; set; }
        private bool isHPCPEnabled { get; set; }

        private bool isHarmonicPercussiveSourceSeparationFeaturesEnabled { get; set; }

        private bool isSpectralContrastEnabled { get; set; }
        private bool isSpectralCentroidEnabled { get; set; }
        private bool isSpectralBandwidthEnabled { get; set; }

        private bool isZeroCrossingRateEnabled { get; set; }
        private bool isRmsEnergyEnabled { get; set; }
        private bool isPitchEnabled { get; set; }

        private List<float[]> chromaFeatureList { get; set; }
        private List<float[]> tonnetzFeatureList { get; set; }
        private List<float[]> mfccFeatureList { get; set; }
        private List<float[]> hpcpFeatureList { get; set; }

        private List<float[]> harmonicPercussiveSourceSeparationFeatureList { get; set; }
        private List<float[]> harmonicFeatureList { get; set; }
        private List<float[]> percussiveFeatureList { get; set; }

        private List<float[]> spectralContrastFeatureList { get; set; }
        private List<float[]> spectralCentroidFeatureList { get; set; }
        private List<float[]> spectralBandwidthFeatureList { get; set; }

        private List<float[]> zeroCrossingRateFeatureList { get; set; }
        private List<float[]> rMSEnergyFeatureList { get; set; }
        private List<float[]> pitchFeatureList { get; set; }
       
        private float[] ExtractFeatures(string filePath, CancellationToken cancellationToken)
        {
            float[] trainingData = new float[282];

            using (var reader = new AudioFileReader(filePath))
            {
                var sampleProvider = reader.ToSampleProvider();
                float[] buffer = new float[reader.WaveFormat.SampleRate * intervalSeconds];
                int samplesRead;

                List<float[]> featuresList = new List<float[]>();
                int totalSamples = (int)Math.Ceiling((double)reader.Length / (buffer.Length * 2 * reader.WaveFormat.Channels)); // Adjust for bytes per sample and channels
                int currentSample = 0;

                if (enableExtractFeaturesLog)
                    Console.WriteLine($"Total samples: {totalSamples}");

                this.chromaFeatureList = new List<float[]>();
                this.tonnetzFeatureList = new List<float[]>();
                this.mfccFeatureList = new List<float[]>();
                this.hpcpFeatureList = new List<float[]>();
                this.harmonicPercussiveSourceSeparationFeatureList = new List<float[]>();
                this.harmonicFeatureList = new List<float[]>();
                this.percussiveFeatureList = new  List<float[]>();
                this.spectralContrastFeatureList = new List<float[]>();
                this.spectralCentroidFeatureList = new List<float[]>();
                this.spectralBandwidthFeatureList = new List<float[]>();
                this.zeroCrossingRateFeatureList = new List<float[]>();
                this.rMSEnergyFeatureList = new List<float[]>();
                this.pitchFeatureList = new List<float[]>();

                while ((samplesRead = sampleProvider.Read(buffer, 0, buffer.Length)) > 0)
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

                    // Extract and aggregate features
                    this.ExtractFeatures(fftBuffer, buffer, reader.WaveFormat.SampleRate);

                    //Log the progress
                    currentSample++;
                    int progressValue = currentSample * 100 / totalSamples;

                    if (enableExtractFeaturesLog)
                        Console.WriteLine($"{progressValue}% complete (Sample {currentSample} of {totalSamples}) - File: {filePath}");

                    this.ConcBag.Add(new MessageTest()
                    {
                        LogState = LogState.Extraction,
                        ExtractionProgressValue = progressValue,
                        CurrentSample = currentSample,
                        TotalSamples = totalSamples,
                        FilePath = filePath
                    });
                }

                trainingData = this.AggregateFeatures();

                return trainingData;
            }
        }
        private void ExtractFeatures(Complex[] fftBuffer, float[] buffer, int sampleRate)
        {
            // Extract Chroma features
            if (isChromaEnabled)
            {
                var chromaFeatures = new float[chromaFeaturesLength];
                chromaFeatures = ExtractChromaFeatures(fftBuffer);
                chromaFeatureList.Add(chromaFeatures);

                if (isTonnetzFeaturesEnabled)
                {
                    // Extract Tonnetz features
                    var tonnetzFeatures = new float[tonnetzFeaturesLength];
                    tonnetzFeatures = ExtractTonnetzFeatures(chromaFeatures);
                    tonnetzFeatureList.Add(chromaFeatures);
                }
            }
            if (isMFCCsEnabled)
            {
                // Extract MFCCs features
                var mfccFeatures= new float[mFCCFeaturesLength];
                mfccFeatures = ExtractMFCCsFeatures(buffer, sampleRate);
                mfccFeatureList.Add(mfccFeatures);
            }
            if (isHPCPEnabled)
            {
                // Extract Harmonic Percussive Source Separation features
                var hPCPFeatures = new float[hpcpFeaturesLength];
                hPCPFeatures = this.ExtractHPCPFeatures(fftBuffer, sampleRate);
                hpcpFeatureList.Add(hPCPFeatures);
            }
            if (isHarmonicPercussiveSourceSeparationFeaturesEnabled)
            {
                // Harmonic/Percussive Source Separation features
                var (harmonic, percussive) = ExtractHarmonicPercussionSourceSeparationFeatures(buffer, sampleRate);
                harmonicFeatureList.Add(harmonic);
                percussiveFeatureList.Add(percussive);
            }

            if (isSpectralContrastEnabled)
            {
                // Extract Spectral Contrast features
                var spectralContrastFeatures = new float[spectralContrastFeaturesLength];
                spectralContrastFeatures = ExtractSpectralContrastFeatures(fftBuffer);
                spectralContrastFeatureList.Add(spectralContrastFeatures);
            }
            if (isSpectralCentroidEnabled)
            {
                // Extract Spectral Centroid features
                var spectralCentroidFeatures = new float[spectralCentroidFeaturesLength];
                spectralCentroidFeatures[0] = ExtractSpectralCentroidFeatures(fftBuffer);
                this.spectralCentroidFeatureList.Add(spectralCentroidFeatures);
            }
            if (isSpectralBandwidthEnabled)
            {
                // Extract Spectral Bandwidth features
                var spectralBandwidthFeatures = new float[spectralBandwidthFeaturesLength];
                spectralBandwidthFeatures[0] = ExtractSpectralBandwidthFeatures(buffer);
                this.spectralBandwidthFeatureList.Add(spectralBandwidthFeatures);
            }

            if (isZeroCrossingRateEnabled)
            {
                // Extract Zero-Crossing Rate features
                var zeroCrossingRateFeatures = new float[zeroCrossingRateFeaturesLength];
                zeroCrossingRateFeatures[0] = ExtractZeroCrossingRateFeatures(buffer);
                this.zeroCrossingRateFeatureList.Add(zeroCrossingRateFeatures);
            }
            if (isRmsEnergyEnabled)
            {
                // Extract RMS Energy features
                var rmsEnergyFeatures = new float[rMSEnergyFeaturesLength];
                rmsEnergyFeatures[0] = ExtractRmsEnergyFeatures(buffer);
                this.rMSEnergyFeatureList.Add(rmsEnergyFeatures);
            }
            if (isPitchEnabled)
            {
                // Extract Pitch features
                var pitchFeatures = new float[pitchFeaturesLength];
                pitchFeatures[0] = ExtractPitchFeatures(buffer, sampleRate);
                this.pitchFeatureList.Add(pitchFeatures);
            }
        }
        private float[] AggregateFeatures()
        {
            List<float> trainingData = new List<float>();

            //Chroma
            if (isChromaEnabled && chromaFeatureList != null && chromaFeatureList.Count > 0)
            {
                for (int i = 0; i < chromaFeatureList.Count; i++)
                {
                    float[] chromaValues = chromaFeatureList.Select(cf => cf[i]).ToArray();
                    trainingData.Add(Statistics.Mean(chromaValues));
                    trainingData.Add(Statistics.Variance(chromaValues));
                    trainingData.Add(Statistics.StandardDeviation(chromaValues));
                    trainingData.Add(Statistics.Min(chromaValues));
                    trainingData.Add(Statistics.Max(chromaValues));
                    trainingData.Add(Statistics.Median(chromaValues));
                    trainingData.Add(Statistics.Entropy(chromaValues));
                    trainingData.Add(Statistics.Mode(chromaValues));
                }
            }

            //MFCCs
            if (isMFCCsEnabled && mfccFeatureList != null && mfccFeatureList.Count > 0)
            {
                for (int i = 0; i < mfccFeatureList.Count; i++)
                {
                    float[] mfccsValues = mfccFeatureList.Select(cf => cf[i]).ToArray();
                    trainingData.Add(Statistics.Mean(mfccsValues));
                    trainingData.Add(Statistics.Variance(mfccsValues));
                    trainingData.Add(Statistics.StandardDeviation(mfccsValues));
                    trainingData.Add(Statistics.Min(mfccsValues));
                    trainingData.Add(Statistics.Max(mfccsValues));
                    trainingData.Add(Statistics.Median(mfccsValues));
                    trainingData.Add(Statistics.Skewness(mfccsValues));
                    trainingData.Add(Statistics.Kurtosis(mfccsValues));
                }
            }

            //Spectral Contrast
            if (isSpectralContrastEnabled && spectralContrastFeatureList != null && spectralContrastFeatureList.Count > 0)
            {
                for (int i = 0; i < spectralContrastFeatureList.Count; i++)
                {
                    float[] spectralContrastValues = spectralContrastFeatureList.Select(sc => sc[i]).ToArray();
                    trainingData.Add(Statistics.Mean(spectralContrastValues));
                    trainingData.Add(Statistics.Variance(spectralContrastValues));
                    trainingData.Add(Statistics.StandardDeviation(spectralContrastValues));
                    trainingData.Add(Statistics.Min(spectralContrastValues));
                    trainingData.Add(Statistics.Max(spectralContrastValues));
                    trainingData.Add(Statistics.Median(spectralContrastValues));
                    trainingData.Add(Statistics.Range(spectralContrastValues));
                    trainingData.Add(Statistics.InterquartileRange(spectralContrastValues));
                }
            }

            //Spectral Centroid
            if (isSpectralCentroidEnabled && spectralCentroidFeatureList != null && spectralCentroidFeatureList.Count > 0)
            {
                for (int i = 0; i < spectralCentroidFeatureList.Count; i++)
                {
                    float[] spectralCentroidValues = spectralCentroidFeatureList.Select(sc => sc[i]).ToArray();
                    trainingData.Add(Statistics.Mean(spectralCentroidValues));
                    trainingData.Add(Statistics.Variance(spectralCentroidValues));
                    trainingData.Add(Statistics.StandardDeviation(spectralCentroidValues));
                    trainingData.Add(Statistics.Min(spectralCentroidValues));
                    trainingData.Add(Statistics.Max(spectralCentroidValues));
                    trainingData.Add(Statistics.Median(spectralCentroidValues));
                    trainingData.Add(Statistics.Skewness(spectralCentroidValues));
                }
            }

            //Spectral Bandwidth
            if (isSpectralBandwidthEnabled && spectralBandwidthFeatureList != null && spectralBandwidthFeatureList.Count > 0)
            {
                for (int i = 0; i < spectralBandwidthFeatureList.Count; i++)
                {
                    float[] spectralBandwidthValues = spectralBandwidthFeatureList.Select(sc => sc[i]).ToArray();
                    trainingData.Add(Statistics.Mean(spectralBandwidthValues));
                    trainingData.Add(Statistics.Variance(spectralBandwidthValues));
                    trainingData.Add(Statistics.StandardDeviation(spectralBandwidthValues));
                    trainingData.Add(Statistics.Min(spectralBandwidthValues));
                    trainingData.Add(Statistics.Max(spectralBandwidthValues));
                    trainingData.Add(Statistics.Median(spectralBandwidthValues));
                    trainingData.Add(Statistics.Skewness(spectralBandwidthValues));
                }
            }

            //Zero-Crossing Rate
            if (isZeroCrossingRateEnabled && zeroCrossingRateFeatureList != null && zeroCrossingRateFeatureList.Count > 0)
            {
                for (int i = 0; i < zeroCrossingRateFeatureList.Count; i++)
                {
                    float[] zcrValues = zeroCrossingRateFeatureList.Select(sc => sc[i]).ToArray();
                    trainingData.Add(Statistics.Mean(zcrValues));
                    trainingData.Add(Statistics.Variance(zcrValues));
                    trainingData.Add(Statistics.StandardDeviation(zcrValues));
                    trainingData.Add(Statistics.Min(zcrValues));
                    trainingData.Add(Statistics.Max(zcrValues));
                    trainingData.Add(Statistics.Mode(zcrValues));
                }
            }

            //Rms
            if (isRmsEnergyEnabled && rMSEnergyFeatureList != null && rMSEnergyFeatureList.Count > 0)
            {
                for (int i = 0; i < rMSEnergyFeatureList.Count; i++)
                {
                    float[] rmsEnergyValues = rMSEnergyFeatureList.Select(sc => sc[i]).ToArray();
                    trainingData.Add(Statistics.Mean(rmsEnergyValues));
                    trainingData.Add(Statistics.Variance(rmsEnergyValues));
                    trainingData.Add(Statistics.StandardDeviation(rmsEnergyValues));
                    trainingData.Add(Statistics.Min(rmsEnergyValues));
                    trainingData.Add(Statistics.Max(rmsEnergyValues));
                    trainingData.Add(Statistics.Sum(rmsEnergyValues));
                    trainingData.Add(Statistics.PeakToPeakAmplitude(rmsEnergyValues));
                }
            }

            //Pitch
            if (isPitchEnabled && pitchFeatureList != null && pitchFeatureList.Count > 0)
            {
                for (int i = 0; i < pitchFeatureList.Count; i++)
                {
                    float[] pitchValues = pitchFeatureList.Select(sc => sc[i]).ToArray();
                    trainingData.Add(Statistics.Mean(pitchValues));
                    trainingData.Add(Statistics.Variance(pitchValues));
                    trainingData.Add(Statistics.StandardDeviation(pitchValues));
                    trainingData.Add(Statistics.Min(pitchValues));
                    trainingData.Add(Statistics.Max(pitchValues));
                    trainingData.Add(Statistics.Median(pitchValues));
                    trainingData.Add(Statistics.Mode(pitchValues));
                }
            }

            return trainingData.ToArray();
        }



       



        /* private List<float[]> ExtractChromaFeatures(float[] audioSamples, int segmentSize)
         {
             List<float[]> chromaFeaturesList = new List<float[]>();
             int numSegments = audioSamples.Length / segmentSize;

             for (int i = 0; i < numSegments; i++)
             {
                 float[] segment = audioSamples.Skip(i * segmentSize).Take(segmentSize).ToArray();
                 float[] chromaFeatures = CalculateChromaFeatures(segment);
                 chromaFeaturesList.Add(chromaFeatures);
             }
         }*/






        /*
            private void AggregateAndFillFeatureVector(List<float> featureVector, float[][] features, int targetSize)
            {
                int originalSize = features.Length;
                int segmentSize = originalSize / targetSize;

                for (int i = 0; i < targetSize; i++)
                {
                    int start = i * segmentSize;
                    int end = Math.Min(start + segmentSize, originalSize);
                    var segment = features.Skip(start).Take(end - start).SelectMany(f => f).ToArray();

                    featureVector.Add(segment.Average()); // Mean
                    featureVector.Add(Variance(segment)); // Variance
                    featureVector.Add(StandardDeviation(segment)); // Standard Deviation
                    featureVector.Add(segment.Min()); // Min
                    featureVector.Add(segment.Max()); // Max
                    featureVector.Add(Percentile(segment, 25)); // 25th Percentile
                    featureVector.Add(Percentile(segment, 75)); // 75th Percentile
                }
            }*/




        /*
        private void ExtractAndAggregateFeatures(List<float[]> featureList, Complex[] fftBuffer, float[] buffer, int sampleRate)
        {
            List<float> aggregatedFeatures = new List<float>();
            if (IsChromaFeaturesEnabled)
            {
                // Extract chroma features from FFT result
                float[] chromaFeatures = ExtractChromaFeatures(fftBuffer);
                float meanChroma = chromaFeatures.Average();
                float varianceChroma = chromaFeatures.Select(f => (f - meanChroma) * (f - meanChroma)).Average();
                float minChroma = chromaFeatures.Min();
                float maxChroma = chromaFeatures.Max();
                aggregatedFeatures.AddRange(new float[] { meanChroma, varianceChroma, minChroma, maxChroma });

                if (IsTonnetzFeaturesEnabled)
                {
                    // Extract Tonnetz features
                    float[] tonnetzFeatures = ExtractTonnetzFeatures(chromaFeatures);
                    aggregatedFeatures.AddRange(tonnetzFeatures);
                }
                
            }

            if (IsMFCCsEnabled)
            {
                // Extract MFCCs
                float[] mfccFeatures = ExtractMFCCs(buffer, sampleRate);
                float meanMFCC = mfccFeatures.Average();
                float varianceMFCC = mfccFeatures.Select(f => (f - meanMFCC) * (f - meanMFCC)).Average();
                float minMFCC = mfccFeatures.Min();
                float maxMFCC = mfccFeatures.Max();
                aggregatedFeatures.AddRange(new float[] { meanMFCC, varianceMFCC, minMFCC, maxMFCC });
            }

            if (IsSpectralContrastEnabled)
            {
                // Extract spectral contrast
                float[] spectralContrastFeatures = ExtractSpectralContrast(fftBuffer, 6); // Adjust length as needed
                float meanSpectralContrast = spectralContrastFeatures.Average();
                float varianceSpectralContrast = spectralContrastFeatures.Select(f => (f - meanSpectralContrast) * (f - meanSpectralContrast)).Average();
                float minSpectralContrast = spectralContrastFeatures.Min();
                float maxSpectralContrast = spectralContrastFeatures.Max();
                aggregatedFeatures.AddRange(new float[] { meanSpectralContrast, varianceSpectralContrast, minSpectralContrast, maxSpectralContrast });
            }

            if (IsHPCPEnabled)
            {
                // Extract HPCP
                float[] hpcpFeatures = ExtractHPCP(fftBuffer, sampleRate);
                float meanHPCP = hpcpFeatures.Average();
                float varianceHPCP = hpcpFeatures.Select(f => (f - meanHPCP) * (f - meanHPCP)).Average();
                float minHPCP = hpcpFeatures.Min();
                float maxHPCP = hpcpFeatures.Max();
                aggregatedFeatures.AddRange(new float[] { meanHPCP, varianceHPCP, minHPCP, maxHPCP });
            }

            if (IsHPSEnabled)
            {
                // Harmonic/Percussive Source Separation
                var (harmonic, percussive) = ExtractHPSFeatures(buffer, sampleRate);
                float meanHarmonic = harmonic.Average();
                float varianceHarmonic = harmonic.Select(f => (f - meanHarmonic) * (f - meanHarmonic)).Average();
                float minHarmonic = harmonic.Min();
                float maxHarmonic = harmonic.Max();
                aggregatedFeatures.AddRange(new float[] { meanHarmonic, varianceHarmonic, minHarmonic, maxHarmonic });

                float meanPercussive = percussive.Average();
                float variancePercussive = percussive.Select(f => (f - meanPercussive) * (f - meanPercussive)).Average();
                float minPercussive = percussive.Min();
                float maxPercussive = percussive.Max();
                aggregatedFeatures.AddRange(new float[] { meanPercussive, variancePercussive, minPercussive, maxPercussive });
            }

            if (IsSpectralCentroidEnabled)
            {
                // Extract spectral centroid
                float spectralCentroid = ExtractSpectralCentroid(fftBuffer);
                aggregatedFeatures.Add(spectralCentroid);
            }

            if (IsSpectralBandwidthEnabled)
            {
                // Extract spectral bandwidth
                float spectralBandwidth = ExtractSpectralBandwidth(buffer);
                aggregatedFeatures.Add(spectralBandwidth);
            }

            if (IsZeroCrossingRateEnabled)
            {
                // Extract zero-crossing rate
                float zcr = ExtractZeroCrossingRate(buffer);
                aggregatedFeatures.Add(zcr);
            }

            if (IsRmsEnergyEnabled)
            {
                // Extract RMS energy
                float rms = ExtractRmsEnergy(buffer);
                aggregatedFeatures.Add(rms);
            }

            if (IsPitchEnabled)
            {
                // Extract pitch
                float pitch = ExtractPitch(buffer, sampleRate);
                aggregatedFeatures.Add(pitch);
            }

            featureList.Add(aggregatedFeatures.ToArray());
        }
        */
        private float[] ApplyPCA(List<float[]> featureList, int targetDimension)
        {
            
                // Convert the list of padded feature arrays to a MathNet matrix
                var matrix = Matrix<double>.Build.DenseOfRowArrays(featureList.Select(f => f.Select(v => (double)v).ToArray()).ToArray());

                // Calculate the mean of each column
                var mean = matrix.ColumnSums() / matrix.RowCount;

                // Center the matrix by subtracting the mean from each column
                var centered = matrix - Matrix<double>.Build.Dense(matrix.RowCount, matrix.ColumnCount, (r, c) => mean[c]);

                // Compute the covariance matrix
                var covariance = centered.TransposeThisAndMultiply(centered) / (matrix.RowCount - 1);

                // Perform eigen decomposition
                var evd = covariance.Evd();
                var eigenVectors = evd.EigenVectors;

                // Ensure targetDimension does not exceed the number of original features
                targetDimension = Math.Min(targetDimension, eigenVectors.ColumnCount);

                // Select the top 'targetDimension' eigenvectors
                var selectedEigenVectors = eigenVectors.SubMatrix(0, eigenVectors.RowCount, 0, targetDimension);

                // Project the centered data onto the selected eigenvectors
                var reducedData = centered * selectedEigenVectors;

                // Flatten the reduced data matrix to a 1D array
                return reducedData.ToRowMajorArray().Select(d => (float)d).ToArray();
            }
        

        private float[] ExtractChromaFeatures(Complex[] fftBuffer)
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
        private float[] ExtractTonnetzFeatures(float[] chromaFeatures)
        {
            // Ensure the chroma features are in the correct format
            if (chromaFeatures.Length % chromaFeaturesLength != 0)
            {
                throw new ArgumentException("Chroma features length must be a multiple of 12.");
            }

            int numFrames = chromaFeatures.Length / chromaFeaturesLength;
            float[] tonnetzFeatures = new float[numFrames * tonnetzFeaturesLength];


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
                float[] chromaFrame = new float[chromaFeaturesLength];
                Array.Copy(chromaFeatures, frame * chromaFeaturesLength, chromaFrame, 0, chromaFeaturesLength);

                for (int i = 0; i < tonnetzFeaturesLength; i++)
                {
                    float tonnetzValue = 0;
                    for (int j = 0; j < chromaFeaturesLength; j++)
                    {
                        tonnetzValue += tonnetzMatrix[i, j] * chromaFrame[j];
                    }
                    tonnetzFeatures[frame * tonnetzFeaturesLength + i] = tonnetzValue;
                }
            }

            return tonnetzFeatures;
        }
        private float[] ExtractMFCCsFeatures(float[] buffer, int sampleRate, int numCoefficients = mFCCFeaturesLength, int numFilters = 26, int fftSize = 512)
        {
            // Step 1: Pre-emphasis
            float[] preEmphasized = new float[buffer.Length];
            float preEmphasis = 0.97f;
            for (int i = 1; i < buffer.Length; i++)
            {
                preEmphasized[i] = buffer[i] - preEmphasis * buffer[i - 1];
            }
            if (enableExtractMFCCsLog)
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
        private float MelScale(float freq)
        {
            return 2595 * (float)Math.Log10(1 + freq / 700);
        }
        private float[] ExtractHPCPFeatures(Complex[] fftBuffer, int sampleRate)
        {
            float[] hpcp = new float[hpcpFeaturesLength];
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

        private float[] ExtractSpectralContrastFeatures(Complex[] fftBuffer, int numBands = spectralContrastFeaturesLength)
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
            if (enableExtractSpectralContrastLog)
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

            // Log the length of the returned array
            if (enableExtractSpectralContrastLog)
                Console.WriteLine($"Spectral contrast features length: {spectralContrast.Length}");

            return spectralContrast;
        }
        private float ExtractSpectralCentroidFeatures(Complex[] fftBuffer)
        {
            double weightedSum = 0;
            double totalMagnitude = 0;
            int numBins = fftBuffer.Length / 2;

            if (enableExtractSpectralCentroidLog)
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
        private float ExtractSpectralBandwidthFeatures(float[] audioSamples)
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

        private (float[] harmonic, float[] percussive) ExtractHarmonicPercussionSourceSeparationFeatures(float[] buffer, int sampleRate)
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
        
        private float ExtractZeroCrossingRateFeatures(float[] buffer)
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
        private float ExtractRmsEnergyFeatures(float[] buffer)
        {
            double sumOfSquares = buffer.Select(sample => sample * sample).Sum();
            return (float)Math.Sqrt(sumOfSquares / buffer.Length);
        }
        private float ExtractPitchFeatures(float[] buffer, int sampleRate)
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
        private static double Log2(double x)
        {
            return Math.Log(x) / Math.Log(2);
        }


        public void WriteToHDF5(string fullPath)
        {
            using (LoadingDialog loadingDialog = new LoadingDialog())
            {
                loadingDialog.Show();

                // Open the HDF5 file for read/write access, creating it if it doesn't exist
                long fileId = H5F.open(fullPath, H5F.ACC_RDWR);
                if (fileId < 0)
                {
                    fileId = H5F.create(fullPath, H5F.ACC_TRUNC);
                    if (fileId < 0)
                    {
                        throw new Exception("Failed to create or open HDF5 file.");
                    }
                }

                int writtenTracks = 0;
                long totalSizeInBytes = 0;

                foreach (var track in tracks)
                {
                    try
                    {
                        // Estimate size of the key attribute
                        long keyAttrSize = track.Key.Length;

                        // Estimate size of the features dataset
                        long featuresSize = track.Features.Length * sizeof(float);

                        // Add to total size
                        totalSizeInBytes += keyAttrSize + featuresSize;

                        // Create a group for each track
                        long groupId = H5G.create(fileId, track.Path);
                        if (groupId < 0)
                        {
                            throw new Exception($"Failed to create group for track {track.Path}.");
                        }

                        // Save key as an attribute
                        long keyAttrType = H5T.copy(H5T.C_S1);
                        H5T.set_size(keyAttrType, new IntPtr(track.Key.Length));
                        long keyAttrSpace = H5S.create(H5S.class_t.SCALAR);
                        long keyAttrId = H5A.create(groupId, "Key", keyAttrType, keyAttrSpace);
                        byte[] keyBytes = System.Text.Encoding.ASCII.GetBytes(track.Key);
                        GCHandle keyHandle = GCHandle.Alloc(keyBytes, GCHandleType.Pinned);
                        H5A.write(keyAttrId, keyAttrType, keyHandle.AddrOfPinnedObject());
                        keyHandle.Free();
                        H5A.close(keyAttrId);
                        H5S.close(keyAttrSpace);
                        H5T.close(keyAttrType);

                        // Save features as a dataset
                        long spaceId = H5S.create_simple(1, new ulong[] { (ulong)track.Features.Length }, null);
                        long datasetId = H5D.create(groupId, "Features", H5T.NATIVE_FLOAT, spaceId);
                        GCHandle handle = GCHandle.Alloc(track.Features, GCHandleType.Pinned);
                        H5D.write(datasetId, H5T.NATIVE_FLOAT, H5S.ALL, H5S.ALL, H5P.DEFAULT, handle.AddrOfPinnedObject());
                        handle.Free();
                        H5D.close(datasetId);
                        H5S.close(spaceId);

                        H5G.close(groupId);

                        // Update the writing progress bar
                        writtenTracks++;

                        if (totalSizeInBytes > 0)
                        {
                            double totalSizeInMegabytes = totalSizeInBytes / (1024.0 * 1024.0);
                            loadingDialog.SetProcessDescription($"Written track {writtenTracks}/{tracks.Count} to HDF5 file. Estimated size: {totalSizeInMegabytes:F2} MB.");
                            loadingDialog.Refresh();

                            this.ConcBag.Add(new MessageTest()
                            {
                                LogState = LogState.FileReading,
                                EstimatedSize = totalSizeInMegabytes * this.trackList.Count
                            });

                            Console.WriteLine($"Written track {writtenTracks}/{tracks.Count} to HDF5 file. Estimated size: {totalSizeInMegabytes:F2} MB.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error writing track {track.Path}: {ex.Message}");
                    }
                }

                H5F.close(fileId);
            }
        }
        private void DeleteTrainingDataEvent(object sender, Messenger e)
        {
            DialogResult messageBoxResult = MessageBox.Show("Do you really want to delete this training data?", "Delete Confirmation", MessageBoxButtons.YesNo);
            if (messageBoxResult == DialogResult.Yes)
            {
                this.trackDao.DeleteTrainingData(e.IntegerField1);

                for (int i = 0; i < this.trainingDataTable.Rows.Count; i++)
                {
                    if (Convert.ToInt32(this.trainingDataTable.Rows[i]["Id"]) == e.IntegerField1)
                    {
                        this.trainingDataTable.Rows.RemoveAt(i);
                        break;
                    }
                }
            }
        }
        /*  public void SaveFeaturesToCsv(string filePath, List<TrackForTraining> tracks)
          {
              using (var writer = new StreamWriter(filePath))
              using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
              {
                  csv.WriteRecords(tracks.Select(t => new
                  {
                      t.Path,
                      t.Key,
                      Features = string.Join(",", t.Features)
                  }));
              }
          }*/


       /* public class ChromaExtractor : FeatureExtractor
        {
            private readonly int _fftSize;
            private readonly Fft _fft;
            private readonly float[] _window;
            private readonly float[][] _filterbank;

            public ChromaExtractor(int sampleRate, int fftSize = 2048) : base(sampleRate, 12)
            {
                _fftSize = fftSize;
                _fft = new Fft(fftSize);
                _window = Window.OfType(WindowTypes.Hann, fftSize);
                _filterbank = FilterBanks.Chroma(sampleRate, fftSize);
            }

            public override float[][] ComputeFrom(float[] signal)
            {
                int hopSize = _fftSize / 2;
                int numFrames = (signal.Length - _fftSize) / hopSize + 1;
                float[][] chromaFeatures = new float[numFrames][];

                for (int i = 0; i < numFrames; i++)
                {
                    float[] frame = signal.Skip(i * hopSize).Take(_fftSize).ToArray();
                    frame.ApplyWindow(_window);

                    var spectrum = new Complex[_fftSize];
                    _fft.RealFft(frame, spectrum);

                    var magnitudes = spectrum.Select(c => c.Magnitude).ToArray();
                    chromaFeatures[i] = new float[FeatureCount];

                    for (int j = 0; j < FeatureCount; j++)
                    {
                        chromaFeatures[i][j] = _filterbank[j].Zip(magnitudes, (f, m) => f * m).Sum();
                    }

                    // Normalize chroma features
                    var sum = chromaFeatures[i].Sum();
                    if (sum > 0)
                    {
                        for (int j = 0; j < FeatureCount; j++)
                        {
                            chromaFeatures[i][j] /= sum;
                        }
                    }
                }

                return chromaFeatures;
            }
        }
        */




        private String modelFilePath { get; set; }
        private bool isTraining { get; set; }
        private CancellationTokenSource cancellationTokenSourceForTraining { get; set; }
        private void TrainModelEvent(object sender, EventArgs e)
        {
            if (isTraining)
            {
                MessageBox.Show("Training is in progress, please wait!", "Training Model Validation", MessageBoxButtons.OK);
            }
            else
            {
                if (this.CurrentTrainingData == null)
                {
                    MessageBox.Show("Training Data must be load for trainng!", "Training Model Validation", MessageBoxButtons.OK);
                }
                else
                {

                    modelFilePath = this.SaveModel();
                    if (!String.IsNullOrEmpty(modelFilePath))
                    {
                        isTraining = true;
                        this.view.ChangeTrainingStatus(true);
                        cancellationTokenSourceForTraining = new CancellationTokenSource();
                        this.TrainModelEvent(cancellationTokenSourceForTraining.Token);
                    }

                }
            }
        }
        private String SaveModel()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "zip files (*.zip)|*.zip";
            sfd.RestoreDirectory = true;
            sfd.FileName = "Model_" + DateTime.Now.ToString("yyyyMMddHHmm") + "_" + this.CurrentPlaylist.Name + "_" + this.CurrentTag.Name;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                return sfd.FileName;
            }
            else
            {
                return "";
            }
        }
        private async void TrainModelEvent(CancellationToken cancellationToken)
        {
            try
            {
                await TrainModelAsync(cancellationToken);
            }
            finally
            {
                isTraining = false;
                this.view.ChangeTrainingStatus(false);
            }
        }
        private async Task TrainModelAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                try
                {
                    var models = TrainModels(this.CurrentTrainingData.FilePath);
                    var ensembleModel = CombineModels(models);
                }
                catch (Exception ex)
                {

                }
            });
        }

        private static class FeatureConstants
        {
            public const int FeatureCount = 100;
        }
        public class AudioData
        {
            [LoadColumn(0)]
            public string Path { get; set; }

            [LoadColumn(1)]
            public string Label { get; set; } // Keep the label as a string

            [LoadColumn(2, 2 + FeatureConstants.FeatureCount - 1)]
            [VectorType(FeatureConstants.FeatureCount)]
            public float[] Features { get; set; }
        }
        private MLContext _mlContext = new MLContext();
        private List<ITransformer> TrainModels(string fileName)
        {
            var models = new List<ITransformer>();
            int batchNumber = 0;

            var pipeline = _mlContext.Transforms.Conversion.MapValueToKey(nameof(AudioData.Label))
            .Append(_mlContext.Transforms.Concatenate("Features", nameof(AudioData.Features)))
            .Append(_mlContext.Transforms.NormalizeMinMax("Features"))
            .Append(_mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy(new SdcaMaximumEntropyMulticlassTrainer.Options
            {
                LabelColumnName = nameof(AudioData.Label),
                FeatureColumnName = "Features"
            }))
            .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            // Load data in batches and train separate models
            foreach (var batch in LoadBatchesFromHDF5(fileName))
            {
                batchNumber++;
                Console.WriteLine($"Starting training for batch {batchNumber}...");

                var data = _mlContext.Data.LoadFromEnumerable(batch);
                var model = pipeline.Fit(data);
                models.Add(model);

                Console.WriteLine($"Completed training for batch {batchNumber}.");
            }

            Console.WriteLine("All batches have been processed and models trained.");
            return models;
        }
        private IEnumerable<List<AudioData>> LoadBatchesFromHDF5(string fileName)
        {
            const int batchSize = 1000;
            var allTracks = LoadFromHDF5(fileName);
            int totalBatches = (int)Math.Ceiling((double)allTracks.Count / batchSize);
            int currentBatch = 0;

            for (int i = 0; i < allTracks.Count; i += batchSize)
            {
                currentBatch++;
                Console.WriteLine($"Loading batch {currentBatch} of {totalBatches}...");
                yield return allTracks.Skip(i).Take(batchSize).Select(track => new AudioData
                {
                    Path = track.Path,
                    Label = track.Label, // Use the Label property
                    Features = track.Features
                }).ToList();
                Console.WriteLine($"Batch {currentBatch} loaded.");
            }
        }
        private ITransformer CombineModels(List<ITransformer> models)
        {
            // Load a sample dataset to get the schema
            var sampleData = LoadSampleData();
            var sampleDataView = _mlContext.Data.LoadFromEnumerable(sampleData);

            // Create a list to store the predictions
            var predictions = new List<float[]>();

            // Make predictions with each model
            foreach (var model in models)
            {
                var transformedData = model.Transform(sampleDataView);
                var predictionColumn = _mlContext.Data.CreateEnumerable<Prediction>(transformedData, reuseRowObject: false)
                .Select(p => p.Score)
                .ToArray();
                predictions.Add(predictionColumn);
            }

            // Average the predictions
            var averagedPredictions = new float[predictions[0].Length];
            for (int i = 0; i < averagedPredictions.Length; i++)
            {
                averagedPredictions[i] = predictions.Select(p => p[i]).Average();
            }

            // Create a new model with the averaged predictions
            var averagedModel = CreateAveragedModel(averagedPredictions, sampleDataView.Schema);

            // Save the combined model
            string modelPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ensembleVocalDetectionModel.zip");
            _mlContext.Model.Save(averagedModel, sampleDataView.Schema, modelFilePath);
            Console.WriteLine($"Ensemble model saved to {modelPath}");

            return averagedModel;
        }

        private IEnumerable<AudioData> LoadSampleData()
        {
            var audioDataList = new List<AudioData>();

            // Open the HDF5 file
            long fileId = H5F.open(this.CurrentTrainingData.FilePath, H5F.ACC_RDONLY);
            if (fileId < 0)
            {
                throw new Exception("Failed to open HDF5 file.");
            }

            // Iterate over each group (track) in the file
            foreach (var trackPath in GetTrackPaths(fileId))
            {
                // Open the group
                long groupId = H5G.open(fileId, trackPath);
                if (groupId < 0)
                {
                    throw new Exception($"Failed to open group for track {trackPath}.");
                }

                // Read the key attribute
                long keyAttrId = H5A.open(groupId, "Key");
                long keyAttrType = H5A.get_type(keyAttrId);
                long keyAttrSpace = H5A.get_space(keyAttrId);
                int keyLength = (int)H5T.get_size(keyAttrType);
                byte[] keyBytes = new byte[keyLength];
                GCHandle keyHandle = GCHandle.Alloc(keyBytes, GCHandleType.Pinned);
                H5A.read(keyAttrId, keyAttrType, keyHandle.AddrOfPinnedObject());
                keyHandle.Free();
                string key = System.Text.Encoding.ASCII.GetString(keyBytes);
                H5A.close(keyAttrId);
                H5S.close(keyAttrSpace);
                H5T.close(keyAttrType);

                // Read the features dataset
                long datasetId = H5D.open(groupId, "Features");
                long spaceId = H5D.get_space(datasetId);
                ulong[] dims = new ulong[1];
                ulong[] maxdims = null; // You can set this to null if you don't need the maximum dimensions
                H5S.get_simple_extent_dims(spaceId, dims, maxdims);
                float[] features = new float[dims[0]];
                GCHandle handle = GCHandle.Alloc(features, GCHandleType.Pinned);
                H5D.read(datasetId, H5T.NATIVE_FLOAT, H5S.ALL, H5S.ALL, H5P.DEFAULT, handle.AddrOfPinnedObject());
                handle.Free();
                H5D.close(datasetId);
                H5S.close(spaceId);

                // Close the group
                H5G.close(groupId);

                // Create an AudioData object and add it to the list
                audioDataList.Add(new AudioData
                {
                    Path = trackPath,
                    Label = key, // Keep the label as a string
                    Features = features
                });
            }

            H5F.close(fileId);

            return audioDataList;
        }

        private IEnumerable<string> GetTrackPaths(long fileId)
        {
            var trackPaths = new List<string>();

            // Define a callback function to be called for each group
            H5L.iterate_t callback = (long group, IntPtr name, ref H5L.info_t info, IntPtr op_data) =>
            {
                // Convert the name pointer to a string
                string trackPath = Marshal.PtrToStringAnsi(name);
                trackPaths.Add(trackPath);
                return 0; // Continue iteration
            };

            // Initialize the index for iteration
            ulong idx = 0;

            // Iterate over all groups in the root of the file
            H5L.iterate(fileId, H5.index_t.NAME, H5.iter_order_t.NATIVE, ref idx, callback, IntPtr.Zero);

            return trackPaths;
        }

        private ITransformer CreateAveragedModel(float[] averagedPredictions, DataViewSchema schema)
        {
            // Create a list of Prediction objects from the averaged predictions
            var predictionList = averagedPredictions.Select(score => new Prediction { Score = score }).ToList();

            var data = _mlContext.Data.LoadFromEnumerable(predictionList);

            var pipeline = _mlContext.Transforms.CopyColumns("PredictedLabel", "Score");
            var model = pipeline.Fit(data);

            return model;
        }

        public class Prediction
        {
            public float Score { get; set; }
        }
       

    

        public List<AudioData> LoadFromHDF5(string fileName)
        {
            var audioDataList = new List<AudioData>();

            // Open the HDF5 file
            long fileId = H5F.open(fileName, H5F.ACC_RDONLY);
            if (fileId < 0)
            {
                throw new Exception("Failed to open HDF5 file.");
            }

            // Iterate over each group (track) in the file
            foreach (var trackPath in GetTrackPaths(fileId))
            {
                // Open the group
                long groupId = H5G.open(fileId, trackPath);
                if (groupId < 0)
                {
                    throw new Exception($"Failed to open group for track {trackPath}.");
                }

                // Read the key attribute
                long keyAttrId = H5A.open(groupId, "Key");
                long keyAttrType = H5A.get_type(keyAttrId);
                long keyAttrSpace = H5A.get_space(keyAttrId);
                int keyLength = (int)H5T.get_size(keyAttrType);
                byte[] keyBytes = new byte[keyLength];
                GCHandle keyHandle = GCHandle.Alloc(keyBytes, GCHandleType.Pinned);
                H5A.read(keyAttrId, keyAttrType, keyHandle.AddrOfPinnedObject());
                keyHandle.Free();
                string key = System.Text.Encoding.ASCII.GetString(keyBytes);
                H5A.close(keyAttrId);
                H5S.close(keyAttrSpace);
                H5T.close(keyAttrType);

                // Read the features dataset
                long datasetId = H5D.open(groupId, "Features");
                long spaceId = H5D.get_space(datasetId);
                ulong[] dims = new ulong[1];
                ulong[] maxdims = null; // You can set this to null if you don't need the maximum dimensions
                H5S.get_simple_extent_dims(spaceId, dims, maxdims);
                float[] features = new float[dims[0]];
                GCHandle handle = GCHandle.Alloc(features, GCHandleType.Pinned);
                H5D.read(datasetId, H5T.NATIVE_FLOAT, H5S.ALL, H5S.ALL, H5P.DEFAULT, handle.AddrOfPinnedObject());
                handle.Free();
                H5D.close(datasetId);
                H5S.close(spaceId);

                // Ensure the feature length is correct
                if (features.Length != 44) // Replace 44 with the expected feature length
                {
                    throw new Exception($"Unexpected feature length: {features.Length}. Expected: 44.");
                }

                // Close the group
                H5G.close(groupId);

                // Create an AudioData object and add it to the list
                audioDataList.Add(new AudioData
                {
                    Path = trackPath,
                    Label = key, // Use the key as the label
                    Features = features
                });
            }

            H5F.close(fileId);

            return audioDataList;
        }

        private void CloseViewWithOk(object sender, EventArgs e)
        {
            ((ModelTrainerView)this.view).DialogResult = DialogResult.OK;
            ((ModelTrainerView)this.view).Close();
        }

        bool enableExtractChromaFeaturesLog = false;
        bool enableExtractMFCCsLog = false;
        bool enableExtractSpectralContrastLog = false;
        bool enableExtractHPCPLog = false;
        bool enableExtractSpectralCentroidLog = false;
        bool enableExtractFeaturesLog = true;

        /* private float[] ExtractFeatures(string filePath)
         {
             using (var reader = new AudioFileReader(filePath))
             {
                 var sampleProvider = reader.ToSampleProvider();
                 float[] buffer = new float[reader.WaveFormat.SampleRate * 2];
                 int samplesRead;
                 List<float> features = new List<float>();
                 int totalSamples = (int)Math.Ceiling((double)reader.Length / (buffer.Length * 2 * 2)); // Adjust for bytes per sample and channels
                 int currentSample = 0;

                 if (enableExtractFeaturesLog)
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

                     if (this.IsChromaFeaturesEnabled)
                     {
                         // Extract chroma features from FFT result
                         float[] chromaFeatures = ExtractChromaFeatures(fftBuffer);
                         features.AddRange(chromaFeatures);

                         if (this.IsTonnetzFeaturesEnabled)
                         {
                             // Extract Tonnetz features
                             float[] tonnetzFeatures = ExtractTonnetzFeatures(chromaFeatures);
                             features.AddRange(tonnetzFeatures);
                         }
                     }

                     if(this.IsMFCCsEnabled)
                     {
                         // Extract MFCCs
                         float[] mfccFeatures = ExtractMFCCs(buffer, reader.WaveFormat.SampleRate);
                         features.AddRange(mfccFeatures);
                     }

                     if (this.IsSpectralContrastEnabled)
                     {
                         // Extract spectral contrast
                         float[] spectralContrastFeatures = ExtractSpectralContrast(fftBuffer, reader.WaveFormat.SampleRate);
                         features.AddRange(spectralContrastFeatures);
                     }

                     if(this.IsHPCPEnabled)
                     {
                         // Extract HPCP
                         float[] hpcpFeatures = ExtractHPCP(fftBuffer, reader.WaveFormat.SampleRate);
                         features.AddRange(hpcpFeatures);
                     }

                     if (this.IsHPSEnabled)
                     {
                         // Harmonic/Percussive Source Separation
                         var (harmonic, percussive) = HarmonicPercussiveSeparation(buffer, reader.WaveFormat.SampleRate);
                         features.AddRange(harmonic);
                         features.AddRange(percussive);
                     }

                     if(this.IsSpectralCentroidEnabled)
                     {
                         // Extract spectral centroid
                         float spectralCentroid = ExtractSpectralCentroid(fftBuffer, reader.WaveFormat.SampleRate);
                         features.Add(spectralCentroid);
                     }

                     currentSample++;
                     int progressValue = currentSample * 100 / totalSamples;

                     if (enableExtractFeaturesLog)
                         Console.WriteLine($"Processing {filePath}: {progressValue}% complete (Sample {currentSample} of {totalSamples})");

                 }

                 return features.ToArray();
             }
         }*/


        
      

        

        public enum LogState
        {
            Extraction,
            ParallelProcess,
            FileReading,
            Finish,
            Canceled
        }
        //TODO
       /* private void AddTrainingDataEvent(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = "HDF5 files (*.hdf5)|*.hdf5";
            // ofd.FilterIndex = this.settingDao.GetIntegerSetting(Settings.LastOpenTrainingDataFilterIndex.ToString());
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                /* TrainingData trainingData = new TrainingData()
                 {
                     Id = this.trackDao.GetNextId(TableName.TrainingData.ToString()),
                     FilePath = ofd.FileName,
                     TagId = this.CurrentTag.Id,
                     Name = this.CurrentPlaylist?.Name,
                     CreateDate = DateTime.Now,
                     SampleCount = this.trackList.Count,
                     Balance = this.balance
                 };

                //this.trackDao.CreateTrainingData(trainingData);
            }
             this.settingDao.SetIntegerSetting(Settings.LastOpenTrainingDataFilterIndex.ToString(), ofd.FilterIndex);

            this.InitializeTrainingDataListContent();
        }*/

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

        

        /*

        private List<float[]> ExtractFeaturesAtIntervals(string filePath, int intervalSeconds, CancellationToken cancellationToken)
        {
            using (var reader = new AudioFileReader(filePath))
            {
                var sampleProvider = reader.ToSampleProvider();
                int sampleRate = reader.WaveFormat.SampleRate;
                int bufferSize = sampleRate * intervalSeconds; // Buffer size for the interval
                float[] buffer = new float[bufferSize];
                int samplesRead;
                List<float[]> featuresList = new List<float[]>();

                while ((samplesRead = sampleProvider.Read(buffer, 0, buffer.Length)) > 0)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    // Perform FFT and extract features
                    Complex[] fftBuffer = new Complex[buffer.Length];
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        fftBuffer[i] = new Complex(buffer[i], 0);
                    }
                    Fourier.Forward(fftBuffer, FourierOptions.Matlab);

                    // Extract chroma features
                    float[] chromaFeatures = ExtractChromaFeatures(fftBuffer);
                    float pitch = ExtractPitch(buffer, sampleRate);

                    // Combine features
                    List<float> combinedFeatures = new List<float>(chromaFeatures) { pitch };
                    featuresList.Add(combinedFeatures.ToArray());
                }

                // Convert featuresList to a 2D array for PCA
                double[,] featureArray = new double[featuresList.Count, featuresList[0].Length];
                for (int i = 0; i < featuresList.Count; i++)
                {
                    for (int j = 0; j < featuresList[i].Length; j++)
                    {
                        featureArray[i, j] = featuresList[i][j];
                    }
                }

                // Apply PCA to reduce dimensionality
                double[,] reducedFeatures = ApplyPCA(featureArray, numComponents: 50); // Set desired number of components

                // Convert reduced features back to List<float[]>
                List<float[]> reducedFeaturesList = new List<float[]>();
                for (int i = 0; i < reducedFeatures.GetLength(0); i++)
                {
                    float[] row = new float[reducedFeatures.GetLength(1)];
                    for (int j = 0; j < reducedFeatures.GetLength(1); j++)
                    {
                        row[j] = (float)reducedFeatures[i, j];
                    }
                    reducedFeaturesList.Add(row);
                }

                return reducedFeaturesList;
            }
        }
        */
        /*
        private double[,] ApplyPCA(double[,] data, int numComponents)
        {
            var matrix = DenseMatrix.OfArray(data);
            var pca = new PrincipalComponentAnalysis(matrix, true);
            pca.Compute();

            var components = pca.Transform(matrix, numComponents);
            return components.ToArray();
        }

        private List<float[]> ExtractAndAggregateFeatures(string filePath, int intervalSeconds, CancellationToken cancellationToken)
        {
            using (var reader = new AudioFileReader(filePath))
            {
                var sampleProvider = reader.ToSampleProvider();
                int sampleRate = reader.WaveFormat.SampleRate;
                int bufferSize = sampleRate * intervalSeconds; // Buffer size for the interval
                float[] buffer = new float[bufferSize];
                int samplesRead;
                List<float[]> featuresList = new List<float[]>();

                while ((samplesRead = sampleProvider.Read(buffer, 0, buffer.Length)) > 0)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    // Perform FFT and extract features
                    Complex[] fftBuffer = new Complex[buffer.Length];
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        fftBuffer[i] = new Complex(buffer[i], 0);
                    }
                    Fourier.Forward(fftBuffer, FourierOptions.Matlab);

                    // Extract chroma features
                    float[] chromaFeatures = ExtractChromaFeatures(fftBuffer);
                    float pitch = ExtractPitch(buffer, sampleRate);

                    // Aggregate features
                    float meanChroma = chromaFeatures.Average();
                    float varianceChroma = chromaFeatures.Select(f => (f - meanChroma) * (f - meanChroma)).Average();

                    // Combine features
                    List<float> aggregatedFeatures = new List<float> { meanChroma, varianceChroma, pitch };
                    featuresList.Add(aggregatedFeatures.ToArray());
                }

                return featuresList;
            }
        }
        */
        private void TrainModel(List<float[]> featuresList, List<string> labels)
        {
            var mlContext = new MLContext();
            var data = featuresList.Select((features, index) => new AudioData
            {
                Features = features,
                Label = labels[index]
            });

            var dataView = mlContext.Data.LoadFromEnumerable(data);

            var pipeline = mlContext.Transforms.Conversion.MapValueToKey(nameof(AudioData.Label))
            .Append(mlContext.Transforms.Concatenate("Features", nameof(AudioData.Features)))
            .Append(mlContext.Transforms.NormalizeMinMax("Features"))
            .Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy(new SdcaMaximumEntropyMulticlassTrainer.Options
            {
                LabelColumnName = nameof(AudioData.Label),
                FeatureColumnName = "Features"
            }))
            .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            var model = pipeline.Fit(dataView);
            // Save or use the model as needed
        }

        /*
        private List<float[]> ExtractAndAggregateFeatures(string filePath, int intervalSeconds, CancellationToken cancellationToken)
        {
            using (var reader = new AudioFileReader(filePath))
            {
                var sampleProvider = reader.ToSampleProvider();
                int sampleRate = reader.WaveFormat.SampleRate;
                int bufferSize = sampleRate * intervalSeconds; // Buffer size for the interval
                float[] buffer = new float[bufferSize];
                int samplesRead;
                List<float[]> featuresList = new List<float[]>();

                while ((samplesRead = sampleProvider.Read(buffer, 0, buffer.Length)) > 0)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    // Perform FFT and extract features
                    Complex[] fftBuffer = new Complex[buffer.Length];
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        fftBuffer[i] = new Complex(buffer[i], 0);
                    }
                    Fourier.Forward(fftBuffer, FourierOptions.Matlab);

                    // Extract chroma features
                    float[] chromaFeatures = ExtractChromaFeatures(fftBuffer);
                    float pitch = ExtractPitch(buffer, sampleRate);

                    // Aggregate features
                    float meanChroma = chromaFeatures.Average();
                    float varianceChroma = chromaFeatures.Select(f => (f - meanChroma) * (f - meanChroma)).Average();

                    // Combine features
                    List<float> aggregatedFeatures = new List<float> { meanChroma, varianceChroma, pitch };
                    featuresList.Add(aggregatedFeatures.ToArray());
                }

                // Convert featuresList to a 2D array for PCA
                double[,] featureArray = new double[featuresList.Count, featuresList[0].Length];
                for (int i = 0; i < featuresList.Count; i++)
                {
                    for (int j = 0; j < featuresList[i].Length; j++)
                    {
                        featureArray[i, j] = featuresList[i][j];
                    }
                }

                // Apply PCA to reduce dimensionality
                double[,] reducedFeatures = ApplyPCA(featureArray, numComponents: 20); // Set desired number of components

                // Convert reduced features back to List<float[]>
                List<float[]> reducedFeaturesList = new List<float[]>();
                for (int i = 0; i < reducedFeatures.GetLength(0); i++)
                {
                    float[] row = new float[reducedFeatures.GetLength(1)];
                    for (int j = 0; j < reducedFeatures.GetLength(1); j++)
                    {
                        row[j] = (float)reducedFeatures[i, j];
                    }
                    reducedFeaturesList.Add(row);
                }

                return reducedFeaturesList;
            }
        }

        private double[,] ApplyPCA(double[,] data, int numComponents)
        {
            var matrix = DenseMatrix.OfArray(data);
            var pca = new PrincipalComponentAnalysis(matrix, true);
            pca.Compute();

            var components = pca.Transform(matrix, numComponents);
            return components.ToArray();
        }


        private float[] ExtractChromaFeatures(Complex[] fftBuffer)
        {
            // Implement chroma feature extraction logic here
            // This is a placeholder example
            return new float[12]; // Example: 12 chroma bins
        }

        private float ExtractPitch(float[] buffer, int sampleRate)
        {
            // Implement pitch extraction logic here
            // This is a placeholder example
            return 0.0f; // Example: pitch value
        }
        */

    }
}
