namespace FaceDetectionOnnx.GUIS
{
    partial class FaceDetectionOnnxFrm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            splitContainer1 = new SplitContainer();
            PB = new PictureBox();
            btnBrows = new Button();
            btndet = new Button();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PB).BeginInit();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(btndet);
            splitContainer1.Panel1.Controls.Add(btnBrows);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(PB);
            splitContainer1.Size = new Size(709, 344);
            splitContainer1.SplitterDistance = 236;
            splitContainer1.TabIndex = 0;
            // 
            // PB
            // 
            PB.BackColor = Color.Cyan;
            PB.Location = new Point(62, 12);
            PB.Name = "PB";
            PB.Size = new Size(354, 307);
            PB.SizeMode = PictureBoxSizeMode.StretchImage;
            PB.TabIndex = 0;
            PB.TabStop = false;
            // 
            // btnBrows
            // 
            btnBrows.Location = new Point(12, 12);
            btnBrows.Name = "btnBrows";
            btnBrows.Size = new Size(91, 30);
            btnBrows.TabIndex = 0;
            btnBrows.Text = "Brows Pic";
            btnBrows.UseVisualStyleBackColor = true;
            btnBrows.Click += btnBrows_Click;
            // 
            // btndet
            // 
            btndet.Location = new Point(10, 46);
            btndet.Name = "btndet";
            btndet.Size = new Size(93, 31);
            btndet.TabIndex = 1;
            btndet.Text = "Detect Face";
            btndet.UseVisualStyleBackColor = true;
            btndet.Click += btndet_Click;
            // 
            // FaceDetectionOnnxFrm
            // 
            AutoScaleDimensions = new SizeF(7F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(709, 344);
            Controls.Add(splitContainer1);
            Font = new Font("Times New Roman", 9.75F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            Name = "FaceDetectionOnnxFrm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FaceDetectionOnnxFrm";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)PB).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private Button btndet;
        private Button btnBrows;
        private PictureBox PB;
    }
}