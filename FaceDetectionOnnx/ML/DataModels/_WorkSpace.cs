using Microsoft.ML.OnnxRuntime;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceDetectionOnnx.ML.DataModels
{
    public static class _WorkSpace
    {
        [AllowNull]
        public static InferenceSession _session { get; set; }
        [AllowNull]
        public static OnnxOutputParser _outputParser { get; set; }
        [AllowNull]
        public static OpenFileDialog _odl { get; set; }
        [AllowNull]
        public static Bitmap loadedImage { get; set; }
        public const float _NmsThreshold = 0.4f; 
    }
}
