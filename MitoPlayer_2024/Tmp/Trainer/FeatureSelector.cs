using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MitoPlayer_2024.Helpers
{
    public class FeatureSelector
    {
        private MLContext mlContext;

        public FeatureSelector()
        {
            mlContext = new MLContext();
        }
        private static float[] PadOrTruncate(float[] array, int length)
        {
            if (array.Length > length)
            {
                return array.Take(length).ToArray();
            }
            else if (array.Length < length)
            {
                var paddedArray = new float[length];
                Array.Copy(array, paddedArray, array.Length);
                return paddedArray;
            }
            else
            {
                return array;
            }
        }
    

    public float[][] SelectRelevantFeatures(float[][] features, float[] labels)
        {
            // Ensure features and labels are not null and have the same length
            if (features == null || labels == null || features.Length != labels.Length)
            {
                throw new ArgumentException("Features and labels must be non-null and have the same length.");
            }

            // Ensure all feature vectors have the same length
            int fixedSize = 6464; // Adjust this size as needed
            var fixedSizeFeatures = features.Select(f => PadOrTruncate(f, fixedSize)).ToArray();

            // Create data view
            var data = features.Zip(labels, (f, l) => new InputData { Features = f, Label = l });
            var dataView = mlContext.Data.LoadFromEnumerable(data);

            // Define the pipeline
            var pipeline = mlContext.Transforms.Concatenate("Features", nameof(InputData.Features))
            .Append(mlContext.Regression.Trainers.Sdca(new SdcaRegressionTrainer.Options
            {
                L1Regularization = 0.1f,
                LabelColumnName = nameof(InputData.Label),
                FeatureColumnName = "Features"
            }));

            // Fit the model
            ITransformer model;
            try
            {
                model = pipeline.Fit(dataView);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fitting the model: {ex.Message}");
                throw;
            }

            // Transform the data
            IDataView transformedData;
            try
            {
                transformedData = model.Transform(dataView);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error transforming the data: {ex.Message}");
                throw;
            }

            // Extract the selected features
            var selectedFeatures = mlContext.Data.CreateEnumerable<TransformedData>(transformedData, reuseRowObject: false)
            .Select(td => td.Features)
            .ToArray();

            return selectedFeatures;
        
    }

        public class InputData
        {
            [VectorType(6464)] // Adjust the size as needed
            public float[] Features { get; set; }
            public float Label { get; set; }
        }

        public class TransformedData
        {
            [VectorType(200)] // Adjust the size as needed
            public float[] Features { get; set; }
        }

        /* public class TransformedData
         {
             public float[] Features { get; set; }
         }*/
    }
}
