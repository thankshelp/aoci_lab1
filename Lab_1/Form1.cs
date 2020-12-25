using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;


namespace Lab_1
{


    public partial class Form1 : Form
    {
        private VideoCapture capture;
        private Image<Bgr, byte> sourceImage;
        int cannyThreshold, cannyThresholdLinking;
        int frameCount = 0;


        public Form1()
        {
            InitializeComponent();

            trackBar1.Minimum = 0;
            trackBar1.Maximum = 100;
            trackBar1.TickFrequency = 1;
            trackBar1.Value = cannyThreshold = 20;
            trackBar2.Minimum = 0;
            trackBar2.Maximum = 100;
            trackBar2.TickFrequency = 1;
            trackBar2.Value = cannyThresholdLinking = 30;


        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            var result = openFileDialog.ShowDialog(); // открытие диалога выбора файла
            if (result == DialogResult.OK) // открытие выбранного файла
            {
                string fileName = openFileDialog.FileName;
                sourceImage = new Image<Bgr, byte>(fileName).Resize(640, 480, Inter.Linear);

                imageBox1.Image = sourceImage;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Image<Gray, byte> grayImage = sourceImage.Convert<Gray, byte>();

            var tempImage = grayImage.PyrDown();
            var destImage = tempImage.PyrUp();

            
            Image<Gray, byte> cannyEdges = destImage.Canny(cannyThreshold, cannyThresholdLinking);

            var cannyEdgesBgr = cannyEdges.Convert<Bgr, byte>();
            var resultImage = sourceImage.Sub(cannyEdgesBgr);

            for (int channel = 0; channel < resultImage.NumberOfChannels; channel++)
                for (int x = 0; x < resultImage.Width; x++)
                    for (int y = 0; y < resultImage.Height; y++) // обход по пискелям
                    {
                        // получение цвета пикселя
                        byte color = resultImage.Data[y, x, channel];
                        if (color <= 50)
                            color = 0;
                        else if (color <= 100)
                            color = 25;
                        else if (color <= 150)
                            color = 100;
                        else if (color <= 200)
                            color = 200;
                        else
                            color = 255;
                        resultImage.Data[y, x, channel] = color; // изменение цвета пикселя
                    }

            imageBox2.Image = resultImage;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            var result = openFileDialog.ShowDialog(); // открытие диалога выбора файла
            if (result == DialogResult.OK) // открытие выбранного файла
            {
                string fileName = openFileDialog.FileName;

                capture = new VideoCapture(fileName);
                timer1.Enabled = true;

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var frame = capture.QueryFrame();

            imageBox1.Image = frame;

            Image<Bgr, byte> image = frame.ToImage<Bgr, byte>();

            Image<Gray, byte> grayImage = image.Convert<Gray, byte>();

            var tempImage = grayImage.PyrDown();
            var destImage = tempImage.PyrUp();

            
            Image<Gray, byte> cannyEdges = destImage.Canny(cannyThreshold, cannyThresholdLinking);

            var cannyEdgesBgr = cannyEdges.Convert<Bgr, byte>();
            var resultImage = image.Sub(cannyEdgesBgr);

            for (int channel = 0; channel < resultImage.NumberOfChannels; channel++)
                for (int x = 0; x < resultImage.Width; x++)
                    for (int y = 0; y < resultImage.Height; y++) // обход по пискелям
                    {
                        // получение цвета пикселя
                        byte color = resultImage.Data[y, x, channel];
                        if (color <= 50)
                            color = 0;
                        else if (color <= 100)
                            color = 25;
                        else if (color <= 150)
                            color = 100;
                        else if (color <= 200)
                            color = 200;
                        else
                            color = 255;
                        resultImage.Data[y, x, channel] = color; // изменение цвета пикселя
                    }
            imageBox2.Image = resultImage;

            frameCount++;

            if (frameCount >= capture.GetCaptureProperty(CapProp.FrameCount))
            {
                timer1.Enabled = false;
                capture.Stop();
            }
            }

        private void button4_Click(object sender, EventArgs e)
        {
            capture = new VideoCapture();
            capture.ImageGrabbed += ProcessFrame;
            capture.Start();
        }

        private void ProcessFrame(object sender, EventArgs e)
        {
            var frame = new Mat();
            capture.Retrieve(frame); // получение текущего кадра

            Image<Bgr, byte> image = frame.ToImage<Bgr, byte>();

            Image<Gray, byte> grayImage = image.Convert<Gray, byte>();

            var tempImage = grayImage.PyrDown();
            var destImage = tempImage.PyrUp();

            //cannyThreshold = 20;
            //cannyThresholdLinking = 30;
            Image<Gray, byte> cannyEdges = destImage.Canny(cannyThreshold, cannyThresholdLinking);

            var cannyEdgesBgr = cannyEdges.Convert<Bgr, byte>();
            var resultImage = image.Sub(cannyEdgesBgr);

            for (int channel = 0; channel < resultImage.NumberOfChannels; channel++)
                for (int x = 0; x < resultImage.Width; x++)
                    for (int y = 0; y < resultImage.Height; y++) // обход по пискелям
                    {
                        // получение цвета пикселя
                        byte color = resultImage.Data[y, x, channel];
                        if (color <= 50)
                            color = 0;
                        else if (color <= 100)
                            color = 25;
                        else if (color <= 150)
                            color = 100;
                        else if (color <= 200)
                            color = 200;
                        else
                            color = 255;
                        resultImage.Data[y, x, channel] = color; // изменение цвета пикселя
                    }
            imageBox2.Image = resultImage;

        }

      

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            cannyThreshold = trackBar1.Value;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            cannyThresholdLinking = trackBar2.Value;
        }

    }
}
