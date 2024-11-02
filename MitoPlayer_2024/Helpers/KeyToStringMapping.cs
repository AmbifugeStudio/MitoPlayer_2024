using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Helpers
{
    public static class KeyToStringMapping
    {
        public static IEstimator<ITransformer> CreateMappingPipeline(MLContext mlContext)
        {
            return mlContext.Transforms.Conversion.MapKeyToValue("LabelString", "Label")
            .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabelString", "PredictedLabel"));
        }
    }
}
