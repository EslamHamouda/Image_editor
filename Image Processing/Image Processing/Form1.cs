using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImageProcessor.Processors;
using openCV;

namespace Image_Processing
{
    public partial class Form1 : Form
    {
        IplImage image1, image2;
        Bitmap bitmap, bitmapSource, bitmapDestination;
        public Form1()
        {
            InitializeComponent();
        }


        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            openFileDialog1.FileName = " ";
            openFileDialog1.Filter = "JPEG|*JPG|Bitmap|*.bmp|All|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    image1 = cvlib.CvLoadImage(openFileDialog1.FileName, cvlib.CV_LOAD_IMAGE_COLOR);
                    CvSize size = new CvSize(pictureBox1.Width, pictureBox1.Height);
                    IplImage resized_image = cvlib.CvCreateImage(size, image1.depth, image1.nChannels);
                    bitmapSource = new Bitmap((Image)resized_image);
                    cvlib.CvResize(ref image1, ref resized_image, cvlib.CV_INTER_LINEAR);
                    pictureBox1.BackgroundImage = (Image)resized_image;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void greenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            image2 = cvlib.CvCreateImage(new CvSize(image1.width, image1.height), image1.depth, image1.nChannels);
            int srcAdd = image1.imageData.ToInt32();
            int dstAdd = image2.imageData.ToInt32();
            unsafe
            {
                int srcIndex, dstIndex;
                for (int r = 0; r < image1.height; r++)
                    for (int c = 0; c < image1.width; c++)
                    {
                        srcIndex = dstIndex = (image1.width * r * image1.nChannels) + (c * image1.nChannels);
                        *(byte*)(dstAdd + dstIndex + 0) = 0;
                        *(byte*)(dstAdd + dstIndex + 1) = *(byte*)(srcAdd + srcIndex + 1);
                        *(byte*)(dstAdd + dstIndex + 2) = 0;
                    }
            }
            CvSize size = new CvSize(pictureBox2.Width, pictureBox2.Height);
            IplImage resized_image = cvlib.CvCreateImage(size, image2.depth, image2.nChannels);
            bitmapDestination = new Bitmap((Image)resized_image);
            cvlib.CvResize(ref image2, ref resized_image, cvlib.CV_INTER_LINEAR);
            pictureBox2.BackgroundImage = (Image)resized_image;
        }

        private void blueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            image2 = cvlib.CvCreateImage(new CvSize(image1.width, image1.height), image1.depth, image1.nChannels);
            int srcAdd = image1.imageData.ToInt32();
            int dstAdd = image2.imageData.ToInt32();
            unsafe
            {
                int srcIndex, dstIndex;
                for (int r = 0; r < image1.height; r++)
                    for (int c = 0; c < image1.width; c++)
                    {
                        srcIndex = dstIndex = (image1.width * r * image1.nChannels) + (c * image1.nChannels);
                        *(byte*)(dstAdd + dstIndex + 0) = *(byte*)(srcAdd + srcIndex + 0);
                        *(byte*)(dstAdd + dstIndex + 1) = 0;
                        *(byte*)(dstAdd + dstIndex + 2) = 0;
                    }
            }
            CvSize size = new CvSize(pictureBox2.Width, pictureBox2.Height);
            IplImage resized_image = cvlib.CvCreateImage(size, image2.depth, image2.nChannels);
            bitmapDestination = new Bitmap((Image)resized_image);
            cvlib.CvResize(ref image2, ref resized_image, cvlib.CV_INTER_LINEAR);
            pictureBox2.BackgroundImage = (Image)resized_image;
        }

        private void grayLevelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bitmap = (Bitmap)image1;
            int width = bitmap.Width;
            int height = bitmap.Height;
            Color p;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    p = bitmap.GetPixel(x, y);
                    int a = p.A;
                    int r = p.R;
                    int g = p.G;
                    int b = p.B;
                    int avg = (r + g + b) / 3;
                    bitmap.SetPixel(x, y, Color.FromArgb(a, avg, avg, avg));
                }

            }
            bitmapDestination = bitmap;
            pictureBox2.Image = (Image)bitmap;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //button1.Visible = false;
                chart1.Visible = true;
                chart1.ChartAreas[0].AxisX.Maximum=256;
                chart1.ChartAreas[0].AxisY.Maximum = 256;
                chart1.Series["Red"].Points.Clear();
                chart1.Series["Green"].Points.Clear();
                chart1.Series["Blue"].Points.Clear();

                Bitmap bmpImg = bitmapSource;
                int width = bmpImg.Width;
                int hieght = bmpImg.Height;

                int[] ni_Red = new int[256];
                int[] ni_Green = new int[256];
                int[] ni_Blue = new int[256];

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < hieght; j++)
                    {
                        Color pixelColor = bmpImg.GetPixel(i, j);

                        ni_Red[pixelColor.R]++;
                        ni_Green[pixelColor.G]++;
                        ni_Blue[pixelColor.B]++;

                    }
                }

                for (int i = 0; i < 256; i++)
                {
                    chart1.Series["Red"].Points.AddY(ni_Red[i]);
                    chart1.Series["Green"].Points.AddY(ni_Green[i]);
                    chart1.Series["Blue"].Points.AddY(ni_Blue[i]);
                }
            }
            catch (Exception ex)
            {
                button1.Visible = true;
                chart1.Visible = false;
                MessageBox.Show(ex.Message);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                //button2.Visible = false;
                chart2.Visible = true;
                chart2.ChartAreas[0].AxisX.Maximum = 256;
                chart2.ChartAreas[0].AxisY.Maximum = 256;
                chart2.Series["Red"].Points.Clear();
                chart2.Series["Green"].Points.Clear();
                chart2.Series["Blue"].Points.Clear();

                Bitmap bmpImg = bitmapDestination;
                int width = bmpImg.Width;
                int hieght = bmpImg.Height;

                int[] ni_Red = new int[256];
                int[] ni_Green = new int[256];
                int[] ni_Blue = new int[256];

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < hieght; j++)
                    {
                        Color pixelColor = bmpImg.GetPixel(i, j);

                        ni_Red[pixelColor.R]++;
                        ni_Green[pixelColor.G]++;
                        ni_Blue[pixelColor.B]++;

                    }
                }

                for (int i = 0; i < 256; i++)
                {
                    chart2.Series["Red"].Points.AddY(ni_Red[i]);
                    chart2.Series["Green"].Points.AddY(ni_Green[i]);
                    chart2.Series["Blue"].Points.AddY(ni_Blue[i]);
                }
            }
            catch (Exception ex)
            {
                button2.Visible = true;
                chart2.Visible = false;
                MessageBox.Show(ex.Message);
            }

        }

        private void removeSaltPepperNoiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IplImage image = cvlib.CvCreateImage(new CvSize(image1.width, image1.height), image1.depth, image1.nChannels);
            cvlib.CvSmooth(ref image1, ref image, cvlib.CV_MEDIAN, 3, 3, 0.01, 0.01);
            CvSize size = new CvSize(pictureBox2.Width, pictureBox2.Height);
            IplImage resized_image = cvlib.CvCreateImage(size, image.depth, image.nChannels);
            cvlib.CvResize(ref image, ref resized_image, cvlib.CV_INTER_LINEAR);
            pictureBox2.BackgroundImage = (Image)resized_image;
        }

        private void laplacianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = new Bitmap((Image)image1);
            Bitmap bitmap0 = new Bitmap((Image)image1);
            
            for (int x = 1; x < bitmap.Width - 1; x++)
            {
                for (int y = 1; y < bitmap.Height - 1; y++)
                {
                    Color color2, color4, color5, color6, color8;
                    color2 = bitmap.GetPixel(x, y - 1);
                    color4 = bitmap.GetPixel(x - 1, y);   
                    color5 = bitmap.GetPixel(x, y);
                    color6 = bitmap.GetPixel(x + 1, y);
                    color8 = bitmap.GetPixel(x, y + 1);
                    int r = color2.R + color4.R + color5.R * (-4) + color6.R + color8.R;
                    int g = color2.G + color4.G + color5.G * (-4) + color6.G + color8.G;
                    int b = color2.B + color4.B + color5.B * (-4) + color6.B + color8.B;
                    int avg = (r + g + b) / 3;
                    if (avg > 255) avg = 255;
                    if (avg < 0) avg = 0;
                    bitmap0.SetPixel(x, y, Color.FromArgb(avg, avg, avg));
                }
            }
            pictureBox2.BackgroundImage=(Image)bitmap0;
        }

        private void gaussianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IplImage image = cvlib.CvCreateImage(new CvSize(image1.width, image1.height), image1.depth, image1.nChannels);
            cvlib.CvSmooth(ref image1, ref image, cvlib.CV_GAUSSIAN, 3, 3, 10, 10);
            CvSize size = new CvSize(pictureBox2.Width, pictureBox2.Height);
            IplImage resized_image = cvlib.CvCreateImage(size, image.depth, image.nChannels);
            cvlib.CvResize(ref image, ref resized_image, cvlib.CV_INTER_LINEAR);
            pictureBox2.BackgroundImage = (Image)resized_image;
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void chart2_Click(object sender, EventArgs e)
        {

        }

        private void redToolStripMenuItem_Click(object sender, EventArgs e)
        {
            image2 = cvlib.CvCreateImage(new CvSize(image1.width, image1.height), image1.depth, image1.nChannels);
            int srcAdd = image1.imageData.ToInt32();
            int dstAdd = image2.imageData.ToInt32();
            unsafe
            {
                int srcIndex, dstIndex;
                for (int r = 0; r < image1.height; r++)
                    for (int c = 0; c < image1.width; c++)
                    {
                        srcIndex = dstIndex = (image1.width * r * image1.nChannels) + (c * image1.nChannels);
                        *(byte*)(dstAdd + dstIndex + 0) = 0;
                        *(byte*)(dstAdd + dstIndex + 1) = 0;
                        *(byte*)(dstAdd + dstIndex + 2) = *(byte*)(srcAdd + srcIndex + 2);
                    }
            }
            CvSize size = new CvSize(pictureBox2.Width, pictureBox2.Height);
            IplImage resized_image = cvlib.CvCreateImage(size, image2.depth, image2.nChannels);
            bitmapDestination = new Bitmap((Image)resized_image);
            cvlib.CvResize(ref image2, ref resized_image, cvlib.CV_INTER_LINEAR);
            pictureBox2.BackgroundImage = (Image)resized_image;

        }
    }

}
 






