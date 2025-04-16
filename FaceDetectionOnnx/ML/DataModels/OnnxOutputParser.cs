using Microsoft.ML.OnnxRuntime;
using System;

namespace FaceDetectionOnnx.ML.DataModels
{
    public class OnnxOutputParser
    {
        private const float ConfidenceThreshold = 0.5f;

        //private const float ConfidenceThreshold = 0.5f;
        public const float IouThreshold = 0.4f;

        public List<RectangleF> GetFaceBoxes(IDisposableReadOnlyCollection<DisposableNamedOnnxValue> results, int imageWidth, int imageHeight)
        {
            var boxes = new List<(RectangleF box, float confidence)>();
            var output = results.First().AsEnumerable<float>().ToArray();
            int numDetections = 8400;

            float xScale = imageWidth;
            float yScale = imageHeight;

            for (int i = 0; i < numDetections; i++)
            {
                float xcNorm = output[i * 5];
                float ycNorm = output[i * 5 + 1];
                float wNorm = output[i * 5 + 2];
                float hNorm = output[i * 5 + 3];
                float conf = output[i * 5 + 4];

                if (conf >= ConfidenceThreshold)
                {
                    float x1 = (xcNorm - wNorm / 2f) * xScale;
                    float y1 = (ycNorm - hNorm / 2f) * yScale;
                    float width = wNorm * xScale;
                    float height = hNorm * yScale;

                    x1 = Clamp(x1, 0, imageWidth - 1);
                    y1 = Clamp(y1, 0, imageHeight - 1);
                    width = Clamp(width, 0, imageWidth - x1);
                    height = Clamp(height, 0, imageHeight - y1);

                    var rect = new RectangleF(x1, y1, width, height);
                    boxes.Add((rect, conf));
                }
            }

            return ApplyNms(boxes, IouThreshold);
        }

        private List<RectangleF> ApplyNms(List<(RectangleF box, float confidence)> boxes, float iouThreshold)
        {
            var finalBoxes = new List<RectangleF>();

            var sorted = boxes.OrderByDescending(b => b.confidence).ToList();

            while (sorted.Any())
            {
                var current = sorted[0];
                finalBoxes.Add(current.box);
                sorted.RemoveAt(0);

                sorted = sorted.Where(b => ComputeIoU(current.box, b.box) < iouThreshold).ToList();
            }

            return finalBoxes;
        }

        private float ComputeIoU(RectangleF a, RectangleF b)
        {
            float intersectionX = Math.Max(a.Left, b.Left);
            float intersectionY = Math.Max(a.Top, b.Top);
            float intersectionWidth = Math.Min(a.Right, b.Right) - intersectionX;
            float intersectionHeight = Math.Min(a.Bottom, b.Bottom) - intersectionY;

            if (intersectionWidth <= 0 || intersectionHeight <= 0)
                return 0;

            float intersectionArea = intersectionWidth * intersectionHeight;
            float unionArea = a.Width * a.Height + b.Width * b.Height - intersectionArea;

            return intersectionArea / unionArea;
        }

        private float Clamp(float value, float min, float max)
        {
            return Math.Max(min, Math.Min(max, value));
        }



        public List<YoloPrediction> ParseOutputs(IDisposableReadOnlyCollection<DisposableNamedOnnxValue> results, int imageWidth, int imageHeight)
        {
            var _boxes = new List<YoloPrediction>();
            var result = results.First().AsEnumerable<float>().ToArray();
            int _NumDet = 8400;

            for (int i = 0; i < _NumDet; i++)
            {
                float conf = result[4 * _NumDet + i];
                if (conf >= ConfidenceThreshold)
                {

                    float x = (result[0 * _NumDet + i] - result[2 * _NumDet + i] / 2) * imageWidth / 640;
                    float y = (result[1 * _NumDet + i] - result[3 * _NumDet + i] / 2) * imageHeight / 640;
                    float w = result[2 * _NumDet + i] * imageWidth / 640;
                    float h = result[3 * _NumDet + i] * imageHeight / 640;

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
