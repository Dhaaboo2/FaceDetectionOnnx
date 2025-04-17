using Microsoft.ML.OnnxRuntime;
using System;

namespace FaceDetectionOnnx.ML.DataModels
{
    public class OnnxOutputParser
    {
        private const float ConfidenceThreshold = 0.5f;

        //private const float ConfidenceThreshold = 0.5f;
        public const float IouThreshold = 0.4f;

        public List<YoloPrediction> ParseOutputs(IDisposableReadOnlyCollection<DisposableNamedOnnxValue> results, int imgW, int imgeH)
        {
            var _boxes = new List<YoloPrediction>();
            var result = results.First().AsEnumerable<float>().ToArray();
            int _NumDet = 8400;
            var _imgW = ImageSettings.ImageWidth;
            var _imgH = ImageSettings.ImageHeight;
            for (int i = 0; i < _NumDet; i++)
            {
                float conf = result[4 * _NumDet + i];
                if (conf >= ConfidenceThreshold)
                {

                    float x = (result[0 * _NumDet + i] - result[2 * _NumDet + i] / 2) * imgW / _imgW;
                    float y = (result[1 * _NumDet + i] - result[3 * _NumDet + i] / 2) * imgeH / _imgH;
                    float w = result[2 * _NumDet + i] * imgW / _imgW;
                    float h = result[3 * _NumDet + i] * imgeH / _imgH;

                    _boxes.Add(new YoloPrediction
                    {
                        Label = "Face",
                        Confidence = conf,
                        Box = new RectangleF(x, y, w, h)
                    });
                }
            }
            return _boxes;
        }

        public List<RectangleF> ApplyNms(List<YoloPrediction> detections, float threshold)
        {
            var results = new List<RectangleF>();

            var sorted = detections.OrderByDescending(d => d.Confidence).ToList();

            while (sorted.Count > 0)
            {
                var best = sorted[0];
                results.Add(best.Box);
                sorted.RemoveAt(0);

                sorted = sorted.Where(d => IoU(best.Box, d.Box) < threshold).ToList();
            }

            // Debug.WriteLine($"[FaceDetector] {results.Count} boxes remaining after NMS.");
            return results;
        }

        private float IoU(RectangleF a, RectangleF b)
        {
            float x1 = Math.Max(a.Left, b.Left);
            float y1 = Math.Max(a.Top, b.Top);
            float x2 = Math.Min(a.Right, b.Right);
            float y2 = Math.Min(a.Bottom, b.Bottom);

            float interArea = Math.Max(0, x2 - x1) * Math.Max(0, y2 - y1);
            float unionArea = a.Width * a.Height + b.Width * b.Height - interArea;

            return unionArea == 0 ? 0 : interArea / unionArea;
        }

    }
}
