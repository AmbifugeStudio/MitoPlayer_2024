using MitoPlayer_2024.Helpers;
using NWaves.FeatureExtractors.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Trainer
{
    public class TrackForTraining
    {
        public string Path { get; set; }
        public string Key { get; set; }
        public float[] Features { get; set; }
    }
    public class TrainingDataCreator
    {
        private List<TrackForTraining> tracks = new List<TrackForTraining>();
        FeatureExtractor extractor = new FeatureExtractor();
        private int sampleRate;
        private int intervalSeconds;
        private int windowSize;
        private int stepSize;

        public TrainingDataCreator(int sampleRate, int intervalSeconds, int windowSize, int stepSize)
        {
            this.sampleRate = sampleRate;
            this.intervalSeconds = intervalSeconds;
            this.windowSize = windowSize;
            this.stepSize = stepSize;
        }

        public void AddTrack(string filePath, string tagValue, CancellationToken cancellationToken)
        {
           // var features = extractor.ExtractFeatures(filePath, cancellationToken);
           // tracks.Add(new TrackForTraining { Path = filePath, Key = tagValue, Features = features });


        }

        public void CreateTrainingDataset(int sampleRate, int intervalSeconds, int windowSize, int stepSize)
        {
            var featureAggregator = new FeatureAggregator();
            var featureSelector = new FeatureSelector();
            var dimensionalityReducer = new DimensionalityReducer();

            List<float[]> trainingData = new List<float[]>();
            List<float> labels = new List<float>();

            foreach (var track in tracks)
            {
                var featureList = featureAggregator.AggregateFeaturesWithSlidingWindow(track.Features, windowSize, stepSize);

                float label = GetLabelForKey(track.Key);
                labels.AddRange(Enumerable.Repeat(label, featureList.Count));

                trainingData.AddRange(featureList);
            }

            var selectedFeatures = featureSelector.SelectRelevantFeatures(trainingData.ToArray(), labels.ToArray());
            var reducedFeatures = dimensionalityReducer.ReduceDimensionality(selectedFeatures);

            SaveToCsv(reducedFeatures, labels.ToArray(), "training_data.csv");
        }

        private float GetLabelForKey(string key)
        {
            return key == "HasVocal" ? 1 : 0;
        }

        private void SaveToCsv(float[][] features, float[] labels, string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            {
                for (int i = 0; i < features.Length; i++)
                {
                    var line = string.Join(",", features[i]) + "," + labels[i];
                    writer.WriteLine(line);
                }
            }
        }
    }
}