using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Trainer
{
    public class DimensionalityReducer
    {
        private MLContext mlContext;

        public DimensionalityReducer()
        {
            mlContext = new MLContext();
        }

        public float[][] ReduceDimensionality(float[][] features)
        {
            var data = mlContext.Data.LoadFromEnumerable(features.Select(f => new InputData { Features = f }));

            var pipeline = mlContext.Transforms.Concatenate("Features", "Features")
            .Append(mlContext.Transforms.ProjectToPrincipalComponents("PCAFeatures", "Features", rank: 2));

            var transformer = pipeline.Fit(data);
            var transformedData = transformer.Transform(data);

            var pcaFeatures = mlContext.Data.CreateEnumerable<TransformedData>(transformedData, reuseRowObject: false)
            .Select(td => td.PCAFeatures)
            .ToArray();

            return pcaFeatures;
        }

        public class InputData
        {
            public float[] Features { get; set; }
        }

        public class TransformedData
        {
            public float[] PCAFeatures { get; set; }
        }
    }
}
