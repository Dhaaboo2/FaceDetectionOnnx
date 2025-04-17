using Microsoft.ML.OnnxRuntime.Tensors;
using System.Diagnostics.CodeAnalysis;

namespace FaceDetectionOnnx.ML.DataModels
{
    public class ImageInput
    {
        [AllowNull]
        public DenseTensor<float> Tensor { get; private set; }

        public ImageInput(Bitmap image)
        {
            PrepareTensor(image);
        }

        private void PrepareTensor(Bitmap _bit)
        {
            int inputW = ImageSettings.ImageWidth;
            int inputH = ImageSettings.ImageHeight;

            var resizedBit = new Bitmap(_bit, new Size(inputW, inputH));
            Tensor = new DenseTensor<float>(new[] { 1, 3, inputH, inputW });

            for (int y = 0; y < inputH; y++)
            {
                for (int x = 0; x < inputW; x++)
                {
                    Color pixel = resizedBit.GetPixel(x, y);
                    Tensor[0, 0, y, x] = pixel.R / 255f;
                    Tensor[0, 1, y, x] = pixel.G / 255f;
                    Tensor[0, 2, y, x] = pixel.B / 255f;
                }
            }
        }
    }
}
