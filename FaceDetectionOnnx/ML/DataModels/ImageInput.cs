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

        private void PrepareTensor(Bitmap bitmap)
        {
            int inputWidth = ImageSettings.ImageWidth;
            int inputHeight = ImageSettings.ImageHeight;

            var resizedBitmap = new Bitmap(bitmap, new Size(inputWidth, inputHeight));
            Tensor = new DenseTensor<float>(new[] { 1, 3, inputHeight, inputWidth });

            for (int y = 0; y < inputHeight; y++)
            {
                for (int x = 0; x < inputWidth; x++)
                {
                    Color pixel = resizedBitmap.GetPixel(x, y);
                    Tensor[0, 0, y, x] = pixel.R / 255f;
                    Tensor[0, 1, y, x] = pixel.G / 255f;
                    Tensor[0, 2, y, x] = pixel.B / 255f;
                }
            }
        }
    }
}
