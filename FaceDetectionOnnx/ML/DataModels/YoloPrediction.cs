using System.Diagnostics.CodeAnalysis;

namespace FaceDetectionOnnx.ML.DataModels
{
    public class YoloPrediction
    {
        [AllowNull]
        public string Label { get; set; }
        public float Confidence { get; set; }
        public RectangleF Box { get; set; }
    }
}
