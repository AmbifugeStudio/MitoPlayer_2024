using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Helpers
{
    public class DynamicFeatureLoader
    {
        public static IEnumerable<DynamicFeatureData> LoadData(string filePath, int actualFeatureCount)
        {
            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines.Skip(1)) // Skip header
            {
                var columns = line.Split(',');
                var features = new float[actualFeatureCount];
                for (int i = 0; i < actualFeatureCount; i++)
                {
                    features[i] = float.Parse(columns[2 + i]);
                }
                yield return new DynamicFeatureData { Features = features };
            }
        }
        public class DynamicFeatureData
        {
            public float[] Features { get; set; }
        }
    }
}
