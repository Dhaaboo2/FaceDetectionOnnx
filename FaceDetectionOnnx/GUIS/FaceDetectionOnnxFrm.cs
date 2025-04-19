using FaceDetectionOnnx.ML.DataModels;
using Microsoft.ML.OnnxRuntime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FaceDetectionOnnx.GUIS
{
    public partial class FaceDetectionOnnxFrm : Form
    {
        [AllowNull]
        public Bitmap loadedImage;
        [AllowNull]
        public OnnxOutputParser _OutPar;
        [AllowNull]
        public  InferenceSession _session;
        public FaceDetectionOnnxFrm()
        {
            InitializeComponent();

            _WorkSpace._odl = new OpenFileDialog { Filter = "Image Files|*.jpg;*.png;*.bmp" };
            LoadModel();
        }
        private void LoadModel()
        {
            try
            {
                var _Prodir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../", "ML"));
                var _yolov8x = Path.Combine(_Prodir, "OnnxModels", "yolov8x-face-lindevs.onnx");
                var options = new SessionOptions();
                //_WorkSpace._session = new InferenceSession(_yolov8x, options);
                _session = new InferenceSession(_yolov8x, options);
               // var _OutPar = _WorkSpace._outputParser;
                _OutPar = new OnnxOutputParser();
                MessageBox.Show("Model Loaded Successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }

        private void btnBrows_Click(object sender, EventArgs e)
        {
            try
            {
                var _odl = _WorkSpace._odl;
                if (_odl.ShowDialog() == DialogResult.OK)
                {
                    
                    loadedImage = new Bitmap(_odl.FileName);
                    PB.Image = loadedImage;
                    //var image = Image.FromFile(_odl.FileName);
                    //PB.Image = DetectFaces(image);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void btndet_Click(object sender, EventArgs e)
        {
            try
            {
                
                if (loadedImage == null)
                {
                    MessageBox.Show("Please load an image first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DetectFaces(loadedImage);
                //var faces = FaceDetect(loadedImage);
                //var displayImage = new Bitmap(loadedImage);

                //using (Graphics g = Graphics.FromImage(displayImage))
                //{
                //    Pen pen = new Pen(Color.Red, 2);

                //    foreach (var face in faces)
                //    {
                //        g.DrawRectangle(pen, face.X, face.Y, face.Width, face.Height);
                //    }
                //}
                //PB.Image = displayImage;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        public List<RectangleF> FaceDetect(Bitmap image)
        {

            //var resized = new Bitmap(image, new Size(640, 640));
            var input =  new ImageInput(image);

            var inputs = new List<NamedOnnxValue>
            {
               NamedOnnxValue.CreateFromTensor(_session.InputMetadata.Keys.First(), input.Tensor)
            };

            using var results = _session.Run(inputs);
            //var output = results.First().AsTensor<float>().ToArray();
            //var _OutPar = _WorkSpace._outputParser;
            var rawBoxes = _OutPar.ParseOutputs(results, image.Width, image.Height);
            var finalBoxes = _OutPar.ApplyNms(rawBoxes, _WorkSpace._NmsThreshold);
           // DrawPredictions(image, rawBoxes);

            return finalBoxes;

        }

        private void DetectFaces(Bitmap bitmap)
        {
            var imageInput = new ImageInput(bitmap);
            var inputs = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor(_session.InputMetadata.Keys.First(), imageInput.Tensor)
            };

            using var results = _session.Run(inputs);
            //var _OutPar = _WorkSpace._outputParser;
            var boxes = _OutPar.ParseOutputs(results, bitmap.Width, bitmap.Height);
            DrawPredictions(bitmap, boxes);
        }

        private void DrawPredictions(Bitmap image, List<YoloPrediction> predictions)
        {
            var bitmap = new Bitmap(image);
            var graphics = Graphics.FromImage(bitmap);
            var pen = new Pen(Color.Red, 2);
            var font = new Font("Arial", 12);
            var brush = new SolidBrush(Color.Red);

            foreach (var prediction in predictions)
            {
                graphics.DrawRectangle(pen, prediction.Box.X, prediction.Box.Y, prediction.Box.Width, prediction.Box.Height);
                graphics.DrawString($"{prediction.Label} {prediction.Confidence:P1}", font, brush, prediction.Box.X, prediction.Box.Y - 20);
            }

            PB.Image = bitmap;
        }

    }
}
